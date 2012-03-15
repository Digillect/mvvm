using System;

namespace Digillect.Mvvm
{
	public interface IPageDecorator
	{
		void AddDecoration( PhoneApplicationPage page );
		void RemoveDecoration( PhoneApplicationPage page );
	}
}
