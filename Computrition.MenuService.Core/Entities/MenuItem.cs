namespace Computrition.MenuService.Core.Entities;

public class MenuItem
{
    public int Id { get; set; }
    public string Name { get; set; } = string.Empty;
    public string Category { get; set; } = string.Empty;
    public bool IsGlutenFree { get; set; }
    public bool IsSugarFree { get; set; }
    public bool IsHeartHealthy { get; set; }
    public int TenantId { get; set; } // For multi-tenancy
}