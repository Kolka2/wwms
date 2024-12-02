namespace wwms
{
    internal class Statistic
    {
        private double _totalCost;
        private double _totalLose;
        private readonly Warehouse _wh;
        private readonly string _outputFile;

        public Statistic(Warehouse warehouse, string f)
        {
            _outputFile = f;
            _wh = warehouse;
        }

        // Вывод общей статистики
        public void WarehouseStatus(Warehouse wh)
        {
            using (StreamWriter sw = new(_outputFile, true))
            {
                sw.WriteLine($"День:{wh.CurrentDay}");
                foreach (Product p in wh.Inventory.Keys)
                {
                    sw.WriteLine($"Продукт:{p}");
                    sw.WriteLine(
                        $"    Количество оптовых упаковок без скидки:{wh.Inventory[p].Count(x => !x.IsDiscounted)}");
                    sw.WriteLine(
                        $"    Количество оптовых упаковок со скидкой:{wh.Inventory[p].Count(x => x.IsDiscounted)}");
                    sw.WriteLine($"    Количество оптовых упаковок всего:{wh.Inventory[p].Count}");
                }

                sw.Close();
            }


            Console.WriteLine($"День:{wh.CurrentDay}");
            foreach (Product p in wh.Inventory.Keys)
            {
                Console.WriteLine($"Продукт:{p}");
                Console.WriteLine(
                    $"    Количество оптовых упаковок без скидки:{wh.Inventory[p].Count(x => !x.IsDiscounted)}");
                Console.WriteLine(
                    $"    Количество оптовых упаковок со скидкой:{wh.Inventory[p].Count(x => x.IsDiscounted)}");
                Console.WriteLine($"    Количество оптовых упаковок всего:{wh.Inventory[p].Count}");
            }
        }

        public void ChangeCostWithoutDiscount(List<ShopPackage> packages)
        {
            foreach (ShopPackage package in packages)
            {
                foreach (Product p in package.ItemsWithoutDiscount.Keys)
                {
                    _totalCost += p.Price * package.ItemsWithoutDiscount[p] * _wh.Helper[p].PackageCount;
                }
            }

            ;
        }

        public void ChangeCostWithDiscount(List<ShopPackage> packages, double discount)
        {
            foreach (ShopPackage package in packages)
            {
                foreach (Product p in package.ItemsWithDiscount.Keys)
                {
                    _totalCost += (p.Price - p.Price * discount) * package.ItemsWithDiscount[p] * _wh.Helper[p].PackageCount;
                }
            }
        }

        public void ChangeLostDeleted(List<WholesalePackage> deleted)
        {
            foreach (WholesalePackage package in deleted)
            {
                _totalLose += package.DiscountPrice * package.PackageCount;
            }
        }

        public void ChangeLostDiscounted(List<WholesalePackage> discounted)
        {
            foreach (WholesalePackage package in discounted)
            {
                _totalLose += (package.Price - package.DiscountPrice) * package.PackageCount;
            }
        }


        public void ForShopPacks(List<ShopPackage> packages)
        {
            using (StreamWriter sw = new(_outputFile, true))
            {
                sw.WriteLine($"Перевозки на {_wh.CurrentDay + 1}");
                foreach (var package in packages)
                {
                    sw.WriteLine("----------");
                    sw.WriteLine(package.Name);
                    sw.WriteLine("Оптовые упаковки со скидкой");
                    foreach (Product p in package.ItemsWithDiscount.Keys)
                    {
                        sw.WriteLine($"Продукт:{p} Кол-во оптовых упаковок:{package.ItemsWithDiscount[p]}");
                    }

                    sw.WriteLine("Оптовые оптовых упаковки без скидки");
                    foreach (Product p in package.ItemsWithoutDiscount.Keys)
                    {
                        sw.WriteLine($"Продукт:{p} Кол-во упаковок:{package.ItemsWithoutDiscount[p]}");
                    }

                    sw.WriteLine("------");
                }

                foreach (var package in packages)
                {
                    Console.WriteLine("----------");
                    Console.WriteLine(package.Name);
                    Console.WriteLine("Оптовые упаковки со скидкой");
                    foreach (Product p in package.ItemsWithDiscount.Keys)
                    {
                        Console.WriteLine($"Продукт:{p} Кол-во оптовых упаковок:{package.ItemsWithDiscount[p]}");
                    }

                    Console.WriteLine("Оптовые оптовых упаковки без скидки");
                    foreach (Product p in package.ItemsWithoutDiscount.Keys)
                    {
                        Console.WriteLine($"Продукт:{p} Кол-во упаковок:{package.ItemsWithoutDiscount[p]}");
                    }

                    Console.WriteLine("------");
                }

                sw.Close();
            }
        }

        public void ForShopOrders(List<ShopOrder> orders)
        {
            foreach (var order in orders)
            {
                Console.WriteLine(order.Name);
                Console.WriteLine();
            }
        }

        public void AllStat()
        {
            Console.WriteLine("Конец");
            Console.WriteLine($"Всего прибыли:{_totalCost}");
            Console.WriteLine($"Всего убытков:{_totalLose}");
            using (StreamWriter wr = new(_outputFile, true))
            {
                wr.WriteLine($"Всего прибыли:{_totalCost}");
                wr.WriteLine($"Всего убытков:{_totalLose}");
            }
        }
    }
}