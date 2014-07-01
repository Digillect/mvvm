#region Copyright (c) 2011-2014 Gregory Nickonov and Andrew Nefedkin (Actis® Wunderman)
// Copyright (c) 2011-2014 Gregory Nickonov and Andrew Nefedkin (Actis® Wunderman).
// 
// Permission is hereby granted, free of charge, to any person obtaining a copy of this
// software and associated documentation files (the "Software"), to deal in the Software
// without restriction, including without limitation the rights to use, copy, modify, merge,
// publish, distribute, sublicense, and/or sell copies of the Software, and to permit persons
// to whom the Software is furnished to do so, subject to the following conditions:
// 
// The above copyright notice and this permission notice shall be included in all copies or
// substantial portions of the Software.
// 
// THE SOFTWARE IS PROVIDED "AS IS", WITHOUT WARRANTY OF ANY KIND, EXPRESS OR
// IMPLIED, INCLUDING BUT NOT LIMITED TO THE WARRANTIES OF MERCHANTABILITY,
// FITNESS FOR A PARTICULAR PURPOSE AND NONINFRINGEMENT. IN NO EVENT SHALL
// THE AUTHORS OR COPYRIGHT HOLDERS BE LIABLE FOR ANY CLAIM, DAMAGES OR OTHER
// LIABILITY, WHETHER IN AN ACTION OF CONTRACT, TORT OR OTHERWISE, ARISING FROM,
// OUT OF OR IN CONNECTION WITH THE SOFTWARE OR THE USE OR OTHER DEALINGS
// IN THE SOFTWARE.
#endregion

using System;
using System.Diagnostics.Contracts;

namespace Digillect.Mvvm.UI
{
	/// <summary>
	///     Declares view parameter's name and type.
	/// </summary>
	[AttributeUsage( AttributeTargets.Class, Inherited = true, AllowMultiple = true )]
	public sealed class ViewParameterAttribute : Attribute
	{
		private readonly string _parameterName;
		private readonly Type _parameterType;

		#region Constructors/Disposer
		/// <summary>
		///     Initializes a new instance of the <see cref="ViewParameterAttribute" /> class.
		/// </summary>
		/// <param name="parameterName">Name of the parameter.</param>
		public ViewParameterAttribute( string parameterName )
		{
			Contract.Requires<ArgumentNullException>( !string.IsNullOrEmpty( parameterName ), "parameterName" );

			_parameterName = parameterName;
			_parameterType = typeof( string );

			Required = true;
		}

		/// <summary>
		///     Initializes a new instance of the <see cref="ViewParameterAttribute" /> class.
		/// </summary>
		/// <param name="parameterName">Name of the parameter.</param>
		/// <param name="parameterType">Type of the parameter.</param>
		public ViewParameterAttribute( string parameterName, Type parameterType )
		{
			Contract.Requires<ArgumentNullException>( !string.IsNullOrEmpty( parameterName ), "parameterName" );
			Contract.Requires<ArgumentNullException>( parameterType != null, "parameterType" );

			_parameterName = parameterName;
			_parameterType = parameterType;

			Required = true;
		}
		#endregion

		#region Public Properties
		/// <summary>
		///     Gets the name of the parameter.
		/// </summary>
		/// <value>
		///     The name of the parameter.
		/// </value>
		public string ParameterName
		{
			get { return _parameterName; }
		}

		/// <summary>
		///     Gets the type of the parameter.
		/// </summary>
		/// <value>
		///     The type of the parameter.
		/// </value>
		public Type ParameterType
		{
			get { return _parameterType; }
		}

		/// <summary>
		///     Gets or sets a value indicating whether this parameter is required.
		/// </summary>
		/// <value>
		///     <c>true</c> if required; otherwise, <c>false</c>.
		/// </value>
		public bool Required { get; set; }
		#endregion
	}
}