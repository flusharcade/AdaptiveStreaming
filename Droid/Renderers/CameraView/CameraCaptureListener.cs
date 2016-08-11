// --------------------------------------------------------------------------------------------------------------------
// <copyright file="CameraCaptureListener.cs" company="Flush Arcade Pty Ltd.">
//   Copyright (c) 2016 Flush Arcade Pty Ltd. All rights reserved.
// </copyright>
// --------------------------------------------------------------------------------------------------------------------

namespace Camera.Droid.Renderers.CameraView
{
	using System;

	using Java.IO;

	using Android.Hardware.Camera2;
	using Android.Widget;

	/// <summary>
	/// This CameraCaptureSession.StateListener uses Action delegates to allow 
	/// the methods to be defined inline, as they are defined more than once.
	/// </summary>
	public class CameraCaptureListener : CameraCaptureSession.CaptureCallback
	{
		public File File;

		public event EventHandler<byte[]> Photo;

		public override void OnCaptureCompleted(CameraCaptureSession session, CaptureRequest request, 
		                                        TotalCaptureResult result)
		{
			if (File != null)
			{
				var randomAccessFile = new RandomAccessFile(File.AbsolutePath, "r");
				byte[] bytes = new byte[(int)randomAccessFile.Length()];
				randomAccessFile.ReadFully(bytes);

				Photo?.Invoke(this, bytes);
			}
		}
	}
}