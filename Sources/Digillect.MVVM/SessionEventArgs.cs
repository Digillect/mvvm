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
		/// Gets session.
		/// </summary>
		public Session Session { get; private set; }

		#region Constructors/Disposer
		public SessionEventArgs( Session session )
		{
			this.Session = session;
		}
		#endregion
	}
}
