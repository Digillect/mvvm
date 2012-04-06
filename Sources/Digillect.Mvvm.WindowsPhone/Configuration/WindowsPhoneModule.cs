
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
			builder.RegisterType<NetworkAvailabilityService>().As<INetworkAvailabilityService>().SingleInstance();
			builder.RegisterType<PageDecorationService>().As<IPageDecorationService>().SingleInstance();

			// Parts
			builder.RegisterType<UI.PageDataContext>().AsSelf();
			builder.RegisterType<UI.ViewModelPageDataContext>().AsSelf();
		}
	}
}
