using System;
using System.Linq;
using System.Reflection;

using Digillect.Mvvm.Services;

using MetroIoc;

namespace Digillect.Mvvm.Configuration
{
	/// <summary>
	/// Module that is used to register sevices and components into Autofac container.
	/// </summary>
	public class Module
	{
		/// <summary>
		/// Override to add registrations to the container.
		/// </summary>
		/// <param name="builder">The builder through which components can be
		/// registered.</param>
		public virtual void Load( IContainer container )
		{
			// Services
			container.Register<IDataExchangeService, DataExchangeService>( lifecycle: new SingletonLifecycle() );
			//builder.RegisterType<DataExchangeService>().As<IDataExchangeService>().SingleInstance();
		}
	}
}