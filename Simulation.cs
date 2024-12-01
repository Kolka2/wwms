using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Linq.Expressions;
using System.Security.Policy;
using System.Text;
using System.Threading.Tasks;
using System.Xml;
using System.Xml.Linq;
namespace Simul
{
    internal class Simulation
    {

        private readonly Random _random = new Random();
        private readonly string _filename;
        private readonly double _disc;
        private readonly int _totalDays;
        private readonly int _numStores;
        private readonly int _numProducts;
        private readonly RandomGenerator _randomGenerator = new RandomGenerator();
        public Dictionary<Product, List<WholesalePackage>> forWh = new Dictionary<Product, List<WholesalePackage>>(new ProductComparer());
        private string[] _productList;
        public Warehouse wh;
        public int _MinimalCountProducts;
        public bool created = true;

        public Simulation(string f,double d,int totaldDays,int numStores,int numProducts,int MinimalCountProducts, RandomGenerator r1) {
            _filename = f;
            _disc = d;
            _totalDays = totaldDays;
            _numStores = numStores;
            _numProducts = numProducts;
            _MinimalCountProducts = MinimalCountProducts;
            _randomGenerator = r1;
            created = false;
        }
        public Simulation(string f,double d,int totalDays, int numStore,int numProducts,int MinimalCountProducts,Dictionary<Product,List<WholesalePackage>> products,RandomGenerator r1)
        {
            _filename = f;
            _disc = d;
            _randomGenerator = r1;
            _totalDays = totalDays;
            _numStores = numStore;
            _numProducts=numProducts;
            _MinimalCountProducts = MinimalCountProducts;
            forWh = products;
            created = true;


        }

        public void Run()
        {
            _productList = new string[]{"Консервированные овощи", "Макаронные изделия", "Крупы",
        "Растительное масло", "Томатная паста", "Сахар",
        "Мука", "Соль", "Чай", "Кофе", "Шоколад", "Печенье",
        "Молочные продукты", "Мясо и мясопродукты",
        "Рыба и морепродукты", "Фрукты", "Овощи", "Бакалея",
        "Безалкогольные напитки", "Алкогольная продукция",};

            //Warehouse wh = new Warehouse(_totalDays,_numStores,_numProducts,_productList,1);
            //Supplier supplier = new Supplier(wh);
            // Product p = new Product("Крупы", 1, 1, 1, 1);
            // SupplyOrder so = wh.CreateSupplyOrder();
            // SupplyPackage  sp = supplier.CreatePackage(so);
            // sp.DeliveryDay = 1;
            // wh.AddPackge(sp);
            // wh.FullFill();

            // shopOrder order = new shopOrder(wh.Inventory);
            // shopOrder order2 = new shopOrder(wh.Inventory);
            // List<shopOrder> orders = new List<shopOrder>() { order, order2 };
            //List<shopOrder> tr = wh.createTransportList(orders);
            // wh.MakeTransport(tr);

            // Начальный список продуктов не формируется случайным образом, а задается!

            ///
            Warehouse wh = new Warehouse(); //Склад
            if (created)
            {
                
               wh = new Warehouse(1, forWh);
            }
           else
            {
                 wh = new Warehouse(_totalDays, _numStores, _numProducts, _productList, 1);
            }

           
            Supplier supplier = new Supplier(wh); //Поставщик
            
            Statistic statistic = new Statistic(wh,_filename); //Статистика 
            List<shopPackage> shopPacks = new List<shopPackage>(); //Перевозки для следующего дня
            List<shopOrder> shopOrders = new List<shopOrder>(); //Заказы что пришли сегодня
            statistic.AllStat();
            for (int i = 1; i <= _totalDays;i++)
            {
                Console.WriteLine("===============");
                Console.WriteLine("===============");
                Console.WriteLine($"День:{i}");
               
                
               
                wh.FullFill(); //сделали обновление ассортимента
                Console.WriteLine("После обновления асссортимента");
                statistic.WarehouseStatus(wh);
                wh.MakeTransport(shopPacks); //Сделали перевозки по магазинам
                List<WholesalePackage> discpack = wh.DiscountPrices(_disc); //Сделали скидки //возвращаемое значение создать
                statistic.ChangeLostDiscounted(discpack);
              List<WholesalePackage> Deleted = wh.RemoveExpirated(); //удалили просрочку //Возвращаемое значение создать
                statistic.ChangeLostDeleted(Deleted);
                
                Console.WriteLine("После перевозок");
                statistic.WarehouseStatus(wh);
               
               SupplyOrder supOrder = wh.CreateSupplyOrder(); //Сделали новый заказ к поставщику
                SupplyPackage supPackage = supplier.CreatePackage(supOrder);//сделали посылку от поставщика
                wh.AddPackge(supPackage); //Добавили в ожидание
                
                //Делаем список заказов перевозки на следующий день
                shopOrders.Clear();
                for (int j = 0; j < _numStores; j++)
                {
                    shopOrder order = new shopOrder(wh.Inventory,$"Shop{j}",_randomGenerator);     
                    shopOrders.Add(order);
                }
                Console.WriteLine($"Перевозки по магазинам на {i + 1}:");
                shopPacks = wh.createTransportList(shopOrders); //Сделали новый перевозочный лист
                statistic.ChangeCostWithoutDiscount(shopPacks);
                statistic.ChangeCostWithDiscount(shopPacks, _disc);
                statistic.ForShopPacks(shopPacks);
                Console.WriteLine($"Состояние WAREHOUSE");
                statistic.WarehouseStatus(wh);
                Console.WriteLine("===============");
                Console.WriteLine("===============");
                wh._tempday += 1;
                supplier._tempday += 1;



            }
            statistic.AllStat();

        }

    }
}
