using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Diagnostics.Contracts;
using System.Linq;
using System.Threading.Tasks;

using Digillect.Mvvm.Services;

namespace Digillect.Mvvm
{
	/// <summary>
	///     Base ViewModel that can be used to build Model-View-ViewModel architecture. All application ViewModels must be descendant of this class.
	/// </summary>
	public class ViewModel : ObservableObject, IDisposable
	{
		private readonly Dictionary<string, PartInfo> _parts = new Dictionary<string, PartInfo>();
		private readonly List<Session> _sessions = new List<Session>();
		private bool _preserveSessions;

		#region Constructors/Disposer
		/// <summary>
		///     Initializes a new instance of the <see cref="ViewModel" /> class.
		/// </summary>
		protected ViewModel()
		{
		}

		/// <summary>
		/// Finalizes an instance of the <see cref="ViewModel" /> class.
		/// </summary>
		~ViewModel()
		{
			Dispose( false );
		}

		/// <summary>
		/// Performs application-defined tasks associated with freeing, releasing, or resetting unmanaged resources.
		/// </summary>
		public void Dispose()
		{
			Dispose( true );
			GC.SuppressFinalize( this );
		}

		/// <summary>
		/// Releases unmanaged and - optionally - managed resources.
		/// </summary>
		/// <param name="disposing"><c>true</c> to release both managed and unmanaged resources; <c>false</c> to release only unmanaged resources.</param>
		protected virtual void Dispose( bool disposing )
		{
			if( disposing )
			{
				if( !_preserveSessions )
				{
					CancelActiveSessions();
				}
			}
		}
		#endregion

		#region Public Properties
		/// <summary>
		///     Gets or sets the data exchange service.
		/// </summary>
		/// <value>
		///     The service used to indicate that data exchange is in the progress.
		/// </value>
		public IDataExchangeService DataExchangeService { get; set; }

		/// <summary>
		///     Gets or sets view model exception handling service.
		/// </summary>
		/// <value>
		///     The service used to report any exceptions occurred while view model loads a session.
		/// </value>
		public IViewModelExceptionHandlingService ViewModelExceptionHandlingService { get; set; }
		#endregion

		#region Data-Exchange notifications
		/// <summary>
		///     Occurs when <see cref="Digillect.Mvvm.Session" /> load is started.
		/// </summary>
		public event EventHandler<SessionEventArgs> SessionStarted;

		/// <summary>
		///     Occurs when <see cref="Digillect.Mvvm.Session" /> load is successfully completed.
		/// </summary>
		public event EventHandler<SessionEventArgs> SessionComplete;

		/// <summary>
		///     Occurs when <see cref="Digillect.Mvvm.Session" /> load was aborted due to unhandled error.
		/// </summary>
		public event EventHandler<SessionAbortedEventArgs> SessionAborted;

		/// <summary>
		///     Raises the session started event.
		/// </summary>
		/// <param name="e">
		///     The <see cref="Digillect.Mvvm.SessionEventArgs" /> instance containing the event data.
		/// </param>
		protected virtual void OnSessionStarted( SessionEventArgs e )
		{
			if( SessionStarted != null )
			{
				SessionStarted( this, e );
			}
		}

		/// <summary>
		///     Raises the session complete event.
		/// </summary>
		/// <param name="e">
		///     The <see cref="Digillect.Mvvm.SessionEventArgs" /> instance containing the event data.
		/// </param>
		protected virtual void OnSessionComplete( SessionEventArgs e )
		{
			if( SessionComplete != null )
			{
				SessionComplete( this, e );
			}
		}

		/// <summary>
		///     Raises the session aborted event.
		/// </summary>
		/// <param name="e">
		///     The <see cref="Digillect.Mvvm.SessionAbortedEventArgs" /> instance containing the event data.
		/// </param>
		protected virtual void OnSessionAborted( SessionAbortedEventArgs e )
		{
			if( SessionAborted != null )
			{
				SessionAborted( this, e );
			}
		}
		#endregion

		#region Loading/Sessions
		/// <summary>
		/// Creates the session with the list of parts to be processed.
		/// </summary>
		/// <param name="parts">Parts to load.</param>
		/// <returns>Session that loads specified parts.</returns>
		public virtual Session CreateSession( params string[] parts )
		{
			Contract.Ensures( Contract.Result<Session>() != null );

			return new Session( parts );
		}

		/// <summary>
		///     Loads the specified session.
		/// </summary>
		/// <param name="session">The session to load.</param>
		/// <returns>
		///     <see cref="System.Threading.Tasks.Task{T}" /> that can be awaited.
		/// </returns>
		/// <exception cref="ArgumentNullException">If <paramref name="session"/> is null.</exception>
		/// <exception cref="ArgumentException">If <paramref name="session"/> has been already loaded or canceled.</exception>
		public async Task<Session> Load( Session session )
		{
			Contract.Requires<ArgumentNullException>( session != null );

			if( session.State != SessionState.Created )
			{
				throw new ArgumentException( "Invalid session state.", "session" );
			}

			if( !ShouldLoadSession( session ) )
			{
				session.State = SessionState.Complete;

				return session;
			}

			lock( _sessions )
			{
				if( session.Exclusive )
				{
					if( _sessions.Count > 0 )
					{
						foreach( var existingSession in _sessions )
						{
							existingSession.Cancel();
						}

						_sessions.Clear();
					}
				}

				_sessions.Add( session );
			}

			OnSessionStarted( new SessionEventArgs( session ) );

			if( session.IsCancellationRequested )
			{
				lock( _sessions )
				{
					_sessions.Remove( session );
				}

				return session;
			}

			session.State = SessionState.Active;

			if( DataExchangeService != null )
			{
				DataExchangeService.BeginDataExchange();
			}

			try
			{
				LoadSession( session );
			}
			catch( Exception ex )
			{
				lock( _sessions )
				{
					_sessions.Remove( session );
				}

				if( DataExchangeService != null )
				{
					DataExchangeService.EndDataExchange();
				}

				var canceled = ex is OperationCanceledException;
				var eventArgs = new SessionAbortedEventArgs( session, canceled ? null : ex );

				OnSessionAborted( eventArgs );

				if( !canceled && !eventArgs.Handled )
				{
					if( ViewModelExceptionHandlingService == null || !ViewModelExceptionHandlingService.HandleException( this, session, ex ) )
					{
						throw;
					}
				}

				return session;
			}

			if( session.Tasks.Count == 0 )
			{
				lock( _sessions )
				{
					_sessions.Remove( session );
				}

				session.State = SessionState.Complete;

				if( DataExchangeService != null )
				{
					DataExchangeService.EndDataExchange();
				}

				OnSessionComplete( new SessionEventArgs( session ) );

				return session;
			}

			try
			{
#if !NET45
				await TaskEx.WhenAll( session.Tasks );
#else
				await Task.WhenAll( session.Tasks );
#endif

				session.State = SessionState.Complete;

				OnSessionComplete( new SessionEventArgs( session ) );
			}
			catch( Exception ex )
			{
				var canceled = ex is OperationCanceledException;
				var eventArgs = new SessionAbortedEventArgs( session, canceled ? null : ex );
				var aggregateException = ex as AggregateException;
				
				if( aggregateException != null )
				{
					canceled = aggregateException.InnerExceptions.OfType<OperationCanceledException>().Any();
				}

				OnSessionAborted( eventArgs );

				if( !canceled && !eventArgs.Handled )
				{
					if( ViewModelExceptionHandlingService == null || !ViewModelExceptionHandlingService.HandleException( this, session, ex ) )
					{
						throw;
					}
				}
			}
			finally
			{
				lock( _sessions )
				{
					_sessions.Remove( session );
				}

				if( DataExchangeService != null )
				{
					DataExchangeService.EndDataExchange();
				}
			}

			return session;
		}

		private void CancelActiveSessions()
		{
			List<Session> sessions;

			lock( _sessions )
			{
				if( _sessions.Count == 0 )
				{
					return;
				}

				sessions = new List<Session>( _sessions );
			}

			foreach( var session in sessions )
			{
				session.Cancel();
			}
		}

		/// <summary>
		/// Preserves the sessions from being canceled upon disposal.
		/// </summary>
		/// <param name="preserve">if set to <c>true</c> then sessions will not be cancelled when <see cref="Dispose()"/> method will be called.</param>
		[EditorBrowsable( EditorBrowsableState.Never )]
		protected void PreserveSessions( bool preserve )
		{
			_preserveSessions = preserve;
		}

		/// <summary>
		///     When overridden checks that the specified session should be loaded or ignored. Default behavior is
		///     to load any session.
		/// </summary>
		/// <param name="session">The session to check.</param>
		/// <returns>
		///     <c>true</c> if view model should proceed with loading session; otherwise, <c>false</c>.
		/// </returns>
		/// <exception cref="ArgumentNullException">If <paramref name="session"/> is null.</exception>
		protected virtual bool ShouldLoadSession( Session session )
		{
			Contract.Requires<ArgumentNullException>( session != null );

			var result = false;

			foreach( var pair in _parts )
			{
				if( (session.Parts == null && pair.Value.LoadIfNoPartsSpecified) || session.Includes( pair.Key ) )
				{
					result |= pair.Value.Check( session, pair.Key );
				}

				if( result )
				{
					break;
				}
			}

			return result;
		}

		/// <summary>
		///     Override this method to perform actual session loading.
		/// </summary>
		/// <param name="session">The session.</param>
		/// <exception cref="ArgumentNullException">If <paramref name="session"/> is null.</exception>
		protected virtual void LoadSession( Session session )
		{
			Contract.Requires<ArgumentNullException>( session != null );

			foreach( var pair in _parts )
			{
				if( (session.Parts == null && pair.Value.LoadIfNoPartsSpecified) || session.Includes( pair.Key ) )
				{
					if( pair.Value.Check( session, pair.Key ) )
					{
						session.Tasks.Add( pair.Value.Process( session, pair.Key ) );
					}
				}
			}
		}
		#endregion

		#region Parts
		/// <summary>
		///     Registers handler of multipart processor.
		/// </summary>
		/// <param name="part">Part identifier.</param>
		/// <param name="processor">Function to load specified part.</param>
		/// <exception cref="ArgumentNullException">If <paramref name="part"/> is null.</exception>
		protected void RegisterPart( string part, Func<Session, string, Task> processor )
		{
			Contract.Requires<ArgumentNullException>( part != null );
			Contract.Requires<ArgumentNullException>( processor != null );

			RegisterPart( part, processor, (Func<Session, string, bool>) null, true );
		}

		/// <summary>
		///     Registers handler of multipart processor.
		/// </summary>
		/// <param name="part">Part identifier.</param>
		/// <param name="processor">Function to load specified part.</param>
		/// <exception cref="ArgumentNullException">If <paramref name="part"/> is null.</exception>
		protected void RegisterPart( string part, Func<Session, Task> processor )
		{
			Contract.Requires<ArgumentNullException>( part != null );
			Contract.Requires<ArgumentNullException>( processor != null );

			RegisterPart( part, processor, (Func<Session, bool>) null, true );
		}

		/// <summary>
		///     Registers handler of multipart processor.
		/// </summary>
		/// <param name="part">Part identifier.</param>
		/// <param name="processor">Function to load specified part.</param>
		/// <param name="loadIfNoPartsSpecified">
		///     <c>true</c> if this part loads when no parts specified for the session.
		/// </param>
		/// <exception cref="ArgumentNullException">If <paramref name="part"/> is null.</exception>
		protected void RegisterPart( string part, Func<Session, string, Task> processor, bool loadIfNoPartsSpecified )
		{
			Contract.Requires<ArgumentNullException>( part != null );
			Contract.Requires<ArgumentNullException>( processor != null );

			RegisterPart( part, processor, (Func<Session, string, bool>) null, loadIfNoPartsSpecified );
		}

		/// <summary>
		///     Registers handler of multipart processor.
		/// </summary>
		/// <param name="part">Part identifier.</param>
		/// <param name="processor">Function to load specified part.</param>
		/// <param name="loadIfNoPartsSpecified">
		///     <c>true</c> if this part loads when no parts specified for the session.
		/// </param>
		/// <exception cref="ArgumentNullException">If <paramref name="part"/> is null.</exception>
		protected void RegisterPart( string part, Func<Session, Task> processor, bool loadIfNoPartsSpecified )
		{
			Contract.Requires<ArgumentNullException>( part != null );
			Contract.Requires<ArgumentNullException>( processor != null );

			RegisterPart( part, processor, (Func<Session, bool>) null, loadIfNoPartsSpecified );
		}

		/// <summary>
		///     Registers handler of multipart processor.
		/// </summary>
		/// <param name="part">Part identifier.</param>
		/// <param name="processor">Function to load specified part.</param>
		/// <param name="checker">Function to check if the specified part should be loaded.</param>
		/// <exception cref="ArgumentNullException">If <paramref name="part"/> is null.</exception>
		protected void RegisterPart( string part, Func<Session, string, Task> processor, Func<Session, string, bool> checker )
		{
			Contract.Requires<ArgumentNullException>( part != null );
			Contract.Requires<ArgumentNullException>( processor != null );

			RegisterPart( part, processor, checker, true );
		}

		/// <summary>
		///     Registers handler of multipart processor.
		/// </summary>
		/// <param name="part">Part identifier.</param>
		/// <param name="processor">Function to load specified part.</param>
		/// <param name="checker">Function to check if the specified part should be loaded.</param>
		/// <exception cref="ArgumentNullException">If <paramref name="part"/> is null.</exception>
		protected void RegisterPart( string part, Func<Session, Task> processor, Func<Session, string, bool> checker )
		{
			Contract.Requires<ArgumentNullException>( part != null );
			Contract.Requires<ArgumentNullException>( processor != null );

			RegisterPart( part, processor, checker, true );
		}

		/// <summary>
		///     Registers handler of multipart processor.
		/// </summary>
		/// <param name="part">Part identifier.</param>
		/// <param name="processor">Function to load specified part.</param>
		/// <param name="checker">Function to check if the specified part should be loaded.</param>
		/// <exception cref="ArgumentNullException">If <paramref name="part"/> is null.</exception>
		protected void RegisterPart( string part, Func<Session, string, Task> processor, Func<Session, bool> checker )
		{
			Contract.Requires<ArgumentNullException>( part != null );
			Contract.Requires<ArgumentNullException>( processor != null );

			RegisterPart( part, processor, checker, true );
		}

		/// <summary>
		///     Registers handler of multipart processor.
		/// </summary>
		/// <param name="part">Part identifier.</param>
		/// <param name="processor">Function to load specified part.</param>
		/// <param name="checker">Function to check if the specified part should be loaded.</param>
		/// <exception cref="ArgumentNullException">If <paramref name="part"/> is null.</exception>
		protected void RegisterPart( string part, Func<Session, Task> processor, Func<Session, bool> checker )
		{
			Contract.Requires<ArgumentNullException>( part != null );
			Contract.Requires<ArgumentNullException>( processor != null );

			RegisterPart( part, processor, checker, true );
		}

		/// <summary>
		///     Registers handler of multipart processor.
		/// </summary>
		/// <param name="part">Part identifier.</param>
		/// <param name="processor">Function to load specified part.</param>
		/// <param name="checker">Function to check if the specified part should be loaded.</param>
		/// <param name="loadIfNoPartsSpecified">
		///     <c>true</c> if this part loads when no parts specified for the session.
		/// </param>
		/// <exception cref="ArgumentNullException">If <paramref name="part"/> is null.</exception>
		protected void RegisterPart( string part, Func<Session, string, Task> processor, Func<Session, string, bool> checker, bool loadIfNoPartsSpecified )
		{
			Contract.Requires<ArgumentNullException>( part != null );
			Contract.Requires<ArgumentNullException>( processor != null );

			_parts[part] = new PartInfo
			{
				ProcessorWithPart = processor,
				CheckerWithPart = checker,
				LoadIfNoPartsSpecified = loadIfNoPartsSpecified
			};
		}

		/// <summary>
		///     Registers handler of multipart processor.
		/// </summary>
		/// <param name="part">Part identifier.</param>
		/// <param name="processor">Function to load specified part.</param>
		/// <param name="checker">Function to check if the specified part should be loaded.</param>
		/// <param name="loadIfNoPartsSpecified">
		///     <c>true</c> if this part loads when no parts specified for the session.
		/// </param>
		/// <exception cref="ArgumentNullException">If <paramref name="part"/> is null.</exception>
		protected void RegisterPart( string part, Func<Session, Task> processor, Func<Session, string, bool> checker, bool loadIfNoPartsSpecified )
		{
			Contract.Requires<ArgumentNullException>( part != null );
			Contract.Requires<ArgumentNullException>( processor != null );

			_parts[part] = new PartInfo
			{
				Processor = processor,
				CheckerWithPart = checker,
				LoadIfNoPartsSpecified = loadIfNoPartsSpecified
			};
		}

		/// <summary>
		///     Registers handler of multipart processor.
		/// </summary>
		/// <param name="part">Part identifier.</param>
		/// <param name="processor">Function to load specified part.</param>
		/// <param name="checker">Function to check if the specified part should be loaded.</param>
		/// <param name="loadIfNoPartsSpecified">
		///     <c>true</c> if this part loads when no parts specified for the session.
		/// </param>
		/// <exception cref="ArgumentNullException">If <paramref name="part"/> is null.</exception>
		protected void RegisterPart( string part, Func<Session, string, Task> processor, Func<Session, bool> checker, bool loadIfNoPartsSpecified )
		{
			Contract.Requires<ArgumentNullException>( part != null );
			Contract.Requires<ArgumentNullException>( processor != null );

			_parts[part] = new PartInfo
			{
				ProcessorWithPart = processor,
				Checker = checker,
				LoadIfNoPartsSpecified = loadIfNoPartsSpecified
			};
		}

		/// <summary>
		///     Registers handler of multipart processor.
		/// </summary>
		/// <param name="part">Part identifier.</param>
		/// <param name="processor">Function to load specified part.</param>
		/// <param name="checker">Function to check if the specified part should be loaded.</param>
		/// <param name="loadIfNoPartsSpecified">
		///     <c>true</c> if this part loads when no parts specified for the session.
		/// </param>
		/// <exception cref="ArgumentNullException">If <paramref name="part"/> is null.</exception>
		protected void RegisterPart( string part, Func<Session, Task> processor, Func<Session, bool> checker, bool loadIfNoPartsSpecified )
		{
			Contract.Requires<ArgumentNullException>( part != null );
			Contract.Requires<ArgumentNullException>( processor != null );

			_parts[part] = new PartInfo
			{
				Processor = processor,
				Checker = checker,
				LoadIfNoPartsSpecified = loadIfNoPartsSpecified
			};
		}

		private class PartInfo
		{
			public Func<Session, string, Task> ProcessorWithPart { get; set; }
			public Func<Session, string, bool> CheckerWithPart { get; set; }
			public Func<Session, Task> Processor { get; set; }
			public Func<Session, bool> Checker { get; set; }
			public bool LoadIfNoPartsSpecified { get; set; }

			public bool Check( Session session, string part )
			{
				if( Checker != null )
				{
					return Checker( session );
				}

				if( CheckerWithPart != null )
				{
					return CheckerWithPart( session, part );
				}

				return true;
			}

			public Task Process( Session session, string part )
			{
				if( Processor != null )
				{
					return Processor( session );
				}
				else
				{
					return ProcessorWithPart( session, part );
				}
			}
		}
		#endregion
	}
}