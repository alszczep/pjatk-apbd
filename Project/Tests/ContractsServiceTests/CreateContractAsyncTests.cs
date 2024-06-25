using Api.DTOs;
using api.Models;
using Api.Services;
using Shouldly;
using Tests.Fakes;
using Xunit.Abstractions;

namespace Tests.ContractsServiceTests;

public class CreateContractAsyncTests
{
    private readonly ContractsService contractsService;
    private readonly FakeContractsRepository fakeContractsRepository;
    private readonly ITestOutputHelper testOutputHelper;

    public CreateContractAsyncTests(ITestOutputHelper testOutputHelper)
    {
        this.testOutputHelper = testOutputHelper;
        this.fakeContractsRepository = new FakeContractsRepository();
        this.contractsService = new ContractsService(
            new FakeContractPaymentsRepository(),
            new ContractsAndSubscriptionsSharedService(
                new FakeClientsRepository(),
                new FakeSoftwareProductsRepository()),
            this.fakeContractsRepository);
    }

    [Fact]
    public async void Should_ThrowException_WhenStartDateIsAfterEndDate()
    {
        await Should.ThrowAsync<ArgumentException>(this.contractsService.CreateContractAsync(new CreateContractDTO
        {
            ClientId = FakesConsts.ClientCompany1.Id,
            SoftwareProductId = FakesConsts.SoftwareProduct2.Id,
            YearsOfExtendedSupport = 1,
            StartDate = DateTime.Now.AddDays(20),
            EndDate = DateTime.Now
        }, CancellationToken.None));
    }

    [Fact]
    public async void Should_ThrowException_WhenDatesSpanIsShorterThan3Days()
    {
        await Should.ThrowAsync<ArgumentException>(this.contractsService.CreateContractAsync(new CreateContractDTO
        {
            ClientId = FakesConsts.ClientCompany1.Id,
            SoftwareProductId = FakesConsts.SoftwareProduct2.Id,
            YearsOfExtendedSupport = 1,
            StartDate = DateTime.Now,
            EndDate = DateTime.Now.AddDays(2)
        }, CancellationToken.None));
    }

    [Fact]
    public async void Should_ThrowException_WhenDatesSpanIsLongerThan30Days()
    {
        await Should.ThrowAsync<ArgumentException>(this.contractsService.CreateContractAsync(new CreateContractDTO
        {
            ClientId = FakesConsts.ClientCompany1.Id,
            SoftwareProductId = FakesConsts.SoftwareProduct2.Id,
            YearsOfExtendedSupport = 1,
            StartDate = DateTime.Now,
            EndDate = DateTime.Now.AddDays(31)
        }, CancellationToken.None));
    }

    [Fact]
    public async void Should_ThrowException_WhenYearsOfExtendedSupportIsNegative()
    {
        await Should.ThrowAsync<ArgumentException>(this.contractsService.CreateContractAsync(new CreateContractDTO
        {
            ClientId = FakesConsts.ClientCompany1.Id,
            SoftwareProductId = FakesConsts.SoftwareProduct2.Id,
            YearsOfExtendedSupport = -1,
            StartDate = DateTime.Now,
            EndDate = DateTime.Now.AddDays(20)
        }, CancellationToken.None));
    }

    [Fact]
    public async void Should_ThrowException_WhenYearsOfExtendedSupportIsMoreThan3()
    {
        await Should.ThrowAsync<ArgumentException>(this.contractsService.CreateContractAsync(new CreateContractDTO
        {
            ClientId = FakesConsts.ClientCompany1.Id,
            SoftwareProductId = FakesConsts.SoftwareProduct2.Id,
            YearsOfExtendedSupport = 4,
            StartDate = DateTime.Now,
            EndDate = DateTime.Now.AddDays(20)
        }, CancellationToken.None));
    }

    [Fact]
    public async void Should_ThrowException_WhenSoftwareProductDoesNotExist()
    {
        await Should.ThrowAsync<ArgumentException>(this.contractsService.CreateContractAsync(new CreateContractDTO
        {
            ClientId = FakesConsts.ClientCompany1.Id,
            SoftwareProductId = FakesConsts.NotExistingId,
            YearsOfExtendedSupport = 1,
            StartDate = DateTime.Now,
            EndDate = DateTime.Now.AddDays(20)
        }, CancellationToken.None));
    }

    [Fact]
    public async void Should_ThrowException_WhenClientDoesNotExist()
    {
        await Should.ThrowAsync<ArgumentException>(this.contractsService.CreateContractAsync(new CreateContractDTO
        {
            ClientId = FakesConsts.NotExistingId,
            SoftwareProductId = FakesConsts.SoftwareProduct2.Id,
            YearsOfExtendedSupport = 1,
            StartDate = DateTime.Now,
            EndDate = DateTime.Now.AddDays(20)
        }, CancellationToken.None));
    }

    [Fact]
    public async void Should_ThrowException_WhenClientHasUnsignedContractForTheSameSoftwareProduct()
    {
        await Should.ThrowAsync<ArgumentException>(this.contractsService.CreateContractAsync(new CreateContractDTO
        {
            ClientId = FakesConsts.ClientIndividual1.Id,
            SoftwareProductId = FakesConsts.SoftwareProduct1.Id,
            YearsOfExtendedSupport = 1,
            StartDate = DateTime.Now,
            EndDate = DateTime.Now.AddDays(20)
        }, CancellationToken.None));
    }

    [Fact]
    public async void Should_ThrowException_WhenClientHasSignedAndActiveContractForTheSameSoftwareProduct()
    {
        await Should.ThrowAsync<ArgumentException>(this.contractsService.CreateContractAsync(new CreateContractDTO
        {
            ClientId = FakesConsts.ClientIndividual2.Id,
            SoftwareProductId = FakesConsts.SoftwareProduct1.Id,
            YearsOfExtendedSupport = 1,
            StartDate = DateTime.Now,
            EndDate = DateTime.Now.AddDays(20)
        }, CancellationToken.None));
    }

    [Fact]
    public async void Should_ThrowException_WhenClientHasActiveSubscriptionForTheSameSoftwareProduct()
    {
        await Should.ThrowAsync<ArgumentException>(this.contractsService.CreateContractAsync(new CreateContractDTO
        {
            ClientId = FakesConsts.ClientIndividual3.Id,
            SoftwareProductId = FakesConsts.SoftwareProduct1.Id,
            YearsOfExtendedSupport = 1,
            StartDate = DateTime.Now,
            EndDate = DateTime.Now.AddDays(20)
        }, CancellationToken.None));
    }

    [Fact]
    public async void Should_SetCorrectPriceWithClientDiscount_WhenDataIsValid()
    {
        await Should.NotThrowAsync(this.contractsService.CreateContractAsync(new CreateContractDTO
        {
            ClientId = FakesConsts.ClientCompany1.Id,
            SoftwareProductId = FakesConsts.SoftwareProduct1.Id,
            YearsOfExtendedSupport = 2,
            StartDate = DateTime.Now,
            EndDate = DateTime.Now.AddDays(20)
        }, CancellationToken.None));

        this.fakeContractsRepository.addedThroughTests.Count.ShouldBe(1);
        Contract addedContract = this.fakeContractsRepository.addedThroughTests[0];
        addedContract.PriceInPlnAfterDiscounts.ShouldBe(13300);
    }

    [Fact]
    public async void Should_SetCorrectPriceWithProductDiscount_WhenDataIsValid()
    {
        await Should.NotThrowAsync(this.contractsService.CreateContractAsync(new CreateContractDTO
        {
            ClientId = FakesConsts.ClientIndividual4.Id,
            SoftwareProductId = FakesConsts.SoftwareProduct2.Id,
            YearsOfExtendedSupport = 0,
            StartDate = DateTime.Now,
            EndDate = DateTime.Now.AddDays(20)
        }, CancellationToken.None));

        this.fakeContractsRepository.addedThroughTests.Count.ShouldBe(1);
        Contract addedContract = this.fakeContractsRepository.addedThroughTests[0];
        addedContract.PriceInPlnAfterDiscounts.ShouldBe(21600);
    }
}
