using System;
using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;

namespace Digillect.MVVM
{
	public class Session : IDisposable
	{
		public Dictionary<string, object> Parameters { get; private set; }
		public List<Task> Tasks { get; private set; }
		public bool Exclusive { get; set; }
		public SessionState State { get; internal set; }

		private CancellationTokenSource tokenSource = new CancellationTokenSource();

		#region Constructors/Disposer
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
		public bool IsCancellationRequested
		{
			get { return tokenSource.IsCancellationRequested; }
		}

		public CancellationToken Token
		{
			get { return tokenSource.Token; }
		}

		public void Cancel()
		{
			tokenSource.Cancel();
			State = SessionState.Canceled;
		}
		#endregion
		#region Parameters
		public T GetParameter<T>( string name )
		{
			return (T) this.Parameters[name];
		}

		public T GetParameter<T>( string name, T defaultValue )
		{
			if( !this.Parameters.ContainsKey( name ) )
				return defaultValue;

			return (T) this.Parameters[name];
		}
		#endregion
	}
}
