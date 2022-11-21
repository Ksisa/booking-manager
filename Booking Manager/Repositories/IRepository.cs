namespace Booking_Manager.Repositories
{
    internal interface IRepository<TEntity, TIdType>
    {
        public List<TEntity> Get();

        public TEntity? Get(TIdType id);

        public void Save(TEntity entity);
    }
}
