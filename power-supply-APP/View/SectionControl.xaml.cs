using System;
using System.Collections.Generic;
using System.Diagnostics;
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

namespace power_supply_APP.View
{
    /// <summary>
    /// Логика взаимодействия для SectionControl.xaml
    /// </summary>
    public partial class SectionControl : UserControl
    {
        public SectionControl()
        {
            InitializeComponent();
        }
        public void UpdateTextBlock(double value)
        {
            Dispatcher.Invoke(() =>
            {
                var textBlock = this.FindName("I_out") as TextBlock;
                if (textBlock != null)
                {
                    textBlock.Text = value.ToString("F2");
                    Debug.WriteLine($"Обновлён TextBlock: {textBlock.Text}");
                }
                else
                {
                    Debug.WriteLine("TextBlock с Tag='I_out' не найден!");
                }
            });
        }
        public void UpdateVoltageTextBlock(double value)
        {
            Dispatcher.Invoke(() =>
            {
                var textBlock = this.FindName("U_out") as TextBlock;
                if (textBlock != null)
                {
                    textBlock.Text = value.ToString("F2");
                    Debug.WriteLine($"Обновлён TextBlock напряжения: {textBlock.Text}");
                }
                else
                {
                    Debug.WriteLine("TextBlock с Tag='U_out' не найден!");
                }
            });
        }
    }
}
