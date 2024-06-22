using Api.DTOs;

namespace Api.Services.Interfaces;

public interface IClientsService
{
    Task AddClientAsync(AddClientDTO dto, CancellationToken cancellationToken);
    Task UpdateClientAsync(UpdateClientDTO dto, CancellationToken cancellationToken);
    Task DeleteClientAsync(Guid id, CancellationToken cancellationToken);
}
