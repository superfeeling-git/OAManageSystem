using OA.IRepository;
using System;
using System.Collections.Generic;
using System.Text;
using OA.Model;
using System.Threading.Tasks;

namespace OA.Repository
{
    public class UnitOfWork : IUnitOfWork
    {
        protected readonly OADbContext Context;
        public UnitOfWork(OADbContext _context)
        {
            Context = _context;
        }

        /// <summary>
        /// Saves all pending changes
        /// </summary>
        /// <returns>The number of objects in an Added, Modified, or Deleted state</returns>
        public async Task<int> SaveChangesAsync()
        {
            return await Context.SaveChangesAsync();
        }

        /// <summary>
        /// Disposes the current object
        /// </summary>
        public void Dispose()
        {
            Dispose(true);
            GC.SuppressFinalize(this);
        }

        /// <summary>
        /// Disposes all external resources.
        /// </summary>
        /// <param name="disposing">The dispose indicator.</param>
        private void Dispose(bool disposing)
        {
            if (!disposing) return;
            if (Context == null) return;

            Context.Dispose();
        }
    }
}
