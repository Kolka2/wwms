using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Simul
{
    internal class shopOrder
    {
    
        public int _MinimalCountProducts;
        public string Name;
        public RandomGenerator r;
        public Dictionary<Product, double> ItemsWithoutDiscount { get; set; } = new Dictionary<Product, double>(new ProductComparer());
        public Dictionary<Product, double> ItemsWithDiscount { get; set; } = new Dictionary<Product, double>(new ProductComparer());


        public shopOrder(Dictionary<Product,List<WholesalePackage>> inventory,string n, RandomGenerator r1) 
        {
            r = r1;

            //Переписать и сразу забирать оптовые упаковки со скидкой
            Name = n;
           
            List<Product> products = r.GetListProducts(inventory.Keys.ToList());
            //Их обязательно должно быть очень много
            foreach (Product product in products)
            {
                double count = r.GenQuantity();
                List<WholesalePackage> packages = inventory[product];
                if (!ItemsWithoutDiscount.ContainsKey(product))
                {
                    ItemsWithoutDiscount.Add(product, count);
                }
                else
                {
                    ItemsWithoutDiscount[product] += count;
                }

                foreach (WholesalePackage package in packages) {
                    if (package.IsDicounted)
                    {
                        if (!ItemsWithDiscount.ContainsKey(package))
                        {
                            ItemsWithDiscount.Add(package, r.GenQuantity()); //Добавить сюда пользовательский рандом
                        }
                        else
                        {
                            ItemsWithDiscount[package] += r.GenQuantity(); //Добавить и сюда пользовательский рандом
                        }
                    }    
                }

            }



        }
        public shopOrder()
        { }
        public void AddProductWithDiscount(Product product,double count)
        {
            if (ItemsWithDiscount.ContainsKey(product))
            {
                ItemsWithDiscount[product]+= count;
            }
            else
            {
                ItemsWithDiscount.Add(product, count);
            }
        }

        public void AddProductWithoutDiscount(Product product, double count)
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
