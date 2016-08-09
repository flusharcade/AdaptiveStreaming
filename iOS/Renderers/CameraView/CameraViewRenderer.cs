// --------------------------------------------------------------------------------------------------------------------
// <copyright file="CameraViewRenderer.cs" company="Flush Arcade Pty Ltd.">
//   Copyright (c) 2016 Flush Arcade Pty Ltd. All rights reserved.
// </copyright>
// --------------------------------------------------------------------------------------------------------------------

[assembly: Xamarin.Forms.ExportRenderer(typeof(Camera.Controls.CameraView), typeof(Camera.iOS.Renderers.CameraView.CameraViewRenderer))]

namespace Camera.iOS.Renderers.CameraView
{
	using System;

	using Xamarin.Forms;
	using Xamarin.Forms.Platform.iOS;

	using Camera.Controls;

	using Camera.Portable.Enums;

	/// <summary>
	/// Camera renderer.
	/// </summary>
	public class CameraViewRenderer : ViewRenderer<CameraView, CameraIOS>
	{
		#region Private Properties

		/// <summary>
		/// The bodyshop camera IO.
		/// </summary>
		private CameraIOS bodyshopCameraIOS;

		#endregion

		#region Protected Methods

		/// <summary>
		/// Raises the element changed event.
		/// </summary>
		/// <param name="e">E.</param>
		protected override void OnElementChanged(ElementChangedEventArgs<CameraView> e)
		{
			base.OnElementChanged(e);

			if (Element != null) 
			{
				bodyshopCameraIOS = new CameraIOS ();

				// notify xamarin forms control of camera availability
				bodyshopCameraIOS.Busy += Element.NotifyBusy;
				// notify xamarin forms control of camera availability
				bodyshopCameraIOS.Available += Element.NotifyAvailability;
				bodyshopCameraIOS.Photo += Element.NotifyPhoto;

				Element.Flash += HandleFlash;
				Element.OpenCamera += HandleCameraInitialisation;
				Element.FocusChange += HandleFocus;
				Element.Shutter += HandleShutter;
				Element.Widths += HandleWidths;

				SetNativeControl (bodyshopCameraIOS);
			}
		}


		/// <summary>
		/// Dispose the specified disposing.
		/// </summary>
		/// <param name="disposing">If set to <c>true</c> disposing.</param>
		protected override void Dispose(bool disposing)
		{
			if (bodyshopCameraIOS != null)
			{
				// stop output session and dispose camera elements before popping page
				bodyshopCameraIOS.StopAndDispose();

				bodyshopCameraIOS.Busy -= Element.NotifyBusy;
				bodyshopCameraIOS.Available -= Element.NotifyAvailability;
				bodyshopCameraIOS.Photo -= Element.NotifyPhoto;

				bodyshopCameraIOS.Dispose();
			}

			if (Element != null)
			{
				Element.Flash -= HandleFlash;
				Element.OrientationChange -= HandleOrientationChange;
				Element.OpenCamera -= HandleCameraInitialisation;
				Element.FocusChange -= HandleFocus;
				Element.Shutter -= HandleShutter;
				Element.Widths -= HandleWidths;
			}

			base.Dispose(disposing);
		}

		/// <summary>
		/// Raises the element property changed event.
		/// </summary>
		/// <param name="sender">Sender.</param>
		/// <param name="e">E.</param>
		protected override void OnElementPropertyChanged(object sender, System.ComponentModel.PropertyChangedEventArgs e)
		{
			base.OnElementPropertyChanged(sender, e);

			if (Element != null && bodyshopCameraIOS != null)
			{
				if (e.PropertyName == VisualElement.HeightProperty.PropertyName ||
					e.PropertyName == VisualElement.WidthProperty.PropertyName)
				{
					bodyshopCameraIOS.SetBounds((nint)Element.Width, (nint)Element.Height);
				}
			}
		}

		#endregion

		#region Private Methods

		/// <summary>
		/// Handles the widths.
		/// </summary>
		/// <param name="sender">Sender.</param>
		/// <param name="e">E.</param>
		private void HandleWidths (object sender, float e)
		{
			bodyshopCameraIOS.SetWidths (e);
		}

		/// <summary>
		/// Handles the shutter.
		/// </summary>
		/// <param name="sender">Sender.</param>
		/// <param name="e">E.</param>
		private async void HandleShutter (object sender, EventArgs e)
		{
			await bodyshopCameraIOS.TakePhoto ();
		}

		/// <summary>
		/// Handles the orientation change.
		/// </summary>
		/// <param name="sender">Sender.</param>
		/// <param name="e">E.</param>
		private void HandleOrientationChange (object sender, Orientation e)
		{
			bodyshopCameraIOS.HandleOrientationChange (e);
		}

		/// <summary>
		/// Handles the focus.
		/// </summary>
		/// <param name="sender">Sender.</param>
		/// <param name="e">E.</param>
		private void HandleFocus (object sender, Point e)
		{
			bodyshopCameraIOS.ChangeFocusPoint (e);
		}

		/// <summary>
		/// Handles the camera initialisation.
		/// </summary>
		/// <param name="sender">Sender.</param>
		/// <param name="args">If set to <c>true</c> arguments.</param>
		private async void HandleCameraInitialisation (object sender, bool args)
		{
			await bodyshopCameraIOS.InitializeCamera();

			Element.OrientationChange += HandleOrientationChange;
		}

		/// <summary>
		/// Handles the flash.
		/// </summary>
		/// <param name="sender">Sender.</param>
		/// <param name="args">If set to <c>true</c> arguments.</param>
		private void HandleFlash (object sender, bool args)
		{
			bodyshopCameraIOS.SwitchFlash (args);
		}

		/// <summary>
		/// Handles the focus change.
		/// </summary>
		/// <param name="sender">Sender.</param>
		/// <param name="args">Arguments.</param>
		private void HandleFocusChange (object sender, Point args)
		{
			bodyshopCameraIOS.ChangeFocusPoint (args);
		}

		#endregion
	}
}