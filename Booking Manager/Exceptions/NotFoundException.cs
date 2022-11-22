namespace Booking_Manager.Exceptions
{
    /// <summary>
    /// Exception representing an accessed entity not being found
    /// </summary>
    public class NotFoundException : Exception
    {
        /// <summary>
        /// Instantiate exception representing an accessed entity not being found
        /// </summary>
        /// <param name="message">Exception message</param>
        public NotFoundException(string message) : base(message) { }
    }
}
