using Computrition.MenuService.Core.Entities;

namespace Computrition.MenuService.Core.Services;

public interface IMenuService
{
    Task<MenuItem> CreateMenuItemAsync(MenuItem menuItem);
    Task<IEnumerable<MenuItem>> GetAllowedMenuItemsAsync(int patientId); // Removed tenantId parameter
}