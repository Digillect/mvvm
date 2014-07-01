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
using System.Collections.Generic;
using System.Diagnostics.Contracts;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;

namespace Digillect.Mvvm
{
	/// <summary>
	///     Holds information about <see cref="Digillect.Mvvm.ViewModel" />'s loading session.
	/// </summary>
	public class Session : IDisposable
	{
		private readonly string _action;
		private readonly CancellationTokenSource _tokenSource = new CancellationTokenSource();
		private readonly Dictionary<string, object> _values = new Dictionary<string, object>();
		private XParameters _parameters = XParameters.Empty;

		#region Constructors/Disposer
		/// <summary>
		///     Initializes a new instance of the <see cref="Session" /> class.
		/// </summary>
		public Session()
			: this( XParameters.Empty, null )
		{
		}

		/// <summary>
		/// Initializes a new instance of the <see cref="Session"/> class.
		/// </summary>
		/// <param name="parameters">Parameters of the session.</param>
		public Session( XParameters parameters )
			: this( parameters, null )
		{
		}

		/// <summary>
		/// Initializes a new instance of the <see cref="Session"/> class.
		/// </summary>
		/// <param name="action">Action to process.</param>
		public Session( string action )
			: this( XParameters.Empty, action )
		{
		}

		/// <summary>
		/// Initializes a new instance of the <see cref="Session" /> class.
		/// </summary>
		/// <param name="parameters">Parameters of the session.</param>
		/// <param name="action">Action to process.</param>
		public Session( XParameters parameters, string action )
		{
			_parameters = parameters ?? XParameters.Empty;
			_action = action ?? ViewModel.DefaultAction;

			State = SessionState.Created;
		}

		/// <summary>
		///     Releases unmanaged resources and performs other cleanup operations before the
		///     <see cref="Session" /> is reclaimed by garbage collection.
		/// </summary>
		~Session()
		{
			Dispose( false );
		}

		/// <summary>
		///     Performs application-defined tasks associated with freeing, releasing, or resetting unmanaged resources.
		/// </summary>
		public void Dispose()
		{
			Dispose( true );
			GC.SuppressFinalize( this );
		}

		/// <summary>
		///     Releases unmanaged and - optionally - managed resources
		/// </summary>
		/// <param name="disposing">
		///     <c>true</c> to release both managed and unmanaged resources; <c>false</c> to release only unmanaged resources.
		/// </param>
		protected virtual void Dispose( bool disposing )
		{
			if( disposing )
			{
				_tokenSource.Dispose();
			}
		}
		#endregion

		#region Public Properties
		/// <summary>
		///     Gets the parameters.
		/// </summary>
		public XParameters Parameters
		{
			get { return _parameters; }
		}

		/// <summary>
		/// Gets the action associated with this session.
		/// </summary>
		/// <value>
		/// The action.
		/// </value>
		public string Action
		{
			get { return _action; }
		}

		/// <summary>
		/// Gets the dictionary used as name-value storage.
		/// </summary>
		/// <value>
		/// The values.
		/// </value>
		public IDictionary<string, object> Values
		{
			get { return _values; }
		}

		/// <summary>
		///     Gets or sets flag indicating that all other sessions should be terminated when this one
		///     begins to load.
		/// </summary>
		public bool Exclusive { get; set; }

		/// <summary>
		///     Gets session state
		/// </summary>
		public SessionState State { get; internal set; }
		#endregion

		#region Cancellation Support
		/// <summary>
		///     Gets a value indicating whether cancellation of this session is requested.
		/// </summary>
		/// <value>
		///     <c>true</c> if cancellation is requested for this session; otherwise, <c>false</c>.
		/// </value>
		public bool IsCancellationRequested
		{
			get { return _tokenSource.IsCancellationRequested; }
		}

		/// <summary>
		///     Gets the <see cref="System.Threading.CancellationToken" /> to be used in asynchronous operations.
		/// </summary>
		public CancellationToken Token
		{
			get { return _tokenSource.Token; }
		}

		/// <summary>
		///     Cancels this session.
		/// </summary>
		public void Cancel()
		{
			if( State == SessionState.Active || State == SessionState.Created )
			{
				SessionState oldState = State;

				State = SessionState.Canceled;

				if( oldState == SessionState.Active )
				{
					_tokenSource.Cancel();
				}
			}
		}
		#endregion

		#region Parameters
		/// <summary>
		///     Adds the parameter value to the current session.
		/// </summary>
		/// <param name="name">Parameter name.</param>
		/// <param name="value">Parameter value.</param>
		/// <returns>Current session.</returns>
		public Session AddParameter( string name, object value )
		{
			Contract.Requires<ArgumentNullException>( name != null, "name" );
			Contract.Requires<ArgumentNullException>( value != null, "value" );
			Contract.Ensures( Contract.Result<Session>() != null );

			_parameters = _parameters.WithValue( name, value );

			return this;
		}
		#endregion
	}
}