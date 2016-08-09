// --------------------------------------------------------------------------------------------------------------------
// <copyright file="ImageAvailableListener.cs" company="Flush Arcade Pty Ltd.">
//   Copyright (c) 2016 Flush Arcade Pty Ltd. All rights reserved.
// </copyright>
// --------------------------------------------------------------------------------------------------------------------

namespace Camera.Droid.Renderers.CameraView
{
	using Java.Nio;
	using Java.IO;

	using Android.Media;

	/// <summary>
	/// This CameraCaptureSession.StateListener uses Action delegates to allow 
	/// the methods to be defined inline, as they are defined more than once.
	/// </summary>
	public class ImageAvailableListener : Java.Lang.Object, ImageReader.IOnImageAvailableListener
	{
		public File File;
		public void OnImageAvailable(ImageReader reader)
		{
			Image image = null;
			try
			{
				image = reader.AcquireLatestImage();
				ByteBuffer buffer = image.GetPlanes()[0].Buffer;
				byte[] bytes = new byte[buffer.Capacity()];
				buffer.Get(bytes);
				Save(bytes);
			}
			catch (FileNotFoundException ex)
			{
				//Log.WriteLine(LogPriority.Info, "Camera capture session", ex.StackTrace);
			}
			catch (IOException ex)
			{
				//Log.WriteLine(LogPriority.Info, "Camera capture session", ex.StackTrace);
			}
			finally
			{
				if (image != null)
					image.Close();
			}
		}

		/// <summary>
		/// Save the specified bytes.
		/// </summary>
		/// <param name="bytes">Bytes.</param>
		private void Save(byte[] bytes)
		{
			OutputStream output = null;
			try
			{
				if (File != null)
				{
					output = new FileOutputStream(File);
					output.Write(bytes);
				}
			}
			finally
			{
				if (output != null)
				{
					output.Close();
				}
			}
		}
	}
}