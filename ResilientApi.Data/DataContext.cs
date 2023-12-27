using Microsoft.EntityFrameworkCore;
using ResilientApi.Data.Models;

namespace ResilientApi.Data;

public class DataContext(DbContextOptions<DataContext> options) : DbContext(options)
{
   public DbSet<Car> Cars { get; set; }
   public DbSet<Owner> Owners { get; set; }
   
   protected override void OnModelCreating(ModelBuilder modelBuilder)
   {
      modelBuilder.Entity<Car>()
         .HasOne<Owner>(c => c.Owner)
         .WithMany(o => o.Cars)
         .HasForeignKey(c => c.OwnerId);
   }
}