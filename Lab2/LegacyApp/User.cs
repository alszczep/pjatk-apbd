using System;

// should be in users namespace, but cannot be moved since it's being used by the external app
namespace LegacyApp;

public class User
{
    public object Client { get; internal set; }
    public DateTime DateOfBirth { get; internal set; }
    public string EmailAddress { get; internal set; }
    public string FirstName { get; internal set; }
    public string LastName { get; internal set; }
    public bool HasCreditLimit { get; internal set; }
    public int CreditLimit { get; internal set; }
}
