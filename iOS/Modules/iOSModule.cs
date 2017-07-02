// --------------------------------------------------------------------------------------------------------------------
// <copyright file="IOSModule.cs" company="Flush Arcade Pty Ltd.">
//   Copyright (c) 2016 Flush Arcade Pty Ltd. All rights reserved.
// </copyright>
// --------------------------------------------------------------------------------------------------------------------

namespace AdaptiveStreaming.iOS.Modules
{
	using Autofac;

	using AdaptiveStreaming.iOS.Extras;
	using AdaptiveStreaming.iOS.Logging;

	using AdaptiveStreaming.Portable.Extras;
	using AdaptiveStreaming.Portable.Ioc;
	using AdaptiveStreaming.Portable.Logging;

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
		}

		#endregion
	}
}