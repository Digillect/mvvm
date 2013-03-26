using System;

namespace Digillect.Mvvm.UI
{
	/// <summary>
	///     Specifies that attributed class is a view.
	/// </summary>
	[AttributeUsage( AttributeTargets.Class, Inherited = true, AllowMultiple = true )]
	public sealed class ViewAttribute : Attribute
	{
		readonly string _name;

		#region Constructors/Disposer
		/// <summary>
		///     Initializes a new instance of the <see cref="ViewAttribute" /> class.
		/// </summary>
		public ViewAttribute()
		{
		}

		/// <summary>
		///     Initializes a new instance of the <see cref="ViewAttribute" /> class.
		/// </summary>
		/// <param name="name">Name of the attributed view.</param>
		public ViewAttribute( string name )
		{
			_name = name;
		}
		#endregion

		#region Public Properties
		/// <summary>
		///     Gets the name of the view.
		/// </summary>
		public string Name
		{
			get { return _name; }
		}
		#endregion
	}
}