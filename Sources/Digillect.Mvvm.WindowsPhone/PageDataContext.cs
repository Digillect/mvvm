using System;
using System.Windows;

using Digillect.Mvvm.Services;

namespace Digillect.Mvvm
{
	public class PageDataContext : ObservableObject, IDisposable
	{
		public delegate PageDataContext Factory( PhoneApplicationPage page );

		private readonly PhoneApplicationPage page;
		private readonly INetworkAvailabilityService networkAvailabilityService;
		private bool networkAvailable;

		#region Constructors/Disposer
		public PageDataContext( PhoneApplicationPage page, INetworkAvailabilityService networkAvailabilityService )
		{
			this.page = page;
			this.networkAvailabilityService = networkAvailabilityService;
			this.networkAvailable = this.networkAvailabilityService.NetworkAvailable;

			this.networkAvailabilityService.NetworkAvailabilityChanged += NetworkExchangeService_NetworkAvailabilityChanged;
		}

		public void Dispose()
		{
			Dispose( true );
			GC.SuppressFinalize( this );
		}

		protected virtual void Dispose( bool disposing )
		{
			if( disposing )
				this.networkAvailabilityService.NetworkAvailabilityChanged -= NetworkExchangeService_NetworkAvailabilityChanged;
		}
		#endregion

		#region Public Properties
		public PhoneApplicationPage Page
		{
			get { return this.page; }
		}

		public bool NetworkAvailable
		{
			get { return networkAvailable; }
			private set
			{
				if( networkAvailable != value )
				{
					OnPropertyChanging( "NetworkAvailable", networkAvailable, value );
					networkAvailable = value;
					OnPropertyChanged( "NetworkAvailable" );
				}
			}
		}
		#endregion

		private void NetworkExchangeService_NetworkAvailabilityChanged( object sender, EventArgs e )
		{
			if( Deployment.Current.Dispatcher.CheckAccess() )
				this.NetworkAvailable = this.networkAvailabilityService.NetworkAvailable;
			else
				Deployment.Current.Dispatcher.BeginInvoke( () => this.NetworkAvailable = this.networkAvailabilityService.NetworkAvailable );
		}
	}
}