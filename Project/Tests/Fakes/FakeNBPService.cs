using Api.ExternalServices.Interfaces;

namespace Tests.Fakes;

public class FakeNBPService : INBPService
{
    public Task<decimal?> PriceInPlnToCurrencyAsync(decimal priceInPln, string targetCurrency,
        CancellationToken cancellationToken)
    {
        if (targetCurrency == FakesConsts.ExistingCurrency)
            return Task.FromResult((decimal?)(priceInPln * FakesConsts.ExistingCurrencyMultiplier));

        return Task.FromResult((decimal?)null);
    }
}
