using System;
using System.Threading;
using System.Threading.Tasks;

using Digillect.Mvvm.Services;

namespace Digillect.Mvvm
{
	/// <summary>
	/// Entity-oriented view model. We assume that every entity (model/data object class) is uniquelly identified by some primitive type.
	/// </summary>
	/// <typeparam name="TId">The type of the entity identifier.</typeparam>
	/// <typeparam name="TEntity">The type of the entity.</typeparam>
	public abstract class EntityViewModel<TId, TEntity> : ViewModel
		where TId : IComparable<TId>, IEquatable<TId>
		where TEntity : XObject<TId>
	{
		private TEntity entity;

		#region Public Properties
		/// <summary>
		/// Gets or sets the entity.
		/// </summary>
		/// <value>
		/// The entity.
		/// </value>
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
		/// <summary>
		/// Loads the entity with specified id and all other parts that can be needed by ViewModel logic.
		/// </summary>
		/// <param name="id">Entity identifier.</param>
		/// <returns><see cref="System.Threading.Tasks.Task{T}"/> that can be awaited and will return <see cref="Digillect.Mvvm.Session"/>.</returns>
		public Task<Session> Load( TId id )
		{
			var session = new EntitySession<TId>( id );

			return Load( session );
		}

		/// <summary>
		/// Loads the entity alone.
		/// </summary>
		/// <param name="id">Entity identifier.</param>
		/// <returns><see cref="System.Threading.Tasks.Task{T}"/> that can be awaited.</returns>
		public Task<Session> LoadEntity( TId id )
		{
			var session = new EntitySession<TId>( id, EntitySession<TId>.Entity );

			return Load( session );
		}

		/// <summary>
		/// If entity loading is involved in this session ensures that entity with specified id
		/// is not yet loaded.
		/// </summary>
		/// <param name="session">The session to check.</param>
		/// <returns>
		///   <c>true</c> if view model should proceed with loading session; otherwise, <c>false</c>.
		/// </returns>
		protected override bool ShouldLoadSession( Session session )
		{
			if( !base.ShouldLoadSession( session ) )
				return false;

			var entitySession = (EntitySession<TId>) session;

			if( this.Entity != null && entitySession.IsEntity && object.Equals( entitySession.Id, Entity.Id ) )
				return false;

			return true;
		}

		/// <summary>
		/// If entity loading is involved in this session creates <see cref="System.Threading.Tasks.Task{T}"/> to load entity
		/// and adds it to session.
		/// </summary>
		/// <param name="session">The session.</param>
		protected override void LoadSession( Session session )
		{
			base.LoadSession( session );

			var entitySession = (EntitySession<TId>) session;

			if( entitySession.IsEntity )
				entitySession.Tasks.Add( LoadEntity( entitySession ) );
		}

		/// <summary>
		/// Implement this method to load entity.
		/// </summary>
		/// <param name="session">The session.</param>
		/// <returns><see cref="System.Threading.Tasks.Task"/> that can be awaited.</returns>
		protected abstract Task LoadEntity( EntitySession<TId> session );
		#endregion
	}
}