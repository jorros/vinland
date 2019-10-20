using Jorros.Vinland.Data.Entities;
using Microsoft.EntityFrameworkCore;

namespace Jorros.Vinland.Data
{
    public class VinlandContext : DbContext
    {
        public DbSet<Order> Orders { get; set; }

        public DbSet<Bottle> Bottles { get; set; }

        public VinlandContext(DbContextOptions<VinlandContext> options) : base(options)
        {
        }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder
                .Entity<Order>()
                .HasKey(x => x.Id);
            modelBuilder
                .Entity<Order>()
                .Property(x => x.Id)
                .ValueGeneratedOnAdd();

            modelBuilder
                .Entity<Bottle>()
                .HasKey(x => x.Id);
            modelBuilder
                .Entity<Bottle>()
                .Property(x => x.Id)
                .ValueGeneratedOnAdd();
            modelBuilder
                .Entity<Bottle>()
                .HasOne(x => x.Order)
                .WithMany(x => x.Bottles);
        }
    }
}