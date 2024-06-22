using Api.Context;
using api.Models;
using Api.Repositories.Interfaces;

namespace Api.Repositories;

public class ContractPaymentsRepository : IContractPaymentsRepository
{
    private readonly ProjectContext projectContext;

    public ContractPaymentsRepository(ProjectContext projectContext)
    {
        this.projectContext = projectContext;
    }


    public void AddContractPayment(ContractPayment contractPayment)
    {
        this.projectContext.ContractPayments.Add(contractPayment);
    }

    public Task SaveChangesAsync(CancellationToken cancellationToken)
    {
        return this.projectContext.SaveChangesAsync(cancellationToken);
    }
}
