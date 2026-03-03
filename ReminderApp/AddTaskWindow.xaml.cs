using System;
using System.Collections.Generic;
using System.Linq;
using System.Text.RegularExpressions;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;
using System.Windows.Media;

namespace ReminderApp
{
    public partial class AddTaskWindow : Window
    {
        public bool IsSaved { get; private set; }
        public string TaskTitle { get; private set; }
        public DateTime? TaskDate { get; private set; }
        public string TaskTime { get; private set; }

        public AddTaskWindow()
        {
            InitializeComponent();

            TaskDateDatePicker.DisplayDateStart = DateTime.Today;
            TaskDateDatePicker.SelectedDate = DateTime.Today;

            // Заполняем ComboBox временем от 00:00 до 23:50 с шагом 10 минут
            var timeList = new List<string>();
            for (int hour = 0; hour < 24; hour++)
            {
                for (int minute = 0; minute < 60; minute += 10)
                {
                    timeList.Add($"{hour:D2}:{minute:D2}");
                }
            }
            TaskTimeComboBox.ItemsSource = timeList;
            TaskTimeComboBox.SelectedIndex = 72; // 12:00
        }

        // ---------- DatePicker ----------
        private void TaskDateDatePicker_Loaded(object sender, RoutedEventArgs e)
        {
            var textBox = TaskDateDatePicker.Template.FindName("PART_TextBox", TaskDateDatePicker) as TextBox;
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
                    TaskDateDatePicker.SelectedDate = date;
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
                    TaskDateDatePicker.SelectedDate = date;
                }
            }
        }

        // ---------- ComboBox времени ----------
        private void TaskTimeComboBox_Loaded(object sender, RoutedEventArgs e)
        {
            var comboBox = sender as ComboBox;
            if (comboBox == null) return;
            var textBox = comboBox.Template.FindName("PART_EditableTextBox", comboBox) as TextBox;
            if (textBox != null)
            {
                textBox.MaxLength = 5;
                textBox.PreviewTextInput += TaskTimeComboBox_PreviewTextInput;
                textBox.TextChanged += TaskTimeComboBox_TextChanged;
                textBox.LostFocus += TaskTimeComboBox_LostFocus;
                DataObject.AddPastingHandler(textBox, TaskTimeComboBox_OnPaste);
            }
        }

        private void TaskTimeComboBox_PreviewTextInput(object sender, TextCompositionEventArgs e)
        {
            e.Handled = !IsTextAllowed(e.Text);
        }

        private void TaskTimeComboBox_OnPaste(object sender, DataObjectPastingEventArgs e)
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

        private void TaskTimeComboBox_TextChanged(object sender, TextChangedEventArgs e)
        {
            var textBox = sender as TextBox;
            if (textBox == null) return;

            string digits = GetDigits(textBox.Text);
            if (digits.Length == 4)
            {
                string formattedTime = $"{digits.Substring(0, 2)}:{digits.Substring(2, 2)}";
                if (IsValidTime(formattedTime))
                {
                    textBox.TextChanged -= TaskTimeComboBox_TextChanged;
                    textBox.Text = formattedTime;
                    textBox.CaretIndex = formattedTime.Length;
                    textBox.TextChanged += TaskTimeComboBox_TextChanged;
                }
            }
        }

        private void TaskTimeComboBox_LostFocus(object sender, RoutedEventArgs e)
        {
            var textBox = sender as TextBox;
            if (textBox == null) return;

            string digits = GetDigits(textBox.Text);
            if (digits.Length == 4)
            {
                string formattedTime = $"{digits.Substring(0, 2)}:{digits.Substring(2, 2)}";
                if (IsValidTime(formattedTime))
                {
                    textBox.TextChanged -= TaskTimeComboBox_TextChanged;
                    textBox.Text = formattedTime;
                    textBox.TextChanged += TaskTimeComboBox_TextChanged;
                }
            }
        }

        // ---------- Вспомогательные методы ----------
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

        // ---------- Кнопки ----------
        private void CancelButton_Click(object sender, RoutedEventArgs e)
        {
            IsSaved = false;
            Close();
        }

        private void SaveButton_Click(object sender, RoutedEventArgs e)
        {
            if (string.IsNullOrWhiteSpace(TaskTitleTextBox.Text))
            {
                MessageBox.Show("Введите название задачи", "Ошибка", MessageBoxButton.OK, MessageBoxImage.Warning);
                return;
            }

            if (TaskDateDatePicker.SelectedDate == null)
            {
                MessageBox.Show("Выберите дату", "Ошибка", MessageBoxButton.OK, MessageBoxImage.Warning);
                return;
            }

            string timeText = TaskTimeComboBox.Text;
            string digits = GetDigits(timeText);
            string formattedTime = null;

            if (digits.Length == 4)
            {
                formattedTime = $"{digits.Substring(0, 2)}:{digits.Substring(2, 2)}";
                if (!IsValidTime(formattedTime))
                {
                    MessageBox.Show("Введите корректное время в формате чч:мм", "Ошибка", MessageBoxButton.OK, MessageBoxImage.Warning);
                    return;
                }
            }
            else if (TaskTimeComboBox.SelectedItem != null)
            {
                formattedTime = TaskTimeComboBox.SelectedItem.ToString();
            }
            else
            {
                MessageBox.Show("Выберите или введите время", "Ошибка", MessageBoxButton.OK, MessageBoxImage.Warning);
                return;
            }

            TaskTitle = TaskTitleTextBox.Text;
            TaskDate = TaskDateDatePicker.SelectedDate;
            TaskTime = formattedTime;
            IsSaved = true;
            Close();
        }

        private void Window_Loaded(object sender, RoutedEventArgs e) { }
    }
}