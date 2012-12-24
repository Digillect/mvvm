﻿using System;
using System.Collections.Generic;
using System.Linq;

namespace Digillect.Mvvm.Services
{
	/// <summary>
	/// Service that handles the navigation between views.
	/// </summary>
	public interface INavigationService
	{
		/// <summary>
		/// Navigates to the specified view.
		/// </summary>
		/// <param name="viewName">Name of the view.</param>
		void Navigate( string viewName );
		/// <summary>
		/// Navigates to the specified view with parameters.
		/// </summary>
		/// <param name="viewName">Name of the view.</param>
		/// <param name="parameters">The parameters.</param>
		void Navigate( string viewName, Parameters parameters );
	}
}
