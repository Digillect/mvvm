using System;

using Autofac;

namespace Digillect.Mvvm.Services
{
	/// <summary>
	/// Autofac module that registers default and system services.
	/// </summary>
	[CLSCompliant( false )]
	public class MvvmModule : Autofac.Module
	{
		/// <summary>
		/// Override to add registrations to the container.
		/// </summary>
		/// <param name="builder">The builder through which components can be registered.</param>
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
