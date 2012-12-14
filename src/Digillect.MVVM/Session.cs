using System;
using System.Collections.Generic;
using System.Diagnostics.Contracts;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;

namespace Digillect.Mvvm
{
	/// <summary>
	/// Holds information about <see cref="Digillect.Mvvm.ViewModel"/>'s loading session.
	/// </summary>
	public class Session : IDisposable
	{
		private readonly Parameters _parameters = new Parameters();
		private readonly List<Task> _tasks = new List<Task>();
		private readonly CancellationTokenSource _tokenSource = new CancellationTokenSource();
		private readonly string[] _parts;

		#region Constructors/Disposer
		/// <summary>
		/// Initializes a new instance of the <see cref="Session"/> class.
		/// </summary>
		public Session()
		{
			State = SessionState.Created;
		}

		public Session( params string[] parts )
		{
			_parts = parts;
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
				_tokenSource.Dispose();
			}
		}
		#endregion

		#region Public Properties
		/// <summary>
		/// Gets the parameters.
		/// </summary>
		public Parameters Parameters
		{
			get { return _parameters; }
		}

		/// <summary>
		/// Gets the collection of tasks, associated with this session.
		/// </summary>
		public IList<Task> Tasks
		{
			get { return _tasks; }
		}
		/// <summary>
		/// Gets logical part for multipart requests.
		/// </summary>
		public IEnumerable<string> Parts
		{
			get { return _parts; }
		}
		/// <summary>
		/// Gets a value indicating whether this instance is partial.
		/// </summary>
		/// <value>
		/// 	<c>true</c> if this instance is partial; otherwise, <c>false</c>.
		/// </value>
		public bool IsPartial
		{
			get { return _parts != null && _parts.Length > 0; }
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
			get { return _tokenSource.IsCancellationRequested; }
		}

		/// <summary>
		/// Gets the <see cref="System.Threading.CancellationToken"/> to be used in asynchronous operations.
		/// </summary>
		public CancellationToken Token
		{
			get { return _tokenSource.Token; }
		}

		/// <summary>
		/// Cancels this session.
		/// </summary>
		public void Cancel()
		{
			_tokenSource.Cancel();
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
		[Obsolete( "Use Parameters.Get<T>() instead." )]
		public T GetParameter<T>( string name )
		{
			return _parameters.Get<T>( name );
		}

		/// <summary>
		/// Gets the parameter.
		/// </summary>
		/// <typeparam name="T">Parameter type.</typeparam>
		/// <param name="name">Parameter name.</param>
		/// <param name="defaultValue">Default value.</param>
		/// <returns>Parameter value or <paramref name="defaultValue"/> if parameter with corresponding <paramref name="name"/> is not found, casted to <typeparamref name="T"/>.</returns>
		[Obsolete( "Use Parameters.GetParameter<T>() instead." )]
		public T GetParameter<T>( string name, T defaultValue )
		{
			return _parameters.Get<T>( name, defaultValue );
		}

		public Session AddParameter( string name, object value )
		{
			_parameters.Add( name, value );

			return this;
		}
		#endregion
		#region Parts
		/// <summary>
		/// Checks that session is used to load specified logical part.
		/// </summary>
		/// <param name="part">Part to check, can't be <c>null</c>.</param>
		/// <returns><c>true</c> if specified part is loading; otherwise, <c>false</c>.</returns>
		/// <exception cref="System.ArgumentNullException">if part is <c>null</c>.</exception>
		public bool Includes( string part )
		{
			if( part == null )
				throw new ArgumentNullException( "part" );

			return _parts.Contains( part );
		}
		#endregion
	}
}
