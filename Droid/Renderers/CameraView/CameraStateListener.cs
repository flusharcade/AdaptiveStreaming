// --------------------------------------------------------------------------------------------------------------------
// <copyright file="CameraDroid.cs" company="Flush Arcade Pty Ltd.">
//   Copyright (c) 2016 Flush Arcade Pty Ltd. All rights reserved.
// </copyright>
// --------------------------------------------------------------------------------------------------------------------

namespace Camera.Droid.Renderers.CameraView
{
	using Android.Hardware.Camera2;

	using Xamarin.Forms;

	public class CameraStateListener : CameraDevice.StateCallback
	{
		public CameraDroid Camera;

		public override void OnOpened(CameraDevice camera)
		{
			if (Camera != null)
			{
				Camera.mCameraDevice = camera;
				Camera.StartPreview();
				Camera.mOpeningCamera = false;
			}
		}

		public override void OnDisconnected(CameraDevice camera)
		{
			if (Camera != null)
			{
				camera.Close();
				Camera.mCameraDevice = null;
				Camera.mOpeningCamera = false;
			}
		}

		public override void OnError(CameraDevice camera, CameraError error)
		{
			camera.Close();
			if (Camera != null)
			{
				Camera.mCameraDevice = null;
				//Activity activity = Camera.Activity;
				Camera.mOpeningCamera = false;
				//if (activity != null)
				//{
				//	activity.Finish();
				//}
			}
		}
	}
}

