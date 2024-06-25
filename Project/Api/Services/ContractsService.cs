using Api.DTOs;
using api.Models;
using Api.Repositories.Interfaces;
using Api.Services.Interfaces;

namespace Api.Services;

public class ContractsService : IContractsService
{
    private readonly IContractPaymentsRepository contractPaymentsRepository;
    private readonly IContractsAndSubscriptionsSharedService contractsAndSubscriptionsSharedService;
    private readonly IContractsRepository contractsRepository;

    public ContractsService(IContractPaymentsRepository contractPaymentsRepository,
        IContractsAndSubscriptionsSharedService contractsAndSubscriptionsSharedService,
        IContractsRepository contractsRepository)
    {
        this.contractPaymentsRepository = contractPaymentsRepository;
        this.contractsAndSubscriptionsSharedService = contractsAndSubscriptionsSharedService;
        this.contractsRepository = contractsRepository;
    }

    public async Task CreateContractAsync(CreateContractDTO dto, CancellationToken cancellationToken)
    {
        EnsureContractDatesAreValid(dto.StartDate, dto.EndDate);
        EnsureYearsOfExtendedSupportAreValid(dto.YearsOfExtendedSupport);

        SoftwareProduct softwareProduct =
            await this.contractsAndSubscriptionsSharedService.GetSoftwareProductWithDiscountsByIdAsync(
                dto.SoftwareProductId,
                cancellationToken);
        Client client =
            await this.contractsAndSubscriptionsSharedService
                .GetClientWithContractsAndSubscriptionsWithPaymentsAndSoftwareProductsByIdAsync(dto.ClientId,
                    cancellationToken);

        this.contractsAndSubscriptionsSharedService.EnsureThereIsNoActiveContractWithTheSameSoftwareProductAndClient(
            client, softwareProduct);
        this.contractsAndSubscriptionsSharedService
            .EnsureThereIsNoActiveSubscriptionWithTheSameSoftwareProductAndClient(
                client, softwareProduct);

        Contract contract = new()
        {
            Id = Guid.NewGuid(),
            SoftwareProduct = softwareProduct,
            Client = client,
            StartDate = dto.StartDate,
            EndDate = dto.EndDate,
            PriceInPlnAfterDiscounts = this.CalculateContractPrice(dto.YearsOfExtendedSupport, softwareProduct, client),
            YearsOfExtendedSupport = dto.YearsOfExtendedSupport,
            IsSigned = false
        };

        this.contractsRepository.AddContract(contract);
        await this.contractsRepository.SaveChangesAsync(cancellationToken);
    }

    public async Task MakePaymentAsync(ContractPaymentDTO dto, CancellationToken cancellationToken)
    {
        Contract contract = await this.GetContractWithPaymentsByIdAsync(dto.ContractId, cancellationToken);

        EnsureContractNotSigned(contract);
        EnsurePaymentAmountIsPositive(dto.PaymentAmountInPln);

        decimal remainingAmountToPay = GetRemainingAmountToPayForContract(contract);
        EnsurePaymentAmountIsLessThanRemainingAmount(dto.PaymentAmountInPln, remainingAmountToPay);

        ContractPayment payment = new()
        {
            Id = Guid.NewGuid(),
            Contract = contract,
            PaymentAmountInPln = dto.PaymentAmountInPln
        };

        if (remainingAmountToPay - dto.PaymentAmountInPln == 0)
            contract.IsSigned = true;

        this.contractPaymentsRepository.AddContractPayment(payment);
        await this.contractPaymentsRepository.SaveChangesAsync(cancellationToken);
    }

    private static void EnsureContractDatesAreValid(DateTime startDate, DateTime endDate)
    {
        if (startDate > endDate)
            throw new ArgumentException("Start date cannot be later than end date");
        if (endDate - startDate < TimeSpan.FromDays(3))
            throw new ArgumentException("Time range should be at least 3 days");
        if (endDate - startDate > TimeSpan.FromDays(30))
            throw new ArgumentException("Time range should be at most 30 days");
    }

    private static void EnsureYearsOfExtendedSupportAreValid(int yearsOfExtendedSupport)
    {
        if (yearsOfExtendedSupport < 0 || yearsOfExtendedSupport > 3)
            throw new ArgumentException("Years of extended support should be between 0 and 3");
    }

    private decimal CalculateContractPrice(int yearsOfExtendedSupport, SoftwareProduct softwareProduct,
        Client client)
    {
        decimal productDiscountMultiplier =
            this.contractsAndSubscriptionsSharedService.CalculateDiscountMultiplierForProduct(softwareProduct);
        decimal clientDiscountMultiplier =
            this.contractsAndSubscriptionsSharedService.CalculateDiscountMultiplierForClient(client);

        return (softwareProduct.UpfrontYearlyPriceInPln + Contract.SupportYearPriceInPln * yearsOfExtendedSupport) *
               productDiscountMultiplier *
               clientDiscountMultiplier;
    }

    private async Task<Contract> GetContractWithPaymentsByIdAsync(Guid id, CancellationToken cancellationToken)
    {
        Contract? contract = await this.contractsRepository.GetContractWithPaymentsByIdAsync(id, cancellationToken);
        if (contract == null)
            throw new ArgumentException("Contract not found");
        return contract;
    }

    private static void EnsureContractNotSigned(Contract contract)
    {
        if (contract.IsSigned)
            throw new ArgumentException("Contract is already signed");
    }

    private static void EnsurePaymentAmountIsPositive(decimal paymentAmountInPln)
    {
        if (paymentAmountInPln <= 0)
            throw new ArgumentException("Payment amount should be greater than 0");
    }

    private static decimal GetRemainingAmountToPayForContract(Contract contract)
    {
        return contract.PriceInPlnAfterDiscounts - contract.Payments.Sum(p => p.PaymentAmountInPln);
    }

    private static void EnsurePaymentAmountIsLessThanRemainingAmount(decimal paymentAmountInPln,
        decimal remainingAmount)
    {
        if (paymentAmountInPln > remainingAmount)
            throw new ArgumentException("Payment amount should be less than remaining amount");
    }
}
