using System.Text;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;

namespace ReminderApp
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        public MainWindow()
        {
            InitializeComponent();
            MainFrame.Navigate(new TodayPage());
        }

        private void NavigationListBox_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            // Защита от вызова до инициализации Frame
            if (MainFrame == null) return;

            if (NavigationListBox.SelectedItem is ListBoxItem item && item.Tag is string pageName)
            {
                // Избегаем повторной навигации на ту же страницу
                var currentPage = MainFrame.Content as Page;
                if (currentPage != null && currentPage.Title == pageName) return;

                switch (pageName)
                {
                    case "TodayPage":
                        MainFrame.Navigate(new TodayPage());
                        break;
                    case "AllTasksPage":
                        MainFrame.Navigate(new AllTasksPage());
                        break;
                    case "RemindersPage":
                        MainFrame.Navigate(new RemindersPage());
                        break;
                    default:
                        MainFrame.Navigate(new TodayPage());
                        break;
                }
            }
        }
    }
}