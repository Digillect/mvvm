using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;

namespace Digillect.Mvvm
{
	public class ViewModel : ObservableObject, IDisposable
	{
		#region Constructors/Disposer
		protected ViewModel()
		{
		}

		public void Dispose()
		{
			Dispose( true );
			GC.SuppressFinalize( this );
		}

		protected virtual void Dispose( bool disposing )
		{
		}
		#endregion

		#region Data-Exchange notifications
		public event EventHandler<SessionEventArgs> SessionStarted;
		public event EventHandler<SessionEventArgs> SessionComplete;
		public event EventHandler<SessionAbortedEventArgs> SessionAborted;

		protected void RaiseSessionStarted( SessionEventArgs e )
		{
			if( SessionStarted != null )
				SessionStarted( this, e );
		}

		protected void RaiseSessionComplete( SessionEventArgs e )
		{
			if( SessionComplete != null )
				SessionComplete( this, e );
		}

		protected void RaiseSessionAborted( SessionAbortedEventArgs e )
		{
			if( SessionAborted != null )
				SessionAborted( this, e );
		}
		#endregion

		#region Activate/Deactivate
		public bool Active { get; private set; }

		public void Activate()
		{
			if( !this.Active )
			{
				this.Active = true;
				OnActivated();
			}
		}

		public void Deactivate()
		{
			if( this.Active )
			{
				OnDeactivated();
				this.Active = false;
			}
		}

		protected virtual void OnActivated() {}
		protected virtual void OnDeactivated()
		{
			/*
			if( m_sessions.Count > 0 )
			{
				lock( m_sessions )
				{
					foreach( var session in m_sessions )
						session.Cancel();

					m_sessions.Clear();
				}
			}
			*/
		}
		#endregion
		#region Loading/Sessions
		private readonly List<Session> m_sessions = new List<Session>();

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
						foreach( var existingSession in m_sessions )
							existingSession.Cancel();

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
			NetworkExchangeService.Current.BeginDataExchange();

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

				NetworkExchangeService.Current.EndDataExchange();

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

				NetworkExchangeService.Current.EndDataExchange();
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

				NetworkExchangeService.Current.EndDataExchange();
			}

			return session;
		}

		protected virtual bool ShouldLoadSession( Session session )
		{
			return true;
		}

		protected virtual void LoadSession( Session session )
		{
		}
		#endregion
	}
}