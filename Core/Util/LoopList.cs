using System;
using System.Collections;
using System.Collections.Generic;
using System.Threading;

namespace LifeSimulation.Core
{
	public class LoopList<T>: IEnumerable<T>
	{
		
		
		#region Private Variables 
		
		// Locks
		private volatile bool _isIterating = false;
		
		// Data
		private List<T> _items;
		private List<T> _itemsToAdd;
		private List<T> _itemsToRemove;
		private List<T> _removedItems;
		
		
		#endregion
		
		
		
		
		
		
		
		#region Properties 
		
		public int Capacity
		{
			get { return this._items.Capacity; }
		}
		
		public int Count
		{
			get { return this._items.Count; }
		}
		
		public T this [int index]
		{
			get { return this._items [index]; }
		}
		
		#endregion
		
		
		
		
		#region Public Methods
		
		public LoopList ()
		{
			this._items = new List<T>();
			this._itemsToAdd = new List<T>();
			this._itemsToRemove = new List<T>();
			this._removedItems = new List<T>();
		}
		
		public IEnumerator<T> GetEnumerator ()
		{
			doBookKeeping();
			_isIterating = true;
			for(int i = 0; i < _items.Count; i++) {
				yield return _items[i];
			}
			_isIterating = false;
		}
		
		IEnumerator IEnumerable.GetEnumerator ()
		{
			doBookKeeping();
			_isIterating = true;
			for(int i = 0; i < _items.Count; i++) {
				yield return _items[i];
			}
			_isIterating = false;
		}
		
		public void Add (T item)
		{
			if(_isIterating) {
				_itemsToAdd.Add(item);
			} else {
				_items.Add(item);
			}
		}
		
		public bool Remove (T item)
		{
			if(_isIterating) {
				_itemsToRemove.Add(item);
				return true;
			} else {
				return _items.Remove(item);
			}
		}
		
		public void Clear ()
		{
			//TODO:
		}
		
		public bool Contains (T item)
		{
			_isIterating = true;
			bool ret = _items.Contains(item);
			_isIterating = false;
			return ret;
		}
		
		public void ForEach (Action<T> action)
		{
			if (action == null)
			{
				throw new ArgumentNullException ("action");
			}
			doBookKeeping();
			_isIterating = true;
			for (int i = 0; i < this._items.Count; i++)
			{
				action (this._items [i]);
			}
			_isIterating = false;
		}
		
		public int IndexOf (T item)
		{
			_isIterating = true;
			int ret = _items.IndexOf(item);
			_isIterating = false;
			return ret;
		}
		
		public T[] ToArray ()
		{
			_isIterating = true;
			T[] ret = _items.ToArray();
			_isIterating = false;
			return ret;
		}
		
		public void Sync() {
			doBookKeeping();	
		}
		
		#endregion
		
		
		
		
		
		
		#region Private Methods
		
		private bool doBookKeeping() {
			if(_isIterating) {
				return false;	
			} else {
				
				_isIterating = true;
				
				foreach(T item in _itemsToAdd) {
					_items.Add(item);
				}
				_itemsToAdd.Clear();
				
				foreach(T item in _itemsToRemove) {
					_items.Remove(item);
					_removedItems.Add(item);
				}
				_itemsToRemove.Clear();
				
				_isIterating = false;
				
				return true;
			}
		}
		
		#endregion
	}
}

