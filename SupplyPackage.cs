using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Simul
{
    internal class SupplyPackage
    {
        public SupplyOrder order;
        public Dictionary<Product,List<WholesalePackage>> Products = new Dictionary<Product, List<WholesalePackage>>(new ProductComparer());
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
