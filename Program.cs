namespace wwms
{
    internal class Program
    {
        static void Main(string[] args)
        {
            int minProductsCountInOrder = 5;
            int totalDays = 6;
            int NumStores = 1;
            int numProducts = 7;
            int minQ = 4000;
            int maxQ = 7000;
            double discount = 0.15;
            string filename = "logfile1.txt";
            RandomGenerator r = new(numProducts, minProductsCountInOrder, minQ, maxQ);
            
            Dictionary<Product, List<WholesalePackage>> dict = new(new ProductComparer());
            
            // Есть еще один конструктор для Simulation s=new Simulation(filename,Discount,totalDays,NumStores,numProducts,minProductsCountInOrder,dict);
            // Задача реализовать создание на фронте словаря для передачи в этот конструктор. Далее он будет передан в Inventory класса Warehouse
            Simulation s = new Simulation(filename, discount, totalDays, NumStores, numProducts, r);
            s.Run();
        }
    }
}