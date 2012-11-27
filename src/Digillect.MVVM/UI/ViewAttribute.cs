using System;
using System.Collections.Generic;
using System.Linq;

namespace Digillect.Mvvm.UI
{
	[AttributeUsage( AttributeTargets.Class, Inherited = false, AllowMultiple = true )]
	public sealed class ViewAttribute : Attribute
	{
		private readonly string _name;
		private readonly string _path;

		#region Constructors/Disposer
		public ViewAttribute( string name = null, string path = null )
		{
			_name = name;
			_path = path;
		}
		#endregion

		public string Name
		{
			get { return _name; }
		}

		public string Path
		{
			get { return _path; }
		}
	}
}
