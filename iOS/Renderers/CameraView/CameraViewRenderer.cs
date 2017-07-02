// --------------------------------------------------------------------------------------------------------------------
// <copyright file="adaptiveViewRenderer.cs" company="Flush Arcade Pty Ltd.">
//   Copyright (c) 2016 Flush Arcade Pty Ltd. All rights reserved.
// </copyright>
// --------------------------------------------------------------------------------------------------------------------

[assembly: Xamarin.Forms.ExportRenderer(typeof(adaptive.Controls.adaptiveView), typeof(adaptive.iOS.Renderers.adaptiveView.adaptiveViewRenderer))]

namespace AdaptiveStreaming.iOS.Renderers.adaptiveView
{
	using System;

	using Xamarin.Forms;
	using Xamarin.Forms.Platform.iOS;

	using AdaptiveStreaming.Controls;

	using AdaptiveStreaming.Portable.Enums;

	/// <summary>
	/// adaptive renderer.
	/// </summary>
	public class adaptiveViewRenderer : ViewRenderer<adaptiveView, adaptiveIOS>
	{
		#region Private Properties

		/// <summary>
		/// The bodyshop adaptive IO.
		/// </summary>
		private adaptiveIOS _bodyshopadaptiveIOS;

		#endregion

		#region Protected Methods

		/// <summary>
		/// Raises the element changed event.
		/// </summary>
		/// <param name="e">E.</param>
		protected override void OnElementChanged(ElementChangedEventArgs<adaptiveView> e)
		{
			base.OnElementChanged(e);

			if (Control == null)
			{
				_bodyshopadaptiveIOS = new adaptiveIOS();

				SetNativeControl(_bodyshopadaptiveIOS);
			}

			if (e.OldElement != null)
			{
				e.OldElement.Flash -= HandleFlash;
				e.OldElement.Openadaptive -= HandleadaptiveInitialisation;
				e.OldElement.Focus -= HandleFocus;
				e.OldElement.Shutter -= HandleShutter;
				e.OldElement.Widths -= HandleWidths;

				_bodyshopadaptiveIOS.Busy -= e.OldElement.NotifyBusy;
				_bodyshopadaptiveIOS.Available -= e.OldElement.NotifyAvailability;
				_bodyshopadaptiveIOS.Photo -= e.OldElement.NotifyPhoto;
			}

			if (e.NewElement != null)
			{
				e.NewElement.Flash += HandleFlash;
				e.NewElement.Openadaptive += HandleadaptiveInitialisation;
				e.NewElement.Focus += HandleFocus;
				e.NewElement.Shutter += HandleShutter;
				e.NewElement.Widths += HandleWidths;

				_bodyshopadaptiveIOS.Busy += e.NewElement.NotifyBusy;
				_bodyshopadaptiveIOS.Available += e.NewElement.NotifyAvailability;
				_bodyshopadaptiveIOS.Photo += e.NewElement.NotifyPhoto;
			}
		}

		/// <summary>
		/// Dispose the specified disposing.
		/// </summary>
		/// <param name="disposing">If set to <c>true</c> disposing.</param>
		protected override void Dispose(bool disposing)
		{
			if (_bodyshopadaptiveIOS != null)
			{
				// stop output session and dispose adaptive elements before popping page
				_bodyshopadaptiveIOS.StopAndDispose();
				_bodyshopadaptiveIOS.Dispose();
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

			if (Element != null && _bodyshopadaptiveIOS != null)
			{
				if (e.PropertyName == VisualElement.HeightProperty.PropertyName ||
					e.PropertyName == VisualElement.WidthProperty.PropertyName)
				{
					_bodyshopadaptiveIOS.SetBounds((nint)Element.Width, (nint)Element.Height);
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
			_bodyshopadaptiveIOS.SetWidths (e);
		}

		/// <summary>
		/// Handles the shutter.
		/// </summary>
		/// <param name="sender">Sender.</param>
		/// <param name="e">E.</param>
		private async void HandleShutter (object sender, EventArgs e)
		{
			await _bodyshopadaptiveIOS.TakePhoto ();
		}

		/// <summary>
		/// Handles the orientation change.
		/// </summary>
		/// <param name="sender">Sender.</param>
		/// <param name="e">E.</param>
		private void HandleOrientationChange (object sender, Orientation e)
		{
			_bodyshopadaptiveIOS.HandleOrientationChange (e);
		}

		/// <summary>
		/// Handles the focus.
		/// </summary>
		/// <param name="sender">Sender.</param>
		/// <param name="e">E.</param>
		private void HandleFocus (object sender, Point e)
		{
			_bodyshopadaptiveIOS.ChangeFocusPoint (e);
		}

		/// <summary>
		/// Handles the adaptive initialisation.
		/// </summary>
		/// <param name="sender">Sender.</param>
		/// <param name="args">If set to <c>true</c> arguments.</param>
		private void HandleadaptiveInitialisation (object sender, bool args)
		{
			_bodyshopadaptiveIOS.Initializeadaptive();

			Element.OrientationChange += HandleOrientationChange;
		}

		/// <summary>
		/// Handles the flash.
		/// </summary>
		/// <param name="sender">Sender.</param>
		/// <param name="args">If set to <c>true</c> arguments.</param>
		private void HandleFlash (object sender, bool args)
		{
			_bodyshopadaptiveIOS.SwitchFlash (args);
		}

		/// <summary>
		/// Handles the focus change.
		/// </summary>
		/// <param name="sender">Sender.</param>
		/// <param name="args">Arguments.</param>
		private void HandleFocusChange (object sender, Point args)
		{
			_bodyshopadaptiveIOS.ChangeFocusPoint (args);
		}

		#endregion
	}
}