namespace wwms
{
    internal class Product : IComparable<Product>, IComparer<Product>
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


        public int CompareTo(Product p)
        {
            return Name.CompareTo(p.Name);
        }

        public bool Equals(Product x, Product y)
        {
            return x.Name == y.Name;
        }

        public override int GetHashCode()
        {
            return Name.GetHashCode();
        }

        int IComparer<Product>.Compare(Product x, Product y)
        {
            return x.Name.CompareTo(y.Name);
        }

        public override string ToString()
        {
            return Name;
        }
    }
}