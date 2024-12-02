namespace wwms
{
    internal class ShopPackage
    {
        public readonly string Name;
        public Dictionary<Product, int> ItemsWithoutDiscount { get; } = new(new ProductComparer());
        public Dictionary<Product, int> ItemsWithDiscount { get; } = new(new ProductComparer());

        public ShopPackage(string name)
        {
            Name = name;
        }
        public void AddProductWithDiscount(Product product, int count)
        {
            if (!ItemsWithDiscount.TryAdd(product, count))
            {
                ItemsWithDiscount[product] += count;
            }
        }

        public void AddProductWithoutDiscount(Product product, int count)
        {
            if (!ItemsWithoutDiscount.TryAdd(product, count))
            {
                ItemsWithoutDiscount[product] += count;
            }
        }
    }
}
