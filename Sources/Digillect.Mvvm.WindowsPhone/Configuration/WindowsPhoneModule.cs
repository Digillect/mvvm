
using System;

using Autofac;

using Digillect.Mvvm.Services;

namespace Digillect.Mvvm.Configuration
{
	/// <summary>
	/// Module that is used Windows Phone related services and components.
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
			builder.Register( c => new NetworkAvailabilityService() ).As<INetworkAvailabilityService>().SingleInstance();
			builder.Register( c => new PageDecorationService() ).As<IPageDecorationService>().SingleInstance();

			// Parts
			builder.RegisterType<PageDataContext>().AsSelf();
			builder.RegisterType<ViewModelPageDataContext>().AsSelf();
		}
	}
}
