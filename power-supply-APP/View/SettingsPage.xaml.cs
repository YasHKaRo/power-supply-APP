using power_supply_APP.View;
using System;
using System.Collections.Generic;
using System.Globalization;
using System.IO;
using IOPath = System.IO.Path;
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
using System.Xml.Linq;

namespace power_supply_APP
{
    /// <summary>
    /// Логика взаимодействия для Page1.xaml
    /// </summary>
    public partial class SettingsPage : Page
    {
        public SettingsPage()
        {
            InitializeComponent();
            LoadData();
            LoadPowerSupplyFiles();
        }
        private void BackButton_Click(object sender, RoutedEventArgs e)
        {
            // Закрываем текущее окно настроек
            NavigationService.GoBack();
        }
        public class RelayData
        {
            public string RelayName { get; set; }
            public int Col1 { get; set; }
            public int Col2 { get; set; }
            public int Col3 { get; set; }
            public int Col4 { get; set; }
            public int Col5 { get; set; }
            public int Col6 { get; set; }
            public int Col7 { get; set; }
            public int Col8 { get; set; }
        }
        private void LoadData()
        {
            var relayDataList = new List<RelayData>
            {
                new RelayData { RelayName = "Реле 90 В", Col1 = 0, Col2 = 0, Col3 = 0, Col4 = 0, Col5 = 0, Col6 = 0, Col7 = 0, Col8 = 0 },
                new RelayData { RelayName = "Реле 260 В", Col1 = 0, Col2 = 0, Col3 = 0, Col4 = 0, Col5 = 0, Col6 = 0, Col7 = 0, Col8 = 0 }
            };

            RelayDataGrid.ItemsSource = relayDataList;
        }
        private void OnChangeButtonClick(object sender, RoutedEventArgs e)
        {
            Button button = sender as Button;
            if (button != null)
            {
                string textBoxName = button.Tag?.ToString();
                TextBox textBox = (TextBox)this.FindName(textBoxName);

                if (textBox != null)
                {
                    // Заменяем запятую на точку для корректного парсинга
                    string input = textBox.Text.Replace(',', '.');

                    if (double.TryParse(input, NumberStyles.Any, CultureInfo.InvariantCulture, out double value))
                    {
                        if (button.Content.ToString() == "-")
                        {
                            value -= 0.5;
                        }
                        else if (button.Content.ToString() == "+")
                        {
                            value += 0.5;
                        }
                        if (button.Content.ToString() == "-0.001")
                        {
                            value -= 0.001;
                        }
                        else if (button.Content.ToString() == "+0.001")
                        {
                            value += 0.001;
                        }
                        textBox.Text = value.ToString(CultureInfo.InvariantCulture);
                    }
                    else
                    {
                        MessageBox.Show("Пожалуйста, введите корректное значение.");
                        textBox.Text = "0";
                    }
                }
            }
        }
        private void Click_Admin(object sender, RoutedEventArgs e)
        {
            Button button = sender as Button;
            string password = PasswordBox.Password;
            if (button != null && password == "55555")
            {
                // Скрываем окно авторизации
                AuthorizationAdmin.Visibility = Visibility.Collapsed;

                // Показываем настройки администратора
                AdminSettings.Visibility = Visibility.Visible;
            }
        }
        private void LoadPowerSupplyFiles()
        {
            string directoryPath = "C:\\Users\\ro517\\Рабочий стол\\ВКР\\power-supply-APP\\power-supply-APP\\PowerUnit"; // Укажите путь к папке с XML-файлами

            if (Directory.Exists(directoryPath))
            {
                var files = Directory.GetFiles(directoryPath, "*.xml");
                foreach (var file in files)
                {
                    ComboBoxItem item = new ComboBoxItem
                    {
                        Content = IOPath.GetFileNameWithoutExtension(file),
                        Tag = file // Храним полный путь
                    };
                    PowerSupplyComboBox.Items.Add(item);
                }
            }
        }
        private void PowerSupplyComboBox_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            if (PowerSupplyComboBox.SelectedItem is ComboBoxItem selectedItem)
            {
                string filePath = selectedItem.Tag.ToString();
                LoadPowerSupplyData(filePath);
            }
        }

        private void LoadPowerSupplyData(string filePath)
        {
            if (File.Exists(filePath))
            {
                XDocument xmlDoc = XDocument.Load(filePath);
                var powerSupply = xmlDoc.Root;

                if (powerSupply != null)
                {
                    PowerSupplyNameTextBox.Text = powerSupply.Element("NamePoweUnit")?.Value;
                    PowerSupplyCodeTextBox.Text = powerSupply.Element("CodePoweUnit")?.Value;
                    TempWarkUpModule.Text = powerSupply.Element("PIDTemperature")?.Value;
                    TempWarkUpModuleApprox.Text = powerSupply.Element("PIDTemperatureLimit")?.Value;
                    EnergyDuration.Text = powerSupply.Element("TimeCycle")?.Value;
                    WarmUpDuration.Text = powerSupply.Element("TimeWarming")?.Value;
                    IhhDuration.Text = powerSupply.Element("TimeNoLoad")?.Value;
                    IprotectDuration.Text = powerSupply.Element("TimeShortCircuit")?.Value;
                    IkzDuration.Text = powerSupply.Element("TimeProtected")?.Value;
                    UPulsDuration.Text = powerSupply.Element("TimeRipple")?.Value;
                    ValueExitEnergy.Text = powerSupply.Element("Voltage_PU_EL_Measure_Max")?.Value;
                    DeviationValueExitEnergy.Text = powerSupply.Element("Voltage_PU_EL_Measure_Limit")?.Value;
                    ProtectionRangeCurrentMin.Text = powerSupply.Element("Current_PU_Protected_Min")?.Value;
                    ProtectionRangeCurrentMax.Text = powerSupply.Element("Current_PU_Protected_Max")?.Value;
                    ProtectionRangeIncreasingValueMin.Text = powerSupply.Element("Current_PU_EL_Protected_Min")?.Value;
                    ProtectionRangeIncreasingValueMax.Text = powerSupply.Element("Current_PU_EL_Protected_Max")?.Value;
                    MaxOutputPulse.Text = powerSupply.Element("RippleVoltage")?.Value;
                    MaxInputCurrent.Text = powerSupply.Element("Current_Multimetr_Max_PU")?.Value;
                    MaxInputHHCurrent.Text = powerSupply.Element("Current_No_load_Multimetr_Max_PU")?.Value;
                    MaxInputKZCurrent.Text = powerSupply.Element("Current_Short_Circuit_Multimetr_Max_PU")?.Value;
                    MaxValueExitCurrent.Text = powerSupply.Element("Current_PU_EL_Measure_Max")?.Value;
                    CurrentLimit.Text = powerSupply.Element("Current_EL_Protected")?.Value;
                    int rangeNL_SC = int.Parse(powerSupply.Element("CurrentRange_Multimetr_NL_SC")?.Value ?? "0");
                    int rangeMax = int.Parse(powerSupply.Element("CurrentRange_Multimetr_Max")?.Value ?? "0");


                    // Заполняем ComboBox в зависимости от значений
                    UpdateComboBox(CurrentRangeComboBoxNL_SC, rangeNL_SC);
                    UpdateComboBox(CurrentRangeComboBoxMax, rangeMax);
                }
            }
        }
       

        // Метод для обновления ComboBox в зависимости от параметра
        private void UpdateComboBox(ComboBox comboBox, int rangeValue)
        {
            comboBox.Items.Clear();

            if (rangeValue == 0)
            {
                comboBox.Items.Add("0.005-1.2");
                comboBox.Items.Add("0.025-6.0");
            }
            else
            {
                comboBox.Items.Add("0.001-0.5");
                comboBox.Items.Add("0.01-3.0");
            }

            // Выбираем первый элемент по умолчанию
            comboBox.SelectedIndex = 0;
        }
        private void SaveButton_Click(object sender, RoutedEventArgs e)
        {
            if (PowerSupplyComboBox.SelectedItem is ComboBoxItem selectedItem)
            {
                string filePath = selectedItem.Tag.ToString();
                SavePowerSupplyData(filePath);
            }
        }

        private void SavePowerSupplyData(string filePath)
        {
            if (File.Exists(filePath))
            {
                XDocument xmlDoc = XDocument.Load(filePath);
                var powerSupply = xmlDoc.Root;

                if (powerSupply != null)
                {
                    powerSupply.Element("Name")?.SetValue(PowerSupplyNameTextBox.Text);
                    powerSupply.Element("Code")?.SetValue(PowerSupplyCodeTextBox.Text);

                    xmlDoc.Save(filePath);
                }
            }
        }
    }
}