using Api.DTOs;
using Api.Services;
using Shouldly;
using Tests.Fakes;
using Xunit.Abstractions;

namespace Tests.RevenueServiceTests;

public class GetPredictedRevenueAsync
{
    private readonly RevenueService revenueService;
    private readonly ITestOutputHelper testOutputHelper;

    public GetPredictedRevenueAsync(ITestOutputHelper testOutputHelper)
    {
        this.testOutputHelper = testOutputHelper;
        this.revenueService = new RevenueService(
            new ContractsAndSubscriptionsSharedService(
                new FakeClientsRepository(),
                new FakeSoftwareProductsRepository()),
            new FakeContractsRepository(),
            new FakeNBPService(),
            new FakeSubscriptionsRepository());
    }

    [Fact]
    public async void Should_ReturnZero_WhenNoContractsForSoftwareProduct()
    {
        decimal result = await this.revenueService.GetPredictedRevenueAsync(new RevenueDTO
        {
            ForClientId = null,
            ForSoftwareProductId = FakesConsts.NotExistingId,
            ForCurrency = null
        }, CancellationToken.None);

        result.ShouldBe(0);
    }

    [Fact]
    public async void Should_ReturnZero_WhenNoContractsForClient()
    {
        decimal result = await this.revenueService.GetPredictedRevenueAsync(new RevenueDTO
        {
            ForClientId = FakesConsts.NotExistingId,
            ForSoftwareProductId = null,
            ForCurrency = null
        }, CancellationToken.None);

        result.ShouldBe(0);
    }

    [Fact]
    public async void Should_ThrowException_WhenCurrencyDoesNotExist()
    {
        await Should.ThrowAsync<ArgumentException>(this.revenueService.GetPredictedRevenueAsync(new RevenueDTO
        {
            ForClientId = null,
            ForSoftwareProductId = null,
            ForCurrency = "FakeCurrency"
        }, CancellationToken.None));
    }

    [Fact]
    public async void Should_CorrectRevenue_WhenNoArgumentsArePassed()
    {
        decimal result = await this.revenueService.GetPredictedRevenueAsync(new RevenueDTO
        {
            ForClientId = null,
            ForSoftwareProductId = null,
            ForCurrency = null
        }, CancellationToken.None);

        decimal expectedContractsRevenue = 11000;
        decimal expectedSubscriptionsRevenue = 1900;
        result.ShouldBe(expectedContractsRevenue + expectedSubscriptionsRevenue);
    }

    [Fact]
    public async void Should_CorrectRevenue_WhenExistingSoftwareProductIdIsPassed()
    {
        decimal result = await this.revenueService.GetPredictedRevenueAsync(new RevenueDTO
        {
            ForClientId = null,
            ForSoftwareProductId = FakesConsts.SoftwareProduct1.Id,
            ForCurrency = null
        }, CancellationToken.None);

        decimal expectedContractsRevenue = 9000;
        decimal expectedSubscriptionsRevenue = 1000;
        result.ShouldBe(expectedContractsRevenue + expectedSubscriptionsRevenue);
    }

    [Fact]
    public async void Should_CorrectRevenue_WhenExistingClientIdIsPassed()
    {
        decimal result = await this.revenueService.GetPredictedRevenueAsync(new RevenueDTO
        {
            ForClientId = FakesConsts.ClientIndividual1.Id,
            ForSoftwareProductId = null,
            ForCurrency = null
        }, CancellationToken.None);

        decimal expectedContractsRevenue = 7000;
        decimal expectedSubscriptionsRevenue = 650;
        result.ShouldBe(expectedContractsRevenue + expectedSubscriptionsRevenue);
    }

    [Fact]
    public async void Should_CorrectRevenue_WhenExistingCurrencyIsPassed()
    {
        decimal result = await this.revenueService.GetPredictedRevenueAsync(new RevenueDTO
        {
            ForClientId = null,
            ForSoftwareProductId = null,
            ForCurrency = FakesConsts.ExistingCurrency
        }, CancellationToken.None);

        decimal expectedContractsRevenue = 11000;
        decimal expectedSubscriptionsRevenue = 1900;
        result.ShouldBe((expectedContractsRevenue + expectedSubscriptionsRevenue) *
                        FakesConsts.ExistingCurrencyMultiplier);
    }

    [Fact]
    public async void Should_CorrectRevenue_WhenExistingClientIdAndSoftwareProductIdArePassed()
    {
        decimal result = await this.revenueService.GetPredictedRevenueAsync(new RevenueDTO
        {
            ForClientId = FakesConsts.ClientIndividual1.Id,
            ForSoftwareProductId = FakesConsts.SoftwareProduct1.Id,
            ForCurrency = null
        }, CancellationToken.None);

        decimal expectedContractsRevenue = 7000;
        decimal expectedSubscriptionsRevenue = 650;
        result.ShouldBe(expectedContractsRevenue + expectedSubscriptionsRevenue);
    }
}
