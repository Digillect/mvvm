using System;

namespace Digillect.Mvvm
{
	/// <summary>
	/// Indicates that attributed view model should have only one instance no matter how many pages will use it.
	/// </summary>
	[AttributeUsage( AttributeTargets.Class )]
	public sealed class SingletonViewModelAttribute : Attribute
	{
	}
}
