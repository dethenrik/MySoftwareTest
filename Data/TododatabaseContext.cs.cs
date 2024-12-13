using Microsoft.EntityFrameworkCore;
using SoftwareTest.Models;

namespace SoftwareTest.Data
{
    public class TododatabaseContext : DbContext
    {
        public DbSet<Cpr> CprTables { get; set; }
        public DbSet<TodoItem> TodoItems { get; set; }

        public TododatabaseContext(DbContextOptions<TododatabaseContext> options)
            : base(options) { }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            // Configuring the relationship between Cpr and TodoItems
            modelBuilder.Entity<Cpr>()
                .HasMany(c => c.TodoItems)
                .WithOne(t => t.CprTable)
                .HasForeignKey(t => t.CprTableId);

            base.OnModelCreating(modelBuilder);
        }
    }
}
