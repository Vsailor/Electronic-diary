using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;

namespace ElectronicDiary
{
    partial class MainWindow : Window
    {
        private void ShowAllUsers()
        {
            AdminDataGrid.ItemsSource = (from users in model.Users 
                                         select new { users.Login, users.Password, users.Role }).ToList();
        }
        private void ShowStudents()
        {
            AdminDataGrid.ItemsSource = (from students in model.Students 
                                         join groups in model.Groups on students.Group_Id equals groups.Id 
                                         join users in model.Users on students.User_Id equals users.Id      
                                         select new { Name = students.Name, Surname = students.Surname, Group = groups.Name, Login = users.Login, Password = users.Password} ).ToList();
        }
        private void AdminPanelShow()
        {
            Admin.Visibility = Visibility.Visible;
            ShowAllUsers();
            AdminComboBox.Items.Clear();
            AdminComboBox.Items.Add("All users");
            AdminComboBox.Items.Add("Students");
            AdminComboBox.Items.Add("Teachers");
            AdminComboBox.Items.Add("Groups");
            AdminComboBox.Items.Add("Subjects");
        }

        private void AdminComboBox_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            switch (AdminComboBox.SelectedIndex)
            { 
                case 0:
                    ShowAllUsers();
                    break;
                case 1:
                    ShowStudents();
                    break;
            }
        }
    }
}
