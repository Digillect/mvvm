#region Copyright (c) 2011-2014 Gregory Nickonov and Andrew Nefedkin (Actis® Wunderman)
// Copyright (c) 2011-2014 Gregory Nickonov and Andrew Nefedkin (Actis® Wunderman).
// 
// Permission is hereby granted, free of charge, to any person obtaining a copy of this
// software and associated documentation files (the "Software"), to deal in the Software
// without restriction, including without limitation the rights to use, copy, modify, merge,
// publish, distribute, sublicense, and/or sell copies of the Software, and to permit persons
// to whom the Software is furnished to do so, subject to the following conditions:
// 
// The above copyright notice and this permission notice shall be included in all copies or
// substantial portions of the Software.
// 
// THE SOFTWARE IS PROVIDED "AS IS", WITHOUT WARRANTY OF ANY KIND, EXPRESS OR
// IMPLIED, INCLUDING BUT NOT LIMITED TO THE WARRANTIES OF MERCHANTABILITY,
// FITNESS FOR A PARTICULAR PURPOSE AND NONINFRINGEMENT. IN NO EVENT SHALL
// THE AUTHORS OR COPYRIGHT HOLDERS BE LIABLE FOR ANY CLAIM, DAMAGES OR OTHER
// LIABILITY, WHETHER IN AN ACTION OF CONTRACT, TORT OR OTHERWISE, ARISING FROM,
// OUT OF OR IN CONNECTION WITH THE SOFTWARE OR THE USE OR OTHER DEALINGS
// IN THE SOFTWARE.
#endregion

using System;
using System.Diagnostics.CodeAnalysis;
using System.Diagnostics.Contracts;

namespace Digillect.Mvvm
{
	/// <summary>
	///     Delegate-based implementation of <see cref="System.Windows.Input.ICommand" />.
	/// </summary>
	public class RelayCommand : IRelayCommand
	{
		private readonly Func<bool> _canExecute;
		private readonly Action _execute;

		#region Constructors/Disposer
		/// <summary>
		///     Initializes a new instance of the <see cref="RelayCommand" /> class.
		/// </summary>
		/// <param name="execute">Action that will be used for command execution.</param>
		public RelayCommand( Action execute )
			: this( execute, null )
		{
			Contract.Requires<ArgumentNullException>( execute != null, "execute" );
		}

		/// <summary>
		///     Initializes a new instance of the <see cref="RelayCommand" /> class.
		/// </summary>
		/// <param name="execute">Action that will be used for command execution.</param>
		/// <param name="canExecute">Function that indicates whether command can be executed or not.</param>
		/// <exception cref="ArgumentNullException">
		///     If <paramref name="execute" /> is <c>null</c>.
		/// </exception>
		public RelayCommand( Action execute, Func<bool> canExecute )
		{
			Contract.Requires<ArgumentNullException>( execute != null, "execute" );

			_execute = execute;
			_canExecute = canExecute;
		}
		#endregion

		#region IRelayCommand Members
		/// <summary>
		///     Occurs when changes occur that affect whether or not the command should execute.
		/// </summary>
		public event EventHandler CanExecuteChanged;

		/// <summary>
		///     Raises event that indicates that <see cref="CanExecute" /> return value has been changed.
		/// </summary>
		[SuppressMessage( "Microsoft.Design", "CA1030:UseEventsWhereAppropriate" )]
		public void RaiseCanExecuteChanged()
		{
			EventHandler handler = CanExecuteChanged;

			if( handler != null )
			{
				handler( this, EventArgs.Empty );
			}
		}

		/// <summary>
		///     Defines the method that determines whether the command can execute in its current state.
		/// </summary>
		/// <param name="parameter">Data used by the command. Always ignored in this implementation.</param>
		/// <returns>
		///     true if this command can be executed; otherwise, false.
		/// </returns>
		public bool CanExecute( object parameter )
		{
			return _canExecute == null || _canExecute();
		}

		/// <summary>
		///     Defines the method to be called when the command is invoked.
		/// </summary>
		/// <param name="parameter">Data used by the command. Always ignored in this implementation.</param>
		public void Execute( object parameter )
		{
			if( CanExecute( parameter ) )
			{
				_execute();
			}
		}
		#endregion
	}
}