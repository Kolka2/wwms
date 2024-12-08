using System.Windows;
using backend;

namespace MPProject
{
    /// <summary>
    /// Логика взаимодействия для MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        Settings settings = new Settings();
        public MainWindow()
        {
            InitializeComponent();
        }

        private void OnStartSimulationClick(object sender, RoutedEventArgs e)
        {
            int minProductsCountInOrder = settings._MinProductsCountInOrder; //Минимальное количество продуктов в заказе
            int totalDays = settings._TotalDays; //от 5 до 30
            int NumStores = settings._NumStores;
            int numProducts = settings._NumProducts;
            int MinQ = settings._MinQuantity; //Минимальное количество продукта для заказа
            int MaxQ = settings._MaxQuantity; //Максимальное количество продукта для заааза
            double Discount = settings._Discount; //скидка. 
            string filename = settings._Path;
            RandomGenerator r = new RandomGenerator(numProducts, minProductsCountInOrder, MinQ, MaxQ);
            //Разберемся с продуктами. Реализовать метод для создания начального набора продуктов на складе

            Dictionary<Product, List<WholesalePackage>> dict = new Dictionary<Product, List<WholesalePackage>>(new ProductComparer());
            //Есть еще один конструктор для Simulation s=new Simulation(filename,Discount,totalDays,NumStores,numProducts,minProductsCountInOrder,dict);
            //Задача реализовать создание на фронте словаря для передачи в этот конструктор. Далее он будет передан в Inventory класса Warehouse
            Simulation s = new Simulation(filename, Discount, totalDays, NumStores, numProducts, r);
            s.Run();
            MessageBox.Show("Симуляция прошла успешно!");
        }

        private void OnSettingsClick(object sender, RoutedEventArgs e)
        {
             settings = new Settings();
            settings.Show();
           
        }

        private void OnExitClick(object sender, RoutedEventArgs e)
        {

        }
    }
}
