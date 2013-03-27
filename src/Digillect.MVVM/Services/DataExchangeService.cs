using System;

namespace Digillect.Mvvm.Services
{
	/// <summary>
	///     Default implementation of <see cref="IDataExchangeService" />
	/// </summary>
	[System.Diagnostics.CodeAnalysis.SuppressMessage( "Microsoft.Performance", "CA1812:AvoidUninstantiatedInternalClasses", Justification = "Instance of this class will be created through IoC container.")]
	internal sealed class DataExchangeService : IDataExchangeService
	{
		private static readonly object SyncRoot = new object();
		private int _dataExchangeCount;

		#region IDataExchangeService Members
		/// <summary>
		///     Occurs when data exchange started.
		/// </summary>
		public event EventHandler DataExchangeStarted;

		/// <summary>
		///     Occurs when data exchange complete.
		/// </summary>
		public event EventHandler DataExchangeComplete;

		/// <summary>
		///     Gets the number of active data exchange operations.
		/// </summary>
		public int DataExchangeCount
		{
			get { return _dataExchangeCount; }
		}

		/// <summary>
		///     Informs that another data exchange operation has begun.
		/// </summary>
		public void BeginDataExchange()
		{
			lock( SyncRoot )
			{
				_dataExchangeCount++;
			}

			if( _dataExchangeCount == 1 )
			{
				EventHandler handler = DataExchangeStarted;

				if( handler != null )
				{
					handler( this, EventArgs.Empty );
				}
			}
		}

		/// <summary>
		///     Informs that one of the data exchange operations has ended.
		/// </summary>
		public void EndDataExchange()
		{
			if( _dataExchangeCount > 0 )
			{
				bool fire = false;
				
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
					EventHandler handler = DataExchangeComplete;

					if( handler != null )
					{
						handler( this, EventArgs.Empty );
					}
				}
			}
		}
		#endregion
	}
}