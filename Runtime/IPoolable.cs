using UnityEngine;

namespace Shafir.MonoPool
{
    /// <summary>
    /// Interface for poolable object
    /// </summary>
    public interface IPoolable
    {
        /// <summary>
        /// Can be object be released from pool?
        /// </summary>
        bool IsActive { get; }

        /// <summary>
        /// Activate object (release object from pool)
        /// </summary>
        void Activate();

        /// <summary>
        /// Deactivate object (return object to pool)
        /// </summary>
        void Deactivate();
        
        /// <summary>
        /// Set parent for poolable object
        /// </summary>
        /// <param name="parent">Parent</param>
        void SetParent(Transform parent);
    }
}