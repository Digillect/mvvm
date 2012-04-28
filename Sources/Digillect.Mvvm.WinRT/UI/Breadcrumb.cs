using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Digillect.Mvvm.UI
{
	internal class Breadcrumb
	{
		private Type type;
		private NavigationParameters parameters;

		#region Constructors/Disposer
		/// <summary>
		/// Initializes a new instance of the Breadcrumb class.
		/// </summary>
		/// <param name="type"></param>
		/// <param name="parameters"></param>
		public Breadcrumb( Type type, NavigationParameters parameters )
		{
			this.type = type;
			this.parameters = parameters;
		}
		#endregion

		#region Public Properties
		public Type Type
		{
			get { return this.type; }
		}

		public NavigationParameters Parameters
		{
			get { return this.parameters; }
		}
		#endregion
	}
}
