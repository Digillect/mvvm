using System;

namespace Digillect.Mvvm.UI
{
	/// <summary>
	///     Declares view's name and path.
	/// </summary>
	[AttributeUsage( AttributeTargets.Class, Inherited = true, AllowMultiple = true )]
	public sealed class ViewAttribute : Attribute
	{
		readonly string _name;
		readonly string _path;

		#region Constructors/Disposer
		/// <summary>
		///     Initializes a new instance of the <see cref="ViewAttribute" /> class.
		/// </summary>
		public ViewAttribute()
			: this( null, null )
		{
		}

		/// <summary>
		///     Initializes a new instance of the <see cref="ViewAttribute" /> class.
		/// </summary>
		/// <param name="name">Name of the decorated view.</param>
		public ViewAttribute( string name )
			: this( name, null )
		{
		}

		/// <summary>
		///     Initializes a new instance of the <see cref="ViewAttribute" /> class.
		/// </summary>
		/// <param name="name">Name of the decorated view.</param>
		/// <param name="path">System-related path to the decorated view.</param>
		public ViewAttribute( string name, string path )
		{
			_name = name;
			_path = path;
		}
		#endregion

		#region Public Properties
		/// <summary>
		///     Gets the name of the view.
		/// </summary>
		/// <value>
		///     The name.
		/// </value>
		public string Name
		{
			get { return _name; }
		}

		/// <summary>
		///     Gets the path to the view.
		/// </summary>
		/// <value>
		///     The path.
		/// </value>
		public string Path
		{
			get { return _path; }
		}

		/// <summary>
		/// Gets or sets a value indicating whether authentication is required prior to the navigation to this view.
		/// </summary>
		/// <value>
		/// <c>true</c> if authentication is required; otherwise, <c>false</c>.
		/// </value>
		public bool AuthenticationRequired { get; set; }

		/// <summary>
		/// Gets or sets a value indicating whether this view is a part of authentication flow.
		/// </summary>
		/// <value>
		/// <c>true</c> if the view is part of authentication flow; otherwise, <c>false</c>.
		/// </value>
		public bool PartOfAuthentication { get; set; }
		#endregion
	}
}