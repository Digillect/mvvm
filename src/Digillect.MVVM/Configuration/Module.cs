using System;
using System.Linq;
using System.Reflection;

using Autofac;

using Digillect.Mvvm.Services;

namespace Digillect.Mvvm.Configuration
{
	/// <summary>
	/// Module that is used to register sevices and components into Autofac container.
	/// </summary>
	public class Module : Autofac.Module
	{
		/// <summary>
		/// Override to add registrations to the container.
		/// </summary>
		/// <param name="builder">The builder through which components can be
		/// registered.</param>
		protected override void Load( ContainerBuilder builder )
		{
			// Services
			builder.RegisterType<DataExchangeService>().As<IDataExchangeService>().SingleInstance();
		}
	}
}