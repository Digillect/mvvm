using System;
using System.Diagnostics.CodeAnalysis;
using System.Diagnostics.Contracts;
using System.Threading.Tasks;

namespace Digillect.Mvvm
{
	/// <summary>
	///     Entity-oriented view model. We assume that every entity (model/data object class) is uniquelly identified by some primitive type.
	/// </summary>
	/// <typeparam name="TId">The type of the entity identifier.</typeparam>
	/// <typeparam name="TEntity">The type of the entity.</typeparam>
	[Obsolete( "Use XKey-operated EntityViewModel." )]
	public abstract class EntityViewModel<TId, TEntity> : ViewModel
		where TId : IComparable<TId>, IEquatable<TId>
		where TEntity : class, IXIdentified<TId>
	{
		private const string EntityPart = "Entity";

		private TEntity _entity;

		#region Constructors/Disposer
		/// <summary>
		///     Initializes a new instance of the <see cref="EntityViewModel{TId,TEntity}" /> class.
		/// </summary>
		protected EntityViewModel()
		{
			RegisterPart( EntityPart, ( session, part ) => LoadEntity( (EntitySession<TId>) session ), ( session, part ) => ShouldLoadEntity( (EntitySession<TId>) session ) );
		}
		#endregion

		#region Public Properties
		/// <summary>
		///     Gets or sets the entity.
		/// </summary>
		/// <value>
		///     The entity.
		/// </value>
		public TEntity Entity
		{
			get { return _entity; }
			protected set { SetProperty( ref _entity, value, "Entity" ); }
		}
		#endregion

		#region Loaders
		/// <summary>
		///     Creates the session that loads everything.
		/// </summary>
		/// <param name="id">Entity identifier.</param>
		/// <returns>Session that (usually) loads everything, including entity using specified <paramref name="id"/>.</returns>
		[SuppressMessage( "Microsoft.Reliability", "CA2000:Dispose objects before losing scope", Justification = "Session can be used by caller." )]
		public EntitySession<TId> CreateSession( TId id )
		{
			return new EntitySession<TId>( id );
		}

		/// <summary>
		///     Creates the session that loads only the entity.
		/// </summary>
		/// <param name="id">Entity identifier.</param>
		/// <returns>Session that loads entity using specified <paramref name="id"/>.</returns>
		[SuppressMessage( "Microsoft.Reliability", "CA2000:Dispose objects before losing scope", Justification = "Session can be used by caller." )]
		public EntitySession<TId> CreateEntitySession( TId id )
		{
			return new EntitySession<TId>( id, EntityPart );
		}
		#endregion

		#region Parts Loaders
		/// <summary>
		///     Implement this method to load entity.
		/// </summary>
		/// <param name="session">The session.</param>
		/// <returns>
		///     <see cref="System.Threading.Tasks.Task" /> that can be awaited.
		/// </returns>
		protected abstract Task LoadEntity( EntitySession<TId> session );

		/// <summary>
		///     Determines if entity part should participate in session.
		/// </summary>
		/// <param name="session">The session.</param>
		/// <returns>
		///     <c>true</c> if loader should be executed, otherwise <c>false</c>.
		/// </returns>
		/// <exception cref="System.ArgumentNullException">
		///     If <paramref name="session" /> is null.
		/// </exception>
		protected virtual bool ShouldLoadEntity( EntitySession<TId> session )
		{
			if( session == null )
			{
				throw new ArgumentNullException( "session" );
			}

			Contract.EndContractBlock();

			return _entity == null || !Equals( (session).Id, _entity.Id );
		}
		#endregion
	}
}