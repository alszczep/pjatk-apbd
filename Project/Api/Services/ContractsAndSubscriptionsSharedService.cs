using Api.Helpers;
using api.Models;
using Api.Repositories.Interfaces;
using Api.Services.Interfaces;

namespace Api.Services;

public class ContractsAndSubscriptionsSharedService : IContractsAndSubscriptionsSharedService
{
    private readonly IClientsRepository clientsRepository;
    private readonly ISoftwareProductsRepository softwareProductsRepository;

    public ContractsAndSubscriptionsSharedService(IClientsRepository clientsRepository,
        ISoftwareProductsRepository softwareProductsRepository)
    {
        this.clientsRepository = clientsRepository;
        this.softwareProductsRepository = softwareProductsRepository;
    }

    public async Task<Client> GetClientWithContractsAndSubscriptionsWithPaymentsAndSoftwareProductsByIdAsync(
        Guid clientId,
        CancellationToken cancellationToken)
    {
        Client? client =
            await this.clientsRepository.GetClientWithContractsAndSubscriptionsWithPaymentsAndSoftwareProductsByIdAsync(
                clientId,
                cancellationToken);
        if (client == null)
            throw new ArgumentException("Client not found");
        return client;
    }

    public void EnsureThereIsNoActiveContractWithTheSameSoftwareProductAndClient(Client client,
        SoftwareProduct softwareProduct)
    {
        if (client.Contracts.Any(c => c.SoftwareProduct.Id == softwareProduct.Id && IsContractNotSignedOrInEffect(c)))
            throw new ArgumentException("Client already has a contract with this software product");
    }

    public void EnsureThereIsNoActiveSubscriptionWithTheSameSoftwareProductAndClient(Client client,
        SoftwareProduct softwareProduct)
    {
        if (client.Subscriptions.Any(s => s.SoftwareProduct.Id == softwareProduct.Id && this.IsSubscriptionActive(s)))
            throw new ArgumentException("Client already has a subscription with this software product");
    }

    public async Task<SoftwareProduct> GetSoftwareProductWithDiscountsByIdAsync(Guid softwareProductId,
        CancellationToken cancellationToken)
    {
        SoftwareProduct? softwareProduct =
            await this.softwareProductsRepository.GetSoftwareProductWithDiscountsByIdAsync(softwareProductId,
                cancellationToken);
        if (softwareProduct == null)
            throw new ArgumentException("Software product not found");
        return softwareProduct;
    }

    public decimal CalculateDiscountMultiplierForProduct(SoftwareProduct softwareProduct)
    {
        var activeDiscounts = softwareProduct.Discounts
            .Where(d => d.StartDate <= DateTime.Now && d.EndDate >= DateTime.Now).ToList();

        return MathHelpers.PercentageToMultiplier(activeDiscounts.Count == 0
            ? 0
            : activeDiscounts.Max(d => d.DiscountPercentage));
    }

    public decimal CalculateDiscountMultiplierForClient(Client client)
    {
        return client.Contracts.Any(c => c.IsSigned) || client.Subscriptions.Count > 0
            ? Discount.ReturningClientDiscountMultiplier
            : 1m;
    }

    public bool IsSubscriptionActive(Subscription subscription)
    {
        if (subscription.Payments.Count == 0) return false;

        // checks if the last renewal period was paid for
        return subscription.Payments.Max(p => p.PeriodLastDay).AddMonths(subscription.RenewalPeriodInMonths) >=
               DateOnly.FromDateTime(DateTime.Now);
    }

    public bool WasSubscriptionCurrentRenewalPeriodPaidFor(Subscription subscription)
    {
        return subscription.Payments.Max(p => p.PeriodLastDay) >= DateOnly.FromDateTime(DateTime.Now);
    }

    private static bool IsContractNotSignedOrInEffect(Contract contract)
    {
        if (!contract.IsSigned) return true;

        if (!contract.SignedDate.HasValue)
            throw new ArgumentException("Contract is signed but has no signed date");

        return contract.SignedDate.Value.AddYears(1 + contract.YearsOfExtendedSupport) > DateTime.Now;
    }
}
