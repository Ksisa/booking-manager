using Booking_Manager.Entities;

namespace Booking_Manager.Repositories
{
    public class RoomRepository : IRepository<Room, int>
    {
        private List<Room> Rooms { get; set; }

        public RoomRepository(int[] rooms)
        {
            this.Rooms = rooms.Select(r => new Room(r)).ToList();
        }

        public RoomRepository() : this(new int[] { }) { }

        public List<Room> Get() => this.Rooms;

        public Room? Get(int number) => this.Get().Find(r => r.Number == number);

        public void Save(Room room)
        {
            if(room == null)
            {
                throw new ArgumentNullException("Can't save room as null");
            }

            Room? _room = this.Get(room.Number);

            if (_room == null)
            {
                throw new NotImplementedException("Adding new rooms at runtime is not yet supported");
            }
            else
            {
                this.Update(_room, room);
            }
        }

        private void Update(Room oldRoom, Room newRoom)
        {
            this.Rooms.Remove(oldRoom);
            this.Rooms.Add(newRoom);
        }
    }
}
