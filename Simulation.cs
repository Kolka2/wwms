namespace wwms;

public class Simulation
{
    private readonly Warehouse _warehouse;
    private readonly Supplier _supplier;
    private readonly Random _random = new();
    private readonly RandomGenerator _randomGenerator;
    private readonly Statistics _statistics;
    private readonly int _totalDays;
    private readonly int _numStores;
    private readonly int _numProducts;
    private string[] _productList = [
        "Консервированные овощи", "Макаронные изделия", "Крупы",
        "Растительное масло", "Томатная паста", "Сахар",
        "Мука", "Соль", "Чай", "Кофе", "Шоколад", "Печенье",
        "Молочные продукты", "Мясо и мясопродукты",
        "Рыба и морепродукты", "Фрукты", "Овощи", "Бакалея",
        "Безалкогольные напитки", "Алкогольная продукция"
    ];
    private string[] ProductList
    {
        get => _productList;
        set
        {
            // https://learn.microsoft.com/en-us/dotnet/csharp/language-reference/operators/patterns
            if (value.Length is >= 12 and <= 20)
                _productList = value;
        }
    }
    public int CurrentDay { get; private set; }

    public Simulation(int totalDays, int numStores, int numProducts, int maxCapacityPerProduct)
    {
        _totalDays = totalDays;
        CurrentDay = 1;
        _numStores = numStores;
        _numProducts = numProducts;
        _randomGenerator = new RandomGenerator();
        _warehouse = new Warehouse(maxCapacityPerProduct);
        _supplier = new Supplier(_randomGenerator);
        _statistics = new Statistics();
    }

    public void Run()
    {
        InitializeWarehouse();

        for (; CurrentDay <= _totalDays; CurrentDay++)
        {
            ProcessOrders(CurrentDay);
            CheckExpiryAndRecordLosses(CurrentDay);
            _warehouse.RequestSupply(_supplier);
            // TODO: Убрать это из симуляции
            var supplies = _supplier.FulfillSupply(_warehouse);
            foreach (var supply in supplies)
            {
                _warehouse.AddProduct(new Product(supply.Key, supply.Value,
                    _randomGenerator.Next(CurrentDay, _totalDays), _randomGenerator.NextDecimal(10, 100)));
            }
        }

        _statistics.PrintStatistics();
    }

    private void InitializeWarehouse()
    {
        for (int i = 0; i < _numProducts; i++)
        {
            string productName = ProductList[i];
            int quantity = _randomGenerator.Next(10, 50);
            int expiryDays = _randomGenerator.Next(5, _totalDays);
            decimal price = _randomGenerator.NextDecimal(10, 100);
            _warehouse.AddProduct(new Product(productName, quantity, expiryDays, price));
        }
    }

    // TODO: магазин с большей охотой должен брать уценённый товар
    private void ProcessOrders(int day)
    {
        for (int i = 0; i < _numStores; i++)
        {
            // TODO: Создать класс (интерфейс) магазина?
            // var order = GenerateOrder();
            // if (_warehouse.TryFulfillOrder(order))
            // {
            //     decimal orderRevenue = CalculateOrderRevenue(order);
            //     _statistics.RecordOrder(order.Items.Sum(item => item.Value), orderRevenue);
            // }
        }
    }
    private void ProcessOrders()
    {
        for (int i = 0; i < _numStores; i++)
        {
            // Магазин.СгенерироватьЗаказ
            // Магазин.
        }
    }

    private decimal CalculateOrderRevenue(Order order)
    {
        decimal revenue = 0;
        foreach (var item in order.Items)
        {
            if (_warehouse.Inventory.TryGetValue(item.Key, out var product))
            {
                revenue += product.DiscountedPrice * item.Value;
            }
        }
        return revenue;
    }

    private void CheckExpiryAndRecordLosses(int day)
    {
        var expiredProducts = new List<Product>();
        foreach (var product in _warehouse.Inventory.Values)
        {
            if (product.ExpiryDays <= day)
            {
                expiredProducts.Add(product);
                _statistics.RecordExpiredProduct(product.Quantity);
            }
            else if (product.ExpiryDays - day <= 3)
            {
                decimal discountLoss = product.Quantity * (product.Price - product.DiscountedPrice);
                _statistics.RecordDiscountLoss(discountLoss);
            }
        }

        foreach (var expiredProduct in expiredProducts)
        {
            _warehouse.Inventory.Remove(expiredProduct.Name);
        }
    }
    private Order GenerateOrder()
    {
        var order = new Order();
        for (int i = 0; i < _random.Next(0, _numProducts); i++)
        {
            string productName = ProductList[_random.Next(0, _numProducts - 1)];
            // TODO: Подправить верхнюю границу?
            int quantity = _randomGenerator.Next(0, 10);
            order.AddItem(productName, quantity);
        }
        return order;
    }
}