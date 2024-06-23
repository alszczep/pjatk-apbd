using Api.Context;
using api.Models;
using Api.Repositories.Interfaces;
using Microsoft.EntityFrameworkCore;

namespace Api.Repositories;

public class ContractsRepository : IContractsRepository
{
    private readonly ProjectContext projectContext;

    public ContractsRepository(ProjectContext projectContext)
    {
        this.projectContext = projectContext;
    }

    public void AddContract(Contract contract)
    {
        this.projectContext.Contracts.Add(contract);
    }

    public Task<Contract?> GetContractWithPaymentsByIdAsync(Guid id, CancellationToken cancellationToken)
    {
        return this.projectContext.Contracts
            .Include(c => c.Payments)
            .FirstOrDefaultAsync(c => c.Id == id, cancellationToken);
    }

    public Task<List<Contract>> GetContractsAsync(CancellationToken cancellationToken)
    {
        return this.projectContext.Contracts
            .Include(c => c.Client)
            .Include(c => c.SoftwareProduct)
            .ToListAsync(cancellationToken);
    }

    public Task SaveChangesAsync(CancellationToken cancellationToken)
    {
        return this.projectContext.SaveChangesAsync(cancellationToken);
    }
}
