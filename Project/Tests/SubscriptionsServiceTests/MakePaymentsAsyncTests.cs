using Api.DTOs;
using Api.Services;
using Shouldly;
using Tests.Fakes;
using Xunit.Abstractions;

namespace Tests.SubscriptionsServiceTests;

public class MakePaymentsAsyncTests
{
    private readonly SubscriptionsService subscriptionsService;
    private readonly ITestOutputHelper testOutputHelper;

    public MakePaymentsAsyncTests(ITestOutputHelper testOutputHelper)
    {
        this.testOutputHelper = testOutputHelper;
        this.subscriptionsService = new SubscriptionsService(
            new ContractsAndSubscriptionsSharedService(
                new FakeClientsRepository(),
                new FakeSoftwareProductsRepository()),
            new FakeSubscriptionsRepository(),
            new FakeSubscriptionPaymentsRepository());
    }

    [Fact]
    public async void Should_ThrowException_WhenSubscriptionDoesNotExist()
    {
        await Should.ThrowAsync<ArgumentException>(this.subscriptionsService.MakePaymentAsync(new SubscriptionPaymentDTO
        {
            SubscriptionId = FakesConsts.NotExistingId,
            PaymentAmountInPln = 200
        }, CancellationToken.None));
    }

    [Fact]
    public async void Should_ThrowException_WhenSubscriptionWasAlreadyPaidFor()
    {
        await Should.ThrowAsync<ArgumentException>(this.subscriptionsService.MakePaymentAsync(new SubscriptionPaymentDTO
        {
            SubscriptionId = FakesConsts.SubscriptionActiveForSoftwareProduct1.Id,
            PaymentAmountInPln = 200
        }, CancellationToken.None));
    }

    [Fact]
    public async void Should_ThrowException_WhenSubscriptionIsInactive()
    {
        await Should.ThrowAsync<ArgumentException>(this.subscriptionsService.MakePaymentAsync(new SubscriptionPaymentDTO
        {
            SubscriptionId = FakesConsts.SubscriptionInactiveForSoftwareProduct1.Id,
            PaymentAmountInPln = 200
        }, CancellationToken.None));
    }

    [Fact]
    public async void Should_ThrowException_WhenPaymentAmountIsNotEqualToRequired()
    {
        await Should.ThrowAsync<ArgumentException>(this.subscriptionsService.MakePaymentAsync(new SubscriptionPaymentDTO
        {
            SubscriptionId = FakesConsts.SubscriptionThatCanBePaidFor.Id,
            PaymentAmountInPln = FakesConsts.SubscriptionThatCanBePaidFor.BasePriceForRenewalPeriod - 1
        }, CancellationToken.None));
    }

    [Fact]
    public async void Should_Succeed_WhenPaymentAmountIsEqualToContractPrice()
    {
        await Should.NotThrowAsync(this.subscriptionsService.MakePaymentAsync(new SubscriptionPaymentDTO
        {
            SubscriptionId = FakesConsts.SubscriptionThatCanBePaidFor.Id,
            PaymentAmountInPln = FakesConsts.SubscriptionThatCanBePaidFor.BasePriceForRenewalPeriod
        }, CancellationToken.None));
    }
}
