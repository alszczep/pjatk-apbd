using LegacyApp;

namespace LegacyAppTests;

public class UnitTest1
{
    [Fact]
    public void AddUser_Should_Return_False_When_Email_Without_At_And_Dot()
    {
        string firstName = "John";
        string lastName = "Doe";
        string email = "doe";
        DateTime birthDate = new DateTime(1980, 1, 1);
        int clientId = 1;
        var service = new UserService();
        
        bool result = service.AddUser(firstName, lastName, email, birthDate, clientId);
        
        Assert.False(result);
    }
}