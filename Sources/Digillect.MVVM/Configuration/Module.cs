using System;
using System.Linq;
using System.Reflection;

using Digillect.Mvvm.Services;

namespace Digillect.Mvvm.Configuration
{
	/// <summary>
	/// Module that is used to register sevices and components into Autofac container.
	/// </summary>
	public class Module
	{
		public virtual void RegisterServices( IServiceProvider provider )
		{
			provider.RegisterService<IDataExchangeService>( () => new DataExchangeService() );
		}
	}
}