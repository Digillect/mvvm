using System;
using System.Collections.Generic;

namespace Digillect.MVVM
{
	public class ViewModelFactory
	{
		private static readonly Dictionary<Type, ViewModelInfo> viewModels = new Dictionary<Type, ViewModelInfo>();

		#region Constructors/Disposer
		public ViewModelFactory()
		{
		}

		public static void Cleanup()
		{
			foreach( var viewModel in viewModels.Values )
				if( viewModel.Instance != null )
					viewModel.Instance.Dispose();

			viewModels.Clear();
		}
		#endregion

		#region GetViewModel<T>
		public static T GetViewModel<T>()
			where T: ViewModel, new()
		{
			return (T) GetViewModel( typeof( T ) );
		}

		public static ViewModel GetViewModel( Type type )
		{
			if( type == null )
				throw new ArgumentNullException( "type" );

			if( !typeof( ViewModel ).IsAssignableFrom( type ) )
				throw new ArgumentException( "Invalid ViewModel type.", "type" );

			ViewModelInfo info = null;

			if( !viewModels.ContainsKey( type ) )
			{
				info = new ViewModelInfo()
				{ Type = type };

				var attrs = type.GetCustomAttributes( typeof( SingletonViewModelAttribute ), false );

				info.IsSingleton = attrs != null && attrs.Length == 1;

				viewModels.Add( type, info );
			}
			else
			{
				info = viewModels[type];
			}

			ViewModel viewModel;

			if( info.IsSingleton )
				viewModel = info.Instance;
			else
				viewModel = null;

			if( viewModel == null )
			{
				viewModel = (ViewModel) Activator.CreateInstance( type );

				if( info.IsSingleton )
					info.Instance = viewModel;
			}

			return viewModel;
		}
		#endregion
		#region ReleaseViewModel
		public static void ReleaseViewModel( ViewModel viewModel )
		{
			if( viewModel == null )
				return;

			var type = viewModel.GetType();
			ViewModelInfo info = null;

			viewModels.TryGetValue( type, out info );

			if( info == null )
				return;

			if( !info.IsSingleton )
				viewModel.Dispose();
		}
		#endregion

		private class ViewModelInfo
		{
			public Type Type { get; set; }
			public ViewModel Instance { get; set; }
			public bool IsSingleton { get; set; }
		}
	}
}