using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;

namespace ElectronicDiary
{
    partial class MainWindow : Window
    {
        private void AdminPanelStudentsAddStudent_Click(object sender, RoutedEventArgs e)
        {
            ShowAdminGrid(AdminStudentsAdd);
            AdminColName.Content = "Add student";
        }

        private void AdminPanelStudentsEditStudent_Click(object sender, RoutedEventArgs e)
        {
            ShowAdminGrid(AdminStudentsEdit);
            AdminColName.Content = "Edit student";
        }

        private void AdminPanelStudentsRemoveStudent_Click(object sender, RoutedEventArgs e)
        {
            ShowAdminGrid(AdminStudentsDelete);
            AdminColName.Content = "Remove student";
        }

        private void AdminPanelBackStudent_Click(object sender, RoutedEventArgs e)
        {
            ShowAdminGrid(AdminStudentsGrid);
            AdminColName.Content = String.Empty;
        }

        private void AdminPanelAddStudent_Click(object sender, RoutedEventArgs e)
        {
            // Adding student
        }


        private void AdminPanelBackDelete_Click(object sender, RoutedEventArgs e)
        {
            ShowAdminGrid(AdminStudentsGrid);
            AdminColName.Content = String.Empty;
        }

        private void AdminPanelDelete_Click(object sender, RoutedEventArgs e)
        {
            // Deleting student
        }


        private void AdminPanelEditBack_Click(object sender, RoutedEventArgs e)
        {
            ShowAdminGrid(AdminStudentsGrid);
            AdminColName.Content = String.Empty;
        }

        private void AdminPanelEditSave_Click(object sender, RoutedEventArgs e)
        {
            // Editing student
        }

    }
}
