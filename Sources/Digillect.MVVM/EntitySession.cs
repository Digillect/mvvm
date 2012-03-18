using System;

namespace Digillect.Mvvm
{
	/// <summary>
	/// Represents session to be used to load entity into <see cref="Digillect.Mvvm.EntityViewModel{TId,TEntity}"/>.
	/// </summary>
	/// <typeparam name="TId">Type of entity's identifier.</typeparam>
	public class EntitySession<TId> : Session
	{
		/// <summary>
		/// Constant to identify entity part.
		/// </summary>
		public const string Entity = "Entity";

		/// <summary>
		/// Gets entity identifier.
		/// </summary>
		public TId Id { get; private set; }
		/// <summary>
		/// Gets logical part for multipart requests.
		/// </summary>
		public string Part { get; private set; }

		#region Constructors/Disposer
		/// <summary>
		/// Initializes a new instance of the <see cref="EntitySession&lt;TId&gt;"/> class.
		/// </summary>
		/// <param name="id">Entity identifier.</param>
		/// <param name="part">Specifies what part of multipart entity to load, by default load everything.</param>
		public EntitySession( TId id, string part = null )
		{
			this.Id = id;
			this.Part = part;
			this.Exclusive = part == null;
		}
		#endregion

		#region Public Properties
		/// <summary>
		/// Gets a value indicating whether this instance is partial.
		/// </summary>
		/// <value>
		/// 	<c>true</c> if this instance is partial; otherwise, <c>false</c>.
		/// </value>
		public bool IsPartial
		{
			get { return this.Part != null; }
		}

		/// <summary>
		/// Gets a value indicating whether this session loads entity.
		/// </summary>
		/// <value>
		///   <c>true</c> if this session loads entity itself; otherwise, <c>false</c>.
		/// </value>
		public bool IsEntity
		{
			get { return this.Part == null || this.Part == Entity; }
		}
		#endregion

		/// <summary>
		/// Checks that session is used to load specified logical part.
		/// </summary>
		/// <param name="part">Part to check, can't be <c>null</c>.</param>
		/// <returns><c>true</c> if specified part is loading; otherwise, <c>false</c>.</returns>
		/// <exception cref="System.ArgumentNullException">if part is <c>null</c>.</exception>
		public bool Includes( string part )
		{
			if( part == null )
				throw new ArgumentNullException( "part" );

			if( this.Part == null )
				return true;

			return this.Part == part;
		}
	}
}
