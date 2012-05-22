using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using Digillect.Mvvm.Services;

using MetroIoc;

namespace Digillect.Mvvm.Configuration
{
	public class WinRTModule : Module
	{
		public override void Load( IContainer container )
		{
			base.Load( container );

			// Services
			container.Register<INetworkAvailabilityService, NetworkAvailabilityService>( lifecycle: new SingletonLifecycle() );
			container.Register<IPageDecorationService, PageDecorationService>( lifecycle: new SingletonLifecycle() );
			//builder.RegisterType<NetworkAvailabilityService>().As<INetworkAvailabilityService>().SingleInstance();
			//builder.RegisterType<PageDecorationService>().As<IPageDecorationService>().SingleInstance();

			// Data contexts
			container.Register<UI.PageDataContext>();
			container.Register<UI.ViewModelPageDataContext>();
			//builder.RegisterType<UI.PageDataContext>().AsSelf();
			//builder.RegisterType<UI.ViewModelPageDataContext>().AsSelf();
		}
	}
}
