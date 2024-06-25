using api.Models;
using Api.Repositories.Interfaces;

namespace Tests.Fakes;

public class FakeSubscriptionsRepository : ISubscriptionsRepository
{
    public List<Subscription> addedThroughTests = new();

    public void AddSubscription(Subscription subscription)
    {
        this.addedThroughTests.Add(subscription);
    }

    public Task<Subscription?> GetSubscriptionWithPaymentsByIdAsync(Guid id, CancellationToken cancellationToken)
    {
        throw new NotImplementedException();
    }

    public Task SaveChangesAsync(CancellationToken cancellationToken)
    {
        return Task.CompletedTask;
    }
}
