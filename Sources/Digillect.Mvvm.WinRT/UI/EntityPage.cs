using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Windows.UI.Xaml.Navigation;

namespace Digillect.Mvvm.UI
{
	[Windows.Foundation.Metadata.WebHostHidden]
	public class EntityPage : ViewModelPage
	{
		private object entityId;

		#region Public Properties
		protected object EntityId
		{
			get { return this.entityId; }
		}
		#endregion

		#region OnNavigatedTo
		/// <summary>
		/// Raises the <see cref="E:NavigatedTo"/> event. Used to extract entity identifier from query string.
		/// </summary>
		/// <param name="e">The <see cref="NavigationEventArgs"/> instance containing the event data.</param>
		/// <exception cref="System.ArgumentException">when identifier can't be found in query string.</exception>
		protected override void OnNavigatedTo( NavigationEventArgs e )
		{
			this.entityId = e.Parameter;

			base.OnNavigatedTo( e );
		}
		#endregion
	}
}
