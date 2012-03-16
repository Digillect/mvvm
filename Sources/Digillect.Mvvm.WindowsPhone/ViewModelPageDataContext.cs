using System;

using Digillect.Mvvm.Services;

namespace Digillect.Mvvm
{
	public class ViewModelPageDataContext : PageDataContext
	{
		public new delegate ViewModelPageDataContext Factory( PhoneApplicationPage page, ViewModel viewModel );

		#region Constructors/Disposer
		public ViewModelPageDataContext( PhoneApplicationPage page, ViewModel viewModel, INetworkAvailabilityService networkAvailabilityService )
			: base( page, networkAvailabilityService )
		{
			this.ViewModel = viewModel;
		}
		#endregion

		#region Public Properties
		public ViewModel ViewModel { get; private set; }
		#endregion
	}
}
