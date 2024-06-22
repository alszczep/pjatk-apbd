using api.Models;
using Api.Repositories.Interfaces;

namespace Tests.Fakes;

public class FakeContractsRepository : IContractsRepository
{
    public void AddContract(Contract contract)
    {
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

    public Task<List<Contract>> GetContractsAsync(Guid? clientId, Guid? softwareProductId,
        CancellationToken cancellationToken)
    {
        throw new NotImplementedException();
    }

    public Task SaveChangesAsync(CancellationToken cancellationToken)
    {
        return Task.CompletedTask;
    }
}
