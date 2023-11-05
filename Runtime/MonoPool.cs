using System.Collections.Generic;
using UnityEngine;

namespace Shafir.MonoPool
{
    /// <summary>
    /// Pool of objects
    /// </summary>
    public static class MonoPool
    {
        // Dictionary with all pooled objects
        private static Dictionary<IPoolable, List<IPoolable>> _pool = new Dictionary<IPoolable, List<IPoolable>>();

        // Default parent for all poolable objects
        private static Transform _parent;

        /// <summary>
        /// Fill pool with objects
        /// </summary>
        /// <param name="prefab">Goal prefab</param>
        /// <param name="count">Count of prefabs to instantiate</param>
        /// <param name="parent">Container for instantiated prefabs</param>
        public static void Fill<T>(T prefab, int count, Transform parent) where T : MonoBehaviour, IPoolable
        {
            FillInternal(prefab, count, parent);
        }

        /// <summary>
        /// Fill pool with objects
        /// </summary>
        /// <param name="prefab">Goal prefab</param>
        /// <param name="count">Count of prefabs to instantiate</param>
        public static void Fill<T>(T prefab, int count) where T : MonoBehaviour, IPoolable
        {
            FillInternal(prefab, count, _parent);
        }

        // internal method to fill pool
        private static void FillInternal<T>(T prefab, int count, Transform parent) where T : MonoBehaviour, IPoolable
        {
            // if there is no item in dictionary - creates new list
            if (!_pool.ContainsKey(prefab)) _pool.Add(prefab, new List<IPoolable>());

            for (int i = 0; i < count; i++)
            {
                // create new item and deactivates it
                T item = CreateItem(prefab, parent);
                item.Deactivate();
            }
        }

        /// <summary>
        /// Get object from pool
        /// </summary>
        /// <param name="prefab">Goal prefab</param>
        /// <returns>Object from pool</returns>
        public static T Get<T>(T prefab) where T : MonoBehaviour, IPoolable
        {
            return GetInternal(prefab, _parent);
        }

        /// <summary>
        /// Get object from pool and set parent
        /// </summary>
        /// <param name="prefab">Goal prefab</param>
        /// <param name="parent">Goal parent for pooled object</param>
        /// <returns>Object from pool</returns>
        public static T Get<T>(T prefab, Transform parent) where T : MonoBehaviour, IPoolable
        {
            return GetInternal(prefab, parent);
        }

        // Internal method for getting object from pool
        private static T GetInternal<T>(T prefab, Transform parent) where T : MonoBehaviour, IPoolable
        {
            T result;

            // if where is no item in dictionary - create new list
            if (!_pool.ContainsKey(prefab))
            {
                _pool.Add(prefab, new List<IPoolable>());
            }

            // finds inactive object in list
            result = _pool[prefab].Find(x => !x.IsActive) as T;

            // if where is no inactive object - creates a new one
            if (result is null)
            {
                result = CreateItem(prefab, parent);
            }

            // sets parent to new object
            result.SetParent(parent);
            
            // activates object
            result.Activate();

            return result;
        }

        // internal method for creating new objects
        private static T CreateItem<T>(T prefab, Transform parent) where T : MonoBehaviour, IPoolable
        {
            T instantiatedItem = Object.Instantiate(prefab, parent);
            _pool[prefab].Add(instantiatedItem);

            return instantiatedItem;
        }
    }
}