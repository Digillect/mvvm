using System;
using System.Threading;
using System.Windows;
using System.Windows.Markup;

using Autofac;

using Digillect.Mvvm.Services;

namespace Digillect.Mvvm
{
	public class PhoneApplicationPage : Microsoft.Phone.Controls.PhoneApplicationPage, ILifetimeScopeProvider
	{
		private const string RessurectionMark = "__mark$mark__";

		private ILifetimeScope lifetimeScope;

		#region Constructor
		/// <summary>
		/// Constructs new instance of the page and sets Page's <see cref="Language"/> to current culture.
		/// </summary>
		public PhoneApplicationPage()
		{
			this.Language = XmlLanguage.GetLanguage( Thread.CurrentThread.CurrentCulture.Name );
		}
		#endregion

		#region Scope
		/// <summary>
		/// Gets IoC lifetime scope that can be used to resolve components and services.
		/// </summary>
		public ILifetimeScope Scope
		{
			get { return lifetimeScope; }
		}
		#endregion

		#region Navigation handling
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
		/// Creates data context to be set for the page
		/// </summary>
		/// <returns>PageDataContext that will be set to the page's <see cref="DataContext"/> property.</returns>
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
		/// Use <see cref="State"/> property to restore state.
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
