namespace backend
{
    public class SupplyPackage
    {
        public readonly Dictionary<Product, List<WholesalePackage>> Products = new(new ProductComparer());
        public readonly int DeliveryDay;

        public SupplyPackage(SupplyOrder order)
        {
            DeliveryDay = order.DeliveryDay;
        }

        public void AddProduct(Product product, List<WholesalePackage> list)
        {
            Products.Add(product, list);
        }
    }
}