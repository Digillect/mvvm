using System;
using System.Collections.Generic;
using System.Linq;

using Windows.Foundation.Collections;

namespace Digillect.Mvvm.UI
{
	/// <summary>
	/// Instances of this class are used by MVVM infrastructure to support data binding.
	/// </summary>
	public class PageDataContext : ObservableObject, IDisposable
	{
		public delegate PageDataContext Factory( Page page );

		private readonly Page page;
		private readonly ObservableDictionary values = new ObservableDictionary();

		#region Constructors/Disposer
		/// <summary>
		/// Initializes a new instance of the <see cref="PageDataContext"/> class.
		/// </summary>
		/// <param name="page">The page used in this context.</param>
		public PageDataContext( Page page )
		{
			this.page = page;
			this.values["Loaded"] = false;
		}

		/// <summary>
		/// Releases unmanaged resources and performs other cleanup operations before the
		/// <see cref="PageDataContext"/> is reclaimed by garbage collection.
		/// </summary>
		~PageDataContext()
		{
			Dispose( false );
			GC.SuppressFinalize( this );
		}

		/// <summary>
		/// Performs application-defined tasks associated with freeing, releasing, or resetting unmanaged resources.
		/// </summary>
		public void Dispose()
		{
			Dispose( true );
			GC.SuppressFinalize( this );
		}

		/// <summary>
		/// Releases unmanaged and - optionally - managed resources
		/// </summary>
		/// <param name="disposing"><c>true</c> to release both managed and unmanaged resources; <c>false</c> to release only unmanaged resources.</param>
		protected virtual void Dispose( bool disposing )
		{
		}
		#endregion

		#region Public Properties
		/// <summary>
		/// Gets the page.
		/// </summary>
		public Page Page
		{
			get { return this.page; }
		}

		public IObservableMap<string, object> Values
		{
			get { return this.values; }
		}
		#endregion

		#region ObservableDictionary
		private class ObservableDictionary : IObservableMap<string, object>
		{
			private readonly Dictionary<string, object> dictionary = new Dictionary<string, object>();

			public event MapChangedEventHandler<string, object> MapChanged;

			private void InvokeMapChanged( CollectionChange change, string key )
			{
				var eventHandler = MapChanged;
				if( eventHandler != null )
				{
					eventHandler( this, new ObservableDictionaryChangedEventArgs( CollectionChange.ItemInserted, key ) );
				}
			}

			public void Add( string key, object value )
			{
				this.dictionary.Add( key, value );
				this.InvokeMapChanged( CollectionChange.ItemInserted, key );
			}

			public void Add( KeyValuePair<string, object> item )
			{
				this.Add( item.Key, item.Value );
			}

			public bool Remove( string key )
			{
				if( this.dictionary.Remove( key ) )
				{
					this.InvokeMapChanged( CollectionChange.ItemRemoved, key );
					return true;
				}

				return false;
			}

			public bool Remove( KeyValuePair<string, object> item )
			{
				object currentValue;

				if( this.dictionary.TryGetValue( item.Key, out currentValue ) &&
					Object.Equals( item.Value, currentValue ) && this.dictionary.Remove( item.Key ) )
				{
					this.InvokeMapChanged( CollectionChange.ItemRemoved, item.Key );
					return true;
				}

				return false;
			}

			public object this[string key]
			{
				get { return this.dictionary[key]; }
				set
				{
					this.dictionary[key] = value;
					this.InvokeMapChanged( CollectionChange.ItemChanged, key );
				}
			}

			public void Clear()
			{
				var priorKeys = this.dictionary.Keys.ToArray();

				this.dictionary.Clear();

				foreach( var key in priorKeys )
				{
					this.InvokeMapChanged( CollectionChange.ItemRemoved, key );
				}
			}

			public ICollection<string> Keys
			{
				get { return this.dictionary.Keys; }
			}

			public bool ContainsKey( string key )
			{
				return this.dictionary.ContainsKey( key );
			}

			public bool TryGetValue( string key, out object value )
			{
				return this.dictionary.TryGetValue( key, out value );
			}

			public ICollection<object> Values
			{
				get { return this.dictionary.Values; }
			}

			public bool Contains( KeyValuePair<string, object> item )
			{
				return this.dictionary.Contains( item );
			}

			public int Count
			{
				get { return this.dictionary.Count; }
			}

			public bool IsReadOnly
			{
				get { return false; }
			}

			public IEnumerator<KeyValuePair<string, object>> GetEnumerator()
			{
				return this.dictionary.GetEnumerator();
			}

			System.Collections.IEnumerator System.Collections.IEnumerable.GetEnumerator()
			{
				return this.dictionary.GetEnumerator();
			}

			public void CopyTo( KeyValuePair<string, object>[] array, int arrayIndex )
			{
				int arraySize = array.Length;

				foreach( var pair in this.dictionary )
				{
					if( arrayIndex >= arraySize ) break;
					array[arrayIndex++] = pair;
				}
			}

			#region private class ObservableDictionaryChangedEventArgs
			private class ObservableDictionaryChangedEventArgs : IMapChangedEventArgs<string>
			{
				public ObservableDictionaryChangedEventArgs( CollectionChange change, string key )
				{
					this.CollectionChange = change;
					this.Key = key;
				}

				public CollectionChange CollectionChange { get; private set; }
				public string Key { get; private set; }
			}
			#endregion
		}
		#endregion
	}
}