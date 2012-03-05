﻿using System;
using System.Collections.Generic;
using System.Linq;
using Microsoft.Phone.Controls;
using System.Windows.Controls;
using System.Windows.Controls.Primitives;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows;
using System.Windows.Markup;
using System.Threading;

namespace Digillect.MVVM
{
	public class PhoneApplicationPage : Microsoft.Phone.Controls.PhoneApplicationPage
	{
		private PageDataContext dataContext;

		#region Constructor
		public PhoneApplicationPage()
		{
			this.Language = XmlLanguage.GetLanguage( Thread.CurrentThread.CurrentCulture.Name );

			this.Loaded += Page_Loaded;
			this.Unloaded += Page_Unloaded;
		}
		#endregion

		#region Page Load/Unload events handling
		private void Page_Loaded( object sender, EventArgs e )
		{
			if( !IsInDesignMode )
				OnPageLoaded();
		}

		private void Page_Unloaded( object sender, EventArgs e )
		{
			if( !IsInDesignMode )
				OnPageUnloaded();
		}

		protected virtual void OnPageLoaded()
		{
			dataContext = CreateDataContext();
			this.DataContext = dataContext;

			PageDecorationService.Current.AddDecoration( this );
		}

		protected virtual void OnPageUnloaded()
		{
			PageDecorationService.Current.RemoveDecoration( this );

			if( dataContext != null )
			{
				dataContext.Dispose();
			}
		}

		protected virtual PageDataContext CreateDataContext()
		{
			return new PageDataContext( this );
		}
		#endregion

		#region IsInDesignMode
		private static bool? isInDesignMode;

		public static bool IsInDesignMode
		{
			get
			{
				if( !isInDesignMode.HasValue )
					isInDesignMode = System.ComponentModel.DesignerProperties.IsInDesignTool;

				return isInDesignMode.Value;
			}
		}
		#endregion
	}
}
