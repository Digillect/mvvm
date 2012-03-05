using System;
using System.Net;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Documents;
using System.Windows.Ink;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Animation;
using System.Windows.Shapes;

namespace Digillect.MVVM
{
	public class PageDataContext : ObservableObject, IDisposable
	{
		private readonly PhoneApplicationPage page;
		private bool networkAvailable;

		#region Constructors/Disposer
		public PageDataContext( PhoneApplicationPage page )
		{
			this.page = page;
			this.networkAvailable = NetworkExchangeService.Current.NetworkAvailable;

			NetworkExchangeService.Current.NetworkAvailabilityChanged += NetworkExchangeService_NetworkAvailabilityChanged;
		}

		public void Dispose()
		{
			Dispose( true );
			GC.SuppressFinalize( this );
		}

		protected virtual void Dispose( bool disposing )
		{
			if( disposing )
				NetworkExchangeService.Current.NetworkAvailabilityChanged -= NetworkExchangeService_NetworkAvailabilityChanged;
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
				this.NetworkAvailable = NetworkExchangeService.Current.NetworkAvailable;
			else
				Deployment.Current.Dispatcher.BeginInvoke( () => this.NetworkAvailable = NetworkExchangeService.Current.NetworkAvailable );
		}
	}
}