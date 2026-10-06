using Microsoft.EntityFrameworkCore;
using Model;

namespace DataAccessLayer.EF
{
    public class DBContext : DbContext
    {
        private readonly string _dbPath;

        public DbSet<Student> Students { get; set; } = null!;

        public DBContext(string dbPath = "app.db")
        {
            _dbPath = dbPath;
            Database.EnsureCreated();
        }

        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {
            if (!optionsBuilder.IsConfigured)
            {
                optionsBuilder.UseSqlite($"Data Source={_dbPath}");
            }
        }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);
            modelBuilder.Entity<Student>(entity =>
            {
                entity.HasKey(s => s.Id);
                entity.Property(s => s.Id).ValueGeneratedOnAdd();
                entity.Property(s => s.Name).IsRequired();
                entity.Property(s => s.Speciality).IsRequired();
                entity.Property(s => s.Group).IsRequired();
            });
        }
    }
}
