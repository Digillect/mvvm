using System;
using System.Diagnostics.Contracts;
using System.Windows;

using Digillect.Mvvm.Services;

namespace Digillect.Mvvm
{
	/// <summary>
	/// Instances of this class are used by MVVM infrastructure to support data binding.
	/// </summary>
	public class PageDataContext : ObservableObject, IDisposable
	{
		/// <summary>
		/// Factory to be used to create new instances.
		/// </summary>
		/// <param name="page">The page used in this context.</param>
		/// <returns></returns>
		public delegate PageDataContext Factory( PhoneApplicationPage page );

		private readonly PhoneApplicationPage page;
		private readonly INetworkAvailabilityService networkAvailabilityService;
		private bool networkAvailable;

		#region Constructors/Disposer
		/// <summary>
		/// Initializes a new instance of the <see cref="PageDataContext"/> class.
		/// </summary>
		/// <param name="page">The page used in this context.</param>
		/// <param name="networkAvailabilityService">The network availability service (provided by container).</param>
		public PageDataContext( PhoneApplicationPage page, INetworkAvailabilityService networkAvailabilityService )
		{
			Contract.Requires( page != null );
			Contract.Requires( networkAvailabilityService != null );

			this.page = page;
			this.networkAvailabilityService = networkAvailabilityService;
			this.networkAvailable = this.networkAvailabilityService.NetworkAvailable;

			this.networkAvailabilityService.NetworkAvailabilityChanged += NetworkExchangeService_NetworkAvailabilityChanged;
		}

		/// <summary>
		/// Releases unmanaged resources and performs other cleanup operations before the
		/// <see cref="PageDataContext"/> is reclaimed by garbage collection.
		/// </summary>
		~PageDataContext()
		{
			Dispose( false );
			GC.SuppressFinalize( this );
		}

		/// <summary>
		/// Performs application-defined tasks associated with freeing, releasing, or resetting unmanaged resources.
		/// </summary>
		public void Dispose()
		{
			Dispose( true );
			GC.SuppressFinalize( this );
		}

		/// <summary>
		/// Releases unmanaged and - optionally - managed resources
		/// </summary>
		/// <param name="disposing"><c>true</c> to release both managed and unmanaged resources; <c>false</c> to release only unmanaged resources.</param>
		protected virtual void Dispose( bool disposing )
		{
			if( disposing )
				this.networkAvailabilityService.NetworkAvailabilityChanged -= NetworkExchangeService_NetworkAvailabilityChanged;
		}
		#endregion

		#region Public Properties
		/// <summary>
		/// Gets the page.
		/// </summary>
		public PhoneApplicationPage Page
		{
			get { return this.page; }
		}

		/// <summary>
		/// Gets a value indicating whether network connection is available.
		/// </summary>
		/// <value>
		///   <c>true</c> if network connection is available; otherwise, <c>false</c>.
		/// </value>
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