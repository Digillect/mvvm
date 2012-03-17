using System;
using System.Collections.Generic;

namespace Digillect.Mvvm.Services
{
	internal class PageDecorationService : IPageDecorationService
	{
		private readonly List<IPageDecorator> decorators = new List<IPageDecorator>();

		#region Add/Remove Decorators
		public void AddDecorator( IPageDecorator decorator )
		{
			if( decorator == null )
				throw new ArgumentNullException( "decorator" );

			decorators.Add( decorator );
		}

		public void RemoveDecorator( IPageDecorator decorator )
		{
			if( decorator == null )
				throw new ArgumentNullException( "decorator" );

			decorators.Remove( decorator );
		}
		#endregion

		#region Add/Remove Page decoration
		public void AddDecoration( PhoneApplicationPage page )
		{
			if( page == null )
				throw new ArgumentNullException( "page" );

			decorators.ForEach( d => d.AddDecoration( page ) );
		}

		public void RemoveDecoration( PhoneApplicationPage page )
		{
			if( page == null )
				throw new ArgumentNullException( "page" );

			decorators.ForEach( d => d.RemoveDecoration( page ) );
		}
		#endregion
	}
}
