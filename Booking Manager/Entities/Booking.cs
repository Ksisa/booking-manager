namespace Booking_Manager.Entities
{
    public class Booking
    {
        public Guest Guest { get; set; }

        public Room Room { get; set; }

        public DateTime Date { get; set; }

        public Booking(Guest guest, Room room, DateTime date)
        {
            Guest = guest;
            Room = room;
            Date = date;
        }
    }
}
