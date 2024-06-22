using Api.Context;
using api.Models;
using Api.Repositories.Interfaces;
using Microsoft.EntityFrameworkCore;

namespace Api.Repositories;

public class SoftwareProductsRepository : ISoftwareProductsRepository
{
    private readonly ProjectContext projectContext;

    public SoftwareProductsRepository(ProjectContext projectContext)
    {
        this.projectContext = projectContext;
    }

    public Task<SoftwareProduct?> GetSoftwareProductWithDiscountsByIdAsync(Guid softwareProductId,
        CancellationToken cancellationToken)
    {
        return this.projectContext.SoftwareProducts
            .Include(sp => sp.Discounts)
            .FirstOrDefaultAsync(sp => sp.Id == softwareProductId, cancellationToken);
    }
}
