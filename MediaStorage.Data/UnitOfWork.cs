using System;
using System.Threading.Tasks;
using MediaStorage.Data.Entities;

namespace MediaStorage.Data
{
    public class UnitOfWork : IUnitOfWork
    {
        private readonly IMediaContext _context;

        public IRepository<MenuItem> MenuItemRepository { get; }
        public IRepository<Menu> MenuRepository { get;  }

        public UnitOfWork(IMediaContext context)
        {
            _context = context;
            MenuRepository = new Repository<Menu>(_context);
            MenuItemRepository = new Repository<MenuItem>(_context);
        }
        
        public int Commit()
        {
            int commitCount = -1;
            using (var transaction = _context.Database.BeginTransaction())
            {
                try
                {
                    commitCount = SaveChanges();

                    transaction.Commit();
                }
                catch (Exception ex)
                {
                    transaction.Rollback();
                    throw ex;
                }
            }

            return commitCount;
        }

        public int SaveChanges()
        {
            return _context.SaveChanges();
        }

        public Task<int> SaveChangesAsync()
        {
            return _context.SaveChangesAsync();
        }

        #region IDisposable implementation
        /*
         * DbContext has been disposed error on IDisposable pattern.
         */
        private bool disposed = false;

        protected virtual void Dispose(bool disposing)
        {
            if (disposed == false)
                if (disposing)
                    if (_context != null)
                        _context.Dispose();

            disposed = true;
        }

        public void Dispose()
        {
            Dispose(true);
            GC.SuppressFinalize(this);
        }
        #endregion
    }
}
