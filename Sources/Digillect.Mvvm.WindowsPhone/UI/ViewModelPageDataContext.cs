using System;

using Digillect.Mvvm.Services;

namespace Digillect.Mvvm.UI
{
	/// <summary>
	/// Instances of this class are used by <see cref="Digillect.Mvvm.UI.ViewModelPage{TViewModel}"/> and descendants to provide data binding support.
	/// </summary>
	public class ViewModelPageDataContext : PageDataContext
	{
		/// <summary>
		/// Factory that is used to create new instances.
		/// </summary>
		/// <param name="page">The page used in this context.</param>
		/// <param name="viewModel">The view model used in this context.</param>
		/// <returns></returns>
		public new delegate ViewModelPageDataContext Factory( PhoneApplicationPage page, ViewModel viewModel );

		#region Constructors/Disposer
		/// <summary>
		/// Initializes a new instance of the <see cref="ViewModelPageDataContext"/> class.
		/// </summary>
		/// <param name="page">The page used in this context.</param>
		/// <param name="viewModel">The view model used in this context.</param>
		/// <param name="networkAvailabilityService">The network availability service (provided by container).</param>
		public ViewModelPageDataContext( PhoneApplicationPage page, ViewModel viewModel, INetworkAvailabilityService networkAvailabilityService )
			: base( page, networkAvailabilityService )
		{
			this.ViewModel = viewModel;
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
