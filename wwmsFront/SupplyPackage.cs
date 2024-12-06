namespace wwms
{
    internal class SupplyPackage
    {
        public SupplyOrder order;
        public Dictionary<Product, List<WholesalePackage>> Products = new(new ProductComparer());
        public int DeliveryDay;

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