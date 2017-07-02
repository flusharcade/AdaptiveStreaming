﻿using System;
using AdaptiveStreaming.Portable.Models;
using Xamarin.Forms;

namespace AdaptiveStreaming.Controls
{
	/// <summary>
	/// Adaptive player view.
	/// </summary>
	public class AdaptivePlayerView : View
	{
		/// <summary>
		/// Occurs when init player.
		/// </summary>
		public event EventHandler<Stream> InitPlayer;

		/// <summary>
		/// Notifies the init player.
		/// </summary>
		/// <param name="stream">Stream.</param>
		public void NotifyInitPlayer(Stream stream)
		{
			InitPlayer?.Invoke(this, stream);
		}
	}
}
