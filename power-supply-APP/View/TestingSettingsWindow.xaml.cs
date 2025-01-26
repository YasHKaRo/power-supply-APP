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
using System.Windows.Shapes;

namespace power_supply_APP
{
    /// <summary>
    /// Логика взаимодействия для TestingSettingsWindow.xaml
    /// </summary>
    public partial class TestingSettingsWindow : Window
    {
        public string InputText { get; private set; }
        public string SerialText { get; private set; }
        public TestingSettingsWindow()
        {
            InitializeComponent();
        }
        private void TestingOkButton_Click(object sender, RoutedEventArgs e)
        {
            InputText = DeviceBox.Text;
            SerialText = SerialNumberBox.Text;
            this.DialogResult = true; // Указываем, что окно закрывается успешно
            this.Close();
        }
        private void TestingCloseButton_Click(Object sender, RoutedEventArgs e)
        {
            this.DialogResult = true; // Указываем, что окно закрывается успешно
            this.Close();
        }
    }
}
