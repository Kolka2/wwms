namespace wwms;

public class Statistics
{
    public int TotalOrdersProcessed { get; private set; }
    public int TotalProductsSold { get; private set; }
    public decimal TotalRevenue { get; private set; }
    public int TotalExpiredProducts { get; private set; }
    public decimal TotalDiscountLoss { get; private set; }

    public void RecordOrder(int productsSold, decimal revenue)
    {
        TotalOrdersProcessed++;
        TotalProductsSold += productsSold;
        TotalRevenue += revenue;
    }

    public void RecordExpiredProduct(int quantity)
    {
        TotalExpiredProducts += quantity;
    }

    public void RecordDiscountLoss(decimal loss)
    {
        TotalDiscountLoss += loss;
    }

    public void PrintStatistics()
    {
        Console.WriteLine("Simulation Statistics:");
        Console.WriteLine($"Total Orders Processed: {TotalOrdersProcessed}");
        Console.WriteLine($"Total Products Sold: {TotalProductsSold}");
        Console.WriteLine($"Total Revenue: {TotalRevenue:C}");
        Console.WriteLine($"Total Expired Products: {TotalExpiredProducts}");
        Console.WriteLine($"Total Discount Loss: {TotalDiscountLoss:C}");
    }
}