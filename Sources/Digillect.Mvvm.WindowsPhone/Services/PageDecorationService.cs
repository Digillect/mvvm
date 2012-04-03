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
		private readonly List<IPageDecorator> decorators = new List<IPageDecorator>();

		#region Add/Remove Decorators
		/// <summary>
		/// Adds the decorator to collection of active decorators.
		/// </summary>
		/// <param name="decorator">The decorator.</param>
		public void AddDecorator( IPageDecorator decorator )
		{
			if( decorator == null )
				throw new ArgumentNullException( "decorator" );

			decorators.Add( decorator );
		}

		/// <summary>
		/// Removes the decorator from collection of active decorators.
		/// </summary>
		/// <param name="decorator">The decorator.</param>
		public void RemoveDecorator( IPageDecorator decorator )
		{
			if( decorator == null )
				throw new ArgumentNullException( "decorator" );

			decorators.Remove( decorator );
		}
		#endregion

		#region Add/Remove Page decoration
		/// <summary>
		/// Performs decoration of the page.
		/// </summary>
		/// <param name="page">The page.</param>
		public void AddDecoration( PhoneApplicationPage page )
		{
			if( page == null )
				throw new ArgumentNullException( "page" );

			decorators.ForEach( d => d.AddDecoration( page ) );
		}

		/// <summary>
		/// Optionally removes decoration from the page.
		/// </summary>
		/// <param name="page">The page.</param>
		public void RemoveDecoration( PhoneApplicationPage page )
		{
			if( page == null )
				throw new ArgumentNullException( "page" );

			decorators.ForEach( d => d.RemoveDecoration( page ) );
		}
		#endregion
	}
}
