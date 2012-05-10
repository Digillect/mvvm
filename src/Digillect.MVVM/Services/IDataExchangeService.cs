using System;

namespace Digillect.Mvvm.Services
{
	/// <summary>
	/// Service that is used to indicate that data exchange is in the progress.
	/// </summary>
	public interface IDataExchangeService
	{
		/// <summary>
		/// Gets the number of active data exchange operations.
		/// </summary>
		int DataExchangeCount { get; }

		/// <summary>
		/// Occurs when first data exchange operation begins.
		/// </summary>
		event EventHandler DataExchangeStarted;

		/// <summary>
		/// Occurs when last data exchange operation ends.
		/// </summary>
		event EventHandler DataExchangeComplete;

		/// <summary>
		/// Informs that another data exchange operation has begun.
		/// </summary>
		void BeginDataExchange();

		/// <summary>
		/// Informs that one of the data exchange operations has ended.
		/// </summary>
		void EndDataExchange();
	}
}
