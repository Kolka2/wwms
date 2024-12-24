using System.Windows;
using System.Windows.Controls;
using Microsoft.Win32;

namespace frontend
{
    /// <summary>
    /// Логика взаимодействия для Settings.xaml
    /// </summary>
    public partial class Settings : Window
    {
        public int _MinProductsCountInOrder;
        public int _TotalDays;
        public int _NumStores;
        public int _NumProducts;
        public int _MinQuantity;
        public int _MaxQuantity;
        public double _Discount;
        public string _Path;

        public Settings()
        {
            InitializeComponent();
            LoadSettings();
        }

        private void LoadSettings()
        {
            _MinProductsCountInOrder = int.Parse(MinProductsCountInOrder.Text);
            _TotalDays = int.Parse(TotalDays.Text);
            _NumStores = int.Parse(NumStores.Text);
            _NumProducts = int.Parse(NumProducts.Text);
            _MinQuantity = int.Parse(MinQ.Text);
            _MaxQuantity = int.Parse(MaxQ.Text);
            _Discount = double.Parse(Discount.Text);
            _Path = "logfile1.txt";
        }

        private bool ValidateMinProductsCountInOrder()
        {
            int minProduct;
            int countProduct;
            try
            {
                minProduct = int.Parse(MinProductsCountInOrder.Text);
            }
            catch
            {
                MessageBox.Show("Неверные символы в минимальном количестве продуктов в заказе");
                return false;
            }

            try
            {
                countProduct = int.Parse(NumProducts.Text);
            }
            catch
            {
                MessageBox.Show("Неверные символы в количестве продуктов всего");
                return false;
            }

            if (minProduct < 0)
            {
                MessageBox.Show("Минимальное количество продуктов в заказе не может быть меньше 0!");
                return false;
            }

            if (countProduct < minProduct)
            {
                MessageBox.Show(
                    "Минимальное количество продуктов в заказе не может быть больше чем количество продуктов всего!");
                return false;
            }


            _MinProductsCountInOrder = minProduct;
            return true;
        }

        private bool ValidateTotalDays()
        {
            try
            {
                int allDays = int.Parse(TotalDays.Text);
                if (allDays >= 12 && allDays <= 30)
                {
                    _TotalDays = allDays;
                    return true;
                }

                MessageBox.Show("Неверное количество дней!");
                return false;
            }
            catch
            {
                MessageBox.Show("Неверные символы в количестве дней!");
                return false;
            }
        }

        private bool ValidateNumStores()
        {
            try
            {
                int num = int.Parse(NumStores.Text);
                if (num >= 3 && num <= 9)
                {
                    _NumStores = num;
                    return true;
                }
                else
                {
                    MessageBox.Show("Неверное количество магазинов!");
                    return false;
                }
            }
            catch
            {
                MessageBox.Show("Неверные символы в количестве магазинов!");
                return false;
            }
        }

        private bool ValidateNumProducts()
        {
            try
            {
                int numP = int.Parse(NumProducts.Text);
                if (numP >= 12 && numP <= 20)
                {
                    _NumProducts = numP;
                    return true;
                }
                else
                {
                    MessageBox.Show("Неверное количество продуктов!");
                    return false;
                }
            }
            catch
            {
                MessageBox.Show("Неверные символы в количестве продуктов!");
                return false;
            }
        }

        private bool ValidateMinQuantity()
        {
            int minQuantity;
            int maxQuantity;
            try
            {
                minQuantity = int.Parse(MinQ.Text);
            }
            catch
            {
                MessageBox.Show("Неверные символы в минимальном количестве продукта в заказе");
                return false;
            }

            try
            {
                maxQuantity = int.Parse(MaxQ.Text);
            }
            catch
            {
                MessageBox.Show("Неверные символы в максимальном количестве продукта всего");
                return false;
            }


            if (minQuantity < 0)
            {
                MessageBox.Show("Минимальное количество продукта в заказе не может быть меньше 0!");
                return false;
            }

            if (minQuantity > maxQuantity)
            {
                MessageBox.Show(
                    "Минимальное количество продуктов в заказе не может быть больше чем количество продуктов всего!");
                return false;
            }

            _MinQuantity = minQuantity;

            return true;
        }

        private bool ValidateDiscount()
        {
            try
            {
                double disc = double.Parse(Discount.Text);
                if (disc >= 0.1 && disc <= 0.99)
                {
                    _Discount = disc;
                    return true;
                }
                else
                {
                    MessageBox.Show("Неверная скидка!");
                    return false;
                }
            }
            catch
            {
                MessageBox.Show("Неверные символы в скидке !");
                return false;
            }
        }

        private bool ValidateInput()
        {
            if (!ValidateMinProductsCountInOrder())
            {
                return false;
            }
            else if (!ValidateTotalDays())
            {
                return false;
            }
            else if (!ValidateNumStores())
            {
                return false;
            }
            else if (!ValidateNumProducts())
            {
                return false;
            }
            else if (!ValidateMinQuantity())
            {
                return false;
            }
            else if (!ValidateDiscount())
            {
                return false;
            }

            MessageBox.Show("Все ок");
            return true;
        }

        private void OnSaveClick(object sender, RoutedEventArgs e)
        {
            if (ValidateInput())
            {
                this.Hide();
            }
        }

        private void OnCancelClick(object sender, RoutedEventArgs e)
        {
            this.Hide();
        }

        private void OnBrowseFileClick(object sender, RoutedEventArgs e)
        {
            OpenFileDialog ofd = new() { Filter = "Text Files (*.txt)|*.txt" };
            if (ofd.ShowDialog() == true)
            {
                Filename.Text = ofd.FileName;
                _Path = Filename.Text;
            }
        }

        private void MinQ_TextChanged(object sender, TextChangedEventArgs e)
        {
        }

        private void MaxQ_TextChanged(object sender, TextChangedEventArgs e)
        {
        }
    }
}