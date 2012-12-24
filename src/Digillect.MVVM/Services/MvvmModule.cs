using System;
using System.Collections.Generic;
using System.Linq;

using Autofac;

namespace Digillect.Mvvm.Services
{
	/// <summary>
	/// Autofac module that registers default and system services.
	/// </summary>
	public class MvvmModule : Module
	{
		/// <summary>
		/// Override to add registrations to the container.
		/// </summary>
		/// <param name="builder">The builder through which components can be
		/// registered.</param>
		/// <remarks>
		/// Note that the ContainerBuilder parameter is unique to this module.
		/// </remarks>
		protected override void Load( ContainerBuilder builder )
		{
			base.Load( builder );

			builder.RegisterType<DataExchangeService>().As<IDataExchangeService>().SingleInstance();
		}
	}
}
