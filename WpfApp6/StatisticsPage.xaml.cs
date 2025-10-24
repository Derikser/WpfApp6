using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;

namespace WpfApp6
{
    /// <summary>
    /// Логика взаимодействия для StatisticsPage.xaml
    /// </summary>
    public partial class StatisticsPage : Page
    {
        private PR_GITEntities _context;

        public StatisticsPage()
        {
            InitializeComponent();
            _context = new PR_GITEntities();
            LoadStatistics();
        }

        private void LoadStatistics()
        {
            // Общее количество товаров
            int totalProducts = _context.Products.Sum(p => p.Quantity);

            // Общая стоимость товаров
            decimal totalValue = _context.Products.Sum(p => p.Price * p.Quantity);

            // Средняя цена по категориям
            var avgPriceByCategory = _context.Products
                .GroupBy(p => p.Categories.CategoryName)
                .Select(g => new
                {
                    Category = g.Key,
                    AvgPrice = g.Average(p => p.Price)
                })
                .ToList();

            // Отображение статистики
            TxtTotalProducts.Text = $"Общее количество товаров: {totalProducts}";
            TxtTotalValue.Text = $"Общая стоимость товаров: {totalValue:C}";

            string avgPriceText = "Средняя цена по категориям:\n";
            foreach (var category in avgPriceByCategory)
            {
                avgPriceText += $"{category.Category}: {category.AvgPrice:C}\n";
            }
            TxtAveragePrice.Text = avgPriceText;
        }
    }
}
