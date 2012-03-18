using System;

namespace Digillect.Mvvm.Services
{
	/// <summary>
	/// Service that should be used to check network connection availability.
	/// </summary>
	public interface INetworkAvailabilityService
	{
		/// <summary>
		/// Gets a value indicating whether network connection is available.
		/// </summary>
		/// <value>
		///   <c>true</c> if network connection available; otherwise, <c>false</c>.
		/// </value>
		bool NetworkAvailable { get; }

		/// <summary>
		/// Occurs when network availability changed.
		/// </summary>
		event EventHandler NetworkAvailabilityChanged;
	}
}
