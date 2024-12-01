namespace wwms
{
    internal class Program
    {
        static void Main(string[] args)
        {
            // Добавить рандом минимальное-максимальное количество товара на заказ
            // Добавить рандом минимальное-максимальное полезного сырья для товара
            // Прикрутить интерфейсы
            int minProductsCountInOrder = 5; // Минимальное количество продуктов в заказе
            int totalDays = 6; // от 5 до 30
            int NumStores = 1;
            int numProducts = 7;
            int minQ = 4000; // Минимальное количество продукта для заказа
            int maxQ = 7000; // Максимальное количество продукта для заказа
            double discount = 0.15; // скидка. 
            string filename = "logfile1.txt";
            RandomGenerator r = new(numProducts, minProductsCountInOrder, minQ, maxQ);
            
            // Разберемся с продуктами. Реализовать метод для создания начального набора продуктов на складе
            Dictionary<Product, List<WholesalePackage>> dict = new(new ProductComparer());
            
            // Есть еще один конструктор для Simulation s=new Simulation(filename,Discount,totalDays,NumStores,numProducts,minProductsCountInOrder,dict);
            // Задача реализовать создание на фронте словаря для передачи в этот конструктор. Далее он будет передан в Inventory класса Warehouse
            Simulation s = new Simulation(filename, discount, totalDays, NumStores, numProducts,
                minProductsCountInOrder, r);
            s.Run();
        }
    }
}