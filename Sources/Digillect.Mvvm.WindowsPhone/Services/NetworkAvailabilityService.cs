using System;
using System.Net.NetworkInformation;
using System.Threading.Tasks;

using Microsoft.Phone.Net.NetworkInformation;

using Autofac;

namespace Digillect.Mvvm.Services
{
	internal sealed class NetworkAvailabilityService : INetworkAvailabilityService, IStartable
	{
		public void Start()
		{
			NetworkAvailable = true;
			NetworkChange.NetworkAddressChanged += NetworkChange_NetworkAddressChanged;

			NetworkChange_NetworkAddressChanged( null, EventArgs.Empty );
		}

		public bool NetworkAvailable { get; private set; }
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
			switch( Microsoft.Phone.Net.NetworkInformation.NetworkInterface.NetworkInterfaceType )
			{
				case NetworkInterfaceType.Wireless80211:
				case NetworkInterfaceType.MobileBroadbandGsm:
				case NetworkInterfaceType.MobileBroadbandCdma:
				case NetworkInterfaceType.Ethernet:
					return true;

				default:
					break;
			}

			return false;
		}
	}
}
