using api.Models;

namespace Api.Services.Interfaces;

public interface IContractsAndSubscriptionsSharedService
{
    Task<SoftwareProduct> GetSoftwareProductWithDiscountsByIdAsync(Guid softwareProductId,
        CancellationToken cancellationToken);

    Task<Client> GetClientWithContractsAndSubscriptionsWithPaymentsAndSoftwareProductsByIdAsync(
        Guid clientId,
        CancellationToken cancellationToken);

    void EnsureThereIsNoActiveContractWithTheSameSoftwareProductAndClient(Client client,
        SoftwareProduct softwareProduct);

    void EnsureThereIsNoActiveSubscriptionWithTheSameSoftwareProductAndClient(Client client,
        SoftwareProduct softwareProduct);

    decimal CalculateDiscountMultiplierForProduct(SoftwareProduct softwareProduct);

    decimal CalculateDiscountMultiplierForClient(Client client);
    bool IsSubscriptionActive(Subscription subscription);
}
