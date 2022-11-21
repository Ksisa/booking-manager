using Booking_Manager.Entities;

namespace Booking_Manager.Exceptions
{
    internal class RoomUnavailableException : Exception
    {
        public RoomUnavailableException(int roomNumber, DateTime date) : base($"Room {roomNumber} is unavailble for {date}") { }

        public RoomUnavailableException(Room room, DateTime date) : this(room.Number, date){ }
    }
}
