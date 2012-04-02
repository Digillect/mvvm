using System;
using System.Collections.Generic;

using Digillect.Mvvm.UI;

namespace Digillect.Mvvm.Services
{
	/// <summary>
	/// Service that is used to handle page decoration and decorators management.
	/// </summary>
	public interface IPageDecorationService
	{
		/// <summary>
		/// Adds the decorator to collection of active decorators.
		/// </summary>
		/// <param name="decorator">The decorator.</param>
		void AddDecorator( IPageDecorator decorator );
		
		/// <summary>
		/// Removes the decorator from collection of active decorators.
		/// </summary>
		/// <param name="decorator">The decorator.</param>
		void RemoveDecorator( IPageDecorator decorator );

		/// <summary>
		/// Performs decoration of the page.
		/// </summary>
		/// <param name="page">The page.</param>
		void AddDecoration( PhoneApplicationPage page );
		
		/// <summary>
		/// Optionally removes decoration from the page.
		/// </summary>
		/// <param name="page">The page.</param>
		void RemoveDecoration( PhoneApplicationPage page );
	}
}
