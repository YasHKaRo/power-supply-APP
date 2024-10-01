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
using System.Windows.Threading;

namespace power_supply_APP
{
    /// <summary>
    /// Логика взаимодействия для TestPage.xaml
    /// </summary>
    public partial class TestPage : Page
    {
        private DispatcherTimer timer;
        private DispatcherTimer basicTimer;
        private bool[] isFirstState; // Массив для отслеживания состояния ячеек
        private int cellCount = 8; // Количество ячеек
        public TestPage()
        {
            InitializeComponent();
            isFirstState = new bool[cellCount]; // Инициализация массива состояний
           // Показать начальные состояния при запуске
        }

        private void StandartTimer()
        {
            basicTimer = new DispatcherTimer();
            basicTimer.Interval = TimeSpan.FromSeconds(1);
            basicTimer.Tick += Basic_Timer_Tick;
            basicTimer.Start();
        }
        private void  Basic_Timer_Tick(object sender, EventArgs e)
        {
            // Обновить текст таймера
            TimerTextFirst.Text = DateTime.Now.ToString("HH:mm:ss");
        }
        

    }
}
