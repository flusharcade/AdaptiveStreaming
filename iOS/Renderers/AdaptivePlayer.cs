// --------------------------------------------------------------------------------------------------------------------
// <copyright file="AdaptivePlayer.cs" company="Flush Arcade Pty Ltd.">
//   Copyright (c) 2016 Flush Arcade Pty Ltd. All rights reserved.
// </copyright>
// --------------------------------------------------------------------------------------------------------------------

namespace AdaptiveStreaming.iOS.Renderers
{
	using System;
	using System.Globalization;

	using Foundation;
	using UIKit;
	using AVFoundation;
	using CoreGraphics;

	using AdaptiveStreaming.Portable.Logging;
	using AdaptiveStreaming.Portable.Ioc;
    using AdaptiveStreaming.Portable.Models;

    /// <summary>
    /// Adaptive player.
    /// </summary>
    public sealed class AdaptivePlayer : UIView
	{
		#region Private Properties

		/// <summary>
		/// The tag.
		/// </summary>
		private readonly string _tag;

		/// <summary>
		/// The log.
		/// </summary>
		private readonly ILogger _log;

		/// <summary>
		/// The main view.
		/// </summary>
		private UIView _mainView;

        /// <summary>
        /// The player item.
        /// </summary>
        private AVPlayerItem _playerItem;

        /// <summary>
        /// The player.
        /// </summary>
        private AVPlayer _player;

        /// <summary>
        /// The player layer.
        /// </summary>
        private AVPlayerLayer _playerLayer;

		/// <summary>
		/// The stream.
		/// </summary>
		private Stream _stream;

		#endregion

		#region Events

		/// <summary>
		/// Occurs when busy.
		/// </summary>
		public event EventHandler<bool> Busy;

		/// <summary>
		/// Occurs when available.
		/// </summary>
		public event EventHandler<bool> Available;

		/// <summary>
		/// Occurs when photo.
		/// </summary>
		public event EventHandler<byte[]> Photo;

		#endregion

		#region Constructors

		/// <summary>
		/// Initializes a new instance of the <see cref="T:Camera.iOS.Renderers.CameraView.CameraIOS"/> class.
		/// </summary>
		public AdaptivePlayer()
		{
			_log = IoC.Resolve<ILogger>();
			_tag = $"{GetType()} ";

			_mainView = new UIView() { TranslatesAutoresizingMaskIntoConstraints = false };
			AutoresizingMask = UIViewAutoresizing.FlexibleMargins;

			_player = new AVPlayer();
			_playerLayer = AVPlayerLayer.FromPlayer(_player);

			_mainView.Layer.AddSublayer(_playerLayer);

			Add(_mainView);

			// set layout constraints for main view
			AddConstraints(NSLayoutConstraint.FromVisualFormat("V:|[mainView]|", NSLayoutFormatOptions.DirectionLeftToRight, null, new NSDictionary("mainView", _mainView)));
			AddConstraints(NSLayoutConstraint.FromVisualFormat("H:|[mainView]|", NSLayoutFormatOptions.AlignAllTop, null, new NSDictionary("mainView", _mainView)));
		}

		#endregion

		/// <summary>
		/// Inits the stream.
		/// </summary>
		/// <param name="stream">Stream.</param>
		public void InitStream(Stream stream)
		{
			_stream = stream;
		}

		/// <summary>
		/// Initializes the player.
		/// </summary>
		public void InitializePlayer()
		{
			var myUrl = NSUrl.FromString(_stream.Url);

			_playerItem = new AVPlayerItem(myUrl);
            _player.ReplaceCurrentItemWithPlayerItem(_playerItem);
			_player.Play();
        }

		#region Private Methods

		#endregion

		#region Public Methods

		/// <summary>
		/// Draw the specified rect.
		/// </summary>
		/// <param name="rect">Rect.</param>
		public override void Draw(CGRect rect)
		{
			_playerLayer.Frame = rect;

			base.Draw(rect);
		}

		/// <summary>
		/// Stops the and dispose.
		/// </summary>
		public void StopAndDispose()
		{
            _player.Pause();
            _player.Dispose();
		}

		#endregion
	}
}