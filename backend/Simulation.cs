namespace backend
{
    public class Simulation
    {
        private readonly string _filename;
        private readonly double _disc;
        private readonly int _totalDays;
        private readonly int _numStores;
        private readonly int _numProducts;
        private readonly RandomGenerator _randomGenerator;
        private readonly Dictionary<Product, List<WholesalePackage>> _forWh = new(new ProductComparer());
        private string[] _productList;
        private readonly bool _created;
        private Statistic _statistic;

        public Simulation(string filename, double discount, int totaldDays, int numStores, int numProducts,
            RandomGenerator rg)
        {
            _filename = filename;
            _disc = discount;
            _totalDays = totaldDays;
            _numStores = numStores;
            _numProducts = numProducts;
            _randomGenerator = rg;
            _created = false;
        }

        public Simulation(string filename, double discount, int totalDays, int numStore, int numProducts,
            Dictionary<Product, List<WholesalePackage>> products, RandomGenerator rg)
        {
            _filename = filename;
            _disc = discount;
            _randomGenerator = rg;
            _totalDays = totalDays;
            _numStores = numStore;
            _numProducts = numProducts;
            _forWh = products;
            _created = true;
        }

        public void Run()
        {
            _productList =
            [
                "Консервированные овощи", "Макаронные изделия", "Крупы",
                "Растительное масло", "Томатная паста", "Сахар",
                "Мука", "Соль", "Чай", "Кофе", "Шоколад", "Печенье",
                "Молочные продукты", "Мясо и мясопродукты",
                "Рыба и морепродукты", "Фрукты", "Овощи", "Бакалея",
                "Безалкогольные напитки", "Алкогольная продукция"
            ];

            Warehouse wh;
            if (_created)
            {
                wh = new Warehouse(1, _forWh);
            }
            else
            {
                wh = new Warehouse(_totalDays, _numProducts, _productList, 1);
            }


            Supplier supplier = new(wh);

            _statistic = new(wh, _filename);
            List<ShopPackage> shopPacks = []; // Перевозки для следующего дня
            List<ShopOrder> shopOrders = []; // Заказы, что пришли сегодня
            _statistic.AllStat();
            for (int i = 1; i <= _totalDays; i++)
            {
                wh.FullFill();
                _statistic.WarehouseStatus(wh);
                wh.MakeTransport(shopPacks);
                List<WholesalePackage> discpack = wh.DiscountPrices(_disc);
                _statistic.ChangeLostDiscounted(discpack);
                List<WholesalePackage> deleted = wh.RemoveExpirated();
                _statistic.ChangeLostDeleted(deleted);

                _statistic.WarehouseStatus(wh);

                SupplyOrder supOrder = wh.CreateSupplyOrder(); // Сделали новый заказ к поставщику
                SupplyPackage supPackage = supplier.CreatePackage(supOrder); // Сделали посылку от поставщика
                wh.AddPackage(supPackage); // Добавили в ожидание

                // Делаем список заказов перевозки на следующий день
                shopOrders.Clear();
                for (int j = 0; j < _numStores; j++)
                {
                    ShopOrder order = new(wh.Inventory, $"Shop{j}", _randomGenerator);
                    shopOrders.Add(order);
                }

                shopPacks = wh.CreateTransportList(shopOrders); // Сделали новый перевозочный лист
                _statistic.ChangeCostWithoutDiscount(shopPacks);
                _statistic.ChangeCostWithDiscount(shopPacks, _disc);
                _statistic.ForShopPacks(shopPacks);
                _statistic.WarehouseStatus(wh);
                wh.CurrentDay += 1;
                supplier.CurrentDay += 1;
            }

        }
        public string Stats()
        {
            return _statistic.AllStat();
        }
    }
}