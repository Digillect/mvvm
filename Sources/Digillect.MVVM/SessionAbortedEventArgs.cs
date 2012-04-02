using System;
using System.Collections.Generic;
using System.Diagnostics.Contracts;

namespace Digillect.Mvvm
{
	/// <summary>
	/// Provides data for session error-reporting event.
	/// </summary>
	public class SessionAbortedEventArgs : SessionEventArgs
	{
		private readonly Exception exception;

		#region Constructors/Disposer
		/// <summary>
		/// Initializes a new instance of the <see cref="SessionAbortedEventArgs"/> class.
		/// </summary>
		/// <param name="session">The session.</param>
		/// <param name="exception">The exception.</param>
		public SessionAbortedEventArgs( Session session, Exception exception )
			: base( session )
		{
			Contract.Requires( session != null );

			this.exception = exception;
		}
		#endregion

		#region Public Properties
		/// <summary>
		/// Gets the exception occured while loading <see cref="Digillect.Mvvm.Session"/>.
		/// </summary>
		public Exception Exception
		{
			get { return this.exception; }
		}

		/// <summary>
		/// Gets or sets a value indicating whether this <see cref="SessionAbortedEventArgs"/> is handled.
		/// </summary>
		/// <value>
		///   <c>true</c> if handled; otherwise, <c>false</c>.
		/// </value>
		public bool Handled { get; set; }
		#endregion
	}
}
