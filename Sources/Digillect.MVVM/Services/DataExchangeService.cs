using System;

namespace Digillect.Mvvm.Services
{
	internal class DataExchangeService : IDataExchangeService
	{
		private readonly static object syncRoot = new object();

		public event EventHandler DataExchangeStarted;
		public event EventHandler DataExchangeComplete;

		private int dataExchangeCount = 0;

		public int DataExchangeCount
		{
			get { return dataExchangeCount; }
		}

		public void BeginDataExchange()
		{
			lock( syncRoot )
			{
				dataExchangeCount++;
			}

			if( dataExchangeCount == 1 && DataExchangeStarted != null )
				DataExchangeStarted( this, EventArgs.Empty );
		}

		public void EndDataExchange()
		{
			if( dataExchangeCount > 0 )
			{
				bool fire = false;

				lock( syncRoot )
				{
					if( dataExchangeCount > 0 )
					{
						if( --dataExchangeCount == 0 )
							fire = true;
					}
				}

				if( fire && DataExchangeComplete != null )
					DataExchangeComplete( this, EventArgs.Empty );
			}
		}
	}
}
