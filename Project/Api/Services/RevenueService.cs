using Api.DTOs;
using Api.ExternalServices.Interfaces;
using api.Models;
using Api.Repositories.Interfaces;
using Api.Services.Interfaces;

namespace Api.Services;

public class RevenueService : IRevenueService
{
    private readonly IContractsAndSubscriptionsSharedService contractsAndSubscriptionsSharedService;
    private readonly IContractsRepository contractsRepository;
    private readonly INBPService nbpService;
    private readonly ISubscriptionsRepository subscriptionsRepository;

    public RevenueService(IContractsAndSubscriptionsSharedService contractsAndSubscriptionsSharedService,
        IContractsRepository contractsRepository,
        INBPService nbpService,
        ISubscriptionsRepository subscriptionsRepository)
    {
        this.contractsAndSubscriptionsSharedService = contractsAndSubscriptionsSharedService;
        this.contractsRepository = contractsRepository;
        this.nbpService = nbpService;
        this.subscriptionsRepository = subscriptionsRepository;
    }

    public async Task<decimal> GetCurrentRevenueAsync(RevenueDTO dto, CancellationToken cancellationToken)
    {
        var contracts = await this.GetFilteredContracts(dto.ForClientId, dto.ForSoftwareProductId, cancellationToken);
        var subscriptions =
            await this.GetFilteredSubscriptions(dto.ForClientId, dto.ForSoftwareProductId, cancellationToken);

        decimal contractsRevenue = contracts.Where(c => c.IsSigned).Sum(c => c.PriceInPlnAfterDiscounts);
        decimal subscriptionsRevenue = subscriptions.Sum(s => s.Payments.Sum(p => p.AmountPaid));

        return await this.GetRevenueInCurrency(contractsRevenue + subscriptionsRevenue, dto.ForCurrency,
            cancellationToken);
    }

    public async Task<decimal> GetPredictedRevenueAsync(RevenueDTO dto, CancellationToken cancellationToken)
    {
        var contracts = await this.GetFilteredContracts(dto.ForClientId, dto.ForSoftwareProductId, cancellationToken);
        var subscriptions =
            await this.GetFilteredSubscriptions(dto.ForClientId, dto.ForSoftwareProductId, cancellationToken);

        decimal contractsRevenue = contracts.Sum(c => c.PriceInPlnAfterDiscounts);
        decimal subscriptionsCurrentRevenue = subscriptions.Sum(s => s.Payments.Sum(p => p.AmountPaid));
        decimal subscriptionsPredictedRevenue = subscriptions
            .Where(s => this.contractsAndSubscriptionsSharedService.IsSubscriptionActive(s)
                        && !this.contractsAndSubscriptionsSharedService.WasSubscriptionCurrentRenewalPeriodPaidFor(s))
            // assumes that client will pay for the next renewal period, bcs ofc we cannot count it infinitely
            .Sum(s => s.BasePriceForRenewalPeriod);


        return await this.GetRevenueInCurrency(
            contractsRevenue + subscriptionsCurrentRevenue + subscriptionsPredictedRevenue, dto.ForCurrency,
            cancellationToken);
    }

    private async Task<List<Contract>> GetFilteredContracts(Guid? clientId, Guid? softwareProductId,
        CancellationToken cancellationToken)
    {
        var contracts = await this.contractsRepository.GetContractsAsync(cancellationToken);

        if (clientId != null)
            contracts = contracts.Where(c => c.Client.Id == clientId).ToList();

        if (softwareProductId != null)
            contracts = contracts.Where(c => c.SoftwareProduct.Id == softwareProductId).ToList();

        return contracts.ToList();
    }

    private async Task<List<Subscription>> GetFilteredSubscriptions(Guid? clientId, Guid? softwareProductId,
        CancellationToken cancellationToken)
    {
        var subscriptions = await this.subscriptionsRepository.GetSubscriptionsWithPaymentsAsync(cancellationToken);

        if (clientId != null)
            subscriptions = subscriptions.Where(s => s.Client.Id == clientId).ToList();

        if (softwareProductId != null)
            subscriptions = subscriptions.Where(s => s.SoftwareProduct.Id == softwareProductId).ToList();

        return subscriptions.ToList();
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
