using Computrition.MenuService.Core.Entities;
using Computrition.MenuService.Core.Interfaces;

namespace Computrition.MenuService.Core.Services;

public class MenuManagementService : IMenuService
{
    private readonly IMenuItemRepository _menuItemRepository;

    public MenuManagementService(IMenuItemRepository menuItemRepository)
    {
        _menuItemRepository = menuItemRepository;
    }

    public async Task<MenuItem> CreateMenuItemAsync(MenuItem menuItem)
    {
        if (string.IsNullOrWhiteSpace(menuItem.Name))
            throw new ArgumentException("Menu item name is required");

        return await _menuItemRepository.CreateAsync(menuItem);
    }

    public async Task<IEnumerable<MenuItem>> GetAllowedMenuItemsAsync(int patientId)
    {
        return await _menuItemRepository.GetAllowedMenuItemsAsync(patientId);
    }
}