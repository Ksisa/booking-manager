using Booking_Manager.Entities;

namespace Booking_Manager.Repositories
{
    /// <summary>
    /// Room repository that stores rooms in memory
    /// </summary>
    public class MemoryRoomRepository : IRoomRepository
    {
        /// <summary>
        /// Memory room store
        /// </summary>
        private List<Room> _Rooms;

        /// <summary>
        /// Instantiate an instance of the repository, creates room objects from room numbers
        /// </summary>
        /// <param name="rooms">Existing room numbers</param>
        public MemoryRoomRepository(int[] rooms)
        {
            this._Rooms = rooms.Select(r => new Room(r)).ToList();
        }

        /// <summary>
        /// Creates an instances of the repository with an empty store;
        /// </summary>
        public MemoryRoomRepository() : this(new int[] { }) { }

        /// <summary>
        /// Retrieves all rooms from the store
        /// </summary>
        public IEnumerable<Room> GetAll() => this._Rooms;

        /// <summary>
        /// Returns a room with a specific ID, returns null if not found
        /// </summary>
        /// <param name="number">Room number</param>
        public Room? Get(int number) => this.GetAll().ToList().Find(r => r.Number == number);

        /// <summary>
        /// Saves a room to the store (not thread-safe)
        /// </summary>
        /// <param name="room">Room number</param>
        /// <exception cref="ArgumentNullException">Throws if attempting to save null</exception>
        public void Save(Room room)
        {
            if(room == null)
            {
                throw new ArgumentNullException(nameof(room));
            }

            Room? _room = this.Get(room.Number);

            if (_room == null)
            {
                this.Add(room);
            }
            else
            {
                this.Update(_room, room);
            }
        }

        /// <summary>
        /// Replaces an existing room in the store with the new one.
        /// </summary>
        private void Update(Room oldRoom, Room newRoom)
        {
            this._Rooms.Remove(oldRoom);
            this.Add(newRoom);
        }

        /// <summary>
        /// Adds a new room to the store
        /// </summary>
        /// <param name="newRoom"></param>
        private void Add(Room newRoom)
        {
            this._Rooms.Add(newRoom);
        }
    }
}
