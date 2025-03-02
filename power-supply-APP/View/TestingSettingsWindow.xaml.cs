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
using System.Windows.Shapes;

namespace power_supply_APP
{
    /// <summary>
    /// Логика взаимодействия для TestingSettingsWindow.xaml
    /// </summary>
    public partial class TestingSettingsWindow : Window
    {
        private TestPage _testPage;
        public string InputText { get; set; }
        public string SerialText { get; set; }
        public int SectionIndex { get; set; }

        public TestingSettingsWindow(TestPage testPage)
        {
            InitializeComponent();
            _testPage = testPage;
        }

        private void TestingOkButton_Click(object sender, RoutedEventArgs e)
        {
            InputText = DeviceBox.Text;
            SerialText = SerialNumberBox.Text;
            this.DialogResult = true; // Указываем, что окно закрывается успешно

            List<string> selectedTests = GetSelectedTests();

            // Теперь передаём индекс секции
            _testPage.UpdateSelectedTests(SectionIndex, selectedTests);

            this.Close();
        }
        private void TestingCloseButton_Click(Object sender, RoutedEventArgs e)
        {
            this.DialogResult = true; // Указываем, что окно закрывается успешно
            this.Close();
        }

        private List<string> GetSelectedTests()
        {
            List<string> tests = new List<string>();

            if (CheckBox_EnergyCycle.IsChecked == true) tests.Add("EnergyCycle");
            if (CheckBox_Ihh.IsChecked == true) tests.Add("Ihh");
            if (CheckBox_Iprotect.IsChecked == true) tests.Add("Iprotect");
            if (CheckBox_Ikz.IsChecked == true) tests.Add("Ikz");
            if (CheckBox_UPulse.IsChecked == true) tests.Add("Upulse");

            return tests;
        }
        public void SetCheckBoxStates(bool isEnergyCycleChecked, bool isIhhChecked, bool isIprotectChecked,
            bool isIkzChecked, bool isUPulseChecked, bool isWarmUpChecked, bool isCoolChecked)
        {
            Console.WriteLine($"EnergyCycle: {isEnergyCycleChecked}, WarmUp: {isWarmUpChecked}");
            CheckBox_EnergyCycle.IsChecked = isEnergyCycleChecked;
            CheckBox_Ihh.IsChecked = isIhhChecked;
            CheckBox_Iprotect.IsChecked = isIprotectChecked;
            CheckBox_Ikz.IsChecked = isIkzChecked;
            CheckBox_UPulse.IsChecked = isUPulseChecked;
            CheckBox_WarmUp.IsChecked = isWarmUpChecked;
            CheckBox_Cool.IsChecked = isCoolChecked;
        }
    }
}
