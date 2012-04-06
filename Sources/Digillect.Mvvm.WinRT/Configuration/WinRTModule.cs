using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using Autofac;

using Digillect.Mvvm.Services;

namespace Digillect.Mvvm.Configuration
{
	public class WinRTModule : Module
	{
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
