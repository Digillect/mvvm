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
			builder.Register( c => new NetworkAvailabilityService() ).As<INetworkAvailabilityService>().SingleInstance();
			builder.Register( c => new PageDecorationService() ).As<IPageDecorationService>().SingleInstance();

			// Parts
			builder.RegisterType<PageDataContext>().AsSelf();
			builder.RegisterType<ViewModelPageDataContext>().AsSelf();
		}
	}
}
