namespace Booking_Manager.Entities
{
    /// <summary>
    /// Hotel guest
    /// </summary>
    public class Guest
    {
        /// <summary>
        /// Surname (currently ID of the guest)
        /// </summary>
        public string Surname { get; set; }

        /// <summary>
        /// Instantiate a new guest 
        /// </summary>
        public Guest(string surname)
        {
            this.Surname = surname;
        }
    }
}
