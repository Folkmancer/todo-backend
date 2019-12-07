using Microsoft.EntityFrameworkCore;

namespace backend.Models
{
    public class DataBaseContext : DbContext
    {
        public DataBaseContext() { }

        public DataBaseContext(DbContextOptions<DataBaseContext> options) 
            : base(options)
        {
        }

        public virtual DbSet<Event> Events { get; set; }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
            => modelBuilder.UseIdentityColumns();

    }
}
