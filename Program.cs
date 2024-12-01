using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Simul
{
    internal class Program
    {
        static void Main(string[] args)
        {

            //Добавить рандом минимальное-максимальное количество товара на заказ
            //Добавить рандом минимальное-максимальное полезного сырья для товара
            //Прикрутить интерфейсы
            int minProductsCountInOrder = 5; //Минимальное количество продуктов в заказе
            int totalDays =6; //от 5 до 30
            int NumStores = 1;
            int numProducts = 7;
            int MinQ = 4000; //Минимальное количество продукта для заказа
            int MaxQ = 7000; //Максимальное количество продукта для заааза
            double Discount = 0.15; //скидка. 
            string filename = "logfile1.txt";
            RandomGenerator r = new RandomGenerator(numProducts,minProductsCountInOrder,MinQ,MaxQ);
            //Разберемся с продуктами. Реализовать метод для создания начального набора продуктов на складе
            
            Dictionary<Product,List<WholesalePackage>> dict  = new Dictionary<Product, List<WholesalePackage>>(new ProductComparer());
            //Есть еще один конструктор для Simulation s=new Simulation(filename,Discount,totalDays,NumStores,numProducts,minProductsCountInOrder,dict);
            //Задача реализовать создание на фронте словаря для передачи в этот конструктор. Далее он будет передан в Inventory класса Warehouse
            Simulation s = new Simulation(filename,Discount,totalDays,NumStores, numProducts, minProductsCountInOrder,r);
            s.Run();
        }
    }
}
