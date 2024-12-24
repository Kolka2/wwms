namespace backend
{
    public class Product
    {
        public string Name { get; set; }
        public double Quantity { get; set; } // Гр или кг или штук в одной пачке
        public int ExpiryDays { get; set; }
        public double Price { get; set; }
        public int MinWholesalePackages { get; set; }

        public Product(string name, double quantity, int expiryDays, double price, int minWholesalePackages)
        {
            Name = name;
            Quantity = quantity;
            ExpiryDays = expiryDays;
            Price = price;
            MinWholesalePackages = minWholesalePackages;
        }

        public Product()
        {
            Name = "";
            Quantity = 0;
            ExpiryDays = 0;
            Price = 0;
            MinWholesalePackages = 0;
        }

      

        public override int GetHashCode()
        {
            return Name.GetHashCode();
        }

        public override string ToString()
        {
            return Name;
        }
    }
}