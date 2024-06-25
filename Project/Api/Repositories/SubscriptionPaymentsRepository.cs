using Api.Context;
using api.Models;
using Api.Repositories.Interfaces;

namespace Api.Repositories;

public class SubscriptionPaymentsRepository : ISubscriptionPaymentsRepository
{
    private readonly ProjectContext projectContext;

    public SubscriptionPaymentsRepository(ProjectContext projectContext)
    {
        this.projectContext = projectContext;
    }

    public void AddSubscriptionPayment(SubscriptionPayment subscriptionPayment)
    {
        this.projectContext.SubscriptionPayments.Add(subscriptionPayment);
    }

    public Task SaveChangesAsync(CancellationToken cancellationToken)
    {
        return this.projectContext.SaveChangesAsync(cancellationToken);
    }
}
