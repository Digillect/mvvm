using System;
using System.Diagnostics.CodeAnalysis;
using System.Diagnostics.Contracts;

using Digillect.Mvvm.Services;

namespace Digillect.Mvvm
{
	/// <summary>
	///     Command that performs navigation to the specified view.
	/// </summary>
	public class NavigationCommand<T> : IRelayCommand
	{
		private readonly INavigationService _navigationService;
		private readonly string _view;
		private readonly Func<T, bool> _canNavigate;
		private readonly Func<T, Parameters> _parametersProvider;

		#region Constructors/Disposer
		/// <summary>
		///     Initializes a new instance of the <see cref="NavigationCommand" /> class.
		/// </summary>
		/// <param name="navigationService">Instance of navigation service to be used to perform navigation.</param>
		/// <param name="view">Target view.</param>
		public NavigationCommand( INavigationService navigationService, string view )
			: this( navigationService, view, null, (Func<T, Parameters>) null )
		{
			Contract.Requires<ArgumentNullException>( navigationService != null, "navigationService" );
			Contract.Requires<ArgumentNullException>( view != null, "view" );
		}

		/// <summary>
		///     Initializes a new instance of the <see cref="NavigationCommand" /> class.
		/// </summary>
		/// <param name="navigationService">Instance of navigation service to be used to perform navigation.</param>
		/// <param name="view">Target view.</param>
		/// <param name="canNavigate">Function that determines whether it is possible to navigate to the specified view.</param>
		public NavigationCommand( INavigationService navigationService, string view, Func<T, bool> canNavigate )
			: this( navigationService, view, canNavigate, (Func<T, Parameters>) null )
		{
			Contract.Requires<ArgumentNullException>( navigationService != null, "navigationService" );
			Contract.Requires<ArgumentNullException>( view != null, "view" );
		}

		/// <summary>
		///     Initializes a new instance of the <see cref="NavigationCommand" /> class.
		/// </summary>
		/// <param name="navigationService">Instance of navigation service to be used to perform navigation.</param>
		/// <param name="view">Target view.</param>
		/// <param name="parameters">Parameters to pass to the target view.</param>
		public NavigationCommand( INavigationService navigationService, string view, Parameters parameters )
			: this( navigationService, view, null, item => parameters )
		{
			Contract.Requires<ArgumentNullException>( navigationService != null, "navigationService" );
			Contract.Requires<ArgumentNullException>( view != null, "view" );
		}

		/// <summary>
		///     Initializes a new instance of the <see cref="NavigationCommand" /> class.
		/// </summary>
		/// <param name="navigationService">Instance of navigation service to be used to perform navigation.</param>
		/// <param name="view">Target view.</param>
		/// <param name="canNavigate">Function that determines whether it is possible to navigate to the specified view.</param>
		/// <param name="parameters">Parameters to pass to the target view.</param>
		public NavigationCommand( INavigationService navigationService, string view, Func<T, bool> canNavigate, Parameters parameters )
			: this( navigationService, view, canNavigate, item => parameters )
		{
			Contract.Requires<ArgumentNullException>( navigationService != null, "navigationService" );
			Contract.Requires<ArgumentNullException>( view != null, "view" );
		}

		/// <summary>
		///     Initializes a new instance of the <see cref="NavigationCommand" /> class.
		/// </summary>
		/// <param name="navigationService">Instance of navigation service to be used to perform navigation.</param>
		/// <param name="view">Target view.</param>
		/// <param name="parametersProvider">Function that returns parameters to pass to the target view.</param>
		public NavigationCommand( INavigationService navigationService, string view, Func<T, Parameters> parametersProvider )
			: this( navigationService, view, null, parametersProvider )
		{
			Contract.Requires<ArgumentNullException>( navigationService != null, "navigationService" );
			Contract.Requires<ArgumentNullException>( view != null, "view" );
		}

		/// <summary>
		///     Initializes a new instance of the <see cref="NavigationCommand" /> class.
		/// </summary>
		/// <param name="navigationService">Instance of navigation service to be used to perform navigation.</param>
		/// <param name="view">Target view.</param>
		/// <param name="canNavigate">Function that determines whether it is possible to navigate to the specified view.</param>
		/// <param name="parametersProvider">Function that returns parameters to pass to the target view.</param>
		/// <exception cref="ArgumentNullException">If <paramref name="navigationService"/> or <paramref name="view"/> is <c>null</c>.</exception>
		public NavigationCommand( INavigationService navigationService, string view, Func<T, bool> canNavigate, Func<T, Parameters> parametersProvider )
		{
			Contract.Requires<ArgumentNullException>( navigationService != null, "navigationService" );
			Contract.Requires<ArgumentNullException>( view != null, "view" );

			_navigationService = navigationService;
			_view = view;
			_canNavigate = canNavigate;
			_parametersProvider = parametersProvider;
		}
		#endregion

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
			var handler = CanExecuteChanged;

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
			return _canNavigate == null || _canNavigate( (T) parameter );
		}

		/// <summary>
		///     Defines the method to be called when the command is invoked.
		/// </summary>
		/// <param name="parameter">Data used by the command. Always ignored in this implementation.</param>
		public void Execute( object parameter )
		{
			if( CanExecute( parameter ) )
			{
				Parameters parameters = _parametersProvider == null ? null : _parametersProvider( (T) parameter );

				_navigationService.Navigate( _view, parameters );
			}
		}
	}
}