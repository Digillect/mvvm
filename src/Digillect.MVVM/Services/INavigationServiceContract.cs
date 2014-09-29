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

namespace Digillect.Mvvm.Services
{
	[ContractClassFor( typeof( INavigationService ) )]
	internal abstract class INavigationServiceContract : INavigationService
	{
		#region Implementation of INavigationService
		public void Navigate( string viewName )
		{
			Contract.Requires<ArgumentNullException>( viewName != null, "viewName" );
		}

		public void Navigate( string viewName, XParameters parameters )
		{
			Contract.Requires<ArgumentNullException>( viewName != null, "viewName" );
		}

		public abstract void GoBack();

        	public void GoBack( string viewName )
        	{
        	    Contract.Requires<ArgumentNullException>( viewName != null, "viewName" );
        	}
		#endregion
	}
}