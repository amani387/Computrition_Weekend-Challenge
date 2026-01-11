using Microsoft.EntityFrameworkCore;
using Computrition.MenuService.Core.Entities;
using Computrition.MenuService.Core.Interfaces;

namespace Computrition.MenuService.Infrastructure.Data;

public class ApplicationDbContext : DbContext
{
    private readonly ITenantContext _tenantContext;

    public ApplicationDbContext(
        DbContextOptions<ApplicationDbContext> options,
        ITenantContext tenantContext)
        : base(options)
    {
        _tenantContext = tenantContext;
    }

    public DbSet<Patient> Patients { get; set; }
    public DbSet<MenuItem> MenuItems { get; set; }

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        base.OnModelCreating(modelBuilder);

        // Dynamic tenant filtering based on current request
        modelBuilder.Entity<Patient>().HasQueryFilter(p => p.TenantId == _tenantContext.TenantId);
        modelBuilder.Entity<MenuItem>().HasQueryFilter(m => m.TenantId == _tenantContext.TenantId);

        // Seed data for testing
        modelBuilder.Entity<Patient>().HasData(
            new Patient { Id = 1, Name = "John Doe", DietaryRestrictionCode = "GF", TenantId = 1 },
            new Patient { Id = 2, Name = "Jane Smith", DietaryRestrictionCode = "SF", TenantId = 1 },
            new Patient { Id = 3, Name = "Bob Johnson", DietaryRestrictionCode = "NONE", TenantId = 1 },
            // Tenant 2 patients
            new Patient { Id = 4, Name = "Alice Brown", DietaryRestrictionCode = "GF", TenantId = 2 },
            new Patient { Id = 5, Name = "Charlie Davis", DietaryRestrictionCode = "NONE", TenantId = 2 }
        );

        modelBuilder.Entity<MenuItem>().HasData(
            new MenuItem { Id = 1, Name = "Grilled Chicken", Category = "Main", IsGlutenFree = true, IsSugarFree = true, IsHeartHealthy = true, TenantId = 1 },
            new MenuItem { Id = 2, Name = "Whole Wheat Pasta", Category = "Main", IsGlutenFree = false, IsSugarFree = true, IsHeartHealthy = true, TenantId = 1 },
            new MenuItem { Id = 3, Name = "Chocolate Cake", Category = "Dessert", IsGlutenFree = false, IsSugarFree = false, IsHeartHealthy = false, TenantId = 1 },
            new MenuItem { Id = 4, Name = "Fruit Salad", Category = "Dessert", IsGlutenFree = true, IsSugarFree = true, IsHeartHealthy = true, TenantId = 1 },
            // Tenant 2 menu items
            new MenuItem { Id = 5, Name = "Vegan Burger", Category = "Main", IsGlutenFree = true, IsSugarFree = true, IsHeartHealthy = true, TenantId = 2 },
            new MenuItem { Id = 6, Name = "Quinoa Salad", Category = "Main", IsGlutenFree = true, IsSugarFree = true, IsHeartHealthy = true, TenantId = 2 },
            new MenuItem { Id = 7, Name = "Apple Pie", Category = "Dessert", IsGlutenFree = false, IsSugarFree = false, IsHeartHealthy = false, TenantId = 2 }
        );
    }
}