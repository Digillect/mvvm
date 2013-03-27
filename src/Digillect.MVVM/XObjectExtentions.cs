namespace Digillect.Mvvm
{
	/// <summary>
	/// Contains the extensions to simplify creation of navigation commands.
	/// </summary>
	public static class XObjectExtensions
	{
		/// <summary>
		/// Create navigation parameters that contains object key.
		/// </summary>
		/// <param name="source">The object to get key from.</param>
		/// <returns><see cref="Digillect.XKey"/> that contains entry named <c>key</c> with the value of object's key.</returns>
		public static Parameters KeyParameter( this XObject source )
		{
			return Parameters.Create( "key", source.GetKey() );
		}
	}
}