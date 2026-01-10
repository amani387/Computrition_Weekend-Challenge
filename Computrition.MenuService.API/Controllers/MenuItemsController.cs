using Microsoft.AspNetCore.Mvc;
using Computrition.MenuService.Core.Entities;
using Computrition.MenuService.Core.Services;

namespace Computrition.MenuService.API.Controllers;

[ApiController]
[Route("api/[controller]")]
public class MenuItemsController : ControllerBase
{
    private readonly IMenuService _menuService;
    private readonly ILogger<MenuItemsController> _logger;

    public MenuItemsController(IMenuService menuService, ILogger<MenuItemsController> logger)
    {
        _menuService = menuService;
        _logger = logger;
    }

    [HttpPost]
    public async Task<ActionResult<MenuItem>> CreateMenuItem(MenuItem menuItem)
    {
        try
        {
            // Get tenant from header (for multi-tenancy)
            if (Request.Headers.TryGetValue("X-Tenant-Id", out var tenantHeader))
            {
                if (int.TryParse(tenantHeader, out var tenantId))
                {
                    menuItem.TenantId = tenantId;
                }
            }

            var createdItem = await _menuService.CreateMenuItemAsync(menuItem);
            return CreatedAtAction(nameof(CreateMenuItem), new { id = createdItem.Id }, createdItem);
        }
        catch (ArgumentException ex)
        {
            return BadRequest(new { error = ex.Message });
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Error creating menu item");
            return StatusCode(500, new { error = "Internal server error" });
        }
    }
}