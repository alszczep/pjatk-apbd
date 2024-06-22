namespace Api.ExternalServices.Interfaces;

public interface INBPService
{
    Task<decimal?> PriceInPlnToCurrencyAsync(decimal priceInPln, string targetCurrency,
        CancellationToken cancellationToken);
}
