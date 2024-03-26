namespace LegacyApp;

public interface IClientRepository
{
    public Client GetById(int clientId);
}