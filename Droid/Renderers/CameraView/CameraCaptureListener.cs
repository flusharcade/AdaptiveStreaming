// --------------------------------------------------------------------------------------------------------------------
// <copyright file="CameraCaptureListener.cs" company="Flush Arcade Pty Ltd.">
//   Copyright (c) 2016 Flush Arcade Pty Ltd. All rights reserved.
// </copyright>
// --------------------------------------------------------------------------------------------------------------------
using System;

namespace Camera.Droid.Renderers.CameraView
{
	using Java.IO;

	using Android.Hardware.Camera2;
	using Android.Widget;

	/// <summary>
	/// This CameraCaptureSession.StateListener uses Action delegates to allow 
	/// the methods to be defined inline, as they are defined more than once.
	/// </summary>
	public class CameraCaptureListener : CameraCaptureSession.CaptureCallback
	{
		public CameraDroid Camera;

		public File File;

		public event EventHandler<File> Photo;

		public override void OnCaptureCompleted(CameraCaptureSession session, CaptureRequest request, 
		                                        TotalCaptureResult result)
		{
			if (Camera != null && File != null)
			{
				var activity = Camera.Context;
				if (activity != null)
				{
					Camera.StartPreview();
				}
			}
		}
	}
}