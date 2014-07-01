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

namespace Digillect.Mvvm.Services
{
	/// <summary>
	///     Default implementation of <see cref="IViewModelExceptionHandlingService" />.
	/// </summary>
	public sealed class DefaultViewModelExceptionHandlingService : IViewModelExceptionHandlingService
	{
		private readonly IExceptionHandlingService _exceptionHandlingService;

		#region Constructors/Disposer
		/// <summary>
		///     Initializes a new instance of the <see cref="DefaultViewModelExceptionHandlingService" /> class.
		/// </summary>
		public DefaultViewModelExceptionHandlingService()
		{
		}

		/// <summary>
		///     Initializes a new instance of the <see cref="DefaultViewModelExceptionHandlingService" /> class.
		/// </summary>
		/// <param name="exceptionHandlingService">The exception handling service.</param>
		public DefaultViewModelExceptionHandlingService( IExceptionHandlingService exceptionHandlingService )
		{
			_exceptionHandlingService = exceptionHandlingService;
		}
		#endregion

		#region IViewModelExceptionHandlingService Members
		/// <summary>
		///     Handle exception the right way.
		/// </summary>
		/// <param name="viewModel">View model that processed session.</param>
		/// <param name="session">Session that caused an exception.</param>
		/// <param name="ex">Exception to handle.</param>
		/// <returns>
		///     <c>true</c> if exception was handled, otherwise <c>false</c>.
		/// </returns>
		public bool HandleException( ViewModel viewModel, Session session, Exception ex )
		{
			if( _exceptionHandlingService != null )
			{
				_exceptionHandlingService.HandleException( ex );

				return true;
			}

			return false;
		}
		#endregion
	}
}