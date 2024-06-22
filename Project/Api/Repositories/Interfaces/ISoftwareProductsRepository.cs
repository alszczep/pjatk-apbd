using api.Models;

namespace Api.Repositories.Interfaces;

public interface ISoftwareProductsRepository
{
    public Task<SoftwareProduct?> GetSoftwareProductWithDiscountsByIdAsync(Guid softwareProductId,
        CancellationToken cancellationToken);
}
