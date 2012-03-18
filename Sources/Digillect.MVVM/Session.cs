using System;
using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;

namespace Digillect.Mvvm
{
	/// <summary>
	/// Holds information about <see cref="Digillect.Mvvm.ViewModel"/>'s loading session.
	/// </summary>
	public class Session : IDisposable
	{
		/// <summary>
		/// Gets the parameters.
		/// </summary>
		public Dictionary<string, object> Parameters { get; private set; }
		/// <summary>
		/// Gets the collection of tasks, associated with this session.
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
		/// Initializes a new instance of the <see cref="Session"/> class.
		/// </summary>
		public Session()
		{
			this.Parameters = new Dictionary<string, object>();
			this.Tasks = new List<Task>();
			this.State = SessionState.Created;
		}

		/// <summary>
		/// Releases unmanaged resources and performs other cleanup operations before the
		/// <see cref="Session"/> is reclaimed by garbage collection.
		/// </summary>
		~Session()
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
		/// Releases unmanaged and - optionally - managed resources
		/// </summary>
		/// <param name="disposing"><c>true</c> to release both managed and unmanaged resources; <c>false</c> to release only unmanaged resources.</param>
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
		/// Gets a value indicating whether cancellation of this session is requested.
		/// </summary>
		/// <value>
		/// 	<c>true</c> if cancellation is requested for this session; otherwise, <c>false</c>.
		/// </value>
		public bool IsCancellationRequested
		{
			get { return tokenSource.IsCancellationRequested; }
		}

		/// <summary>
		/// Gets the <see cref="System.Threading.CancellationToken"/> to be used in asynchronous operations.
		/// </summary>
		public CancellationToken Token
		{
			get { return tokenSource.Token; }
		}

		/// <summary>
		/// Cancels this session.
		/// </summary>
		public void Cancel()
		{
			tokenSource.Cancel();
			State = SessionState.Canceled;
		}
		#endregion
		#region Parameters
		/// <summary>
		/// Gets the parameter.
		/// </summary>
		/// <typeparam name="T">Parameter type.</typeparam>
		/// <param name="name">Parameter name</param>
		/// <returns>Parameter value, casted to <typeparamref name="T"/>.</returns>
		public T GetParameter<T>( string name )
		{
			return (T) this.Parameters[name];
		}

		/// <summary>
		/// Gets the parameter.
		/// </summary>
		/// <typeparam name="T">Parameter type.</typeparam>
		/// <param name="name">Parameter name.</param>
		/// <param name="defaultValue">Default value.</param>
		/// <returns>Parameter value or <paramref name="defaultValue"/> if parameter with corresponding <paramref name="name"/> is not found, casted to <typeparamref name="T"/>.</returns>
		public T GetParameter<T>( string name, T defaultValue )
		{
			if( !this.Parameters.ContainsKey( name ) )
				return defaultValue;

			return (T) this.Parameters[name];
		}
		#endregion
	}
}
