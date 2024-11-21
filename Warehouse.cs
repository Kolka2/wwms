namespace wwms;

public class Warehouse
{
    private readonly int _maxCapacityPerProduct;

    public Warehouse(int maxCapacityPerProduct)
    {
        // TODO: этот параметр нигде не учитывается, а должен.
        // Вместимость упаковки.
        _maxCapacityPerProduct = maxCapacityPerProduct;
    }
    
    public Dictionary<string, Product> Inventory { get; } = new();

    public void AddProduct(Product product)
    {
        if (!Inventory.TryAdd(product.Name, product))
        {
            Inventory[product.Name].IncreaseQuantity(product.Quantity);
        }
    }

    public bool TryFulfillOrder(Order order)
    {
        foreach (var item in order.Items)
        {
            // TODO: Бессмысленно проверять наличие ключа в словаре,
            // скорее нужно проверять количество, чтобы оно было отлично от нуля
            // и если оно равно нулю, то добавить продукт в заявку к поставщику
            // Можно использовать имеющийся класс Order
            if (Inventory.TryGetValue(item.Key, out var product))
            {
                int availableQuantity = product.Quantity;
                int requestedQuantity = item.Value;

                if (availableQuantity >= requestedQuantity)
                {
                    product.ReduceQuantity(requestedQuantity);
                }
                else
                {
                    product.ReduceQuantity(availableQuantity);
                    // TODO: Мы нигде не используем информацию о невыполненной части заказа,
                    // поэтому строку ниже можно удалить.
                    order.UpdateItemQuantity(item.Key, requestedQuantity - availableQuantity);
                }
            }
            else
            {
                order.RemoveItem(item.Key);
            }
        }

        return order.Items.Count > 0;
    }

    public void CheckExpiry(int currentDay)
    {
        foreach (var product in Inventory.Values)
        {
            if (product.ExpiryDays <= currentDay)
            {
                // Списать просроченный товар
                product.ReduceQuantity(product.Quantity);
            }
            else if (product.ExpiryDays - currentDay <= 3)
            {
                // Пример уценки на 20% за 3 дня до истечения срока годности
                product.ApplyDiscount(20);
            }
        }
    }
    // TODO: Использовать интерфейс ISupplier
    public void RequestSupply(Supplier supplier)
    {
        foreach (var product in Inventory.Values)
        {
            if (product.Quantity < _maxCapacityPerProduct / 10)
            {
                supplier.RequestSupply(product.Name, _maxCapacityPerProduct - product.Quantity);
            }
        }
    }
}