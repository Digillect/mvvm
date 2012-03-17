using System;
using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;

namespace Digillect.Mvvm
{
	/// <summary>
	/// Holds information about <see cref="ViewModel"/>'s loading session.
	/// </summary>
	public class Session : IDisposable
	{
		/// <summary>
		/// Gets dictionary of session key-value organized parameters.
		/// </summary>
		public Dictionary<string, object> Parameters { get; private set; }
		/// <summary>
		/// Gets collection of tasks that should be finished for the session to complete.
		/// </summary>
		public List<Task> Tasks { get; private set; }
		/// <summary>
		/// Gets or sets flag indicating that all other sessions should be terminated when this one
		/// begins to load.
		/// </summary>
		public bool Exclusive { get; set; }
		/// <summary>
		/// Gets session state
		/// </summary>
		public SessionState State { get; internal set; }

		private CancellationTokenSource tokenSource = new CancellationTokenSource();

		#region Constructors/Disposer
		/// <summary>
		/// Creates new session.
		/// </summary>
		public Session()
		{
			this.Parameters = new Dictionary<string, object>();
			this.Tasks = new List<Task>();
			this.State = SessionState.Created;
		}

		~Session()
		{
			Dispose( false );
		}

		public void Dispose()
		{
			Dispose( true );
			GC.SuppressFinalize( this );
		}

		protected virtual void Dispose( bool disposing )
		{
			if( disposing )
			{
				if( tokenSource != null )
				{
					tokenSource.Dispose();
					tokenSource = null;
				}
			}
		}
		#endregion

		#region Cancellation Support
		/// <summary>
		/// Gets value indicating that session cancellation was requested.
		/// </summary>
		public bool IsCancellationRequested
		{
			get { return tokenSource.IsCancellationRequested; }
		}

		/// <summary>
		/// Gets cancellation token to be used in asynchronous operations.
		/// </summary>
		public CancellationToken Token
		{
			get { return tokenSource.Token; }
		}

		/// <summary>
		/// Requests session cancellation.
		/// </summary>
		public void Cancel()
		{
			tokenSource.Cancel();
			State = SessionState.Canceled;
		}
		#endregion
		#region Parameters
		/// <summary>
		/// Gets parameter from session's parameters dictionary.
		/// </summary>
		/// <typeparam name="T">Type of the parameter</typeparam>
		/// <param name="name">Parameter's name</param>
		/// <returns>Parameter converted to specified type.</returns>
		public T GetParameter<T>( string name )
		{
			return (T) this.Parameters[name];
		}

		/// <summary>
		/// Gets parameter from session's parameters dictionary or default value, if parameter doesn't exists.
		/// </summary>
		/// <typeparam name="T">Type of the parameter</typeparam>
		/// <param name="name">Parameter's name</param>
		/// <param name="defaultValue">Default value to be used if parameter doesn't exists.</param>
		/// <returns>Parameter's value or default value.</returns>
		public T GetParameter<T>( string name, T defaultValue )
		{
			if( !this.Parameters.ContainsKey( name ) )
				return defaultValue;

			return (T) this.Parameters[name];
		}
		#endregion
	}
}
