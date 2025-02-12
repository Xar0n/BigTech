using BigTech.Domain.Entity;
using BigTech.Domain.Interfaces.Databases;
using BigTech.Domain.Interfaces.Repositories;
using Microsoft.EntityFrameworkCore.Storage;

namespace BigTech.DAL.Repositories;
public class UnitOfWork : IUnitOfWork
{
    public IBaseRepository<User> Users { get; set; }
    public IBaseRepository<Role> Roles { get; set; }
    public IBaseRepository<UserRole> UserRoles { get; set; }

    private readonly ApplicationDbContext _context;
    private bool _disposed;

    public UnitOfWork(IBaseRepository<User> users, 
        IBaseRepository<Role> roles,
        ApplicationDbContext context,
        IBaseRepository<UserRole> userRoles)
    {
        Users = users;
        Roles = roles;
        _context = context;
        UserRoles = userRoles;
    }

    public async Task<IDbContextTransaction> BeginTransactionAsync()
    {
       return await _context.Database.BeginTransactionAsync();
    }

    public void Dispose()
    {
       GC.SuppressFinalize(this);
    }

    public async Task<int> SaveChangesAsync()
    {
        return await _context.SaveChangesAsync();
    }
}
