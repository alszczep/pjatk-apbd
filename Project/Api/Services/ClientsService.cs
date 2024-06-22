using Api.DTOs;
using api.Models;
using Api.Repositories.Interfaces;
using Api.Services.Interfaces;

namespace Api.Services;

public class ClientsService : IClientsService
{
    private readonly IClientsRepository clientsRepository;

    public ClientsService(IClientsRepository clientsRepository)
    {
        this.clientsRepository = clientsRepository;
    }

    public async Task AddClientAsync(AddClientDTO dto, CancellationToken cancellationToken)
    {
        if (dto.Type == ClientTypeDTO.Individual)
        {
            if (dto.Pesel == null)
                throw new ArgumentException("Pesel is required for individual clients");

            if (dto.FirstName == null)
                throw new ArgumentException("First name is required for individual clients");

            if (dto.LastName == null)
                throw new ArgumentException("Last name is required for individual clients");

            ClientIndividual? existingClient =
                await this.clientsRepository.GetClientByPeselAsync(dto.Pesel, cancellationToken);

            if (existingClient != null)
                throw new ArgumentException("Client with this pesel already exists");

            ClientIndividual client = new()
            {
                Id = Guid.NewGuid(),
                Pesel = dto.Pesel,
                FirstName = dto.FirstName,
                LastName = dto.LastName,
                Address = dto.Address,
                Email = dto.Email,
                PhoneNumber = dto.PhoneNumber
            };

            this.clientsRepository.AddClient(client);
        }
        else
        {
            if (dto.Krs == null)
                throw new ArgumentException("Krs is required for company clients");

            if (dto.CompanyName == null)
                throw new ArgumentException("Company name is required for company clients");

            ClientCompany? existingClient =
                await this.clientsRepository.GetClientByKrsAsync(dto.Krs, cancellationToken);

            if (existingClient != null)
                throw new ArgumentException("Client with this krs already exists");

            ClientCompany client = new()
            {
                Id = Guid.NewGuid(),
                Krs = dto.Krs,
                CompanyName = dto.CompanyName,
                Address = dto.Address,
                Email = dto.Email,
                PhoneNumber = dto.PhoneNumber
            };

            this.clientsRepository.AddClient(client);
        }

        await this.clientsRepository.SaveChangesAsync(cancellationToken);
    }

    public async Task UpdateClientAsync(UpdateClientDTO dto, CancellationToken cancellationToken)
    {
        Client? client = await this.clientsRepository.GetClientByIdAsync(dto.Id, cancellationToken);

        if (client == null)
            throw new ArgumentException("Client not found");

        client.Address = dto.Address;
        client.Email = dto.Email;
        client.PhoneNumber = dto.PhoneNumber;

        if (client.Type == ClientType.Individual)
        {
            if (dto.FirstName == null)
                throw new ArgumentException("First name is required for individual clients");

            if (dto.LastName == null)
                throw new ArgumentException("Last name is required for individual clients");

            ((ClientIndividual)client).FirstName = dto.FirstName;
            ((ClientIndividual)client).LastName = dto.LastName;
        }
        else
        {
            if (dto.CompanyName == null)
                throw new ArgumentException("Company name is required for company clients");

            ((ClientCompany)client).CompanyName = dto.CompanyName;
        }

        this.clientsRepository.UpdateClient(client);
        await this.clientsRepository.SaveChangesAsync(cancellationToken);
    }

    public async Task DeleteClientAsync(Guid id, CancellationToken cancellationToken)
    {
        Client? client = await this.clientsRepository.GetClientByIdAsync(id, cancellationToken);

        if (client == null)
            throw new ArgumentException("Client not found");

        if (client.Type == ClientType.Company)
            throw new InvalidOperationException("Companies cannot be deleted");

        ((ClientIndividual)client).IsDeleted = true;

        this.clientsRepository.UpdateClient(client);
        await this.clientsRepository.SaveChangesAsync(cancellationToken);
    }
}
