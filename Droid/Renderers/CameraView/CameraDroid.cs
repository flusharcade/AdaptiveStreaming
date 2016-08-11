// --------------------------------------------------------------------------------------------------------------------
// <copyright file="CameraDroid.cs" company="Flush Arcade Pty Ltd.">
//   Copyright (c) 2016 Flush Arcade Pty Ltd. All rights reserved.
// </copyright>
// --------------------------------------------------------------------------------------------------------------------

namespace Camera.Droid.Renderers.CameraView
{
	using System;
	using System.Collections.Generic;
	using System.Linq;
	using System.Text;
	using System.Threading.Tasks;

	using Java.IO;

	using Xamarin.Forms;

	using Android.App;
	using Android.Content;
	using Android.OS;
	using Android.Runtime;
	using Android.Util;
	using Android.Views;
	using Android.Widget;
	using Android.Hardware;
	using Android.Hardware.Camera2;
	using Android.Hardware.Camera2.Params;
	using Android.Graphics;
	using Android.Content.Res;
	using Android.Media;

	using Camera.Portable.Ioc;
	using Camera.Portable.Logging;
	using Java.Lang;

	/// <summary>
	/// Bodyshop camera droid.
	/// </summary>
	public class CameraDroid : FrameLayout, TextureView.ISurfaceTextureListener
	{
		private static readonly SparseIntArray ORIENTATIONS = new SparseIntArray();

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
		/// The media sound loaded.
		/// </summary>
		private bool _mediaSoundLoaded = false;

		/// <summary>
		/// The surface available.
		/// </summary>
		private bool _surfaceAvailable = false;

		/// <summary>
		/// The media sound.
		/// </summary>
		private MediaActionSound mediaSound;

		/// <summary>
		/// The device.
		/// </summary>
		private string _device;

		/// <summary>
		/// The camera open.
		/// </summary>
		private bool _cameraOpen = false;

		/// <summary>
		/// The size of the camera preview.
		/// </summary>
		private Android.Util.Size mPreviewSize;

		/// <summary>
		/// The context.
		/// </summary>
		private Context _context;

		/// <summary>
		/// The camera busy.
		/// </summary>
		public bool CameraBusy = true;

		/// <summary>
		/// The opening camera.
		/// </summary>
		public bool OpeningCamera;

		/// <summary>
		/// The reference to the opened CameraDevice.
		/// </summary>
		public CameraDevice mCameraDevice;

		/// <summary>
		/// Initializes a new instance of the <see cref="Android_Shared.Controls.CameraDroid"/> class.
		/// </summary>
		/// <param name="context">Context.</param>
		public CameraDroid (Context context) : base (context)
		{
			_context = context;
			_mediaSoundLoaded = LoadShutterSound ();

			var inflater = LayoutInflater.FromContext (context);
			if (inflater == null)
				return;

			_device = Build.Model.ToUpper();

			var view = inflater.Inflate (Resource.Layout.CameraLayout, this);

			_cameraTexture = view.FindViewById<AutoFitTextureView> (Resource.Id.CameraTexture);
			_cameraTexture.SurfaceTextureListener = this;

			mStateListener = new CameraStateListener() { Camera = this };

			ORIENTATIONS.Append((int)SurfaceOrientation.Rotation0, 90);
			ORIENTATIONS.Append((int)SurfaceOrientation.Rotation90, 0);
			ORIENTATIONS.Append((int)SurfaceOrientation.Rotation180, 270);
			ORIENTATIONS.Append((int)SurfaceOrientation.Rotation270, 180);
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
			catch (Java.Lang.Exception e) 
			{
				IoC.Resolve<ILogger> ().WriteLineTime ("CameraDroid: Error loading shutter sound " + e);
				return false;
			}
		}

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

			var manager = (CameraManager)_context.GetSystemService(Context.CameraService);

			try
			{
				string cameraId = manager.GetCameraIdList()[0];

				// To get a list of available sizes of camera preview, we retrieve an instance of
				// StreamConfigurationMap from CameraCharacteristics
				CameraCharacteristics characteristics = manager.GetCameraCharacteristics(cameraId);
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
				manager.OpenCamera(cameraId, mStateListener, null);

				Available?.Invoke(this, true);
			}
			catch (Java.Lang.Exception ex)
			{
				Available?.Invoke(this, false);
			}
		}

		/// <summary>
		/// Takes the photo.
		/// </summary>
		public void TakePhoto ()
		{
			try
			{
				if (_mediaSoundLoaded)
				{
					mediaSound.Play(MediaActionSoundType.ShutterClick);
				}

				TakePicture();
			}
			catch (Java.Lang.Exception e)
			{
				IoC.Resolve<ILogger>().WriteLineTime("CameraDroid: Error taking photo " + e);
			}
		}

		/// <summary>
		/// Takes the picture.
		/// </summary>
		private void TakePicture()
		{
			try
			{
				if (_context == null || mCameraDevice == null)
				{
					return;
				}

				var manager = (CameraManager)_context.GetSystemService(Context.CameraService);

				// Pick the best JPEG size that can be captures with this CameraDevice
				var characteristics = manager.GetCameraCharacteristics(mCameraDevice.Id);
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
				captureBuilder.Set(CaptureRequest.ControlMode, new Java.Lang.Integer((int)ControlMode.Auto));

				// Orientation
				var windowManager = _context.GetSystemService(Context.WindowService).JavaCast<IWindowManager>();
				SurfaceOrientation rotation = windowManager.DefaultDisplay.Rotation;
				captureBuilder.Set(CaptureRequest.JpegOrientation, new Java.Lang.Integer(ORIENTATIONS.Get((int)rotation)));

				// Output file
				File file = new File(_context.GetExternalFilesDir(null), "pic.jpg");

				// This listener is called when an image is ready in ImageReader 
				// Right click on ImageAvailableListener in your IDE and go to its definition
				ImageAvailableListener readerListener = new ImageAvailableListener() { File = file };

				// We create a Handler since we want to handle the resulting JPEG in a background thread
				HandlerThread thread = new HandlerThread("CameraPicture");
				thread.Start();
				Handler backgroundHandler = new Handler(thread.Looper);
				reader.SetOnImageAvailableListener(readerListener, backgroundHandler);

				var captureListener = new CameraCaptureListener() { Camera = this, File = file };

				mCameraDevice.CreateCaptureSession(outputSurfaces, new CameraCaptureStateListener()
				{
					OnConfiguredAction = (CameraCaptureSession session) =>
					{
						try
						{
							session.Capture(captureBuilder.Build(), captureListener, backgroundHandler);
						}
						catch (CameraAccessException ex)
						{
							Log.WriteLine(LogPriority.Info, "Capture Session error: ", ex.ToString());
						}
					}
				}, backgroundHandler);
			}
			catch (CameraAccessException ex)
			{
				Log.WriteLine(LogPriority.Info, "Taking picture error: ", ex.StackTrace);
			}
		}

		/// <summary>
		/// Starts the camera previe
		/// </summary>
		public void StartPreview()
		{
			if (mCameraDevice == null || !_cameraTexture.IsAvailable || mPreviewSize == null)
			{
				return;
			}

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
			catch (CameraAccessException ex)
			{
				Log.WriteLine(LogPriority.Info, "Camera2BasicFragment", ex.StackTrace);
			}
		}

		/// <summary>
		/// Configures the transform.
		/// </summary>
		/// <param name="viewWidth">View width.</param>
		/// <param name="viewHeight">View height.</param>
		private void ConfigureTransform(int viewWidth, int viewHeight)
		{
			if (_viewSurface == null || mPreviewSize == null || _context == null)
			{
				return;
			}

			var windowManager = _context.GetSystemService(Context.WindowService).JavaCast<IWindowManager>();

			SurfaceOrientation rotation = windowManager.DefaultDisplay.Rotation;
			Matrix matrix = new Matrix();
			RectF viewRect = new RectF(0, 0, viewWidth, viewHeight);
			RectF bufferRect = new RectF(0, 0, mPreviewSize.Width, mPreviewSize.Height);
			float centerX = viewRect.CenterX();
			float centerY = viewRect.CenterY();

			if (rotation == SurfaceOrientation.Rotation90 || rotation == SurfaceOrientation.Rotation270)
			{
				bufferRect.Offset(centerX - bufferRect.CenterX(), centerY - bufferRect.CenterY());
				matrix.SetRectToRect(viewRect, bufferRect, Matrix.ScaleToFit.Fill);
				float scale = System.Math.Max((float)viewHeight / mPreviewSize.Height, (float)viewWidth / mPreviewSize.Width);
				matrix.PostScale(scale, scale, centerX, centerY);
				matrix.PostRotate(90 * ((int)rotation - 2), centerX, centerY);
			}

			_cameraTexture.SetTransform(matrix);
		}

		/// <summary>
		/// Updates the camera preview, StartPreview() needs to be called in advance
		/// </summary>
		private void UpdatePreview()
		{
			if (mCameraDevice == null)
			{
				return;
			}

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
			catch (CameraAccessException ex)
			{
				Log.WriteLine(LogPriority.Info, "Camera2BasicFragment", ex.StackTrace);
			}
		}

		/// <summary>
		/// Raises the surface texture available event.
		/// </summary>
		/// <param name="surface">Surface.</param>
		/// <param name="w">The width.</param>
		/// <param name="h">The height.</param>
		public void OnSurfaceTextureAvailable (Android.Graphics.SurfaceTexture surface, int w, int h)
		{
			_viewSurface = surface;

			ConfigureTransform(w, h);
			StartPreview();
		}

		/// <summary>
		/// Raises the surface texture destroyed event.
		/// </summary>
		/// <param name="surface">Surface.</param>
		public bool OnSurfaceTextureDestroyed (Android.Graphics.SurfaceTexture surface)
		{
			return true;
		}

		/// <summary>
		/// Raises the surface texture size changed event.
		/// </summary>
		/// <param name="surface">Surface.</param>
		/// <param name="width">Width.</param>
		/// <param name="height">Height.</param>
		public void OnSurfaceTextureSizeChanged (Android.Graphics.SurfaceTexture surface, int width, int height)
		{
			ConfigureTransform(width, height);
			StartPreview();
		}

		/// <summary>
		/// Raises the surface texture updated event.
		/// </summary>
		/// <param name="surface">Surface.</param>
		public void OnSurfaceTextureUpdated (Android.Graphics.SurfaceTexture surface)
		{
		}

		/// <summary>
		/// Releases all resource used by the <see cref="T:Camera.Droid.Renderers.GestureView.CameraDroid"/> object.
		/// </summary>
		/// <remarks>Call <see cref="Dispose"/> when you are finished using the
		/// <see cref="T:Camera.Droid.Renderers.GestureView.CameraDroid"/>. The <see cref="Dispose"/> method leaves the
		/// <see cref="T:Camera.Droid.Renderers.GestureView.CameraDroid"/> in an unusable state. After calling
		/// <see cref="Dispose"/>, you must release all references to the
		/// <see cref="T:Camera.Droid.Renderers.GestureView.CameraDroid"/> so the garbage collector can reclaim the memory
		/// that the <see cref="T:Camera.Droid.Renderers.GestureView.CameraDroid"/> was occupying.</remarks>
		public void Dispose()
		{
			Android.App.Application.SynchronizationContext.Post (state => 
				{
					try
					{
						
					}
					catch (Java.Lang.Exception e)
					{
						IoC.Resolve<ILogger> ().WriteLineTime ("CameraDroid: Camera failed to dispose.");
					}
				} , null);
		}
	}
}