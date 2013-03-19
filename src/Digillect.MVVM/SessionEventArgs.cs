using System;

namespace Digillect.Mvvm
{
	/// <summary>
	///     Provides data for session events.
	/// </summary>
	public class SessionEventArgs : EventArgs
	{
		readonly Session _session;

		#region Constructors/Disposer
		/// <summary>
		///     Initializes a new instance of the <see cref="SessionEventArgs" /> class.
		/// </summary>
		/// <param name="session">The session.</param>
		public SessionEventArgs( Session session )
		{
			if( session == null )
			{
				throw new ArgumentNullException( "session" );
			}

			_session = session;
		}
		#endregion

		#region Public Properties
		/// <summary>
		///     Gets the session.
		/// </summary>
		public Session Session
		{
			get { return _session; }
		}
		#endregion
	}
}