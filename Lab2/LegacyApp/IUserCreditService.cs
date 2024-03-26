using System;

namespace LegacyApp;

public interface IUserCreditService : IDisposable
{
    public int GetCreditLimit(string lastName, DateTime dateOfBirth);
}