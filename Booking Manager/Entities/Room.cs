namespace Booking_Manager.Entities
{
    /// <summary>
    /// Hotel room
    /// </summary>
    public class Room
    {
        /// <summary>
        /// Room number (currently also ID of the room)
        /// </summary>
        public int Number { get; set; }

        /// <summary>
        /// Bookings for the room
        /// </summary>
        public List<Booking> Bookings { get; set; }

        /// <summary>
        /// Instantiate a new hotel room
        /// </summary>
        public Room(int roomNumber)
        {
            this.Number = roomNumber;
            this.Bookings = new();
        }

    }
}
