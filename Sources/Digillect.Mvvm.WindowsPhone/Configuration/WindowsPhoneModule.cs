using System;

using Autofac;

using Digillect.Mvvm.Services;

namespace Digillect.Mvvm.Configuration
{
	public class WindowsPhoneModule : Module
	{
		protected override void Load( ContainerBuilder builder )
		{
			base.Load( builder );

			// Services
			builder.RegisterType<NetworkAvailabilityService>().As<INetworkAvailabilityService>().SingleInstance();

			// Parts
			builder.RegisterType<PageDataContext>().AsSelf();
			builder.RegisterType<ViewModelPageDataContext>().AsSelf();
		}
	}
}
