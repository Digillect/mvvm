using System;

using Digillect.Mvvm.Services;

namespace Digillect.Mvvm.UI
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
		public delegate PageDataContext Factory( Page page );

		private readonly Page page;
		private readonly INetworkAvailabilityService networkAvailabilityService;
		private bool networkAvailable;

		#region Constructors/Disposer
		/// <summary>
		/// Initializes a new instance of the <see cref="PageDataContext"/> class.
		/// </summary>
		/// <param name="page">The page used in this context.</param>
		/// <param name="networkAvailabilityService">The network availability service (provided by container).</param>
		public PageDataContext( Page page, INetworkAvailabilityService networkAvailabilityService )
		{
			this.page = page;
			this.networkAvailabilityService = networkAvailabilityService;

			if( this.networkAvailabilityService != null )
			{
				this.networkAvailable = this.networkAvailabilityService.NetworkAvailable;

				this.networkAvailabilityService.NetworkAvailabilityChanged += NetworkExchangeService_NetworkAvailabilityChanged;
			}
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
			{
				if( this.networkAvailabilityService != null )
					this.networkAvailabilityService.NetworkAvailabilityChanged -= NetworkExchangeService_NetworkAvailabilityChanged;
			}
		}
		#endregion

		#region Public Properties
		/// <summary>
		/// Gets the page.
		/// </summary>
		public Page Page
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
			private set { SetProperty( ref this.networkAvailable, value ); }
		}
		#endregion

		private void NetworkExchangeService_NetworkAvailabilityChanged( object sender, EventArgs e )
		{
			if( this.page.Dispatcher.HasThreadAccess )
				this.NetworkAvailable = this.networkAvailabilityService.NetworkAvailable;
			else
				this.page.Dispatcher.RunAsync( Windows.UI.Core.CoreDispatcherPriority.Normal, () => this.NetworkAvailable = this.networkAvailabilityService.NetworkAvailable );
		}
	}
}