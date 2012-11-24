using System;
using System.Collections.Generic;
using System.Diagnostics.Contracts;
using System.Globalization;
using System.Linq;

namespace Digillect.Mvvm.UI
{
	public class NavigationParameters : IEnumerable<KeyValuePair<string, object>>
	{
		private readonly Dictionary<string, object> _values = new Dictionary<string, object>( StringComparer.OrdinalIgnoreCase );

		#region Factory
		public static NavigationParameters From<T>( string name, T value )
			where T : struct
		{
			return new NavigationParameters().Add( name, value );
		}

		public static NavigationParameters From( string name, string value )
		{
			return new NavigationParameters().Add( name, value );
		}
		#endregion

		#region Accessors
		public NavigationParameters Add<T>( string name, T value )
			where T : struct
		{
			Contract.Assert( name != null );

			_values[name] = value;

			return this;
		}

		public NavigationParameters Add( string name, string value )
		{
			Contract.Assert( name != null );
			Contract.Assert( value != null );

			_values[name] = value;

			return this;
		}

		public T Get<T>( string name )
			where T : struct
		{
			if( !_values.ContainsKey( name ) )
			{
				return default( T );
			}

			return (T) _values[name];
		}

		public T Get<T>( string name, T defaultValue )
			where T : struct
		{
			if( !_values.ContainsKey( name ) )
			{
				return defaultValue;
			}

			return (T) _values[name];
		}

		public string Get( string name, string defaultValue = null )
		{
			if( !_values.ContainsKey( name ) )
			{
				return defaultValue;
			}

			return (string) _values[name];
		}
		#endregion

		#region Serialization
		public void WriteTo( System.IO.BinaryWriter writer )
		{
			writer.Write( _values.Count );

			foreach( var kv in _values )
			{
				writer.Write( kv.Key );
				writer.Write( kv.Value.GetType().AssemblyQualifiedName );
				writer.Write( (string) Convert.ChangeType( kv.Value, typeof( string ), CultureInfo.InvariantCulture ) );
			}
		}

		public void ReadFrom( System.IO.BinaryReader reader )
		{
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
		public IEnumerator<KeyValuePair<string, object>> GetEnumerator()
		{
			return _values.GetEnumerator();
		}

		System.Collections.IEnumerator System.Collections.IEnumerable.GetEnumerator()
		{
			return _values.GetEnumerator();
		}
		#endregion
	}
}
