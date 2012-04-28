using System;
using System.Collections.Generic;
using System.Diagnostics.Contracts;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;

namespace Digillect.Mvvm.UI
{
	public class NavigationParameters
	{
		private readonly Dictionary<string, object> values = new Dictionary<string, object>();

		#region Constructors/Disposer
		public NavigationParameters()
		{
		}
		#endregion

		#region Factory
		public static NavigationParameters From<T>( T value )
			where T : struct
		{
			return new NavigationParameters().Add( value );
		}

		public static NavigationParameters From( string value )
		{
			return new NavigationParameters().Add( value );
		}

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
		public NavigationParameters Add<T>( T value )
			where T : struct
		{
			return Add( "default", value );
		}

		public NavigationParameters Add( string value )
		{
			return Add( "default", value );
		}

		public NavigationParameters Add<T>( string name, T value )
			where T : struct
		{
			Contract.Assert( name != null );

			this.values[name] = value;

			return this;
		}

		public NavigationParameters Add( string name, string value )
		{
			Contract.Assert( name != null );
			Contract.Assert( value != null );

			if( value == null )
				throw new ArgumentNullException( "value" );

			this.values[name] = value;

			return this;
		}

		public T Get<T>()
			where T : struct
		{
			return Get<T>( "default" );
		}

		public string Get()
		{
			return Get( "default" );
		}

		public T Get<T>( string name )
			where T : struct
		{
			if( !this.values.ContainsKey( name ) )
				return default( T );

			return (T) this.values[name];
		}

		public T Get<T>( string name, T defaultValue )
			where T : struct
		{
			if( !this.values.ContainsKey( name ) )
				return defaultValue;

			return (T) this.values[name];
		}

		public string Get( string name, string defaultValue = null )
		{
			if( !this.values.ContainsKey( name ) )
				return defaultValue;

			return (string) this.values[name];
		}
		#endregion

		#region Serialization
		internal void WriteTo( System.IO.BinaryWriter writer )
		{
			writer.Write( this.values.Count );

			foreach( var kv in this.values )
			{
				writer.Write( kv.Key );
				writer.Write( kv.Value.GetType().AssemblyQualifiedName );
				writer.Write( (string) Convert.ChangeType( kv.Value, typeof( string ) ) );
			}
		}

		internal void ReadFrom( System.IO.BinaryReader reader )
		{
			int count = reader.ReadInt32();

			if( count == 0 )
				return;

			while( count-- > 0 )
			{
				var key = reader.ReadString();
				var typeName = reader.ReadString();
				var type = Type.GetType( typeName );
				var valueString = reader.ReadString();
				object value = Convert.ChangeType( valueString, type );

				this.values[key] = value;
			}
		}
		#endregion
	}
}
