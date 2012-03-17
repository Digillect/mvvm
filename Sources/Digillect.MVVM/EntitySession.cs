using System;

namespace Digillect.Mvvm
{
	/// <summary>
	/// Represents session to be used to load entity into <see cref="EntityViewModel"/>.
	/// </summary>
	/// <typeparam name="TId">Type of entity's identifier.</typeparam>
	public class EntitySession<TId> : Session
	{
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
		/// Constructs new session with specified identifier and part.
		/// </summary>
		/// <param name="id">Entity identifier</param>
		/// <param name="part">Part to be loaded, by default load everything.</param>
		public EntitySession( TId id, string part = null )
		{
			this.Id = id;
			this.Part = part;
			this.Exclusive = part == null;
		}
		#endregion

		#region Public Properties
		/// <summary>
		/// Gets whether this session is partial or not.
		/// </summary>
		public bool IsPartial
		{
			get { return this.Part != null; }
		}

		/// <summary>
		/// Gets whether this session loads main entity.
		/// </summary>
		public bool IsEntity
		{
			get { return this.Part == null || this.Part == Entity; }
		}
		#endregion

		/// <summary>
		/// Checks that session is used to load specified logical part.
		/// </summary>
		/// <param name="part">Part to check</param>
		/// <returns>True if part is loading, otherwise false.</returns>
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
