namespace backend
{
    public class ShopOrder
    {
        public readonly string Name;

        public Dictionary<Product, double> ItemsWithoutDiscount { get; } = new(new ProductComparer());

        public Dictionary<Product, double> ItemsWithDiscount { get; } = new(new ProductComparer());


        public ShopOrder(Dictionary<Product, List<WholesalePackage>> inventory, string n, RandomGenerator r)
        {
            Name = n;
            List<Product> products = r.GetListProducts(inventory.Keys.ToList());

            foreach (Product product in products)
            {
                double count = r.GenQuantity();
                List<WholesalePackage> packages = inventory[product];
                if (!ItemsWithoutDiscount.TryAdd(product, count))
                {
                    ItemsWithoutDiscount[product] += count;
                }

                foreach (WholesalePackage package in packages)
                {
                    if (!package.IsDiscounted) continue;
                    
                    if (!ItemsWithDiscount.ContainsKey(package))
                    {
                        ItemsWithDiscount.Add(package, r.GenQuantity());
                    }
                    else
                    {
                        ItemsWithDiscount[package] += r.GenQuantity();
                    }
                }
            }
        }
    }
}