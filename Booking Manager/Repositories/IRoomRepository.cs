using Booking_Manager.Entities;

namespace Booking_Manager.Repositories
{
    /// <summary>
    /// A repository for a hotel's rooms
    /// </summary>
    public interface IRoomRepository : IRepository<Room, int> { }
}
