using System;

namespace Digillect.Mvvm.Services
{
	/// <summary>
	/// Windows 8 implementation of <see cref="IDataExchangeService"/>
	/// </summary>
	public sealed class DataExchangeService : IDataExchangeService
	{
		private readonly static object SyncRoot = new object();

		/// <summary>
		/// Occurs when data exchange started.
		/// </summary>
		public event EventHandler DataExchangeStarted;
		/// <summary>
		/// Occurs when data exchange complete.
		/// </summary>
		public event EventHandler DataExchangeComplete;

		private int _dataExchangeCount = 0;

		/// <summary>
		/// Gets the number of active data exchange operations.
		/// </summary>
		public int DataExchangeCount
		{
			get { return _dataExchangeCount; }
		}

		/// <summary>
		/// Informs that another data exchange operation has begun.
		/// </summary>
		public void BeginDataExchange()
		{
			lock( SyncRoot )
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

		/// <summary>
		/// Informs that one of the data exchange operations has ended.
		/// </summary>
		public void EndDataExchange()
		{
			if( _dataExchangeCount > 0 )
			{
				var fire = false;

				lock( SyncRoot )
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
