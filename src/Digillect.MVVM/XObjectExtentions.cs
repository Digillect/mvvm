﻿#region Copyright (c) 2011-2014 Gregory Nickonov and Andrew Nefedkin (Actis® Wunderman)
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

namespace Digillect.Mvvm
{
	/// <summary>
	///     Contains the extensions to simplify creation of navigation commands.
	/// </summary>
	public static class XObjectExtensions
	{
		/// <summary>
		///     Create navigation parameters that contains object key.
		/// </summary>
		/// <param name="source">The object to get key from.</param>
		/// <returns>
		///     <see cref="Digillect.XKey" /> that contains entry named <c>key</c> with the value of object's key.
		/// </returns>
		[System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Design", "CA1062:Validate arguments of public methods", MessageId = "0", Justification = "Validated through Code Contracts")]
		public static XParameters KeyParameter( this XObject source )
		{
			Contract.Requires<ArgumentNullException>( source != null, "source" );
			Contract.Ensures( Contract.Result<XParameters>() != null );

			return XParameters.Create( "Key", source.GetKey() );
		}
	}
}