using System;

namespace LegacyApp.Users;

public interface IUserCreditService : IDisposable
{
    public int GetCreditLimit(string lastName, DateTime dateOfBirth);
}
