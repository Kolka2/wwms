namespace backend
{
    public class SupplyOrder : IComparable<SupplyOrder>
    {
        public Dictionary<Product, int> Items { get; set; } = new(new ProductComparer());
        public readonly int DeliveryDay;

        public SupplyOrder(int tempDay)
        {
            Random random = new();
            DeliveryDay = tempDay + random.Next(1, 6);
        }

        public void AddItem(Product product, int quantity)
        {
            if (!Items.TryAdd(product, quantity))
            {
                Items[product] += quantity;
            }
        }

        public void UpdateItemQuantity(Product product, int quantity)
        {
            if (Items.ContainsKey(product))
            {
                Items[product] = quantity;
            }
        }

        public void RemoveItem(Product product)
        {
            Items.Remove(product);
        }

        public int CompareTo(SupplyOrder order)
        {
            return DeliveryDay.CompareTo(order.DeliveryDay);
        }
    }
}