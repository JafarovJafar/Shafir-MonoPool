using System.Collections.Generic;
using UnityEngine;

namespace Shafir.MonoPool
{
    /*
     * |-----------------------------------|
     * |     MonoPool by Jafarov Jafar     |
     * |-----------------------------------|
     * | For poolable object:              |
     * | 1. implement IPoolable interface  |
     * | 2. ???                            |
     * | 3. PROFIT!!!                      |
     * | For pooling:                      |
     * | 1. Use "Get" method to get        |
     * |     object from pool              |
     * | 2. User ReturnItem method to      |
     * |     return object to pool         |
     * |-----------------------------------|
     * |               @2023               |
     * |-----------------------------------| 
    */



    /// <summary>
    /// Pool of objects
    /// </summary>
    public static class MonoPool
    {
        // Containers for each pooled type
        private static Dictionary<IPoolable, Transform> _containers;

        // Dictionary with all pooled objects
        private static Dictionary<IPoolable, List<IPoolable>> _pool = new Dictionary<IPoolable, List<IPoolable>>();

        /// <summary>
        /// Fill pool with objects
        /// </summary>
        /// <param name="prefab">Goal prefab</param>
        /// <param name="count">Count of prefabs to instantiate</param>
        public static void Fill<T>(T prefab, int count) where T : MonoBehaviour, IPoolable
        {
            // if there is no item in dictionary - creates new list
            if (!_pool.ContainsKey(prefab)) _pool.Add(prefab, new List<IPoolable>());

            for (int i = 0; i < count; i++)
            {
                // create new item and deactivates it
                T item = CreateItem(prefab);
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
                result = CreateItem(prefab);
            }

            // activates object
            result.Activate();

            return result;
        }

        /// <summary>
        /// Return object to pool
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="prefab">Object to return</param>
        public static void Return<T>(T prefab) where T : MonoBehaviour, IPoolable
        {
            // generates new container if there is no one for prefab
            if (!_containers.TryGetValue(prefab, out var container))
            {
                Debug.LogWarning("Incorrent return attempt!");

                container = new GameObject().transform;
                container.name = prefab.name;

                _containers.Add(prefab, container);
            }

            prefab.Deactivate();
            prefab.transform.SetParent(container);
        }

        // internal method for creating new objects
        private static T CreateItem<T>(T prefab) where T : MonoBehaviour, IPoolable
        {
            // generates new container if there is no one for prefab
            if (!_containers.TryGetValue(prefab, out var container))
            {
                container = new GameObject().transform;
                container.name = prefab.name;

                _containers.Add(prefab, container);
            }

            T instantiatedItem = Object.Instantiate(prefab, container);
            _pool[prefab].Add(instantiatedItem);

            return instantiatedItem;
        }
    }
}