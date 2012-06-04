using System;
using System.Collections.Generic;
using System.Diagnostics.Contracts;
using System.Threading;
using System.Threading.Tasks;

namespace Digillect.Mvvm
{
	/// <summary>
	/// Holds information about <see cref="Digillect.Mvvm.ViewModel"/>'s loading session.
	/// </summary>
	public class Session : IDisposable
	{
		private readonly Dictionary<string, object> parameters = new Dictionary<string, object>();
		private readonly List<Task> tasks = new List<Task>();
		private readonly CancellationTokenSource tokenSource = new CancellationTokenSource();

		#region Constructors/Disposer
		/// <summary>
		/// Initializes a new instance of the <see cref="Session"/> class.
		/// </summary>
		public Session()
		{
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
				this.tokenSource.Dispose();
			}
		}
		#endregion

		#region Public Properties
		/// <summary>
		/// Gets the parameters.
		/// </summary>
		public IDictionary<string, object> Parameters
		{
			get { return this.parameters; }
		}

		/// <summary>
		/// Gets the collection of tasks, associated with this session.
		/// </summary>
		public IList<Task> Tasks
		{
			get { return this.tasks; }
		}

		/// <summary>
		/// Gets or sets flag indicating that all other sessions should be terminated when this one
		/// begins to load.
		/// </summary>
		public bool Exclusive { get; set; }
		/// <summary>
		/// Gets session state
		/// </summary>
		public SessionState State { get; internal set; }
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
			get { return this.tokenSource.IsCancellationRequested; }
		}

		/// <summary>
		/// Gets the <see cref="System.Threading.CancellationToken"/> to be used in asynchronous operations.
		/// </summary>
		public CancellationToken Token
		{
			get { return this.tokenSource.Token; }
		}

		/// <summary>
		/// Cancels this session.
		/// </summary>
		public void Cancel()
		{
			this.tokenSource.Cancel();
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
			if( string.IsNullOrEmpty( name ) )
				throw new ArgumentNullException( "name", "Parameter can't be null or empty string." );

			return (T) this.parameters[name];
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

		public Session AddParameter( string name, object value )
		{
			this.Parameters[name] = value;

			return this;
		}
		#endregion
	}
}
