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
        public Brush IndicatorEnergyReport
        {
            get { return (Brush)GetValue(IndicatorEnergyReportProperty); }
            set { SetValue(IndicatorEnergyReportProperty, value); }
        }
        public static readonly DependencyProperty IndicatorEnergyReportProperty =
            DependencyProperty.Register("IndicatorEnergyReport", typeof(Brush), typeof(SectionControl), new PropertyMetadata(Brushes.Gray));

        public Brush IndicatorIhhReport
        {
            get { return (Brush)GetValue(IndicatorIhhReportProperty); }
            set { SetValue(IndicatorIhhReportProperty, value); }
        }
        public static readonly DependencyProperty IndicatorIhhReportProperty =
            DependencyProperty.Register("IndicatorIhhReport", typeof(Brush), typeof(SectionControl), new PropertyMetadata(Brushes.Gray));

        public Brush IndicatorIprotReport
        {
            get { return (Brush)GetValue(IndicatorIprotReportProperty); }
            set { SetValue(IndicatorIprotReportProperty, value); }
        }
        public static readonly DependencyProperty IndicatorIprotReportProperty =
            DependencyProperty.Register("IndicatorIprotReport", typeof(Brush), typeof(SectionControl), new PropertyMetadata(Brushes.Gray));

        public Brush IndicatorIkzReport
        {
            get { return (Brush)GetValue(IndicatorIkzReportProperty); }
            set { SetValue(IndicatorIkzReportProperty, value); }
        }
        public static readonly DependencyProperty IndicatorIkzReportProperty =
            DependencyProperty.Register("IndicatorIkzReport", typeof(Brush), typeof(SectionControl), new PropertyMetadata(Brushes.Gray));

        public Brush IndicatorUpulsReport
        {
            get { return (Brush)GetValue(IndicatorUpulsReportProperty); }
            set { SetValue(IndicatorUpulsReportProperty, value); }
        }
        public static readonly DependencyProperty IndicatorUpulsReportProperty =
            DependencyProperty.Register("IndicatorUpulsReport", typeof(Brush), typeof(SectionControl), new PropertyMetadata(Brushes.Gray));

        public void SetIndicatorColor(string testName, Brush color)
        {
            switch (testName)
            {
                case "EnergyCycle":
                    SetValue(IndicatorEnergyReportProperty, color);
                    break;
                case "Ihh":
                    SetValue(IndicatorIhhReportProperty, color);
                    break;
                case "Iprotect":
                    SetValue(IndicatorIprotReportProperty, color);
                    break;
                case "Ikz":
                    SetValue(IndicatorIkzReportProperty, color);
                    break;
                case "Upulse":
                    SetValue(IndicatorUpulsReportProperty, color);
                    break;
            }
        }
    }
}
