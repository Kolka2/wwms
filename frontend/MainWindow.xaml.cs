using System.Windows;
using backend;

namespace frontend
{
    /// <summary>
    /// Логика взаимодействия для MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        private Settings _settings;

        public MainWindow()
        {
            InitializeComponent();
            _settings = new();
        }

        private void OnStartSimulationClick(object sender, RoutedEventArgs e)
        {
            int minProductsCountInOrder = _settings._MinProductsCountInOrder;
            int totalDays = _settings._TotalDays;
            int numStores = _settings._NumStores;
            int numProducts = _settings._NumProducts;
            int minQ = _settings._MinQuantity;
            int maxQ = _settings._MaxQuantity;
            double discount = _settings._Discount;
            string filename = _settings._Path;
            RandomGenerator r = new(numProducts, minProductsCountInOrder, minQ, maxQ);

            Simulation s = new(filename, discount, totalDays, numStores, numProducts, r);
            s.Run();
            MessageBox.Show($"Симуляция прошла успешно!\n\n{s.Stats()}", "Результаты");
        }

        private void OnSettingsClick(object sender, RoutedEventArgs e)
        {
            _settings.Show();
            _settings.Activate();
        }

        private void OnExitClick(object sender, RoutedEventArgs e)
        {
            _settings.Close();
            Application.Current.Shutdown();
        }
    }
}