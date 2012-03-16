using System;
using System.Collections.Generic;

namespace Digillect.Mvvm
{
	public class PageDecorationService
	{
		private readonly List<IPageDecorator> decorators = new List<IPageDecorator>();

		#region Constructors/Disposer
		private PageDecorationService()
		{
		}
		#endregion

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
		#region Singleton implementation
		private static PageDecorationService current;

		public static PageDecorationService Current
		{
			get
			{
				if( current == null )
					current = new PageDecorationService();

				return current;
			}
		}
		#endregion
	}
}
