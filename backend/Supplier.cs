namespace backend
{
    public class Supplier
    {
        public int CurrentDay;
        private readonly Dictionary<Product, WholesalePackage> _packages = new(new ProductComparer());

        public Supplier(Warehouse wh)
        {
            foreach (Product key in wh.Inventory.Keys)
            {
                CurrentDay = wh.CurrentDay;
                _packages.Add(key, wh.Inventory[key][0]);
            }
        }


        public SupplyPackage CreatePackage(SupplyOrder order)
        {
            SupplyPackage sp = new SupplyPackage(order);
            foreach (Product product in order.Items.Keys)
            {
                List<WholesalePackage> list = [];
                for (int i = 0; i < order.Items[product]; i++)
                {
                    WholesalePackage p = new WholesalePackage(product, _packages[product].PackageCount,
                        CurrentDay + product.ExpiryDays);
                    list.Add(p);
                }

                sp.AddProduct(product, list);
            }

            return sp;
        }
    }
}