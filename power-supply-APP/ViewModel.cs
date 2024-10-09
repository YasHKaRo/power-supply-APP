using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Input;

namespace power_supply_APP
{
    public class MainViewModel : INotifyPropertyChanged
    {
        private string _currentTheme;

        public string CurrentTheme
        {
            get => _currentTheme;
            set
            {
                if (_currentTheme != value)
                {
                    _currentTheme = value;
                    OnPropertyChanged(nameof(CurrentTheme));
                }
            }
        }

        public ICommand ToggleThemeCommand { get; }

        public MainViewModel()
        {
            // Установите начальную тему
            CurrentTheme = "DarkTheme.xaml"; // или "LightTheme.xaml"
            ToggleThemeCommand = new RelayCommand(ToggleTheme);
        }

        private void ToggleTheme()
        {
            // Переключение между темами
            CurrentTheme = CurrentTheme == "DarkTheme.xaml" ? "LightTheme.xaml" : "DarkTheme.xaml";
        }

        public event PropertyChangedEventHandler PropertyChanged;

        protected void OnPropertyChanged(string propertyName)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }
    }
}
