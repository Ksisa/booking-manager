using Booking_Manager;
using Booking_Manager.Exceptions;
using Booking_Manager.Repositories;

namespace Booking_Manager_Tests1
{
    public class Tests
    {

        IBookingManager BookingManager;

        [SetUp]
        public void Setup()
        {
            var roomRepository = new RoomRepository(new int[] { 101, 102, 103, 201, 202 });
            this.BookingManager = new BookingManager(roomRepository);
        }


        [Test(Description = "Booked room is unavailable")]
        public void RoomIsUnavailable()
        {
            this.BookingManager.AddBooking("Peter", 101, DateTime.Now);
            Assert.IsFalse(this.BookingManager.IsRoomAvailable(101, DateTime.Now));
            Assert.Pass();
        }


        [Test(Description = "Can book multiple rooms at the same time")]
        public void ConcurrentBookings()
        {
            for (int i = 0; i < 10; i++)
            {
                this.BookingManager.AddBooking("Peter", 101, DateTime.Now.AddDays(i));
            }

            this.BookingManager.AddBooking("Joshua", 102, DateTime.Now);
            Assert.True(this.BookingManager.getAvailableRooms(DateTime.Now.AddDays(8)).Contains(101));
            Assert.False(this.BookingManager.getAvailableRooms(DateTime.Now).Contains(102));


        }

        [Test(Description = "Cannot book booked room")]
        public void OverBookedRoom()
        {
            this.BookingManager.AddBooking("Peter", 101, DateTime.Now);
            Assert.Throws<RoomUnavailableException>(() => this.BookingManager.AddBooking("Josh", 101, DateTime.Now));
            Assert.Pass();
        }
    }
}