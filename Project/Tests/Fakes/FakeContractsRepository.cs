using api.Models;
using Api.Repositories.Interfaces;

namespace Tests.Fakes;

public class FakeContractsRepository : IContractsRepository
{
    public List<Contract> addedThroughTests = new();

    public void AddContract(Contract contract)
    {
        this.addedThroughTests.Add(contract);
    }

    public Task<Contract?> GetContractWithPaymentsByIdAsync(Guid id, CancellationToken cancellationToken)
    {
        if (id == FakesConsts.ContractUnsignedForSoftwareProduct1.Id)
            return Task.FromResult<Contract?>(FakesConsts.ContractUnsignedForSoftwareProduct1);
        if (id == FakesConsts.ContractUnsignedForSoftwareProduct2WithPayment.Id)
            return Task.FromResult<Contract?>(FakesConsts.ContractUnsignedForSoftwareProduct2WithPayment);
        if (id == FakesConsts.ContractSignedAndActiveForSoftwareProduct1.Id)
            return Task.FromResult<Contract?>(FakesConsts.ContractSignedAndActiveForSoftwareProduct1);
        if (id == FakesConsts.ContractSignedAndInactiveForSoftwareProduct1.Id)
            return Task.FromResult<Contract?>(FakesConsts.ContractSignedAndInactiveForSoftwareProduct1);
        return Task.FromResult<Contract?>(null);
    }

    public Task SaveChangesAsync(CancellationToken cancellationToken)
    {
        return Task.CompletedTask;
    }

    public Task<List<Contract>> GetContractsAsync(CancellationToken cancellationToken)
    {
        return Task.FromResult(new List<Contract>
        {
            new()
            {
                PriceInPlnAfterDiscounts = 2000,
                SoftwareProduct = FakesConsts.SoftwareProduct1,
                Client = FakesConsts.ClientCompany1,
                Id = Guid.Parse("00000000-0000-0000-0000-000000000001"),
                StartDate = DateTime.Now.Subtract(TimeSpan.FromDays(10)),
                EndDate = DateTime.Now.Add(TimeSpan.FromDays(10)),
                IsSigned = true,
                SignedDate = DateTime.Now.Subtract(TimeSpan.FromDays(5)),
                YearsOfExtendedSupport = 1
            },
            new()
            {
                PriceInPlnAfterDiscounts = 3000,
                SoftwareProduct = FakesConsts.SoftwareProduct1,
                Client = FakesConsts.ClientIndividual1,
                Id = Guid.Parse("00000000-0000-0000-0000-000000000001"),
                StartDate = DateTime.Now.Subtract(TimeSpan.FromDays(10)),
                EndDate = DateTime.Now.Add(TimeSpan.FromDays(10)),
                IsSigned = true,
                SignedDate = DateTime.Now.Subtract(TimeSpan.FromDays(5)),
                YearsOfExtendedSupport = 1
            },
            new()
            {
                PriceInPlnAfterDiscounts = 4000,
                SoftwareProduct = FakesConsts.SoftwareProduct1,
                Client = FakesConsts.ClientIndividual1,
                Id = Guid.Parse("00000000-0000-0000-0000-000000000001"),
                StartDate = DateTime.Now.Subtract(TimeSpan.FromDays(10)),
                EndDate = DateTime.Now.Add(TimeSpan.FromDays(10)),
                IsSigned = false,
                YearsOfExtendedSupport = 1
            },
            new()
            {
                PriceInPlnAfterDiscounts = 2000,
                SoftwareProduct = FakesConsts.SoftwareProduct2,
                Client = FakesConsts.ClientCompany1,
                Id = Guid.Parse("00000000-0000-0000-0000-000000000001"),
                StartDate = DateTime.Now.Subtract(TimeSpan.FromDays(10)),
                EndDate = DateTime.Now.Add(TimeSpan.FromDays(10)),
                IsSigned = true,
                SignedDate = DateTime.Now.Subtract(TimeSpan.FromDays(5)),
                YearsOfExtendedSupport = 1
            }
        });
    }
}
