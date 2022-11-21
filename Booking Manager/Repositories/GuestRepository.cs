using Booking_Manager.Entities;

namespace Booking_Manager.Repositories
{
    internal class GuestRepository : IRepository<Guest, string>
    {
        private List<Guest> Guests { get; set; }

        public GuestRepository(string[] guests)
        {
            this.Guests = guests.Select(name => new Guest(name)).ToList();
        }

        public GuestRepository() : this(new string[] { }) { }

        public List<Guest> Get() => this.Guests;

        public Guest? Get(string surname) => this.Get().Find(guest => guest.Surname == surname);

        public void Save(Guest guest)
        {
            if (guest == null)
            {
                throw new ArgumentNullException("Can't save guest as null");
            }

            Guest? _guest = this.Get(guest.Surname);

            if (_guest == null)
            {
                this.Guests.Add(new Guest(guest.Surname));
            }
            else
            {
                throw new NotImplementedException("Updating guests is not yet supported");
            }
        }
    }
}
