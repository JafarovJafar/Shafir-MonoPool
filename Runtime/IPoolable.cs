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
        /// <remarks>
        /// Implement flag of object what can be used by pool
        /// If object IsActive property if false - MonoPool will use it
        /// </remarks>
        bool IsActive { get; }

        /// <summary>
        /// Activate object (release object from pool)
        /// </summary>
        /// <remarks>
        /// Implement goal behaviour for object when release from pool
        /// </remarks>
        void Activate();

        /// <summary>
        /// Deactivate object (return object to pool)
        /// </summary>
        /// <remarks>
        /// Implement goal behaviour for object when return to pool 
        /// </remarks>
        void Deactivate();
    }
}