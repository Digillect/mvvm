using System;
using System.Windows;
using System.Windows.Navigation;

using Microsoft.Phone.Controls;

using Digillect.Mvvm.Services;

namespace Digillect.Mvvm.UI
{
	/// <summary>
	/// Base class for Windows Phone 7 applications.
	/// </summary>
	public abstract class PhoneApplication : Application
	{
		/// <summary>
		/// Gets the application root frame.
		/// </summary>
		public PhoneApplicationFrame RootFrame { get; private set; }
		/// <summary>
		/// Gets the IoC container.
		/// </summary>
		public IContainer Container { get; private set; }

		#region Constructors/Disposer
		/// <summary>
		/// Initializes a new instance of the <see cref="PhoneApplication"/> class.
		/// </summary>
		public PhoneApplication()
		{
			Container = CreateContainer();
			InitializePhoneApplication();
		}
		#endregion

		#region Phone application initialization
		private bool phoneApplicationInitialized;

		// Do not add any additional code to this method
		private void InitializePhoneApplication()
		{
			if( phoneApplicationInitialized )
				return;

			// Create the frame but don't set it as RootVisual yet; this allows the splash
			// screen to remain active until the application is ready to render.
			RootFrame = CreateRootFrame();
			RootFrame.Navigated += CompleteInitializePhoneApplication;

			// Handle navigation failures
			RootFrame.NavigationFailed += RootFrame_NavigationFailed;

			// Ensure we don't initialize again
			phoneApplicationInitialized = true;
		}

		private void CompleteInitializePhoneApplication( object sender, NavigationEventArgs e )
		{
			if( RootVisual != RootFrame )
				RootVisual = RootFrame;

			RootFrame.Navigated -= CompleteInitializePhoneApplication;
		}

		/// <summary>
		/// Creates application root frame. By default creates instance of <see cref="Microsoft.Phone.Controls.PhoneApplicationFrame"/>, override
		/// to create instance of other type.
		/// </summary>
		/// <returns>application frame.</returns>
		protected virtual PhoneApplicationFrame CreateRootFrame()
		{
			return new PhoneApplicationFrame();
		}
		#endregion

		#region Navigation
		private void RootFrame_NavigationFailed( object sender, NavigationFailedEventArgs e )
		{
			HandleNavigationFailed( e );
		}

		/// <summary>
		/// Executes when navigation has been failed. Override to provide your own handling.
		/// </summary>
		/// <param name="e">The <see cref="System.Windows.Navigation.NavigationFailedEventArgs"/> instance containing the event data.</param>
		protected virtual void HandleNavigationFailed( NavigationFailedEventArgs e ) { }
		#endregion

		#region IoC Support
		/// <summary>
		/// Creates implementation of <see cref="Digillect.Mvvm.Services.IContainer"/>.
		/// </summary>
		protected abstract IContainer CreateContainer();
		#endregion
	}
}
