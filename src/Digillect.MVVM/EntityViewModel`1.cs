﻿using System;
using System.Diagnostics.CodeAnalysis;
using System.Diagnostics.Contracts;
using System.Threading.Tasks;

namespace Digillect.Mvvm
{
	/// <summary>
	///     Entity-oriented view model. We assume that every entity (model/data object class) is uniquelly identified by some primitive type.
	/// </summary>
	/// <typeparam name="TEntity">The type of the entity.</typeparam>
	public abstract class EntityViewModel<TEntity> : ViewModel
		where TEntity : XObject
	{
		private const string EntityPart = "Entity";

		private TEntity _entity;

		#region Constructors/Disposer
		/// <summary>
		///     Initializes a new instance of the <see cref="EntityViewModel{TId,TEntity}" /> class.
		/// </summary>
		protected EntityViewModel()
		{
			RegisterPart( EntityPart, ( session, part ) => LoadEntity( (EntitySession) session ), ( session, part ) => ShouldLoadEntity( (EntitySession) session ) );
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
		///     Loads the entity with specified id and all other parts that can be needed by ViewModel logic.
		/// </summary>
		/// <param name="key">Entity key.</param>
		/// <returns>
		///     <see cref="System.Threading.Tasks.Task{T}" /> that can be awaited and will return
		///     <see
		///         cref="Digillect.Mvvm.Session" />
		///     .
		/// </returns>
		[SuppressMessage( "Microsoft.Reliability", "CA2000:Dispose objects before losing scope", Justification = "Session can be used by caller." )]
		public Task<Session> Load( XKey key )
		{
			var session = new EntitySession( key );

			return Load( session );
		}

		/// <summary>
		///     Loads the entity alone.
		/// </summary>
		/// <param name="key">Entity key.</param>
		/// <returns>
		///     <see cref="System.Threading.Tasks.Task{T}" /> that can be awaited.
		/// </returns>
		[SuppressMessage( "Microsoft.Reliability", "CA2000:Dispose objects before losing scope", Justification = "Session can be used by caller." )]
		public Task<Session> LoadEntity( XKey key )
		{
			var session = new EntitySession( key, EntityPart );

			return Load( session );
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
		protected abstract Task LoadEntity( EntitySession session );

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
		protected virtual bool ShouldLoadEntity( EntitySession session )
		{
			if( session == null )
			{
				throw new ArgumentNullException( "session" );
			}

			Contract.EndContractBlock();

			return _entity == null || !_entity.GetKey().Equals( session.Key );
		}
		#endregion
	}
}