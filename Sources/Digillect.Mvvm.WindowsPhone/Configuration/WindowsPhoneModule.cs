using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using Autofac;

using Digillect.Mvvm.Services;

namespace Digillect.Mvvm.Configuration
{
	/// <summary>
	/// Performs registration of services and components.
	/// </summary>
	public class WindowsPhoneModule : Module
	{
		/// <summary>
		/// Override to add registrations to the container.
		/// </summary>
		/// <param name="builder">The builder through which components can be
		/// registered.</param>
		protected override void Load( ContainerBuilder builder )
		{
			base.Load( builder );

			// Services
			builder.RegisterType<NetworkAvailabilityService>().As<INetworkAvailabilityService>().SingleInstance();

			// Data contexts
			builder.RegisterType<UI.PageDataContext>().AsSelf();
			builder.RegisterType<UI.ViewModelPageDataContext>().AsSelf();
		}
	}
}
