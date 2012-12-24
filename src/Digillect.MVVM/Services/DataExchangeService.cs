using System;

namespace Digillect.Mvvm.Services
{
	[System.Diagnostics.CodeAnalysis.SuppressMessage( "Microsoft.Performance", "CA1812:AvoidUninstantiatedInternalClasses" )]
	internal sealed class DataExchangeService : IDataExchangeService
	{
		private readonly static object syncRoot = new object();

		public event EventHandler DataExchangeStarted;
		public event EventHandler DataExchangeComplete;

		private int _dataExchangeCount = 0;

		public int DataExchangeCount
		{
			get { return _dataExchangeCount; }
		}

		public void BeginDataExchange()
		{
			lock( syncRoot )
			{
				_dataExchangeCount++;
			}

			if( _dataExchangeCount == 1 )
			{
				var handler = DataExchangeStarted;

				if( handler != null )
				{
					handler( this, EventArgs.Empty );
				}
			}
		}

		public void EndDataExchange()
		{
			if( _dataExchangeCount > 0 )
			{
				var fire = false;

				lock( syncRoot )
				{
					if( _dataExchangeCount > 0 )
					{
						if( --_dataExchangeCount == 0 )
						{
							fire = true;
						}
					}
				}

				if( fire )
				{
					var handler = DataExchangeComplete;

					if( handler != null )
					{
						handler( this, EventArgs.Empty );
					}
				}
			}
		}
	}
}
