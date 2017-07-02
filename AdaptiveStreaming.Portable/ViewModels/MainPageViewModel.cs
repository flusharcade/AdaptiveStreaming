// --------------------------------------------------------------------------------------------------------------------
// <copyright file="MainPageViewModel.cs" company="Flush Arcade Pty Ltd.">
//   Copyright (c) 2016 Flush Arcade Pty Ltd. All rights reserved.
// </copyright>
// --------------------------------------------------------------------------------------------------------------------
using AdaptiveStreaming.Portable.Logging;

namespace AdaptiveStreaming.Portable.ViewModels
{
	using System;
	using System.Windows.Input;

	using AdaptiveStreaming.Portable.Enums;
	using AdaptiveStreaming.Portable.UI;
	using AdaptiveStreaming.Portable.Extras;

	/// <summary>
	/// Main page view model.
	/// </summary>
	public class MainPageViewModel : ViewModelBase
	{
		#region Private Properties

		/// <summary>
		/// The methods.
		/// </summary>
		private readonly IMethods _methods;

		/// <summary>
		/// The description message.
		/// </summary>
		private string _descriptionMessage = "Welcome to Adaptive Streaming";

		/// <summary>
		/// The location title.
		/// </summary>
		private string _adaptiveTitle = "Player";

		/// <summary>
		/// The exit title.
		/// </summary>
		private string _exitTitle = "Exit";

		/// <summary>
		/// The location command.
		/// </summary>
		private ICommand _adaptiveCommand;

		/// <summary>
		/// The exit command.
		/// </summary>
		private ICommand _exitCommand;

		#endregion

		#region Public Properties

		/// <summary>
		/// Gets or sets the description message.
		/// </summary>
		/// <value>The description message.</value>
		public string DescriptionMessage
		{
			get { return _descriptionMessage; }
			set { SetProperty(nameof(DescriptionMessage), ref _descriptionMessage, value); }
		}

		/// <summary>
		/// Gets or sets the location title.
		/// </summary>
		/// <value>The location title.</value>
		public string AdaptiveTitle
		{
			get { return _adaptiveTitle; }
			set { SetProperty(nameof(AdaptiveTitle), ref _adaptiveTitle, value); }
		}

		/// <summary>
		/// Gets or sets the adaptive title.
		/// </summary>
		/// <value>The adaptive title.</value>
		public string ExitTitle
		{
			get { return _exitTitle; }
			set { SetProperty(nameof(ExitTitle), ref _exitTitle, value); }
		}

		/// <summary>
		/// Gets or sets the location command.
		/// </summary>
		/// <value>The location command.</value>
		public ICommand adaptiveCommand
		{
			get { return _adaptiveCommand; }
			set { SetProperty(nameof(adaptiveCommand), ref _adaptiveCommand, value); }
		}

		/// <summary>
		/// Gets or sets the exit command.
		/// </summary>
		/// <value>The exit command.</value>
		public ICommand ExitCommand
		{
			get { return _exitCommand; }
			set { SetProperty(nameof(ExitCommand), ref _exitCommand, value); }
		}

		#endregion

		#region Constructors

		/// <summary>
		/// Initializes a new instance of the <see cref="T:adaptive.Portable.ViewModels.MainPageViewModel"/> class.
		/// </summary>
		/// <param name="navigation">Navigation.</param>
		/// <param name="commandFactory">Command factory.</param>
		/// <param name="methods">Methods.</param>
		public MainPageViewModel (INavigationService navigation, Func<Action, ICommand> commandFactory,
			IMethods methods, ILogger logger) : base (navigation)
		{
			_methods = methods;

			_exitCommand = commandFactory (async () =>
			{
				await NotifyAlert("GoodBye!!");

				_methods.Exit();
			});

			_adaptiveCommand = commandFactory (async () => await Navigation.Navigate(PageNames.adaptivePage, null));
		}

		#endregion
	}
}