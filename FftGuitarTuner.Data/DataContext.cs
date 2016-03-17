using System.Data.Entity;
using System.Data.Entity.ModelConfiguration.Conventions;
using FftGuitarTuner.Data.Entities;

namespace FftGuitarTuner.Data
{
    public class DataContext: DbContext
    {
        public DataContext() : base("DConnection")
        {
            Database.SetInitializer<DataContext>(null);
        }

        public DbSet<Notes> Notes { get; set; }

        protected override void OnModelCreating(DbModelBuilder modelBuilder)
        {
            modelBuilder.Conventions.Remove<PluralizingTableNameConvention>();
        }
    }
}
