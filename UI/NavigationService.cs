// --------------------------------------------------------------------------------------------------------------------
// <copyright file="NavigationService.cs" company="Flush Arcade Pty Ltd.">
//   Copyright (c) 2016 Flush Arcade Pty Ltd. All rights reserved.
// </copyright>
// --------------------------------------------------------------------------------------------------------------------

namespace AdaptiveStreaming.UI
{
	using System.Threading.Tasks;
	using System.Collections.Generic;

	using Xamarin.Forms;

	using AdaptiveStreaming.Pages;

	using AdaptiveStreaming.Portable.UI;
	using AdaptiveStreaming.Portable.Enums;
	using AdaptiveStreaming.Portable.Ioc;

	/// <summary>
	/// Navigation service.
	/// </summary>
	public class NavigationService : INavigationService
	{
		#region INavigationService implementation

		/// <summary>
		/// Navigate the specified pageName and navigationParameters.
		/// </summary>
		/// <param name="pageName">Page name.</param>
		/// <param name="navigationParameters">Navigation parameters.</param>
		public async Task Navigate (PageNames pageName, IDictionary<string, object> navigationParameters)
		{
			var page = GetPage (pageName);

			if (page != null) 
			{
				var navigablePage = page as INavigableXamarinFormsPage;

				if (navigablePage != null) 
				{
					await IoC.Resolve<NavigationPage> ().PushAsync (page);
					navigablePage.OnNavigatedTo (navigationParameters);
				}
			}
		}

		/// <summary>
		/// Pop this instance.
		/// </summary>
		public async Task Pop()
		{
			await IoC.Resolve<NavigationPage>().PopAsync();
		}

		#endregion

		/// <summary>
		/// Gets the page.
		/// </summary>
		/// <returns>The page.</returns>
		/// <param name="page">Page.</param>
		private Page GetPage(PageNames page)
		{
			switch(page)
			{
				case PageNames.MainPage:
					return IoC.Resolve<MainPage> ();
				case PageNames.adaptivePage:
					return IoC.Resolve<PlayerPage> ();
				default:
					return null;
			}
		}
	}
}