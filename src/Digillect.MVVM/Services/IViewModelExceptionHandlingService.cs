using System;
using System.Collections.Generic;
using System.Linq;

namespace Digillect.Mvvm.Services
{
	/// <summary>
	/// Service interface to handle exceptions that may occure while ViewModel loads the session.
	/// </summary>
	public interface IViewModelExceptionHandlingService
	{
		/// <summary>
		/// Handle exception the right way.
		/// </summary>
		/// <param name="viewModel">ViewModel that loads session.</param>
		/// <param name="session">Session that is loading.</param>
		/// <param name="ex">Exception to handle.</param>
		/// <returns><c>true</c> if exception was handled, otherwise <c>false</c>.</returns>
		bool HandleException( ViewModel viewModel, Session session, Exception ex );
	}
}
