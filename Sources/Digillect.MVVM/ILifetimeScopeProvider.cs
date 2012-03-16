using System;

using Autofac;

namespace Digillect.Mvvm
{
	public interface ILifetimeScopeProvider
	{
		ILifetimeScope Scope { get; }
	}
}
