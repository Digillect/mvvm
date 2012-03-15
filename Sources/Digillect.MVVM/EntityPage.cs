using System;

namespace Digillect.Mvvm
{
	public class EntityPage<TId, TEntity, TViewModel> : ViewModelPage<TViewModel>
		where TId: IComparable<TId>, IEquatable<TId>
		where TEntity: XObject<TId>
		where TViewModel: EntityViewModel<TId, TEntity>, new()
	{
		private TId m_id;

		#region InitialLoadData
		protected override void InitialLoadData()
		{
			this.ViewModel.Load( m_id );
		}
		#endregion
		#region OnNavigatedTo
		protected override void OnNavigatedTo( System.Windows.Navigation.NavigationEventArgs e )
		{
			if( this.NavigationContext.QueryString.ContainsKey( "Id" ) )
			{
				m_id = (TId) Convert.ChangeType( NavigationContext.QueryString["Id"], typeof( TId ), null );
			}

			base.OnNavigatedTo( e );
		}
		#endregion

		#region Protected Properties
		public TId EntityId
		{
			get { return m_id; }
		}
		#endregion
	}
}
