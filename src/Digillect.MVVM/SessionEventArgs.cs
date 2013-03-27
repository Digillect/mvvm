using System;
using System.Diagnostics.Contracts;

namespace Digillect.Mvvm
{
	/// <summary>
	///     Provides data for session events.
	/// </summary>
	public class SessionEventArgs : EventArgs
	{
		private readonly Session _session;

		#region Constructors/Disposer
		/// <summary>
		///     Initializes a new instance of the <see cref="SessionEventArgs" /> class.
		/// </summary>
		/// <param name="session">The session.</param>
		public SessionEventArgs( Session session )
		{
			Contract.Requires<ArgumentNullException>( session != null );

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