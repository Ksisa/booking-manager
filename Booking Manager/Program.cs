using Booking_Manager.Entities;
using Booking_Manager.Managers;
using Booking_Manager.Repositories;
namespace Booking_Manager
{
    internal class Program
    {
        static void Main(string[] args)
        {
            IRoomRepository roomRepository = new MemoryRoomRepository(new int[] { 101, 102, 103, 201, 202 });
            ILockProvider<int> lockProvider = new LockProvider();

            IBookingManager bm = new BookingManager(roomRepository, lockProvider);// create your manager here; 
            var today = new DateTime(2012, 3, 28);
            Console.WriteLine(bm.IsRoomAvailable(101, today)); // outputs true 
            bm.AddBooking("Patel", 101, today);
            Console.WriteLine(bm.IsRoomAvailable(101, today)); // outputs false 
            bm.AddBooking("Li", 101, today); // throws an exception
        }
    }
}