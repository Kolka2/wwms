using System;
using System.IO;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Security.Policy;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace Simul
{
    internal class Statistic
    {

        public double totalCost=0; //Заработано
        public double totalLose=0; //Потрачено
        public Warehouse wh;
        public string file; //адрес файла для сохранения
     


        public Statistic(Warehouse warehouse,string f)
        {
            file = f;
            wh = warehouse;
          
        }

        //Вывод общей статистики
        public void WarehouseStatus(Warehouse wh)
        {
            using (StreamWriter sw = new StreamWriter(file, true))
            {
                sw.WriteLine($"День:{wh._tempday}");
                foreach (Product p in wh.Inventory.Keys)
                {
                    sw.WriteLine($"Продукт:{p}");
                    sw.WriteLine($"    Количество оптовых упаковок без скидки:{wh.Inventory[p].Count(x => !x.IsDicounted)}");
                    sw.WriteLine($"    Количество оптовых упаковок со скидкой:{wh.Inventory[p].Count(x => x.IsDicounted)}");
                    sw.WriteLine($"    Количество оптовых упаковок всего:{wh.Inventory[p].Count()}");


                }
                sw.Close();
            }
            
            
            Console.WriteLine($"День:{wh._tempday}");
            foreach (Product p in wh.Inventory.Keys)
            {
                Console.WriteLine($"Продукт:{p}");
                Console.WriteLine($"    Количество оптовых упаковок без скидки:{wh.Inventory[p].Count(x=>!x.IsDicounted)}");
                Console.WriteLine($"    Количество оптовых упаковок со скидкой:{wh.Inventory[p].Count(x => x.IsDicounted)}");
                Console.WriteLine($"    Количество оптовых упаковок всего:{wh.Inventory[p].Count()}");


            }
        }
        
        public void ChangeCostWithoutDiscount(List<shopPackage> packages)
        {
            foreach (shopPackage package in packages)
            {
                foreach (Product p in package.ItemsWithoutDiscount.Keys)
                {
                    totalCost += p.Price * package.ItemsWithoutDiscount[p] * wh.helper[p].PackageCount;//Цена продукта*кол-во оптовых пачек*кол-во продуктов в оптовой пачке
                }

            };
        }

        //Подумать как передать
        public void ChangeCostWithDiscount(List<shopPackage> packages,double discount)
        {
            foreach (shopPackage package in packages)
            {
                foreach (Product p in package.ItemsWithDiscount.Keys)
                {
                    
                    totalCost += (p.Price - p.Price * discount) * package.ItemsWithDiscount[p] * wh.helper[p].PackageCount;//Цена продукта*кол-во оптовых пачек*кол-во продуктов в оптовой пачке

                }

            };
        }

        public void ChangeLostDeleted(List<WholesalePackage> deleted) 
        {
            //Сначала теряем в результате Discount, а затем еще и оставшееся. Учесть
           foreach (WholesalePackage package in deleted)
            {
                totalLose += package.DiscountPrice * package.PackageCount;
            }
        }

        public void ChangeLostDiscounted(List<WholesalePackage> discounted)
        {
            foreach (WholesalePackage package in discounted)
            {
                totalLose += (package.Price - package.DiscountPrice) * package.PackageCount;
            }

        }


        //Перевозки на следующий день
        public void ForShopPacks(List<shopPackage> packages)
        {
            using (StreamWriter sw = new StreamWriter(file, true))
            {
                sw.WriteLine($"Перевозки на {wh._tempday + 1}");
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

        public void ForShopOrders(List<shopOrder> orders)
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
            Console.WriteLine($"Всего прибыли:{totalCost}");
            Console.WriteLine($"Всего убытков:{totalLose}");
            using (StreamWriter wr = new StreamWriter(file, true))
            {
                wr.WriteLine($"Всего прибыли:{totalCost}");
                wr.WriteLine($"Всего убытков:{totalLose}");
            }
         
        }
    }
}
