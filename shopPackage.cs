using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Simul
{
    internal class shopPackage
    {
        public int _dayForRealize;
        public string Name;
        public Dictionary<Product, int> ItemsWithoutDiscount { get; set; } = new Dictionary<Product, int>(new ProductComparer());
        public Dictionary<Product, int> ItemsWithDiscount { get; set; } = new Dictionary<Product, int>(new ProductComparer());

        public shopPackage(string name)
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
