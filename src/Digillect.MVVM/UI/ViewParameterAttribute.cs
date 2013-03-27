using System;
using System.Diagnostics.Contracts;

namespace Digillect.Mvvm.UI
{
	/// <summary>
	/// Declares view parameter's name and type.
	/// </summary>
	[AttributeUsage( AttributeTargets.Class, Inherited = true, AllowMultiple = true )]
	public sealed class ViewParameterAttribute : Attribute
	{
		private readonly string _parameterName;
		private readonly Type _parameterType;

		#region Constructors/Disposer
		/// <summary>
		/// Initializes a new instance of the <see cref="ViewParameterAttribute" /> class.
		/// </summary>
		/// <param name="parameterName">Name of the parameter.</param>
		public ViewParameterAttribute( string parameterName )
		{
			Contract.Requires<ArgumentNullException>( !string.IsNullOrEmpty( parameterName ) );

			_parameterName = parameterName;
			_parameterType = typeof( string );

			Required = true;
		}

		/// <summary>
		/// Initializes a new instance of the <see cref="ViewParameterAttribute" /> class.
		/// </summary>
		/// <param name="parameterName">Name of the parameter.</param>
		/// <param name="parameterType">Type of the parameter.</param>
		public ViewParameterAttribute( string parameterName, Type parameterType )
		{
			Contract.Requires<ArgumentNullException>( !string.IsNullOrEmpty( parameterName ) );
			Contract.Requires<ArgumentNullException>( parameterType != null );

			_parameterName = parameterName;
			_parameterType = parameterType;

			Required = true;
		}
		#endregion

		/// <summary>
		/// Gets the name of the parameter.
		/// </summary>
		/// <value>
		/// The name of the parameter.
		/// </value>
		public string ParameterName
		{
			get { return _parameterName; }
		}

		/// <summary>
		/// Gets the type of the parameter.
		/// </summary>
		/// <value>
		/// The type of the parameter.
		/// </value>
		public Type ParameterType
		{
			get { return _parameterType; }
		}

		/// <summary>
		/// Gets or sets a value indicating whether this parameter is required.
		/// </summary>
		/// <value>
		///   <c>true</c> if required; otherwise, <c>false</c>.
		/// </value>
		public bool Required { get; set; }
	}
}
