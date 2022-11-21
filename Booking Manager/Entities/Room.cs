namespace Booking_Manager.Entities
{
    public class Room
    {
        public int Number { get; set; }

        public List<Booking> Bookings { get; set; }

        public Room(int number)
        {
            Number = number;
            Bookings = new();
        }

    }
}
