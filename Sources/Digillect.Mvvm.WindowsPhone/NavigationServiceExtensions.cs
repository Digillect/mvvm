using System;
using System.Net;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Documents;
using System.Windows.Ink;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Animation;
using System.Windows.Navigation;

namespace Digillect.Mvvm
{
	public static class NavigationServiceExtensions
	{
		public static void Navigate<TPage>( this NavigationService navigationService, string queryString = null )
			where TPage : PhoneApplicationPage
		{
			var pageType = typeof( TPage );
			var applicationType = Application.Current.GetType();
			var samePath = pageType.Namespace.StartsWith( applicationType.Namespace );

			if( !pageType.Namespace.StartsWith( applicationType.Namespace ) )
				throw new InvalidOperationException( "Page must be in the child namespace relative to Application's namespace." );

			var uri = string.Format( "{0}{1}/{2}.xaml", pageType.Namespace == applicationType.Namespace ? "/" : "", pageType.Namespace.Substring( applicationType.Namespace.Length ).Replace( '.', '/' ), pageType.Name );

			if( !string.IsNullOrWhiteSpace( queryString ) )
				uri += "?" + queryString;

			navigationService.Navigate( new Uri( uri, UriKind.Relative ) );
		}
	}
}
