using System;
using System.Net.NetworkInformation;
using System.Threading.Tasks;

using Autofac;

namespace Digillect.Mvvm.Services
{
	/// <summary>
	/// Windows 8 implementation of <see cref="INetworkAvailabilityService"/>.
	/// </summary>
	public sealed class NetworkAvailabilityService : INetworkAvailabilityService, IStartable
	{
		/// <summary>
		/// Perform once-off startup processing.
		/// </summary>
		public void Start()
		{
			NetworkAvailable = true;
			NetworkChange.NetworkAddressChanged += NetworkChange_NetworkAddressChanged;

			NetworkChange_NetworkAddressChanged( null, EventArgs.Empty );
		}

		/// <summary>
		/// Gets a value indicating whether network connection is available.
		/// </summary>
		/// <value>
		/// <c>true</c> if network connection available; otherwise, <c>false</c>.
		/// </value>
		public bool NetworkAvailable { get; private set; }
		/// <summary>
		/// Occurs when network availability changed.
		/// </summary>
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
