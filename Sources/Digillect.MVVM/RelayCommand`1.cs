using System;
using System.Windows.Input;

namespace Digillect.Mvvm
{
	public class RelayCommand<T> : ICommand
	{
		private readonly Action<T> execute;
		private readonly Func<T, bool> canExecute;

		#region Constructors/Disposer
		public RelayCommand( Action<T> execute, Func<T, bool> canExecute = null )
		{
			this.execute = execute;
			this.canExecute = canExecute;
		}
		#endregion

		public event EventHandler CanExecuteChanged;
		
		public void RaiseCanExecuteChanged()
		{
			var handler = CanExecuteChanged;

			if( handler != null )
				handler( this, EventArgs.Empty );
		}

		public bool CanExecute( object parameter )
		{
			return canExecute == null ? true : canExecute( (T) parameter );
		}

		public void Execute( object parameter )
		{
			if( CanExecute( parameter ) )
				execute( (T) parameter );
		}
	}
}
