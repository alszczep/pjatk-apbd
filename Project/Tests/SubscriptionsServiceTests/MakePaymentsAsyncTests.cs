using Api.Services;
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
}
