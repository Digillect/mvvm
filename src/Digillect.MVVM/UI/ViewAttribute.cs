using System;
using System.Collections.Generic;
using System.Linq;

namespace Digillect.Mvvm.UI
{
	/// <summary>
	/// Declares view's name and path.
	/// </summary>
	[AttributeUsage( AttributeTargets.Class, Inherited = false, AllowMultiple = true )]
	public sealed class ViewAttribute : Attribute
	{
		private readonly string _name;
		private readonly string _path;

		#region Constructors/Disposer
		/// <summary>
		/// Initializes a new instance of the <see cref="ViewAttribute" /> class.
		/// </summary>
		public ViewAttribute()
			: this( null, null )
		{
		}

		/// <summary>
		/// Initializes a new instance of the <see cref="ViewAttribute" /> class.
		/// </summary>
		/// <param name="name">Name of the decorated view.</param>
        public ViewAttribute( string name )
			: this( name, null )
		{
		}

		/// <summary>
		/// Initializes a new instance of the <see cref="ViewAttribute" /> class.
		/// </summary>
		/// <param name="name">Name of the decorated view.</param>
		/// <param name="path">System-related path to the decorated view.</param>
        public ViewAttribute( string name, string path )
		{
			_name = name;
			_path = path;
		}
		#endregion

		/// <summary>
		/// Gets the name of the view.
		/// </summary>
		/// <value>
		/// The name.
		/// </value>
		public string Name
		{
			get { return _name; }
		}

		/// <summary>
		/// Gets the path to the view.
		/// </summary>
		/// <value>
		/// The path.
		/// </value>
		public string Path
		{
			get { return _path; }
		}
	}
}
