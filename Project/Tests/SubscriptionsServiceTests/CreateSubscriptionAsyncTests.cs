using Api.DTOs;
using api.Models;
using Api.Services;
using Shouldly;
using Tests.Fakes;
using Xunit.Abstractions;

namespace Tests.SubscriptionsServiceTests;

public class CreateSubscriptionAsyncTests
{
    private readonly FakeSubscriptionsRepository fakeSubscriptionsRepository;
    private readonly SubscriptionsService subscriptionsService;
    private readonly ITestOutputHelper testOutputHelper;

    public CreateSubscriptionAsyncTests(ITestOutputHelper testOutputHelper)
    {
        this.testOutputHelper = testOutputHelper;
        this.fakeSubscriptionsRepository = new FakeSubscriptionsRepository();
        this.subscriptionsService = new SubscriptionsService(
            new ContractsAndSubscriptionsSharedService(
                new FakeClientsRepository(),
                new FakeSoftwareProductsRepository()),
            this.fakeSubscriptionsRepository,
            new FakeSubscriptionPaymentsRepository());
    }

    [Fact]
    public async void Should_ThrowException_WhenRenewalPeriodIsInvalid()
    {
        await Should.ThrowAsync<ArgumentException>(this.subscriptionsService.CreateSubscriptionAsync(
            new CreateSubscriptionDTO
            {
                ClientId = FakesConsts.ClientCompany1.Id,
                SoftwareProductId = FakesConsts.SoftwareProduct1.Id,
                RenewalPeriodInMonths = 0
            }, CancellationToken.None));

        await Should.ThrowAsync<ArgumentException>(this.subscriptionsService.CreateSubscriptionAsync(
            new CreateSubscriptionDTO
            {
                ClientId = FakesConsts.ClientCompany1.Id,
                SoftwareProductId = FakesConsts.SoftwareProduct1.Id,
                RenewalPeriodInMonths = 25
            }, CancellationToken.None));
    }

    [Fact]
    public async void Should_ThrowException_WhenSoftwareProductDoesNotExist()
    {
        await Should.ThrowAsync<ArgumentException>(this.subscriptionsService.CreateSubscriptionAsync(
            new CreateSubscriptionDTO
            {
                ClientId = FakesConsts.ClientCompany1.Id,
                SoftwareProductId = FakesConsts.NotExistingId,
                RenewalPeriodInMonths = 6
            }, CancellationToken.None));
    }

    [Fact]
    public async void Should_ThrowException_WhenClientDoesNotExist()
    {
        await Should.ThrowAsync<ArgumentException>(this.subscriptionsService.CreateSubscriptionAsync(
            new CreateSubscriptionDTO
            {
                ClientId = FakesConsts.NotExistingId,
                SoftwareProductId = FakesConsts.SoftwareProduct2.Id,
                RenewalPeriodInMonths = 6
            }, CancellationToken.None));
    }

    [Fact]
    public async void Should_ThrowException_WhenClientHasUnsignedContractForTheSameSoftwareProduct()
    {
        await Should.ThrowAsync<ArgumentException>(this.subscriptionsService.CreateSubscriptionAsync(
            new CreateSubscriptionDTO
            {
                ClientId = FakesConsts.ClientIndividual1.Id,
                SoftwareProductId = FakesConsts.SoftwareProduct1.Id,
                RenewalPeriodInMonths = 6
            }, CancellationToken.None));
    }

    [Fact]
    public async void Should_ThrowException_WhenClientHasSignedAndActiveContractForTheSameSoftwareProduct()
    {
        await Should.ThrowAsync<ArgumentException>(this.subscriptionsService.CreateSubscriptionAsync(
            new CreateSubscriptionDTO
            {
                ClientId = FakesConsts.ClientIndividual2.Id,
                SoftwareProductId = FakesConsts.SoftwareProduct1.Id,
                RenewalPeriodInMonths = 6
            }, CancellationToken.None));
    }

    [Fact]
    public async void Should_ThrowException_WhenClientHasActiveSubscriptionForTheSameSoftwareProduct()
    {
        await Should.ThrowAsync<ArgumentException>(this.subscriptionsService.CreateSubscriptionAsync(
            new CreateSubscriptionDTO
            {
                ClientId = FakesConsts.ClientIndividual3.Id,
                SoftwareProductId = FakesConsts.SoftwareProduct1.Id,
                RenewalPeriodInMonths = 6
            }, CancellationToken.None));
    }

    [Fact]
    public async void Should_SetCorrectPriceWithClientDiscount_WhenDataIsValid()
    {
        await Should.NotThrowAsync(this.subscriptionsService.CreateSubscriptionAsync(new CreateSubscriptionDTO
        {
            ClientId = FakesConsts.ClientCompany1.Id,
            SoftwareProductId = FakesConsts.SoftwareProduct1.Id,
            RenewalPeriodInMonths = 3
        }, CancellationToken.None));

        this.fakeSubscriptionsRepository.addedThroughTests.Count.ShouldBe(1);
        Subscription addedSubscription = this.fakeSubscriptionsRepository.addedThroughTests[0];
        addedSubscription.BasePriceForRenewalPeriod.ShouldBe(2850);

        addedSubscription.Payments.Count.ShouldBe(1);
        SubscriptionPayment addedPayment = addedSubscription.Payments.ToList()[0];
        addedPayment.AmountPaid.ShouldBe(2850);
    }

    [Fact]
    public async void Should_SetCorrectPriceWithProductDiscount_WhenDataIsValid()
    {
        await Should.NotThrowAsync(this.subscriptionsService.CreateSubscriptionAsync(new CreateSubscriptionDTO
        {
            ClientId = FakesConsts.ClientIndividual4.Id,
            SoftwareProductId = FakesConsts.SoftwareProduct2.Id,
            RenewalPeriodInMonths = 6
        }, CancellationToken.None));

        this.fakeSubscriptionsRepository.addedThroughTests.Count.ShouldBe(1);
        Subscription addedSubscription = this.fakeSubscriptionsRepository.addedThroughTests[0];
        addedSubscription.BasePriceForRenewalPeriod.ShouldBe(12000);

        addedSubscription.Payments.Count.ShouldBe(1);
        SubscriptionPayment addedPayment = addedSubscription.Payments.ToList()[0];
        addedPayment.AmountPaid.ShouldBe(10800);
    }
}
