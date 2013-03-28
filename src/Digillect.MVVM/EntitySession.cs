#region Copyright (c) 2011-2013 Gregory Nickonov and Andrew Nefedkin (Actis® Wunderman)
// Copyright (c) 2011-2013 Gregory Nickonov and Andrew Nefedkin (Actis® Wunderman).
// 
// Permission is hereby granted, free of charge, to any person obtaining a copy of this
// software and associated documentation files (the "Software"), to deal in the Software
// without restriction, including without limitation the rights to use, copy, modify, merge,
// publish, distribute, sublicense, and/or sell copies of the Software, and to permit persons
// to whom the Software is furnished to do so, subject to the following conditions:
// 
// The above copyright notice and this permission notice shall be included in all copies or
// substantial portions of the Software.
// 
// THE SOFTWARE IS PROVIDED "AS IS", WITHOUT WARRANTY OF ANY KIND, EXPRESS OR
// IMPLIED, INCLUDING BUT NOT LIMITED TO THE WARRANTIES OF MERCHANTABILITY,
// FITNESS FOR A PARTICULAR PURPOSE AND NONINFRINGEMENT. IN NO EVENT SHALL
// THE AUTHORS OR COPYRIGHT HOLDERS BE LIABLE FOR ANY CLAIM, DAMAGES OR OTHER
// LIABILITY, WHETHER IN AN ACTION OF CONTRACT, TORT OR OTHERWISE, ARISING FROM,
// OUT OF OR IN CONNECTION WITH THE SOFTWARE OR THE USE OR OTHER DEALINGS
// IN THE SOFTWARE.
#endregion

using System;
using System.Diagnostics.CodeAnalysis;
using System.Diagnostics.Contracts;

namespace Digillect.Mvvm
{
	/// <summary>
	///     Represents session to be used to load entity into <see cref="Digillect.Mvvm.EntityViewModel{TEntity}" />.
	/// </summary>
	public class EntitySession : Session
	{
		private readonly XKey _key;

		#region Constructors/Disposer
		/// <summary>
		///     Initializes a new instance of the <see cref="EntitySession" /> class.
		/// </summary>
		/// <param name="key">Entity key.</param>
		public EntitySession( XKey key )
		{
			Contract.Requires<ArgumentNullException>( key != null, "key" );

			_key = key;
			Exclusive = true;
		}

		/// <summary>
		///     Initializes a new instance of the <see cref="EntitySession" /> class.
		/// </summary>
		/// <param name="key">Entity key.</param>
		/// <param name="parts">Specifies what part(s) of multipart entity to load.</param>
		public EntitySession( XKey key, params string[] parts )
			: base( parts )
		{
			Contract.Requires<ArgumentNullException>( key != null, "key" );

			_key = key;
			Exclusive = parts == null;
		}
		#endregion

		#region Public Properties
		/// <summary>
		///     Gets entity identifier.
		/// </summary>
		public XKey Key
		{
			get { return _key; }
		}
		#endregion

		#region Parameters
		/// <summary>
		///     Adds the parameter to session parameters.
		/// </summary>
		/// <param name="name">Parameter name.</param>
		/// <param name="value">Parameter value.</param>
		/// <returns>Current session.</returns>
		public new EntitySession AddParameter( string name, object value )
		{
			Contract.Requires<ArgumentNullException>( name != null, "name" );
			Contract.Requires<ArgumentNullException>( value != null, "value" );
			Contract.Ensures( Contract.Result<EntitySession>() != null );

			Parameters.Add( name, value );

			return this;
		}
		#endregion

		#region ObjectInvariant
		[ContractInvariantMethod]
		[SuppressMessage( "Microsoft.Performance", "CA1822:MarkMembersAsStatic", Justification = "Required for code contracts." )]
		private void ObjectInvariant()
		{
			Contract.Invariant( Key != null );
		}
		#endregion
	}
}