using Api.DTOs;
using Api.Services;
using Shouldly;
using Tests.Fakes;
using Xunit.Abstractions;

namespace Tests.ClientServiceTests;

public class UpdateClientAsync
{
    private readonly ClientsService clientsService;
    private readonly ITestOutputHelper testOutputHelper;

    public UpdateClientAsync(ITestOutputHelper testOutputHelper)
    {
        this.testOutputHelper = testOutputHelper;
        this.clientsService = new ClientsService(new FakeClientsRepository());
    }

    [Fact]
    public async void Should_ThrowException_WhenIndividualClientDoesNotExist()
    {
        await Should.ThrowAsync<ArgumentException>(this.clientsService.UpdateClientAsync(new UpdateClientDTO
        {
            Id = FakesConsts.NotExistingId,
            FirstName = "1",
            LastName = "1",
            Address = "1",
            Email = "1",
            PhoneNumber = "1"
        }, CancellationToken.None));
    }

    [Fact]
    public async void Should_ThrowException_WhenIndividualClientHasNoFirstName()
    {
        await Should.ThrowAsync<ArgumentException>(this.clientsService.UpdateClientAsync(new UpdateClientDTO
        {
            Id = FakesConsts.ClientIndividual1.Id,
            LastName = "1",
            Address = "1",
            Email = "1",
            PhoneNumber = "1"
        }, CancellationToken.None));
    }

    [Fact]
    public async void Should_ThrowException_WhenIndividualClientHasNoLastName()
    {
        await Should.ThrowAsync<ArgumentException>(this.clientsService.UpdateClientAsync(new UpdateClientDTO
        {
            Id = FakesConsts.ClientIndividual1.Id,
            FirstName = "1",
            Address = "1",
            Email = "1",
            PhoneNumber = "1"
        }, CancellationToken.None));
    }

    [Fact]
    public async void Should_ThrowException_WhenCompanyClientDoesNotExist()
    {
        await Should.ThrowAsync<ArgumentException>(this.clientsService.UpdateClientAsync(new UpdateClientDTO
        {
            Id = FakesConsts.NotExistingId,
            CompanyName = "1",
            Address = "1",
            Email = "1",
            PhoneNumber = "1"
        }, CancellationToken.None));
    }

    [Fact]
    public async void Should_ThrowException_WhenCompanyClientHasNoCompanyName()
    {
        await Should.ThrowAsync<ArgumentException>(this.clientsService.UpdateClientAsync(new UpdateClientDTO
        {
            Id = FakesConsts.ClientCompany1.Id,
            Address = "1",
            Email = "1",
            PhoneNumber = "1"
        }, CancellationToken.None));
    }

    [Fact]
    public async void Should_Succeed_WhenIndividualClientIsValid()
    {
        await Should.NotThrowAsync(this.clientsService.UpdateClientAsync(new UpdateClientDTO
        {
            Id = FakesConsts.ClientIndividual1.Id,
            FirstName = "1",
            LastName = "1",
            Address = "1",
            Email = "1",
            PhoneNumber = "1"
        }, CancellationToken.None));
    }

    [Fact]
    public async void Should_Succeed_WhenCompanyClientIsValid()
    {
        await Should.NotThrowAsync(this.clientsService.UpdateClientAsync(new UpdateClientDTO
        {
            Id = FakesConsts.ClientCompany1.Id,
            CompanyName = "1",
            Address = "1",
            Email = "1",
            PhoneNumber = "1"
        }, CancellationToken.None));
    }
}
