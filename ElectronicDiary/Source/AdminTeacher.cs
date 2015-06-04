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
        private void AdminPanelTeacherAdd(object sender, RoutedEventArgs e)
        {
            ShowAdminGrid(AdminTeachersAdd);
            AdminColName.Content = "Add teacher";
        }
        private void AdminPanelBackTeacher_Click(object sender, RoutedEventArgs e)
        {
            ShowAdminGrid(AdminTeachersGrid);
            AdminColName.Content = String.Empty;
            AdminTeacherNameTextBox.Text = String.Empty;
            AdminTeacherSurnameTextBox.Text = String.Empty;
            AdminTeacherLoginTextBox.Text = String.Empty;
            AdminTeacherPasswordTextBox.Password = String.Empty;

        }

        private void AdminPanelAddTeacher_Click(object sender, RoutedEventArgs e)
        {
            string name = AdminTeacherNameTextBox.Text;
            string surname = AdminTeacherSurnameTextBox.Text;
            string login = AdminTeacherLoginTextBox.Text;
            string pass = AdminTeacherPasswordTextBox.Password;
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
                AdminTeacherNameTextBox.Text = String.Empty;
                AdminTeacherSurnameTextBox.Text = String.Empty;
                AdminTeacherLoginTextBox.Text = String.Empty;
                AdminTeacherPasswordTextBox.Password = String.Empty;
                AdminPanelTeacherAdd(sender, e);
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
            AdminTeacherIdComboboxEdit.Items.Clear();
            var teachers = (from items in model.Teachers
                            select items.Id).ToList();
            foreach (var items in teachers)
            {
                AdminTeacherIdComboboxEdit.Items.Add(items);
            }
        }

        private void AdminTeacherIdComboboxEdit_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            if (AdminTeacherIdComboboxEdit.SelectedIndex != -1)
            {
                int index = (int)(AdminTeacherIdComboboxEdit.SelectedValue);
                var teacher = (from teachers in model.Teachers
                               where teachers.Id == index
                               select teachers).FirstOrDefault();
                AdminTeacherNameTextBoxEdit.Text = teacher.Name;
                AdminTeacherSurnameTextBoxEdit.Text = teacher.Surname;
                AdminTeacherLoginTextBoxEdit.Text = teacher.User.Login;
                AdminTeacherPasswordTextBoxEdit.Password = String.Empty;
            }
        }

        private void AdminPanelBackEditTeacher_Click(object sender, RoutedEventArgs e)
        {
            ShowAdminGrid(AdminTeachersGrid);
            AdminColName.Content = String.Empty;
            AdminTeacherNameTextBoxEdit.Text = String.Empty;
            AdminTeacherSurnameTextBoxEdit.Text = String.Empty;
            AdminTeacherLoginTextBoxEdit.Text = String.Empty;
            AdminTeacherPasswordTextBoxEdit.Password = String.Empty;
        }

        private void AdminPanelSaveTeacher_Click(object sender, RoutedEventArgs e)
        {
            if (AdminTeacherIdComboboxEdit.SelectedIndex == -1)
            {
                StatusBar.Content = "Id isn't selected";
                return;
            }
            int index = (int)(AdminTeacherIdComboboxEdit.SelectedValue);
            string name = AdminTeacherNameTextBoxEdit.Text;
            string surname = AdminTeacherSurnameTextBoxEdit.Text;
            string login = AdminTeacherLoginTextBoxEdit.Text;
            string pass = AdminTeacherPasswordTextBoxEdit.Password;
            if (name == String.Empty || surname == String.Empty
                || login == String.Empty || pass == String.Empty)
            {
                StatusBar.Content = "At least one textbox is emty";
                return;
            }
            User teacher = new User() { Login = login, Password = pass };
            try
            {
                var user = (from teachers in model.Teachers
                            where teachers.Id == index
                            select teachers).FirstOrDefault();
                if (user.User.Login != login)
                {
                    if (IsExist(teacher))
                    {
                        StatusBar.Content = "This teacher already exists";
                        return;  
                    }
                }
                user.Name = name;
                user.Surname = surname;
                user.User.Login = login;
                user.User.Password = pass;
                model.SaveChanges();
                StatusBar.Content = "Teacher saved";
                AdminTeacherNameTextBoxEdit.Text = String.Empty;
                AdminTeacherSurnameTextBoxEdit.Text = String.Empty;
                AdminTeacherLoginTextBoxEdit.Text = String.Empty;
                AdminTeacherPasswordTextBoxEdit.Password = String.Empty;
                ShowTeachers();
                AdminPanelTeacherEdit(sender, e);
            }
            catch (Exception ex)
            {
                StatusBar.Content = ex.Message;
            }
        }
        private void AdminPanelTeacherRemove(object sender, RoutedEventArgs e)
        {
            ShowAdminGrid(AdminTeachersRemove);
            AdminColName.Content = "Remove teacher";
            AdminTeacherLoginDeleteCombobox.Items.Clear();
            try
            {
                var items = (from teachers in model.Teachers
                             select teachers).ToList();
                foreach (var item in items)
                {
                    AdminTeacherLoginDeleteCombobox.Items.Add(item.User.Login);
                }
            }
            catch (Exception ex)
            {
                StatusBar.Content = ex.Message;
            }

        }



        private void AdminPanelBackDeleteTeacher_Click(object sender, RoutedEventArgs e)
        {
            ShowAdminGrid(AdminTeachersGrid);
            AdminColName.Content = String.Empty;
        }

        private void AdminPanelDeleteTeacher_Click(object sender, RoutedEventArgs e)
        {
            if (AdminTeacherLoginDeleteCombobox.SelectedIndex == -1)
            {
                StatusBar.Content = "Login is not chosen";
                return;
            }
            string login = AdminTeacherLoginDeleteCombobox.SelectedValue.ToString();
            var item = (from teachers in model.Teachers
                        where teachers.User.Login == login &&
                        teachers.User.Role == "teacher"
                        select teachers).FirstOrDefault();
            if (item == null)
            {
                StatusBar.Content = "Teacher not found";
                return;
            }
            try
            {
                model.Users.Remove(item.User);
                model.Teachers.Remove(item);               
                model.SaveChanges();
                ShowTeachers();
                ShowAdminGrid(AdminTeachersGrid);
                AdminPanelTeacherRemove(sender, e);
                AdminColName.Content = String.Empty;
            }
            catch (Exception ex)
            {
                StatusBar.Content = ex.Message;
            }
        }
    }
}
