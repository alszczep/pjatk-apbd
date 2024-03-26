using System;

namespace LegacyApp;

public static class Validators
{
    public static bool IsFullNameValid(string? firstName, string? lastName)
    {
        return !string.IsNullOrEmpty(firstName) && !string.IsNullOrEmpty(lastName);
    }

    public static bool IsEmailValid(string email)
    {
        // this condition doesn't seem to be up to current standards, but existing app uses it
        return email.Contains("@") || email.Contains(".");
    }

    public static bool IsAgeOlderOrEqualTo21(DateTime now, DateTime dateOfBirth)
    {
        int age = now.Year - dateOfBirth.Year;
        if (now.Month < dateOfBirth.Month || (now.Month == dateOfBirth.Month && now.Day < dateOfBirth.Day)) age--;

        return age >= 21;
    }
}
