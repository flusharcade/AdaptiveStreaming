// --------------------------------------------------------------------------------------------------------------------
// <copyright file="PortableModule.cs" company="Flush Arcade Pty Ltd.">
//   Copyright (c) 2016 Flush Arcade Pty Ltd. All rights reserved.
// </copyright>
// --------------------------------------------------------------------------------------------------------------------

namespace AdaptiveStreaming.Portable.Modules
{
	using System;

	using Autofac;

	using AdaptiveStreaming.Portable.Ioc;
	using AdaptiveStreaming.Portable.ViewModels;
	using AdaptiveStreaming.Portable.UI;

	/// <summary>
	/// Portable module.
	/// </summary>
	public class PortableModule : IModule
	{
		#region Public Methods

		/// <summary>
		/// Register the specified builder.
		/// </summary>
		/// <param name="builder">builder.</param>
		public void Register(ContainerBuilder builder)
		{
			builder.RegisterType<MainPageViewModel> ().SingleInstance();
			builder.RegisterType<PlayerPageViewModel> ().SingleInstance();
		}

		#endregion
	}
}