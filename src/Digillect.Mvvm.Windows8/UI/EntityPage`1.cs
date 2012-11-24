using System;

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
		where TId: struct, IComparable<TId>, IEquatable<TId>
		where TEntity: XObject<TId>
		where TViewModel: EntityViewModel<TId, TEntity>
	{
		#region InitialLoadData
		/// <summary>
		/// Initials the load data.
		/// </summary>
		protected override async void InitialLoadData( NavigationParameters parameters )
		{
			await this.ViewModel.Load( parameters.Get<TId>( "value" ) );

			Context.Values["Loaded"] = true;
		}
		#endregion
	}
}
