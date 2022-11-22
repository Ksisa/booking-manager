using Booking_Manager;
using Booking_Manager.Exceptions;
using Booking_Manager.Repositories;

namespace Booking_Manager_Tests1.BookingManager
{
    public class SingleThreaded
    {
        private IBookingManager _BookingManager;

        private int[] RoomNumbers = new int[] { 101, 102, 103, 201, 202 };

        [SetUp]
        public void Setup()
        {
            var roomRepository = new RoomRepository(RoomNumbers);
            this._BookingManager = new Booking_Manager.BookingManager(roomRepository);
        }


        [Test]
        public void Booked_Room_Is_Unavailable_On_Date()
        {
            _BookingManager.AddBooking("Peter", RoomNumbers[0], DateTime.Now);

            Assert.IsFalse(_BookingManager.IsRoomAvailable(RoomNumbers[0], DateTime.Now));
            Assert.IsTrue(_BookingManager.IsRoomAvailable(RoomNumbers[0], DateTime.Now.AddDays(1)));
        }


        [Test]
        public void Booking_Room_Twice_Throws()
        {
            _BookingManager.AddBooking("Peter", RoomNumbers[0], DateTime.Now);
            Assert.Throws<RoomUnavailableException>(() => _BookingManager.AddBooking("Josh", RoomNumbers[0], DateTime.Now));
        }

        [Test]
        public void Gets_All_Rooms_When_None_Booked()
        {
            Assert.That(_BookingManager.getAvailableRooms(DateTime.Now).Count(), Is.EqualTo(RoomNumbers.Length));
        }

        [Test]
        public void Gets_Available_Rooms()
        {
            _BookingManager.AddBooking("Josh", RoomNumbers[0], DateTime.Now);
            _BookingManager.AddBooking("Peter", RoomNumbers[1], DateTime.Now);
            _BookingManager.AddBooking("Tom", RoomNumbers[0], DateTime.Now.AddDays(1));
            _BookingManager.AddBooking("Peter", RoomNumbers[3], DateTime.Now.AddDays(-1));

            Assert.That(_BookingManager.getAvailableRooms(DateTime.Now), Is.EquivalentTo(RoomNumbers.Skip(2)));
        }

        [Test]
        public void Cant_Book_NonExistant_Room()
        {
            Assert.Throws<NotFoundException>(() => _BookingManager.AddBooking("Josh", 1, DateTime.Now));
        }
    }
}