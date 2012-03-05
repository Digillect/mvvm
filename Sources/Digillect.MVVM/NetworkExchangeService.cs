using System;
using System.Threading;
using System.Threading.Tasks;

#if WINDOWS_PHONE
using Microsoft.Phone.Net.NetworkInformation;
#endif

namespace Digillect.MVVM
{
	public class NetworkExchangeService
	{
		public static NetworkExchangeService Current { get; private set; }

		#region Constructors/Disposer
		static NetworkExchangeService()
		{
			Current = new NetworkExchangeService();
		}

		private NetworkExchangeService()
		{
			NetworkAvailable = true;
			System.Net.NetworkInformation.NetworkChange.NetworkAddressChanged += NetworkChange_NetworkAddressChanged;
			NetworkChange_NetworkAddressChanged( this, EventArgs.Empty );
		}
		#endregion

		#region Network Availability
		public bool NetworkAvailable { get; set; }
		public event EventHandler NetworkAvailabilityChanged;

		private async void NetworkChange_NetworkAddressChanged( object sender, EventArgs e )
		{
			var oldNetworkAvailable = NetworkAvailable;

			NetworkAvailable = await TaskEx.Run<bool>( InspectNetwork );

			if( NetworkAvailable != oldNetworkAvailable && NetworkAvailabilityChanged != null )
				NetworkAvailabilityChanged( this, EventArgs.Empty );
		}

		private static bool InspectNetwork()
		{
			bool result = false;

#if !WINDOWS_PHONE
			result = System.Net.NetworkInformation.NetworkInterface.GetIsNetworkAvailable();
#else
			switch( NetworkInterface.NetworkInterfaceType )
			{
				case NetworkInterfaceType.Wireless80211:
				case NetworkInterfaceType.MobileBroadbandGsm:
				case NetworkInterfaceType.MobileBroadbandCdma:
				case NetworkInterfaceType.Ethernet:
					result = true;
					break;

				default:
					break;
			}
#endif

			return result;
		}
		#endregion
		#region Data-Exchange notifications
		public event EventHandler DataExchangeStarted;
		public event EventHandler DataExchangeComplete;

		private int dataExchangeCount = 0;

		public int DataExchangeCount
		{
			get { return dataExchangeCount; }
		}
		
		public void BeginDataExchange()
		{
			if( Interlocked.Increment( ref dataExchangeCount ) == 1 && DataExchangeStarted != null )
				DataExchangeStarted( this, EventArgs.Empty );
		}

		public void EndDataExchange()
		{
			if( dataExchangeCount > 0 )
			{
				if( Interlocked.Decrement( ref dataExchangeCount ) == 0 && DataExchangeComplete != null )
					DataExchangeComplete( this, EventArgs.Empty );
			}
		}
		#endregion
	}
}
