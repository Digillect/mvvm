using System;
using System.Collections.Generic;
using System.Diagnostics.Contracts;

namespace Digillect.Mvvm
{
	/// <summary>
	/// Provides data for session events.
	/// </summary>
	public class SessionEventArgs : EventArgs
	{
		private readonly Session session;

		#region Constructors/Disposer
		/// <summary>
		/// Initializes a new instance of the <see cref="SessionEventArgs"/> class.
		/// </summary>
		/// <param name="session">The session.</param>
		public SessionEventArgs( Session session )
		{
			if( session == null )
				throw new ArgumentNullException( "session" );

			this.session = session;
		}
		#endregion

		#region Public Properties
		/// <summary>
		/// Gets the session.
		/// </summary>
		public Session Session
		{
			get { return this.session; }
		}
		#endregion
	}
}
