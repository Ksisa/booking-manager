using Booking_Manager.Repositories;
namespace Booking_Manager
{
    internal class Program
    {
        static void Main(string[] args)
        {
            // Mimics some kind of persistant store
            var roomRepository = new RoomRepository(new int[] { 101, 102, 103, 201, 202 });

            // Assume only one BookingManager will be instantiated per repository.
            // If not, I would have added RoomScheduler dependency to BookingManager.
            IBookingManager bm = new BookingManager(roomRepository);// create your manager here; 
            var today = new DateTime(2012, 3, 28);
            Console.WriteLine(bm.IsRoomAvailable(101, today)); // outputs true 
            bm.AddBooking("Patel", 101, today);
            Console.WriteLine(bm.IsRoomAvailable(101, today)); // outputs false 
            bm.AddBooking("Li", 101, today); // throws an exception
        }
    }
}