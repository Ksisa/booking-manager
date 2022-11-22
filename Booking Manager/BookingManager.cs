using Booking_Manager.Entities;
using Booking_Manager.Exceptions;
using Booking_Manager.Repositories;
using System.Collections.Concurrent;

namespace Booking_Manager
{
    public class BookingManager : IBookingManager
    {
        private readonly IRepository<Room, int> _RoomRepository;

        private QueueManager<int> _QueueManager;

        private ConcurrentDictionary<int, Task> _Queue = new();

        public BookingManager(IRepository<Room, int> roomRepository)
        {
            this._RoomRepository = roomRepository;
            this._QueueManager = new();
        }

        public void AddBooking(string guest, int room, DateTime date)
        {
            this._QueueManager.AddToQueue(room, new Task(() =>
            {
                Room _room = this.GetRoomAndAssert(room);
                if (!this.TemporalIsRoomAvailable(_room, date.Date))
                {
                    throw new RoomUnavailableException(_room, date.Date);
                }

                Guest _guest = new Guest(guest);
                    
                Booking newBooking = new Booking(_guest, _room, date.Date);
                _room.Bookings.Add(newBooking);
                _guest.Bookings.Add(newBooking);

                this._RoomRepository.Save(_room);
            }));
        }

        public bool IsRoomAvailable(int room, DateTime date) => this.IsRoomAvailable(this.GetRoomAndAssert(room), date.Date);

        public bool IsRoomAvailable(Room room, DateTime date)
        {
            this._QueueManager.WaitForQueueToFinish(room.Number);
            return this.TemporalIsRoomAvailable(room, date);
        }

        /// Checks availability without waiting for the queue to finish.
        private bool TemporalIsRoomAvailable(Room room, DateTime date) => !room.Bookings.Select(b => b.Date).Contains(date.Date);

        public IEnumerable<int> getAvailableRooms(DateTime date) => this._RoomRepository.Get()
            .Where(r => !r.Bookings
                .Select(b => b.Date)
                .Contains(date.Date))
            .Select(r => r.Number);

        private Room GetRoomAndAssert(int room)
        {
            Room? _room = this._RoomRepository.Get(room);

            if (_room == null)
            {
                throw new NotFoundException($"Room {room} not found.");
            }

            return _room;
        }
    }
}
