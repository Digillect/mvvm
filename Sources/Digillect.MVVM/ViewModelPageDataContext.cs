using System;

namespace Digillect.MVVM
{
	public class ViewModelPageDataContext : PageDataContext
	{
		#region Constructors/Disposer
		public ViewModelPageDataContext( PhoneApplicationPage page, ViewModel viewModel )
			: base( page )
		{
			this.ViewModel = viewModel;
		}
		#endregion

		#region Public Properties
		public ViewModel ViewModel { get; private set; }
		#endregion
	}
}
