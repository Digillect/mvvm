using System;
using System.Net.NetworkInformation;
using System.Threading.Tasks;

using Autofac;

namespace Digillect.Mvvm.Services
{
	[System.Diagnostics.CodeAnalysis.SuppressMessage( "Microsoft.Performance", "CA1812:AvoidUninstantiatedInternalClasses" )]
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

		private void NetworkChange_NetworkAddressChanged( object sender, EventArgs e )
		{
			var oldNetworkAvailable = NetworkAvailable;

			NetworkAvailable = InspectNetwork();//await Task.Run<bool>( InspectNetwork );

			if( NetworkAvailable != oldNetworkAvailable && NetworkAvailabilityChanged != null )
				NetworkAvailabilityChanged( this, EventArgs.Empty );
		}

		private static bool InspectNetwork()
		{
			return NetworkInterface.GetIsNetworkAvailable();
		}
	}
}
