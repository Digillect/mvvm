using System;

namespace Digillect.Mvvm.Services
{
	/// <summary>
	///     Default implementation of <see cref="IViewModelExceptionHandlingService" />.
	/// </summary>
	public class DefaultViewModelExceptionHandlingService : IViewModelExceptionHandlingService
	{
		readonly IExceptionHandlingService _exceptionHandlingService;

		#region Constructors/Disposer
		/// <summary>
		/// Initializes a new instance of the <see cref="DefaultViewModelExceptionHandlingService"/> class.
		/// </summary>
		public DefaultViewModelExceptionHandlingService()
		{
		}

		/// <summary>
		/// Initializes a new instance of the <see cref="DefaultViewModelExceptionHandlingService"/> class.
		/// </summary>
		/// <param name="exceptionHandlingService">The exception handling service.</param>
		public DefaultViewModelExceptionHandlingService( IExceptionHandlingService exceptionHandlingService )
		{
			_exceptionHandlingService = exceptionHandlingService;
		}
		#endregion

		#region IViewModelExceptionHandlingService Members
		/// <summary>
		///     Handle exception the right way.
		/// </summary>
		/// <param name="viewModel">View model that processed session.</param>
		/// <param name="session">Session that caused an exception.</param>
		/// <param name="ex">Exception to handle.</param>
		/// <returns>
		///     <c>true</c> if exception was handled, otherwise <c>false</c>.
		/// </returns>
		public bool HandleException( ViewModel viewModel, Session session, Exception ex )
		{
			if( _exceptionHandlingService != null )
			{
				_exceptionHandlingService.HandleException( ex );

				return true;
			}

			return false;
		}
		#endregion
	}
}