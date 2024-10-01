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
         private void LoginButton_Click(object sender, RoutedEventArgs e)
        {
            string username = UsernameTextBox.Text;
            string password = PasswordBox.Password;

            if (username == "admin" && password == "55555")
            {
                // Получаем доступ к главному окну
                MainWindow mainWindow = (MainWindow)Application.Current.MainWindow;
                // Переходим на TestPage
                mainWindow.MainFrame.Navigate(new TestPage());

            }
            else
            {
                MessageTextBlock.Text = "Это была фатальная ошибка";
            }
        }
    }
}
