﻿using System;

using AdaptiveStreaming.Portable.Helpers;

namespace AdaptiveStreaming.Portable.Models
{
    public class Stream : ObservableObject
    {
        /// <summary>
        /// The main URL.
        /// </summary>
        public string Url;

		/// <summary>
		/// The prefer extension decoders.
		/// </summary>
		public bool PreferExtensionDecoders;

		/// <summary>
		/// The drm scheme UUID extra.
		/// </summary>
		public string DrmSchemeUuidExtra;

		/// <summary>
		/// The drm license URL.
		/// </summary>
		public string DrmLicenseUrl;

		/// <summary>
		/// The extension extra.
		/// </summary>
		public string ExtensionExtra;

		/// <summary>
		/// The drm key request properties.
		/// </summary>
		public string[] DrmKeyRequestProperties;

		/// <summary>
		/// The URI list extra.
		/// </summary>
		public string[] UriListExtra;

		/// <summary>
		/// The extension list extra.
		/// </summary>
		public string[] ExtensionListExtra;
    }
}
