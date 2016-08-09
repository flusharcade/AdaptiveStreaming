// --------------------------------------------------------------------------------------------------------------------
// <copyright file="CameraPageViewModel.cs" company="Flush Arcade Pty Ltd.">
//   Copyright (c) 2016 Flush Arcade Pty Ltd. All rights reserved.
// </copyright>
// --------------------------------------------------------------------------------------------------------------------

namespace Camera.Portable.ViewModels
{
	using System;
	using System.Collections.Generic;
	using System.Threading.Tasks;
	using System.Windows.Input;

	using Camera.Portable.Resources;
	using Camera.Portable.Enums;
	using Camera.Portable.UI;
	using Camera.Portable.DataAccess.Storage;
	using Camera.Portable.Extras;

	/// <summary>
	/// Camera page view model.
	/// </summary>
	public sealed class CameraPageViewModel : ViewModelBase
	{
		#region Private Properties

		/// <summary>
		/// The storage.
		/// </summary>
		private readonly ISQLiteStorage _storage;

		/// <summary>
		/// </summary>
		private readonly Func<PhotoItemViewModel> _photoFactory;

		/// <summary>
		/// </summary>
		private Orientation _pageOrientation;

		/// <summary>
		/// </summary>
		private byte[] _photoData;

		/// <summary>
		/// </summary>
		private string _loadingMessage = LabelResources.LoadingCameraMessage;

		/// <summary>
		/// The can capture.
		/// </summary>
		private bool _canCapture;

		/// <summary>
		/// </summary>
		private bool _cameraLoading;

		/// <summary>
		/// </summary>
		private bool _focusShowing;

		/// <summary>
		/// </summary>
		private bool _isFlashOn;

		/// <summary>
		/// </summary>
		private bool _photoEditOn;

		#endregion

		#region Constructors and Destructors

		/// <summary>
		/// Initializes a new instance of the <see cref="T:Camera.Portable.ViewModels.CameraPageViewModel"/> class.
		/// </summary>
		/// <param name="navigation">Navigation.</param>
		/// <param name="commandFactory">Command factory.</param>
		/// <param name="methods">Methods.</param>
		/// <param name="photoFactory">Photo factory.</param>
		/// <param name="storage">Storage.</param>
		public CameraPageViewModel(INavigationService navigation, Func<Action, ICommand> commandFactory,
			IMethods methods, Func<PhotoItemViewModel> photoFactory, ISQLiteStorage storage) 
			: base (navigation, methods)
		{
			_storage = storage;
			_photoFactory = photoFactory;
		}

		#endregion

		#region Public Properties

		/// <summary>
		/// Gets or sets a value indicating whether this <see cref="T:Camera.Portable.ViewModels.CameraPageViewModel"/> can capture.
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
		/// Gets or sets a value indicating whether this <see cref="T:Camera.Portable.ViewModels.CameraPageViewModel"/> focus showing.
		/// </summary>
		/// <value><c>true</c> if focus showing; otherwise, <c>false</c>.</value>
		public bool FocusShowing
		{
			get { return _focusShowing; }
			set { SetProperty(nameof(FocusShowing), ref _focusShowing, value); }
		}

		/// <summary>
		/// Gets or sets a value indicating whether this <see cref="T:Camera.Portable.ViewModels.CameraPageViewModel"/> camera loading.
		/// </summary>
		/// <value><c>true</c> if camera loading; otherwise, <c>false</c>.</value>
		public bool CameraLoading
		{
			get { return _cameraLoading; }
			set { SetProperty(nameof(CameraLoading), ref _cameraLoading, value); }
		}

		/// <summary>
		/// Gets or sets a value indicating whether this <see cref="T:Camera.Portable.ViewModels.CameraPageViewModel"/> is
		/// flash on.
		/// </summary>
		/// <value><c>true</c> if is flash on; otherwise, <c>false</c>.</value>
		public bool IsFlashOn
		{
			get { return _cameraLoading; }
			set { SetProperty(nameof(IsFlashOn), ref _cameraLoading, value); }
		}

		/// <summary>
		/// Gets or sets a value indicating whether this <see cref="T:Camera.Portable.ViewModels.CameraPageViewModel"/> photo
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
		/// <param name="imageData">Image data.</param>
		public void AddPhoto(byte[] imageData)
		{
			PhotoData = imageData;
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
			CameraLoading = false;
		}

		/// <summary>
		/// Ons the disappear.
		/// </summary>
		public void OnDisappear()
		{
			FocusShowing = false;
			CameraLoading = true;
			ResetEditPhoto();
		}

		#endregion

		#region Protected Methods

		/// <summary>
		/// Loads the async.
		/// </summary>
		/// <returns>The async.</returns>
		/// <param name="parameters">Parameters.</param>
		protected override async Task LoadAsync(IDictionary<string, object> parameters)
		{
		}

		#endregion
	}
}