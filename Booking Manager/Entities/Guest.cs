namespace Booking_Manager.Entities
{
    public class Guest
    {
        public string Surname { get; set; }

        public List<Booking> Bookings { get; set; }

        public Guest(string surname)
        {
            Surname = surname;
            Bookings = new();
        }
    }
}
