using Api.DTOs;
using Api.ExternalServices.Interfaces;
using Api.Repositories.Interfaces;
using Api.Services.Interfaces;

namespace Api.Services;

public class RevenueService : IRevenueService
{
    private readonly IContractsRepository contractsRepository;
    private readonly INBPService nbpService;

    public RevenueService(IContractsRepository contractsRepository, INBPService nbpService)
    {
        this.contractsRepository = contractsRepository;
        this.nbpService = nbpService;
    }

    public async Task<decimal> GetCurrentRevenueAsync(RevenueDTO dto, CancellationToken cancellationToken)
    {
        var contracts = await this.contractsRepository.GetContractsAsync(dto.ForClientId, dto.ForSoftwareProductId,
            cancellationToken);

        decimal revenue = contracts.Where(c => c.IsSigned).Sum(c => c.PriceInPlnAfterDiscounts);

        return await this.GetRevenueInCurrency(revenue, dto.ForCurrency, cancellationToken);
    }

    public async Task<decimal> GetPredictedRevenueAsync(RevenueDTO dto, CancellationToken cancellationToken)
    {
        var contracts = await this.contractsRepository.GetContractsAsync(dto.ForClientId, dto.ForSoftwareProductId,
            cancellationToken);

        decimal revenue = contracts.Sum(c => c.PriceInPlnAfterDiscounts);

        return await this.GetRevenueInCurrency(revenue, dto.ForCurrency, cancellationToken);
    }

    private async Task<decimal> GetRevenueInCurrency(decimal revenue, string? currencyCode,
        CancellationToken cancellationToken)
    {
        if (currencyCode == null || currencyCode.Equals("PLN", StringComparison.InvariantCultureIgnoreCase))
            return revenue;


        decimal? exchangedRevenue =
            await this.nbpService.PriceInPlnToCurrencyAsync(revenue, currencyCode, cancellationToken);

        if (!exchangedRevenue.HasValue)
            throw new ArgumentException("Currency not found");

        return exchangedRevenue.Value;
    }
}
