using power_supply_APP.View;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.InteropServices;
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
using System.Windows.Threading;


namespace power_supply_APP
{
    /// <summary>
    /// Логика взаимодействия для TestPage.xaml
    /// </summary>
    public partial class TestPage : Page
    {
        private DispatcherTimer timer;
        private int elapsedSeconds = 0;
        private bool[] isFirstState; // Массив для отслеживания состояния ячеек
        private int cellCount = 8; // Количество ячеек
        public List<double> Values { get; set; }
        public TestPage()
        {
            InitializeComponent();
           
            // Инициализация данных
            Values = new List<double> {3, 5, 7, 4, 6 };

            // Установка контекста данных для привязки
            DataContext = this;
        }




        private void Button_Cell_Click(object sender, RoutedEventArgs e)
        {
            // Скрыть все элементы SectionControl
            Section_Detail_1.Visibility = Visibility.Collapsed;
            Section_Detail_2.Visibility = Visibility.Collapsed;
            Section_Detail_3.Visibility = Visibility.Collapsed;
            Section_Detail_4.Visibility = Visibility.Collapsed;
            Section_Detail_5.Visibility = Visibility.Collapsed;
            Section_Detail_6.Visibility = Visibility.Collapsed;
            Section_Detail_7.Visibility = Visibility.Collapsed;
            Section_Detail_8.Visibility = Visibility.Collapsed;

            // Получить кнопку, вызвавшую событие
            Button clickedButton = sender as Button;
            if (clickedButton != null)
            {
                // Получить Tag кнопки
                string tag = clickedButton.Tag as string;

                // Показать соответствующий SectionControl
                switch (tag)
                {
                    case "Ячейка №1":
                        Section_Detail_1.Visibility = Visibility.Visible;
                        break;
                    case "Ячейка №2":
                        Section_Detail_2.Visibility = Visibility.Visible;
                        break;
                    case "Ячейка №3":
                        Section_Detail_3.Visibility = Visibility.Visible;
                        break;
                    case "Ячейка №4":
                        Section_Detail_4.Visibility = Visibility.Visible;
                        break;
                    case "Ячейка №5":
                        Section_Detail_5.Visibility = Visibility.Visible;
                        break;
                    case "Ячейка №6":
                        Section_Detail_6.Visibility = Visibility.Visible;
                        break;
                    case "Ячейка №7":
                        Section_Detail_7.Visibility = Visibility.Visible;
                        break;
                    case "Ячейка №8":
                        Section_Detail_8.Visibility = Visibility.Visible;
                        break;
                }

                // Обновить текст TextBlock
                if (!string.IsNullOrEmpty(tag))
                {
                    TextBlockDisplay.Text = tag; // Устанавливаем текст из Tag
                }
            }
        }
        private void Add_Button_Click(object sender, RoutedEventArgs e)
        {
            var button = sender as Button;
            int index = Convert.ToInt32(button.Tag); // Получаем индекс кнопки

            TestingSettingsWindow settingsWindow = new TestingSettingsWindow();
            if (settingsWindow.ShowDialog() == true) // Проверяем, было ли окно закрыто успешно
            {
                string inputText = settingsWindow.InputText; // Получаем текст из окна добавления
                string serialText = settingsWindow.SerialText;
                switch (index)
                {
                    case 1:
                        DeviceBlock1.Text = inputText;
                        SerialBlock1.Text = serialText;
                        break;
                    case 2:
                        DeviceBlock2.Text = inputText;
                        SerialBlock2.Text = serialText;
                        break;
                    case 3:
                        DeviceBlock3.Text = inputText;
                        SerialBlock3.Text = serialText;
                        break;
                    case 4:
                        DeviceBlock4.Text = inputText;
                        SerialBlock4.Text = serialText;
                        break;
                    case 5:
                        DeviceBlock5.Text = inputText;
                        SerialBlock5.Text = serialText;
                        break;
                    case 6:
                        DeviceBlock6.Text = inputText;
                        SerialBlock6.Text = serialText;
                        break;
                    case 7:
                        DeviceBlock7.Text = inputText;
                        SerialBlock7.Text = serialText;
                        break;
                    case 8:
                        DeviceBlock8.Text = inputText;
                        SerialBlock8.Text = serialText;
                        break;
                }
            }
        }

        private void Start_Test(object sender, RoutedEventArgs e)
        {
            // Получаем кнопку, которая была нажата
            Button clickedButton = sender as Button;
            string sectionName = clickedButton.Tag.ToString();

            // Находим соответствующий SectionControl по имени
            var sectionControl = FindName(sectionName) as SectionControl;

            if (sectionControl != null)
            {
                // Показываем InnerGrid1
                var innerGrid1 = sectionControl.FindName("InnerGrid1") as Grid;
                if (innerGrid1 != null)
                {
                    innerGrid1.Visibility = Visibility.Visible;
                }
                // Прячем InnerGrid2
                var innerGrid2 = sectionControl.FindName("InnerGrid2") as Grid;
                if (innerGrid2 != null)
                {
                    innerGrid2.Visibility = Visibility.Collapsed;
                }
            }

        }
        private void Stop_Test(object sender, RoutedEventArgs e)
        {
            // Получаем кнопку, которая была нажата
            Button clickedButton = sender as Button;
            string sectionName = clickedButton.Tag.ToString();

            // Находим соответствующий SectionControl по имени
            var sectionControl = FindName(sectionName) as SectionControl;

            if (sectionControl != null)
            {
                // Показываем InnerGrid2
                var innerStackPanel2 = sectionControl.FindName("InnerGrid2") as Grid;
                if (innerStackPanel2 != null)
                {
                    innerStackPanel2.Visibility = Visibility.Visible;
                }
                // Прячем InnerGrid1
                var innerGrid1 = sectionControl.FindName("InnerGrid1") as Grid;
                if (innerGrid1 != null)
                {
                    innerGrid1.Visibility = Visibility.Collapsed;
                }
            }
        }
    }
}
