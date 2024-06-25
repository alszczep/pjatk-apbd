using Api.Context;
using api.Models;
using Api.Repositories.Interfaces;
using Microsoft.EntityFrameworkCore;

namespace Api.Repositories;

public class ClientsRepository : IClientsRepository
{
    private readonly ProjectContext projectContext;

    public ClientsRepository(ProjectContext projectContext)
    {
        this.projectContext = projectContext;
    }

    public Task<Client?> GetClientByIdAsync(Guid id, CancellationToken cancellationToken)
    {
        return this.projectContext.Clients.FirstOrDefaultAsync(
            c => c.Id == id && (c.Type == ClientType.Company || !((ClientIndividual)c).IsDeleted),
            cancellationToken);
    }

    public Task<Client?> GetClientWithContractsAndSubscriptionsWithPaymentsAndSoftwareProductsByIdAsync(Guid id,
        CancellationToken cancellationToken)
    {
        return this.projectContext.Clients
            .Include(c => c.Contracts)
            .ThenInclude(c => c.SoftwareProduct)
            .Include(c => c.Subscriptions)
            .ThenInclude(s => s.SoftwareProduct)
            .Include(c => c.Subscriptions)
            .ThenInclude(s => s.Payments)
            .FirstOrDefaultAsync(
                c => c.Id == id && (c.Type == ClientType.Company || !((ClientIndividual)c).IsDeleted),
                cancellationToken);
    }

    public Task<ClientIndividual?> GetClientByPeselAsync(string pesel, CancellationToken cancellationToken)
    {
        return this.projectContext.Clients.OfType<ClientIndividual>()
            .FirstOrDefaultAsync(c => c.Pesel == pesel && !c.IsDeleted, cancellationToken);
    }

    public Task<ClientCompany?> GetClientByKrsAsync(string krs, CancellationToken cancellationToken)
    {
        return this.projectContext.Clients.OfType<ClientCompany>()
            .FirstOrDefaultAsync(c => c.Krs == krs, cancellationToken);
    }

    public void AddClient(Client client)
    {
        this.projectContext.Clients.Add(client);
    }

    public void UpdateClient(Client client)
    {
        this.projectContext.Clients.Update(client);
    }

    public Task SaveChangesAsync(CancellationToken cancellationToken)
    {
        return this.projectContext.SaveChangesAsync(cancellationToken);
    }
}
