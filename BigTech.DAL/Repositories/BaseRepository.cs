using BigTech.Domain.Interfaces.Repositories;

namespace BigTech.DAL.Repositories;
internal class BaseRepository<TEntity> :
    IBaseRepository<TEntity>
    where TEntity : class
{
    private readonly ApplicationDbContext _context;

    public BaseRepository(ApplicationDbContext context)
    {
        _context = context;
    }

    public async Task<TEntity> CreateAsync(TEntity entity)
    {
        if (entity == null)
            throw new ArgumentNullException("Entity is null");

        await _context.AddAsync(entity);
        await _context.SaveChangesAsync();

        return entity;
    }


    public IQueryable<TEntity> GetAll()
    {
        return _context.Set<TEntity>();
    }

    public async Task<int> SaveChangesAsync()
    {
        return await _context.SaveChangesAsync();
    }

    public async Task<TEntity> RemoveAsync(TEntity entity)
    {
        if (entity == null)
            throw new ArgumentNullException("Entity is null");
        _context.Remove(entity);
        await _context.SaveChangesAsync();

        return entity;
    }

    public async Task<TEntity> UpdateAsync(TEntity entity)
    {
        if (entity == null)
            throw new ArgumentNullException("Entity is null");

        _context.Update(entity);
        await _context.SaveChangesAsync();

        return entity;
    }
}
