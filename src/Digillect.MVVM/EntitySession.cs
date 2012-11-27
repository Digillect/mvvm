using System;
using System.Diagnostics.Contracts;

namespace Digillect.Mvvm
{
	/// <summary>
	/// Represents session to be used to load entity into <see cref="Digillect.Mvvm.EntityViewModel{TId,TEntity}"/>.
	/// </summary>
	/// <typeparam name="TId">Type of entity's identifier.</typeparam>
	public class EntitySession<TId> : Session
	{
		private readonly TId _id;

		#region Constructors/Disposer
		/// <summary>
		/// Initializes a new instance of the <see cref="EntitySession&lt;TId&gt;"/> class.
		/// </summary>
		/// <param name="id">Entity identifier.</param>
		public EntitySession( TId id )
		{
			_id = id;
			Exclusive = true;
		}

		/// <summary>
		/// Initializes a new instance of the <see cref="EntitySession&lt;TId&gt;"/> class.
		/// </summary>
		/// <param name="id">Entity identifier.</param>
		/// <param name="parts">Specifies what part(s) of multipart entity to load.</param>
		public EntitySession( TId id, params string[] parts )
			: base( parts )
		{
			_id = id;
			Exclusive = parts == null;
		}
		#endregion

		#region Public Properties
		/// <summary>
		/// Gets entity identifier.
		/// </summary>
		public TId Id
		{
			get { return _id; }
		}
		#endregion

		#region Parameters
		public new EntitySession<TId> AddParameter( string name, object value )
		{
			Parameters.Add( name, value );

			return this;
		}
		#endregion
	}
}
