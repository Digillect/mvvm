using System;

namespace Digillect.Mvvm.Services
{
	public interface IDataExchangeService
	{
		int DataExchangeCount { get; }

		event EventHandler DataExchangeStarted;
		event EventHandler DataExchangeComplete;

		void BeginDataExchange();
		void EndDataExchange();
	}
}
