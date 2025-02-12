using BigTech.Domain.Interfaces.Databases;

namespace BigTech.Domain.Interfaces.Repositories;
public interface IBaseRepository<TEntity> : IStateSaveChanges
{
    IQueryable<TEntity> GetAll();

    Task CreateAsync(TEntity entity);

    void Update(TEntity entity);

    void Remove(TEntity entity);
}
