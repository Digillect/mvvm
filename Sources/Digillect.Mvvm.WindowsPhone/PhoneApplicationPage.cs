using System;
using System.Threading;
using System.Windows;
using System.Windows.Markup;

using Autofac;

using Digillect.Mvvm.Services;

namespace Digillect.Mvvm
{
	/// <summary>
	/// Base for application pages.
	/// </summary>
	public class PhoneApplicationPage : Microsoft.Phone.Controls.PhoneApplicationPage, ILifetimeScopeProvider
	{
		private const string RessurectionMark = "__mark$mark__";

		private ILifetimeScope lifetimeScope;

		#region Constructor
		/// <summary>
		/// Initializes a new instance of the <see cref="PhoneApplicationPage"/> class.
		/// </summary>
		public PhoneApplicationPage()
		{
			this.Language = XmlLanguage.GetLanguage( Thread.CurrentThread.CurrentCulture.Name );
		}
		#endregion

		#region Scope
		/// <summary>
		/// Gets the scope that can be used to resolve services and components.
		/// </summary>
		public ILifetimeScope Scope
		{
			get { return lifetimeScope; }
		}
		#endregion

		#region Navigation handling
		/// <summary>
		/// Called when a page becomes the active page in a frame.
		/// </summary>
		/// <param name="e">An object that contains the event data.</param>
		protected override void OnNavigatedTo( System.Windows.Navigation.NavigationEventArgs e )
		{
			base.OnNavigatedTo( e );

			if( this.lifetimeScope == null )
			{
				this.lifetimeScope = (Application.Current as ILifetimeScopeProvider).Scope.BeginLifetimeScope();

				if( State.ContainsKey( RessurectionMark ) )
					OnPageResurrected();
				else
					OnPageCreated();

				this.DataContext = CreateDataContext();

				Scope.Resolve<IPageDecorationService>().AddDecoration( this );
			}
		}

		/// <summary>
		/// Called when a page is no longer the active page in a frame.
		/// </summary>
		/// <param name="e">An object that contains the event data.</param>
		protected override void OnNavigatedFrom( System.Windows.Navigation.NavigationEventArgs e )
		{
			if( e.NavigationMode == System.Windows.Navigation.NavigationMode.Back )
			{
				OnPageDestroyed();

				if( this.lifetimeScope != null )
				{
					this.lifetimeScope.Dispose();
					this.lifetimeScope = null;
				}

				Scope.Resolve<IPageDecorationService>().RemoveDecoration( this );
			}
			else
			{
				State[RessurectionMark] = true;
				OnPageAsleep();
			}

			base.OnNavigatedFrom( e );
		}
		#endregion

		#region Page Lifecycle handlers
		/// <summary>
		/// Creates data context to be set for the page. Override to create your own data context.
		/// </summary>
		/// <returns>Data context that will be set to <see cref="System.Windows.FrameworkElement.DataContext"/> property.</returns>
		protected virtual PageDataContext CreateDataContext()
		{
			var factory = Scope.Resolve<PageDataContext.Factory>();

			return factory( this );
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
		/// This method is called when page navigated after application has been resurrected from thombstombed state.
		/// Use <see cref="Microsoft.Phone.Controls.PhoneApplicationPage.State"/> property to restore state.
		/// </summary>
		protected virtual void OnPageResurrected()
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

		#region IsInDesignMode
		private static bool? isInDesignMode;

		/// <summary>
		/// Gets a value indicating whether this page is in design mode.
		/// </summary>
		/// <value>
		/// 	<c>true</c> if this page is in design mode; otherwise, <c>false</c>.
		/// </value>
		public static bool IsInDesignMode
		{
			get
			{
				if( !isInDesignMode.HasValue )
					isInDesignMode = System.ComponentModel.DesignerProperties.IsInDesignTool;

				return isInDesignMode.Value;
			}
		}
		#endregion
	}
}
