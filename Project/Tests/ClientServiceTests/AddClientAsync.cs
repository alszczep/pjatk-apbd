using Api.Services;
using Shouldly;
using Tests.Fakes;
using Xunit.Abstractions;

namespace Tests;

public class ClientsServiceTests
{
    private readonly ClientsService clientsService;
    private readonly ITestOutputHelper testOutputHelper;

    public ClientsServiceTests(ITestOutputHelper testOutputHelper)
    {
        this.testOutputHelper = testOutputHelper;
        this.clientsService = new ClientsService(new FakeClientsRepository());
    }

    private Task TODO()
    {
        throw new InvalidOperationException("TODO");
    }

    [Fact]
    public async void Should_ThrowException_WhenDateIsMoreThatDateDue()
    {
        await Should.ThrowAsync<InvalidOperationException>(this.TODO());
    }
}
