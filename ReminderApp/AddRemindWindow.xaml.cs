using System;
using System.Collections.Generic;
using System.Linq;
using System.Text.RegularExpressions;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;

namespace ReminderApp
{
    /// <summary>
    /// Логика взаимодействия для AddRemindWindow.xaml
    /// </summary>
    public partial class AddRemindWindow : Window
    {
        // Свойства для возврата результата
        public string RemindTitle { get; private set; }
        public DateTime RemindDateTime { get; private set; }
        public bool IsWeekly { get; private set; }
        public List<DayOfWeek> SelectedDays { get; private set; }

        public AddRemindWindow()
        {
            InitializeComponent();
            RemindDateDatePicker.DisplayDateStart = DateTime.Today;
            RemindDateDatePicker.SelectedDate = DateTime.Today;
        }

        private void Window_Loaded(object sender, RoutedEventArgs e)
        {
            RemindTitleTextBox.Focus();
        }

        private void RemindDateDatePicker_Loaded(object sender, RoutedEventArgs e)
        {
            var textBox = RemindDateDatePicker.Template.FindName("PART_TextBox", RemindDateDatePicker) as TextBox;
            if (textBox != null)
            {
                textBox.PreviewTextInput += DatePickerTextBox_PreviewTextInput;
                textBox.TextChanged += DatePickerTextBox_TextChanged;
                textBox.LostFocus += DatePickerTextBox_LostFocus;
                DataObject.AddPastingHandler(textBox, DatePickerTextBox_OnPaste);
            }
        }
        private void DatePickerTextBox_PreviewTextInput(object sender, TextCompositionEventArgs e)
        {
            e.Handled = !IsTextAllowed(e.Text);
        }

        private void DatePickerTextBox_OnPaste(object sender, DataObjectPastingEventArgs e)
        {
            if (e.DataObject.GetDataPresent(DataFormats.Text))
            {
                string text = e.DataObject.GetData(DataFormats.Text) as string;
                if (!IsTextAllowed(text))
                    e.CancelCommand();
            }
            else
            {
                e.CancelCommand();
            }
        }

        private void DatePickerTextBox_TextChanged(object sender, TextChangedEventArgs e)
        {
            var textBox = sender as TextBox;
            if (textBox == null) return;

            string digits = GetDigits(textBox.Text);
            if (digits.Length == 8)
            {
                string formattedDate = $"{digits.Substring(0, 2)}.{digits.Substring(2, 2)}.{digits.Substring(4, 4)}";
                if (DateTime.TryParseExact(formattedDate, "dd.MM.yyyy", null, System.Globalization.DateTimeStyles.None, out DateTime date))
                {
                    textBox.TextChanged -= DatePickerTextBox_TextChanged;
                    textBox.Text = formattedDate;
                    textBox.CaretIndex = formattedDate.Length;
                    textBox.TextChanged += DatePickerTextBox_TextChanged;
                    RemindDateDatePicker.SelectedDate = date;
                }
            }
        }

        private void DatePickerTextBox_LostFocus(object sender, RoutedEventArgs e)
        {
            var textBox = sender as TextBox;
            if (textBox == null) return;

            string digits = GetDigits(textBox.Text);
            if (digits.Length == 8)
            {
                string formattedDate = $"{digits.Substring(0, 2)}.{digits.Substring(2, 2)}.{digits.Substring(4, 4)}";
                if (DateTime.TryParseExact(formattedDate, "dd.MM.yyyy", null, System.Globalization.DateTimeStyles.None, out DateTime date))
                {
                    textBox.TextChanged -= DatePickerTextBox_TextChanged;
                    textBox.Text = formattedDate;
                    textBox.TextChanged += DatePickerTextBox_TextChanged;
                    RemindDateDatePicker.SelectedDate = date;
                }
            }
        }
        private void RemindTimeComboBox_Loaded(object sender, RoutedEventArgs e)
        {
            // Генерируем временные слоты с шагом 30 минут
            var times = new List<string>();
            for (int hour = 0; hour < 24; hour++)
            {
                times.Add($"{hour:00}:00");
                times.Add($"{hour:00}:30");
            }
            RemindTimeComboBox.ItemsSource = times;
            RemindTimeComboBox.Text = DateTime.Now.ToString("HH:mm");
        }

        private void CancelButton_Click(object sender, RoutedEventArgs e)
        {
            DialogResult = false;
            Close();
        }

        private void SaveButton_Click(object sender, RoutedEventArgs e)
        {
            // 1. Проверка названия
            if (string.IsNullOrWhiteSpace(RemindTitleTextBox.Text))
            {
                MessageBox.Show("Введите название напоминания", "Ошибка", MessageBoxButton.OK, MessageBoxImage.Warning);
                RemindTitleTextBox.Focus();
                return;
            }

            // 2. Проверка даты
            if (!RemindDateDatePicker.SelectedDate.HasValue)
            {
                MessageBox.Show("Выберите дату", "Ошибка", MessageBoxButton.OK, MessageBoxImage.Warning);
                RemindDateDatePicker.Focus();
                return;
            }

            // 3. Проверка времени
            string timeText = RemindTimeComboBox.Text;
            if (!System.Text.RegularExpressions.Regex.IsMatch(timeText, @"^([01]?\d|2[0-3]):[0-5]\d$"))
            {
                MessageBox.Show("Введите время в формате чч:мм (например, 14:30)", "Ошибка", MessageBoxButton.OK, MessageBoxImage.Warning);
                RemindTimeComboBox.Focus();
                return;
            }

            var timeParts = timeText.Split(':');
            int hour = int.Parse(timeParts[0]);
            int minute = int.Parse(timeParts[1]);

            DateTime selectedDate = RemindDateDatePicker.SelectedDate.Value;
            DateTime fullDateTime = new DateTime(selectedDate.Year, selectedDate.Month, selectedDate.Day, hour, minute, 0);

            // 4. Определяем тип повтора
            bool isWeekly = WeekDaysRadio.IsChecked == true;
            List<DayOfWeek> selectedDays = new List<DayOfWeek>();

            if (isWeekly)
            {
                // Собираем отмеченные дни
                foreach (var child in DaysPanel.Children)
                {
                    if (child is CheckBox checkBox && checkBox.IsChecked == true && checkBox.Tag is string dayString)
                    {
                        if (Enum.TryParse(dayString, out DayOfWeek day))
                            selectedDays.Add(day);
                    }
                }

                if (selectedDays.Count == 0)
                {
                    MessageBox.Show("Выберите хотя бы один день недели", "Ошибка", MessageBoxButton.OK, MessageBoxImage.Warning);
                    return;
                }
            }

            // Заполняем свойства результата
            RemindTitle = RemindTitleTextBox.Text.Trim();
            RemindDateTime = fullDateTime;
            IsWeekly = isWeekly;
            SelectedDays = selectedDays;

            DialogResult = true;
            Close();
        }
        private bool IsTextAllowed(string text)
        {
            return Regex.IsMatch(text, "^[0-9]*$");
        }
        private string GetDigits(string input)
        {
            return new string(input.Where(char.IsDigit).ToArray());
        }

        private bool IsValidTime(string time)
        {
            return TimeSpan.TryParseExact(time, @"hh\:mm", null, out _);
        }
    }
}