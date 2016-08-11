// --------------------------------------------------------------------------------------------------------------------
// <copyright file="CameraRenderer.cs" company="Flush Arcade Pty Ltd.">
//   Copyright (c) 2016 Flush Arcade Pty Ltd. All rights reserved.
// </copyright>
// --------------------------------------------------------------------------------------------------------------------

[assembly: Xamarin.Forms.ExportRenderer (typeof(Camera.Controls.CameraView), typeof(Camera.Droid.Renderers.CameraView.CameraRenderer))]

namespace Camera.Droid.Renderers.CameraView
{
	using System.Reactive.Linq;
	using System;

	using Xamarin.Forms;
	using Xamarin.Forms.Platform.Android;

	using Camera.Controls;

	using Camera.Portable.Enums;

	/// <summary>
	/// Bodyshop camera renderer.
	/// </summary>
	public class CameraRenderer : ViewRenderer<CameraView, CameraDroid>
	{
		/// <summary>
		/// The bodyshop camera droid.
		/// </summary>
		private CameraDroid BodyshopCameraDroid;

		/// <summary>
		/// Raises the element changed event.
		/// </summary>
		/// <param name="e">E.</param>
		protected override void OnElementChanged(ElementChangedEventArgs<CameraView> e)
		{
			base.OnElementChanged(e);

			if (Element != null)
			{
				BodyshopCameraDroid = new CameraDroid (Context);

				BodyshopCameraDroid.Available += Element.NotifyAvailability;
				BodyshopCameraDroid.Photo += Element.NotifyPhoto;
				BodyshopCameraDroid.Busy += Element.NotifyBusy;

				Element.Flash += HandleFlashChange;
				Element.OpenCamera += HandleCameraInitialisation;
				//Element.Focus += HandleFocus;
				Element.Shutter += HandleShutter;

				SetNativeControl(BodyshopCameraDroid);
			}
		}

		/// <summary>
		/// Handles the camera initialisation.
		/// </summary>
		/// <param name="sender">Sender.</param>
		/// <param name="args">If set to <c>true</c> arguments.</param>
		protected async void HandleCameraInitialisation (object sender, bool args)
		{
			BodyshopCameraDroid.OpenCamera();
			//await BodyshopCameraDroid.InitializeCamera();
			// set orientation handler after initalization
			//Element.OrientationChange += HandleOrientationChange;
			// set starting orientation
			HandleOrientationChange (null, Element.Orientation);
		}

		/// <summary>
		/// Handles the flash change.
		/// </summary>
		/// <param name="sender">Sender.</param>
		/// <param name="args">If set to <c>true</c> arguments.</param>
		protected void HandleFlashChange (object sender, bool args)
		{
			//BodyshopCameraDroid.SwitchFlash (args);
		}

		/// <summary>
		/// Handles the shutter.
		/// </summary>
		/// <param name="sender">Sender.</param>
		/// <param name="e">E.</param>
		private async void HandleShutter (object sender, EventArgs e)
		{
			await Observable.Start (BodyshopCameraDroid.TakePhoto).FirstAsync();
		}

		/// <summary>
		/// Handles the focus.
		/// </summary>
		/// <param name="sender">Sender.</param>
		/// <param name="e">E.</param>
		private void HandleFocus (object sender, Point e)
		{
			//BodyshopCameraDroid.ChangeFocusPoint (e);
		}

		/// <summary>
		/// Raises the measure event.
		/// </summary>
		/// <param name="widthMeasureSpec">Width measure spec.</param>
		/// <param name="heightMeasureSpec">Height measure spec.</param>
		//protected override void OnMeasure(int widthMeasureSpec, int heightMeasureSpec)
		//{
		//	// We purposely disregard child measurements because act as a
		//	// wrapper to a SurfaceView that centers the camera preview instead
		//	// of stretching it.
		//	int width = ResolveSize(SuggestedMinimumWidth, widthMeasureSpec);
		//	int height = ResolveSize(SuggestedMinimumHeight, heightMeasureSpec);

		//	SetMeasuredDimension(width, height);
		//}

		/// <summary>
		/// Handles the orientation change.
		/// </summary>
		/// <param name="sender">Sender.</param>
		/// <param name="e">E.</param>
		protected void HandleOrientationChange (object sender, Orientation e)
		{
			//BodyshopCameraDroid.HandleOrientationChange ();
		}

		/// <summary>
		/// Dispose the specified disposing.
		/// </summary>
		/// <param name="disposing">If set to <c>true</c> disposing.</param>
		protected override void Dispose(bool disposing)
		{
			if (BodyshopCameraDroid != null) 
			{
				BodyshopCameraDroid.Dispose ();

				BodyshopCameraDroid.Available -= Element.NotifyAvailability;
				BodyshopCameraDroid.Photo -= Element.NotifyPhoto;
				BodyshopCameraDroid.Busy -= Element.NotifyBusy;
			}

			if (Element != null) 
			{
				Element.Flash -= HandleFlashChange;
				Element.OpenCamera -= HandleCameraInitialisation;
				//Element.Focus -= HandleFocus;
				Element.Shutter -= HandleShutter;
				Element.OrientationChange -= HandleOrientationChange;
			}

			try
			{
				base.Dispose(disposing);
			}catch (Exception e) {
			}
		}
	}
}