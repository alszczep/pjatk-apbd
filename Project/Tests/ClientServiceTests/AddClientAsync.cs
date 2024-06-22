using Api.DTOs;
using Api.Services;
using Shouldly;
using Tests.Fakes;
using Xunit.Abstractions;

namespace Tests.ClientServiceTests;

public class AddClientAsync
{
    private readonly ClientsService clientsService;
    private readonly ITestOutputHelper testOutputHelper;

    public AddClientAsync(ITestOutputHelper testOutputHelper)
    {
        this.testOutputHelper = testOutputHelper;
        this.clientsService = new ClientsService(new FakeClientsRepository());
    }

    [Fact]
    public async void Should_ThrowException_WhenIndividualClientHasNoPesel()
    {
        await Should.ThrowAsync<ArgumentException>(this.clientsService.AddClientAsync(new AddClientDTO
        {
            FirstName = "1",
            LastName = "1",
            Type = ClientTypeDTO.Individual,
            Address = "1",
            Email = "1",
            PhoneNumber = "1"
        }, CancellationToken.None));
    }

    [Fact]
    public async void Should_ThrowException_WhenIndividualClientHasNoFirstName()
    {
        await Should.ThrowAsync<ArgumentException>(this.clientsService.AddClientAsync(new AddClientDTO
        {
            Pesel = "1",
            LastName = "1",
            Type = ClientTypeDTO.Individual,
            Address = "1",
            Email = "1",
            PhoneNumber = "1"
        }, CancellationToken.None));
    }

    [Fact]
    public async void Should_ThrowException_WhenIndividualClientHasNoLastName()
    {
        await Should.ThrowAsync<ArgumentException>(this.clientsService.AddClientAsync(new AddClientDTO
        {
            Pesel = "1",
            FirstName = "1",
            Type = ClientTypeDTO.Individual,
            Address = "1",
            Email = "1",
            PhoneNumber = "1"
        }, CancellationToken.None));
    }

    [Fact]
    public async void Should_ThrowException_WhenIndividualClientsPeselIsTaken()
    {
        await Should.ThrowAsync<ArgumentException>(this.clientsService.AddClientAsync(new AddClientDTO
        {
            Pesel = FakesConsts.ClientIndividual1.Pesel,
            FirstName = "1",
            LastName = "1",
            Type = ClientTypeDTO.Individual,
            Address = "1",
            Email = "1",
            PhoneNumber = "1"
        }, CancellationToken.None));
    }

    [Fact]
    public async void Should_ThrowException_WhenCompanyClientHasNoKrs()
    {
        await Should.ThrowAsync<ArgumentException>(this.clientsService.AddClientAsync(new AddClientDTO
        {
            CompanyName = "1",
            Type = ClientTypeDTO.Company,
            Address = "1",
            Email = "1",
            PhoneNumber = "1"
        }, CancellationToken.None));
    }

    [Fact]
    public async void Should_ThrowException_WhenCompanyClientHasNoCompanyName()
    {
        await Should.ThrowAsync<ArgumentException>(this.clientsService.AddClientAsync(new AddClientDTO
        {
            Krs = "1",
            Type = ClientTypeDTO.Company,
            Address = "1",
            Email = "1",
            PhoneNumber = "1"
        }, CancellationToken.None));
    }

    [Fact]
    public async void Should_ThrowException_WhenCompanyClientsKrsIsTaken()
    {
        await Should.ThrowAsync<ArgumentException>(this.clientsService.AddClientAsync(new AddClientDTO
        {
            Krs = FakesConsts.ClientCompany1.Krs,
            CompanyName = "1",
            Type = ClientTypeDTO.Company,
            Address = "1",
            Email = "1",
            PhoneNumber = "1"
        }, CancellationToken.None));
    }

    [Fact]
    public async void Should_Succeed_WhenIndividualClientIsValid()
    {
        await Should.NotThrowAsync(this.clientsService.AddClientAsync(new AddClientDTO
        {
            Pesel = "1",
            FirstName = "1",
            LastName = "1",
            Type = ClientTypeDTO.Individual,
            Address = "1",
            Email = "1",
            PhoneNumber = "1"
        }, CancellationToken.None));
    }

    [Fact]
    public async void Should_Succeed_WhenCompanyClientIsValid()
    {
        await Should.NotThrowAsync(this.clientsService.AddClientAsync(new AddClientDTO
        {
            Krs = "1",
            CompanyName = "1",
            Type = ClientTypeDTO.Company,
            Address = "1",
            Email = "1",
            PhoneNumber = "1"
        }, CancellationToken.None));
    }
}
