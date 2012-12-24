using System;
using System.Collections.Generic;
using System.Diagnostics.Contracts;
using System.Linq;
using Windows.Foundation.Collections;

namespace Digillect.Mvvm.UI
{
	/// <summary>
	/// Instances of this class are used by MVVM infrastructure to support data binding.
	/// </summary>
	public class PageDataContext : ObservableObject, IDisposable
	{
		/// <summary>
		/// Factory delegate to create instance of this class through Autofac.
		/// </summary>
		/// <param name="page">The page.</param>
		/// <returns>Instance of context.</returns>
		[System.Diagnostics.CodeAnalysis.SuppressMessage( "Microsoft.Design", "CA1034:NestedTypesShouldNotBeVisible" )]
		public delegate PageDataContext Factory( Page page );

		private readonly Page _page;
		private readonly ObservableDictionary _values = new ObservableDictionary();

		#region Constructors/Disposer
		/// <summary>
		/// Initializes a new instance of the <see cref="PageDataContext"/> class.
		/// </summary>
		/// <param name="page">The page used in this context.</param>
		public PageDataContext( Page page )
		{
			_page = page;
			_values["Loaded"] = false;
		}

		/// <summary>
		/// Releases unmanaged resources and performs other cleanup operations before the
		/// <see cref="PageDataContext"/> is reclaimed by garbage collection.
		/// </summary>
		~PageDataContext()
		{
			Dispose( false );
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
			get { return _page; }
		}

		/// <summary>
		/// Gets the values bag.
		/// </summary>
		/// <value>
		/// The values.
		/// </value>
		public IObservableMap<string, object> Values
		{
			get { return _values; }
		}
		#endregion

		#region ObservableDictionary
		private class ObservableDictionary : IObservableMap<string, object>
		{
			private readonly Dictionary<string, object> _dictionary = new Dictionary<string, object>();

			public event MapChangedEventHandler<string, object> MapChanged;

			private void InvokeMapChanged( CollectionChange change, string key )
			{
				var eventHandler = MapChanged;

				if( eventHandler != null )
				{
					eventHandler( this, new ObservableDictionaryChangedEventArgs( change, key ) );
				}
			}

			public void Add( string key, object value )
			{
				_dictionary.Add( key, value );
				InvokeMapChanged( CollectionChange.ItemInserted, key );
			}

			public void Add( KeyValuePair<string, object> item )
			{
				Add( item.Key, item.Value );
			}

			public bool Remove( string key )
			{
				if( _dictionary.Remove( key ) )
				{
					InvokeMapChanged( CollectionChange.ItemRemoved, key );
					return true;
				}

				return false;
			}

			public bool Remove( KeyValuePair<string, object> item )
			{
				object currentValue;

				if( _dictionary.TryGetValue( item.Key, out currentValue ) &&
					Object.Equals( item.Value, currentValue ) && _dictionary.Remove( item.Key ) )
				{
					InvokeMapChanged( CollectionChange.ItemRemoved, item.Key );
					return true;
				}

				return false;
			}

			public object this[string key]
			{
				get { return _dictionary[key]; }
				set
				{
					_dictionary[key] = value;
					InvokeMapChanged( CollectionChange.ItemChanged, key );
				}
			}

			public void Clear()
			{
				var priorKeys = _dictionary.Keys.ToArray();

				_dictionary.Clear();

				foreach( var key in priorKeys )
				{
					InvokeMapChanged( CollectionChange.ItemRemoved, key );
				}
			}

			public ICollection<string> Keys
			{
				get { return _dictionary.Keys; }
			}

			public bool ContainsKey( string key )
			{
				var result = _dictionary.ContainsKey( key );

				Contract.Assume( !result || (Count > 0) );

				return result;
			}

			public bool TryGetValue( string key, out object value )
			{
				return _dictionary.TryGetValue( key, out value );
			}

			public ICollection<object> Values
			{
				get { return _dictionary.Values; }
			}

			public bool Contains( KeyValuePair<string, object> item )
			{
				var result = _dictionary.Contains( item );

				Contract.Assume( !result || (Count > 0) );

				return result;
			}

			public int Count
			{
				get { return _dictionary.Count; }
			}

			public bool IsReadOnly
			{
				get { return false; }
			}

			public IEnumerator<KeyValuePair<string, object>> GetEnumerator()
			{
				return _dictionary.GetEnumerator();
			}

			System.Collections.IEnumerator System.Collections.IEnumerable.GetEnumerator()
			{
				return _dictionary.GetEnumerator();
			}

			public void CopyTo( KeyValuePair<string, object>[] array, int arrayIndex )
			{
				if( array == null )
				{
					throw new ArgumentNullException( "array" );
				}

				Contract.EndContractBlock();

				var arraySize = array.Length;

				foreach( var pair in _dictionary )
				{
					if( arrayIndex >= arraySize )
					{
						break;
					}
					array[arrayIndex++] = pair;
				}
			}

			#region private class ObservableDictionaryChangedEventArgs
			private class ObservableDictionaryChangedEventArgs : IMapChangedEventArgs<string>
			{
				public ObservableDictionaryChangedEventArgs( CollectionChange change, string key )
				{
					CollectionChange = change;
					Key = key;
				}

				public CollectionChange CollectionChange { get; private set; }
				public string Key { get; private set; }
			}
			#endregion
		}
		#endregion
	}
}
