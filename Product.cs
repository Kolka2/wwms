namespace wwms;

// TODO: Добавить класс оптовых упаковок
public class Product
{
    public string Name { get; }
    public int Quantity { get; private set; }
    public int ExpiryDays { get; }
    public decimal Price { get; private set; }
    public decimal DiscountedPrice { get; private set; }
    public bool IsDiscounted => DiscountedPrice < Price;

    public Product(string name, int quantity, int expiryDays, decimal price)
    {
        Name = name;
        Quantity = quantity;
        ExpiryDays = expiryDays;
        Price = price;
        DiscountedPrice = price;
    }

    public void ApplyDiscount(decimal discountPercentage)
    {
        DiscountedPrice = Price * (1 - discountPercentage / 100);
    }

    public void ReduceQuantity(int amount)
    {
        if (amount > Quantity)
            throw new ArgumentException("Not enough quantity available.");
        Quantity -= amount;
    }

    public void IncreaseQuantity(int amount)
    {
        Quantity += amount;
    }
}