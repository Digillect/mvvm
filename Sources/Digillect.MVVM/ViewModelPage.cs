using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;

namespace Digillect.Mvvm
{
	public class ViewModelPage<TViewModel> : PhoneApplicationPage, IViewModelPage
		where TViewModel: ViewModel, new()
	{
		private readonly TViewModel viewModel;

		#region Constructors/Disposer
		public ViewModelPage()
		{
			if( !IsInDesignMode )
			{
				viewModel = ViewModelFactory.GetViewModel<TViewModel>();
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
		protected override void OnNavigatedTo( System.Windows.Navigation.NavigationEventArgs e )
		{
			if( viewModel != null )
				viewModel.Activate();

			base.OnNavigatedTo( e );
		}

		protected override void OnNavigatedFrom( System.Windows.Navigation.NavigationEventArgs e )
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
			ViewModelFactory.ReleaseViewModel( viewModel );

			base.OnPageUnloaded();
		}
		#endregion

		protected override PageDataContext CreateDataContext()
		{
			return new ViewModelPageDataContext( this, viewModel );
		}

		protected virtual void InitialLoadData()
		{
		}

		#region IViewModelPage implementation
		ViewModel IViewModelPage.ViewModel
		{
			get { return viewModel; }
		}
		#endregion
	}
}
