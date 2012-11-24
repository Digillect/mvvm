using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;

namespace Digillect.Mvvm.Services
{
	public interface INavigationServiceContext
	{
		void Navigate( Uri uri );

		Assembly GetMainAssemblyContainingViews();
		string GetRootNamespace();
	}
}
