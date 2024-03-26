using LegacyApp.Users;

namespace LegacyAppTests.Mocks;

public class UserCreditServiceMock : IUserCreditService
{
    private int MockCreditLimit { get; }

    public UserCreditServiceMock(int mockCreditLimit)
    {
        this.MockCreditLimit = mockCreditLimit;
    }

    public void Dispose() {}

    public int GetCreditLimit(string lastName, DateTime dateOfBirth)
    {
        return this.MockCreditLimit;
    }
}
