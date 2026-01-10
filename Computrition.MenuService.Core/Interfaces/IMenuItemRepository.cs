using Computrition.MenuService.Core.Entities;

namespace Computrition.MenuService.Core.Interfaces;

public interface IMenuItemRepository
{
    Task<MenuItem> CreateAsync(MenuItem menuItem);
    Task<IEnumerable<MenuItem>> GetAllowedMenuItemsAsync(int patientId, int tenantId);
}