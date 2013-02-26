using System.Windows.Input;

namespace Digillect.Mvvm
{
	/// <summary>
	///     <see cref="System.Windows.Input.ICommand" /> with event raiser.
	/// </summary>
	public interface IRelayCommand : ICommand
	{
		/// <summary>
		///     Raises event that indicates that <see cref="System.Windows.Input.ICommand.CanExecute" /> return value has been changed.
		/// </summary>
		void RaiseCanExecuteChanged();
	}
}