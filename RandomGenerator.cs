namespace wwms;

public class RandomGenerator
{
    private readonly Random _random = new Random();

    public int Next(int min, int max)
    {
        return _random.Next(min, max);
    }

    public decimal NextDecimal(decimal min, decimal max)
    {
        return (decimal)_random.NextDouble() * (max - min) + min;
    }
}