// --------------------------------------------------------------------------------------------------------------------
// <copyright file="DroidModule.cs" company="Flush Arcade Pty Ltd.">
//   Copyright (c) 2016 Flush Arcade Pty Ltd. All rights reserved.
// </copyright>
// --------------------------------------------------------------------------------------------------------------------

namespace AdaptiveStreaming.Droid.Modules
{
	using Autofac;

	using AdaptiveStreaming.Droid.Extras;
	using AdaptiveStreaming.Droid.Logging;

	using AdaptiveStreaming.Portable.Extras;
	using AdaptiveStreaming.Portable.Logging;
	using AdaptiveStreaming.Portable.Ioc;

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
		}

		#endregion
	}
}