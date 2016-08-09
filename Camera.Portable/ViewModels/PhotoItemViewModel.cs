// --------------------------------------------------------------------------------------------------------------------
// <copyright file="PhotoItemViewModel.cs" company="Flush Arcade Pty Ltd.">
//   Copyright (c) 2016 Flush Arcade Pty Ltd. All rights reserved.
// </copyright>
// --------------------------------------------------------------------------------------------------------------------

namespace Camera.Portable.ViewModels
{
	using System;

	using Camera.Portable.UI;
	using Camera.Portable.Enums;
	using Camera.Portable.Extras;

	/// <summary>
	/// Photo item view model.
	/// </summary>
	public sealed class PhotoItemViewModel : ViewModelBase
	{
		#region Static Fields

		#endregion

		#region Constructors and Destructors

		/// <summary>
		/// Initializes a new instance of the <see cref="T:Camera.Portable.ViewModels.PhotoItemViewModel"/> class.
		/// </summary>
		/// <param name="navigation">Navigation.</param>
		/// <param name="methods">Methods.</param>
		public PhotoItemViewModel(INavigationService navigation, IMethods methods) 
			: base (navigation, methods)
		{

		}

		#endregion

		#region Fields

		/// <summary>
		/// </summary>
		private byte[] _imageData;

		/// <summary>
		/// The orientation.
		/// </summary>
		private Orientation _orientation;

		#endregion

		#region Public Properties

		/// <summary>
		/// Gets or sets the image data.
		/// </summary>
		/// <value>The image data.</value>
		public byte[] ImageData
		{
			get { return _imageData; }
			set { SetProperty(nameof(ImageData), ref _imageData, value); }
		}

		/// <summary>
		/// Gets or sets the orientation.
		/// </summary>
		/// <value>The orientation.</value>
		public Orientation Orientation
		{
			get { return _orientation; }
			set { SetProperty(nameof(Orientation), ref _orientation, value); }
		}

		#endregion

		#region Public Methods and Operators

		/// <summary>
		/// Apply the specified imageData and orientation.
		/// </summary>
		/// <param name="imageData">Image data.</param>
		/// <param name="orientation">Orientation.</param>
		public void Apply(byte[] imageData, Orientation orientation)
		{
			ImageData = imageData;
			Orientation = orientation;
		}

		#endregion
	}
}