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

namespace power_supply_APP
{
    /// <summary>
    /// Логика взаимодействия для MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        public MainWindow()
        {
            InitializeComponent();
            MainFrame.Navigate(new Authorization()); // Устанавливаем начальную страницу
        }
        private void SettingButton_Click(object sender, RoutedEventArgs e)
        {
            // Проверка, на какой странице мы находимся. Если авторизация, то мы спокойно переходим на страницу с настройками.
            // Иначе же кнопка будет недействительна. Так будет открываться только 1 окно
            if (MainFrame.Content is Authorization){ MainFrame.Navigate(new SettingsPage());}
            else {MessageBox.Show("Настройки уже открыты! Не трогай меня, начальник!!!");}
        }

    }
}

