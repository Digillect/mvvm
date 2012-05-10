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
	/// name <c>Id</c> that is used as entity id for view model. If that parameter is not found then <see cref="System.ArgumentException"/> will be thrown.</remarks>
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
		/// <param name="e">The <see cref="System.Windows.Navigation.NavigationEventArgs"/> instance containing the event data.</param>
		/// <exception cref="System.ArgumentException">when identifier can't be found in query string.</exception>
		protected override void OnNavigatedTo( System.Windows.Navigation.NavigationEventArgs e )
		{
			if( this.NavigationContext.QueryString.ContainsKey( "Id" ) )
			{
				this.entityId = (TId) Convert.ChangeType( NavigationContext.QueryString["Id"], typeof( TId ), null );
			}
			else
			{
				throw new ArgumentException( "Entity identifier is not passed in query string." );
			}

			base.OnNavigatedTo( e );
		}
		#endregion
	}
}
