using api.Models;
using Api.Repositories.Interfaces;

namespace Tests.Fakes;

public class FakeClientsRepository : IClientsRepository
{
    public Task<Client?> GetClientByIdAsync(Guid id, CancellationToken cancellationToken)
    {
        if (id == FakesConsts.ClientIndividual1.Id) return Task.FromResult<Client?>(FakesConsts.ClientIndividual1);
        if (id == FakesConsts.ClientCompany1.Id) return Task.FromResult<Client?>(FakesConsts.ClientCompany1);
        return Task.FromResult<Client?>(null);
    }

    public Task<Client?> GetClientWithContractsAndSubscriptionsWithPaymentsAndSoftwareProductsByIdAsync(Guid id,
        CancellationToken cancellationToken)
    {
        if (id == FakesConsts.ClientIndividual1.Id) return Task.FromResult<Client?>(FakesConsts.ClientIndividual1);
        if (id == FakesConsts.ClientIndividual2.Id) return Task.FromResult<Client?>(FakesConsts.ClientIndividual2);
        if (id == FakesConsts.ClientIndividual3.Id) return Task.FromResult<Client?>(FakesConsts.ClientIndividual3);
        if (id == FakesConsts.ClientIndividual4.Id) return Task.FromResult<Client?>(FakesConsts.ClientIndividual4);
        if (id == FakesConsts.ClientCompany1.Id) return Task.FromResult<Client?>(FakesConsts.ClientCompany1);
        return Task.FromResult<Client?>(null);
    }

    public Task<ClientIndividual?> GetClientByPeselAsync(string pesel, CancellationToken cancellationToken)
    {
        if (pesel == FakesConsts.ClientIndividual1.Pesel)
            return Task.FromResult<ClientIndividual?>(FakesConsts.ClientIndividual1);
        return Task.FromResult<ClientIndividual?>(null);
    }

    public Task<ClientCompany?> GetClientByKrsAsync(string krs, CancellationToken cancellationToken)
    {
        if (krs == FakesConsts.ClientCompany1.Krs) return Task.FromResult<ClientCompany?>(FakesConsts.ClientCompany1);
        return Task.FromResult<ClientCompany?>(null);
    }

    public void AddClient(Client client)
    {
    }

    public void UpdateClient(Client client)
    {
    }

    public Task SaveChangesAsync(CancellationToken cancellationToken)
    {
        return Task.CompletedTask;
    }
}
