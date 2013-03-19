namespace Digillect.Mvvm
{
	/// <summary>
	/// Contains the extentions to simplify creation of navigation commands.
	/// </summary>
	public static class XObjectExtentions
	{
		/// <summary>
		/// Create navigation parameters that contains object key.
		/// </summary>
		/// <param name="obj">The object to get key from.</param>
		/// <returns><see cref="Digillect.XKey"/> that contains entry named <c>key</c> with the value of object's key.</returns>
		public static Parameters KeyParameter( this XObject obj )
		{
			return Parameters.Create( "key", obj.GetKey() );
		}
	}
}