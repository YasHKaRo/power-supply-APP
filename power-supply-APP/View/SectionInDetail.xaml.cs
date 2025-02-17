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
using System.Timers;

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
        private List<string> activeTests = new List<string>();

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
        

        public void SetSelectedTests(List<string> selectedTests)
        {
            activeTests = selectedTests;
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
            ResetAllIndicators(); // Сбрасываем все индикаторы перед началом тестирования

            var chartPairs = new List<(CartesianChart, CartesianChart, string)>();

            if (activeTests.Contains("EnergyCycle"))
                chartPairs.Add((Current_Chart_En, Voltage_Chart_En, "EnergyCycle"));
            if (activeTests.Contains("Ihh"))
                chartPairs.Add((Current_Chart_Ihh, Voltage_Chart_Ihh, "Ihh"));
            if (activeTests.Contains("Iprotect"))
                chartPairs.Add((Current_Chart_Iprot, Voltage_Chart_Iprot, "Iprotect"));
            if (activeTests.Contains("Ikz"))
                chartPairs.Add((Current_Chart_Ikz, Voltage_Chart_Ikz, "Ikz"));
            if (activeTests.Contains("Upulse"))
                chartPairs.Add((Current_Chart_Upuls, Voltage_Chart_Upuls, "Upulse"));

            foreach (var (currentChart, voltageChart, testName) in chartPairs)
            {
                if (token.IsCancellationRequested)
                    return;

                Task<bool> currentTask = UpdateChartForDuration(currentChart, token, 10000);
                Task<bool> voltageTask = UpdateChartForDuration(voltageChart, token, 10000);

                bool[] results = await Task.WhenAll(currentTask, voltageTask);
                bool isSuccessful = results.All(result => result); // Успешно, если оба графика в норме

                UpdateIndicator(testName, isSuccessful); // Меняем цвет индикатора
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

        private async Task<bool> UpdateChartForDuration(CartesianChart chart, CancellationToken token, int durationMs)
        {
            int elapsedTime = 0;
            bool isSuccessful = true;

            while (elapsedTime < durationMs)
            {
                if (token.IsCancellationRequested)
                    return false;

                double randomNumber = random.Next(0, 11);
                await UpdateChartData(chart, randomNumber);

                if (!IsTestSuccessful(randomNumber))
                {
                    isSuccessful = false;
                }

                await Task.Delay(1000, token);
                elapsedTime += 1000;
            }

            return isSuccessful;
        }

        // Функция проверки успешности теста
        private bool IsTestSuccessful(double value)
        {
            return value >= 1 && value <= 10; // Успех, если значение в пределах [1,10]
        }

        // Функция обновления индикатора
        private void UpdateIndicator(string testName, bool isSuccessful)
        {
            SolidColorBrush color = isSuccessful ? new SolidColorBrush(Colors.Green) : new SolidColorBrush(Colors.Red);

            switch (testName)
            {
                case "EnergyCycle":
                    GrayIndicatorEnergy.Fill = color;
                    break;
                case "Ihh":
                    GrayIndicatorIhh.Fill = color;
                    break;
                case "Iprotect":
                    GrayIndicatorIprot.Fill = color;
                    break;
                case "Ikz":
                    GrayIndicatorIkz.Fill = color;
                    break;
                case "Upulse":
                    GrayIndicatorUpuls.Fill = color;
                    break;
            }
        }
        private void ResetAllIndicators()
        {
            GrayIndicatorEnergy.Fill = new SolidColorBrush(Colors.Gray);
            GrayIndicatorIhh.Fill = new SolidColorBrush(Colors.Gray);
            GrayIndicatorIprot.Fill = new SolidColorBrush(Colors.Gray);
            GrayIndicatorIkz.Fill = new SolidColorBrush(Colors.Gray);
            GrayIndicatorUpuls.Fill = new SolidColorBrush(Colors.Gray);
        }

        public void StartCharts(List<string> tests)
        {
            ClearAllCharts();
            activeTests = tests; // Присваиваем индивидуальный список тестов

            if (cancellationTokenSource != null)
            {
                cancellationTokenSource.Cancel();
            }
            cancellationTokenSource = new CancellationTokenSource();
            StartSequentialChartUpdates(cancellationTokenSource.Token);
        }



        public void StopCharts()
        {
            cancellationTokenSource?.Cancel();
            cancellationTokenSource = null;
        }

        private void Start_Click(object sender, RoutedEventArgs e)
        {
            StartCharts(activeTests); // Теперь передаём актуальный список тестов
            TestStateChanged?.Invoke(this, true); // Оповещаем TestPage о старте
            ChangeInnerGridState(true);
        }

        private void Stop_Click(object sender, RoutedEventArgs e)
        {
            StopCharts();
            TestStateChanged?.Invoke(this, false); // Оповещаем TestPage о стопе
            ChangeInnerGridState(false);
        }
        public void SetDeviceInfo(string deviceName, string serialNumber)
        {
            DetailName.Text = deviceName;
            DetailNumb.Text = serialNumber;
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
