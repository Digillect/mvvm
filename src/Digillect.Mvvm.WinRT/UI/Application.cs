using System;
using System.Collections.Generic;
using System.Linq;

using Windows.ApplicationModel;
using Windows.ApplicationModel.Activation;
using Windows.Storage;
using Windows.UI.Xaml;
using Windows.UI.Xaml.Controls;
using Windows.UI.Xaml.Navigation;

using Digillect.Mvvm.Services;

namespace Digillect.Mvvm.UI
{
	/// <summary>
	/// Base class for Windows Metro applications. Implements <see cref="Digillect.Mvvm.ILifetimeScopeProvider"/> and performs registration of services and components.
	/// </summary>
	[Windows.Foundation.Metadata.WebHostHidden]
	public abstract class Application : Windows.UI.Xaml.Application
	{
		/// <summary>
		/// Gets the application root frame.
		/// </summary>
		public Frame RootFrame { get; private set; }
		public IContainer Container { get; private set; }

		#region Constructors/Disposer
		/// <summary>
		/// Initializes a new instance of the <see cref="Application"/> class.
		/// </summary>
		public Application()
		{
			Container = CreateContainer();
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

		protected void SaveState()
		{
			SaveBreadcrumbs();
		}

		protected void RestoreStateOrGoToLandingPage( Type pageType )
		{
			if( !RestoreBreadcrumbs() )
				RootFrame.Navigate( pageType );
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
		/// <param name="e">The <see cref="System.Windows.Navigation.NavigationFailedEventArgs"/> instance containing the event data.</param>
		protected virtual void HandleNavigationFailed( NavigationFailedEventArgs e ) { }
		#endregion

		#region IoC/Services
		protected abstract IContainer CreateContainer();
		#endregion

		#region Breadcrumbing
		private readonly Stack<Breadcrumb> breadcrumbs = new Stack<Breadcrumb>();

		internal bool IsUnwinding { get; private set; }

		internal void PushBreadcrumb( Type type, NavigationParameters parameters = null )
		{
			this.breadcrumbs.Push( new Breadcrumb( type, parameters ) );
		}

		internal Breadcrumb PopBreadcrumb( Type pageType )
		{
			if( this.breadcrumbs.Count == 0 )
				return null;

			var bc = this.breadcrumbs.Peek();

			if( bc.Type == pageType )
			{
				this.breadcrumbs.Pop();

				return bc;
			}

			return null;
		}

		internal Breadcrumb PeekBreadcrumb( Type pageType )
		{
			if( this.breadcrumbs.Count == 0 )
				return null;

			var bc = this.breadcrumbs.Peek();

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
					using( var writer = new System.IO.BinaryWriter(stream) )
					{
						writer.Write( RootFrame.GetNavigationState() );
						writer.Write( this.breadcrumbs.Count );

						foreach( var breadcrumb in this.breadcrumbs )
						{
							writer.Write( breadcrumb.Type.AssemblyQualifiedName );
							writer.Write( breadcrumb.Parameters != null );

							if( breadcrumb.Parameters != null )
								breadcrumb.Parameters.WriteTo( writer );
						}

						writer.Flush();

						history = Convert.ToBase64String( stream.ToArray() );
					}
				}
			}
			catch
			{
			}

			ApplicationData.Current.RoamingSettings.Values["Breadcrumbs"] = history;
		}

		internal bool RestoreBreadcrumbs( bool clearOnSuccess = true )
		{
			if( !ApplicationData.Current.RoamingSettings.Values.ContainsKey( "Breadcrumbs" ) )
				return false;

			Stack<Breadcrumb> unwind = new Stack<Breadcrumb>();
			string state = null;

			try
			{
				var history = (string) ApplicationData.Current.RoamingSettings.Values["Breadcrumbs"];

				using( var stream = new System.IO.MemoryStream(Convert.FromBase64String( history )) )
				{
					using( var reader = new System.IO.BinaryReader(stream) )
					{
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
				}

				if( unwind.Count == 0 )
					return false;
			}
			catch
			{
				return false;
			}

			foreach( var bc in unwind )
				this.breadcrumbs.Push( bc );

			this.IsUnwinding = true;

			RootFrame.SetNavigationState( state );

			this.IsUnwinding = false;

			if( clearOnSuccess )
				ApplicationData.Current.RoamingSettings.Values["Breadcrumbs"] = null;

			return true;
		}
		#endregion
	}

}
