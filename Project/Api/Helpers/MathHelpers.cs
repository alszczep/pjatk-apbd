namespace Api.Helpers;

public class MathHelpers
{
    public static decimal PercentageToMultiplier(decimal percentage)
    {
        return 1 - percentage / 100;
    }
}
