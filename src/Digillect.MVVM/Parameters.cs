using System;
using System.Collections.Generic;
using System.Linq;

namespace Digillect.Mvvm
{
	public class Parameters : IEnumerable<KeyValuePair<string, object>>
	{
		private readonly Dictionary<string, object> _values = new Dictionary<string,object>( StringComparer.OrdinalIgnoreCase );

		#region From<T>
		public static Parameters From<T>( string name, T value )
		{
			Parameters parameters = new Parameters();

			parameters.Add( name, value );

			return parameters;
		}
		#endregion
		#region Add<T>
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
		public T Get<T>( string name )
		{
			if( !_values.ContainsKey( name ) )
			{
				return default( T );
			}

			return (T) _values[name];
		}

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
