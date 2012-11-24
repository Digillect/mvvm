using System;
using System.Collections.Generic;
using System.Linq;

namespace Digillect.Mvvm.UI
{
	[AttributeUsage( AttributeTargets.Class, Inherited = false, AllowMultiple = true )]
	public sealed class ViewParameterAttribute : Attribute
	{
		private readonly string _parameterName;
		private readonly Type _parameterType;

		#region Constructors/Disposer
		public ViewParameterAttribute( string parameterName )
		{
			_parameterName = parameterName;
			_parameterType = typeof( string );
		}

		public ViewParameterAttribute( string parameterName, Type parameterType )
		{
			_parameterName = parameterName;
			_parameterType = parameterType;
		}
		#endregion

		public string Name
		{
			get { return _parameterName; }
		}

		public Type Type
		{
			get { return _parameterType; }
		}

		public bool Required { get; set; }
	}
}
