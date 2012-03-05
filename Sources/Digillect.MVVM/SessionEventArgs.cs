using System;
using System.Collections.Generic;

namespace Digillect.MVVM
{
	public class SessionEventArgs : EventArgs
	{
		public Session Session { get; private set; }

		#region Constructors/Disposer
		public SessionEventArgs( Session session )
		{
			this.Session = session;
		}
		#endregion
	}
}
