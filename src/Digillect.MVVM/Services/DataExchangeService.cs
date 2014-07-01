#region Copyright (c) 2011-2014 Gregory Nickonov and Andrew Nefedkin (Actis® Wunderman)
// Copyright (c) 2011-2014 Gregory Nickonov and Andrew Nefedkin (Actis® Wunderman).
// 
// Permission is hereby granted, free of charge, to any person obtaining a copy of this
// software and associated documentation files (the "Software"), to deal in the Software
// without restriction, including without limitation the rights to use, copy, modify, merge,
// publish, distribute, sublicense, and/or sell copies of the Software, and to permit persons
// to whom the Software is furnished to do so, subject to the following conditions:
// 
// The above copyright notice and this permission notice shall be included in all copies or
// substantial portions of the Software.
// 
// THE SOFTWARE IS PROVIDED "AS IS", WITHOUT WARRANTY OF ANY KIND, EXPRESS OR
// IMPLIED, INCLUDING BUT NOT LIMITED TO THE WARRANTIES OF MERCHANTABILITY,
// FITNESS FOR A PARTICULAR PURPOSE AND NONINFRINGEMENT. IN NO EVENT SHALL
// THE AUTHORS OR COPYRIGHT HOLDERS BE LIABLE FOR ANY CLAIM, DAMAGES OR OTHER
// LIABILITY, WHETHER IN AN ACTION OF CONTRACT, TORT OR OTHERWISE, ARISING FROM,
// OUT OF OR IN CONNECTION WITH THE SOFTWARE OR THE USE OR OTHER DEALINGS
// IN THE SOFTWARE.
#endregion

using System;
using System.ComponentModel;
using System.Diagnostics.CodeAnalysis;
using System.Runtime.CompilerServices;

namespace Digillect.Mvvm.Services
{
	/// <summary>
	///     Default implementation of <see cref="IDataExchangeService" />
	/// </summary>
	[SuppressMessage( "Microsoft.Performance", "CA1812:AvoidUninstantiatedInternalClasses", Justification = "Instance of this class will be created through IoC container." )]
#if WINDOWS_PHONE_71
	public
#else
	internal
#endif
	sealed class DataExchangeService : IDataExchangeService, INotifyPropertyChanged
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
			set
			{
				_dataExchangeCount = value;
				OnPropertyChanged();
			}
		}

		/// <summary>
		///     Informs that another data exchange operation has begun.
		/// </summary>
		public void BeginDataExchange()
		{
			lock( SyncRoot )
			{
				DataExchangeCount++;
			}

			if( DataExchangeCount == 1 )
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
			if( DataExchangeCount > 0 )
			{
				bool fire = false;

				lock( SyncRoot )
				{
					if( DataExchangeCount > 0 )
					{
						if( --DataExchangeCount == 0 )
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

		#region INotifyPropertyChanged
		public event PropertyChangedEventHandler PropertyChanged;

		void OnPropertyChanged( [CallerMemberName] string propertyName = null )
		{
			PropertyChangedEventHandler handler = PropertyChanged;
			if( handler != null )
			{
				handler( this, new PropertyChangedEventArgs( propertyName ) );
			}
		}
		#endregion
	}
}