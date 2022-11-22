namespace Booking_Manager.Entities
{
    /// <summary>
    /// Booking for a room by a guest on a specific date
    /// </summary>
    public class Booking
    {
        /// <summary>
        /// Guest for the booking
        /// </summary>
        public Guest Guest { get; set; }

        /// <summary>
        /// Booked room
        /// </summary>
        public Room Room { get; set; }

        /// <summary>
        /// Private date field
        /// </summary>
        private DateTime _Date;

        /// <summary>
        /// Date of the booking
        /// </summary>
        public DateTime Date 
        {
            get 
            {
                return _Date; 
            }
            
            set 
            { 
                this._Date = value.Date; 
            } 
        }

        /// <summary>
        /// Instantiate a new booking
        /// </summary>
        /// <param name="guest">Guest for the booking</param>
        /// <param name="room">Room to book</param>
        /// <param name="date">Date when room will be booked</param>
        public Booking(Guest guest, Room room, DateTime date)
        {
            this.Guest = guest;
            this.Room = room;
            this.Date = date;
        }
    }
}
