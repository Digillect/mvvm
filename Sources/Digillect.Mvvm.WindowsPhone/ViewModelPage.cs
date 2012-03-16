using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;

using System.Windows;
using System.Windows.Navigation;

using Autofac;

namespace Digillect.Mvvm
{
	public class ViewModelPage<TViewModel> : PhoneApplicationPage
		where TViewModel : ViewModel, new()
	{
		private readonly TViewModel viewModel;

		#region Constructors/Disposer
		public ViewModelPage()
		{
			if( !IsInDesignMode )
			{
				viewModel = Scope.Resolve<TViewModel>();
			}
		}
		#endregion

		#region Public Properties
		public TViewModel ViewModel
		{
			get { return viewModel; }
		}
		#endregion

		#region OnNavigatedTo/OnNavigatedFrom
		protected override void OnNavigatedTo( NavigationEventArgs e )
		{
			if( viewModel != null )
				viewModel.Activate();

			base.OnNavigatedTo( e );
		}

		protected override void OnNavigatedFrom( NavigationEventArgs e )
		{
			if( viewModel != null )
				ViewModel.Deactivate();

			base.OnNavigatedFrom( e );
		}
		#endregion
		#region OnPageLoaded/OnPageUnloaded
		protected override void OnPageLoaded()
		{
			base.OnPageLoaded();

			if( !IsInDesignMode )
			{
				viewModel.Activate();
				InitialLoadData();
			}
		}

		protected override void OnPageUnloaded()
		{
			base.OnPageUnloaded();
		}
		#endregion

		protected override PageDataContext CreateDataContext()
		{
			var factory = Scope.Resolve<ViewModelPageDataContext.Factory>();

			return factory( this, viewModel );
		}

		protected virtual void InitialLoadData()
		{
		}
	}
}
