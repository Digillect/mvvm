using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using Windows.Foundation;
using Windows.Foundation.Collections;
using Windows.Graphics.Display;
using Windows.UI.ViewManagement;
using Windows.UI.Xaml;
using Windows.UI.Xaml.Controls;
using Windows.UI.Xaml.Controls.Primitives;
using Windows.UI.Xaml.Data;
using Windows.UI.Xaml.Input;
using Windows.UI.Xaml.Media;
using Windows.UI.Xaml.Navigation;

using MetroIoc;

namespace Digillect.Mvvm.UI
{
	/// <summary>
	/// Provides infrastructure for page backed up with <see cref="Digillect.Mvvm.ViewModel"/>.
	/// </summary>
	/// <typeparam name="TViewModel">The type of the view model.</typeparam>
	[Windows.Foundation.Metadata.WebHostHidden]
	public class ViewModelPage<TViewModel> : ViewModelPage
		where TViewModel : ViewModel
	{
		#region Public Properties
		/// <summary>
		/// Gets the view model.
		/// </summary>
		public new TViewModel ViewModel
		{
			get { return (TViewModel) base.ViewModel; }
		}
		#endregion

		#region CreateViewModel
		protected override ViewModel CreateViewModel()
		{
			return this.Container.Resolve<TViewModel>();
		}
		#endregion
	}
}
