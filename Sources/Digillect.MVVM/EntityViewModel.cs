using System;
using System.Threading;
using System.Threading.Tasks;

using Digillect.Mvvm.Services;

namespace Digillect.Mvvm
{
	public abstract class EntityViewModel<TId, TEntity> : ViewModel
		where TId : IComparable<TId>, IEquatable<TId>
		where TEntity : XObject<TId>
	{
		private TEntity entity;

		#region Constructors/Disposer
		public EntityViewModel( IDataExchangeService dataExchangeService )
			: base( dataExchangeService )
		{
		}
		#endregion

		#region Public Properties
		public TEntity Entity
		{
			get
			{
				return entity;
			}

			protected set
			{
				if( entity != value )
				{
					OnPropertyChanging( "Entity", entity, value );
					entity = value;
					OnPropertyChanged( "Entity" );
				}
			}
		}
		#endregion

		#region Load
		public Task<Session> Load( TId id )
		{
			var session = new EntitySession<TId>( id );

			return Load( session );
		}

		public Task<Session> LoadEntity( TId id )
		{
			var session = new EntitySession<TId>( id, EntitySession<TId>.Entity );

			return Load( session );
		}

		protected override bool ShouldLoadSession( Session session )
		{
			if( !base.ShouldLoadSession( session ) )
				return false;

			var entitySession = (EntitySession<TId>) session;

			if( this.Entity != null && entitySession.IsEntity && object.Equals( entitySession.Id, Entity.Id ) )
				return false;

			return true;
		}

		protected override void LoadSession( Session session )
		{
			base.LoadSession( session );

			var entitySession = (EntitySession<TId>) session;

			if( entitySession.IsEntity )
				entitySession.Tasks.Add( LoadEntity( entitySession ) );
		}

		protected abstract Task LoadEntity( EntitySession<TId> session );
		#endregion
	}
}