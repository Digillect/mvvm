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
using System.Collections;
using System.Collections.Generic;
using System.Diagnostics.CodeAnalysis;
using System.Diagnostics.Contracts;

namespace Digillect.Mvvm
{
	/// <summary>
	///     Parameters to pass name/value pairs between parts of the system.
	/// </summary>
	[SuppressMessage( "Microsoft.Naming", "CA1710:IdentifiersShouldHaveCorrectSuffix" )]
	public class Parameters : IEnumerable<KeyValuePair<string, object>>
	{
		private readonly Dictionary<string, object> _values = new Dictionary<string, object>( StringComparer.OrdinalIgnoreCase );

		/// <summary>
		///     Creates instance from the specified name/value pair.
		/// </summary>
		/// <param name="name">Parameter name.</param>
		/// <param name="value">Parameter value.</param>
		/// <returns>Constructed instance with added name/value pair.</returns>
		public static Parameters Create( string name, object value )
		{
			Contract.Requires<ArgumentNullException>( name != null, "name" );
			Contract.Requires<ArgumentNullException>( value != null, "value" );
			Contract.Ensures( Contract.Result<Parameters>() != null );

			Parameters parameters = new Parameters { { name, value } };

			return parameters;
		}

		/// <summary>
		///     Adds the value with the specified name to current parameters.
		/// </summary>
		/// <param name="name">Parameter name.</param>
		/// <param name="value">Parameter value.</param>
		/// <returns>Current instance</returns>
		/// <exception cref="System.ArgumentNullException">
		///     If <paramref name="name" /> or <paramref name="value" /> is <c>null</c>.
		/// </exception>
		public Parameters Add( string name, object value )
		{
			Contract.Requires<ArgumentNullException>( name != null, "name" );
			Contract.Requires<ArgumentNullException>( value != null, "value" );
			Contract.Ensures( Contract.Result<Parameters>() != null );

			_values.Add( name, value );

			return this;
		}

		/// <summary>
		///     Gets the value for the specified name.
		/// </summary>
		/// <typeparam name="T">Value type.</typeparam>
		/// <param name="name">Parameter name.</param>
		/// <returns>
		///     Value or default value for the <typeparamref name="T" /> if the specified name does not exists.
		/// </returns>
		/// <exception cref="System.ArgumentNullException">
		///     If <paramref name="name" /> is <c>null</c>.
		/// </exception>
		public T Get<T>( string name )
		{
			Contract.Requires<ArgumentNullException>( name != null, "name" );

			if( !_values.ContainsKey( name ) )
			{
				return default(T);
			}

			return (T) _values[name];
		}

		/// <summary>
		///     Gets the value for the specified name.
		/// </summary>
		/// <typeparam name="T">Value type</typeparam>
		/// <param name="name">Parameter name.</param>
		/// <param name="defaultValue">Value to use as a default.</param>
		/// <returns>
		///     Parameter value or <paramref name="defaultValue" /> if specified <paramref name="name" /> is not found.
		/// </returns>
		/// <exception cref="System.ArgumentNullException">
		///     If <paramref name="name" /> is <c>null</c>.
		/// </exception>
		public T Get<T>( string name, T defaultValue )
		{
			Contract.Requires<ArgumentNullException>( name != null, "name" );

			if( !_values.ContainsKey( name ) )
			{
				return defaultValue;
			}

			return (T) _values[name];
		}

		/// <summary>
		///     Determines whether parameters contains the specified name.
		/// </summary>
		/// <param name="name">Name to check.</param>
		/// <returns>
		///     <c>true</c> if parameters contains the specified name; otherwise, <c>false</c>.
		/// </returns>
		/// <exception cref="System.ArgumentNullException">
		///     If <paramref name="name" /> is <c>null</c>.
		/// </exception>
		public bool Contains( string name )
		{
			Contract.Requires<ArgumentNullException>( name != null, "name" );

			return _values.ContainsKey( name );
		}

		#region IEnumerable implementation
		/// <summary>
		///     Gets the enumerator.
		/// </summary>
		/// <returns></returns>
		public IEnumerator<KeyValuePair<string, object>> GetEnumerator()
		{
			return _values.GetEnumerator();
		}

		/// <summary>
		///     Returns an enumerator that iterates through a collection.
		/// </summary>
		/// <returns>
		///     An <see cref="T:System.Collections.IEnumerator" /> object that can be used to iterate through the collection.
		/// </returns>
		IEnumerator IEnumerable.GetEnumerator()
		{
			return _values.GetEnumerator();
		}
		#endregion
	}
}