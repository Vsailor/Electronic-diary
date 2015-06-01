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
        private void AdminPanelTeacherAdd(object sender, RoutedEventArgs e)
        {
            ShowAdminGrid(AdminTeachersAdd);
            AdminColName.Content = "Add teacher";
        }
        private void AdminPanelBackTeacher_Click(object sender, RoutedEventArgs e)
        {
            ShowAdminGrid(AdminTeachersGrid);
            AdminColName.Content = String.Empty;
        }

        private void AdminPanelAddTeacher_Click(object sender, RoutedEventArgs e)
        {
            string name = AdminTeacherNameTextBox.Text;
            string surname = AdminTeacherSurnameTextBox.Text;
            string login = AdminTeacherLoginTextBox.Text;
            string pass = AdminTeacherPasswordTextBox.Text;
            if (name == String.Empty || surname == String.Empty
                || login == String.Empty || pass == String.Empty)
            {
                StatusBar.Content = "At least one textbox is emty";
                return;
            }
            var newUser = new User()
            {
                Login = login,
                Password = pass,
                Role = "teacher"
            };
            try
            {
                if (IsExist(newUser))
                {
                    StatusBar.Content = "This user already exists";
                    return;
                }
                model.Users.Add(newUser);
                model.SaveChanges();
                var user = (from users in model.Users
                            where users.Login == newUser.Login
                            select users).FirstOrDefault();
                var teacher = new Teacher()
                {
                    Name = name,
                    Surname = surname,
                    UserId = user.Id
                };
                if (IsExist(teacher))
                {
                    StatusBar.Content = "This teacher already exists";
                    return;
                }
                model.Teachers.Add(teacher);
                model.SaveChanges();
                ShowTeachers();
                StatusBar.Content = "Teacher added";
            }
            catch (Exception ex)
            {
                StatusBar.Content = ex.Message;
            }
        }

        private void AdminPanelTeacherEdit(object sender, RoutedEventArgs e)
        {
            ShowAdminGrid(AdminTeachersEdit);
            AdminColName.Content = "Edit teacher";
        }
        private void AdminPanelBackEditTeacher_Click(object sender, RoutedEventArgs e)
        {
            ShowAdminGrid(AdminTeachersGrid);
            AdminColName.Content = String.Empty;
        }

        private void AdminPanelSaveTeacher_Click(object sender, RoutedEventArgs e)
        {
            // Editing teacher
        }
        private void AdminPanelTeacherRemove(object sender, RoutedEventArgs e)
        {
            ShowAdminGrid(AdminTeachersRemove);
            AdminColName.Content = "Remove teacher";
        }


        private void AdminPanelBackDeleteTeacher_Click(object sender, RoutedEventArgs e)
        {
            ShowAdminGrid(AdminTeachersGrid);
            AdminColName.Content = String.Empty;
        }

        private void AdminPanelDeleteTeacher_Click(object sender, RoutedEventArgs e)
        {
            // Removing teacher

        }
    }
}
