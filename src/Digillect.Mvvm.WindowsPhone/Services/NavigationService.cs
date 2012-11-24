using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Globalization;

using Digillect.Mvvm.UI;

namespace Digillect.Mvvm.Services
{
	public class NavigationService : INavigationService
	{
		private readonly Dictionary<string, string> _views = new Dictionary<string, string>( StringComparer.InvariantCultureIgnoreCase );
		private readonly INavigationServiceContext _navigationServiceContext;

		public NavigationService( INavigationServiceContext navigationServiceContext )
		{
			_navigationServiceContext = navigationServiceContext;

			Assembly assemblyToScan = _navigationServiceContext.GetMainAssemblyContainingViews();
			string rootNamespace = _navigationServiceContext.GetRootNamespace();

			foreach( Type type in assemblyToScan.GetTypes().Where( t => !t.IsAbstract && t.GetCustomAttributes( typeof( ViewAttribute ), false ).Count() > 0 ) )
			{
				string typeName = type.FullName.StartsWith( rootNamespace ) ? type.FullName.Substring( rootNamespace.Length + 1 ) : type.FullName;
				var viewAttribute = type.GetCustomAttributes( typeof( ViewAttribute ), false ).Cast<ViewAttribute>().FirstOrDefault();
				
				if( viewAttribute.Path != null )
				{
					_views.Add( viewAttribute.Name, viewAttribute.Path );
				}
				else
				{
					_views.Add( viewAttribute.Name, typeName.Replace( '.', '/' ) + ".xaml" );
				}
			}
		}

		public void Navigate( string viewName, Parameters parameters = null )
		{
			if( viewName == null )
			{
				throw new ArgumentNullException( "viewName" );
			}

			string path = null;

			if( !_views.TryGetValue( viewName, out path ) )
			{
				throw new ArgumentException( "View with name '" + viewName + "' is not registered.", "viewName" );
			}

			try
			{
				Uri uri = new Uri( string.Format( "/{0}{1}", path, parameters != null ? "?" + string.Join( "&", parameters.Select( p => p.Key + "=" + EncodeValue( p.Value ) ) ) : "" ), UriKind.Relative );

				_navigationServiceContext.Navigate( uri );
			}
			catch( InvalidOperationException )
			{
			}
		}

		public const string DateTimeFormatString = "yyyy-MM-ddThh:mm:sszzz";
		
		public static string EncodeValue( object value )
		{
			if( value == null )
			{
				return null;
			}

			Type valueType = value.GetType();
			string formattedValue = null;

			if( valueType == typeof( DateTime ) )
			{
				DateTime dtValue = (DateTime) value;

				formattedValue = dtValue.ToString( DateTimeFormatString, CultureInfo.InvariantCulture );
			}
			else if( valueType == typeof( DateTimeOffset ) )
			{
				DateTimeOffset dtValue = (DateTimeOffset) value;

				formattedValue = dtValue.ToString( DateTimeFormatString, CultureInfo.InvariantCulture );
			}

			else
			{
				formattedValue = (string) Convert.ChangeType( value, typeof( string ), CultureInfo.InvariantCulture );
			}

			return Uri.EscapeDataString( formattedValue );
		}

		public static object DecodeValue( string stringValue, Type targetType )
		{
			if( stringValue == null )
			{
				return null;
			}

			if( targetType == typeof( string ) )
			{
				return stringValue;
			}

			try
			{
				if( targetType == typeof( DateTime ) )
				{
					return DateTime.ParseExact( stringValue, DateTimeFormatString, CultureInfo.InvariantCulture );
				}
				else if( targetType == typeof( DateTimeOffset ) )
				{
					return DateTimeOffset.ParseExact( stringValue, DateTimeFormatString, CultureInfo.InvariantCulture );
				}
				else
				{
					return Convert.ChangeType( stringValue, targetType, CultureInfo.InvariantCulture );
				}
			}
			catch( Exception )
			{
				return null;
			}
		}
	}
}
