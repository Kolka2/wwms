namespace wwms;

public class Supplier
{
    private readonly RandomGenerator _randomGenerator;
    private readonly Dictionary<string, int> _supplyRequests = new();

    public Supplier(RandomGenerator randomGenerator)
    {
        _randomGenerator = randomGenerator;
    }

    //
    public void RequestSupply(string productName, int quantity)
    {
        if (!_supplyRequests.TryAdd(productName, quantity))
        {
            // TODO: Генерировать новые продукты со случайным 
            _supplyRequests[productName] += quantity;
        }
    }

    public Dictionary<string, int> FulfillSupply(Warehouse warehouse)
    {
        var supplies = new Dictionary<string, int>();
        foreach (var request in _supplyRequests)
        {
            int deliveryTime = _randomGenerator.Next(1, 6); // Случайное время доставки от 1 до 5 дней
            supplies[request.Key] = request.Value;
        }
        _supplyRequests.Clear();
        return supplies;
    }
}