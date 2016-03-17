using System;
using System.Linq;
using FftGuitarTuner.Data.Entities;

namespace FftGuitarTuner.Data
{
    public class RepositoryService: IRepository
    {
        private DataContext _context = new DataContext();

        public IQueryable<Notes> GetAll()
        {
            return _context.Notes.AsQueryable();
        }
        
        public void Insert(Notes entity)
        {
            if (entity != null)
            {
                _context.Notes.Add(entity);
                _context.SaveChanges();
            }
        }

        public void Dispose()
        {
            Dispose(true);
            GC.SuppressFinalize(this);
        }

        protected virtual void Dispose(bool disposing)
        {
            if (disposing)
            {
                if (_context != null)
                {
                    _context.Dispose();
                    _context = null;
                }
            }
        }
    }
}
