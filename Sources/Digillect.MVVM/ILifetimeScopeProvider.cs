using System;

using Autofac;

namespace Digillect.Mvvm
{
	/// <summary>
	/// Enables infrastructure to access <see cref="Autofac.ILifetimeScope"/>.
	/// </summary>
	public interface ILifetimeScopeProvider
	{
		/// <summary>
		/// Gets the scope.
		/// </summary>
		ILifetimeScope Scope { get; }
	}
}
