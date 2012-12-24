using System;
using System.Collections.Generic;
using System.Linq;
using System.Globalization;

using Digillect.Mvvm.UI;
using System.Diagnostics.Contracts;
using Autofac;

namespace Digillect.Mvvm.Services
{
	internal sealed class NavigationService : INavigationService, IStartable
	{
		private readonly Dictionary<string, string> _views = new Dictionary<string, string>( StringComparer.InvariantCultureIgnoreCase );
		private readonly INavigationServiceContext _navigationServiceContext;

		#region Constructors/Disposer
		public NavigationService( INavigationServiceContext navigationServiceContext )
		{
			_navigationServiceContext = navigationServiceContext;
		}
		#endregion

		#region Start
		public void Start()
		{
			var assemblyToScan = _navigationServiceContext.GetMainAssemblyContainingViews();
			var rootNamespace = _navigationServiceContext.GetRootNamespace();

			foreach( Type type in assemblyToScan.GetTypes().Where( t => !t.IsAbstract && t.GetCustomAttributes( typeof( ViewAttribute ), false ).Count() > 0 ) )
			{
				var typeName = type.FullName.StartsWith( rootNamespace ) ? type.FullName.Substring( rootNamespace.Length + 1 ) : type.FullName;
				var viewAttribute = type.GetCustomAttributes( typeof( ViewAttribute ), false ).Cast<ViewAttribute>().FirstOrDefault();
				var viewName = viewAttribute.Name ?? type.Name;

				if( viewAttribute.Path != null )
				{
					_views.Add( viewName, viewAttribute.Path );
				}
				else
				{
					_views.Add( viewName, typeName.Replace( '.', '/' ) + ".xaml" );
				}
			}
		}
		#endregion

		#region Navigate
		public void Navigate( string viewName )
		{
			Contract.Ensures( viewName != null );

			Navigate( viewName, null );
		}

		public void Navigate( string viewName, Parameters parameters )
		{
			if( viewName == null )
			{
				throw new ArgumentNullException( "viewName" );
			}

			Contract.EndContractBlock();

			var path = (string) null;

			if( !_views.TryGetValue( viewName, out path ) )
			{
				throw new ArgumentException( String.Format( "View with name '{0}' is not registered.", viewName ), "viewName" );
			}

			try
			{
				var uri = new Uri( string.Format( "/{0}{1}", path, parameters != null ? "?" + string.Join( "&", parameters.Select( p => p.Key + "=" + EncodeValue( p.Value ) ) ) : string.Empty ), UriKind.Relative );

				_navigationServiceContext.Navigate( uri );
			}
			catch( InvalidOperationException )
			{
			}
		}
		#endregion

		#region Encode/Decode values
		public const string DateTimeFormatString = "yyyy-MM-ddThh:mm:sszzz";

		public static string EncodeValue( object value )
		{
			if( value == null )
			{
				return null;
			}

			var valueType = value.GetType();
			string formattedValue = null;

			if( valueType == typeof( DateTime ) )
			{
				var dtValue = (DateTime) value;

				formattedValue = dtValue.ToString( DateTimeFormatString, CultureInfo.InvariantCulture );
			}
			else
			{
				if( valueType == typeof( DateTimeOffset ) )
				{
					var dtValue = (DateTimeOffset) value;

					formattedValue = dtValue.ToString( DateTimeFormatString, CultureInfo.InvariantCulture );
				}

				else
				{
					formattedValue = (string) Convert.ChangeType( value, typeof( string ), CultureInfo.InvariantCulture );
				}
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
				else
				{
					if( targetType == typeof( DateTimeOffset ) )
					{
						return DateTimeOffset.ParseExact( stringValue, DateTimeFormatString, CultureInfo.InvariantCulture );
					}
					else
					{
						return Convert.ChangeType( stringValue, targetType, CultureInfo.InvariantCulture );
					}
				}
			}
			catch( Exception )
			{
				return null;
			}
		}
		#endregion
	}
}
