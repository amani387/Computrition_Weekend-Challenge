using Moq;
using Computrition.MenuService.Core.Entities;
using Computrition.MenuService.Core.Interfaces;
using Computrition.MenuService.Core.Services;
using Xunit;

namespace Computrition.MenuService.Tests2.Services;

public class MenuServiceTests
{
    private readonly Mock<IMenuItemRepository> _mockRepository;
    private readonly MenuManagementService _menuService; // Changed here

    public MenuServiceTests()
    {
        _mockRepository = new Mock<IMenuItemRepository>();
        _menuService = new MenuManagementService(_mockRepository.Object); // Changed here
    }

    [Fact]
    public async Task GetAllowedMenuItemsAsync_GFPatient_ReturnsOnlyGlutenFreeItems()
    {
        // Arrange
        var patientId = 1;
        var tenantId = 1;

        var testMenuItems = new List<MenuItem>
        {
            new() { Id = 1, Name = "Gluten Free Bread", IsGlutenFree = true, TenantId = tenantId },
            new() { Id = 2, Name = "Regular Pasta", IsGlutenFree = false, TenantId = tenantId },
            new() { Id = 3, Name = "Fruit Salad", IsGlutenFree = true, TenantId = tenantId }
        };

        _mockRepository.Setup(repo => repo.GetAllowedMenuItemsAsync(patientId, tenantId))
                      .ReturnsAsync(testMenuItems.Where(i => i.IsGlutenFree));

        // Act
        var result = await _menuService.GetAllowedMenuItemsAsync(patientId, tenantId);

        // Assert
        Assert.All(result, item => Assert.True(item.IsGlutenFree));
        Assert.Equal(2, result.Count());
    }

    [Fact]
    public async Task CreateMenuItemAsync_WithEmptyName_ThrowsArgumentException()
    {
        // Arrange
        var menuItem = new MenuItem { Name = "", TenantId = 1 };

        // Act & Assert
        await Assert.ThrowsAsync<ArgumentException>(() =>
            _menuService.CreateMenuItemAsync(menuItem));
    }

    [Fact]
    public async Task CreateMenuItemAsync_ValidItem_CallsRepository()
    {
        // Arrange
        var menuItem = new MenuItem
        {
            Name = "Test Item",
            Category = "Test",
            TenantId = 1
        };

        _mockRepository.Setup(repo => repo.CreateAsync(menuItem))
                      .ReturnsAsync(menuItem);

        // Act
        var result = await _menuService.CreateMenuItemAsync(menuItem);

        // Assert
        Assert.Equal(menuItem.Name, result.Name);
        _mockRepository.Verify(repo => repo.CreateAsync(menuItem), Times.Once);
    }
}