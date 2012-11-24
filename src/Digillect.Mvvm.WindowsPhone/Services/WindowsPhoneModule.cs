using System;
using System.Collections.Generic;
using System.Linq;

using Autofac;

using Digillect.Mvvm.UI;

namespace Digillect.Mvvm.Services
{
	public class WindowsPhoneModule : Module
	{
		protected override void Load( ContainerBuilder builder )
		{
			base.Load( builder );

			builder.RegisterType<NetworkAvailabilityService>().As<INetworkAvailabilityService>().SingleInstance();
			builder.RegisterType<DataExchangeService>().As<IDataExchangeService>().SingleInstance();
			builder.RegisterType<PageDecorationService>().As<IPageDecorationService>().SingleInstance();
			builder.RegisterType<NavigationService>().As<INavigationService>().SingleInstance();

			builder.RegisterType<PageDataContext>().AsSelf();
			builder.RegisterType<ViewModelPageDataContext>().AsSelf();
		}
	}
}
