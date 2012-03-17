using System;
using System.Collections.Generic;

namespace Digillect.Mvvm.Services
{
	public interface IPageDecorationService
	{
		void AddDecorator( IPageDecorator decorator );
		void RemoveDecorator( IPageDecorator decorator );
		void AddDecoration( PhoneApplicationPage page );
		void RemoveDecoration( PhoneApplicationPage page );
	}
}
