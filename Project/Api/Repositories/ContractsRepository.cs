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

    public Task<List<Contract>> GetContractsAsync(Guid? clientId, Guid? softwareProductId,
        CancellationToken cancellationToken)
    {
        IQueryable<Contract> contracts = this.projectContext.Contracts
            .Include(c => c.Client)
            .Include(c => c.SoftwareProduct);

        if (clientId != null) contracts = contracts.Where(c => c.Client.Id == clientId);

        if (softwareProductId != null) contracts = contracts.Where(c => c.SoftwareProduct.Id == softwareProductId);

        return contracts.ToListAsync(cancellationToken);
    }

    public Task SaveChangesAsync(CancellationToken cancellationToken)
    {
        return this.projectContext.SaveChangesAsync(cancellationToken);
    }
}
