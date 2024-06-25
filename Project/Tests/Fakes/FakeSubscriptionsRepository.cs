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
        if (id == FakesConsts.SubscriptionActiveForSoftwareProduct1.Id)
            return Task.FromResult<Subscription?>(FakesConsts.SubscriptionActiveForSoftwareProduct1);
        if (id == FakesConsts.SubscriptionInactiveForSoftwareProduct1.Id)
            return Task.FromResult<Subscription?>(FakesConsts.SubscriptionInactiveForSoftwareProduct1);
        if (id == FakesConsts.SubscriptionThatCanBePaidFor.Id)
            return Task.FromResult<Subscription?>(FakesConsts.SubscriptionThatCanBePaidFor);
        return Task.FromResult<Subscription?>(null);
    }

    public Task SaveChangesAsync(CancellationToken cancellationToken)
    {
        return Task.CompletedTask;
    }
}
