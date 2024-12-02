namespace wwms
{
    internal interface IOrder
    {
        Dictionary<Product, int> Items { get; set; }

        void AddItem(Product product, int quantity);

        void UpdateItemQuantity(Product product, int quantity);

        void RemoveItem(Product product);
    }
}