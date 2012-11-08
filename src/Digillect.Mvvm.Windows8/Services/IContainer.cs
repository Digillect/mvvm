using System;
using System.Collections.Generic;
using System.Linq;

namespace Digillect.Mvvm.Services
{
	public interface IContainer
	{
		T Resolve<T>() where T : class;
		T ResolveOptional<T>() where T : class;
		IEnumerable<T> ResolveAll<T>() where T : class;
	}
}
