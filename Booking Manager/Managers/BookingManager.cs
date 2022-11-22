using Booking_Manager.Entities;
using Booking_Manager.Exceptions;
using Booking_Manager.Managers;
using Booking_Manager.Repositories;

namespace Booking_Manager
{
    /// <summary>
    /// Manager that handles a hotel's bookings
    /// </summary>
    public class BookingManager : IBookingManager
    {
        /// <summary>
        /// Persistent room store for existing hotel's rooms
        /// </summary>
        private readonly IRoomRepository _RoomRepository;

        /// <summary>
        /// Object that provides locks based on IDs
        /// </summary>
        private readonly ILockProvider<int> _LockProvider;

        /// <summary>
        /// Instantiate a new hotel booking manager
        /// </summary>
        /// <param name="roomRepository">Persistent room store</param>
        /// <param name="lockProvider">Lock provider based on IDs</param>
        public BookingManager(IRoomRepository roomRepository, ILockProvider<int> lockProvider)
        {
            this._RoomRepository = roomRepository;
            this._LockProvider = lockProvider;
        }

        /// <summary>
        /// Add a new booking
        /// </summary>
        /// <param name="guest">Guest's surname</param>
        /// <param name="room">Room number</param>
        /// <param name="date">Date for the booking</param>
        /// <exception cref="RoomUnavailableException"></exception>
        public void AddBooking(string guest, int room, DateTime date)
        {
            var roomLock = this._LockProvider.GetLockForId(room);
            lock (roomLock)
            {
                Room _room = this.GetRoomAndAssert(room);

                if (!this.IsRoomAvailable(_room, date.Date))
                {
                    throw new RoomUnavailableException(_room, date.Date);
                }

                Guest _guest = new Guest(guest);

                Booking newBooking = new Booking(_guest, _room, date);
                _room.Bookings.Add(newBooking);

                lock (this._RoomRepository)
                {
                    this._RoomRepository.Save(_room);
                }
            }
        }

        /// <summary>
        /// Checks if a given room is available on a specific date
        /// </summary>
        /// <param name="room">Room number</param>
        /// <param name="date">Date to check</param>
        public bool IsRoomAvailable(int room, DateTime date)
        {
            var roomLock = this._LockProvider.GetLockForId(room);

            lock (roomLock)
            {
                return this.IsRoomAvailable(this.GetRoomAndAssert(room), date);
            }
        }

        /// <summary>
        /// Checks if a given room is available on a specific date
        /// </summary>
        /// <param name="room">Room</param>
        /// <param name="date">Date to check</param>
        public bool IsRoomAvailable(Room room, DateTime date) => !room.Bookings.Select(b => b.Date).Contains(date.Date);

        /// <summary>
        /// Gets the available rooms for a given date
        /// </summary>
        /// <param name="date">Date to check</param>
        public IEnumerable<int> getAvailableRooms(DateTime date)
        {
            lock (this._RoomRepository)
            {
                return this._RoomRepository.GetAll()
                .Where(r => !r.Bookings
                    .Select(b => b.Date)
                    .Contains(date.Date))
                .Select(r => r.Number);
            }
        }

        /// <summary>
        /// Gets a room from the repository, throws a <see cref="NotFoundException"/> if that room is not found.
        /// </summary>
        private Room GetRoomAndAssert(int roomNumber)
        {
            Room? _room;
            lock (this._RoomRepository)
            {
                _room = this._RoomRepository.Get(roomNumber);
            }

            if (_room == null)
            {
                throw new NotFoundException($"Room {roomNumber} not found.");
            }

            return _room;
        }
    }
}
