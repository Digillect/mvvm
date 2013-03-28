using System;
using System.Diagnostics.Contracts;

namespace Digillect.Mvvm.Services
{
	[ContractClassFor( typeof( INavigationService ) )]
	internal abstract class INavigationServiceContract : INavigationService
	{
		#region Implementation of INavigationService
		public void Navigate( string viewName )
		{
			Contract.Requires<ArgumentNullException>( viewName != null, "viewName" );
		}

		public void Navigate( string viewName, Parameters parameters )
		{
			Contract.Requires<ArgumentNullException>( viewName != null, "viewName" );
		}

		public abstract void GoBack();
		#endregion
	}
}