using api.Models;
using Api.Repositories.Interfaces;

namespace Tests.Fakes;

public class FakeContractPaymentsRepository : IContractPaymentsRepository
{
    public void AddContractPayment(ContractPayment contractPayment)
    {
    }

    public Task SaveChangesAsync(CancellationToken cancellationToken)
    {
        return Task.CompletedTask;
    }
}
