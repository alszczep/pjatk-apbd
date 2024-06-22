using Api.Services;
using Shouldly;
using Tests.Fakes;
using Xunit.Abstractions;

namespace Tests.ClientServiceTests;

public class DeleteClientAsync
{
    private readonly ClientsService clientsService;
    private readonly ITestOutputHelper testOutputHelper;

    public DeleteClientAsync(ITestOutputHelper testOutputHelper)
    {
        this.testOutputHelper = testOutputHelper;
        this.clientsService = new ClientsService(new FakeClientsRepository());
    }

    [Fact]
    public async void Should_ThrowException_WhenClientDoesNotExist()
    {
        await Should.ThrowAsync<ArgumentException>(
            this.clientsService.DeleteClientAsync(FakesConsts.NotExistingId, CancellationToken.None));
    }

    [Fact]
    public async void Should_ThrowException_WhenClientIsCompanyClient()
    {
        await Should.ThrowAsync<InvalidOperationException>(
            this.clientsService.DeleteClientAsync(FakesConsts.ClientCompany1.Id, CancellationToken.None));
    }

    [Fact]
    public async void Should_Succeed_WhenClientExists()
    {
        await Should.NotThrowAsync(
            this.clientsService.DeleteClientAsync(FakesConsts.ClientIndividual1.Id, CancellationToken.None));
    }
}
