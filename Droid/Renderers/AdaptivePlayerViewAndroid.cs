// --------------------------------------------------------------------------------------------------------------------
// <copyright file="AdaptivePlayerViewRenderer.cs" company="Flush Arcade Pty Ltd.">
//   Copyright (c) 2016 Flush Arcade Pty Ltd. All rights reserved.
// </copyright>
// --------------------------------------------------------------------------------------------------------------------

[assembly: Xamarin.Forms.ExportRenderer(typeof(AdaptiveStreaming.Controls.AdaptivePlayerView), 
                                        typeof(AdaptiveStreaming.Droid.Renderers.AdaptivePlayerViewRenderer))]

namespace AdaptiveStreaming.Droid.Renderers
{
	using System;

	using Android.Widget;

	using Xamarin.Forms;
	using Xamarin.Forms.Platform.Android;

	using AdaptiveStreaming.Droid.Exo;

	using AdaptiveStreaming.Controls;

    using AdaptiveStreaming.Portable.Models;

    /// <summary>
    /// Adaptive player view renderer.
    /// </summary>
    public class AdaptivePlayerViewRenderer : ViewRenderer<AdaptivePlayerView, AdaptivePlayer>
	{
		#region Private Properties

		/// <summary>
        /// The player.
        /// </summary>
		private AdaptivePlayer _player;

		#endregion

		#region Protected Methods

		/// <summary>
		/// Raises the element changed event.
		/// </summary>
		/// <param name="e">E.</param>
		protected override void OnElementChanged(ElementChangedEventArgs<AdaptivePlayerView> e)
		{
			base.OnElementChanged(e);

			if (Control == null)
			{
				_player = new AdaptivePlayer(Context);

				SetNativeControl(_player);
			}

			if (e.OldElement != null)
			{
				// something wrong here, not being called on disposal
			}

			if (e.NewElement != null)
			{
				//Camera.Available += e.NewElement.NotifyAvailability;
				//Camera.Photo += e.NewElement.NotifyPhoto;
				//Camera.Busy += e.NewElement.NotifyBusy;

				//e.NewElement.Flash += HandleFlashChange;
				e.NewElement.InitPlayer += HandleInitPlayer;
				//e.NewElement.Focus += HandleFocus;
				//e.NewElement.Shutter += HandleShutter;
			}
		}

		/// <summary>
		/// Dispose the specified disposing.
		/// </summary>
		/// <param name="disposing">If set to <c>true</c> disposing.</param>
		protected override void Dispose(bool disposing)
		{
			//Element.Flash -= HandleFlashChange;
			//Element.OpenCamera -= HandleCameraInitialisation;
			//Element.Focus -= HandleFocus;
			//Element.Shutter -= HandleShutter;

			//Camera.Available -= Element.NotifyAvailability;
			//Camera.Photo -= Element.NotifyPhoto;
			//Camera.Busy -= Element.NotifyBusy;

			base.Dispose(disposing);
		}

		#endregion

		#region Private Methods

		/// <summary>
		/// Handles the camera initialisation.
		/// </summary>
		/// <param name="sender">Sender.</param>
		/// <param name="args">If set to <c>true</c> arguments.</param>
		private void HandleInitPlayer(object sender, Stream args)
		{
			_player.InitStream(args);
            _player.InitializePlayer();
		}

		/// <summary>
		/// Handles the flash change.
		/// </summary>
		/// <param name="sender">Sender.</param>
		/// <param name="args">If set to <c>true</c> arguments.</param>
		private void HandleFlashChange(object sender, bool args)
		{
			//Camera.SwitchFlash(args);
		}

		/// <summary>
		/// Handles the shutter.
		/// </summary>
		/// <param name="sender">Sender.</param>
		/// <param name="e">E.</param>
		private void HandleShutter(object sender, EventArgs e)
		{
			//Camera.TakePhoto();
		}

		/// <summary>
		/// Handles the focus.
		/// </summary>
		/// <param name="sender">Sender.</param>
		/// <param name="e">E.</param>
		private void HandleFocus(object sender, Point e)
		{
			//Camera.ChangeFocusPoint(e);
		}

		#endregion
	}
}