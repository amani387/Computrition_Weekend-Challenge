using Microsoft.AspNetCore.Mvc;
using Computrition.MenuService.Core.Services;

namespace Computrition.MenuService.API.Controllers;

[ApiController]
[Route("api/[controller]")]
public class PatientsController : ControllerBase
{
    private readonly IMenuService _menuService;
    private readonly ILogger<PatientsController> _logger;

    public PatientsController(
        IMenuService menuService,
        ILogger<PatientsController> logger)
    {
        _menuService = menuService;
        _logger = logger;
    }

    [HttpGet("{patientId}/allowed-menu")]
    public async Task<IActionResult> GetAllowedMenu(int patientId)
    {
        try
        {
            var allowedItems = await _menuService.GetAllowedMenuItemsAsync(patientId);

            if (!allowedItems.Any())
            {
                return NotFound(new
                {
                    message = "Patient not found or no menu items available for this tenant"
                });
            }

            return Ok(allowedItems);
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Error retrieving allowed menu items");
            return StatusCode(500, new { error = "Internal server error" });
        }
    }
}