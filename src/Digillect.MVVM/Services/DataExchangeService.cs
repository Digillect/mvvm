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
			get { return this.dataExchangeCount; }
		}

		public void BeginDataExchange()
		{
			lock( syncRoot )
			{
				this.dataExchangeCount++;
			}

			if( this.dataExchangeCount == 1 )
			{
				var handler = DataExchangeStarted;

				if( handler != null )
					handler( this, EventArgs.Empty );
			}
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

				if( fire )
				{
					var handler = DataExchangeComplete;

					if( handler != null )
						handler( this, EventArgs.Empty );
				}
			}
		}
	}
}
