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
using power_supply_APP.Model;
using power_supply_APP.ViewModel;
using Newtonsoft.Json;

namespace power_supply_APP
{
    /// <summary>
    /// Логика взаимодействия для SettingsPage.xaml
    /// </summary>
    public class ModeConfig
    {
        public bool Enabled { get; set; }
        public double Min { get; set; }
        public double Max { get; set; }
        public double Single { get; set; }
        public double Pause { get; set; }
        public bool TempControl { get; set; }
        public string VoltageMode { get; set; }
        public double Duration { get; set; }
        public int Impacts { get; set; }
    }
    public class ReportsConfig
    {
        public ReportPaths Reports { get; set; }
        public NormsConfig Norms { get; set; }
        public string StopMode { get; set; }
        public double PercentRefusal { get; set; }
        public bool PIDMode { get; set; }
        public VoltageConfig VoltageSettings { get; set; }
        public Measurement Measurement { get; set; }
        public double CorrectionFactor { get; set; }
        public string DisableMode { get; set; }
    }

    public class ReportPaths
    {
        public string TxtPath { get; set; }
        public string DocPath { get; set; }
        public string Type { get; set; }
        public bool CreateDoc { get; set; }
        public bool PrintDocAutomatically { get; set; }
    }

    public class NormsConfig
    {
        public double WarmUp { get; set; }
        public double Energy { get; set; }
        public double Ihh { get; set; }
        public double Ikz { get; set; }
        public double IProtect { get; set; }
        public double UPuls { get; set; }
    }

    public class VoltageConfig
    {
        public double MinVolt { get; set; }
        public double MinVoltApprox { get; set; }
        public double MaxVolt { get; set; }
        public double MaxVoltApprox { get; set; }
    }

    public class Measurement
    {
        public string Range { get; set; }
        public string Frequency { get; set; }
        public string Filter { get; set; }
    }
    public partial class SettingsPage : Page
    {
        private ConfigManager configManager = new ConfigManager(); // Загружаем конфиг
        private const string _configFileName = @"..\..\Files\Other\modes_conf.json";
        private const string ReportsConfigRelativePath = @"..\..\Files\Other\reports_conf.json";

        public SettingsPage()
        {
            InitializeComponent();
            LoadData();
            LoadPowerSupplyFiles();
            Loaded += SettingsPage_Loaded;
        }
        private void SettingsPage_Loaded(object sender, RoutedEventArgs e)
        {
            LoadModesConfiguration();
            LoadReportsConfiguration();
            if (AuthManager.CurrentUserRole != UserRole.Admin)
            {
                AdminSettingsPanel.Visibility = Visibility.Collapsed;
                personalSettings.Visibility = Visibility.Collapsed;
                modes.Visibility = Visibility.Collapsed;
                Network.Visibility = Visibility.Collapsed;
            }
            else
            {
                AdminSettingsPanel.Visibility = Visibility.Visible;
                personalSettings.Visibility = Visibility.Visible;
                modes.Visibility = Visibility.Visible;
                Network.Visibility = Visibility.Visible;
            }
        }
        private void LoadModesConfiguration()
        {
            var path = System.IO.Path.Combine(AppDomain.CurrentDomain.BaseDirectory, _configFileName);
            if (!File.Exists(path)) return;

            var json = File.ReadAllText(path);
            var configs = JsonConvert.DeserializeObject<Dictionary<string, ModeConfig>>(json);
            if (configs == null) return;

            // EnergyCycle
            if (configs.TryGetValue("EnergyCycle", out var ec))
            {
                EnergyCycleCheckBox.IsChecked = ec.Enabled;
                TextBoxMinEn.Text = ec.Min.ToString();
                TextBoxMaxEn.Text = ec.Max.ToString();
                TextBoxSingleEn.Text = ec.Single.ToString();
                TextBoxPauseEn.Text = ec.Pause.ToString();
                TempControlEn.IsChecked = ec.TempControl;
            }

            // Ihh
            if (configs.TryGetValue("Ihh", out var ihh))
            {
                IhhCheckBox.IsChecked = ihh.Enabled;
                TextBoxSingleHH.Text = ihh.Single.ToString();
                TextBoxPauseHH.Text = ihh.Pause.ToString();
                TextBoxImpactsHH.Text = ihh.Impacts.ToString();
                TempControlHH.IsChecked = ihh.TempControl;
            }

            // Iprotect
            if (configs.TryGetValue("Iprotect", out var ip))
            {
                IprotectCheckBox.IsChecked = ip.Enabled;
                TextBoxSingleProtect.Text = ip.Single.ToString();
                TextBoxPauseProtect.Text = ip.Pause.ToString();
                TextBoxImpactsProtect.Text = ip.Impacts.ToString();
                TempControlProtect.IsChecked = ip.TempControl;
            }

            // Ikz
            if (configs.TryGetValue("Ikz", out var ikz))
            {
                IkzCheckBox.IsChecked = ikz.Enabled;
                TextBoxSingleKZ.Text = ikz.Single.ToString();
                TextBoxPauseKZ.Text = ikz.Pause.ToString();
                TextBoxImpactsKZ.Text = ikz.Impacts.ToString();
                TempControlKZ.IsChecked = ikz.TempControl;
            }

            // UPulse
            if (configs.TryGetValue("UPulse", out var up))
            {
                UPulseCheckBox.IsChecked = up.Enabled;
                TextBoxSinglePulse.Text = up.Single.ToString();
                TextBoxPausePulse.Text = up.Pause.ToString();
                TempControlPulse.IsChecked = up.TempControl;
            }

            // WarmUp
            if (configs.TryGetValue("WarmUp", out var wu))
            {
                WarmUp.IsChecked = wu.Enabled;
                RadioButtonWarmUpMin.IsChecked = wu.VoltageMode?.Equals("Min", StringComparison.OrdinalIgnoreCase) ?? true;
                RadioButtonWarmUpMax.IsChecked = wu.VoltageMode?.Equals("Max", StringComparison.OrdinalIgnoreCase) ?? false;
                TextBoxWarmUpSingle.Text = wu.Single.ToString();
                TextBoxWarmUpPause.Text = wu.Pause.ToString();
            }

            // WarmDown
            if (configs.TryGetValue("WarmDown", out var wd))
            {
                WarmDown.IsChecked = wd.Enabled;
                TextBoxWarmDownDuration.Text = wd.Duration.ToString();
            }
        }
        private void LoadReportsConfiguration()
        {
            string fullPath = System.IO.Path.GetFullPath(System.IO.Path.Combine(
                AppDomain.CurrentDomain.BaseDirectory,
                ReportsConfigRelativePath));
            if (!File.Exists(fullPath)) return;

            var json = File.ReadAllText(fullPath);
            var cfg = JsonConvert.DeserializeObject<ReportsConfig>(json);
            if (cfg == null) return;

            // пути
            reportsTxtPath.Text = cfg.Reports.TxtPath;
            reportsDocPath.Text = cfg.Reports.DocPath;

            // тип отчёта
            FullReportRadio.IsChecked = cfg.Reports.Type == "Full";
            FailuresReportRadio.IsChecked = cfg.Reports.Type == "Failures";

            // чекбоксы
            CreateReportDoc.IsChecked = cfg.Reports.CreateDoc;
            PrintReportDoc.IsChecked = cfg.Reports.PrintDocAutomatically;

            // нормы
            WarmUpNorma.Text = cfg.Norms.WarmUp.ToString();
            EnergyNorma.Text = cfg.Norms.Energy.ToString();
            IhhNorma.Text = cfg.Norms.Ihh.ToString();
            IkzNorma.Text = cfg.Norms.Ikz.ToString();
            IProtectNorma.Text = cfg.Norms.IProtect.ToString();
            UPulsNorma.Text = cfg.Norms.UPuls.ToString();

            StopCurrentStartNext.IsChecked = cfg.StopMode == "StopCurrentStartNext";
            StopAll.IsChecked = cfg.StopMode == "StopAll";
            StopAfterCurrent.IsChecked = cfg.StopMode == "StopAfterCurrent";

            // прочие простые
            PercentRefusal.Text = cfg.PercentRefusal.ToString();
            PIDMode.IsChecked = cfg.PIDMode;

            // напряжения
            TextBoxMinVolt.Text = cfg.VoltageSettings.MinVolt.ToString();
            TextBoxMinVoltApprox.Text = cfg.VoltageSettings.MinVoltApprox.ToString();
            TextBoxMaxVolt.Text = cfg.VoltageSettings.MaxVolt.ToString();
            TextBoxMaxVoltApprox.Text = cfg.VoltageSettings.MaxVoltApprox.ToString();

            // Measurement
            MeasurementRange.SelectedItem = MeasurementRange.Items
                .Cast<object>()
                .FirstOrDefault(i => ((TextBlock)i).Text == cfg.Measurement.Range);
            Frequency.SelectedItem = Frequency.Items
                .Cast<object>()
                .FirstOrDefault(i => ((TextBlock)i).Text == cfg.Measurement.Frequency);
            Filter.SelectedItem = Filter.Items
                .Cast<object>()
                .FirstOrDefault(i => ((TextBlock)i).Text == cfg.Measurement.Filter);

            // коэффициент и disable-mode
            CorrectionFactor.Text = cfg.CorrectionFactor.ToString();
            DisableOnlyOne.IsChecked = cfg.DisableMode == "OnlyOne";
            DisableAll.IsChecked = cfg.DisableMode == "All";
        }
        private void SaveSettingsButton_Click(object sender, RoutedEventArgs e)
        {
            // Собираем текущие значения из UI
            var configs = new Dictionary<string, ModeConfig>();

            // EnergyCycle
            configs["EnergyCycle"] = new ModeConfig
            {
                Enabled = EnergyCycleCheckBox.IsChecked == true,
                Min = ParseDouble(TextBoxMinEn.Text, 0.5),
                Max = ParseDouble(TextBoxMaxEn.Text, 0.5),
                Single = ParseDouble(TextBoxSingleEn.Text, 0.5),
                Pause = ParseDouble(TextBoxPauseEn.Text, 2),
                TempControl = TempControlEn.IsChecked == true
            };

            // Ihh
            configs["Ihh"] = new ModeConfig
            {
                Enabled = IhhCheckBox.IsChecked == true,
                Single = ParseDouble(TextBoxSingleHH.Text, 0.5),
                Pause = ParseDouble(TextBoxPauseHH.Text, 2),
                Impacts = ParseInt(TextBoxImpactsHH.Text, 20),
                TempControl = TempControlHH.IsChecked == true
            };

            // Iprotect
            configs["Iprotect"] = new ModeConfig
            {
                Enabled = IprotectCheckBox.IsChecked == true,
                Single = ParseDouble(TextBoxSingleProtect.Text, 0.5),
                Pause = ParseDouble(TextBoxPauseProtect.Text, 2),
                Impacts = ParseInt(TextBoxImpactsProtect.Text, 20),
                TempControl = TempControlProtect.IsChecked == true
            };

            // Ikz
            configs["Ikz"] = new ModeConfig
            {
                Enabled = IkzCheckBox.IsChecked == true,
                Single = ParseDouble(TextBoxSingleKZ.Text, 0.5),
                Pause = ParseDouble(TextBoxPauseKZ.Text, 2),
                Impacts = ParseInt(TextBoxImpactsKZ.Text, 20),
                TempControl = TempControlKZ.IsChecked == true
            };

            // UPulse
            configs["UPulse"] = new ModeConfig
            {
                Enabled = UPulseCheckBox.IsChecked == true,
                Single = ParseDouble(TextBoxSinglePulse.Text, 0.5),
                Pause = ParseDouble(TextBoxPausePulse.Text, 2),
                TempControl = TempControlPulse.IsChecked == true
            };

            // WarmUp
            configs["WarmUp"] = new ModeConfig
            {
                Enabled = WarmUp.IsChecked == true,
                VoltageMode = (RadioButtonWarmUpMin.IsChecked == true) ? "Min" : "Max",
                Single = ParseDouble(TextBoxWarmUpSingle.Text, 0.5),
                Pause = ParseDouble(TextBoxWarmUpPause.Text, 2)
            };

            // WarmDown
            configs["WarmDown"] = new ModeConfig
            {
                Enabled = WarmDown.IsChecked == true,
                Duration = ParseDouble(TextBoxWarmDownDuration.Text, 0.5)
            };

            // Путь к файлу
            var path = System.IO.Path.Combine(AppDomain.CurrentDomain.BaseDirectory, _configFileName);

            try
            {
                // Серриализуем и сохраняем
                string json = JsonConvert.SerializeObject(configs, Formatting.Indented);
                File.WriteAllText(path, json);
                MessageBox.Show($"Конфигурация сохранена в:\n{path}",
                                "Сохранение успешно",
                                MessageBoxButton.OK,
                                MessageBoxImage.Information);
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Ошибка при сохранении конфигурации:\n{ex.Message}",
                                "Ошибка",
                                MessageBoxButton.OK,
                                MessageBoxImage.Error);
            }
            SaveReportsConfiguration();
        }
        private void SaveReportsConfiguration()
        {
            try
            {
                Console.WriteLine("Сборка конфигурации: Reports");
                var reports = new ReportPaths
                {
                    TxtPath = reportsTxtPath.Text,
                    DocPath = reportsDocPath.Text,
                    Type = FullReportRadio.IsChecked == true ? "Full" : "Failures",
                    CreateDoc = CreateReportDoc.IsChecked == true,
                    PrintDocAutomatically = PrintReportDoc.IsChecked == true
                };

                Console.WriteLine("Сборка конфигурации: Norms");
                var norms = new NormsConfig
                {
                    WarmUp = ParseDouble(WarmUpNorma.Text, 1.0),
                    Energy = ParseDouble(EnergyNorma.Text, 2.0),
                    Ihh = ParseDouble(IhhNorma.Text, 1.0),
                    Ikz = ParseDouble(IkzNorma.Text, 1.0),
                    IProtect = ParseDouble(IProtectNorma.Text, 1.0),
                    UPuls = ParseDouble(UPulsNorma.Text, 1.0)
                };

                Console.WriteLine("Сборка конфигурации: VoltageSettings");
                var voltageSettings = new VoltageConfig
                {
                    MinVolt = ParseDouble(TextBoxMinVolt.Text, 90.0),
                    MinVoltApprox = ParseDouble(TextBoxMinVoltApprox.Text, 0.1),
                    MaxVolt = ParseDouble(TextBoxMaxVolt.Text, 265.0),
                    MaxVoltApprox = ParseDouble(TextBoxMaxVoltApprox.Text, 0.1)
                };

                Console.WriteLine("Сборка конфигурации: Measurement");
                var measurement = new Measurement
                {
                    Range = ((TextBlock)MeasurementRange.SelectedItem)?.Text,
                    Frequency = ((TextBlock)Frequency.SelectedItem)?.Text,
                    Filter = ((TextBlock)Filter.SelectedItem)?.Text
                };

                Console.WriteLine("Формирование финального объекта ReportsConfig");
                var cfg = new ReportsConfig
                {
                    Reports = reports,
                    Norms = norms,
                    StopMode = StopCurrentStartNext.IsChecked == true ? "StopCurrentStartNext"
                             : StopAll.IsChecked == true ? "StopAll"
                             : "StopAfterCurrent",
                    PercentRefusal = ParseDouble(PercentRefusal.Text, 0.0),
                    PIDMode = PIDMode.IsChecked == true,
                    VoltageSettings = voltageSettings,
                    Measurement = measurement,
                    CorrectionFactor = ParseDouble(CorrectionFactor.Text, 1.0),
                    DisableMode = DisableOnlyOne.IsChecked == true ? "OnlyOne" : "All"
                };

                Console.WriteLine("Формирование пути до файла");
                string fullPath = System.IO.Path.GetFullPath(System.IO.Path.Combine(
                    AppDomain.CurrentDomain.BaseDirectory, ReportsConfigRelativePath));

                Console.WriteLine("Сериализация в JSON и сохранение в файл: " + fullPath);
                File.WriteAllText(fullPath,
                    JsonConvert.SerializeObject(cfg, Formatting.Indented));

                Console.WriteLine("Конфигурация успешно сохранена!");
            }
            catch (Exception ex)
            {
                Console.WriteLine("Ошибка при сохранении конфигурации: " + ex.Message);
                MessageBox.Show("Ошибка при сохранении: " + ex.Message);
            }
        }
        private double ParseDouble(string s, double fallback)
        {
            return double.TryParse(s, out var v) ? v : fallback;
        }

        private int ParseInt(string s, int fallback)
        {
            return int.TryParse(s, out var v) ? v : fallback;
        }
        private void BackButton_Click(object sender, RoutedEventArgs e)
        {
            if (Application.Current.MainWindow is MainWindow mainWindow)
            {
                mainWindow.NavigateToTestPage(); // Переход на уже существующий TestPage
            }
        }
        private void ChangePassword_Click(object sender, RoutedEventArgs e)
        {
            string login = LoginTextBox.Text;
            string newPassword = NewPasswordBox.Password;
            string confirmPassword = ConfirmPasswordBox.Password;

            if (string.IsNullOrEmpty(login) || string.IsNullOrEmpty(newPassword) || string.IsNullOrEmpty(confirmPassword))
            {
                ResultTextBlock.Text = "Все поля должны быть заполнены!";
                return;
            }

            if (newPassword != confirmPassword)
            {
                ResultTextBlock.Text = "Пароли не совпадают!";
                return;
            }

            if (configManager.ChangePassword(login, newPassword))
            {
                ResultTextBlock.Text = "Пароль успешно изменён!";
            }
            else
            {
                ResultTextBlock.Text = "Пользователь не найден!";
            }
        }
        public bool IsEnergyCycleChecked => EnergyCycleCheckBox.IsChecked ?? false;
        public bool IsIhhChecked => IhhCheckBox.IsChecked ?? false;
        public bool IsIprotectChecked => IprotectCheckBox.IsChecked ?? false;
        public bool IsIkzChecked => IkzCheckBox.IsChecked ?? false;
        public bool IsUPulseChecked => UPulseCheckBox.IsChecked ?? false;
        public bool IsWarmUpChecked => WarmUp.IsChecked ?? false;
        public bool IsCoolChecked => WarmDown.IsChecked ?? false;
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
        private void SavePowerButton_Click(object sender, RoutedEventArgs e)
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

                    powerSupply.Element("NamePoweUnit")?.SetValue(PowerSupplyNameTextBox.Text);
                    powerSupply.Element("CodePoweUnit")?.SetValue(PowerSupplyCodeTextBox.Text);
                    powerSupply.Element("PIDTemperature")?.SetValue(PowerSupplyNameTextBox.Text);
                    powerSupply.Element("CodePoweUnit")?.SetValue(PowerSupplyCodeTextBox.Text);
                    powerSupply.Element("PIDTemperature")?.SetValue(TempWarkUpModule.Text);
                    powerSupply.Element("PIDTemperatureLimit")?.SetValue(TempWarkUpModuleApprox.Text);
                    powerSupply.Element("TimeCycle")?.SetValue(EnergyDuration.Text);
                    powerSupply.Element("TimeWarming")?.SetValue(WarmUpDuration.Text);
                    powerSupply.Element("TimeNoLoad")?.SetValue(IhhDuration.Text);
                    powerSupply.Element("TimeShortCircuit")?.SetValue(IprotectDuration.Text);
                    powerSupply.Element("TimeProtected")?.SetValue(IkzDuration.Text);
                    powerSupply.Element("TimeRipple")?.SetValue(UPulsDuration.Text);
                    powerSupply.Element("Voltage_PU_EL_Measure_Max")?.SetValue(ValueExitEnergy.Text);
                    powerSupply.Element("Voltage_PU_EL_Measure_Limit")?.SetValue(DeviationValueExitEnergy.Text);
                    powerSupply.Element("Current_PU_Protected_Min")?.SetValue(ProtectionRangeCurrentMin.Text);
                    powerSupply.Element("Current_PU_Protected_Max")?.SetValue(ProtectionRangeCurrentMax.Text);
                    powerSupply.Element("Current_PU_EL_Protected_Min")?.SetValue(ProtectionRangeIncreasingValueMin.Text);
                    powerSupply.Element("Current_PU_EL_Protected_Max")?.SetValue(ProtectionRangeIncreasingValueMax.Text);
                    powerSupply.Element("RippleVoltage")?.SetValue(MaxOutputPulse.Text);
                    powerSupply.Element("Current_Multimetr_Max_PU")?.SetValue(MaxInputCurrent.Text);
                    powerSupply.Element("Current_No_load_Multimetr_Max_PU")?.SetValue(MaxInputHHCurrent.Text);
                    powerSupply.Element("Current_Short_Circuit_Multimetr_Max_PU")?.SetValue(MaxInputKZCurrent.Text);
                    powerSupply.Element("Current_PU_EL_Measure_Max")?.SetValue(MaxValueExitCurrent.Text);
                    powerSupply.Element("Current_EL_Protected")?.SetValue(CurrentLimit.Text);

                    xmlDoc.Save(filePath);
                }
            }
        }
    }
}