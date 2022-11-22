namespace Booking_Manager.Repositories
{
    public interface IRepository<TEntity, TIdType> where TIdType : notnull
    {
        public List<TEntity> GetAll();

        public TEntity? Get(TIdType id);

        public void Save(TEntity entity);
    }
}
