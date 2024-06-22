using api.Models;

namespace Api.Repositories.Interfaces;

public interface IClientsRepository
{
    Task<Client?> GetClientByIdAsync(Guid id, CancellationToken cancellationToken);
    void AddClient(Client client);
    void UpdateClient(Client client);
    Task SaveChangesAsync(CancellationToken cancellationToken);
}
