using Booking_Manager.Entities;

namespace Booking_Manager.Exceptions
{
    /// <summary>
    /// Exception representing a specific hotel being booked at a requested time.
    /// </summary>
    public class RoomUnavailableException : Exception
    {
        /// <summary>
        /// Instantiate exception representing a specific hotel being booked at a requested time.
        /// </summary>
        /// <param name="roomNumber">Hotel room number</param>
        /// <param name="date">Date for requested booking</param>
        public RoomUnavailableException(int roomNumber, DateTime date) : base($"Room {roomNumber} is unavailble for {date}") { }


        /// <summary>
        /// Instantiate exception representing a specific hotel being booked at a requested time.
        /// </summary>
        /// <param name="roomNumber">Hotel room</param>
        /// <param name="date">Date for requested booking</param>
        public RoomUnavailableException(Room room, DateTime date) : this(room.Number, date){ }
    }
}
