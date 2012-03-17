using System;
using System.Windows.Input;

namespace Digillect.Mvvm
{
	public class RelayCommand : ICommand
	{
		private readonly Action execute;
		private readonly Func<bool> canExecute;

		#region Constructors/Disposer
		public RelayCommand( Action execute, Func<bool> canExecute = null )
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
			return canExecute == null ? true : canExecute();
		}

		public void Execute( object parameter )
		{
			if( CanExecute( parameter ) )
				execute();
		}
	}
}
