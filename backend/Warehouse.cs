namespace backend
{
    public class Warehouse
    {
        public int CurrentDay;
        private readonly List<SupplyPackage> _waitPackage = [];

        public readonly Dictionary<Product, List<WholesalePackage>> Inventory = new(new ProductComparer());

        public readonly Dictionary<Product, WholesalePackage> Helper = new(new ProductComparer());
        
        public Warehouse(int day, Dictionary<Product, List<WholesalePackage>> inventory)
        {
            Inventory = inventory;
            CurrentDay = day;
        }

        public Warehouse(int totalDays, int numProducts, string[] productList, int day)
        {
            List<Product> products = [];
            Dictionary<Product, List<WholesalePackage>> inv = new();
            RandomGenerator randomGenerator = new RandomGenerator(numProducts);


            for (int i = 0; i < numProducts; i++)
            {
                string productName = productList[i];
                double quantity = randomGenerator.NextDouble(10, 50);
                int expiryDays = randomGenerator.NextInt(5, totalDays);
                double price = randomGenerator.NextDouble(10, 100);
                int minPackages = randomGenerator.NextInt(10, 30);
                Product p = new Product(productName, quantity, expiryDays, price, minPackages);
                products.Add(p);
            }

            foreach (Product p in products)
            {
                int count = randomGenerator.NextInt(10, 20);
                List<WholesalePackage> packages = new();
                WholesalePackage package = new(p, count, p.ExpiryDays + day);
                Helper.Add(p, package);
                for (int i = 0; i < count; i++)
                {
                    packages.Add(package);
                }

                inv.Add(p, packages);
            }

            CurrentDay = 1;
            foreach (Product p in inv.Keys)
            {
                Inventory.Add(p, inv[p]);
            }
        }

        public SupplyOrder CreateSupplyOrder() // Создать заказ к поставщику
        {
            SupplyOrder order = new SupplyOrder(CurrentDay);
            foreach (Product product in Inventory.Keys)
            {
                if (Inventory[product].Count == 0)
                {
                    order.AddItem(product, product.MinWholesalePackages + 5);
                }
                else if (Inventory[product].Count < product.MinWholesalePackages)
                {
                    int temp = Inventory[product].Count;
                    int need = product.MinWholesalePackages;
                    int fororder = need - temp;
                    order.AddItem(product, fororder);
                }
                else if (Inventory[product].Count == product.MinWholesalePackages)
                {
                    int need = product.MinWholesalePackages;
                    order.AddItem(product, need + 5);
                }
            }

            return order;
        }

        public void AddPackage(SupplyPackage s) // Добавить посылку от поставщика в ожидаемые
        {
            _waitPackage.Add(s);
        }

        public void FullFill() // Пополнить свои запасы после отдачи заказа поставщиком
        {
            List<SupplyPackage> realisedSupplys = [];
            if (_waitPackage.Count == 0) return;
            
            foreach (SupplyPackage sp in _waitPackage)
            {
                if (sp.DeliveryDay != CurrentDay) continue;
                
                realisedSupplys.Add(sp);
                foreach (Product product in sp.Products.Keys)
                {
                    var tempList = sp.Products[product].ToList();
                    foreach (WholesalePackage package in tempList)
                    {
                        Inventory[product].Add(package);
                    }
                }
            }

            foreach (SupplyPackage pack in realisedSupplys)
            {
                _waitPackage.Remove(pack);
            }
        }


        public void MakeTransport(List<ShopPackage> packs)
        {
            foreach (ShopPackage pack in packs)
            {
                foreach (Product p in pack.ItemsWithDiscount.Keys)
                {
                    int countForDelete = pack.ItemsWithDiscount[p];
                    var itemsToDelete = Inventory[p]
                        .Where(pck => pck.IsDiscounted)
                        .Take(countForDelete)
                        .ToList();

                    foreach (var item in itemsToDelete)
                    {
                        Inventory[p].Remove(item);
                    }
                }

                foreach (Product p in pack.ItemsWithoutDiscount.Keys)
                {
                    int countForDelete = pack.ItemsWithoutDiscount[p];
                    var itemsToDelete = Inventory[p]
                        .Where(pck => !pck.IsDiscounted)
                        .Take(countForDelete)
                        .ToList();

                    foreach (var item in itemsToDelete)
                    {
                        Inventory[p].Remove(item);
                    }
                }
            }
        }


        public List<ShopPackage> CreateTransportList(List<ShopOrder> orders) // Обслужить заказ от магазина.
        {
            List<ShopPackage> newPackages = [];

            foreach (var order in orders)
            {
                ShopPackage newPackage = new ShopPackage(order.Name);
                foreach (Product p in order.ItemsWithoutDiscount.Keys)
                {
                    int countPacks = (int)(order.ItemsWithoutDiscount[p] / p.Quantity);
                    int countWholesalePacks = countPacks / Helper[p].PackageCount;
                    int r = countPacks - countWholesalePacks * Helper[p].PackageCount;
                    if (r > Helper[p].PackageCount / 2)
                    {
                        countWholesalePacks += 1;
                    }

                    int haveWholesalePacks = Inventory[p].Count(x => !x.IsDiscounted);

                    if (haveWholesalePacks >= countWholesalePacks)
                    {
                        newPackage.AddProductWithoutDiscount(p, countWholesalePacks);
                    }
                    else
                    {
                        newPackage.AddProductWithoutDiscount(p, haveWholesalePacks);
                    }
                }

                foreach (Product p in order.ItemsWithDiscount.Keys)
                {
                    int countPacks = (int)(order.ItemsWithDiscount[p] / p.Quantity);
                    int countWholesalePacks = countPacks / Helper[p].PackageCount;
                    int r = countPacks - countWholesalePacks * Helper[p].PackageCount;
                    if (r > Helper[p].PackageCount / 2)
                    {
                        countWholesalePacks += 1;
                    }

                    int haveWholesalePacks = Inventory[p].Count(x => x.IsDiscounted);

                    if (haveWholesalePacks >= countWholesalePacks)
                    {
                        newPackage.AddProductWithDiscount(p, countWholesalePacks);
                    }
                    else
                    {
                        newPackage.AddProductWithDiscount(p, haveWholesalePacks);
                    }
                }

                newPackages.Add(newPackage);
            }

            return newPackages;
        }

        public List<WholesalePackage> DiscountPrices(double disc)
        {
            List<WholesalePackage> discounted = new();
            foreach (Product p in Inventory.Keys)
            {
                foreach (WholesalePackage package in Inventory[p])
                {
                    if (package.WasteDay - CurrentDay < 3 && !package.IsDiscounted)
                    {
                        package.MakeDiscount(disc);
                        discounted.Add(package);
                    }
                }
            }

            return discounted;
        }

        public List<WholesalePackage> RemoveExpirated()
        {
            List<WholesalePackage> deleted = new();
            List<WholesalePackage> packsForDel = new();
            foreach (Product product in Inventory.Keys)
            {
                packsForDel.Clear();
                foreach (WholesalePackage p in Inventory[product])
                {
                    if (p.WasteDay == CurrentDay)
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