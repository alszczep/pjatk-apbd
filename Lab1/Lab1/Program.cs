// See https://aka.ms/new-console-template for more information

namespace Lab1;

public static class Program
{
    public static void Main()
    {
        Console.WriteLine(CalculateAverage(
            [1, 2, 3, 4, 5]
        ));

        Console.WriteLine(FindMax(
            [1, 2, 3, 4, 5]
        ));
    }

    private static float CalculateAverage(int[] numbers)
    {
        float sum = 0;

        foreach (var number1 in numbers) sum += number1;

        return sum / numbers.Length;
    }

    private static int FindMax(int[] numbers)
    {
        var max = numbers[0];

        foreach (var number in numbers)
            if (number > max)
                max = number;

        return max;
    }
}
