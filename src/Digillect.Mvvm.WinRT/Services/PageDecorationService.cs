using System;
using System.Collections.Generic;

using Digillect.Mvvm.UI;

namespace Digillect.Mvvm.Services
{
	/// <summary>
	/// Default implementation of <see cref="IPageDecorationService"/>.
	/// </summary>
	public sealed class PageDecorationService : IPageDecorationService
	{
		#region Constructors/Disposer
		public PageDecorationService()
		{
		}
		#endregion


		#region Add/Remove Page decoration
		/// <summary>
		/// Performs decoration of the page.
		/// </summary>
		/// <param name="page">The page.</param>
		public void AddDecoration( Page page )
		{
			if( page == null )
				throw new ArgumentNullException( "page" );

			var decorators = page.Container.ResolveAll<IPageDecorator>();

			foreach( var decorator in decorators )
				decorator.AddDecoration( page );
		}

		/// <summary>
		/// Optionally removes decoration from the page.
		/// </summary>
		/// <param name="page">The page.</param>
		public void RemoveDecoration( Page page )
		{
			if( page == null )
				throw new ArgumentNullException( "page" );

			var decorators = page.Container.ResolveAll<IPageDecorator>();

			foreach( var decorator in decorators )
				decorator.RemoveDecoration( page );
		}
		#endregion
	}
}
