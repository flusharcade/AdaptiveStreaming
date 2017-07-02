﻿﻿﻿﻿// --------------------------------------------------------------------------------------------------------------------
// <copyright file="adaptivePage.cs" company="Flush Arcade Pty Ltd.">
//   Copyright (c) 2016 Flush Arcade Pty Ltd. All rights reserved.
// </copyright>
// --------------------------------------------------------------------------------------------------------------------

namespace AdaptiveStreaming.Pages
{
    using System;
    using System.Collections.Generic;

    using Xamarin.Forms;

    using AdaptiveStreaming.UI;
    using AdaptiveStreaming.Portable.ViewModels;
    using AdaptiveStreaming.Portable.Enums;
    using AdaptiveStreaming.Portable.Logging;
    using AdaptiveStreaming.Controls;
    using AdaptiveStreaming.Converters;
    using AdaptiveStreaming.Portable.Models;

    /// <summary>
    /// adaptive page.
    /// </summary>
    public partial class PlayerPage : ExtendedContentPage, INavigableXamarinFormsPage
    {
		#region Constructors

		/// <summary>
        /// The model.
        /// </summary>
        private PlayerPageViewModel _model;

		#endregion

		#region Constructors

		/// <summary>
		/// Initializes a new instance of the <see cref="T:adaptive.Pages.MainPage"/> class.
		/// </summary>
		/// <param name="model">MoPlayerPageayerPage   public PlayerPage(PlayerPageViewModel model) : base(model)
		public PlayerPage(PlayerPageViewModel model) : base(model)
        {
            BindingContext = model;
            _model = model;

            InitializeComponent();
        }

        #endregion

        #region INavigableXamarinFormsPage implementation

        /// <summary>
        /// Raises the navigated to event.
        /// </summary>
        /// <param name="navigationParameters">Navigation parameters.</param>
        public void OnNavigatedTo(IDictionary<string, object> navigationParameters)
        {
            var stream = new Stream();
            stream.Url = "https://devimages.apple.com.edgekey.net/streaming/examples/bipbop_4x3/bipbop_4x3_variant.m3u8";

            this.Show(navigationParameters);

            this.Player.NotifyInitPlayer(stream);
        }

        #endregion

        #region Public Methods

        #endregion
    }
}