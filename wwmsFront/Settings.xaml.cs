using Microsoft.Win32;
using System.IO;
using System.Windows;
using System.Windows.Controls;

namespace MPProject
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
        public void LoadSettings()
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

        public bool ValidateMinProductsCountInOrder()
        {
            int MinProduct;
            int CountProduct;
            try
            {
                MinProduct = int.Parse(MinProductsCountInOrder.Text);
            }
            catch
            {
                MessageBox.Show("Неверные символы в минимальном количестве продуктов в заказе");
                return false;

            }

            try
            {
                CountProduct = int.Parse(NumProducts.Text);
            }
            catch
            {
                MessageBox.Show("Неверные символы в количестве продуктов всего");
                return false;
            }

        
           if (MinProduct<0)
           {
                MessageBox.Show("Минимальное количество продуктов в заказе не может быть меньше 0!");
                return false;
           }

            else if (CountProduct<MinProduct)
            {
                MessageBox.Show("Минимальное количество продуктов в заказе не может быть больше чем количество продуктов всего!");
                return false;
            }

            
            _MinProductsCountInOrder = MinProduct;
            return true;

        }
        public bool ValidateTotalDays()
        {
            try
            {
                int AllDays = int.Parse(TotalDays.Text);
                if (AllDays >= 12 && AllDays <= 30)
                {
                    _TotalDays = AllDays;
                    return true;
                }
                else
                {
                    MessageBox.Show("Неверное количество дней!");
                    return false; 
                }

            }
            catch
            {
                MessageBox.Show("Неверные символы в количестве дней!");
                return false;
                
            }
        }
        public bool ValidateNumStores()
        {
            try
            {
                int Num = int.Parse(NumStores.Text);
                if (Num >= 3 && Num <= 9)
                {
                    _NumStores = Num;
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

        public bool ValidateNumProducts()
        {
            try
            {
                int NumP = int.Parse(NumProducts.Text);
                if (NumP >= 12 && NumP <= 20)
                {
                    _NumProducts = NumP;
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

        public bool ValidateMinQuantity()
        {
            int MinQuantity;
            int MaxQuantity;
            try
            {
                MinQuantity = int.Parse(MinQ.Text);
            }
            catch
            {
                MessageBox.Show("Неверные символы в минимальном количестве продукта в заказе");
                return false;

            }

            try
            {
                MaxQuantity= int.Parse(MaxQ.Text);
            }
            catch
            {
                MessageBox.Show("Неверные символы в максимальном количестве продукта всего");
                return false;
            }


            if (MinQuantity < 0)
            {
                MessageBox.Show("Минимальное количество продукта в заказе не может быть меньше 0!");
                return false;
            }

            else if (MinQuantity > MaxQuantity)
            {
                MessageBox.Show("Минимальное количество продуктов в заказе не может быть больше чем количество продуктов всего!");
                return false;
            }

            _MinQuantity = MinQuantity;

            return true;


        }

        public bool ValidateMaxQuantity()
        {
            try
            {
                int MaxQuantity = int.Parse(MaxQ.Text);
                if (MaxQuantity > 0)
                {
                   _MaxQuantity = MaxQuantity;
                    return true;
                }
                else
                {
                    MessageBox.Show("Неверное максимальное количество продуктов для заказа!");
                    return false;
                }

            }
            catch
            {
                MessageBox.Show("Неверные символы в максимальном количестве продуктов для заказа!");
                return false;

            }
        }

        public bool ValidateDiscount()
        {
            try
            {
                double Disc = double.Parse(Discount.Text);
                if (Disc >= 0.1 && Disc <= 0.99)
                {
                    _Discount = Disc;
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

       
        public bool ValidatePath(string filePath)
        {
            try
            {
                // Проверяем существование файла
                if (!File.Exists(filePath))
                {
                    MessageBox.Show("Файл не существует.");
                    return false;
                }

                // Проверяем, доступен ли файл для записи
                using (FileStream fileStream = new(filePath, FileMode.Open, FileAccess.Write))
                {
                    // Если доступ к файлу для записи разрешен, то все в порядке
                    _Path = filePath;
                    return true;
                }
            }
            catch (Exception ex)
            {
                // Если произошла ошибка (например, файл занят или недоступен), выводим сообщение
                MessageBox.Show($"Ошибка при проверке файла: {ex.Message}");
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
