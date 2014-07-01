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
	///     Service that is used to indicate that data exchange is in the progress.
	/// </summary>
	public interface IDataExchangeService
	{
		#region Public Properties
		/// <summary>
		///     Gets the number of active data exchange operations.
		/// </summary>
		int DataExchangeCount { get; }
		#endregion

		#region Events and event raisers
		/// <summary>
		///     Occurs when first data exchange operation begins.
		/// </summary>
		event EventHandler DataExchangeStarted;

		/// <summary>
		///     Occurs when last data exchange operation ends.
		/// </summary>
		event EventHandler DataExchangeComplete;
		#endregion

		/// <summary>
		///     Informs that another data exchange operation has begun.
		/// </summary>
		void BeginDataExchange();

		/// <summary>
		///     Informs that one of the data exchange operations has ended.
		/// </summary>
		void EndDataExchange();
	}
}