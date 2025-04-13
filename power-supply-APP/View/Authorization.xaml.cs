using power_supply_APP.Model;
using power_supply_APP.ViewModel;
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

            // Проверяем введённые данные и устанавливаем роль
            if (configManager.Users.TryGetValue("admin", out string adminPassword) && adminPassword == password)
            {
                // Администратор
                AuthManager.CurrentUserRole = UserRole.Admin;
            }
            else if (configManager.Users.TryGetValue("user", out string userPassword) && userPassword == password)
            {
                // Обычный пользователь
                AuthManager.CurrentUserRole = UserRole.User;
            }
            else
            {
                MessageTextBlock.Text = "Неверное имя пользователя или пароль.";
                return;
            }

            // Получаем доступ к главному окну
            MainWindow mainWindow = Application.Current.MainWindow as MainWindow;
            if (mainWindow != null)
            {
                // Переходим на SettingsPage (используем уже созданный экземпляр)
                mainWindow.MainFrame.Navigate(mainWindow._settingsPage);
            }
        }
    }
}