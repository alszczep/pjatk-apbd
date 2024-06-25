using Api.DTOs;
using Api.Services;
using Shouldly;
using Tests.Fakes;
using Xunit.Abstractions;

namespace Tests.ContractsServiceTests;

public class MakePaymentsAsyncTests
{
    private readonly ContractsService contractsService;
    private readonly ITestOutputHelper testOutputHelper;

    public MakePaymentsAsyncTests(ITestOutputHelper testOutputHelper)
    {
        this.testOutputHelper = testOutputHelper;
        this.contractsService = new ContractsService(
            new FakeContractPaymentsRepository(),
            new ContractsAndSubscriptionsSharedService(
                new FakeClientsRepository(),
                new FakeSoftwareProductsRepository()),
            new FakeContractsRepository());
    }

    [Fact]
    public async void Should_ThrowException_WhenContractDoesNotExist()
    {
        await Should.ThrowAsync<ArgumentException>(this.contractsService.MakePaymentAsync(new ContractPaymentDTO
        {
            ContractId = FakesConsts.NotExistingId,
            PaymentAmountInPln = 200
        }, CancellationToken.None));
    }

    [Fact]
    public async void Should_ThrowException_WhenContractIsAlreadySigned()
    {
        await Should.ThrowAsync<ArgumentException>(this.contractsService.MakePaymentAsync(new ContractPaymentDTO
        {
            ContractId = FakesConsts.ContractSignedAndActiveForSoftwareProduct1.Id,
            PaymentAmountInPln = 200
        }, CancellationToken.None));
    }

    [Fact]
    public async void Should_ThrowException_WhenPaymentAmountIsNegative()
    {
        await Should.ThrowAsync<ArgumentException>(this.contractsService.MakePaymentAsync(new ContractPaymentDTO
        {
            ContractId = FakesConsts.ContractUnsignedForSoftwareProduct1.Id,
            PaymentAmountInPln = -200
        }, CancellationToken.None));
    }

    [Fact]
    public async void Should_ThrowException_WhenPaymentAmountIsZero()
    {
        await Should.ThrowAsync<ArgumentException>(this.contractsService.MakePaymentAsync(new ContractPaymentDTO
        {
            ContractId = FakesConsts.ContractUnsignedForSoftwareProduct1.Id,
            PaymentAmountInPln = 0
        }, CancellationToken.None));
    }

    [Fact]
    public async void Should_ThrowException_WhenPaymentAmountIsGreaterThanContractPrice()
    {
        await Should.ThrowAsync<ArgumentException>(this.contractsService.MakePaymentAsync(new ContractPaymentDTO
        {
            ContractId = FakesConsts.ContractUnsignedForSoftwareProduct1.Id,
            PaymentAmountInPln = FakesConsts.ContractUnsignedForSoftwareProduct1.PriceInPlnAfterDiscounts + 1000
        }, CancellationToken.None));
    }

    [Fact]
    public async void Should_Succeed_WhenPaymentAmountIsEqualToContractPrice()
    {
        await Should.NotThrowAsync(this.contractsService.MakePaymentAsync(new ContractPaymentDTO
        {
            ContractId = FakesConsts.ContractUnsignedForSoftwareProduct1.Id,
            PaymentAmountInPln = FakesConsts.ContractUnsignedForSoftwareProduct1.PriceInPlnAfterDiscounts
        }, CancellationToken.None));
    }

    [Fact]
    public async void Should_Succeed_WhenPaymentAmountIsLessThanContractPrice()
    {
        await Should.NotThrowAsync(this.contractsService.MakePaymentAsync(new ContractPaymentDTO
        {
            ContractId = FakesConsts.ContractUnsignedForSoftwareProduct1.Id,
            PaymentAmountInPln = FakesConsts.ContractUnsignedForSoftwareProduct1.PriceInPlnAfterDiscounts - 1000
        }, CancellationToken.None));
    }

    [Fact]
    public async void
        Should_ThrowException_WhenContractAlreadyHasOtherPaymentsAndPaymentAmountIsGreaterThanContractPrice()
    {
        await Should.ThrowAsync<ArgumentException>(this.contractsService.MakePaymentAsync(new ContractPaymentDTO
        {
            ContractId = FakesConsts.ContractUnsignedForSoftwareProduct2WithPayment.Id,
            PaymentAmountInPln = FakesConsts.ContractUnsignedForSoftwareProduct2WithPayment.PriceInPlnAfterDiscounts -
                FakesConsts.ContractPayment1.PaymentAmountInPln + 100
        }, CancellationToken.None));
    }

    [Fact]
    public async void Should_Succeed_WhenContractAlreadyHasOtherPaymentsButAmountIsLessThanContractPrice()
    {
        await Should.NotThrowAsync(this.contractsService.MakePaymentAsync(new ContractPaymentDTO
        {
            ContractId = FakesConsts.ContractUnsignedForSoftwareProduct2WithPayment.Id,
            PaymentAmountInPln = 1000
        }, CancellationToken.None));
    }
}
