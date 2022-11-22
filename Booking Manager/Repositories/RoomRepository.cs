using Booking_Manager.Entities;

namespace Booking_Manager.Repositories
{
    public class RoomRepository : IRepository<Room, int>
    {
        private List<Room> _Rooms;

        public RoomRepository(int[] rooms)
        {
            this._Rooms = rooms.Select(r => new Room(r)).ToList();
        }

        public RoomRepository() : this(new int[] { }) { }

        public List<Room> GetAll() => this._Rooms;

        public Room? Get(int number) => this.GetAll().Find(r => r.Number == number);

        public void Save(Room room)
        {
            if(room == null)
            {
                throw new ArgumentNullException(nameof(room));
            }

            Room? _room = this.Get(room.Number);

            if (_room == null)
            {
                this.Add(room);
            }
            else
            {
                this.Update(_room, room);
            }
        }

        private void Update(Room oldRoom, Room newRoom)
        {
            this._Rooms.Remove(oldRoom);
            this.Add(newRoom);
        }

        private void Add(Room newRoom)
        {
            this._Rooms.Add(newRoom);
        }
    }
}
