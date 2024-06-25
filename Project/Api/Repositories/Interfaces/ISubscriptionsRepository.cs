using api.Models;

namespace Api.Repositories.Interfaces;

public interface ISubscriptionsRepository
{
    public void AddSubscription(Subscription subscription);
    public Task<Subscription?> GetSubscriptionWithPaymentsByIdAsync(Guid id, CancellationToken cancellationToken);
    public Task<List<Subscription>> GetSubscriptionsWithPaymentsAsync(CancellationToken cancellationToken);
    public Task SaveChangesAsync(CancellationToken cancellationToken);
}
