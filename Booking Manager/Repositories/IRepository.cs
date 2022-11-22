namespace Booking_Manager.Repositories
{
    /// <summary>
    /// Generic repository interface
    /// </summary>
    /// <typeparam name="TEntity">Entity the repository handles</typeparam>
    /// <typeparam name="TId">Primary key type of the entity</typeparam>
    public interface IRepository<TEntity, TId> where TId : notnull
    {
        /// <summary>
        /// Gets all entities from the store
        /// </summary>
        public IEnumerable<TEntity> GetAll();

        /// <summary>
        /// Gets a single entity from the store
        /// </summary>
        /// <param name="id">Primary key of the entity</param>
        /// <returns>Entity, null if not found</returns>
        public TEntity? Get(TId id);

        /// <summary>
        /// Saves entity to the store
        /// </summary>
        public void Save(TEntity entity);
    }
}
