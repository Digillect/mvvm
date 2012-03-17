using System;
using System.Net;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Documents;
using System.Windows.Ink;
using System.Windows.Navigation;
using System.Windows.Media;
using System.Windows.Media.Animation;
using System.Windows.Shapes;

using Microsoft.Phone.Controls;

using Autofac;

namespace Digillect.Mvvm
{
	public class PhoneApplication : Application, ILifetimeScopeProvider
	{
		public PhoneApplicationFrame RootFrame { get; private set; }
		public ILifetimeScope Scope { get; private set; }

		#region Constructors/Disposer
		public PhoneApplication()
		{
			InitializePhoneApplication();
			InitializeIoC();
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

		protected virtual PhoneApplicationFrame CreateRootFrame()
		{
			return new PhoneApplicationFrame();
		}
		#endregion
		
		#region IoC registration
		private void InitializeIoC()
		{
			var builder = new ContainerBuilder();

			RegisterServices( builder );

			var container = builder.Build();

			this.Scope = container;
		}

		protected virtual void RegisterServices( ContainerBuilder builder )
		{
			builder.RegisterModule<Configuration.WindowsPhoneModule>();
		}
		#endregion
		#region Navigation
		private void RootFrame_NavigationFailed( object sender, NavigationFailedEventArgs e )
		{
			HandleNavigationFailed( e );
		}

		protected virtual void HandleNavigationFailed( NavigationFailedEventArgs e ) { }
		#endregion
	}
}
