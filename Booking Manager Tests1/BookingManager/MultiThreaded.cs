using Booking_Manager;
using Booking_Manager.Entities;
using Booking_Manager.Exceptions;
using Booking_Manager.Repositories;

namespace Booking_Manager_Tests1.BookingManager
{
    public class MultiThreadedTests
    {

        private IBookingManager _BookingManager;

        private IRepository<Room, int> _RoomRepository;

        private int[] RoomNumbers = new int[] { 101, 102, 103, 201, 202 };

        [SetUp]
        public void Setup()
        {
            this._RoomRepository = new RoomRepository(RoomNumbers);
            this._BookingManager = new Booking_Manager.BookingManager(this._RoomRepository);
        }


        [Test]
        public void Booking_Room_Twice_Does_Not_Overbook()
        {
            var tasks = new List<Task> { };
            int threadCount = 5;

            for (int i = 0; i < threadCount; i++)
            {
                
                // All except one should fail
                tasks.Add(Task.Run(() =>
                {
                    this._BookingManager.AddBooking("Peter", RoomNumbers[0], DateTime.Now);
                }));

                // First one should fail (i = 0)
                int _i = i;
                tasks.Add(Task.Run(() =>
                {
                    this._BookingManager.AddBooking("Josh", RoomNumbers[0], DateTime.Now.AddDays(_i));
                }));
            }

            Assert.Throws<AggregateException>(() => Task.WaitAll(tasks.ToArray()));

            Assert.That(this._RoomRepository.Get(RoomNumbers[0])?.Bookings.Count, Is.EqualTo(threadCount));
        }
    }
}