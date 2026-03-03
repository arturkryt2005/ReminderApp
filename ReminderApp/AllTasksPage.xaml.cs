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
using System.Windows.Navigation;
using System.Windows.Shapes;

namespace ReminderApp
{
    /// <summary>
    /// Логика взаимодействия для AllTasksPage.xaml
    /// </summary>
    public partial class AllTasksPage : Page
    {
        public AllTasksPage()
        {
            InitializeComponent();
        }
        private void AddTaskButton_Click(object sender, RoutedEventArgs e)
        {
            var addWindow = new AddTaskWindow();
            addWindow.Owner = Window.GetWindow(this); // чтобы окно было модальным к главному
            addWindow.ShowDialog();
            // после закрытия можно обновить список задач
        }
    }
}
