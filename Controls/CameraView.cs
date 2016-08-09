// --------------------------------------------------------------------------------------------------
//  <copyright file="CameraView.cs" company="Flush Arcade Pty Ltd.">
//    Copyright (c) 2016 Flush Arcade Pty Ltd. All rights reserved.
//  </copyright>
// --------------------------------------------------------------------------------------------------

namespace Camera.Controls
{
	using System;

	using Xamarin.Forms;

	using Camera.Portable.Enums;

	/// <summary>
	///  This is the control used to embed into Xamarin Forms that will 
	/// yield a custom rendered for taking photos
	/// </summary>
	public sealed class CameraView : ContentView
	{
		/// <summary>
		/// Occurs when orientation change.
		/// </summary>
		public event EventHandler<Orientation> OrientationChange;

		/// <summary>
		/// Occurs when focus.
		/// </summary>
		public event EventHandler<Point> FocusChange;

		/// <summary>
		/// Occurs when availability change.
		/// </summary>
		public event EventHandler<bool> AvailabilityChange;

		/// <summary>
		/// Occurs when open camera.
		/// </summary>
		public event EventHandler<bool> OpenCamera;

		/// <summary>
		/// Occurs when busy.
		/// </summary>
		public event EventHandler<bool> Busy;

		/// <summary>
		/// Occurs when flash.
		/// </summary>
		public event EventHandler<bool> Flash;

		/// <summary>
		/// Occurs when torch.
		/// </summary>
		public event EventHandler<bool> Torch;

		/// <summary>
		/// Occurs when loading.
		/// </summary>
		public event EventHandler<bool> Loading;

		/// <summary>
		/// Occurs when photo.
		/// </summary>
		public event EventHandler<byte[]> Photo;

		/// <summary>
		/// Occurs when widths.
		/// </summary>
		public event EventHandler<float> Widths;

		/// <summary>
		/// Occurs when shutter.
		/// </summary>
		public event EventHandler Shutter;

		/// <summary>
		/// The image rotation angle.
		/// </summary>
		public int ImgRotationAngle = 0;

		/// <summary>
		/// The width of the target.
		/// </summary>
		public int TargetWidth = 0;

		/// <summary>
		/// The height of the target.
		/// </summary>
		public int TargetHeight = 0;

		/// <summary>
		/// The combo showing.
		/// </summary>
		public bool ComboShowing = false;

		/// <summary>
		/// The camera busy.
		/// </summary>
		public bool CameraBusy = false;

		/// <summary>
		/// The camera available.
		/// </summary>
		public bool CameraAvailable = false;

		/// <summary>
		/// The camera focused.
		/// </summary>
		public bool CameraFocused = false;

		/// <summary>
		/// The orientation.
		/// </summary>
		public Orientation Orientation;

		/// <summary>
		/// The width of the camera button container.
		/// </summary>
		public float CameraButtonContainerWidth = 0f;

		/// <summary>
		/// Notifies the shutter.
		/// </summary>
		public void NotifyShutter()
		{
			if (Shutter != null) 
			{
				Shutter (this, EventArgs.Empty);
			}
		}

		/// <summary>
		/// Notifies the open camera.
		/// </summary>
		/// <param name="open">If set to <c>true</c> open.</param>
		public void NotifyOpenCamera(bool open)
		{
			if (OpenCamera != null) 
			{
				OpenCamera (this, open);
			}
		}

		/// <summary>
		/// Notifies the focus.
		/// </summary>
		/// <param name="touchPoint">Touch point.</param>
		public void NotifyFocus(Point touchPoint)
		{
			if (FocusChange != null) 
			{
				FocusChange (this, touchPoint);
			}
		}

		/// <summary>
		/// Notifies the busy.
		/// </summary>
		/// <param name="sender">Sender.</param>
		/// <param name="busy">If set to <c>true</c> busy.</param>
		public void NotifyBusy(object sender, bool busy)
		{
			if (Busy != null) 
			{
				Busy (this, busy);
			}
		}

		/// <summary>
		/// Notifies the orientation change.
		/// </summary>
		/// <param name="orientation">Orientation.</param>
		public void NotifyOrientationChange(Orientation orientation)
		{
			Orientation = orientation;

			if (OrientationChange != null)
			{
				OrientationChange (this, orientation);
			}
		}

		/// <summary>
		/// Notifies the availability.
		/// </summary>
		/// <param name="sender">Sender.</param>
		/// <param name="isAvailable">If set to <c>true</c> is available.</param>
		public void NotifyAvailability(object sender, bool isAvailable)
		{
			CameraAvailable = isAvailable;

			if (AvailabilityChange != null) 
			{
				AvailabilityChange (this, isAvailable);
			}
		}

		/// <summary>
		/// Notifies the photo.
		/// </summary>
		/// <param name="sender">Sender.</param>
		/// <param name="imageData">Image data.</param>
		public void NotifyPhoto(object sender, byte[] imageData)
		{
			if (Photo != null) 
			{
				Photo (this, imageData);
			}
		}
		/// <summary>
		/// Notifies the flash.
		/// </summary>
		/// <param name="flashOn">If set to <c>true</c> flash on.</param>
		public void NotifyFlash(bool flashOn)
		{
			if (Flash != null)
			{
				Flash (this, flashOn);
			}
		}

		/// <summary>
		/// Notifies the torch.
		/// </summary>
		/// <param name="torchOn">If set to <c>true</c> torch on.</param>
		public void NotifyTorch(bool torchOn)
		{
			if (Torch != null)
			{
				Torch (this, torchOn);
			}
		}

		/// <summary>
		/// Notifies the loading.
		/// </summary>
		/// <param name="sender">Sender.</param>
		/// <param name="loading">If set to <c>true</c> loading.</param>
		public void NotifyLoading(object sender, bool loading)
		{
			if (this.Loading != null) 
			{
				this.Loading (this, loading);
			}
		}

		/// <summary>
		/// Notifies the widths.
		/// </summary>
		/// <param name="cameraButtonContainerWidth">Camera button container width.</param>
		public void NotifyWidths(float cameraButtonContainerWidth)
		{
			this.CameraButtonContainerWidth = cameraButtonContainerWidth;

			if (this.Widths != null)
				this.Widths (this, cameraButtonContainerWidth);
		}

		/// <summary>
		/// Initializes a new instance of the <see cref="T:Camera.Controls.CameraView"/> class.
		/// </summary>
		public CameraView()
		{
			BackgroundColor = Color.Black;
		}
	}
}