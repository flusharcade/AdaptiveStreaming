// --------------------------------------------------------------------------------------------------------------------
// <copyright file="MainPage.cs" company="Flush Arcade Pty Ltd.">
//   Copyright (c) 2016 Flush Arcade Pty Ltd. All rights reserved.
// </copyright>
// --------------------------------------------------------------------------------------------------------------------

namespace AdaptiveStreaming.Pages
{
	using System.Collections.Generic;

	using AdaptiveStreaming.Portable.ViewModels;
	using AdaptiveStreaming.Portable.Ioc;
	using AdaptiveStreaming.UI;

	/// <summary>
	/// Main page.
	/// </summary>
	public partial class MainPage : ExtendedContentPage, INavigableXamarinFormsPage 
	{
		#region Constructors

		/// <summary>
		/// Initializes a new instance of the <see cref="T:adaptive.Pages.MainPage"/> class.
		/// </summary>
		/// <param name="model">Model.</param>
		public MainPage (MainPageViewModel model) : base(model)
		{
			BindingContext = model;
			InitializeComponent ();
		}

		#endregion

		#region INavigableXamarinFormsPage interface

		/// <summary>
		/// Called when page is navigated to.
		/// </summary>
		/// <returns>The navigated to.</returns>
		/// <param name="navigationParameters">Navigation parameters.</param>
		public void OnNavigatedTo(IDictionary<string, object> navigationParameters)
		{
			this.Show (navigationParameters);
		}

		#endregion
	}
}