using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;

using Digillect.Mvvm.Services;

namespace Digillect.Mvvm
{
	/// <summary>
	/// Base ViewModel that can be used to build Model-View-ViewModel architecture. All application ViewModels must be descendant of this class.
	/// </summary>
	public abstract class ViewModel : ObservableObject
	{
		#region Public Properties
		/// <summary>
		/// Gets or sets the data exchange service.
		/// </summary>
		/// <value>
		/// The service used to indicate that data exchange is in the progress.
		/// </value>
		public IDataExchangeService DataExchangeService { get; set; }
		#endregion

		#region Data-Exchange notifications
		/// <summary>
		/// Occurs when <see cref="Digillect.Mvvm.Session"/> load is started.
		/// </summary>
		public event EventHandler<SessionEventArgs> SessionStarted;
		/// <summary>
		/// Occurs when <see cref="Digillect.Mvvm.Session"/> load is successfully completed.
		/// </summary>
		public event EventHandler<SessionEventArgs> SessionComplete;
		/// <summary>
		/// Occurs when <see cref="Digillect.Mvvm.Session"/> load was aborted due to unhandled error.
		/// </summary>
		public event EventHandler<SessionAbortedEventArgs> SessionAborted;

		/// <summary>
		/// Raises the session started event.
		/// </summary>
		/// <param name="e">The <see cref="Digillect.Mvvm.SessionEventArgs"/> instance containing the event data.</param>
		protected void RaiseSessionStarted( SessionEventArgs e )
		{
			if( SessionStarted != null )
				SessionStarted( this, e );
		}

		/// <summary>
		/// Raises the session complete event.
		/// </summary>
		/// <param name="e">The <see cref="Digillect.Mvvm.SessionEventArgs"/> instance containing the event data.</param>
		protected void RaiseSessionComplete( SessionEventArgs e )
		{
			if( SessionComplete != null )
				SessionComplete( this, e );
		}

		/// <summary>
		/// Raises the session aborted event.
		/// </summary>
		/// <param name="e">The <see cref="Digillect.Mvvm.SessionAbortedEventArgs"/> instance containing the event data.</param>
		protected void RaiseSessionAborted( SessionAbortedEventArgs e )
		{
			if( SessionAborted != null )
				SessionAborted( this, e );
		}
		#endregion

		#region Loading/Sessions
		private readonly List<Session> m_sessions = new List<Session>();

		/// <summary>
		/// Loads the specified session.
		/// </summary>
		/// <param name="session">The session.</param>
		/// <returns><see cref="System.Threading.Tasks.Task{T}"/> that can be awaited.</returns>
		[EditorBrowsable( EditorBrowsableState.Never )]
		public async Task<Session> Load( Session session )
		{
			if( session == null )
				throw new ArgumentNullException( "session" );

			if( !ShouldLoadSession( session ) )
			{
				session.State = SessionState.Complete;
				return session;
			}

			lock( m_sessions )
			{
				if( session.Exclusive )
				{
					if( m_sessions.Count > 0 )
					{
						m_sessions.ForEach( existingSession => existingSession.Cancel() );

						m_sessions.Clear();
					}
				}

				m_sessions.Add( session );
			}

			RaiseSessionStarted( new SessionEventArgs( session ) );

			if( session.IsCancellationRequested )
			{
				lock( m_sessions )
				{
					m_sessions.Remove( session );
				}

				return session;
			}

			session.State = SessionState.Active;
			DataExchangeService.BeginDataExchange();

			try
			{
				LoadSession( session );
			}
			catch( Exception ex )
			{
				lock( m_sessions )
				{
					m_sessions.Remove( session );
				}

				DataExchangeService.EndDataExchange();

				var canceled = ex is OperationCanceledException;
				var eventArgs = new SessionAbortedEventArgs( session, canceled ? null : ex );

				RaiseSessionAborted( eventArgs );

				if( !canceled && !eventArgs.Handled )
					throw;

				return session;
			}

			if( session.Tasks.Count == 0 )
			{
				lock( m_sessions )
				{
					m_sessions.Remove( session );
				}

				session.State = SessionState.Complete;

				DataExchangeService.EndDataExchange();
				RaiseSessionComplete( new SessionEventArgs( session ) );

				return session;
			}

			try
			{
				await TaskEx.WhenAll( session.Tasks );

				session.State = SessionState.Complete;

				RaiseSessionComplete( new SessionEventArgs( session ) );
			}
			catch( OperationCanceledException )
			{
				var eventArgs = new SessionAbortedEventArgs( session, null );

				RaiseSessionAborted( eventArgs );
			}
			catch( AggregateException aex )
			{
				bool canceled = aex.InnerExceptions.OfType<OperationCanceledException>().Any();
				var eventArgs = new SessionAbortedEventArgs( session, canceled ? null : aex );

				RaiseSessionAborted( eventArgs );

				if( !canceled && !eventArgs.Handled )
					throw;
			}
			finally
			{
				lock( m_sessions )
				{
					m_sessions.Remove( session );
				}

				DataExchangeService.EndDataExchange();
			}

			return session;
		}

		/// <summary>
		/// When overriden checks that the specified session should be loaded or ignored. Default behavior is
		/// to load any session.
		/// </summary>
		/// <param name="session">The session to check.</param>
		/// <returns><c>true</c> if view model should proceed with loading session; otherwise, <c>false</c>.</returns>
		protected virtual bool ShouldLoadSession( Session session )
		{
			return true;
		}

		/// <summary>
		/// Override this method to perform actual session loading.
		/// </summary>
		/// <param name="session">The session.</param>
		protected virtual void LoadSession( Session session )
		{
		}
		#endregion
	}
}