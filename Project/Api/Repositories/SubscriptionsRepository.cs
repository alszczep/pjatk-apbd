using Api.Context;
using api.Models;
using Api.Repositories.Interfaces;
using Microsoft.EntityFrameworkCore;

namespace Api.Repositories;

public class SubscriptionsRepository : ISubscriptionsRepository
{
    private readonly ProjectContext projectContext;

    public SubscriptionsRepository(ProjectContext projectContext)
    {
        this.projectContext = projectContext;
    }

    public void AddSubscription(Subscription subscription)
    {
        this.projectContext.Subscriptions.Add(subscription);
    }

    public Task<Subscription?> GetSubscriptionWithPaymentsByIdAsync(Guid id, CancellationToken cancellationToken)
    {
        return this.projectContext.Subscriptions
            .Include(s => s.Payments)
            .FirstOrDefaultAsync(s => s.Id == id, cancellationToken);
    }

    public Task SaveChangesAsync(CancellationToken cancellationToken)
    {
        return this.projectContext.SaveChangesAsync(cancellationToken);
    }
}
