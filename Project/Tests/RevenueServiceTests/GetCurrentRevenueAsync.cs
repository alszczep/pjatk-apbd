using Api.Services;
using Tests.Fakes;
using Xunit.Abstractions;

namespace Tests.RevenueServiceTests;

public class GetCurrentRevenueAsync
{
    private readonly RevenueService revenueService;
    private readonly ITestOutputHelper testOutputHelper;

    public GetCurrentRevenueAsync(ITestOutputHelper testOutputHelper)
    {
        this.testOutputHelper = testOutputHelper;
        this.revenueService = new RevenueService(new FakeContractsRepository(), new FakeNBPService());
    }
}
