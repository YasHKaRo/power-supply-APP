using System;
using System.Collections.Generic;
using System.Globalization;
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

namespace power_supply_APP
{
    /// <summary>
    /// Логика взаимодействия для Page1.xaml
    /// </summary>
    public partial class SettingsPage : Page
    {
        public SettingsPage()
        {
            InitializeComponent();
            LoadData();
        }
        private void BackButton_Click(object sender, RoutedEventArgs e)
        {
            // Закрываем текущее окно настроек
            NavigationService.GoBack();
        }
        public class RelayData
        {
            public string RelayName { get; set; }
            public int Col1 { get; set; }
            public int Col2 { get; set; }
            public int Col3 { get; set; }
            public int Col4 { get; set; }
            public int Col5 { get; set; }
            public int Col6 { get; set; }
            public int Col7 { get; set; }
            public int Col8 { get; set; }
        }
        private void LoadData()
        {
            var relayDataList = new List<RelayData>
            {
                new RelayData { RelayName = "Реле 90 В", Col1 = 0, Col2 = 0, Col3 = 0, Col4 = 0, Col5 = 0, Col6 = 0, Col7 = 0, Col8 = 0 },
                new RelayData { RelayName = "Реле 260 В", Col1 = 0, Col2 = 0, Col3 = 0, Col4 = 0, Col5 = 0, Col6 = 0, Col7 = 0, Col8 = 0 }
            };

            RelayDataGrid.ItemsSource = relayDataList;
        }
        private void OnChangeButtonClick(object sender, RoutedEventArgs e)
        {
            Button button = sender as Button;
            if (button != null)
            {
                string textBoxName = button.Tag?.ToString();
                TextBox textBox = (TextBox)this.FindName(textBoxName);

                if (textBox != null)
                {
                    // Заменяем запятую на точку для корректного парсинга
                    string input = textBox.Text.Replace(',', '.');

                    if (double.TryParse(input, NumberStyles.Any, CultureInfo.InvariantCulture, out double value))
                    {
                        if (button.Content.ToString() == "-")
                        {
                            value -= 0.5;
                        }
                        else if (button.Content.ToString() == "+")
                        {
                            value += 0.5;
                        }
                        if (button.Content.ToString() == "-0.001")
                        {
                            value -= 0.001;
                        }
                        else if (button.Content.ToString() == "+0.001")
                        {
                            value += 0.001;
                        }
                        textBox.Text = value.ToString(CultureInfo.InvariantCulture);
                    }
                    else
                    {
                        MessageBox.Show("Пожалуйста, введите корректное значение.");
                        textBox.Text = "0";
                    }
                }
            }
        }
    }
}
