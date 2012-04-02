﻿using System;
using System.Collections.Generic;

using Windows.ApplicationModel;
using Windows.ApplicationModel.Activation;
using Windows.Foundation;
using Windows.Foundation.Collections;
using Windows.UI.Xaml;
using Windows.UI.Xaml.Controls;
using Windows.UI.Xaml.Controls.Primitives;
using Windows.UI.Xaml.Data;
using Windows.UI.Xaml.Input;
using Windows.UI.Xaml.Media;
using Windows.UI.Xaml.Navigation;

namespace Digillect.Mvvm.UI
{
	/// <summary>
	/// Base class for Windows Metro applications. Implements <see cref="Digillect.Mvvm.ILifetimeScopeProvider"/> and performs registration of services and components.
	/// </summary>
	[Windows.Foundation.Metadata.WebHostHidden]
	public class Application : Windows.UI.Xaml.Application
	{
		/// <summary>
		/// Gets the application root frame.
		/// </summary>
		public Frame RootFrame { get; private set; }

		#region Constructors/Disposer
		/// <summary>
		/// Initializes a new instance of the <see cref="Application"/> class.
		/// </summary>
		public Application()
		{
			this.Suspending += ( s, e ) => HandleSuspension( e );
		}
		#endregion

		#region Application Lifecycle handlers
		protected override void OnLaunched( LaunchActivatedEventArgs args )
		{
			this.RootFrame = CreateRootFrame();

			HandleLaunch( args );

			Window.Current.Content = this.RootFrame;
			Window.Current.Activate();
		}

		protected virtual void HandleSuspension( SuspendingEventArgs e )
		{
		}

		protected virtual void HandleLaunch( LaunchActivatedEventArgs e )
		{
		}
		#endregion

		/// <summary>
		/// Creates application root frame. By default creates instance of <see cref="Windows.UI.Xaml.Controls.Frame"/>, override
		/// to create instance of other type.
		/// </summary>
		/// <returns>application frame.</returns>
		protected virtual Frame CreateRootFrame()
		{
			return new Frame();
		}
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

		public T GetService<T>() where T : class
		{
			return (T) GetService( typeof( T ) );
		}

		public virtual object GetService( Type serviceType )
		{
			throw new NotImplementedException();
		}

		/// <summary>
		/// Creates new instance of view model.
		/// </summary>
		/// <typeparam name="T">Type of view model to create.</typeparam>
		/// <returns>View model.</returns>
		public T CreateViewModel<T>() where T : ViewModel
		{
			return (T) CreateViewModel( typeof( T ) );
		}

		public virtual ViewModel CreateViewModel( Type viewModelType )
		{
			throw new NotImplementedException();
		}
	}
}