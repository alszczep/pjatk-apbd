using api.Models;
using Api.Repositories.Interfaces;

namespace Tests.Fakes;

public class FakeSubscriptionsRepository : ISubscriptionsRepository
{
    public List<Subscription> addedThroughTests { get; } = new();

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

    public Task<List<Subscription>> GetSubscriptionsWithPaymentsAsync(CancellationToken cancellationToken)
    {
        return Task.FromResult(new List<Subscription>
        {
            new()
            {
                SoftwareProduct = FakesConsts.SoftwareProduct1,
                Client = FakesConsts.ClientCompany1,
                Id = Guid.Parse("00000000-0000-0000-0000-000000000001"),
                BasePriceForRenewalPeriod = 200,
                RenewalPeriodInMonths = 12,
                Payments = new List<SubscriptionPayment>
                {
                    new()
                    {
                        AmountPaid = 150,
                        PeriodLastDay = DateOnly.FromDateTime(DateTime.Now.Subtract(TimeSpan.FromDays(5)))
                    },
                    new()
                    {
                        AmountPaid = 200,
                        PeriodLastDay = DateOnly.FromDateTime(DateTime.Now.AddMonths(12).Subtract(TimeSpan.FromDays(5)))
                    }
                }
            },
            new()
            {
                SoftwareProduct = FakesConsts.SoftwareProduct1,
                Client = FakesConsts.ClientIndividual1,
                Id = Guid.Parse("00000000-0000-0000-0000-000000000002"),
                BasePriceForRenewalPeriod = 350,
                RenewalPeriodInMonths = 6,
                Payments = new List<SubscriptionPayment>
                {
                    new()
                    {
                        AmountPaid = 300,
                        PeriodLastDay = DateOnly.FromDateTime(DateTime.Now.Subtract(TimeSpan.FromDays(5)))
                    }
                }
            },
            new()
            {
                SoftwareProduct = FakesConsts.SoftwareProduct2,
                Client = FakesConsts.ClientCompany1,
                Id = Guid.Parse("00000000-0000-0000-0000-000000000003"),
                BasePriceForRenewalPeriod = 500,
                RenewalPeriodInMonths = 6,
                Payments = new List<SubscriptionPayment>
                {
                    new()
                    {
                        AmountPaid = 400,
                        PeriodLastDay = DateOnly.FromDateTime(DateTime.Now.Subtract(TimeSpan.FromDays(5)))
                    }
                }
            }
        });
    }

    public Task SaveChangesAsync(CancellationToken cancellationToken)
    {
        return Task.CompletedTask;
    }
}
