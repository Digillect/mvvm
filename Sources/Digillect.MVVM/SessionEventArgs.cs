using System;
using System.Collections.Generic;

namespace Digillect.Mvvm
{
	/// <summary>
	/// Provides data for session events.
	/// </summary>
	public class SessionEventArgs : EventArgs
	{
		/// <summary>
		/// Gets the session.
		/// </summary>
		public Session Session { get; private set; }

		#region Constructors/Disposer
		/// <summary>
		/// Initializes a new instance of the <see cref="SessionEventArgs"/> class.
		/// </summary>
		/// <param name="session">The session.</param>
		public SessionEventArgs( Session session )
		{
			this.Session = session;
		}
		#endregion
	}
}
