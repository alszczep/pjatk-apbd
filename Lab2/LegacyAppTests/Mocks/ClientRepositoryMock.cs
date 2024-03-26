using LegacyApp;

namespace LegacyAppTests.Mocks;

public class ClientRepositoryMock : IClientRepository
{
    private Client MockClient { get; }

    public ClientRepositoryMock(Client mockClient)
    {
        this.MockClient = mockClient;
    }

    public Client GetById(int clientId)
    {
        return this.MockClient;
    }
}
