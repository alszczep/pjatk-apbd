using api.Models;

namespace Api.Repositories.Interfaces;

public interface IClientsRepository
{
    Task<Client?> GetClientByIdAsync(Guid id, CancellationToken cancellationToken);

    Task<Client?> GetClientWithContractsAndSubscriptionsWithPaymentsAndSoftwareProductsByIdAsync(Guid id,
        CancellationToken cancellationToken);

    Task<ClientIndividual?> GetClientByPeselAsync(string pesel, CancellationToken cancellationToken);
    Task<ClientCompany?> GetClientByKrsAsync(string krs, CancellationToken cancellationToken);
    void AddClient(Client client);
    void UpdateClient(Client client);
    Task SaveChangesAsync(CancellationToken cancellationToken);
}
