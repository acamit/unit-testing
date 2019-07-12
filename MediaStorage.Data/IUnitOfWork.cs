using MediaStorage.Data.Entities;
using System;
using System.Threading.Tasks;

namespace MediaStorage.Data
{
    public interface IUnitOfWork : IDisposable
    {
        IRepository<MenuItem> MenuItemRepository { get;  }
        IRepository<Menu> MenuRepository { get; }
        int Commit();
        int SaveChanges();
        Task<int> SaveChangesAsync();
    }
}