using System;
using System.Collections.Generic;

namespace Digillect.MVVM
{
	public class SessionAbortedEventArgs : SessionEventArgs
	{
		public Exception Exception { get; private set; }
		public bool Handled { get; set; }

		#region Constructors/Disposer
		public SessionAbortedEventArgs( Session session, Exception exception )
			: base( session )
		{
			this.Exception = exception;
		}
		#endregion
	}
}
