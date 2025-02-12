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

    public async Task CreateAsync(TEntity entity)
    {
        if (entity == null)
            throw new ArgumentNullException("Entity is null");

        await _context.AddAsync(entity);
    }
    
    public IQueryable<TEntity> GetAll()
    {
        return _context.Set<TEntity>();
    }

    public async Task<int> SaveChangesAsync()
    {
        return await _context.SaveChangesAsync();
    }

    public void Remove(TEntity entity)
    {
        if (entity == null)
            throw new ArgumentNullException("Entity is null");
        _context.Remove(entity);
    }

    public void Update(TEntity entity)
    {
        if (entity == null)
            throw new ArgumentNullException("Entity is null");

        _context.Update(entity);
    }
}
