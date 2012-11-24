using System;
using System.Collections.Generic;
using System.Linq;

namespace Digillect.Mvvm.Services
{
	public interface INavigationService
	{
		void Navigate( string viewName, Parameters parameters = null );
	}
}
