namespace Computrition.MenuService.Core.Entities;

public class Patient
{
    public int Id { get; set; }
    public string Name { get; set; } = string.Empty;
    public string DietaryRestrictionCode { get; set; } = "NONE";
    public int TenantId { get; set; } // For multi-tenancy

    // Business logic for dietary restrictions
    public bool RequiresGlutenFree => DietaryRestrictionCode == "GF";
    public bool RequiresSugarFree => DietaryRestrictionCode == "SF";
    public bool HasNoRestrictions => DietaryRestrictionCode == "NONE";
}