using Booking_Manager;
using Booking_Manager.Exceptions;
using Booking_Manager.Managers;
using Booking_Manager.Repositories;
using NUnit.Framework;

namespace Booking_Manager_Tests1
{
    /// <summary>
    /// Test suite for testing <see cref="Booking_Manager"/>'s <see cref="BookingManager"/>
    /// </summary>
    public class BookingManagerTests
    {
        private IBookingManager _BookingManager;

        private IRoomRepository _RoomRepository;

        private int[] RoomNumbers = new int[] { 101, 102, 103, 201, 202 };

        [SetUp]
        public void Setup()
        {
            ILockProvider<int> lockProvider = new LockProvider();
            this._RoomRepository = new MemoryRoomRepository(this.RoomNumbers);
            this._BookingManager = new BookingManager(this._RoomRepository, lockProvider);
        }


        [Test]
        public void Booked_Room_Is_Unavailable_On_Date()
        {
            this._BookingManager.AddBooking("Peter", RoomNumbers[0], DateTime.Now);

            Assert.IsFalse(this._BookingManager.IsRoomAvailable(this.RoomNumbers[0], DateTime.Now));
            Assert.IsTrue(this._BookingManager.IsRoomAvailable(this.RoomNumbers[0], DateTime.Now.AddDays(1)));
        }

        [Test]
        public void Is_NonExistant_Room_Available_Throws()
        {            
            Assert.Throws<NotFoundException>(() => this._BookingManager.AddBooking("Peter", 1, DateTime.Now));
        }

        [Test]
        public void Booking_Room_Twice_Throws()
        {
            this._BookingManager.AddBooking("Peter", this.RoomNumbers[0], DateTime.Now);
            Assert.Throws<RoomUnavailableException>(() => this._BookingManager.AddBooking("Josh", this.RoomNumbers[0], DateTime.Now));
        }

        [Test]
        public void Gets_All_Rooms_When_None_Booked()
        {
            Assert.That(this._BookingManager.getAvailableRooms(DateTime.Now).Count(), Is.EqualTo(this.RoomNumbers.Length));
        }

        [Test]
        public void Gets_Available_Rooms()
        {
            this._BookingManager.AddBooking("Josh", RoomNumbers[0], DateTime.Now);
            this._BookingManager.AddBooking("Peter", RoomNumbers[1], DateTime.Now);
            this._BookingManager.AddBooking("Tom", RoomNumbers[0], DateTime.Now.AddDays(1));

            Assert.That(this._BookingManager.getAvailableRooms(DateTime.Now), Is.EquivalentTo(this.RoomNumbers.Skip(2)));
        }

        [Test]
        public void Cant_Book_NonExistant_Room()
        {
            Assert.Throws<NotFoundException>(() => this._BookingManager.AddBooking("Josh", 1, DateTime.Now));
        }

        [Test]
        public void One_Manager_Does_Not_Duplicate_Bookings()
        {
            var tasks = new List<Task> { };
            int threadCount = 5;

            for (int i = 0; i < threadCount; i++)
            {

                // All except one should fail
                tasks.Add(Task.Run(() =>
                {
                    this._BookingManager.AddBooking("Peter", this.RoomNumbers[0], DateTime.Now);
                }));

                // First one should fail (i = 0)
                int _i = i;
                tasks.Add(Task.Run(() =>
                {
                    this._BookingManager.AddBooking("Josh", this.RoomNumbers[0], DateTime.Now.AddDays(_i));
                }));
            }

            Assert.Throws<AggregateException>(() => Task.WaitAll(tasks.ToArray()));

            Assert.That(this._RoomRepository.Get(this.RoomNumbers[0])?.Bookings.Count, Is.EqualTo(threadCount));
        }

        [Test]
        public void Multiple_Managers_Do_Not_Duplicate_Bookings()
        {
            var tasks = new List<Task> { };
            int threadCount = 5;
            ILockProvider<int> lp = new LockProvider();

            for (int i = 0; i < threadCount; i++)
            {

                // All except one should fail
                tasks.Add(Task.Run(() =>
                {
                    IBookingManager bm = new BookingManager(this._RoomRepository, lp);
                    bm.AddBooking("Peter", this.RoomNumbers[0], DateTime.Now);
                }));

                // First one should fail (i = 0)
                int _i = i;
                tasks.Add(Task.Run(() =>
                {
                    IBookingManager bm = new BookingManager(this._RoomRepository, lp);
                    bm.AddBooking("Peter", this.RoomNumbers[0], DateTime.Now.AddDays(_i));
                }));
            }

            Assert.Throws<AggregateException>(() => Task.WaitAll(tasks.ToArray()));

            Assert.That(_RoomRepository.Get(this.RoomNumbers[0])?.Bookings.Count, Is.EqualTo(threadCount));
        }
    }
}