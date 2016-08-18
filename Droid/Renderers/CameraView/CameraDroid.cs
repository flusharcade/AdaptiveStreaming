// --------------------------------------------------------------------------------------------------------------------
// <copyright file="CameraDroid.cs" company="Flush Arcade Pty Ltd.">
//   Copyright (c) 2016 Flush Arcade Pty Ltd. All rights reserved.
// </copyright>
// --------------------------------------------------------------------------------------------------------------------

namespace Camera.Droid.Renderers.CameraView
{
	using System;
	using System.Collections.Generic;

	using Java.IO;
	using Java.Lang;

	using Android.Content;
	using Android.OS;
	using Android.Runtime;
	using Android.Util;
	using Android.Views;
	using Android.Widget;
	using Android.Hardware.Camera2;
	using Android.Hardware.Camera2.Params;
	using Android.Graphics;
	using Android.Content.Res;
	using Android.Media;

	using Camera.Portable.Ioc;
	using Camera.Portable.Logging;

	/// <summary>
	/// Camera droid.
	/// </summary>
	public class CameraDroid : FrameLayout, TextureView.ISurfaceTextureListener
	{
		#region Static Properties

		/// <summary>
		/// The orientations.
		/// </summary>
		private static readonly SparseIntArray ORIENTATIONS = new SparseIntArray();

		#endregion

		#region Public Events

		/// <summary>
		/// Occurs when busy.
		/// </summary>
		public event EventHandler<bool> Busy;

		/// <summary>
		/// Occurs when available.
		/// </summary>
		public event EventHandler<bool> Available;

		/// <summary>
		/// Occurs when photo.
		/// </summary>
		public event EventHandler<byte[]> Photo;

		#endregion

		#region Private Properties

		/// <summary>
		/// The tag.
		/// </summary>
		private readonly string _tag;

		/// <summary>
		/// The log.
		/// </summary>
		private readonly ILogger _log;

		/// <summary>
		/// The m state listener.
		/// </summary>
		private CameraStateListener mStateListener;

		/// <summary>
		/// The CameraRequest.Builder for camera preview.
		/// </summary>
		private CaptureRequest.Builder mPreviewBuilder;

		/// <summary>
		/// The CameraCaptureSession for camera preview.
		/// </summary>
		private CameraCaptureSession mPreviewSession;

		/// <summary>
		/// The view surface.
		/// </summary>
		private SurfaceTexture _viewSurface;

		/// <summary>
		/// The camera texture.
		/// </summary>
		private AutoFitTextureView _cameraTexture;

		/// <summary>
		/// The media sound.
		/// </summary>
		private MediaActionSound mediaSound;

		/// <summary>
		/// The size of the camera preview.
		/// </summary>
		private Android.Util.Size mPreviewSize;

		/// <summary>
		/// The context.
		/// </summary>
		private Context _context;

		/// <summary>
		/// The camera manager.
		/// </summary>
		private CameraManager _manager;

		/// <summary>
		/// The media sound loaded.
		/// </summary>
		private bool _mediaSoundLoaded;

		/// <summary>
		/// The opening camera.
		/// </summary>
		private bool _openingCamera;

		#endregion

		#region Public Properties

		/// <summary>
		/// The opening camera.
		/// </summary>
		public bool OpeningCamera
		{
			get
			{
				return _openingCamera;
			}
			set
			{
				if (_openingCamera != value)
				{
					_openingCamera = value;
					Busy?.Invoke(this, value);
				}
			}
		}

		/// <summary>
		/// The reference to the opened CameraDevice.
		/// </summary>
		public CameraDevice mCameraDevice;

		#endregion

		#region Constructors

		/// <summary>
		/// Initializes a new instance of the <see cref="Android_Shared.Controls.CameraDroid"/> class.
		/// </summary>
		/// <param name="context">Context.</param>
		public CameraDroid (Context context) : base (context)
		{
			_context = context;
			_mediaSoundLoaded = LoadShutterSound ();

			_log = IoC.Resolve<ILogger>();
			_tag = $"{GetType()} ";

			var inflater = LayoutInflater.FromContext (context);

			if (inflater != null)
			{
				var view = inflater.Inflate(Resource.Layout.CameraLayout, this);

				_cameraTexture = view.FindViewById<AutoFitTextureView>(Resource.Id.CameraTexture);
				_cameraTexture.SurfaceTextureListener = this;

				mStateListener = new CameraStateListener() { Camera = this };

				ORIENTATIONS.Append((int)SurfaceOrientation.Rotation0, 90);
				ORIENTATIONS.Append((int)SurfaceOrientation.Rotation90, 0);
				ORIENTATIONS.Append((int)SurfaceOrientation.Rotation180, 270);
				ORIENTATIONS.Append((int)SurfaceOrientation.Rotation270, 180);
			}
		}

		#endregion

		#region Private Methods

		/// <summary>
		/// Updates the camera preview, StartPreview() needs to be called in advance
		/// </summary>
		private void UpdatePreview()
		{
			if (mCameraDevice != null && mPreviewSession != null)
			{
				try
				{
					// The camera preview can be run in a background thread. This is a Handler for the camere preview
					mPreviewBuilder.Set(CaptureRequest.ControlMode, new Java.Lang.Integer((int)ControlMode.Auto));
					HandlerThread thread = new HandlerThread("CameraPreview");
					thread.Start();
					Handler backgroundHandler = new Handler(thread.Looper);

					// Finally, we start displaying the camera preview
					mPreviewSession.SetRepeatingRequest(mPreviewBuilder.Build(), null, backgroundHandler);
				}
				catch (CameraAccessException error)
				{
					_log.WriteLineTime(_tag + "\n" +
						"UpdatePreview() Camera access exception.  \n " +
						"ErrorMessage: \n" +
						error.Message + "\n" +
						"Stacktrace: \n " +
						error.StackTrace);
				}
				catch (IllegalStateException error)
				{
					_log.WriteLineTime(_tag + "\n" +
						"UpdatePreview() Illegal exception.  \n " +
						"ErrorMessage: \n" +
						error.Message + "\n" +
						"Stacktrace: \n " +
						error.StackTrace);
				}
			}
		}

		/// <summary>
		/// Loads the shutter sound.
		/// </summary>
		/// <returns><c>true</c>, if shutter sound was loaded, <c>false</c> otherwise.</returns>
		private bool LoadShutterSound()
		{
			try 
			{
				mediaSound = new MediaActionSound ();
				mediaSound.LoadAsync (MediaActionSoundType.ShutterClick);

				return true;
			}
			catch (Java.Lang.Exception error) 
			{
				_log.WriteLineTime(_tag + "\n" +
					"LoadShutterSound() Error loading shutter sound  \n " +
					"ErrorMessage: \n" +
					error.Message + "\n" +
					"Stacktrace: \n " +
					error.StackTrace);
			}

			return false;
		}

		#endregion

		#region Public Methods

		/// <summary>
		/// Opens the camera.
		/// </summary>
		public void OpenCamera()
		{
			if (_context== null || OpeningCamera)
			{
				return;
			}

			OpeningCamera = true;

			_manager = (CameraManager)_context.GetSystemService(Context.CameraService);

			try
			{
				string cameraId = _manager.GetCameraIdList()[0];

				// To get a list of available sizes of camera preview, we retrieve an instance of
				// StreamConfigurationMap from CameraCharacteristics
				CameraCharacteristics characteristics = _manager.GetCameraCharacteristics(cameraId);
				StreamConfigurationMap map = (StreamConfigurationMap)characteristics.Get(CameraCharacteristics.ScalerStreamConfigurationMap);
				mPreviewSize = map.GetOutputSizes(Java.Lang.Class.FromType(typeof(SurfaceTexture)))[0];
				Android.Content.Res.Orientation orientation = Resources.Configuration.Orientation;
				if (orientation == Android.Content.Res.Orientation.Landscape)
				{
					_cameraTexture.SetAspectRatio(mPreviewSize.Width, mPreviewSize.Height);
				}
				else
				{
					_cameraTexture.SetAspectRatio(mPreviewSize.Height, mPreviewSize.Width);
				}

				// We are opening the camera with a listener. When it is ready, OnOpened of mStateListener is called.
				_manager.OpenCamera(cameraId, mStateListener, null);
			}
			catch (Java.Lang.Exception error)
			{
				_log.WriteLineTime(_tag + "\n" +
					"OpenCamera() Failed to open camera  \n " +
					"ErrorMessage: \n" +
					error.Message + "\n" +
					"Stacktrace: \n " +
					error.StackTrace);
				
				Available?.Invoke(this, false);
			}
		}

		/// <summary>
		/// Takes the photo.
		/// </summary>
		public void TakePhoto ()
		{
			if (_context != null && mCameraDevice != null)
			{
				try
				{
					Busy?.Invoke(this, true);

					if (_mediaSoundLoaded)
					{
						mediaSound.Play(MediaActionSoundType.ShutterClick);
					}

					// Pick the best JPEG size that can be captures with this CameraDevice
					var characteristics = _manager.GetCameraCharacteristics(mCameraDevice.Id);
					Android.Util.Size[] jpegSizes = null;
					if (characteristics != null)
					{
						jpegSizes = ((StreamConfigurationMap)characteristics.Get(CameraCharacteristics.ScalerStreamConfigurationMap)).GetOutputSizes((int)ImageFormatType.Jpeg);
					}
					int width = 640;
					int height = 480;

					if (jpegSizes != null && jpegSizes.Length > 0)
					{
						width = jpegSizes[0].Width;
						height = jpegSizes[0].Height;
					}

					// We use an ImageReader to get a JPEG from CameraDevice
					// Here, we create a new ImageReader and prepare its Surface as an output from the camera
					var reader = ImageReader.NewInstance(width, height, ImageFormatType.Jpeg, 1);
					var outputSurfaces = new List<Surface>(2);
					outputSurfaces.Add(reader.Surface);
					outputSurfaces.Add(new Surface(_viewSurface));

					CaptureRequest.Builder captureBuilder = mCameraDevice.CreateCaptureRequest(CameraTemplate.StillCapture);
					captureBuilder.AddTarget(reader.Surface);
					captureBuilder.Set(CaptureRequest.ControlMode, new Integer((int)ControlMode.Auto));

					// Orientation
					var windowManager = _context.GetSystemService(Context.WindowService).JavaCast<IWindowManager>();
					SurfaceOrientation rotation = windowManager.DefaultDisplay.Rotation;

					captureBuilder.Set(CaptureRequest.JpegOrientation, new Integer(ORIENTATIONS.Get((int)rotation)));

					// This listener is called when an image is ready in ImageReader 
					// Right click on ImageAvailableListener in your IDE and go to its definition
					ImageAvailableListener readerListener = new ImageAvailableListener();

					readerListener.Photo += (sender, e) =>
					{
						Photo?.Invoke(this, e);
					};

					// We create a Handler since we want to handle the resulting JPEG in a background thread
					HandlerThread thread = new HandlerThread("CameraPicture");
					thread.Start();
					Handler backgroundHandler = new Handler(thread.Looper);
					reader.SetOnImageAvailableListener(readerListener, backgroundHandler);

					var captureListener = new CameraCaptureListener();

					captureListener.PhotoComplete += (sender, e) =>
					{
						Busy?.Invoke(this, false);
						StartPreview();
					};

					mCameraDevice.CreateCaptureSession(outputSurfaces, new CameraCaptureStateListener()
					{
						OnConfiguredAction = (CameraCaptureSession session) =>
						{
							try
							{
								mPreviewSession = session;
								session.Capture(captureBuilder.Build(), captureListener, backgroundHandler);
							}
							catch (CameraAccessException ex)
							{
								Log.WriteLine(LogPriority.Info, "Capture Session error: ", ex.ToString());
							}
						}
					}, backgroundHandler);
				}
				catch (CameraAccessException error)
				{
					_log.WriteLineTime(_tag + "\n" +
						"TakePhoto() Failed to take photo  \n " +
						"ErrorMessage: \n" +
						error.Message + "\n" +
						"Stacktrace: \n " +
						error.StackTrace);					
				}
				catch (Java.Lang.Exception error)
				{
					_log.WriteLineTime(_tag + "\n" +
						"TakePhoto() Failed to take photo  \n " +
						"ErrorMessage: \n" +
						error.Message + "\n" +
						"Stacktrace: \n " +
						error.StackTrace);					
				}
			}
		}

		/// <summary>
		/// Changes the focus point.
		/// </summary>
		/// <param name="e">E.</param>
		public void ChangeFocusPoint(Xamarin.Forms.Point e)
		{
			string cameraId = _manager.GetCameraIdList()[0];

			// To get a list of available sizes of camera preview, we retrieve an instance of
			// StreamConfigurationMap from CameraCharacteristics
			CameraCharacteristics characteristics = _manager.GetCameraCharacteristics(cameraId);

			var rect = characteristics.Get(CameraCharacteristics.SensorInfoActiveArraySize) as Rect;
			var size = characteristics.Get(CameraCharacteristics.SensorInfoPixelArraySize) as Size;

			int areaSize = 200;
			int right = rect.Right;
			int bottom = rect.Bottom;
			int viewWidth = _cameraTexture.Width;
			int viewHeight = _cameraTexture.Height;
			int ll, rr;

			Rect newRect;
			int centerX = (int)e.X;
			int centerY = (int)e.Y;

			ll = ((centerX * right) - areaSize) / viewWidth;
			rr = ((centerY * bottom) - areaSize) / viewHeight;

			int focusLeft = Clamp(ll, 0, right);
			int focusBottom = Clamp(rr, 0, bottom);

			newRect = new Rect(focusLeft, focusBottom, focusLeft + areaSize, focusBottom + areaSize);
			MeteringRectangle meteringRectangle = new MeteringRectangle(newRect, 500);
			MeteringRectangle[] meteringRectangleArr = { meteringRectangle };
			mPreviewBuilder.Set(CaptureRequest.ControlAfTrigger, (int)ControlAFTrigger.Cancel);
			mPreviewBuilder.Set(CaptureRequest.ControlAeRegions, meteringRectangleArr);
			mPreviewBuilder.Set(CaptureRequest.ControlAfTrigger, (int)ControlAFTrigger.Start);

			UpdatePreview();
		}

		/// <summary>
		/// Clamp the specified value, min and max.
		/// </summary>
		/// <param name="value">Value.</param>
		/// <param name="min">Minimum.</param>
		/// <param name="max">Max.</param>
		private int Clamp(int value, int min, int max)
		{
			return (value < min) ? min : (value > max) ? max : value;
		}

		/// <summary>
		/// Starts the camera previe
		/// </summary>
		public void StartPreview()
		{
			if (mCameraDevice != null && _cameraTexture.IsAvailable && mPreviewSize != null)
			{
				try
				{
					var texture = _cameraTexture.SurfaceTexture;
					System.Diagnostics.Debug.Assert(texture != null);

					// We configure the size of the default buffer to be the size of the camera preview we want
					texture.SetDefaultBufferSize(mPreviewSize.Width, mPreviewSize.Height);

					// This is the output Surface we need to start the preview
					Surface surface = new Surface(texture);

					// We set up a CaptureRequest.Builder with the output Surface
					mPreviewBuilder = mCameraDevice.CreateCaptureRequest(CameraTemplate.Preview);
					mPreviewBuilder.AddTarget(surface);

					// Here, we create a CameraCaptureSession for camera preview.
					mCameraDevice.CreateCaptureSession(new List<Surface>() { surface },
						new CameraCaptureStateListener()
						{
							OnConfigureFailedAction = (CameraCaptureSession session) =>
							{
							},
							OnConfiguredAction = (CameraCaptureSession session) =>
							{
								mPreviewSession = session;
								UpdatePreview();
							}
						},
						null);


				}
				catch (Java.Lang.Exception error)
				{
					_log.WriteLineTime(_tag + "\n" +
						"TakePhoto() Failed to start preview \n " +
						"ErrorMessage: \n" +
						error.Message + "\n" +
						"Stacktrace: \n " +
						error.StackTrace);	
				}
			}
		}

		/// <summary>
		/// Switchs the flash.
		/// </summary>
		/// <param name="flashOn">If set to <c>true</c> flash on.</param>
		public void SwitchFlash(bool flashOn)
		{
			try
			{
				mPreviewBuilder.Set(CaptureRequest.FlashMode, new Integer(flashOn ? (int)FlashMode.Torch : (int)FlashMode.Off));
				UpdatePreview();
			}
			catch (System.Exception error)
			{
				_log.WriteLineTime(_tag + "\n" +
					"TakePhoto() Failed to switch flash on/off \n " +
					"ErrorMessage: \n" +
					error.Message + "\n" +
					"Stacktrace: \n " +
					error.StackTrace);
				
			}
		}

		/// <summary>
		/// Notifies the available.
		/// </summary>
		/// <param name="isAvailable">If set to <c>true</c> is available.</param>
		public void NotifyAvailable(bool isAvailable)
		{
			Available?.Invoke(this, isAvailable);
		}

		/// <summary>
		/// Ons the layout.
		/// </summary>
		/// <param name="l">L.</param>
		/// <param name="t">T.</param>
		/// <param name="r">The red component.</param>
		/// <param name="b">The blue component.</param>
		public void OnLayout(int l, int t, int r, int b)
		{
			var msw = MeasureSpec.MakeMeasureSpec(r - l, MeasureSpecMode.Exactly);
			var msh = MeasureSpec.MakeMeasureSpec(b - t, MeasureSpecMode.Exactly);

			_cameraTexture.Measure(msw, msh);
			_cameraTexture.Layout(0, 0, r - l, b - t);
		}

		/// <summary>
		/// Configures the transform.
		/// </summary>
		/// <param name="viewWidth">View width.</param>
		/// <param name="viewHeight">View height.</param>
		public void ConfigureTransform(int viewWidth, int viewHeight)
		{
			if (_viewSurface != null && mPreviewSize != null && _context != null)
			{
				var windowManager = _context.GetSystemService(Context.WindowService).JavaCast<IWindowManager>();

				var rotation = windowManager.DefaultDisplay.Rotation;
				var matrix = new Matrix();
				var viewRect = new RectF(0, 0, viewWidth, viewHeight);
				var bufferRect = new RectF(0, 0, mPreviewSize.Width, mPreviewSize.Height);

				var centerX = viewRect.CenterX();
				var centerY = viewRect.CenterY();

				if (rotation == SurfaceOrientation.Rotation90 || rotation == SurfaceOrientation.Rotation270)
				{
					bufferRect.Offset(centerX - bufferRect.CenterX() , centerY - bufferRect.CenterY());
					matrix.SetRectToRect(viewRect, bufferRect, Matrix.ScaleToFit.Fill);

					var scale = System.Math.Max((float)viewHeight / mPreviewSize.Height, (float)viewWidth / mPreviewSize.Width);
					matrix.PostScale(scale, scale, centerX, centerY);
					matrix.PostRotate(90 * ((int)rotation - 2), centerX, centerY);
				}

				_cameraTexture.SetTransform(matrix);
			}
		}

		/// <summary>
		/// Raises the surface texture available event.
		/// </summary>
		/// <param name="surface">Surface.</param>
		/// <param name="w">The width.</param>
		/// <param name="h">The height.</param>
		public void OnSurfaceTextureAvailable (SurfaceTexture surface, int w, int h)
		{
			_viewSurface = surface;

			ConfigureTransform(w, h);
			StartPreview();
		}

		/// <summary>
		/// Raises the surface texture destroyed event.
		/// </summary>
		/// <param name="surface">Surface.</param>
		public bool OnSurfaceTextureDestroyed (SurfaceTexture surface)
		{
			return true;
		}

		/// <summary>
		/// Raises the surface texture size changed event.
		/// </summary>
		/// <param name="surface">Surface.</param>
		/// <param name="width">Width.</param>
		/// <param name="height">Height.</param>
		public void OnSurfaceTextureSizeChanged (SurfaceTexture surface, int width, int height)
		{
			ConfigureTransform(width, height);
			StartPreview();
		}

		/// <summary>
		/// Raises the surface texture updated event.
		/// </summary>
		/// <param name="surface">Surface.</param>
		public void OnSurfaceTextureUpdated (SurfaceTexture surface)
		{
		}

		#endregion
	}
}