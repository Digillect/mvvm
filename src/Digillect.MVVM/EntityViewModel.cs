﻿using System;
using System.Collections.Generic;
using System.Diagnostics.Contracts;
using System.Linq;
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
		where TEntity : class, IXIdentified<TId>
	{
		private const string EntityPart = "Entity";

		private TEntity _entity;

		#region Constructors/Disposer
		/// <summary>
		/// Initializes a new instance of the <see cref="EntityViewModel{TId,TEntity}" /> class.
		/// </summary>
		protected EntityViewModel()
		{
			RegisterPart( EntityPart, (session, part) => LoadEntity( (EntitySession<TId>) session ), (session, part) => ShouldLoadEntity( (EntitySession<TId>) session ) );
		}
		#endregion

		#region Public Properties
		/// <summary>
		/// Gets or sets the entity.
		/// </summary>
		/// <value>
		/// The entity.
		/// </value>
		public TEntity Entity
		{
			get { return _entity; }
			protected set
			{
				SetProperty( ref _entity, value, "Entity" );
			}
		}
		#endregion

		#region Load
		/// <summary>
		/// Loads the entity with specified id and all other parts that can be needed by ViewModel logic.
		/// </summary>
		/// <param name="id">Entity identifier.</param>
		/// <returns><see cref="System.Threading.Tasks.Task{T}"/> that can be awaited and will return <see cref="Digillect.Mvvm.Session"/>.</returns>
		[System.Diagnostics.CodeAnalysis.SuppressMessage( "Microsoft.Reliability", "CA2000:Dispose objects before losing scope", Justification = "Session can be used by caller." )]
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
		[System.Diagnostics.CodeAnalysis.SuppressMessage( "Microsoft.Reliability", "CA2000:Dispose objects before losing scope", Justification = "Session can be used by caller." )]
		public Task<Session> LoadEntity( TId id )
		{
			var session = new EntitySession<TId>( id, EntityPart );

			return Load( session );
		}

		/// <summary>
		/// Implement this method to load entity.
		/// </summary>
		/// <param name="session">The session.</param>
		/// <returns><see cref="System.Threading.Tasks.Task"/> that can be awaited.</returns>
		protected abstract Task LoadEntity( EntitySession<TId> session );
		/// <summary>
		/// Determines if entity part should participate in session.
		/// </summary>
		/// <param name="session">The session.</param>
		/// <returns><c>true</c> if loader should be executed, otherwise <c>false</c>.</returns>
		/// <exception cref="System.ArgumentNullException">If <paramref name="session"/> is null.</exception>
		protected virtual bool ShouldLoadEntity( EntitySession<TId> session )
		{
			if( session == null )
			{
				throw new ArgumentNullException( "session" );
			}

			Contract.EndContractBlock();

			return _entity == null || !object.Equals( ((EntitySession<TId>) session).Id, _entity.Id );
		}
		#endregion
	}
}