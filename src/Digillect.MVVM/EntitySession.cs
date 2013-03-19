namespace Digillect.Mvvm
{
	/// <summary>
	/// Represents session to be used to load entity into <see cref="Digillect.Mvvm.EntityViewModel{TEntity}"/>.
	/// </summary>
	public class EntitySession : Session
	{
		private readonly XKey _key;

		#region Constructors/Disposer
		/// <summary>
		/// Initializes a new instance of the <see cref="EntitySession"/> class.
		/// </summary>
		/// <param name="key">Entity key.</param>
		public EntitySession( XKey key )
		{
			_key = key;
			Exclusive = true;
		}

		/// <summary>
		/// Initializes a new instance of the <see cref="EntitySession"/> class.
		/// </summary>
		/// <param name="key">Entity key.</param>
		/// <param name="parts">Specifies what part(s) of multipart entity to load.</param>
		public EntitySession( XKey key, params string[] parts )
			: base( parts )
		{
			_key = key;
			Exclusive = parts == null;
		}
		#endregion

		#region Public Properties
		/// <summary>
		/// Gets entity identifier.
		/// </summary>
		public XKey Key
		{
			get { return _key; }
		}
		#endregion

		#region Parameters
		/// <summary>
		/// Adds the parameter to session parameters.
		/// </summary>
		/// <param name="name">Parameter name.</param>
		/// <param name="value">Parameter value.</param>
		/// <returns>Current session.</returns>
		public new EntitySession AddParameter( string name, object value )
		{
			Parameters.Add( name, value );

			return this;
		}
		#endregion
	}
}
