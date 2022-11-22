using System.Collections.Concurrent;

namespace Booking_Manager
{
    /// <summary>
    /// Generic reference type provider for thread locking
    /// </summary>
    public class LockProvider : ILockProvider<int>
    {
        /// <summary>
        /// Lock store.
        /// </summary>
        private ConcurrentDictionary<int, object> _Locks;

        /// <summary>
        /// Gets a object to thread lock for a given room number
        /// </summary>
        /// <param name="roomNumber">Room number to reference the lock</param>
        public object GetLockForId(int roomNumber) => this._Locks.GetOrAdd(roomNumber, new { });

        /// <summary>
        /// Instantiate this class
        /// </summary>
        public LockProvider()
        {
            this._Locks = new();
        }
    }
}
