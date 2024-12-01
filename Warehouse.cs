using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Simul
{
    internal class Warehouse
    {
      
    
        
        private string[] _productList;
        // private readonly int _totalDays; вынесем в отдельный класс
        public int _tempday; //Текущий день
        private List<SupplyPackage> _WaitPackage = new List<SupplyPackage>(); //Посылки от поставщика ожидающие распаковки
        public List<shopOrder> _TranportList = new List<shopOrder>(); 
        public Dictionary<Product, List<WholesalePackage>> Inventory = new Dictionary<Product, List<WholesalePackage>>(new ProductComparer());//Поиск уцененки по листу
        public Dictionary<Product,WholesalePackage> helper = new Dictionary<Product, WholesalePackage>(new ProductComparer());//Вспомогательный словарь
        
        public Warehouse()
        { }
        public Warehouse(int day,Dictionary<Product,List<WholesalePackage>> inv) 
        {
            Inventory = inv;
            _tempday = day;

        }
        public Warehouse (int _totalDays,int _numStores,int _numProducts, string[] _productList,int day)
        {

          
            List<Product> products = new List<Product>();
            Dictionary<Product, List<WholesalePackage>> inv = new Dictionary<Product, List<WholesalePackage>>();
            RandomGenerator _randomGenerator = new RandomGenerator(_numProducts);


            for (int i = 0; i < _numProducts; i++)
            {
                string productName = _productList[i];
                double quantity = _randomGenerator.NextDouble(10, 50);
                int expiryDays = _randomGenerator.NextInt(5, _totalDays);
                double price = _randomGenerator.NextDouble(10, 100);
                int minPackages = _randomGenerator.NextInt(10, 30);
                Product p = new Product(productName, quantity, expiryDays, price, minPackages);
                products.Add(p);

            }

            foreach (Product p in products)
            {
                int Count = _randomGenerator.NextInt(10, 20);
                List<WholesalePackage> packages = new List<WholesalePackage>();
                WholesalePackage package = new WholesalePackage(p, Count,p.ExpiryDays+day);
                helper.Add(p, package);
                for (int i = 0; i < Count; i++)
                {


                    packages.Add(package);

                }

                inv.Add(p, packages);
            }

            _tempday = 1;
            foreach (Product p in inv.Keys)
            {
                Inventory.Add(p, inv[p]);
            }    
                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                    
            
        }

        public SupplyOrder CreateSupplyOrder() //Создать заказ к поставщику
        {
             SupplyOrder order = new SupplyOrder(_tempday);
            foreach (Product product in Inventory.Keys)
            {
                if (Inventory[product].Count()==0)
                {
                    order.AddItem(product, product.MinWholesalePackages + 5);
                }
                else if (Inventory[product].Count()<product.MinWholesalePackages)
                {
                    int temp = Inventory[product].Count();
                    int need = product.MinWholesalePackages;
                    int fororder = need-temp;
                    order.AddItem(product, fororder);
                }
                else if (Inventory[product].Count() == product.MinWholesalePackages)
                {              
                    int need = product.MinWholesalePackages;
                    order.AddItem(product, need+5);
                }

            }
            
            return order;
        }
        public void AddPackge(SupplyPackage s) //Добавить посылку от поставщика в ожидаемые
        {
            _WaitPackage.Add(s);

        }
        public void FullFill() //Пополнить свои запасы после отдачи заказа поставщиком
        {
            List<SupplyPackage> RealisedSupplys= new List<SupplyPackage> ();
            if (_WaitPackage.Count() != 0)
            {
                foreach (SupplyPackage sp in _WaitPackage)
                {
                    if (sp.DeliveryDay == _tempday)
                    {
                        RealisedSupplys.Add(sp);
                        foreach (Product product in sp.Products.Keys)
                        {
                            var tempList = sp.Products[product].ToList();
                            foreach (WholesalePackage package in tempList)
                            {
                                Inventory[product].Add(package);
                            }
                        }
                    }

                }
                foreach (SupplyPackage pack in RealisedSupplys)
                {
                    _WaitPackage.Remove(pack);
                }
            }


            
        }
       
        
        
        public void MakeTransport(List<shopPackage> packs)
        {
            foreach (shopPackage pack in packs)
            {
                foreach (Product p in pack.ItemsWithDiscount.Keys )
                {
                    int CountForDelete = (int)pack.ItemsWithDiscount[p];
                    var itemsToDelete = Inventory[p]
                    .Where(pck => pck.IsDicounted)
                    .Take(CountForDelete)
                    .ToList();

                    foreach (var item in itemsToDelete)
                    {
                        Inventory[p].Remove(item);
                    }

                }

                foreach (Product p in pack.ItemsWithoutDiscount.Keys)
                {
                    int CountForDelete = (int)pack.ItemsWithoutDiscount[p];
                    var itemsToDelete = Inventory[p]
                    .Where(pck => !pck.IsDicounted)
                    .Take(CountForDelete)
                    .ToList();

                    foreach (var item in itemsToDelete)
                    {
                        Inventory[p].Remove(item);
                    }

                }
            }
        }




        public List<shopPackage> createTransportList(List<shopOrder> orders) //Обслужить заказ от магазина.
                                           //Т.е возвращаемый ордер это то, что будем убират из инвентори
        {
            //Формирует относительно сегодняшнего дня! Ошибка бизнеслогики
            List<shopPackage> newPackages = new List<shopPackage>();
            //Возвращать список новых заказов
            foreach (var order in orders)
            {
                shopPackage newPackage = new shopPackage(order.Name);
                foreach (Product p in order.ItemsWithoutDiscount.Keys)
                {

                    int CountPacks =(int)(order.ItemsWithoutDiscount[p] / p.Quantity); //Нужно обычных упаковок
                    int CountWholesalePacks = CountPacks / helper[p].PackageCount; //Нужно оптовых упаковок
                    int r = CountPacks - CountWholesalePacks * helper[p].PackageCount;
                    if (r>helper[p].PackageCount/2)
                    {
                        CountWholesalePacks += 1;
                    }
                    int HaveWholesalePacks = Inventory[p].Count(x => !x.IsDicounted);

                    if (HaveWholesalePacks>=CountWholesalePacks)
                    {
                        newPackage.AddProductWithoutDiscount(p,CountWholesalePacks);
                    }
                    else
                    {
                        newPackage.AddProductWithoutDiscount(p,HaveWholesalePacks);
                    }







                }

                foreach (Product p in order.ItemsWithDiscount.Keys)
                {

                    int CountPacks = (int)(order.ItemsWithDiscount[p] / p.Quantity); //Нужно обычных упаковок
                    int CountWholesalePacks = CountPacks / helper[p].PackageCount; //Нужно оптовых упаковок
                    int r = CountPacks - CountWholesalePacks * helper[p].PackageCount;
                    if (r > helper[p].PackageCount / 2)
                    {
                        CountWholesalePacks += 1;
                    }
                    int HaveWholesalePacks = Inventory[p].Count(x => x.IsDicounted);

                    if (HaveWholesalePacks >= CountWholesalePacks)
                    {
                        newPackage.AddProductWithDiscount(p,CountWholesalePacks);
                    }
                    else
                    {
                        newPackage.AddProductWithDiscount(p,HaveWholesalePacks);
                    }







                }
                newPackages.Add(newPackage);
            }
            return newPackages;
            
        }

        public List<WholesalePackage> DiscountPrices(double disc) //Создать скидки
        {
            List<WholesalePackage> discounted = new List<WholesalePackage>();
            foreach (Product p in Inventory.Keys)
            {
                foreach (WholesalePackage package in Inventory[p])
                {
                    if (package.WasteDay-_tempday<3 && !package.IsDicounted)
                    {
                        package.MakeDiscount(disc);
                        discounted.Add(package);
                    }
                }

            }
            return discounted;
        }

        public List<WholesalePackage> RemoveExpirated() //Удалить просроченные оптовые упаковки
        {

            List<WholesalePackage> deleted = new List<WholesalePackage> ();
            List<WholesalePackage> packsForDel = new List<WholesalePackage>();
            foreach (Product product in Inventory.Keys)
            {
                packsForDel.Clear();
                foreach (WholesalePackage p in Inventory[product])
                {
                    if (p.WasteDay == _tempday)
                    {
                        packsForDel.Add(p);
                        deleted.Add(p);
                    }
                }
                foreach (WholesalePackage p in packsForDel)
                {
                    Inventory[product].Remove(p);
                }
                
            }
            return deleted;
        }
    }
}
