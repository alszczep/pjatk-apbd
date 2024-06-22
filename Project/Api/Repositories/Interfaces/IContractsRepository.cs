using api.Models;

namespace Api.Repositories.Interfaces;

public interface IContractsRepository
{
    public void AddContract(Contract contract);
    public Task<Contract?> GetContractWithPaymentsByIdAsync(Guid id, CancellationToken cancellationToken);
    public Task SaveChangesAsync(CancellationToken cancellationToken);
}
