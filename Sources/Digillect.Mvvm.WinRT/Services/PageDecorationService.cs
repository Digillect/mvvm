using System;
using System.Collections.Generic;

using Digillect.Mvvm.UI;

namespace Digillect.Mvvm.Services
{
	/// <summary>
	/// Default implementation of <see cref="IPageDecorationService"/>.
	/// </summary>
	internal sealed class PageDecorationService : IPageDecorationService
	{
		private readonly List<IPageDecorator> decorators;

		#region Constructors/Disposer
		public PageDecorationService( IEnumerable<IPageDecorator> decorators )
		{
			this.decorators = new List<IPageDecorator>( decorators );
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

			if( this.decorators == null )
				return;

			foreach( var decorator in this.decorators )
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

			if( this.decorators == null )
				return;

			foreach( var decorator in this.decorators )
				decorator.RemoveDecoration( page );
		}
		#endregion
	}
}
