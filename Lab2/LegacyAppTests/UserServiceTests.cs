using LegacyApp;
using LegacyApp.Clients;
using LegacyAppTests.Mocks;

namespace LegacyAppTests;

public class UserServiceTests
{
    public class CreateUser
    {
        [Fact]
        public void Should_Return_User_For_Client_Type_VeryImportantClient()
        {
            int creditLimit = 100;
            Client client = new Client { Type = "VeryImportantClient" };
            ClientRepositoryMock clientRepositoryMock = new ClientRepositoryMock(client);
            Func<UserCreditServiceMock> userCreditServiceMockFactory = () => new UserCreditServiceMock(creditLimit);
            UserService userService = new UserService(clientRepositoryMock, userCreditServiceMockFactory);

            string firstName = "John";
            string lastName = "Doe";
            string email = "john@test.com";
            DateTime dateOfBirth = DateTime.Now.AddYears(-25);
            int clientId = 1;

            var result = userService.CreateUser(firstName, lastName, email, dateOfBirth, clientId);

            Assert.NotNull(result);
            Assert.Equal(firstName, result.FirstName);
            Assert.Equal(lastName, result.LastName);
            Assert.Equal(email, result.EmailAddress);
            Assert.Equal(dateOfBirth, result.DateOfBirth);
            Assert.Equal(client, result.Client);
            Assert.False(result.HasCreditLimit);
        }

        [Fact]
        public void Should_Return_User_For_Client_Type_ImportantClient()
        {
            int creditLimit = 100;
            int creditLimitAfter = 200;
            Client client = new Client { Type = "ImportantClient" };
            ClientRepositoryMock clientRepositoryMock = new ClientRepositoryMock(client);
            Func<UserCreditServiceMock> userCreditServiceMockFactory = () => new UserCreditServiceMock(creditLimit);
            UserService userService = new UserService(clientRepositoryMock, userCreditServiceMockFactory);


            string firstName = "John";
            string lastName = "Doe";
            string email = "john@test.com";
            DateTime dateOfBirth = DateTime.Now.AddYears(-25);
            int clientId = 1;

            var result = userService.CreateUser(firstName, lastName, email, dateOfBirth, clientId);

            Assert.NotNull(result);
            Assert.Equal(firstName, result.FirstName);
            Assert.Equal(lastName, result.LastName);
            Assert.Equal(email, result.EmailAddress);
            Assert.Equal(dateOfBirth, result.DateOfBirth);
            Assert.Equal(client, result.Client);
            Assert.False(result.HasCreditLimit);
            Assert.Equal(result.CreditLimit, creditLimitAfter);
        }

        [Fact]
        public void Should_Return_User_For_Client_Type_Any_With_CreditLimit_Above_500()
        {
            int creditLimit = 600;
            Client client = new Client { Type = "Any" };
            ClientRepositoryMock clientRepositoryMock = new ClientRepositoryMock(client);
            Func<UserCreditServiceMock> userCreditServiceMockFactory = () => new UserCreditServiceMock(creditLimit);
            UserService userService = new UserService(clientRepositoryMock, userCreditServiceMockFactory);


            string firstName = "John";
            string lastName = "Doe";
            string email = "john@test.com";
            DateTime dateOfBirth = DateTime.Now.AddYears(-25);
            int clientId = 1;

            var result = userService.CreateUser(firstName, lastName, email, dateOfBirth, clientId);

            Assert.NotNull(result);
            Assert.Equal(firstName, result.FirstName);
            Assert.Equal(lastName, result.LastName);
            Assert.Equal(email, result.EmailAddress);
            Assert.Equal(dateOfBirth, result.DateOfBirth);
            Assert.Equal(client, result.Client);
            Assert.True(result.HasCreditLimit);
            Assert.Equal(result.CreditLimit, creditLimit);
        }

        [Fact]
        public void Should_Return_Null_For_Client_Type_Any_With_CreditLimit_Below_500()
        {
            int creditLimit = 400;
            Client client = new Client { Type = "Any" };
            ClientRepositoryMock clientRepositoryMock = new ClientRepositoryMock(client);
            Func<UserCreditServiceMock> userCreditServiceMockFactory = () => new UserCreditServiceMock(creditLimit);
            UserService userService = new UserService(clientRepositoryMock, userCreditServiceMockFactory);

            string firstName = "John";
            string lastName = "Doe";
            string email = "john@test.com";
            DateTime dateOfBirth = DateTime.Now.AddYears(-25);
            int clientId = 1;

            var result = userService.CreateUser(firstName, lastName, email, dateOfBirth, clientId);

            Assert.Null(result);
        }
    }
}
