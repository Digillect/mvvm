using System;
using System.Diagnostics.Contracts;

namespace Digillect.Mvvm.Services
{
	/// <summary>
	/// Service interface to handle exceptions that may occur while ViewModel loads the session.
	/// </summary>
	[ContractClass( typeof( IViewModelExceptionHandlingServiceContract ) )]
	public interface IViewModelExceptionHandlingService
	{
		/// <summary>
		///     Handle exception the right way.
		/// </summary>
		/// <param name="viewModel">View model that processed session.</param>
		/// <param name="session">Session that caused an exception.</param>
		/// <param name="ex">Exception to handle.</param>
		/// <returns>
		///     <c>true</c> if exception was handled, otherwise <c>false</c>.
		/// </returns>
		bool HandleException( ViewModel viewModel, Session session, Exception ex );
	}
}
