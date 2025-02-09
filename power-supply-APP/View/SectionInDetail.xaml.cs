using LiveCharts.Wpf;
using LiveCharts;
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
using System.Threading;
using System.Diagnostics;

namespace power_supply_APP.View
{
    /// <summary>
    /// Логика взаимодействия для SectionInDetail.xaml
    /// </summary>
    public partial class SectionInDetail : UserControl
    {
        private Random random = new Random();
        private CancellationTokenSource cancellationTokenSource;
        private SectionControl linkedSection; // Ссылка на SectionControl
        public event EventHandler<bool> TestStateChanged; // true - старт, false - стоп


        public SectionInDetail()
        {
            InitializeComponent();
            InitializeCharts();
            
        }
        // Метод для привязки SectionControl
        public void LinkSectionControl(SectionControl section)
        {
            linkedSection = section;
            Debug.WriteLine("SectionControl успешно привязан!");
        }
        private void InitializeCharts()
        {
            SetupChart(Current_Chart_En, "Данные I_En");
            SetupChart(Current_Chart_Ihh, "Данные I_Ihh");
            SetupChart(Current_Chart_Iprot, "Данные I_Iprot");
            SetupChart(Current_Chart_Ikz, "Данные I_Ikz");
            SetupChart(Current_Chart_Upuls, "Данные I_Upuls");
            SetupChart(Voltage_Chart_En, "Данные U_En");
            SetupChart(Voltage_Chart_Ihh, "Данные U_Ihh");
            SetupChart(Voltage_Chart_Iprot, "Данные U_Iprot");
            SetupChart(Voltage_Chart_Ikz, "Данные U_Ikz");
            SetupChart(Voltage_Chart_Upuls, "Данные U_Upuls");
        }

        private void SetupChart(CartesianChart chart, string seriesTitle)
        {
            chart.Series = new SeriesCollection
        {
            new LineSeries
            {
                Values = new ChartValues<double>(),
                Title = seriesTitle
            }
        };
        }

        private async Task StartSequentialChartUpdates(CancellationToken token)
        {
            // Пары графиков, которые должны обновляться одновременно
            (CartesianChart, CartesianChart)[] chartPairs =
            {
        (Current_Chart_En, Voltage_Chart_En),
        (Current_Chart_Ihh, Voltage_Chart_Ihh),
        (Current_Chart_Iprot, Voltage_Chart_Iprot),
        (Current_Chart_Ikz, Voltage_Chart_Ikz),
        (Current_Chart_Upuls, Voltage_Chart_Upuls)
            };

            foreach (var (currentChart, voltageChart) in chartPairs)
            {
                if (token.IsCancellationRequested)
                    return;

                // Запускаем обновление сразу двух графиков
                Task currentTask = UpdateChartForDuration(currentChart, token, 20000);
                Task voltageTask = UpdateChartForDuration(voltageChart, token, 20000);

                await Task.WhenAll(currentTask, voltageTask);

                if (token.IsCancellationRequested)
                    return;
            }
        }
        // Функция сброса данных графиков перед началом теста
        private void ClearAllCharts()
        {
            CartesianChart[] charts =
            {
        Current_Chart_En, Voltage_Chart_En,
        Current_Chart_Ihh, Voltage_Chart_Ihh,
        Current_Chart_Iprot, Voltage_Chart_Iprot,
        Current_Chart_Ikz, Voltage_Chart_Ikz,
        Current_Chart_Upuls, Voltage_Chart_Upuls
            };

            foreach (var chart in charts)
            {
                Application.Current.Dispatcher.Invoke(() =>
                {
                    if (chart.Series.Count > 0)
                    {
                        ((ChartValues<double>)chart.Series[0].Values).Clear();
                    }
                });
            }
        }
        private async Task UpdateChartData(CartesianChart chart, double newValue)
        {
            await Application.Current.Dispatcher.InvokeAsync(() =>
            {
                var values = (ChartValues<double>)chart.Series[0].Values;

                values.Add(newValue);
                if (values.Count > 10)
                {
                    values.RemoveAt(0);
                }

                // Определяем тип графика (ток или напряжение)
                string chartTitle = chart.Series[0].Title;
                bool isCurrentChart = chartTitle.StartsWith("Данные I_");
                bool isVoltageChart = chartTitle.StartsWith("Данные U_");

                // Проверяем, привязан ли SectionControl
                if (linkedSection != null)
                {
                    Debug.WriteLine($"Передача числа {newValue} в SectionControl ({chartTitle})"); // Логирование

                    if (isCurrentChart)
                    {
                        linkedSection.UpdateTextBlock(newValue); // Обновляем текстовое поле тока
                    }
                    else if (isVoltageChart)
                    {
                        linkedSection.UpdateVoltageTextBlock(newValue); // Обновляем текстовое поле напряжения
                    }
                }
                else
                {
                    Debug.WriteLine("linkedSection == null или неправильный график");
                }
            });
        }

        private async Task UpdateChartForDuration(CartesianChart chart, CancellationToken token, int durationMs)
        {
            int elapsedTime = 0;

            while (elapsedTime < durationMs)
            {
                if (token.IsCancellationRequested)
                    return;

                double randomNumber = random.Next(0, 11);
                await UpdateChartData(chart, randomNumber); // Вызываем функцию обновления графика

                await Task.Delay(5000, token);
                elapsedTime += 5000;
            }
        }

        public void StartCharts()
        {
            if (cancellationTokenSource != null)
                return; // Уже запущены

            ClearAllCharts();
            cancellationTokenSource = new CancellationTokenSource();
            CancellationToken token = cancellationTokenSource.Token;

            _ = StartSequentialChartUpdates(token);
        }

        

        public void StopCharts()
        {
            cancellationTokenSource?.Cancel();
            cancellationTokenSource = null;
        }

        private void Start_Click(object sender, RoutedEventArgs e)
        {
            StartCharts();
            TestStateChanged?.Invoke(this, true); // Оповещаем TestPage о старте
            ChangeInnerGridState(true);
        }

        private void Stop_Click(object sender, RoutedEventArgs e)
        {
            StopCharts();
            TestStateChanged?.Invoke(this, false); // Оповещаем TestPage о стопе
            ChangeInnerGridState(false);
        }
        private void ChangeInnerGridState(bool isStarted)
        {
            if (linkedSection != null)
            {
                var innerGrid1 = linkedSection.FindName("InnerGrid1") as Grid;
                var innerGrid2 = linkedSection.FindName("InnerGrid2") as Grid;

                if (innerGrid1 != null && innerGrid2 != null)
                {
                    innerGrid1.Visibility = isStarted ? Visibility.Visible : Visibility.Collapsed;
                    innerGrid2.Visibility = isStarted ? Visibility.Collapsed : Visibility.Visible;
                }
            }
        }
    }


}
