#region Copyright (c) 2011-2013 Gregory Nickonov and Andrew Nefedkin (Actis® Wunderman)
// Copyright (c) 2011-2013 Gregory Nickonov and Andrew Nefedkin (Actis® Wunderman).
// 
// Permission is hereby granted, free of charge, to any person obtaining a copy of this
// software and associated documentation files (the "Software"), to deal in the Software
// without restriction, including without limitation the rights to use, copy, modify, merge,
// publish, distribute, sublicense, and/or sell copies of the Software, and to permit persons
// to whom the Software is furnished to do so, subject to the following conditions:
// 
// The above copyright notice and this permission notice shall be included in all copies or
// substantial portions of the Software.
// 
// THE SOFTWARE IS PROVIDED "AS IS", WITHOUT WARRANTY OF ANY KIND, EXPRESS OR
// IMPLIED, INCLUDING BUT NOT LIMITED TO THE WARRANTIES OF MERCHANTABILITY,
// FITNESS FOR A PARTICULAR PURPOSE AND NONINFRINGEMENT. IN NO EVENT SHALL
// THE AUTHORS OR COPYRIGHT HOLDERS BE LIABLE FOR ANY CLAIM, DAMAGES OR OTHER
// LIABILITY, WHETHER IN AN ACTION OF CONTRACT, TORT OR OTHERWISE, ARISING FROM,
// OUT OF OR IN CONNECTION WITH THE SOFTWARE OR THE USE OR OTHER DEALINGS
// IN THE SOFTWARE.
#endregion

using System;
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
		private const string KeyParameter = "Key";
		public const string EntityAction = "Entity";
		private TEntity _entity;

		#region Constructors/Disposer
		/// <summary>
		///     Initializes a new instance of the <see cref="EntityViewModel{TEntity}" /> class.
		/// </summary>
		protected EntityViewModel()
		{
			RegisterAction()
				.AddPart( LoadEntity, ShouldLoadEntity );

			RegisterAction( EntityAction )
				.AddPart( LoadEntity, ShouldLoadEntity );
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

		#region Session factories
		/// <summary>
		///     Creates the session that loads only the entity.
		/// </summary>
		/// <param name="key">Entity key.</param>
		/// <returns>
		///     Session that loads entity using specified <paramref name="key" />.
		/// </returns>
		[SuppressMessage( "Microsoft.Reliability", "CA2000:Dispose objects before losing scope", Justification = "Session can be used by caller." )]
		public Session CreateEntitySession( XKey key )
		{
			Contract.Requires<ArgumentNullException>( key != null, "key" );
			Contract.Ensures( Contract.Result<Session>() != null );

			return new Session( XParameters.Create( KeyParameter, key ), EntityAction );
		}

		/// <summary>
		///     Creates the session that executes default action.
		/// </summary>
		/// <param name="key">Entity key.</param>
		/// <returns>
		///     Session that executes default action with parameters containing specified <paramref name="key" />.
		/// </returns>
		[SuppressMessage( "Microsoft.Reliability", "CA2000:Dispose objects before losing scope", Justification = "Session can be used by caller." )]
		public Session CreateSession( XKey key )
		{
			Contract.Requires<ArgumentNullException>( key != null, "key" );
			Contract.Ensures( Contract.Result<Session>() != null );

			return new Session( XParameters.Create( KeyParameter, key ), null );
		}

		/// <summary>
		///     Creates the session that executes specified action.
		/// </summary>
		/// <param name="key">Entity key.</param>
		/// <param name="action">Action to execute</param>
		/// <returns>
		///     Session that executes specified action with parameters containing specified <paramref name="key" />.
		/// </returns>
		[SuppressMessage( "Microsoft.Reliability", "CA2000:Dispose objects before losing scope", Justification = "Session can be used by caller." )]
		public Session CreateSession( XKey key, string action )
		{
			Contract.Requires<ArgumentNullException>( key != null, "key" );
			Contract.Ensures( Contract.Result<Session>() != null );

			return new Session( XParameters.Create( KeyParameter, key ), action );
		}
		#endregion

		#region Parts processors
		/// <summary>
		///     Implement this method to load entity.
		/// </summary>
		/// <param name="session">The session.</param>
		/// <returns>
		///     <see cref="System.Threading.Tasks.Task" /> that can be awaited.
		/// </returns>
		protected abstract Task LoadEntity( Session session );

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
		protected virtual bool ShouldLoadEntity( Session session )
		{
			Contract.Requires<ArgumentNullException>( session != null, "session" );

			return _entity == null || !_entity.GetKey().Equals( session.Parameters.GetValue<XKey>( KeyParameter ) );
		}
		#endregion
	}
}