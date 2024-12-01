using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Simul
{
    internal class SupplyOrder : IOrder,IComparable<SupplyOrder>
    {
        public Dictionary<Product, int> Items { get; set; } = new Dictionary<Product, int>(new ProductComparer()); //Продукт и кол-во оптовых упаковок
        public int tempDay;
        public int DeliveryDay;
        public SupplyOrder(int tempDay)
        {
            Random random = new Random();
            DeliveryDay =tempDay+random.Next(1,6);
        }


        public void AddItem(Product product, int quantity) //Добавить продукт в заказ
        {
            if (Items.ContainsKey(product))
            {
               
                Items[product] += quantity;
            }
            else
            {
                Items.Add(product, quantity);
            }
        }

        public void UpdateItemQuantity(Product product, int quantity) //Изменить количество продуктов в заказе
        {
            if (Items.ContainsKey(product))
            {
                Items[product] = quantity;
            }
        }

        public void RemoveItem(Product product) //Удалить продукт
        {
            Items.Remove(product);
        }

        public int CompareTo(SupplyOrder order)
        {
            return DeliveryDay.CompareTo(order.DeliveryDay);
        }
    }
}
