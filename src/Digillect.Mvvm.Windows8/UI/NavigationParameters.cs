using System;
using System.Collections.Generic;
using System.Diagnostics.Contracts;
using System.Globalization;
using System.Linq;

namespace Digillect.Mvvm.UI
{
	/// <summary>
	/// Class that holds name-value pairs used as parameters when navigating between pages.
	/// </summary>
	[System.Diagnostics.CodeAnalysis.SuppressMessage( "Microsoft.Naming", "CA1710:IdentifiersShouldHaveCorrectSuffix" )]
	public class NavigationParameters : IEnumerable<KeyValuePair<string, object>>
	{
		private readonly Dictionary<string, object> _values = new Dictionary<string, object>( StringComparer.OrdinalIgnoreCase );

		#region Factory
		/// <summary>
		/// Creates instance from the specified name and value.
		/// </summary>
		/// <typeparam name="T">Value type.</typeparam>
		/// <param name="name">The name.</param>
		/// <param name="value">The value.</param>
		/// <returns>Instance of navigation parameters.</returns>
		public static NavigationParameters From<T>( string name, T value )
			where T : struct
		{
			return new NavigationParameters().Add( name, value );
		}

		/// <summary>
		/// Creates instance from the specified name and value.
		/// </summary>
		/// <param name="name">The name.</param>
		/// <param name="value">The value.</param>
		/// <returns>Instance of navigation parameters.</returns>
		public static NavigationParameters From( string name, string value )
		{
			Contract.Ensures( Contract.Result<NavigationParameters>() != null );

			return new NavigationParameters().Add( name, value );
		}
		#endregion

		#region Accessors
		/// <summary>
		/// Adds the specified name/value pair to parameters.
		/// </summary>
		/// <typeparam name="T">Value type.</typeparam>
		/// <param name="name">The name.</param>
		/// <param name="value">The value.</param>
		/// <returns>Current instance of navigation parameters.</returns>
		/// <exception cref="System.ArgumentNullException">When name is null.</exception>
		public NavigationParameters Add<T>( string name, T value )
			where T : struct
		{
			if( name == null )
			{
				throw new ArgumentNullException( "name" );
			}

			Contract.Ensures( Contract.Result<NavigationParameters>() != null );

			_values[name] = value;

			return this;
		}

		/// <summary>
		/// Adds the specified name/value pair to parameters.
		/// </summary>
		/// <param name="name">The name.</param>
		/// <param name="value">The value.</param>
		/// <returns>Current instance of navigation parameters.</returns>
		/// <exception cref="System.ArgumentNullException">When name or value is null.</exception>
		public NavigationParameters Add( string name, string value )
		{
			if( name == null )
			{
				throw new ArgumentNullException( "name" );
			}

			if( value == null )
			{
				throw new ArgumentNullException( "value" );
			}
			
			Contract.Ensures( Contract.Result<NavigationParameters>() != null );

			_values[name] = value;

			return this;
		}

		/// <summary>
		/// Gets the value for the specified name.
		/// </summary>
		/// <typeparam name="T">Value type.</typeparam>
		/// <param name="name">The name.</param>
		/// <returns>Value of the parameter or default type value if specified name is not exists.</returns>
		/// <exception cref="System.ArgumentNullException">When <paramref name="name"/> is null.</exception>
		public T Get<T>( string name )
			where T : struct
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
		/// <typeparam name="T">Value type.</typeparam>
		/// <param name="name">The name.</param>
		/// <param name="defaultValue">The default value.</param>
		/// <returns>Value of the parameter or <paramref name="defaultValue"/> if specified <paramref name="name"/> is not exists.</returns>
		/// <exception cref="System.ArgumentNullException">When <paramref name="name"/> is null.</exception>
		public T Get<T>( string name, T defaultValue )
			where T : struct
		{
			if( name == null )
			{
				throw new ArgumentNullException( "name" );
			}

			Contract.EndContractBlock();

			if( !_values.ContainsKey( name ) )
			{
				return defaultValue;
			}

			return (T) _values[name];
		}

		/// <summary>
		/// Gets the value for the specified name as string.
		/// </summary>
		/// <param name="name">The name.</param>
		/// <returns>String value or null.</returns>
		public string Get( string name )
		{
			Contract.Assert( name != null );

			return Get( name, null );
		}

		/// <summary>
		/// Gets the value for the specified name as string.
		/// </summary>
		/// <param name="name">The name.</param>
		/// <param name="defaultValue">The default value.</param>
		/// <returns>String value or <paramref name="defaultValue"/> if specified <paramref name="name"/> is not exists.</returns>
		/// <exception cref="System.ArgumentNullException">When <paramref name="name"/> is null</exception>
        public string Get( string name, string defaultValue )
		{
			if( name == null )
			{
				throw new ArgumentNullException( "name" );
			}

			Contract.EndContractBlock();

			if( !_values.ContainsKey( name ) )
			{
				return defaultValue;
			}

			return (string) _values[name];
		}
		#endregion

		#region Serialization
		/// <summary>
		/// Writes parameters to the specified writer.
		/// </summary>
		/// <param name="writer">The writer.</param>
		/// <exception cref="System.ArgumentNullException">When <paramref name="writer"/> is null.</exception>
		public void WriteTo( System.IO.BinaryWriter writer )
		{
			if( writer == null )
			{
				throw new ArgumentNullException( "writer" );
			}

			Contract.EndContractBlock();

			writer.Write( _values.Count );

			foreach( var kv in _values )
			{
				writer.Write( kv.Key );
				writer.Write( kv.Value.GetType().AssemblyQualifiedName );
				writer.Write( (string) Convert.ChangeType( kv.Value, typeof( string ), CultureInfo.InvariantCulture ) );
			}
		}

		/// <summary>
		/// Reads parameters from the specified reader.
		/// </summary>
		/// <param name="reader">The reader.</param>
		/// <exception cref="System.ArgumentNullException">When <paramref name="reader"/> is null.</exception>
		public void ReadFrom( System.IO.BinaryReader reader )
		{
			if( reader == null )
			{
				throw new ArgumentNullException( "reader" );
			}

			Contract.EndContractBlock();

			int count = reader.ReadInt32();

			if( count == 0 )
			{
				return;
			}

			while( count-- > 0 )
			{
				var key = reader.ReadString();
				var typeName = reader.ReadString();
				var type = Type.GetType( typeName );
				var valueString = reader.ReadString();
				object value = Convert.ChangeType( valueString, type, CultureInfo.InvariantCulture );

				_values[key] = value;
			}
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
