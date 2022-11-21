using Booking_Manager.Entities;
using Booking_Manager.Exceptions;
using Booking_Manager.Repositories;

namespace Booking_Manager
{
    internal class BookingManager : IBookingManager
    {
        private readonly IRepository<Room, int> _RoomRepository;

        private readonly IRepository<Guest, string> _GuestRepository;

        public BookingManager(IRepository<Room, int> roomRepository, IRepository<Guest, string> guestRepository)
        {
            this._RoomRepository = roomRepository;
            this._GuestRepository = guestRepository;
        }

        public void AddBooking(string guest, int room, DateTime date)
        {
            Room _room = this.GetRoomAndAssert(room);

            if (!this.IsRoomAvailable(room, date))
            {
                throw new RoomUnavailableException(room, date);
            }

            Guest? _guest = this._GuestRepository.Get(guest);
            if(_guest == null)
            {
                Guest newGuest = new Guest(guest);
                _guest = newGuest;
            }

            Booking newBooking = new Booking(_guest, _room, date);
            _room.Bookings.Add(newBooking);
            _guest.Bookings.Add(newBooking);

            this._GuestRepository.Save(_guest);
            this._RoomRepository.Save(_room);
        }

        public bool IsRoomAvailable(int room, DateTime date) => this.IsRoomAvailable(this.GetRoomAndAssert(room), date);

        public bool IsRoomAvailable(Room room, DateTime date) => room.Bookings.Select(b => b.Date).Contains(date.Date);

        public IEnumerable<int> getAvailableRooms(DateTime date) => this._RoomRepository.Get()
            .Where(r => !r.Bookings
                .Select(b => b.Date)
                .Contains(date))
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
