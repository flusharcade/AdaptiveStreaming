﻿﻿﻿// --------------------------------------------------------------------------------------------------------------------
// <copyright file="PlayerPageViewModel.cs" company="Flush Arcade Pty Ltd.">
//   Copyright (c) 2016 Flush Arcade Pty Ltd. All rights reserved.
// </copyright>
// --------------------------------------------------------------------------------------------------------------------

namespace AdaptiveStreaming.Portable.ViewModels
{
	using System;
	using System.Collections.Generic;
	using System.Threading.Tasks;
	using System.Windows.Input;

	using AdaptiveStreaming.Portable.Resources;
	using AdaptiveStreaming.Portable.Enums;
	using AdaptiveStreaming.Portable.UI;
	using AdaptiveStreaming.Portable.Extras;

	/// <summary>
	/// Adaptive page view model.
	/// </summary>
	public sealed class PlayerPageViewModel : ViewModelBase
	{
		#region Private Properties

		/// <summary>
		/// The page orientation.
		/// </summary>
		private Orientation _pageOrientation;

		/// <summary>
		/// The photo data.
		/// </summary>
		private byte[] _photoData;

		/// <summary>
		/// The loading message.
		/// </summary>
		private string _loadingMessage = LabelResources.LoadingAdaptiveMessage;

		/// <summary>
		/// The can capture.
		/// </summary>
		private bool _canCapture;

		/// <summary>
		/// The adaptive loading.
		/// </summary>
		private bool _adaptiveLoading;

		/// <summary>
		/// The is flash on.
		/// </summary>
		private bool _isFlashOn;

		/// <summary>
		/// The photo edit on.
		/// </summary>
		private bool _photoEditOn;

		#endregion

		#region Constructors and Destructors

		/// <summary>
        /// Initializes a new instance of the <see cref="T:AdaptiveStreaming.Portable.ViewModels.PlayerPageViewModel"/> class.
        /// </summary>
        /// <param name="navigation">Navigation.</param>
        /// <param name="commandFactory">Command factory.</param>
		public PlayerPageViewModel(INavigationService navigation, Func<Action, ICommand> commandFactory) 
            : base (navigation)
		{
		}

		#endregion

		#region Public Properties

		/// <summary>
		/// Gets or sets a value indicating whether this <see cref="T:adaptive.Portable.ViewModels.PlayerPageViewModel"/> can capture.
		/// </summary>
		/// <value><c>true</c> if can capture; otherwise, <c>false</c>.</value>
		public bool CanCapture
		{
			get { return _canCapture; }
			set { SetProperty(nameof(CanCapture), ref _canCapture, value); }
		}

		/// <summary>
		/// Gets or sets the loading message.
		/// </summary>
		/// <value>The loading message.</value>
		public string LoadingMessage
		{
			get { return _loadingMessage; }
			set { SetProperty(nameof(LoadingMessage), ref _loadingMessage, value); }
		}

		/// <summary>
		/// Gets or sets a value indicating whether this <see cref="T:adaptive.Portable.ViewModels.PlayerPageViewModel"/> adaptive loading.
		/// </summary>
		/// <value><c>true</c> if adaptive loading; otherwise, <c>false</c>.</value>
		public bool adaptiveLoading
		{
			get { return _adaptiveLoading; }
			set { SetProperty(nameof(adaptiveLoading), ref _adaptiveLoading, value); }
		}

		/// <summary>
		/// Gets or sets a value indicating whether this <see cref="T:adaptive.Portable.ViewModels.PlayerPageViewModel"/> is
		/// flash on.
		/// </summary>
		/// <value><c>true</c> if is flash on; otherwise, <c>false</c>.</value>
		public bool IsFlashOn
		{
			get { return _isFlashOn; }
			set { SetProperty(nameof(IsFlashOn), ref _isFlashOn, value); }
		}

		/// <summary>
		/// Gets or sets a value indicating whether this <see cref="T:adaptive.Portable.ViewModels.PlayerPageViewModel"/> photo
		/// edit on.
		/// </summary>
		/// <value><c>true</c> if photo edit on; otherwise, <c>false</c>.</value>
		public bool PhotoEditOn
		{
			get { return _photoEditOn; }
			set { SetProperty(nameof(PhotoEditOn), ref _photoEditOn, value); }
		}

		/// <summary>
		/// Gets or sets the page orientation.
		/// </summary>
		/// <value>The page orientation.</value>
		public Orientation PageOrientation
		{
			get { return _pageOrientation; }
			set { SetProperty(nameof(PageOrientation), ref _pageOrientation, value); }
		}

		/// <summary>
		/// Gets or sets the photo data.
		/// </summary>
		/// <value>The photo data.</value>
		public byte[] PhotoData
		{
			get { return _photoData; }
			set { SetProperty(nameof(PhotoData), ref _photoData, value); }
		}

		#endregion

		#region Public Methods and Operators

		/// <summary>
		/// Updates the photo to edit.
		/// </summary>
		/// <param name="data">Image data.</param>
		public void AddPhoto(byte[] data)
		{
			PhotoData = data;
			PhotoEditOn = true;
		}

		/// <summary>
		/// The reset edit photo.
		/// </summary>
		public void ResetEditPhoto()
		{
			PhotoData = new byte[] { };
			PhotoEditOn = false;
		}

		/// <summary>
		/// Ons the appear.
		/// </summary>
		public void OnAppear()
		{
			adaptiveLoading = false;
		}

		/// <summary>
		/// Ons the disappear.
		/// </summary>
		public void OnDisappear()
		{
			adaptiveLoading = true;
			ResetEditPhoto();
		}

		#endregion
	}
}