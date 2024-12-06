namespace wwms
{
    internal class Supplier
    {
        public int _tempday;

        public Dictionary<Product, WholesalePackage> Packages = new(new ProductComparer());

        public Supplier(Warehouse wh)
        {
            foreach (Product key in wh.Inventory.Keys)
            {
                _tempday = wh._tempday;
                Packages.Add(key, wh.Inventory[key][0]);
            }
        }


        public SupplyPackage CreatePackage(SupplyOrder order)
        {
            SupplyPackage sp = new SupplyPackage(order);
            foreach (Product product in order.Items.Keys)
            {
                List<WholesalePackage> list = new();
                for (int i = 0; i < order.Items[product]; i++)
                {
                    WholesalePackage p = new WholesalePackage(product, Packages[product].PackageCount,
                        _tempday + product.ExpiryDays);
                    list.Add(p);
                }

                sp.AddProduct(product, list);
            }

            return sp;
        }
    }
}