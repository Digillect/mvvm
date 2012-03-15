using System;

namespace Digillect.Mvvm
{
	[AttributeUsage( AttributeTargets.Class )]
	public sealed class SingletonViewModelAttribute : Attribute
	{
	}
}
