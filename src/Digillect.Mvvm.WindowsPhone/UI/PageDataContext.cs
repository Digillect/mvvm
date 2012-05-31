﻿using System;
using System.Diagnostics.Contracts;
using System.Windows;

using Digillect.Mvvm.Services;

namespace Digillect.Mvvm.UI
{
	/// <summary>
	/// Instances of this class are used by MVVM infrastructure to support data binding.
	/// </summary>
	public class PageDataContext : ObservableObject, IDisposable
	{
		private readonly PhoneApplicationPage page;

		#region Constructors/Disposer
		/// <summary>
		/// Initializes a new instance of the <see cref="PageDataContext"/> class.
		/// </summary>
		/// <param name="page">The page used in this context.</param>
		public PageDataContext( PhoneApplicationPage page )
		{
			Contract.Requires( page != null );

			this.page = page;
		}

		/// <summary>
		/// Releases unmanaged resources and performs other cleanup operations before the
		/// <see cref="PageDataContext"/> is reclaimed by garbage collection.
		/// </summary>
		~PageDataContext()
		{
			Dispose( false );
			GC.SuppressFinalize( this );
		}

		/// <summary>
		/// Performs application-defined tasks associated with freeing, releasing, or resetting unmanaged resources.
		/// </summary>
		public void Dispose()
		{
			Dispose( true );
			GC.SuppressFinalize( this );
		}

		/// <summary>
		/// Releases unmanaged and - optionally - managed resources
		/// </summary>
		/// <param name="disposing"><c>true</c> to release both managed and unmanaged resources; <c>false</c> to release only unmanaged resources.</param>
		protected virtual void Dispose( bool disposing )
		{
		}
		#endregion

		#region Public Properties
		/// <summary>
		/// Gets the page.
		/// </summary>
		public PhoneApplicationPage Page
		{
			get { return this.page; }
		}
		#endregion
	}
}