using System;

namespace Digillect.Mvvm.Services
{
	/// <summary>
	/// Service interface to handle exceptions that may occure in the application.
	/// </summary>
	public interface IExceptionHandlingService
	{
		/// <summary>
		/// Handle exception the right way.
		/// </summary>
		/// <param name="ex">Exception to handle.</param>
		void HandleException( Exception ex );
	}
}
