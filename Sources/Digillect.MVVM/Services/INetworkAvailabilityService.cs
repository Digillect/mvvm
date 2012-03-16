using System;

namespace Digillect.Mvvm.Services
{
	public interface INetworkAvailabilityService
	{
		bool NetworkAvailable { get; }
		event EventHandler NetworkAvailabilityChanged;
	}
}
