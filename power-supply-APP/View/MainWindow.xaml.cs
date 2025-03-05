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

        private TestPage _testPage; // Сохраняем ссылку на TestPage
        public SettingsPage _settingsPage;

        public MainWindow()
        {
            InitializeComponent();
            _testPage = new TestPage(); // Создаём один экземпляр
            _settingsPage = new SettingsPage(); // Создаём один экземпляр
            MainFrame.Navigate(_testPage); // Навигируем в него
        }

        public void NavigateToTestPage()
        {
            MainFrame.Navigate(_testPage); // Возвращаемся в сохранённый экземпляр
        }
        private void Window_Loaded(object sender, RoutedEventArgs e)
        {
            ApplyTheme();
        }

        private void ApplyTheme()
        {
            var viewModel = (MainViewModel)DataContext;
            var theme = (ResourceDictionary)Application.LoadComponent(new Uri(viewModel.CurrentTheme, UriKind.Relative));

            Application.Current.Resources.MergedDictionaries.Clear();
            Application.Current.Resources.MergedDictionaries.Add(theme);
        }

        private void SettingButton_Click(object sender, RoutedEventArgs e)
        {
            if (!(MainFrame.Content is SettingsPage))
            {
                MainFrame.Navigate(new Authorization());
            }
            else
            {
                MessageBox.Show("Настройки уже открыты! Не трогай меня, начальник!!!");
            }
        }

        public enum Theme
        {
            Light,
            Dark
        }

        private Theme currentTheme;

        public void SetTheme(Theme theme)
        {
            currentTheme = theme;

            // Загрузка ресурсов темы
            if (currentTheme == Theme.Light)
            {
                this.Resources.MergedDictionaries.Clear();
                this.Resources.MergedDictionaries.Add(new ResourceDictionary { Source = new Uri("LightTheme.xaml", UriKind.Relative) });

            }
            else
            {
                this.Resources.MergedDictionaries.Clear();
                this.Resources.MergedDictionaries.Add(new ResourceDictionary { Source = new Uri("DarkTheme.xaml", UriKind.Relative) });
            }
        }

        private void ToggleButton_Checked(object sender, RoutedEventArgs e)
        {
            SetTheme(Theme.Dark);
        }

        private void ToggleButton_Unchecked(object sender, RoutedEventArgs e)
        {
            SetTheme(Theme.Light);
        }
    }
}

