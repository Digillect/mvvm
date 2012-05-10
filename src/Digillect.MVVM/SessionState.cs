using System;
using System.Collections.Generic;

namespace Digillect.Mvvm
{
	/// <summary>
	/// Indicates session state.
	/// </summary>
	public enum SessionState
	{
		/// <summary>
		/// Session is just created.
		/// </summary>
		Created,
		/// <summary>
		/// Session is in the process of loading.
		/// </summary>
		Active,
		/// <summary>
		/// Session completed or failed.
		/// </summary>
		Complete,
		/// <summary>
		/// Session was cancelled.
		/// </summary>
		Canceled,
	}
}
