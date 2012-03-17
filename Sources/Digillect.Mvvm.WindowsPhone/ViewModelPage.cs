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
		where TViewModel : ViewModel
	{
		private TViewModel viewModel;

		#region Public Properties
		/// <summary>
		/// Gets Page's ViewModel
		/// </summary>
		public TViewModel ViewModel
		{
			get { return viewModel; }
		}
		#endregion

		#region Page lifecycle
		protected override void OnPageCreated()
		{
			base.OnPageCreated();

			if( !IsInDesignMode )
			{
				this.viewModel = Scope.Resolve<TViewModel>();
				InitialLoadData();
			}
		}

		protected override void OnPageResurrected()
		{
			base.OnPageResurrected();

			if( !IsInDesignMode )
			{
				this.viewModel = Scope.Resolve<TViewModel>();
				InitialLoadData();
			}
		}

		protected override PageDataContext CreateDataContext()
		{
			var factory = Scope.Resolve<ViewModelPageDataContext.Factory>();

			return factory( this, viewModel );
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
