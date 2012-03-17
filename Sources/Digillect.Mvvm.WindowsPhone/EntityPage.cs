using System;

namespace Digillect.Mvvm
{
	public class EntityPage<TId, TEntity, TViewModel> : ViewModelPage<TViewModel>
		where TId: IComparable<TId>, IEquatable<TId>
		where TEntity: XObject<TId>
		where TViewModel: EntityViewModel<TId, TEntity>
	{
		private TId entityId;

		#region InitialLoadData
		protected override void InitialLoadData()
		{
			this.ViewModel.Load( this.entityId );
		}
		#endregion
		#region OnNavigatedTo
		protected override void OnNavigatedTo( System.Windows.Navigation.NavigationEventArgs e )
		{
			if( this.NavigationContext.QueryString.ContainsKey( "Id" ) )
			{
				this.entityId = (TId) Convert.ChangeType( NavigationContext.QueryString["Id"], typeof( TId ), null );
			}

			base.OnNavigatedTo( e );
		}
		#endregion
	}
}
