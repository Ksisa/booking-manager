namespace Booking_Manager
{
    /// <summary>
    /// Implements a method <see cref="GetLockForId(TId)"/> for getting reference types by ID for thread locking.
    /// </summary>
    /// <typeparam name="TId"></typeparam>
    public interface ILockProvider<TId> where TId : notnull
    {
        /// <summary>
        /// Get a reference object for thread locking by ID
        /// </summary>
        /// <param name="id">ID to lock</param>
        public object GetLockForId(TId id);
    }
}
