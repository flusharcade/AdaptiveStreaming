// --------------------------------------------------------------------------------------------------------------------
// <copyright file="DroidModule.cs" company="Flush Arcade Pty Ltd.">
//   Copyright (c) 2016 Flush Arcade Pty Ltd. All rights reserved.
// </copyright>
// --------------------------------------------------------------------------------------------------------------------

namespace Camera.Droid.Modules
{
	using SQLite.Net.Interop;
	using SQLite.Net.Platform.XamarinAndroid;


	using Autofac;

	using Camera.Droid.Extras;
	using Camera.Droid.DataAccess;
	using Camera.Droid.Logging;

	using Camera.Portable.Extras;
	using Camera.Portable.Logging;
	using Camera.Portable.DataAccess.Storage;
	using Camera.Portable.Ioc;

	/// <summary>
	/// Droid module.
	/// </summary>
	public class DroidModule : IModule
	{
		#region Public Methods

		/// <summary>
		/// Register the specified builder.
		/// </summary>
		/// <param name="builder">builder.</param>
		public void Register(ContainerBuilder builder)
		{
			builder.RegisterType<DroidMethods>().As<IMethods>().SingleInstance();
			builder.RegisterType<LoggerDroid>().As<ILogger>().SingleInstance();

			builder.RegisterType<SQLiteSetup>().As<ISQLiteSetup>().SingleInstance();
			builder.RegisterType<SQLitePlatformAndroid>().As<ISQLitePlatform>().SingleInstance();
		}

		#endregion
	}
}