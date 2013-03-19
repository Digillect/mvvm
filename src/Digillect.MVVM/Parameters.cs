using System;
using System.Collections.Generic;
using System.Diagnostics.Contracts;
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

		/// <summary>
		/// Creates instance from the specified name/value pair.
		/// </summary>
		/// <param name="name">Parameter name.</param>
		/// <param name="value">Parameter value.</param>
		/// <returns>Constructed instance with added name/value pair.</returns>
		public static Parameters Create( string name, object value )
		{
			var parameters = new Parameters { { name, value } };

			return parameters;
		}

		/// <summary>
		/// Adds the value with the specified name to current parameters.
		/// </summary>
		/// <param name="name">Parameter name.</param>
		/// <param name="value">Parameter value.</param>
		/// <returns>Current instance</returns>
		/// <exception cref="System.ArgumentNullException">If <paramref name="name"/> or <paramref name="value"/> is <c>null</c>.</exception>
		public Parameters Add( string name, object value )
		{
			if( name == null )
			{
				throw new ArgumentNullException( "name" );
			}

			if( value == null )
			{
				throw new ArgumentNullException( "value" );
			}

			Contract.EndContractBlock();

			_values.Add( name, value );

			return this;
		}

		/// <summary>
		/// Gets the value for the specified name.
		/// </summary>
		/// <typeparam name="T">Value type.</typeparam>
		/// <param name="name">Parameter name.</param>
		/// <returns>Value or default value for the <typeparamref name="T"/> if the specified name does not exists.</returns>
		/// <exception cref="System.ArgumentNullException">If <paramref name="name"/> is <c>null</c>.</exception>
		public T Get<T>( string name )
		{
			if( name == null )
			{
				throw new ArgumentNullException( "name" );
			}

			Contract.EndContractBlock();

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
		/// <param name="name">Parameter name.</param>
		/// <param name="defaultValue">Value to use as a default.</param>
		/// <returns>Parameter value or <paramref name="defaultValue"/> if specifiedn <paramref name="name"/> is not found.</returns>
		/// <exception cref="System.ArgumentNullException">If <paramref name="name"/> is <c>null</c>.</exception>
		public T Get<T>( string name, T defaultValue )
		{
			if( name == null )
			{
				throw new ArgumentNullException( "name" );
			}

			if( !_values.ContainsKey( name ) )
			{
				return defaultValue;
			}

			return (T) _values[name];
		}

		/// <summary>
		/// Determines whether parameters contains the specified name.
		/// </summary>
		/// <param name="name">Name to check.</param>
		/// <returns>
		///   <c>true</c> if parameters contains the specified name; otherwise, <c>false</c>.
		/// </returns>
		/// <exception cref="System.ArgumentNullException">If <paramref name="name"/> is <c>null</c>.</exception>
		public bool Contains( string name )
		{
			if( name == null )
			{
				throw new ArgumentNullException( "name" );
			}

			Contract.EndContractBlock();

			return _values.ContainsKey( name );
		}

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
