using System;
using System.Diagnostics.Contracts;
using System.Windows.Input;

namespace Digillect.Mvvm
{
	/// <summary>
	/// Delegate-based implementation of <see cref="System.Windows.Input.ICommand"/>.
	/// </summary>
	/// <typeparam name="T">Type of the parameter used by execute and canExecute delegates.</typeparam>
	public class RelayCommand<T> : ICommand
	{
		private readonly Action<T> execute;
		private readonly Func<T, bool> canExecute;

		#region Constructors/Disposer
		/// <summary>
		/// Initializes a new instance of the <see cref="RelayCommand&lt;T&gt;"/> class.
		/// </summary>
		/// <param name="execute">Action that will be used for command execution.</param>
		/// <param name="canExecute">Function that indicates whether command can be executed or not.</param>
		public RelayCommand( Action<T> execute, Func<T, bool> canExecute = null )
		{
			if( execute == null )
				throw new ArgumentNullException( "execute" );

			this.execute = execute;
			this.canExecute = canExecute;
		}
		#endregion

		/// <summary>
		/// Occurs when changes occur that affect whether or not the command should execute.
		/// </summary>
		public event EventHandler CanExecuteChanged;

		/// <summary>
		/// Raises event that indicates that <see cref="CanExecute"/> return value has been changed.
		/// </summary>
		public void RaiseCanExecuteChanged()
		{
			var handler = CanExecuteChanged;

			if( handler != null )
				handler( this, EventArgs.Empty );
		}

		/// <summary>
		/// Defines the method that determines whether the command can execute in its current state.
		/// </summary>
		/// <param name="parameter">Data used by the command. If the command does not require data to be passed, this object can be set to null.</param>
		/// <returns>
		/// true if this command can be executed; otherwise, false.
		/// </returns>
		public bool CanExecute( object parameter )
		{
			return canExecute == null ? true : canExecute( (T) parameter );
		}

		/// <summary>
		/// Defines the method to be called when the command is invoked.
		/// </summary>
		/// <param name="parameter">Data used by the command. If the command does not require data to be passed, this object can be set to null.</param>
		public void Execute( object parameter )
		{
			if( CanExecute( parameter ) )
				execute( (T) parameter );
		}
	}
}
