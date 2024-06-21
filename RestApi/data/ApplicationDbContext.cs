using Microsoft.EntityFrameworkCore;
using RestApi.Models;

namespace RestApi.enums;

public class ApplicationDbContext : DbContext
{
    public ApplicationDbContext(DbContextOptions<ApplicationDbContext> options)
        : base(options)
    {
    }

    public DbSet<VillaDTO> Villa { get; set; }

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        modelBuilder.Entity<VillaDTO>().HasData(new VillaDTO
        {
            Id = "1",
            Name = "Joe",
            DateTime = DateTime.Now
        }, new VillaDTO
        {
            Id = "2",
            Name = "David",
            DateTime = DateTime.Now
        });
    }
}