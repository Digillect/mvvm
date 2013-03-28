using System;
using System.Diagnostics.Contracts;

namespace Digillect.Mvvm
{
	/// <summary>
	/// Delegate-based implementation of <see cref="System.Windows.Input.ICommand"/>.
	/// </summary>
	public class RelayCommand : IRelayCommand
	{
		private readonly Action _execute;
		private readonly Func<bool> _canExecute;

		#region Constructors/Disposer
		/// <summary>
		/// Initializes a new instance of the <see cref="RelayCommand"/> class.
		/// </summary>
		/// <param name="execute">Action that will be used for command execution.</param>
		public RelayCommand( Action execute )
			: this( execute, null )
		{
			Contract.Requires<ArgumentNullException>( execute != null, "execute" );
		}

		/// <summary>
		/// Initializes a new instance of the <see cref="RelayCommand"/> class.
		/// </summary>
		/// <param name="execute">Action that will be used for command execution.</param>
		/// <param name="canExecute">Function that indicates whether command can be executed or not.</param>
		/// <exception cref="ArgumentNullException">If <paramref name="execute"/> is <c>null</c>.</exception>
		public RelayCommand( Action execute, Func<bool> canExecute )
		{
			Contract.Requires<ArgumentNullException>( execute != null, "execute" );

			_execute = execute;
			_canExecute = canExecute;
		}

		#endregion

		/// <summary>
		/// Occurs when changes occur that affect whether or not the command should execute.
		/// </summary>
		public event EventHandler CanExecuteChanged;

		/// <summary>
		/// Raises event that indicates that <see cref="CanExecute"/> return value has been changed.
		/// </summary>
		[System.Diagnostics.CodeAnalysis.SuppressMessage( "Microsoft.Design", "CA1030:UseEventsWhereAppropriate" )]
		public void RaiseCanExecuteChanged()
		{
			var handler = CanExecuteChanged;

			if( handler != null )
			{
				handler( this, EventArgs.Empty );
			}
		}

		/// <summary>
		/// Defines the method that determines whether the command can execute in its current state.
		/// </summary>
		/// <param name="parameter">Data used by the command. Always ignored in this implementation.</param>
		/// <returns>
		/// true if this command can be executed; otherwise, false.
		/// </returns>
		public bool CanExecute( object parameter )
		{
			return _canExecute == null || _canExecute();
		}

		/// <summary>
		/// Defines the method to be called when the command is invoked.
		/// </summary>
		/// <param name="parameter">Data used by the command. Always ignored in this implementation.</param>
		public void Execute( object parameter )
		{
			if( CanExecute( parameter ) )
			{
				_execute();
			}
		}
	}
}
