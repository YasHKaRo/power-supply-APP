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
        private Dictionary<SectionControl, SectionInDetail> sectionMappings = new Dictionary<SectionControl, SectionInDetail>();
        private Dictionary<string, List<string>> sectionTests = new Dictionary<string, List<string>>();

        public TestPage()
        {
            InitializeComponent();
            InitializeSectionMappings();
        }
        private void InitializeSectionMappings()
        {
            sectionMappings[Section_1] = Section_Detail_1;
            sectionMappings[Section_2] = Section_Detail_2;
            sectionMappings[Section_3] = Section_Detail_3;
            sectionMappings[Section_4] = Section_Detail_4;
            sectionMappings[Section_5] = Section_Detail_5;
            sectionMappings[Section_6] = Section_Detail_6;
            sectionMappings[Section_7] = Section_Detail_7;
            sectionMappings[Section_8] = Section_Detail_8;

            // Устанавливаем связь в каждом SectionInDetail
            foreach (var pair in sectionMappings)
            {
                pair.Value.LinkSectionControl(pair.Key);

                // Подписываемся на событие TestStateChanged для обновления UI
                pair.Value.TestStateChanged += (sender, isStarted) =>
                {
                    UpdateInnerGrid(pair.Key, isStarted);
                };
            }
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

            TestingSettingsWindow settingsWindow = new TestingSettingsWindow(this)
            {
                SectionIndex = index // Передаём номер секции
            };
            if (settingsWindow.ShowDialog() == true) // Проверяем, было ли окно закрыто успешно
            {
                string inputText = settingsWindow.InputText; // Получаем текст из окна добавления
                string serialText = settingsWindow.SerialText;


                TextBlock deviceBlock = FindName($"DeviceBlock{index}") as TextBlock;
                TextBlock serialBlock = FindName($"SerialBlock{index}") as TextBlock;

                if (deviceBlock != null) deviceBlock.Text = inputText;
                if (serialBlock != null) serialBlock.Text = serialText;

                if (sectionMappings.TryGetValue(FindName($"Section_{index}") as SectionControl, out SectionInDetail sectionDetail))
                {
                    sectionDetail.SetDeviceInfo(inputText, serialText);
                }
            }
        }
        public void UpdateSelectedTests(int sectionIndex, List<string> selectedTests)
        {
            string sectionKey = $"Section_{sectionIndex}";

            if (sectionTests.ContainsKey(sectionKey))
                sectionTests[sectionKey] = selectedTests;
            else
                sectionTests.Add(sectionKey, selectedTests);
        }

        private void Start_Test(object sender, RoutedEventArgs e)
        {
            Button clickedButton = sender as Button;
            string sectionName = clickedButton.Tag.ToString();

            if (sectionMappings.TryGetValue(FindName(sectionName) as SectionControl, out SectionInDetail sectionDetail))
            {
                // Получаем индивидуальные тесты для этой секции
                if (sectionTests.TryGetValue(sectionName, out List<string> testsForThisSection))
                {
                    sectionDetail.StartCharts(testsForThisSection); // Передаём конкретные тесты
                }

                UpdateInnerGrid(FindName(sectionName) as SectionControl, true);
            }
        }

        private void Stop_Test(object sender, RoutedEventArgs e)
        {
            Button clickedButton = sender as Button;
            string sectionName = clickedButton.Tag.ToString();

            if (sectionMappings.TryGetValue(FindName(sectionName) as SectionControl, out SectionInDetail sectionDetail))
            {
                sectionDetail.StopCharts();
                UpdateInnerGrid(FindName(sectionName) as SectionControl, false);
            }
        }
        // Метод для обновления состояния SectionControl
        private void UpdateInnerGrid(SectionControl sectionControl, bool isStarted)
        {
            if (sectionControl != null)
            {
                var innerGrid1 = sectionControl.FindName("InnerGrid1") as Grid;
                var innerGrid2 = sectionControl.FindName("InnerGrid2") as Grid;

                if (innerGrid1 != null && innerGrid2 != null)
                {
                    innerGrid1.Visibility = isStarted ? Visibility.Visible : Visibility.Collapsed;
                    innerGrid2.Visibility = isStarted ? Visibility.Collapsed : Visibility.Visible;
                }
            }
        }

    }
}
