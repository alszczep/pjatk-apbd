using api.Models;
using Api.Repositories.Interfaces;

namespace Tests.Fakes;

public class FakeSoftwareProductsRepository : ISoftwareProductsRepository
{
    public Task<SoftwareProduct?> GetSoftwareProductWithDiscountsByIdAsync(Guid softwareProductId,
        CancellationToken cancellationToken)
    {
        if (softwareProductId == FakesConsts.SoftwareProduct1.Id)
            return Task.FromResult<SoftwareProduct?>(FakesConsts.SoftwareProduct1);
        if (softwareProductId == FakesConsts.SoftwareProduct2.Id)
            return Task.FromResult<SoftwareProduct?>(FakesConsts.SoftwareProduct2);
        return Task.FromResult<SoftwareProduct?>(null);
    }
}
