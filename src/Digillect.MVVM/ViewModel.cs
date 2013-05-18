#region License
// Copyright (c) 2011-2013 Gregory Nickonov and Andrew Nefedkin (Actis Wunderman).
// 
// Permission is hereby granted, free of charge, to any person obtaining a copy of this
// software and associated documentation files (the "Software"), to deal in the Software
// without restriction, including without limitation the rights to use, copy, modify, merge,
// publish, distribute, sublicense, and/or sell copies of the Software, and to permit persons
// to whom the Software is furnished to do so, subject to the following conditions:
// 
// The above copyright notice and this permission notice shall be included in all copies or
// substantial portions of the Software.
// 
// THE SOFTWARE IS PROVIDED "AS IS", WITHOUT WARRANTY OF ANY KIND, EXPRESS OR
// IMPLIED, INCLUDING BUT NOT LIMITED TO THE WARRANTIES OF MERCHANTABILITY,
// FITNESS FOR A PARTICULAR PURPOSE AND NONINFRINGEMENT. IN NO EVENT SHALL
// THE AUTHORS OR COPYRIGHT HOLDERS BE LIABLE FOR ANY CLAIM, DAMAGES OR OTHER
// LIABILITY, WHETHER IN AN ACTION OF CONTRACT, TORT OR OTHERWISE, ARISING FROM,
// OUT OF OR IN CONNECTION WITH THE SOFTWARE OR THE USE OR OTHER DEALINGS
// IN THE SOFTWARE.
#endregion

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
		public const string DefaultAction = "Default";

		private readonly Dictionary<string, ActionRegistration> _actions = new Dictionary<string, ActionRegistration>();
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
		///     Finalizes an instance of the <see cref="ViewModel" /> class.
		/// </summary>
		~ViewModel()
		{
			Dispose( false );
		}

		/// <summary>
		///     Performs application-defined tasks associated with freeing, releasing, or resetting unmanaged resources.
		/// </summary>
		public void Dispose()
		{
			Dispose( true );
			GC.SuppressFinalize( this );
		}

		/// <summary>
		///     Releases unmanaged and - optionally - managed resources.
		/// </summary>
		/// <param name="disposing">
		///     <c>true</c> to release both managed and unmanaged resources; <c>false</c> to release only unmanaged resources.
		/// </param>
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
		///     Creates the session.
		/// </summary>
		/// <returns>
		///     Session.
		/// </returns>
		public Session CreateSession()
		{
			return CreateSession( null, null );
		}

		/// <summary>
		///     Creates the session.
		/// </summary>
		/// <param name="action">Action to process.</param>
		/// <returns>
		///     Session.
		/// </returns>
		public Session CreateSession( string action )
		{
			return CreateSession( null, action );
		}

		/// <summary>
		///     Creates the session.
		/// </summary>
		/// <param name="parameters">Parameters of the session.</param>
		/// <returns>
		///     Session.
		/// </returns>
		public Session CreateSession( XParameters parameters )
		{
			return CreateSession( parameters, null );
		}

		/// <summary>
		///     Creates the session.
		/// </summary>
		/// <param name="parameters">Parameters of the session.</param>
		/// <param name="action">Action to process.</param>
		/// <returns>
		///     Session.
		/// </returns>
		public Session CreateSession( XParameters parameters, string action )
		{
			Contract.Ensures( Contract.Result<Session>() != null );

			return new Session( parameters, action );
		}

		/// <summary>
		///     Loads the specified session.
		/// </summary>
		/// <param name="session">The session to load.</param>
		/// <returns>
		///     <see cref="System.Threading.Tasks.Task{T}" /> that can be awaited.
		/// </returns>
		/// <exception cref="ArgumentNullException">
		///     If <paramref name="session" /> is null.
		/// </exception>
		/// <exception cref="ArgumentException">
		///     If <paramref name="session" /> has been already loaded or canceled.
		/// </exception>
		public async Task<Session> Load( Session session )
		{
			Contract.Requires<ArgumentNullException>( session != null, "session" );

			if( session.State != SessionState.Created )
			{
				throw new ArgumentException( "Invalid session state.", "session" );
			}

			ActionRegistration action;

			if( !_actions.TryGetValue( session.Action, out action ) )
			{
				throw new Exception( "Session with invalid action." );
			}

			if( !action.Validate( session ) )
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

			Task task = null;

			try
			{
				task = action.Process( session );
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

			if( task == null )
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
				await task;

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
		///     Preserves the sessions from being canceled upon disposal.
		/// </summary>
		/// <param name="preserve">
		///     if set to <c>true</c> then sessions will not be cancelled when <see cref="Dispose()" /> method will be called.
		/// </param>
		[EditorBrowsable( EditorBrowsableState.Never )]
		protected void PreserveSessions( bool preserve )
		{
			_preserveSessions = preserve;
		}
		#endregion

		#region Actions
		/// <summary>
		///     Registers the action with default identifier.
		/// </summary>
		/// <returns>Execution group to modify.</returns>
		public IExecutionGroup RegisterAction()
		{
			return RegisterAction( null );
		}

		/// <summary>
		///     Registers the action.
		/// </summary>
		/// <param name="action">Action identifier.</param>
		/// <returns>Execution group to modify.</returns>
		public IExecutionGroup RegisterAction( string action )
		{
			var registration = new ActionRegistration( action ?? DefaultAction );

			_actions[registration.Name] = registration;

			return registration;
		}

		/// <summary>
		/// Extends existing or creates new action with default identifier.
		/// </summary>
		/// <returns>Execution group of existing action or newly created one.</returns>
		public IExecutionGroup ExtendAction()
		{
			return ExtendAction( null );
		}

		/// <summary>
		/// Extends existing or creates new action with specified identifier.
		/// </summary>
		/// <param name="action">Action identifier.</param>
		/// <returns>Execution group of existing action or newly created one.</returns>
		public IExecutionGroup ExtendAction( string action )
		{
			ActionRegistration registration;

			if( !_actions.TryGetValue( action ?? DefaultAction, out registration ) )
			{
				registration = new ActionRegistration( action ?? DefaultAction );

				_actions[registration.Name] = registration;
			}

			return registration;
		}
		#endregion

		#region Nested type: ActionRegistration
		private class ActionRegistration : ExecutionGroup
		{
			private readonly string _name;

			#region Constructors/Disposer
			public ActionRegistration( string name )
				: base( null )
			{
				_name = name;
			}
			#endregion

			#region Public Properties
			public string Name
			{
				get { return _name; }
			}
			#endregion
		}
		#endregion

		#region Nested type: Executable
		private abstract class Executable
		{
			public abstract bool Validate( Session session );
			public abstract Task Process( Session session );
		}
		#endregion

		#region Nested type: ExecutionGroup
		private class ExecutionGroup : Executable, IExecutionGroup
		{
			private readonly List<Executable> _executables;
			private readonly ExecutionGroup _parent;
			private List<Action<Session>> _initializers;
			private List<Action<Session>> _finalizers;

			private bool _isSequential;
			private ValidatorRegistration _validator;

			#region Constructors/Disposer
			protected ExecutionGroup( ExecutionGroup parent )
			{
				_parent = parent;
				_executables = new List<Executable>();
			}
			#endregion

			#region Overrides of Executable
			public override bool Validate( Session session )
			{
				if( _validator != null )
				{
					return _validator.Validate( session );
				}

				return true;
			}

			private async Task ProcessSequentially( Session session )
			{
				foreach( var executable in _executables )
				{
					if( executable.Validate( session ) )
					{
						Task task = executable.Process( session );

						if( task != null && !task.IsCompleted )
						{
							await task;
						}
					}
				}
			}

			private Task ProcessParallel( Session session )
			{
				var tasks = new List<Task>();

				foreach( var executable in _executables )
				{
					if( executable.Validate( session ) )
					{
						Task task = executable.Process( session );

						if( task != null )
						{
							if( task.IsFaulted )
							{
								throw task.Exception;
							}

							tasks.Add( task );
						}
					}
				}

				if( tasks.Count == 0 )
				{
					return null;
				}

				if( tasks.Count == 1 )
				{
					return tasks[0];
				}
				else
				{
#if NET45
					return Task.WhenAll( tasks );
#else
					return TaskEx.WhenAll( tasks );
#endif
				}
			}

			public override async Task Process( Session session )
			{
				if( _initializers != null )
				{
					foreach( var initializer in _initializers )
					{
						initializer( session );
					}
				}

				if( _isSequential )
				{
					await ProcessSequentially( session );
				}
				else
				{
					await ProcessParallel( session );
				}

				if( _finalizers != null )
				{
					foreach( var finalizer in _finalizers )
					{
						finalizer( session );
					}
				}
			}
			#endregion

			#region Implementation of IExecutionGroup
			/// <summary>
			///     Adds new execution group to the current one.
			/// </summary>
			/// <returns>New execution group.</returns>
			public IExecutionGroup AddGroup()
			{
				var group = new ExecutionGroup( this );

				_executables.Add( group );

				return group;
			}

			public IExecutionGroup AddInitializer( Action<Session> initializer )
			{
				Contract.Requires<ArgumentNullException>( initializer != null, "initializer" );

				if( _initializers == null )
				{
					_initializers = new List<Action<Session>>();
				}

				_initializers.Add( initializer );

				return this;
			}

			public IExecutionGroup AddFinalizer( Action<Session> finalizer )
			{
				Contract.Requires<ArgumentNullException>( finalizer != null, "finalizer" );

				if( _finalizers == null )
				{
					_finalizers = new List<Action<Session>>();
				}

				_finalizers.Add( finalizer );

				return this;
			}

			public IExecutionGroup AddPart( Func<Session, Task> processor )
			{
				var part = new Part( processor );

				_executables.Add( part );

				return this;
			}

			public IExecutionGroup AddPart( Func<Session, Task> processor, Func<bool> validator )
			{
				var part = new Part( processor, new ValidatorRegistration( validator ) );

				_executables.Add( part );

				return this;
			}

			public IExecutionGroup AddPart( Func<Session, Task> processor, Func<Session, bool> validator )
			{
				var part = new Part( processor, new ValidatorRegistration( validator ) );

				_executables.Add( part );

				return this;
			}

			public IExecutionGroup AddValidator( Func<bool> validator )
			{
				_validator = new ValidatorRegistration( validator );

				return this;
			}

			public IExecutionGroup AddValidator( Func<Session, bool> validator )
			{
				_validator = new ValidatorRegistration( validator );

				return this;
			}

			public IExecutionGroup Sequential()
			{
				_isSequential = true;

				return _parent;
			}

			public IExecutionGroup Parallel()
			{
				_isSequential = false;

				return _parent;
			}
			#endregion
		}
		#endregion

		#region Nested type: IExecutionGroup
		/// <summary>
		///     Execution group that consists of individual parts and other execution groups. Used to create session execution tree.
		/// </summary>
		public interface IExecutionGroup
		{
			/// <summary>
			///     Adds new execution group to the current one.
			/// </summary>
			/// <returns>New execution group.</returns>
			IExecutionGroup AddGroup();

			/// <summary>
			/// Adds the initializer to execution group.
			/// </summary>
			/// <param name="initializer">The initializer that will be called before executing parts and groups of this group.</param>
			/// <returns>Current execution group.</returns>
			IExecutionGroup AddInitializer( Action<Session> initializer );
			/// <summary>
			/// Adds the finalizer to execution group.
			/// </summary>
			/// <param name="finalizer">The finalizer that will be called after executing parts and groups of this group.</param>
			/// <returns>Current execution group.</returns>
			IExecutionGroup AddFinalizer( Action<Session> finalizer );

			/// <summary>
			///     Adds part to the current execution group.
			/// </summary>
			/// <param name="processor">Processor that will always be executed.</param>
			/// <returns>Current execution group.</returns>
			IExecutionGroup AddPart( Func<Session, Task> processor );

			/// <summary>
			///     Adds part to the current execution group.
			/// </summary>
			/// <param name="processor">Processor that will be executed if validation will pass.</param>
			/// <param name="validator">Validator that will be called prior to processor execution.</param>
			/// <returns>Current execution group.</returns>
			IExecutionGroup AddPart( Func<Session, Task> processor, Func<bool> validator );

			/// <summary>
			///     Adds part to the current execution group.
			/// </summary>
			/// <param name="processor">Processor that will be executed if validation will pass.</param>
			/// <param name="validator">Validator that will be called prior to processor execution.</param>
			/// <returns>Current execution group.</returns>
			IExecutionGroup AddPart( Func<Session, Task> processor, Func<Session, bool> validator );

			/// <summary>
			///     Add validator to the execution group.
			/// </summary>
			/// <param name="validator">Validator that will be called prior to processing execution group.</param>
			/// <returns>Current execution group.</returns>
			IExecutionGroup AddValidator( Func<bool> validator );

			/// <summary>
			///     Add validator to the execution group.
			/// </summary>
			/// <param name="validator">Validator that will be called prior to processing execution group.</param>
			/// <returns>Current execution group.</returns>
			IExecutionGroup AddValidator( Func<Session, bool> validator );

			/// <summary>
			///     Ensures that parts and other groups in this group will be executed sequentially.
			/// </summary>
			/// <returns>
			///     <b>Parent</b> execution group.
			/// </returns>
			IExecutionGroup Sequential();

			/// <summary>
			///     Ensures that parts and other groups in this group will be executed in parallel.
			/// </summary>
			/// <returns>
			///     <b>Parent</b> execution group.
			/// </returns>
			IExecutionGroup Parallel();
		}
		#endregion

		#region Nested type: Part
		private class Part : Executable
		{
			private readonly Func<Session, Task> _processor;
			private readonly ValidatorRegistration _validator;

			#region Constructors/Disposer
			public Part( Func<Session, Task> processor )
				: this( processor, null )
			{
			}

			public Part( Func<Session, Task> processor, ValidatorRegistration validator )
			{
				_validator = validator;
				_processor = processor;
			}
			#endregion

			#region Overrides of Executable
			public override bool Validate( Session session )
			{
				if( _validator != null )
				{
					return _validator.Validate( session );
				}

				return true;
			}

			public override Task Process( Session session )
			{
				if( _processor != null )
				{
					return _processor( session );
				}

				return null;
			}
			#endregion
		}
		#endregion

		#region Nested type: ValidatorRegistration
		private class ValidatorRegistration
		{
			private readonly Func<bool> _v0;
			private readonly Func<Session, bool> _v1;

			#region Constructors/Disposer
			public ValidatorRegistration( Func<bool> v0 )
			{
				_v0 = v0;
			}

			public ValidatorRegistration( Func<Session, bool> v1 )
			{
				_v1 = v1;
			}
			#endregion

			public bool Validate( Session session )
			{
				if( _v1 != null )
				{
					return _v1( session );
				}

				if( _v0 != null )
				{
					return _v0();
				}

				return false;
			}
		}
		#endregion
	}
}