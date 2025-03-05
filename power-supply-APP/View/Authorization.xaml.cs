using power_supply_APP.Model;
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
    /// Логика взаимодействия для Authorization.xaml
    /// </summary>
    public partial class Authorization : Page
    {
        public Authorization()
        {
            InitializeComponent();
        }
        private ConfigManager configManager = new ConfigManager(); // Загружаем конфиг

        private void LoginButton_Click(object sender, RoutedEventArgs e)
        {
            string username = UsernameTextBox.Text;
            string password = PasswordBox.Password;

            // Проверяем наличие пользователя и правильность пароля через ConfigManager
            if (configManager.Users.TryGetValue(username, out string storedPassword) && storedPassword == password)
            {
                // Получаем доступ к главному окну
                MainWindow mainWindow = (MainWindow)Application.Current.MainWindow;

                if (mainWindow != null)
                {
                    // Переходим на SettingsPage
                    mainWindow.MainFrame.Navigate(mainWindow._settingsPage);
                }
            }
            else
            {
                MessageTextBlock.Text = "Это была фатальная ошибка.";
            }
        }
    }
}