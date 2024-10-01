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
            // Создать таймер для обновления TextBlock каждую секунду
            timer = new DispatcherTimer();
            timer.Interval = TimeSpan.FromSeconds(1);
            timer.Tick += Timer_Tick;
            timer.Start();
        }
        private void Timer_Tick(object sender, EventArgs e)
        {
            // Увеличить количество секунд на 1
            elapsedSeconds++;

            // Обновить значение TextBlock
            timeTextBlock.Text = TimeSpan.FromSeconds(elapsedSeconds).ToString("hh\\:mm\\:ss");
        }

        private void MyChart_Loaded(object sender, RoutedEventArgs e)
        {

        }

        private void Button_Cell_Click(object sender, RoutedEventArgs e)
        {
            // Получаем нажатую кнопку
            Button clickedButton = sender as Button;
            if (clickedButton != null)
            {
                // Изменяем текст TextBlock на основе значения Tag кнопки
                DynamicTextBlock.Text = clickedButton.Tag.ToString();
            }
        }
    }
}
