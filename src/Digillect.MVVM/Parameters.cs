using System;
using System.Collections.Generic;
using System.Linq;

namespace Digillect.Mvvm
{
	/// <summary>
	/// Parameters to pass name/value pairs between parts of the system.
	/// </summary>
	[System.Diagnostics.CodeAnalysis.SuppressMessage( "Microsoft.Naming", "CA1710:IdentifiersShouldHaveCorrectSuffix" )]
	public class Parameters : IEnumerable<KeyValuePair<string, object>>
	{
		private readonly Dictionary<string, object> _values = new Dictionary<string,object>( StringComparer.OrdinalIgnoreCase );

		#region From<T>
		/// <summary>
		/// Creates instance from the specified name.
		/// </summary>
		/// <typeparam name="T"></typeparam>
		/// <param name="name">The name.</param>
		/// <param name="value">The value.</param>
		/// <returns>Constructed instance with added name/value pair.</returns>
		public static Parameters From<T>( string name, T value )
		{
			Parameters parameters = new Parameters();

			parameters.Add( name, value );

			return parameters;
		}
		#endregion
		#region Add<T>
		/// <summary>
		/// Adds the value with the specified name to current parameters.
		/// </summary>
		/// <typeparam name="T">Value type.</typeparam>
		/// <param name="name">The name.</param>
		/// <param name="value">The value.</param>
		/// <returns>Current instance</returns>
		/// <exception cref="System.ArgumentNullException">name</exception>
		public Parameters Add<T>( string name, T value )
		{
			if( name == null )
			{
				throw new ArgumentNullException( "name" );
			}

			if( value == null )
			{
				throw new ArgumentNullException( "value" );
			}

			_values.Add( name, value );

			return this;
		}
		#endregion
		#region Get/Get<T>
		/// <summary>
		/// Gets the value for the specified name.
		/// </summary>
		/// <typeparam name="T">Value type.</typeparam>
		/// <param name="name">The name.</param>
		/// <returns>Value or default value for the <typeparamref name="T"/> if specified name does not exists.</returns>
		public T Get<T>( string name )
		{
			if( !_values.ContainsKey( name ) )
			{
				return default( T );
			}

			return (T) _values[name];
		}

		/// <summary>
		/// Gets the value for the specified name.
		/// </summary>
		/// <typeparam name="T">Value type</typeparam>
		/// <param name="name">The name.</param>
		/// <param name="defaultValue">The default value.</param>
		/// <returns>Value or <paramref name="defaultValue"/> if the specified name does not exists.</returns>
		public T Get<T>( string name, T defaultValue )
		{
			if( !_values.ContainsKey( name ) )
			{
				return defaultValue;
			}

			return (T) _values[name];
		}
		#endregion

		#region IEnumerable implementation
		/// <summary>
		/// Gets the enumerator.
		/// </summary>
		/// <returns></returns>
		public IEnumerator<KeyValuePair<string, object>> GetEnumerator()
		{
			return _values.GetEnumerator();
		}

		/// <summary>
		/// Returns an enumerator that iterates through a collection.
		/// </summary>
		/// <returns>
		/// An <see cref="T:System.Collections.IEnumerator" /> object that can be used to iterate through the collection.
		/// </returns>
		System.Collections.IEnumerator System.Collections.IEnumerable.GetEnumerator()
		{
			return _values.GetEnumerator();
		}
		#endregion
	}
}
