using BigTech.Domain.Entity;
using BigTech.Domain.Interfaces.Repositories;
using Microsoft.EntityFrameworkCore.Storage;

namespace BigTech.Domain.Interfaces.Databases;
public interface IUnitOfWork : IStateSaveChanges, IDisposable
{
    Task<IDbContextTransaction> BeginTransactionAsync();

    IBaseRepository<User> Users { get; set; }

    IBaseRepository<Role> Roles { get; set; }

    IBaseRepository<UserRole> UserRoles { get; set; }
}
