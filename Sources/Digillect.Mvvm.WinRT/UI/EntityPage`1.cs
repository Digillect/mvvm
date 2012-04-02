using System;
using Windows.UI.Xaml.Navigation;

namespace Digillect.Mvvm.UI
{
	/// <summary>
	/// Provides infrastructure for page backed up with <see cref="Digillect.Mvvm.EntityViewModel{TId,TModel}"/>.
	/// </summary>
	/// <typeparam name="TId">The type of the entity identifier.</typeparam>
	/// <typeparam name="TEntity">The type of the entity.</typeparam>
	/// <typeparam name="TViewModel">The type of the view model.</typeparam>
	/// <remarks>Instance of this class performs lookup of the query string upon navigation to find and extract parameter with
	/// name <code>Id</code> that is used as entity id for view model. If that parameter is not found then <see cref="System.ArgumentException"/> will be thrown.</remarks>
	[Windows.Foundation.Metadata.WebHostHidden]
	public class EntityPage<TId, TEntity, TViewModel> : ViewModelPage<TViewModel>
		where TId: IComparable<TId>, IEquatable<TId>
		where TEntity: XObject<TId>
		where TViewModel: EntityViewModel<TId, TEntity>
	{
		private TId entityId;

		#region InitialLoadData
		/// <summary>
		/// Initials the load data.
		/// </summary>
		protected override void InitialLoadData()
		{
			this.ViewModel.Load( this.entityId );
		}
		#endregion
		#region OnNavigatedTo
		/// <summary>
		/// Raises the <see cref="E:NavigatedTo"/> event. Used to extract entity identifier from query string.
		/// </summary>
		/// <param name="e">The <see cref="NavigationEventArgs"/> instance containing the event data.</param>
		/// <exception cref="System.ArgumentException">when identifier can't be found in query string.</exception>
		protected override void OnNavigatedTo( NavigationEventArgs e )
		{
			this.entityId = (TId) e.Parameter;

			base.OnNavigatedTo( e );
		}
		#endregion
	}
}
