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
        return this.projectContext.Clients.FirstOrDefaultAsync(c => c.Id == id && (
            c.Type == ClientType.Company || !((ClientIndividual)c).IsDeleted
        ), cancellationToken);
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
