namespace wwms;

public class Order
{
    public Dictionary<string, int> Items { get; } = new();

    public void AddItem(string productName, int quantity)
    {
        if (!Items.TryAdd(productName, quantity))
        {
            Items[productName] += quantity;
        }
    }

    public void UpdateItemQuantity(string productName, int quantity)
    {
        if (Items.ContainsKey(productName))
        {
            Items[productName] = quantity;
        }
    }

    public void RemoveItem(string productName)
    {
        Items.Remove(productName);
    }
}