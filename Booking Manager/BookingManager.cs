using Booking_Manager.Entities;
using Booking_Manager.Exceptions;
using Booking_Manager.Repositories;
using System.Collections.Concurrent;

namespace Booking_Manager
{
    public class BookingManager : IBookingManager
    {
        private readonly IRepository<Room, int> _Repository;

        private ConcurrentDictionary<int, object> _RoomLocks;

        private object GetRoomLock(int roomNumber) => _RoomLocks.GetOrAdd(roomNumber, new { });

        public BookingManager(IRepository<Room, int> repository)
        {
            this._Repository = repository;
            this._RoomLocks = new();
        }

        public void AddBooking(string guest, int room, DateTime date)
        {
            var roomLock = this.GetRoomLock(room);
            lock (roomLock)
            {
                Room _room = this.GetRoomAndAssert(room);

                if (!this.IsRoomAvailable(_room, date.Date))
                {
                    throw new RoomUnavailableException(_room, date.Date);
                }

                Guest _guest = new Guest(guest);

                Booking newBooking = new Booking(_guest, _room, date.Date);
                _room.Bookings.Add(newBooking);
                _guest.Bookings.Add(newBooking);

                lock (this._Repository)
                {
                    this._Repository.Save(_room);
                }
            }
        }

        public bool IsRoomAvailable(int room, DateTime date)
        {
            var roomLock = this.GetRoomLock(room);

            lock (roomLock)
            {
                return this.IsRoomAvailable(this.GetRoomAndAssert(room), date.Date);
            }
        }

        public bool IsRoomAvailable(Room room, DateTime date) => !room.Bookings.Select(b => b.Date).Contains(date.Date);


        public IEnumerable<int> getAvailableRooms(DateTime date)
        {
            lock (this._Repository)
            {
                return this._Repository.GetAll()
                .Where(r => !r.Bookings
                    .Select(b => b.Date)
                    .Contains(date.Date))
                .Select(r => r.Number);
            }
        }

        private Room GetRoomAndAssert(int room)
        {
            Room? _room;
            lock (this._Repository)
            {
                _room = this._Repository.Get(room);
            }

            if (_room == null)
            {
                throw new NotFoundException($"Room {room} not found.");
            }

            return _room;
        }
    }
}
