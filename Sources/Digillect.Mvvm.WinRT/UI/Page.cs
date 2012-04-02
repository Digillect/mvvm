using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using Windows.Foundation;
using Windows.Foundation.Collections;
using Windows.Graphics.Display;
using Windows.UI.ViewManagement;
using Windows.UI.Xaml;
using Windows.UI.Xaml.Controls;
using Windows.UI.Xaml.Controls.Primitives;
using Windows.UI.Xaml.Data;
using Windows.UI.Xaml.Input;
using Windows.UI.Xaml.Media;
using Windows.UI.Xaml.Navigation;

using Digillect.Mvvm.Services;
using System.ComponentModel;
using System.Runtime.CompilerServices;

namespace Digillect.Mvvm.UI
{
	/// <summary>
	/// Base for application pages.
	/// </summary>
	[Windows.Foundation.Metadata.WebHostHidden]
	public class Page : Windows.UI.Xaml.Controls.Page, INotifyPropertyChanged
	{
		private PageDataContext dataContext;
		private bool useFilledStateForNarrowWindow;
		private List<Control> layoutAwareControls;

		#region Constructor
		/// <summary>
		/// Initializes a new instance of the <see cref="Page"/> class.
		/// </summary>
		public Page()
		{
			if( Windows.ApplicationModel.DesignMode.DesignModeEnabled )
				return;

			// Map application view state to visual state for this page when it is part of the visual tree
			this.Loaded += this.StartLayoutUpdates;
			this.Unloaded += this.StopLayoutUpdates;
			//this.Language = XmlLanguage.GetLanguage( Thread.CurrentThread.CurrentCulture.Name );
		}
		#endregion

		#region Protected Properties
		protected Digillect.Mvvm.UI.Application CurrentApplication
		{
			get { return (Digillect.Mvvm.UI.Application) Application.Current; }
		}
		#endregion

		#region INotifyPropertyChanged support
		public event PropertyChangedEventHandler PropertyChanged;

		protected bool SetProperty<T>( ref T storage, T value, [CallerMemberName] string propertyName = null )
		{
			if( !object.Equals( storage, value ) )
				return false;

			storage = value;
			OnPropertyChanged( new PropertyChangedEventArgs( propertyName ) );

			return true;
		}

		protected virtual void OnPropertyChanged( PropertyChangedEventArgs e )
		{
			var handler = PropertyChanged;

			if( handler != null )
				handler( this, e );
		}
		#endregion

		#region Navigation handling
		/// <summary>
		/// Called when a page becomes the active page in a frame.
		/// </summary>
		/// <param name="e">An object that contains the event data.</param>
		protected override void OnNavigatedTo( NavigationEventArgs e )
		{
			base.OnNavigatedTo( e );

			if( this.dataContext == null )
			{
				OnPageCreated();

				this.dataContext = CreateDataContext();
				DataContext = this.dataContext;
			}
			else
			{
				OnPageAwaken();
			}
		}

		/// <summary>
		/// Called when a page is no longer the active page in a frame.
		/// </summary>
		/// <param name="e">An object that contains the event data.</param>
		protected override void OnNavigatedFrom( NavigationEventArgs e )
		{
			if( e.NavigationMode == NavigationMode.Back )
			{
				OnPageDestroyed();

				if( this.dataContext != null )
				{
					this.dataContext.Dispose();
					this.dataContext = null;
				}
			}
			else
			{
				OnPageAsleep();
			}

			base.OnNavigatedFrom( e );
		}
		#endregion

		#region Page Lifecycle handlers
		/// <summary>
		/// Creates data context to be set for the page. Override to create your own data context.
		/// </summary>
		/// <returns>Data context that will be set to <see cref="DataContext"/> property.</returns>
		protected virtual PageDataContext CreateDataContext()
		{
			return new PageDataContext( this, CurrentApplication.GetService<INetworkAvailabilityService>() );
		}

		/// <summary>
		/// This method is called when page is visited for the very first time. You should perform
		/// initialization and create one-time initialized resources here.
		/// </summary>
		protected virtual void OnPageCreated()
		{
		}

		/// <summary>
		/// This method is called when page is returned from being Dormant. All resources are preserved,
		/// so most of the time you should just ignore this event.
		/// </summary>
		protected virtual void OnPageAwaken()
		{
		}

		/// <summary>
		/// This method is called when navigation outside of the page occures.
		/// </summary>
		protected virtual void OnPageAsleep()
		{
		}

		/// <summary>
		/// This method is called when page is being destroyed, usually after user presses Back key.
		/// </summary>
		protected virtual void OnPageDestroyed()
		{
		}
		#endregion

		#region Layout
		/// <summary>
		/// Gets or sets a value indicating whether visual states can be a loose interpretation
		/// of the actual application view state.  This is often convenient when a page layout
		/// is space constrained.
		/// </summary>
		/// <remarks>
		/// The default value of false indicates that the visual state is identical to the view
		/// state, meaning that Filled is only used when another application is snapped.  When
		/// set to true FullScreenLandscape is used to indicate that at least 1366 virtual
		/// pixels of horizontal real estate are available - even if another application is
		/// snapped - and Filled indicates a lesser width, even if no other application is
		/// snapped.  On a smaller display such as a 1024x768 panel this will result in the
		/// visual state Filled whenever the device is in landscape orientation.
		/// </remarks>
		public bool UseFilledStateForNarrowWindow
		{
			get { return this.useFilledStateForNarrowWindow; }
			set
			{
				this.useFilledStateForNarrowWindow = value;

				this.InvalidateVisualState();
			}
		}

		/// <summary>
		/// Invoked as an event handler to navigate backward in the page's associated
		/// <see cref="Frame"/> until it reaches the top of the navigation stack.
		/// </summary>
		/// <param name="sender">Instance that triggered the event.</param>
		/// <param name="e">Event data describing the conditions that led to the event.</param>
		protected virtual void GoHome( object sender, RoutedEventArgs e )
		{
			// Use the navigation frame to return to the topmost page
			if( this.Frame != null )
			{
				while( this.Frame.CanGoBack )
					this.Frame.GoBack();
			}
		}

		/// <summary>
		/// Invoked as an event handler to navigate backward in the page's associated
		/// <see cref="Frame"/> to go back one step on the navigation stack.
		/// </summary>
		/// <param name="sender">Instance that triggered the event.</param>
		/// <param name="e">Event data describing the conditions that led to the
		/// event.</param>
		protected virtual void GoBack( object sender, RoutedEventArgs e )
		{
			// Use the navigation frame to return to the previous page
			if( this.Frame != null && this.Frame.CanGoBack )
				this.Frame.GoBack();
		}

		/// <summary>
		/// Invoked as an event handler, typically on the <see cref="Loaded"/> event of a
		/// <see cref="Control"/> within the page, to indicate that the sender should start
		/// receiving visual state management changes that correspond to application view state
		/// changes.
		/// </summary>
		/// <param name="sender">Instance of <see cref="Control"/> that supports visual state
		/// management corresponding to view states.</param>
		/// <param name="e">Event data that describes how the request was made.</param>
		/// <remarks>The current view state will immediately be used to set the corresponding
		/// visual state when layout updates are requested.  A corresponding
		/// <see cref="Unloaded"/> event handler connected to <see cref="StopLayoutUpdates"/>
		/// is strongly encouraged.  Instances of <see cref="LayoutAwarePage"/> automatically
		/// invoke these handlers in their Loaded and Unloaded events.</remarks>
		/// <seealso cref="DetermineVisualState"/>
		/// <seealso cref="InvalidateVisualState"/>
		public void StartLayoutUpdates( object sender, RoutedEventArgs e )
		{
			var control = sender as Control;
			
			if( control == null )
				return;

			if( this.layoutAwareControls == null )
			{
				// Start listening to view state changes when there are controls interested in updates
				ApplicationView.GetForCurrentView().ViewStateChanged += this.ViewStateChanged;
				Window.Current.SizeChanged += this.WindowSizeChanged;
				this.layoutAwareControls = new List<Control>();
			}

			this.layoutAwareControls.Add( control );

			// Set the initial visual state of the control
			VisualStateManager.GoToState( control, DetermineVisualState( ApplicationView.Value ), false );
		}

		private void ViewStateChanged( ApplicationView sender, ApplicationViewStateChangedEventArgs e )
		{
			this.InvalidateVisualState( e.ViewState );
		}

		private void WindowSizeChanged( object sender, Windows.UI.Core.WindowSizeChangedEventArgs e )
		{
			if( this.useFilledStateForNarrowWindow )
				InvalidateVisualState();
		}

		/// <summary>
		/// Invoked as an event handler, typically on the <see cref="Unloaded"/> event of a
		/// <see cref="Control"/>, to indicate that the sender should start receiving visual
		/// state management changes that correspond to application view state changes.
		/// </summary>
		/// <param name="sender">Instance of <see cref="Control"/> that supports visual state
		/// management corresponding to view states.</param>
		/// <param name="e">Event data that describes how the request was made.</param>
		/// <remarks>The current view state will immediately be used to set the corresponding
		/// visual state when layout updates are requested.</remarks>
		/// <seealso cref="StartLayoutUpdates"/>
		public void StopLayoutUpdates( object sender, RoutedEventArgs e )
		{
			var control = sender as Control;
			
			if( control == null || this.layoutAwareControls == null )
				return;

			this.layoutAwareControls.Remove( control );

			if( this.layoutAwareControls.Count == 0 )
			{
				// Stop listening to view state changes when no controls are interested in updates
				this.layoutAwareControls = null;

				ApplicationView.GetForCurrentView().ViewStateChanged -= this.ViewStateChanged;
				Window.Current.SizeChanged -= this.WindowSizeChanged;
			}
		}

		/// <summary>
		/// Translates <see cref="ApplicationViewState"/> values into strings for visual state
		/// management within the page.  The default implementation uses the names of enum values.
		/// Subclasses may override this method to control the mapping scheme used.
		/// </summary>
		/// <param name="viewState">View state for which a visual state is desired.</param>
		/// <returns>Visual state name used to drive the
		/// <see cref="VisualStateManager"/></returns>
		/// <seealso cref="InvalidateVisualState"/>
		protected virtual string DetermineVisualState( ApplicationViewState viewState )
		{
			if( this.useFilledStateForNarrowWindow &&
				(viewState == ApplicationViewState.Filled ||
				viewState == ApplicationViewState.FullScreenLandscape) )
			{
				// Allow pages to request that the Filled state be used only for landscape layouts narrower
				// than 1366 virtual pixels
				var windowWidth = Window.Current.Bounds.Width;

				viewState = windowWidth >= 1366 ? ApplicationViewState.FullScreenLandscape : ApplicationViewState.Filled;
			}

			return viewState.ToString();
		}

		/// <summary>
		/// Updates all controls that are listening for visual state changes with the correct
		/// visual state.
		/// </summary>
		/// <remarks>
		/// Typically used in conjunction with overriding <see cref="DetermineVisualState"/> to
		/// signal that a different value may be returned even though the view state has not
		/// changed.
		/// </remarks>
		/// <param name="viewState">The desired view state, or null if the current view state
		/// should be used.</param>
		public void InvalidateVisualState( ApplicationViewState? viewState = null )
		{
			if( this.layoutAwareControls != null )
			{
				string visualState = DetermineVisualState( viewState == null ? ApplicationView.Value : viewState.Value );

				foreach( var layoutAwareControl in this.layoutAwareControls )
				{
					VisualStateManager.GoToState( layoutAwareControl, visualState, false );
				}
			}
		}
		#endregion
	}
}
