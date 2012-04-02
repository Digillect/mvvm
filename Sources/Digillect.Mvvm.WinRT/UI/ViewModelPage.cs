using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Digillect.Mvvm.UI
{
	public class ViewModelPage : Page
	{
		private ViewModel viewModel;

		#region Public Properties
		public ViewModel ViewModel
		{
			get { return this.viewModel; }
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

			if( !Windows.ApplicationModel.DesignMode.DesignModeEnabled )
			{
				this.viewModel = CreateViewModel();
				InitialLoadData();
			}
		}

		protected override void OnPageDestroyed()
		{
			if( this.viewModel is IDisposable )
				((IDisposable) this.viewModel).Dispose();

			this.viewModel = null;
			
			base.OnPageDestroyed();
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

		#region Creates new view model
		/// <summary>
		/// This method is called to create appropriate view model for the page.
		/// </summary>
		/// <returns>Instance of view model.</returns>
		protected virtual ViewModel CreateViewModel()
		{
			throw new NotImplementedException();
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
			throw new NotImplementedException();
		}
		#endregion
	}
}
