using System;
using System.Collections.Generic;
using System.Linq;

namespace Digillect.Mvvm.UI
{
	/// <summary>
	/// Provides infrastructure for page backed up with <see cref="Digillect.Mvvm.ViewModel"/>.
	/// </summary>
	/// <typeparam name="TViewModel">The type of the view model.</typeparam>
	[Windows.Foundation.Metadata.WebHostHidden]
	public class ViewModelPage<TViewModel> : Page
		where TViewModel : ViewModel
	{
		private TViewModel viewModel;

		#region Public Properties
		/// <summary>
		/// Gets the view model.
		/// </summary>
		public TViewModel ViewModel
		{
			get { return this.viewModel; }
		}
		#endregion

		#region Page lifecycle
		/// <summary>
		/// This method is called when page is visited for the very first time. You should perform
		/// initialization and create one-time initialized resources here.
		/// </summary>
		protected override void OnPageCreated( object parameter )
		{
			base.OnPageCreated( parameter );

			if( !Windows.ApplicationModel.DesignMode.DesignModeEnabled )
			{
				if( parameter == null || parameter is NavigationParameters )
				{
					InitialLoadData( (NavigationParameters) parameter );
				}
			}
		}

		protected override void OnPageDestroyed()
		{
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
			this.viewModel = CreateViewModel();

			return new ViewModelPageDataContext( this, this.viewModel );
		}
		#endregion

		#region Creates new view model
		/// <summary>
		/// This method is called to create appropriate view model for the page.
		/// </summary>
		/// <returns>Instance of view model.</returns>
		protected virtual TViewModel CreateViewModel()
		{
			return Container.Resolve<TViewModel>();
		}
		#endregion

		#region InitialLoadData
		/// <summary>
		/// This method is called to perform initial load of data when page is created for the first time
		/// or resurrected from thombstombing if no other state-saving scenario exists. Default implementation
		/// does nothing.
		/// </summary>
		protected virtual void InitialLoadData( NavigationParameters parameter )
		{
			Context.Values["Loaded"] = true;
		}
		#endregion
	}
}
