using System;
using System.Collections.Generic;
using System.Linq;

using Windows.ApplicationModel;
using Windows.ApplicationModel.Activation;
using Windows.Storage;
using Windows.UI.Xaml;
using Windows.UI.Xaml.Controls;
using Windows.UI.Xaml.Navigation;

using Autofac;

using Digillect.Mvvm.Services;

namespace Digillect.Mvvm.UI
{
	/// <summary>
	/// Base class for Windows Metro applications.
	/// </summary>
	[Windows.Foundation.Metadata.WebHostHidden]
	public abstract class Application : Windows.UI.Xaml.Application
	{
		/// <summary>
		/// Gets the application root frame.
		/// </summary>
		public Frame RootFrame { get; private set; }
		/// <summary>
		/// Gets the lifetime scope used to create components.
		/// </summary>
		/// <value>
		/// The scope.
		/// </value>
		public ILifetimeScope Scope { get; private set; }

		#region Constructors/Disposer
		/// <summary>
		/// Initializes a new instance of the <see cref="Application"/> class.
		/// </summary>
		[System.Diagnostics.CodeAnalysis.SuppressMessage( "Microsoft.Usage", "CA2214:DoNotCallOverridableMethodsInConstructors" )]
		protected Application()
		{
			InitializeIoC();

			Suspending += ( s, e ) => HandleSuspension( e );
		}

		#endregion

		#region Application Lifecycle handlers
		/// <summary>
		/// Invoked when the application is launched. Override this method to perform application initialization and to display initial content in the associated Window.
		/// </summary>
		/// <param name="args">Event data for the event.</param>
		protected override void OnLaunched( LaunchActivatedEventArgs args )
		{
			RootFrame = CreateRootFrame();

			HandleLaunch( args );

			RootFrame.NavigationFailed += RootFrame_NavigationFailed;

			Window.Current.Content = RootFrame;
			Window.Current.Activate();
		}

		/// <summary>
		/// Handles the suspension.
		/// </summary>
		/// <param name="eventArgs">The <see cref="SuspendingEventArgs" /> instance containing the event data.</param>
		protected virtual void HandleSuspension( SuspendingEventArgs eventArgs )
		{
		}

		/// <summary>
		/// Handles the launch.
		/// </summary>
		/// <param name="eventArgs">The <see cref="LaunchActivatedEventArgs" /> instance containing the event data.</param>
		protected virtual void HandleLaunch( LaunchActivatedEventArgs eventArgs )
		{
		}

		/// <summary>
		/// Saves the state.
		/// </summary>
		protected void SaveState()
		{
			SaveBreadcrumbs();
		}

		/// <summary>
		/// Restores the state or navigates to the landing page.
		/// </summary>
		/// <param name="pageType">Type of the landing page.</param>
		protected void RestoreStateOrGoToLandingPage( Type pageType )
		{
			if( !RestoreBreadcrumbs() )
			{
				RootFrame.Navigate( pageType );
			}
		}

		/// <summary>
		/// Creates application root frame. By default creates instance of <see cref="Windows.UI.Xaml.Controls.Frame"/>, override
		/// to create instance of other type.
		/// </summary>
		/// <returns>application frame.</returns>
		protected virtual Frame CreateRootFrame()
		{
			return new Frame();
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
		/// <param name="eventArgs">The <see cref="Windows.UI.Xaml.Navigation.NavigationFailedEventArgs"/> instance containing the event data.</param>
		protected virtual void HandleNavigationFailed( NavigationFailedEventArgs eventArgs )
		{
		}
		#endregion

		#region IoC/Services
		private void InitializeIoC()
		{
			var builder = new ContainerBuilder();

			RegisterServices( builder );

			Scope = builder.Build();
		}

		/// <summary>
		/// Called to registers services in container.
		/// </summary>
		/// <param name="builder">The builder.</param>
		protected virtual void RegisterServices( ContainerBuilder builder )
		{
			builder.RegisterModule<Windows8Module>();
		}
		#endregion

		#region Breadcrumbing
		private readonly Stack<Breadcrumb> _breadcrumbs = new Stack<Breadcrumb>();

		internal bool IsUnwinding { get; private set; }

		internal void PushBreadcrumb( Type type, NavigationParameters parameters = null )
		{
			_breadcrumbs.Push( new Breadcrumb( type, parameters ) );
		}

		internal Breadcrumb PopBreadcrumb( Type pageType )
		{
			if( _breadcrumbs.Count == 0 )
			{
				return null;
			}

			var bc = _breadcrumbs.Peek();

			if( bc.Type == pageType )
			{
				_breadcrumbs.Pop();

				return bc;
			}

			return null;
		}

		internal Breadcrumb PeekBreadcrumb( Type pageType )
		{
			if( _breadcrumbs.Count == 0 )
			{
				return null;
			}

			var bc = _breadcrumbs.Peek();

			if( bc.Type == pageType )
			{
				return bc;
			}

			return null;
		}

		internal void SaveBreadcrumbs()
		{
			string history = null;

			try
			{
				using( var stream = new System.IO.MemoryStream() )
				{
					var writer = new System.IO.BinaryWriter( stream );

					writer.Write( RootFrame.GetNavigationState() );
					writer.Write( _breadcrumbs.Count );

					foreach( var breadcrumb in _breadcrumbs )
					{
						writer.Write( breadcrumb.Type.AssemblyQualifiedName );
						writer.Write( breadcrumb.Parameters != null );

						if( breadcrumb.Parameters != null )
						{
							breadcrumb.Parameters.WriteTo( writer );
						}
					}

					writer.Flush();

					history = Convert.ToBase64String( stream.ToArray() );
				}
			}
			catch( System.IO.IOException )
			{
			}

			ApplicationData.Current.RoamingSettings.Values["Breadcrumbs"] = history;
		}

		internal bool RestoreBreadcrumbs( bool clearOnSuccess = true )
		{
			if( !ApplicationData.Current.RoamingSettings.Values.ContainsKey( "Breadcrumbs" ) )
			{
				return false;
			}

			Stack<Breadcrumb> unwind = new Stack<Breadcrumb>();
			string state = null;

			try
			{
				var history = (string) ApplicationData.Current.RoamingSettings.Values["Breadcrumbs"];

				using( var stream = new System.IO.MemoryStream(Convert.FromBase64String( history )) )
				{
					var reader = new System.IO.BinaryReader( stream );

					state = reader.ReadString();
					int count = reader.ReadInt32();

					while( count-- > 0 )
					{
						var pageTypeName = reader.ReadString();
						var hasParameters = reader.ReadBoolean();
						var pageType = Type.GetType( pageTypeName );
						NavigationParameters parameters = null;

						if( hasParameters )
						{
							parameters = new NavigationParameters();

							parameters.ReadFrom( reader );
						}

						var breadcrumb = new Breadcrumb( pageType, parameters );

						unwind.Push( breadcrumb );
					}
				}

				if( unwind.Count == 0 )
				{
					return false;
				}
			}
			catch( System.IO.IOException )
			{
				return false;
			}

			foreach( var bc in unwind )
			{
				_breadcrumbs.Push( bc );
			}

			IsUnwinding = true;

			RootFrame.SetNavigationState( state );

			IsUnwinding = false;

			if( clearOnSuccess )
			{
				ApplicationData.Current.RoamingSettings.Values["Breadcrumbs"] = null;
			}

			return true;
		}
		#endregion
	}
}
