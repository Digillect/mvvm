using System;
using System.Linq;
using System.Reflection;

using Autofac;

using Digillect.Mvvm.Services;

namespace Digillect.Mvvm.Configuration
{
	public class Module : Autofac.Module
	{
		protected override void Load( ContainerBuilder builder )
		{
			// Services
			builder.Register( c => new DataExchangeService() ).As<IDataExchangeService>().SingleInstance();
		}
	}
}