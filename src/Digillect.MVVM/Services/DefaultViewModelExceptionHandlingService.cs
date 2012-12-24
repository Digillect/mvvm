using System;
using System.Collections.Generic;
using System.Linq;

namespace Digillect.Mvvm.Services
{
	/// <summary>
	/// Default implementation of <see cref="IViewModelExceptionHandlingService"/>.
	/// </summary>
	public class DefaultViewModelExceptionHandlingService : IViewModelExceptionHandlingService
	{
		/// <summary>
		/// Gets or sets the exception handling service.
		/// </summary>
		/// <value>
		/// The exception handling service.
		/// </value>
		public IExceptionHandlingService ExceptionHandlingService { get; set; }

		/// <summary>
		/// Handle exception the right way.
		/// </summary>
		/// <param name="viewModel">ViewModel that loads session.</param>
		/// <param name="session">Session that is loading.</param>
		/// <param name="ex">Exception to handle.</param>
		/// <returns><c>true</c> if exception was handled, otherwise <c>false</c>.</returns>
		public bool HandleException( ViewModel viewModel, Session session, Exception ex )
		{
			if( ExceptionHandlingService != null )
			{
				ExceptionHandlingService.HandleException( ex );
				return true;
			}

			return false;
		}
	}
}
