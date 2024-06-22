using Api.DTOs;
using Api.Helpers;
using api.Models;
using Api.Repositories.Interfaces;
using Api.Services.Interfaces;

namespace Api.Services;

public class ContractsService : IContractsService
{
    private readonly IClientsRepository clientsRepository;
    private readonly IContractPaymentsRepository contractPaymentsRepository;
    private readonly IContractsRepository contractsRepository;
    private readonly ISoftwareProductsRepository softwareProductsRepository;

    public ContractsService(IClientsRepository clientsRepository,
        IContractPaymentsRepository contractPaymentsRepository,
        IContractsRepository contractsRepository,
        ISoftwareProductsRepository softwareProductsRepository)
    {
        this.clientsRepository = clientsRepository;
        this.contractPaymentsRepository = contractPaymentsRepository;
        this.contractsRepository = contractsRepository;
        this.softwareProductsRepository = softwareProductsRepository;
    }

    public async Task CreateContractAsync(CreateContractDTO dto, CancellationToken cancellationToken)
    {
        EnsureContractDatesAreValid(dto.StartDate, dto.EndDate);
        EnsureYearsOfExtendedSupportAreValid(dto.YearsOfExtendedSupport);
        SoftwareProduct softwareProduct = await this.GetSoftwareProductWithDiscountsByIdAsync(dto.SoftwareProductId,
            cancellationToken);
        Client client = await this.GetClientWithContractsAndSoftwareProductsByIdAsync(dto.ClientId, cancellationToken);
        EnsureThereIsNoActiveContractWithTheSameSoftwareProductAndClient(client, softwareProduct);

        Contract contract = new()
        {
            Id = Guid.NewGuid(),
            SoftwareProduct = softwareProduct,
            Client = client,
            StartDate = dto.StartDate,
            EndDate = dto.EndDate,
            PriceInPlnAfterDiscounts = CalculateContractPrice(dto.YearsOfExtendedSupport, softwareProduct, client),
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

    private async Task<SoftwareProduct> GetSoftwareProductWithDiscountsByIdAsync(Guid softwareProductId,
        CancellationToken cancellationToken)
    {
        SoftwareProduct? softwareProduct =
            await this.softwareProductsRepository.GetSoftwareProductWithDiscountsByIdAsync(softwareProductId,
                cancellationToken);
        if (softwareProduct == null)
            throw new ArgumentException("Software product not found");
        return softwareProduct;
    }

    private async Task<Client> GetClientWithContractsAndSoftwareProductsByIdAsync(Guid clientId,
        CancellationToken cancellationToken)
    {
        Client? client =
            await this.clientsRepository.GetClientWithContractsAndSoftwareProductsByIdAsync(clientId,
                cancellationToken);
        if (client == null)
            throw new ArgumentException("Client not found");
        return client;
    }

    private static bool IsContractNotSignedOrInEffect(Contract contract)
    {
        if (!contract.IsSigned) return true;

        if (!contract.SignedDate.HasValue)
            throw new AggregateException("Contract is signed but has no signed date");

        return contract.SignedDate.Value.AddYears(1 + contract.YearsOfExtendedSupport) > DateTime.Now;
    }

    private static void EnsureThereIsNoActiveContractWithTheSameSoftwareProductAndClient(Client client,
        SoftwareProduct softwareProduct)
    {
        if (client.Contracts.Any(c => c.SoftwareProduct.Id == softwareProduct.Id && IsContractNotSignedOrInEffect(c)))
            throw new ArgumentException("Client already has a contract with this software product");
    }

    private static decimal CalculateDiscountMultiplierForProduct(SoftwareProduct softwareProduct)
    {
        var activeDiscounts = softwareProduct.Discounts
            .Where(d => d.StartDate <= DateTime.Now && d.EndDate >= DateTime.Now).ToList();
        return MathHelpers.PercentageToMultiplier(activeDiscounts.Count == 0
            ? 0
            : activeDiscounts.Max(d => d.DiscountPercentage));
    }

    private static decimal CalculateDiscountMultiplierForClient(Client client)
    {
        return client.Contracts.Any(c => c.IsSigned) ? 0.95m : 1m;
    }

    private static decimal CalculateContractPrice(int yearsOfExtendedSupport, SoftwareProduct softwareProduct,
        Client client)
    {
        decimal productDiscountMultiplier = CalculateDiscountMultiplierForProduct(softwareProduct);
        decimal clientDiscountMultiplier = CalculateDiscountMultiplierForClient(client);

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
