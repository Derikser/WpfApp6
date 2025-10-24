using System;
using System.Data.Entity;
using System.Linq;
using System.Windows;
using System.Windows.Controls;
using WpfApp6;

namespace WpfApp6
{
    public partial class ProductsPage : Page
    {
        private PR_GITEntities _context;
        private int _currentProductId = 0;

        public ProductsPage()
        {
            InitializeComponent();
            _context = new PR_GITEntities();
            LoadProducts();
            LoadCategories();
        }

        private void LoadProducts()
        {
            ProductsGrid.ItemsSource = _context.Products
                .Include(p => p.Categories)
                .ToList();
        }

        private void LoadCategories()
        {
            var categories = _context.Categories.ToList();
            CmbCategory.ItemsSource = categories; // Исправлено: убраны лишние пробелы
            CmbCategory.DisplayMemberPath = "CategoryName";
            CmbCategory.SelectedValuePath = "CategoryID";
        }

        private void BtnSave_Click(object sender, RoutedEventArgs e)
        {
            if (string.IsNullOrWhiteSpace(TxtProductName.Text) ||
                CmbCategory.SelectedItem == null ||
                string.IsNullOrWhiteSpace(TxtPrice.Text) ||
                string.IsNullOrWhiteSpace(TxtQuantity.Text))
            {
                MessageBox.Show("Заполните все поля!");
                return;
            }

            // Валидация числовых полей
            if (!decimal.TryParse(TxtPrice.Text, out decimal price) || price < 0)
            {
                MessageBox.Show("Введите корректную цену!");
                return;
            }

            if (!int.TryParse(TxtQuantity.Text, out int quantity) || quantity < 0)
            {
                MessageBox.Show("Введите корректное количество!");
                return;
            }

            try
            {
                if (_currentProductId == 0)
                {
                    // Добавление нового товара
                    var product = new Products
                    {
                        Name = TxtProductName.Text,
                        CategoryID = (int)CmbCategory.SelectedValue,
                        Price = price,
                        Quantity = quantity
                    };
                    _context.Products.Add(product);
                }
                else
                {
                    // Обновление существующего товара
                    var product = _context.Products.Find(_currentProductId);
                    if (product != null)
                    {
                        product.Name = TxtProductName.Text;
                        product.CategoryID = (int)CmbCategory.SelectedValue;
                        product.Price = price;
                        product.Quantity = quantity;
                    }
                }

                _context.SaveChanges();
                ClearForm();
                LoadProducts();
                MessageBox.Show("Товар сохранен!");
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Ошибка: {ex.Message}");
            }
        }

        private void EditProduct_Click(object sender, RoutedEventArgs e)
        {
            var button = sender as Button;
            var product = button?.DataContext as Products; // Добавлена проверка на null

            if (product != null)
            {
                _currentProductId = product.ProductID;
                TxtProductName.Text = product.Name;
                CmbCategory.SelectedValue = product.CategoryID;
                TxtPrice.Text = product.Price.ToString();
                TxtQuantity.Text = product.Quantity.ToString();
            }
        }

        private void DeleteProduct_Click(object sender, RoutedEventArgs e)
        {
            var button = sender as Button;
            var product = button?.DataContext as Products; // Добавлена проверка на null

            if (product != null)
            {
                var result = MessageBox.Show($"Удалить товар '{product.Name}'?",
                                           "Подтверждение",
                                           MessageBoxButton.YesNo,
                                           MessageBoxImage.Question);

                if (result == MessageBoxResult.Yes)
                {
                    // Перезагружаем сущность из базы данных для избежания проблем с отслеживанием
                    var productToDelete = _context.Products.Find(product.ProductID);
                    if (productToDelete != null)
                    {
                        _context.Products.Remove(productToDelete);
                        _context.SaveChanges();
                        LoadProducts();
                        MessageBox.Show("Товар удален!");
                    }
                }
            }
        }

        private void ClearForm()
        {
            _currentProductId = 0;
            TxtProductName.Text = "";
            CmbCategory.SelectedIndex = -1;
            TxtPrice.Text = "";
            TxtQuantity.Text = "";
        }
    }
}