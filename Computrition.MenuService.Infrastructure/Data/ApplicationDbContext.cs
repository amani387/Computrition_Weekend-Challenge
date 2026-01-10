
using System.Collections.Generic;
using System.Reflection.Emit;
using Microsoft.EntityFrameworkCore;
using Computrition.MenuService.Core.Entities;
namespace Computrition.MenuService.Infrastructure.Data;

public class ApplicationDbContext : DbContext
{
    public ApplicationDbContext(DbContextOptions<ApplicationDbContext> options)
        : base(options) { }

    public DbSet<Patient> Patients { get; set; }
    public DbSet<MenuItem> MenuItems { get; set; }

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        base.OnModelCreating(modelBuilder);

        // Configure multi-tenancy filters (optional, can be handled in queries)
        // modelBuilder.Entity<Patient>().HasQueryFilter(p => p.TenantId == 1); // We'll handle in repository

        // Seed data for testing
        modelBuilder.Entity<Patient>().HasData(
            new Patient { Id = 1, Name = "John Doe", DietaryRestrictionCode = "GF", TenantId = 1 },
            new Patient { Id = 2, Name = "Jane Smith", DietaryRestrictionCode = "SF", TenantId = 1 },
            new Patient { Id = 3, Name = "Bob Johnson", DietaryRestrictionCode = "NONE", TenantId = 1 }
        );

        modelBuilder.Entity<MenuItem>().HasData(
            new MenuItem { Id = 1, Name = "Grilled Chicken", Category = "Main", IsGlutenFree = true, IsSugarFree = true, IsHeartHealthy = true, TenantId = 1 },
            new MenuItem { Id = 2, Name = "Whole Wheat Pasta", Category = "Main", IsGlutenFree = false, IsSugarFree = true, IsHeartHealthy = true, TenantId = 1 },
            new MenuItem { Id = 3, Name = "Chocolate Cake", Category = "Dessert", IsGlutenFree = false, IsSugarFree = false, IsHeartHealthy = false, TenantId = 1 },
            new MenuItem { Id = 4, Name = "Fruit Salad", Category = "Dessert", IsGlutenFree = true, IsSugarFree = true, IsHeartHealthy = true, TenantId = 1 }
        );
    }
}