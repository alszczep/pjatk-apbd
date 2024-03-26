namespace LegacyApp.Clients;

public interface IClientRepository
{
    public Client GetById(int clientId);
}
