using api.Models;

namespace Api.Repositories.Interfaces;

public interface IContractPaymentsRepository
{
    public void AddContractPayment(ContractPayment contractPayment);
    public Task SaveChangesAsync(CancellationToken cancellationToken);
}
