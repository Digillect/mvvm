using System;
using System.ComponentModel;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Net;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Documents;
using System.Windows.Ink;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Animation;
using System.Windows.Shapes;
using System.Threading;
using System.Threading.Tasks;

namespace Digillect.MVVM
{
	public class EntityPage<TId, TEntity, TViewModel> : ViewModelPage<TViewModel>
		where TId: IComparable<TId>, IEquatable<TId>
		where TEntity: XObject<TId>
		where TViewModel: EntityViewModel<TId, TEntity>, new()
	{
		private TId m_id;

		#region InitialLoadData
		protected override void InitialLoadData()
		{
			this.ViewModel.Load( m_id );
		}
		#endregion
		#region OnNavigatedTo
		protected override void OnNavigatedTo( System.Windows.Navigation.NavigationEventArgs e )
		{
			if( this.NavigationContext.QueryString.ContainsKey( "Id" ) )
			{
				m_id = (TId) Convert.ChangeType( NavigationContext.QueryString["Id"], typeof( TId ), null );
			}

			base.OnNavigatedTo( e );
		}
		#endregion

		#region Protected Properties
		public TId EntityId
		{
			get { return m_id; }
		}
		#endregion
	}
}
