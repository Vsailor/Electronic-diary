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

namespace ElectronicDiary
{
    /// <summary>
    /// Логика взаимодействия для MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        ElectronicDiaryDBEntities model;
        public MainWindow()
        {
            InitializeComponent();
            model=new ElectronicDiaryDBEntities();
            //AdminPanelShow();
            Admin.Visibility = Visibility.Hidden;
            Client.Visibility=Visibility.Hidden;
            Logining.Visibility = Visibility.Visible;
            Exit.Visibility = Visibility.Hidden;
        }

        private void logout(object sender, RoutedEventArgs e)
        {
            Admin.Visibility = Visibility.Hidden;
            Client.Visibility = Visibility.Hidden;
            Logining.Visibility = Visibility.Visible;
            Exit.Visibility = Visibility.Hidden;
            Login.Text = "";
            Password.Password = "";
        }



    }
}
