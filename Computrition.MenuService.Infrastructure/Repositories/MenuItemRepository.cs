using Dapper;
using Microsoft.Data.Sqlite;
using Microsoft.EntityFrameworkCore;
using Computrition.MenuService.Core.Entities;
using Computrition.MenuService.Core.Interfaces;
using Computrition.MenuService.Infrastructure.Data;
namespace Computrition.MenuService.Infrastructure.Repositories;
using Microsoft.Extensions.Configuration;
public class MenuItemRepository : IMenuItemRepository
{
    private readonly ApplicationDbContext _dbContext;
    private readonly IConfiguration _configuration;

    public MenuItemRepository(ApplicationDbContext dbContext, IConfiguration configuration)
    {
        _dbContext = dbContext;
        _configuration = configuration;
    }

    // Using Entity Framework Core for write operations
    public async Task<MenuItem> CreateAsync(MenuItem menuItem)
    {
        _dbContext.MenuItems.Add(menuItem);
        await _dbContext.SaveChangesAsync();
        return menuItem;
    }

    // Using Dapper for high-performance read operations
    public async Task<IEnumerable<MenuItem>> GetAllowedMenuItemsAsync(int patientId, int tenantId)
    {
        var connectionString = _configuration.GetConnectionString("DefaultConnection");
        using var connection = new SqliteConnection(connectionString);

        // First, get patient's dietary restriction
        var patientQuery = @"
            SELECT DietaryRestrictionCode 
            FROM Patients 
            WHERE Id = @PatientId AND TenantId = @TenantId";

        var restriction = await connection.QueryFirstOrDefaultAsync<string>(
            patientQuery,
            new { PatientId = patientId, TenantId = tenantId });

        if (restriction == null)
            return Enumerable.Empty<MenuItem>();

        // Build dynamic SQL based on restriction
        var sql = @"
            SELECT * FROM MenuItems 
            WHERE TenantId = @TenantId";

        var parameters = new { TenantId = tenantId };

        if (restriction == "GF")
        {
            sql += " AND IsGlutenFree = 1";
        }
        else if (restriction == "SF")
        {
            sql += " AND IsSugarFree = 1";
        }
        // NONE restriction returns all items

        return await connection.QueryAsync<MenuItem>(sql, parameters);
    }
}