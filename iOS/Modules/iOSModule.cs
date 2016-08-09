// --------------------------------------------------------------------------------------------------------------------
// <copyright file="IOSModule.cs" company="Flush Arcade Pty Ltd.">
//   Copyright (c) 2016 Flush Arcade Pty Ltd. All rights reserved.
// </copyright>
// --------------------------------------------------------------------------------------------------------------------

namespace Camera.iOS.Modules
{
	using Autofac;

	using Camera.iOS.DataAccess;
	using Camera.iOS.Extras;
	using Camera.iOS.Logging;

	using Camera.Portable.DataAccess.Storage;
	using Camera.Portable.Extras;
	using Camera.Portable.Ioc;
	using Camera.Portable.Logging;

	using SQLite.Net.Interop;
	using SQLite.Net.Platform.XamarinIOS;

	/// <summary>
	/// IOS Module.
	/// </summary>
	public class IOSModule : IModule
	{
		#region Public Methods

		/// <summary>
		/// Register the specified builder.
		/// </summary>
		/// <param name="builder">builder.</param>
		public void Register(ContainerBuilder builder)
		{
			builder.RegisterType<IOSMethods>().As<IMethods>().SingleInstance();
			builder.RegisterType<LoggeriOS>().As<ILogger>().SingleInstance();

			builder.RegisterType<SQLiteSetup>().As<ISQLiteSetup>().SingleInstance();
			builder.RegisterType<SQLitePlatformIOS>().As<ISQLitePlatform>().SingleInstance();
		}

		#endregion
	}
}