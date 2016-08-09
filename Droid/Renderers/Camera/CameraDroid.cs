//// --------------------------------------------------------------------------------------------------------------------
//// <copyright file="CameraDroid.cs" company="Flush Arcade Pty Ltd.">
////   Copyright (c) 2016 Flush Arcade Pty Ltd. All rights reserved.
//// </copyright>
//// --------------------------------------------------------------------------------------------------------------------

//namespace Camera.Droid.Renderers.CameraView
//{
//	using System;
//	using System.Collections.Generic;
//	using System.Linq;
//	using System.Text;
//	using System.Threading.Tasks;

//	using Android.App;
//	using Android.Content;
//	using Android.OS;
//	using Android.Runtime;
//	using Android.Util;
//	using Android.Views;
//	using Android.Widget;
//	using Android.Hardware;
//	using Android.Hardware.Camera2;
//	using Android.Hardware.Camera2.Params;
//	using Android.Graphics;
//	using Android.Content.Res;
//	using Android.Media;

//	using Camera.Portable.Ioc;
//	using Camera.Portable.Logging;

//	/// <summary>
//	/// Bodyshop camera droid.
//	/// </summary>
//	public class CameraDroid : FrameLayout, TextureView.ISurfaceTextureListener
//	{
//		#region Public Events

//		/// <summary>
//		/// Occurs when busy.
//		/// </summary>
//		public event EventHandler<bool> Busy;

//		/// <summary>
//		/// Occurs when available.
//		/// </summary>
//		public event EventHandler<bool> Available;

//		/// <summary>
//		/// Occurs when photo.
//		/// </summary>
//		public event EventHandler<byte[]> Photo;

//		#endregion

//		/// <summary>
//		/// The CameraRequest.Builder for camera preview.
//		/// </summary>
//		private CaptureRequest.Builder mPreviewBuilder;

//		/// <summary>
//		/// The CameraCaptureSession for camera preview.
//		/// </summary>
//		private CameraCaptureSession mPreviewSession;

//		/// <summary>
//		/// The reference to the opened CameraDevice.
//		/// </summary>
//		private CameraDevice mCameraDevice;

//		///// <summary>
//		///// The camera.
//		///// </summary>
//		//Android.Hardware.Camera _camera;

//		///// <summary>
//		///// The size of the camera.
//		///// </summary>
//		//Android.Hardware.Camera.Size _cameraSize;

//		/// <summary>
//		/// The view surface.
//		/// </summary>
//		private SurfaceTexture _viewSurface;

//		/// <summary>
//		/// The camera texture.
//		/// </summary>
//		private TextureView _cameraTexture;

//		/// <summary>
//		/// The picture callback.
//		/// </summary>
//		private PictureCallback _pictureCallback;

//		/// <summary>
//		/// The camera busy.
//		/// </summary>
//		public bool CameraBusy = true;

//		/// <summary>
//		/// The media sound loaded.
//		/// </summary>
//		private bool _mediaSoundLoaded = false;

//		/// <summary>
//		/// The surface available.
//		/// </summary>
//		private bool _surfaceAvailable = false;

//		/// <summary>
//		/// The media sound.
//		/// </summary>
//		private MediaActionSound mediaSound;

//		/// <summary>
//		/// The device.
//		/// </summary>
//		private string _device;

//		/// <summary>
//		/// The camera open.
//		/// </summary>
//		private bool cameraOpen = false;

//		/// <summary>
//		/// Initializes a new instance of the <see cref="Android_Shared.Controls.CameraDroid"/> class.
//		/// </summary>
//		/// <param name="context">Context.</param>
//		public CameraDroid (Context context) : base (context)
//		{
//			_mediaSoundLoaded = loadShutterSound ();

//			_pictureCallback = new PictureCallback ();

//			_pictureCallback.Busy += HandlePictureCallbackBusy;
//			_pictureCallback.Photo += HandlePictureCallbackPhoto;

//			var inflater = LayoutInflater.FromContext (context);
//			if (inflater == null)
//				return;

//			_device = Build.Model.ToUpper();

//			/*var view = inflater.Inflate (Resource.Layout.Ca, this);

//			_cameraTexture = view.FindViewById<TextureView> (Resource.Id.CameraTexture);
//			_cameraTexture.SurfaceTextureListener = this;*/
//		}

//		/// <summary>
//		/// Loads the shutter sound.
//		/// </summary>
//		/// <returns><c>true</c>, if shutter sound was loaded, <c>false</c> otherwise.</returns>
//		private bool loadShutterSound()
//		{
//			try 
//			{
//				mediaSound = new MediaActionSound ();
//				mediaSound.LoadAsync (MediaActionSoundType.ShutterClick);		
//				return true;
//			}
//			catch (Exception e) 
//			{
//				IoC.Resolve<ILogger> ().WriteLineTime ("CameraDroid: Error loading shutter sound " + e);
//				return false;
//			}
//		}

//		/// <summary>
//		/// Handles the picture callback busy.
//		/// </summary>
//		/// <param name="sender">Sender.</param>
//		/// <param name="e">If set to <c>true</c> e.</param>
//		private void HandlePictureCallbackBusy(object sender, bool e)
//		{
//			CameraBusy = e;

//			if (Busy != null) 
//			{
//				Busy (this, e);
//			}
//		}

//		/// <summary>
//		/// Handles the picture callback photo.
//		/// </summary>
//		/// <param name="sender">Sender.</param>
//		/// <param name="e">E.</param>
//		private void HandlePictureCallbackPhoto(object sender, byte[] e)
//		{
//			if (Photo != null)
//			{
//				Photo (this, e);
//			}
//		}

//		/// <summary>
//		/// Removes the handlers.
//		/// </summary>
//		public void RemoveHandlers()
//		{
//			_pictureCallback.Busy -= HandlePictureCallbackBusy;
//			_pictureCallback.Photo -= HandlePictureCallbackPhoto;
//		}

//		/// <summary>
//		/// Takes the photo.
//		/// </summary>
//		public void TakePhoto ()
//		{
//			if (!CameraBusy)
//			{
//				if (_camera != null)
//				{
//					try
//					{
//						if (_mediaSoundLoaded)
//						{
//							mediaSound.Play(MediaActionSoundType.ShutterClick);
//						}

//						_camera.TakePicture(null, null, _pictureCallback);
//						//camera.SetOneShotPreviewCallback(PreviewCallback);
//					}
//					catch (Exception e)
//					{
//						IoC.Resolve<ILogger>().WriteLineTime("CameraDroid: Error taking photo " + e);
//					}
//				}
//			}
//		}

//		/// <summary>
//		/// Handles the orientation change.
//		/// </summary>
//		public void HandleOrientationChange()
//		{
//			if (_camera != null) 
//			{
//				try 
//				{
//					var windowManager = Context.GetSystemService (Context.WindowService).JavaCast<IWindowManager> ();

//					switch (windowManager.DefaultDisplay.Rotation) 
//					{
//						case SurfaceOrientation.Rotation0:
//							PictureCallback.Rotation = 90;
//							_camera.SetDisplayOrientation (90);
//							break;
//						case SurfaceOrientation.Rotation180:
//							PictureCallback.Rotation = 90;
//							_camera.SetDisplayOrientation (270);
//							break;
//						case SurfaceOrientation.Rotation270:
//							PictureCallback.Rotation = 180;
//							_camera.SetDisplayOrientation (180);
//							break;
//						case SurfaceOrientation.Rotation90:
//							PictureCallback.Rotation = 0;
//							_camera.SetDisplayOrientation (0);
//							break;
//						default:
//							break;
//					}
//				}   
//				catch (Exception) 
//				{
//					IoC.Resolve<ILogger> ().WriteLineTime ("CameraDroid: Camera rotation exception.");
//				}
//			}
//		}

//		/// <summary>
//		/// Converts the DP to pixels.
//		/// </summary>
//		/// <returns>The DP to pixels.</returns>
//		/// <param name="DpValue">Dp value.</param>
//		private int ConvertDPToPixels(float DpValue)
//		{
//			return (int) (DpValue * Resources.DisplayMetrics.Density);
//		}

//		/// <summary>
//		/// Sets the size of the camera picture.
//		/// </summary>
//		private void SetCameraPictureSize()
//		{
//			try
//			{
//				var camParams = _camera.GetParameters ();

//				if (_cameraSize != null)
//				{
//					foreach (var size in _camera.GetParameters().SupportedPictureSizes)
//					{
//						if (size.Width > 1400)
//						{
//							_cameraSize = size;
//						}
//					}

//					camParams.SetPictureSize(_cameraSize.Width, _cameraSize.Height);
//				}

//				_camera.SetParameters (camParams);

//				IoC.Resolve<ILogger> ().WriteLineTime ("CameraDroid: Android picture size set. Height: " + _cameraSize.Height + ", Width: " + _cameraSize.Width);
//			}
//			catch (Exception) 
//			{
//				IoC.Resolve<ILogger> ().WriteLineTime ("CameraDroid: Error setting camera size.");
//			}
//		}

//		/// <summary>
//		/// Switchs the flash.
//		/// </summary>
//		/// <param name="flashOn">If set to <c>true</c> flash on.</param>
//		public void SwitchFlash(bool flashOn)
//		{
//			if (_camera != null) 
//			{
//				try 
//				{
//					var parameters = _camera.GetParameters();
//					parameters.FlashMode = flashOn ? Android.Hardware.Camera.Parameters.FlashModeTorch 
//						: Android.Hardware.Camera.Parameters.FlashModeOff;
//					_camera.SetParameters(parameters);
//				}   
//				catch (Exception e) 
//				{
//					IoC.Resolve<ILogger> ().WriteLineTime ("CameraDroid: Flash exception " + e);
//				}
//			}
//		}

//		/// <summary>
//		/// Initializes the camera.
//		/// </summary>
//		/// <returns>The camera.</returns>
//		public async Task InitializeCamera()
//		{
//			Android.App.Application.SynchronizationContext.Post (state => 
//				{
//					try 
//					{
//						_camera = Android.Hardware.Camera.Open ();
//						cameraOpen = true;
//					}
//					catch (Exception) 
//					{
//						IoC.Resolve<ILogger> ().WriteLineTime ("CameraDroid: Failed to open camera.");
//						cameraOpen = false;
//					}

//					if (_camera == null) 
//					{
//						if (Available != null) 
//						{
//							Available (this, false);
//						}

//						IoC.Resolve<ILogger> ().WriteLineTime ("CameraDroid: No device detected");

//						return;
//					}

//					// set picture size
//					SetCameraPictureSize ();

//					try 
//					{
//						// set and start preview texture
//						_camera.SetPreviewTexture (_viewSurface);
//						_camera.StartPreview ();
//						HandleOrientationChange();
//					}     
//					catch (Exception) 
//					{
//						IoC.Resolve<ILogger> ().WriteLineTime ("CameraDroid: Exception starting camera preview.");
//					}

//					try 
//					{
//						if (Available != null)
//						{
//							Available (this, true);
//						}

//						CameraBusy = false;
//					}
//					catch (Exception) 
//					{
//						IoC.Resolve<ILogger> ().WriteLineTime ("CameraDroid: Exception posting availability.");
//					}

//					IoC.Resolve<ILogger> ().WriteLineTime ("CameraDroid: Camera Initialised.");
//				} , null);
//		}

//		/// <summary>
//		/// Calculates the target focus rect.
//		/// </summary>
//		/// <param name="fPoint">F point.</param>
//		private void calculateTargetFocusRect(Xamarin.Forms.Point fPoint)
//		{
//			float x = (float)fPoint.X;
//			float y = (float)fPoint.Y;

//			Android.Graphics.Rect touchRect = new Android.Graphics.Rect(
//				(int)(x - 25), 
//				(int)(y - 25), 
//				(int)(x + 25), 
//				(int)(y + 25));

//			Android.Graphics.Rect targetFocusRect = new Android.Graphics.Rect(
//				touchRect.Left * 2000 / Width - 1000,
//				touchRect.Top * 2000 / Height - 1000,
//				touchRect.Right * 2000 / Width - 1000,
//				touchRect.Bottom * 2000 / Height - 1000);

//			FocusCamera(targetFocusRect);
//		}

//		/// <summary>
//		/// Changes the focus point.
//		/// </summary>
//		/// <param name="fPoint">F point.</param>
//		public void ChangeFocusPoint(Xamarin.Forms.Point fPoint)
//		{
//			if (_camera != null) 
//			{
//				try 
//				{
//					Android.App.Application.SynchronizationContext.Post (state => 
//						{
//							_camera.CancelAutoFocus();
//							calculateTargetFocusRect(fPoint);
//						} , null);
//				}   
//				catch (Exception e) 
//				{
//					IoC.Resolve<ILogger> ().WriteLineTime ("CameraDroid: Focus exception " + e);
//				}
//			}
//		}

//		/// <summary>
//		/// Focuses the camera.
//		/// </summary>
//		/// <param name="tfocusRect">Tfocus rect.</param>
//		public void FocusCamera(Android.Graphics.Rect tfocusRect) 
//		{
//			var focusList = new List<Android.Hardware.Camera.Area>();
//			var focusArea = new Android.Hardware.Camera.Area(tfocusRect, 1000);
//			focusList.Add(focusArea);

//			var param = _camera.GetParameters();
//			param.FocusAreas = focusList;
//			param.MeteringAreas = focusList;

//			Application.SynchronizationContext.Post (state => 
//				{
//					try
//					{
//						_camera.SetParameters(param);
//						_camera.AutoFocus (new AutoFocusCallbackActivity ());
//					}
//					catch (Exception)
//					{
//						IoC.Resolve<ILogger> ().WriteLineTime ("CameraDroid: Focus exception.");
//					}
//				} , null);
//		}

//		/// <summary>
//		/// Raises the surface texture available event.
//		/// </summary>
//		/// <param name="surface">Surface.</param>
//		/// <param name="w">The width.</param>
//		/// <param name="h">The height.</param>
//		public async void OnSurfaceTextureAvailable (Android.Graphics.SurfaceTexture surface, int w, int h)
//		{
//			_viewSurface = surface;

//			if (_camera != null) 
//			{
//				await InitializeCamera ();
//			}
//		}

//		/// <summary>
//		/// Raises the surface texture destroyed event.
//		/// </summary>
//		/// <param name="surface">Surface.</param>
//		public bool OnSurfaceTextureDestroyed (Android.Graphics.SurfaceTexture surface)
//		{
//			return true;
//		}

//		/// <summary>
//		/// Raises the surface texture size changed event.
//		/// </summary>
//		/// <param name="surface">Surface.</param>
//		/// <param name="width">Width.</param>
//		/// <param name="height">Height.</param>
//		public void OnSurfaceTextureSizeChanged (Android.Graphics.SurfaceTexture surface, int width, int height)
//		{

//		}

//		/// <summary>
//		/// Raises the surface texture updated event.
//		/// </summary>
//		/// <param name="surface">Surface.</param>
//		public void OnSurfaceTextureUpdated (Android.Graphics.SurfaceTexture surface)
//		{
//		}

//		/// <summary>
//		/// Releases all resource used by the <see cref="T:Camera.Droid.Renderers.GestureView.CameraDroid"/> object.
//		/// </summary>
//		/// <remarks>Call <see cref="Dispose"/> when you are finished using the
//		/// <see cref="T:Camera.Droid.Renderers.GestureView.CameraDroid"/>. The <see cref="Dispose"/> method leaves the
//		/// <see cref="T:Camera.Droid.Renderers.GestureView.CameraDroid"/> in an unusable state. After calling
//		/// <see cref="Dispose"/>, you must release all references to the
//		/// <see cref="T:Camera.Droid.Renderers.GestureView.CameraDroid"/> so the garbage collector can reclaim the memory
//		/// that the <see cref="T:Camera.Droid.Renderers.GestureView.CameraDroid"/> was occupying.</remarks>
//		public void Dispose()
//		{
//			Application.SynchronizationContext.Post (state => 
//				{
//					try
//					{
//						if (_camera != null) 
//						{
//							if (_camera.GetParameters ().FlashMode == Android.Hardware.Camera.Parameters.FlashModeTorch) 
//							{
//								SwitchFlash (false);
//							}

//							SwitchFlash (false);

//							if (cameraOpen)
//							{
//								_camera.StopPreview ();
//								_camera.Release ();
//							}
//						}
//					}
//					catch (Exception e)
//					{
//						IoC.Resolve<ILogger> ().WriteLineTime ("CameraDroid: Camera failed to dispose.");
//					}
//				} , null);
//		}
//	}

//	/// <summary>
//	/// Auto focus callback activity.
//	/// </summary>
//	public class AutoFocusCallbackActivity : Activity, Android.Hardware.Camera.IAutoFocusCallback
//	{
//		public void OnAutoFocus(bool success, Android.Hardware.Camera camera)
//		{
//			// camera is an instance of the camera service object.

//			if (success)
//			{
//				// Auto focus was successful - do something here.
//				IoC.Resolve<ILogger> ().WriteLineTime ("CameraDroid: Camera focused.");
//			}
//			else
//			{
//				// Auto focus didn't happen for some reason - react to that here.
//			}
//		}
//	}
//}