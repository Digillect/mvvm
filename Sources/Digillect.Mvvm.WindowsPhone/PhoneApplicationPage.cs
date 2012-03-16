using System;
using System.Threading;
using System.Windows;
using System.Windows.Markup;

using Autofac;

namespace Digillect.Mvvm
{
	public class PhoneApplicationPage : Microsoft.Phone.Controls.PhoneApplicationPage, ILifetimeScopeProvider
	{
		private PageDataContext dataContext;
		private ILifetimeScope lifetimeScope;

		#region Constructor
		public PhoneApplicationPage()
		{
			this.Language = XmlLanguage.GetLanguage( Thread.CurrentThread.CurrentCulture.Name );

			this.Loaded += Page_Loaded;
			this.Unloaded += Page_Unloaded;
		}
		#endregion

		public ILifetimeScope Scope
		{
			get
			{
				if( lifetimeScope == null )
				{
					var scopeProvider = Application.Current as ILifetimeScopeProvider;

					lifetimeScope = scopeProvider.Scope.BeginLifetimeScope();
				}

				return lifetimeScope;
			}
		}

		#region Page Load/Unload events handling
		private void Page_Loaded( object sender, EventArgs e )
		{
			if( !IsInDesignMode )
				OnPageLoaded();
		}

		private void Page_Unloaded( object sender, EventArgs e )
		{
			if( !IsInDesignMode )
				OnPageUnloaded();
		}

		protected virtual void OnPageLoaded()
		{
			dataContext = CreateDataContext();
			this.DataContext = dataContext;

			PageDecorationService.Current.AddDecoration( this );
		}

		protected virtual void OnPageUnloaded()
		{
			PageDecorationService.Current.RemoveDecoration( this );

			if( lifetimeScope != null )
				lifetimeScope.Dispose();
		}

		protected virtual PageDataContext CreateDataContext()
		{
			var factory = Scope.Resolve<PageDataContext.Factory>();

			return factory( this );
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
