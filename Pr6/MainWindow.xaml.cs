using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.IO;
using System.Linq;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;
using System.Xml;
using Newtonsoft.Json;

namespace Pr6
{
    /// <summary>
    /// Логика взаимодействия для MainWindow.xaml
    /// </summary>
    public partial class MainWindow
    {
        private int dayIndex;
        private CalendarViewModel _viewModel;
        private TextBox _dayText;

        public MainWindow()
        {
            InitializeComponent();
            _viewModel = new CalendarViewModel();
            DataContext = _viewModel;
        }

        private void PreviousMonthButton_Click(object sender, RoutedEventArgs e)
        {
            _viewModel.PreviousMonthButton_Click(sender, e);
        }

        private void NextMonthButton_Click(object sender, RoutedEventArgs e)
        {
            _viewModel.NextMonthButton_Click(sender, e);
        }

        public void DayButton_Click(object sender, RoutedEventArgs e)
        {
            int dayIndex = Convert.ToInt32((sender as Button).Content.ToString());

            _dayText = new TextBox();
            _dayText.Text = GetDayText(dayIndex);
            _dayText.Focus();
        }

        private void SaveButton_Click(object sender, RoutedEventArgs e)
        {
            string dayText = _dayText.Text;

            AddDayText(dayText, dayIndex);

            _dayText = null;
        }

        private string GetDayText(int dayIndex)
        {
            string dayText = "";


            return dayText;
        }

        private void AddDayText(string dayText, int dayIndex)
        {
            string filePath = @"C:\Users\Matt\Desktop\техникум\2-й курс\2-й семестр\ОАИП - Скорогудаева\Сalendar\logs.json";
            string jsonData = File.ReadAllText(filePath);

            if (string.IsNullOrEmpty(jsonData))
            {
                jsonData = "{}";
            }

            dynamic jsonObject = JsonConvert.DeserializeObject(jsonData);
            jsonObject[dayIndex.ToString("dd.MM.yy")] = dayText;

            File.WriteAllText(filePath, JsonConvert.SerializeObject(jsonObject, Newtonsoft.Json.Formatting.Indented));
        }

    }

    public class CalendarViewModel : INotifyPropertyChanged
    {
        private int _currentMonth;
        public int CurrentMonth
        {
            get { return _currentMonth; }
            set
            {
                _currentMonth = value;
                OnPropertyChanged(nameof(CurrentMonth));
                UpdateDayButtons(_currentMonth, _currentYear);
            }
        }

        private int _currentYear;
        public int CurrentYear
        {
            get { return _currentYear; }
            set
            {
                _currentYear = value;
                OnPropertyChanged(nameof(CurrentYear));
                UpdateDayButtons(_currentMonth, _currentYear);
            }
        }

        private ObservableCollection<Button> _dayButtons;
        public ObservableCollection<Button> DayButtons
        {
            get { return _dayButtons; }
            set
            {
                _dayButtons = value;
                OnPropertyChanged(nameof(DayButtons));
            }
        }

        public CalendarViewModel()
        {
            _currentMonth = DateTime.Now.Month;
            _currentYear = DateTime.Now.Year;
            UpdateDayButtons(_currentMonth, _currentYear);
        }

        private void UpdateDayButtons(int month, int year)
        {
            int daysInMonth = DateTime.DaysInMonth(year, month);
            DayButtons = new ObservableCollection<Button>();

            int rows = (int)Math.Ceiling((double)daysInMonth / 8);

            for (int i = 1; i <= daysInMonth; i++)
            {
                Button button = new Button();
                button.Content = i.ToString();
                button.Click += (sender, e) => DayButton_Click(sender, e);
                DayButtons.Add(button);
            }
        }

        public void PreviousMonthButton_Click(object sender, RoutedEventArgs e)
        {
            if (CurrentMonth - 1 < 1)
            {
                CurrentMonth = 12;
                CurrentYear--;
            }
            else
            {
                CurrentMonth--;
            }
        }

        public void NextMonthButton_Click(object sender, RoutedEventArgs e)
        {
            if (CurrentMonth + 1 > 12)
            {
                CurrentMonth = 1;
                CurrentYear++;
            }
            else
            {
                CurrentMonth++;
            }
        }

        public event PropertyChangedEventHandler PropertyChanged;

        protected virtual void OnPropertyChanged(string propertyName)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }
        private void DayButton_Click(object sender, RoutedEventArgs e)
        {
            int dayIndex = Convert.ToInt32((sender as Button).Content.ToString());

            ((MainWindow)Application.Current.MainWindow).DayButton_Click(sender, e);
        }
    }

    public class DaySelection
    {
        public DateTime Date { get; set; }

        public event PropertyChangedEventHandler PropertyChanged;
    }

    public class Day
    {
        public string Number { get; set; }
        public string Description { get; set; }
    }
}
