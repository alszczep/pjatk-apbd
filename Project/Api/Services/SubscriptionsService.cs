using Api.DTOs;
using api.Models;
using Api.Repositories.Interfaces;
using Api.Services.Interfaces;

namespace Api.Services;

public class SubscriptionsService : ISubscriptionsService
{
    private readonly IContractsAndSubscriptionsSharedService contractsAndSubscriptionsSharedService;
    private readonly ISubscriptionPaymentsRepository subscriptionPaymentsRepository;
    private readonly ISubscriptionsRepository subscriptionsRepository;

    public SubscriptionsService(IContractsAndSubscriptionsSharedService contractsAndSubscriptionsSharedService,
        ISubscriptionsRepository subscriptionsRepository,
        ISubscriptionPaymentsRepository subscriptionPaymentsRepository)
    {
        this.contractsAndSubscriptionsSharedService = contractsAndSubscriptionsSharedService;
        this.subscriptionsRepository = subscriptionsRepository;
        this.subscriptionPaymentsRepository = subscriptionPaymentsRepository;
    }

    public async Task CreateSubscriptionAsync(CreateSubscriptionDTO dto, CancellationToken cancellationToken)
    {
        EnsureRenewalPeriodIsValid(dto.RenewalPeriodInMonths);

        SoftwareProduct softwareProduct =
            await this.contractsAndSubscriptionsSharedService.GetSoftwareProductWithDiscountsByIdAsync(
                dto.SoftwareProductId,
                cancellationToken);
        Client client =
            await this.contractsAndSubscriptionsSharedService
                .GetClientWithContractsAndSubscriptionsWithPaymentsAndSoftwareProductsByIdAsync(dto.ClientId,
                    cancellationToken);

        this.contractsAndSubscriptionsSharedService.EnsureThereIsNoActiveContractWithTheSameSoftwareProductAndClient(
            client, softwareProduct);
        this.contractsAndSubscriptionsSharedService
            .EnsureThereIsNoActiveSubscriptionWithTheSameSoftwareProductAndClient(
                client, softwareProduct);

        decimal basePriceForRenewalPeriod =
            CalculateBasePriceForRenewalPeriod(softwareProduct, dto.RenewalPeriodInMonths);

        decimal productDiscountMultiplier =
            this.contractsAndSubscriptionsSharedService.CalculateDiscountMultiplierForProduct(softwareProduct);
        decimal clientDiscountMultiplier =
            this.contractsAndSubscriptionsSharedService.CalculateDiscountMultiplierForClient(client);

        Subscription subscription = new()
        {
            Id = Guid.NewGuid(),
            SoftwareProduct = softwareProduct,
            Client = client,
            RenewalPeriodInMonths = dto.RenewalPeriodInMonths,
            AddedDate = DateOnly.FromDateTime(DateTime.Now),
            BasePriceForRenewalPeriod = basePriceForRenewalPeriod * clientDiscountMultiplier,
            Payments = new List<SubscriptionPayment>
            {
                new()
                {
                    Id = Guid.NewGuid(),
                    PeriodLastDay = DateOnly.FromDateTime(DateTime.Now.AddMonths(dto.RenewalPeriodInMonths)
                        .Subtract(TimeSpan.FromDays(1))),
                    AmountPaid = basePriceForRenewalPeriod * productDiscountMultiplier * clientDiscountMultiplier
                }
            }
        };

        this.subscriptionsRepository.AddSubscription(subscription);
        await this.subscriptionsRepository.SaveChangesAsync(cancellationToken);
    }

    public async Task MakePaymentAsync(SubscriptionPaymentDTO dto, CancellationToken cancellationToken)
    {
        Subscription subscription =
            await this.GetSubscriptionWithPaymentsByIdAsync(dto.SubscriptionId, cancellationToken);

        this.EnsureSubscriptionIsActive(subscription);
        this.EnsureCurrentRenewalPeriodWasNotPaidFor(subscription);
        EnsurePaymentIsEqualToRenewalPeriodPrice(dto.PaymentAmountInPln, subscription);

        SubscriptionPayment payment = new()
        {
            Id = Guid.NewGuid(),
            AmountPaid = dto.PaymentAmountInPln,
            PeriodLastDay = subscription.Payments.Max(p => p.PeriodLastDay)
                .AddMonths(subscription.RenewalPeriodInMonths),
            Subscription = subscription
        };

        this.subscriptionPaymentsRepository.AddSubscriptionPayment(payment);
        await this.subscriptionPaymentsRepository.SaveChangesAsync(cancellationToken);
    }

    private static void EnsureRenewalPeriodIsValid(int renewalPeriodInMonths)
    {
        if (renewalPeriodInMonths < 1)
            throw new ArgumentException("Renewal period must be at least 1 month");
        if (renewalPeriodInMonths > 24)
            throw new ArgumentException("Renewal period must be at most 24 months");
    }

    private static decimal CalculateBasePriceForRenewalPeriod(SoftwareProduct softwareProduct,
        int renewalPeriodInMonths)
    {
        return softwareProduct.UpfrontYearlyPriceInPln / 12m * renewalPeriodInMonths;
    }

    private async Task<Subscription> GetSubscriptionWithPaymentsByIdAsync(Guid id, CancellationToken cancellationToken)
    {
        Subscription? subscription =
            await this.subscriptionsRepository.GetSubscriptionWithPaymentsByIdAsync(id, cancellationToken);
        if (subscription == null)
            throw new ArgumentException("Subscription not found");
        return subscription;
    }

    private void EnsureSubscriptionIsActive(Subscription subscription)
    {
        if (!this.contractsAndSubscriptionsSharedService.IsSubscriptionActive(subscription))
            throw new ArgumentException("Subscription is not active");
    }

    private void EnsureCurrentRenewalPeriodWasNotPaidFor(Subscription subscription)
    {
        if (this.contractsAndSubscriptionsSharedService.WasSubscriptionCurrentRenewalPeriodPaidFor(subscription))
            throw new ArgumentException("Current renewal period was already paid for");
    }

    private static void EnsurePaymentIsEqualToRenewalPeriodPrice(decimal paymentAmountInPln, Subscription subscription)
    {
        if (paymentAmountInPln != subscription.BasePriceForRenewalPeriod)
            throw new ArgumentException("Payment amount is not equal to renewal period price");
    }
}
