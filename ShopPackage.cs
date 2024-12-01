namespace wwms
{
    internal class ShopPackage
    {
        public int _dayForRealize;
        public string Name;
        public Dictionary<Product, int> ItemsWithoutDiscount { get; set; } = new(new ProductComparer());
        public Dictionary<Product, int> ItemsWithDiscount { get; set; } = new(new ProductComparer());

        public ShopPackage(string name)
        {
            Name = name;
        }
        public void AddProductWithDiscount(Product product, int count)
        {
            if (ItemsWithDiscount.ContainsKey(product))
            {
                ItemsWithDiscount[product] += count;
            }
            else
            {
                ItemsWithDiscount.Add(product, count);
            }
        }

        public void AddProductWithoutDiscount(Product product, int count)
        {
            if (ItemsWithoutDiscount.ContainsKey(product))
            {
                ItemsWithoutDiscount[product] += count;
            }
            else
            {
                ItemsWithoutDiscount.Add(product, count);
            }
        }
    }
}
