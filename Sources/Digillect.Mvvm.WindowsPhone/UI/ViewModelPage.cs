using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;

using System.Windows;
using System.Windows.Navigation;

namespace Digillect.Mvvm.UI
{
	/// <summary>
	/// Provides infrastructure for page backed up with <see cref="Digillect.Mvvm.ViewModel"/>.
	/// </summary>
	/// <typeparam name="TViewModel">The type of the view model.</typeparam>
	public class ViewModelPage<TViewModel> : PhoneApplicationPage
		where TViewModel : ViewModel
	{
		private TViewModel viewModel;

		#region Public Properties
		/// <summary>
		/// Gets the view model.
		/// </summary>
		public TViewModel ViewModel
		{
			get { return viewModel; }
		}
		#endregion

		#region Page lifecycle
		/// <summary>
		/// This method is called when page is visited for the very first time. You should perform
		/// initialization and create one-time initialized resources here.
		/// </summary>
		protected override void OnPageCreated()
		{
			base.OnPageCreated();

			if( !IsInDesignMode )
			{
				this.viewModel = CurrentApplication.CreateViewModel<TViewModel>();
				InitialLoadData();
			}
		}

		/// <summary>
		/// This method is called when page navigated after application has been resurrected from thombstombed state.
		/// Use <see cref="Microsoft.Phone.Controls.PhoneApplicationPage.State"/> property to restore state.
		/// </summary>
		protected override void OnPageResurrected()
		{
			base.OnPageResurrected();

			if( !IsInDesignMode )
			{
				this.viewModel = CurrentApplication.CreateViewModel<TViewModel>();
				InitialLoadData();
			}
		}

		/// <summary>
		/// Creates data context to be set for the page. Override to create your own data context.
		/// </summary>
		/// <returns>
		/// Data context that will be set to <see cref="System.Windows.FrameworkElement.DataContext"/> property.
		/// </returns>
		protected override PageDataContext CreateDataContext()
		{
			return new ViewModelPageDataContext( this, this.viewModel, CurrentApplication.GetService<Digillect.Mvvm.Services.INetworkAvailabilityService>() );
		}
		#endregion

		#region InitialLoadData
		/// <summary>
		/// This method is called to perform initial load of data when page is created for the first time
		/// or resurrected from thombstombing if no other state-saving scenario exists. Default implementation
		/// does nothing.
		/// </summary>
		protected virtual void InitialLoadData()
		{
		}
		#endregion
	}
}
