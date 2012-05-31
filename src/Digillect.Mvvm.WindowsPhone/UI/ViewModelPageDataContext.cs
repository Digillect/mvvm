using System;

using Digillect.Mvvm.Services;
using System.Diagnostics.Contracts;

namespace Digillect.Mvvm.UI
{
	/// <summary>
	/// Instances of this class are used by <see cref="Digillect.Mvvm.UI.ViewModelPage{TViewModel}"/> and descendants to provide data binding support.
	/// </summary>
	public class ViewModelPageDataContext : PageDataContext
	{
		#region Constructors/Disposer
		/// <summary>
		/// Initializes a new instance of the <see cref="ViewModelPageDataContext"/> class.
		/// </summary>
		/// <param name="page">The page used in this context.</param>
		/// <param name="viewModel">The view model used in this context.</param>
		public ViewModelPageDataContext( PhoneApplicationPage page, ViewModel viewModel )
			: base( page )
		{
			Contract.Requires( page != null );

			ViewModel = viewModel;
		}
		#endregion

		#region Public Properties
		/// <summary>
		/// Gets the view model.
		/// </summary>
		public ViewModel ViewModel { get; private set; }
		#endregion
	}
}
