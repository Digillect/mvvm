using System;
using System.Diagnostics.Contracts;

namespace Digillect.Mvvm.Services
{
	[ContractClassFor( typeof( IViewModelExceptionHandlingService ) )]
	internal abstract class IViewModelExceptionHandlingServiceContract : IViewModelExceptionHandlingService
	{
		#region Implementation of IViewModelExceptionHandlingService
		public bool HandleException( ViewModel viewModel, Session session, Exception ex )
		{
			Contract.Requires<ArgumentNullException>( viewModel != null, "viewModel" );
			Contract.Requires<ArgumentNullException>( session != null, "session" );
			Contract.Requires<ArgumentNullException>( ex != null, "ex" );

			return false;
		}
		#endregion
	}
}