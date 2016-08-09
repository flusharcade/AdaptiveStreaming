// --------------------------------------------------------------------------------------------------------------------
// <copyright file="PortableModule.cs" company="Flush Arcade Pty Ltd.">
//   Copyright (c) 2016 Flush Arcade Pty Ltd. All rights reserved.
// </copyright>
// --------------------------------------------------------------------------------------------------------------------
using Camera.Portable.DataAccess.Storage;

namespace Camera.Portable.Modules
{
	using System;

	using Autofac;

	using Camera.Portable.Ioc;
	using Camera.Portable.ViewModels;
	using Camera.Portable.UI;

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
			builder.RegisterType<SQLiteStorage>().As<ISQLiteStorage>().SingleInstance();

			builder.RegisterType<MainPageViewModel> ().SingleInstance();
			builder.RegisterType<CameraPageViewModel> ().SingleInstance();
			builder.RegisterType<EditFilePageViewModel>().SingleInstance();

			builder.RegisterType<PhotoItemViewModel>().SingleInstance();
			builder.RegisterType<FileItemViewModel>().InstancePerDependency();
		}

		#endregion
	}
}