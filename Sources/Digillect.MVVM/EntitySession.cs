using System;

namespace Digillect.MVVM
{
	public class EntitySession<TId> : Session
	{
		public const string Entity = "Entity";

		public TId Id { get; private set; }
		public string Part { get; private set; }

		#region Constructors/Disposer
		public EntitySession( TId id, string part = null )
		{
			this.Id = id;
			this.Part = part;
			this.Exclusive = part == null;
		}
		#endregion

		#region Public Properties
		public bool IsPartial
		{
			get { return this.Part != null; }
		}

		public bool IsEntity
		{
			get { return this.Part == null || this.Part == Entity; }
		}
		#endregion

		public bool Includes( string part )
		{
			if( part == null )
				throw new ArgumentNullException( "part" );

			if( this.Part == null )
				return true;

			return this.Part == part;
		}
	}
}
