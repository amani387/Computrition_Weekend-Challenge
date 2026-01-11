using Moq;
using Computrition.MenuService.Core.Entities;
using Computrition.MenuService.Core.Interfaces;
using Computrition.MenuService.Core.Services;
using Xunit;

namespace Computrition.MenuService.Tests2.Services;

public class MenuServiceTests
{
    private readonly Mock<IMenuItemRepository> _mockRepository;
    private readonly Mock<ITenantContext> _mockTenantContext;
    private readonly MenuManagementService _menuService;

    public MenuServiceTests()
    {
        _mockRepository = new Mock<IMenuItemRepository>();
        _mockTenantContext = new Mock<ITenantContext>();
        _mockTenantContext.Setup(t => t.TenantId).Returns(1);

        _menuService = new MenuManagementService(_mockRepository.Object);
    }

    [Fact]
    public async Task GetAllowedMenuItemsAsync_GFPatient_ReturnsOnlyGlutenFreeItems()
    {
        // Arrange
        var patientId = 1;

        var testMenuItems = new List<MenuItem>
        {
            new() { Id = 1, Name = "Gluten Free Bread", IsGlutenFree = true, TenantId = 1 },
            new() { Id = 2, Name = "Regular Pasta", IsGlutenFree = false, TenantId = 1 },
            new() { Id = 3, Name = "Fruit Salad", IsGlutenFree = true, TenantId = 1 }
        };

        _mockRepository.Setup(repo => repo.GetAllowedMenuItemsAsync(patientId))
                      .ReturnsAsync(testMenuItems.Where(i => i.IsGlutenFree));

        // Act
        var result = await _menuService.GetAllowedMenuItemsAsync(patientId);

        // Assert
        Assert.All(result, item => Assert.True(item.IsGlutenFree));
        Assert.Equal(2, result.Count());
    }

    
}