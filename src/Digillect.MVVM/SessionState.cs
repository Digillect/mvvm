using System.Diagnostics.CodeAnalysis;

namespace Digillect.Mvvm
{
	/// <summary>
	///     Indicates session state.
	/// </summary>
	[SuppressMessage( "Microsoft.Naming", "CA1724:TypeNamesShouldNotMatchNamespaces" )]
	public enum SessionState
	{
		/// <summary>
		///     Session is just created.
		/// </summary>
		Created,

		/// <summary>
		///     Session is in the process of loading.
		/// </summary>
		Active,

		/// <summary>
		///     Session completed or failed.
		/// </summary>
		Complete,

		/// <summary>
		///     Session was cancelled.
		/// </summary>
		Canceled,
	}
}