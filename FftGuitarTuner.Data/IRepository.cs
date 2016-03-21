using System.Linq;
using FftGuitarTuner.Data.Entities;

namespace FftGuitarTuner.Data
{
    public interface IRepository
    {
        IQueryable<Notes> GetAll();
        void Insert(Notes entity);
    }
}
