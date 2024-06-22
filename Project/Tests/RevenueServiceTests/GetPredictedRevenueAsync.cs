using Api.Services;
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
        this.revenueService = new RevenueService(new FakeContractsRepository(), new FakeNBPService());
    }
}
