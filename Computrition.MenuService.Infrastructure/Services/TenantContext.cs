using Microsoft.AspNetCore.Http;
using Computrition.MenuService.Core.Interfaces;

namespace Computrition.MenuService.Infrastructure.Services;

public class TenantContext : ITenantContext
{
    private readonly IHttpContextAccessor _httpContextAccessor;

    public TenantContext(IHttpContextAccessor httpContextAccessor)
    {
        _httpContextAccessor = httpContextAccessor;
    }

    public int TenantId
    {
        get
        {
            // Try to get tenant ID from header
            if (_httpContextAccessor.HttpContext?.Request.Headers.TryGetValue("X-Tenant-Id", out var tenantHeader) == true)
            {
                if (int.TryParse(tenantHeader, out var tenantId))
                {
                    return tenantId;
                }
            }

            // Fallback to default tenant (for development/testing)
            return 1;
        }
    }
}