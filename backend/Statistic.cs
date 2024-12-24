namespace backend
{
    public class Statistic
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

                sw.Close();
            }
        }

        public string AllStat()
        {
	        var resultString = $"Всего прибыли: {Math.Round(_totalCost, 2)}\nВсего убытков: {Math.Round(_totalLose, 2)}"; 
            using (StreamWriter wr = new(_outputFile, true))
            {
                wr.WriteLine(resultString);
            }
            return resultString;
        }
    }
}
