using api.Models;
using Api.Repositories.Interfaces;

namespace Tests.Fakes;

public class FakeSubscriptionPaymentsRepository : ISubscriptionPaymentsRepository
{
    public void AddSubscriptionPayment(SubscriptionPayment subscriptionPayment)
    {
    }

    public Task SaveChangesAsync(CancellationToken cancellationToken)
    {
        return Task.CompletedTask;
    }
}
