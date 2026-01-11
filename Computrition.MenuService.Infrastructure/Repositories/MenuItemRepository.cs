using Dapper;
using Microsoft.Data.Sqlite;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Computrition.MenuService.Core.Entities;
using Computrition.MenuService.Core.Interfaces;
using Computrition.MenuService.Infrastructure.Data;

namespace Computrition.MenuService.Infrastructure.Repositories;

public class MenuItemRepository : IMenuItemRepository
{
    private readonly ApplicationDbContext _dbContext;
    private readonly string _connectionString;
    private readonly ITenantContext _tenantContext;

    public MenuItemRepository(
        ApplicationDbContext dbContext,
        IConfiguration configuration,
        ITenantContext tenantContext)
    {
        _dbContext = dbContext;
        _connectionString = configuration.GetConnectionString("DefaultConnection");
        _tenantContext = tenantContext;
    }

    public async Task<MenuItem> CreateAsync(MenuItem menuItem)
    {
        // Ensure the menu item has the correct tenant ID
        menuItem.TenantId = _tenantContext.TenantId;

        _dbContext.MenuItems.Add(menuItem);
        await _dbContext.SaveChangesAsync();
        return menuItem;
    }

    public async Task<IEnumerable<MenuItem>> GetAllowedMenuItemsAsync(int patientId)
    {
        var tenantId = _tenantContext.TenantId;

        using var connection = new SqliteConnection(_connectionString);

        // First, get patient's dietary restriction WITH TENANT CHECK
        var patientQuery = @"
            SELECT DietaryRestrictionCode 
            FROM Patients 
            WHERE Id = @PatientId AND TenantId = @TenantId";

        var restriction = await connection.QueryFirstOrDefaultAsync<string>(
            patientQuery,
            new { PatientId = patientId, TenantId = tenantId });

        if (restriction == null)
            return Enumerable.Empty<MenuItem>();

        // Build dynamic SQL based on restriction WITH TENANT CHECK
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
        // NONE restriction returns all items for this tenant

        return await connection.QueryAsync<MenuItem>(sql, parameters);
    }
}