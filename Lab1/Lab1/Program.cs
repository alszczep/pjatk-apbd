// See https://aka.ms/new-console-template for more information

namespace Lab1;

public static class Program
{
    public static void Main()
    {
        Console.WriteLine(CalculateAverage(
            [1, 2, 3, 4, 5]
        ));
    }

    private static float CalculateAverage(int[] numbers)
    {
        float sum = 0;
        foreach (var number in numbers) sum += number;
        return sum / numbers.Length;
    }
}
