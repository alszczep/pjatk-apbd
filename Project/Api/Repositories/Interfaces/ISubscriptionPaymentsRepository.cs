using api.Models;

namespace Api.Repositories.Interfaces;

public interface ISubscriptionPaymentsRepository
{
    public void AddSubscriptionPayment(SubscriptionPayment subscriptionPayment);
    public Task SaveChangesAsync(CancellationToken cancellationToken);
}
