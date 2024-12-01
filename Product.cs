using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Simul
{
    internal class Product:IComparable<Product>,IComparer<Product>
    {
        public string Name { get; set; }
        public double Quantity { get; set; } //Гр или кг или штук в одной пачке
        public int ExpiryDays { get; set; } //срок годности в днях
        public double Price { get;  set; }
        Random r = new Random();
        public int MinWholesalePackages { get; set; }

        public Product(string name, double quantity, int expiryDays, double price, int minWholesalePackages)
        {
            Name = name;
            Quantity = quantity;
            ExpiryDays = expiryDays;
            Price = price;
            MinWholesalePackages = minWholesalePackages;
        }
        public Product() 
        {
            Name = "";
            Quantity=0;
            ExpiryDays=0;
            Price=0;
            MinWholesalePackages=0;
        }
      

        public int CompareTo(Product p)
        {
            return Name.CompareTo(p.Name);
        }
        public  bool Equals(Product x, Product y)
        {
            // Сравниваем только по имени
            return x.Name == y.Name;
        }

        public override int GetHashCode()
        {
            // Хеш-код только по имени
            return Name.GetHashCode();
        }

        int IComparer<Product>.Compare(Product x, Product y)
        {
           return x.Name.CompareTo(y.Name);
        }

        public override string ToString()
        {
            return Name;
        }
    }
}
