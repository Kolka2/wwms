namespace wwms;

internal class Program
{
    private static void Main(string[] args)
    {
        int days = 20;
        int numStores = 5;
        int numProducts = 15;
        int maxCapacityPerProduct = 100;

        Simulation simulation = new(days, numStores, numProducts, maxCapacityPerProduct);
        simulation.Run();
    }
}
