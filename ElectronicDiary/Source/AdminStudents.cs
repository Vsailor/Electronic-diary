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
        private void AdminPanelStudentsAddStudent_Click(object sender, RoutedEventArgs e)
        {
            ShowAdminGrid(AdminStudentsAdd);
            AdminColName.Content = "Add student";
            AdminStudentNameTextBox.Text = String.Empty;
            AdminStudentSurnameTextBox.Text= String.Empty;
            AdminStudentLoginTextBox.Text= String.Empty;
            AdminStudentPassTextBox.Password = String.Empty;
            AdminStudentGroupCombobox.Items.Clear();
            var groups = (from items in model.Groups
                          select items.Name).ToList();
            foreach (var item in groups)
            {
                AdminStudentGroupCombobox.Items.Add(item);
            }

            
        }

        private void AdminPanelStudentsEditStudent_Click(object sender, RoutedEventArgs e)
        {
            ShowAdminGrid(AdminStudentsEdit);
            AdminColName.Content = "Edit student";
            AdminStudentNameEditTextBox.Text = String.Empty;
            AdminStudentSurnameEditTextBox.Text = String.Empty;
            AdminStudentLoginEditTextBox.Text = String.Empty;
            AdminStudentPassEditTextBox.Password = String.Empty;
            AdminStudentsIdCombobox.Items.Clear();
            var ids = (from students in model.Students
                       select students.Id).ToList();
            foreach (var item in ids)
            {
                AdminStudentsIdCombobox.Items.Add(item);
            }
        }

        private void AdminPanelStudentsRemoveStudent_Click(object sender, RoutedEventArgs e)
        {
            ShowAdminGrid(AdminStudentsDelete);
            AdminColName.Content = "Remove student";
            AdminStudentLoginDeleteCombobox.Items.Clear();
            try
            {
                var items = (from students in model.Students
                             select students).ToList();
                foreach (var item in items)
                {
                    AdminStudentLoginDeleteCombobox.Items.Add(item.User.Login);
                }
            }
            catch (Exception ex)
            {
                StatusBar.Content = ex.Message;
            }
        }

        private void AdminPanelBackStudent_Click(object sender, RoutedEventArgs e)
        {
            ShowAdminGrid(AdminStudentsGrid);
            AdminColName.Content = String.Empty;
        }

        private void AdminPanelAddStudent_Click(object sender, RoutedEventArgs e)
        {
            string name = AdminStudentNameTextBox.Text;
            string surname = AdminStudentSurnameTextBox.Text;
            string login = AdminStudentLoginTextBox.Text;
            string pass = AdminStudentPassTextBox.Password;
            string groupName = AdminStudentGroupCombobox.SelectedValue.ToString();
            if (name == String.Empty || surname == String.Empty
                || login == String.Empty || pass == String.Empty || groupName == String.Empty)
            {
                StatusBar.Content = "At least one textbox is emty";
                return;
            }
            var newUser = new User()
            {
                Login = login,
                Password = pass,
                Role = "student"
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
                var dbGroup = (from groups in model.Groups
                               where groups.Name == groupName
                               select groups).FirstOrDefault();
                var student = new Student()
                {
                    Name = name,
                    Surname = surname,
                    User_Id = user.Id,
                    Group_Id = dbGroup.Id
                };
                if (IsExist(student))
                {
                    StatusBar.Content = "This student already exists";
                    return;
                }
                model.Students.Add(student);
                model.SaveChanges();
                ShowStudents();
                AdminStudentNameTextBox.Text = String.Empty;
                AdminStudentSurnameTextBox.Text = String.Empty;
                AdminStudentLoginTextBox.Text = String.Empty;
                AdminStudentPassTextBox.Password = String.Empty;
                AdminPanelStudentsAddStudent_Click(sender, e);
                StatusBar.Content = "Student added";
            }
            catch (Exception ex)
            {
                StatusBar.Content = ex.Message;
            }
        }


        private void AdminPanelBackDelete_Click(object sender, RoutedEventArgs e)
        {
            ShowAdminGrid(AdminStudentsGrid);
            AdminColName.Content = "Remove student";

        }

        private void AdminPanelDelete_Click(object sender, RoutedEventArgs e)
        {
            if (AdminStudentLoginDeleteCombobox.SelectedIndex == -1)
            {
                StatusBar.Content = "Login is not chosen";
                return;
            }
            string login = AdminStudentLoginDeleteCombobox.SelectedValue.ToString();
            var item = (from students in model.Students
                        where students.User.Login == login &&
                        students.User.Role == "student"
                        select students).FirstOrDefault();
            if (item == null)
            {
                StatusBar.Content = "Student not found";
                return;
            }
            try
            {
                model.Students.Remove(item);
                model.SaveChanges();
                ShowStudents();
                ShowAdminGrid(AdminStudentsGrid);
                AdminPanelStudentsRemoveStudent_Click(sender, e);
                AdminColName.Content = String.Empty;
            }
            catch (Exception ex)
            {
                StatusBar.Content = ex.Message;
            }
        }


        private void AdminPanelEditBack_Click(object sender, RoutedEventArgs e)
        {
            ShowAdminGrid(AdminStudentsGrid);
            AdminColName.Content = String.Empty;
            AdminStudentGroupEditCombobox.Items.Clear();
        }

        private void AdminStudentsIdCombobox_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            if (AdminStudentsIdCombobox.SelectedIndex != -1)
            {
                int index = (int)(AdminStudentsIdCombobox.SelectedValue);
                var student = (from students in model.Students
                               where students.Id == index
                               select students).FirstOrDefault();
                AdminStudentNameEditTextBox.Text = student.Name;
                AdminStudentSurnameEditTextBox.Text = student.Surname;
                AdminStudentLoginEditTextBox.Text = student.User.Login;
                AdminStudentPassEditTextBox.Password = String.Empty;
                AdminStudentGroupEditCombobox.Items.Clear();
                var groupList = (from groups in model.Groups
                             select groups.Name).ToList();
                foreach (var item in groupList)
                {
                    AdminStudentGroupEditCombobox.Items.Add(item);
                }
                AdminStudentGroupEditCombobox.SelectedItem = student.Group.Name;
            }
        }
        private void AdminPanelEditSave_Click(object sender, RoutedEventArgs e)
        {
            if (AdminStudentsIdCombobox.SelectedIndex == -1)
            {
                StatusBar.Content = "Id isn't selected";
                return;
            }
            int index = (int)(AdminStudentsIdCombobox.SelectedValue);
            string name = AdminStudentNameEditTextBox.Text;
            string surname = AdminStudentSurnameEditTextBox.Text;
            string login = AdminStudentLoginEditTextBox.Text;
            string pass = AdminStudentPassEditTextBox.Password;
            if (name == String.Empty || surname == String.Empty
                || login == String.Empty || pass == String.Empty)
            {
                StatusBar.Content = "At least one textbox is emty";
                return;
            }
            User student = new User() { Login = login, Password = pass };
            try
            {
                var user = (from students in model.Students
                            where students.Id == index
                            select students).FirstOrDefault();
                if (user.User.Login != login)
                {
                    if (IsExist(student))
                    {
                        StatusBar.Content = "This student already exists";
                        return;
                    }
                }
                user.Name = name;
                user.Surname = surname;
                user.User.Login = login;
                user.User.Password = pass;
                model.SaveChanges();
                StatusBar.Content = "Student saved";
                AdminStudentNameEditTextBox.Text = String.Empty;
                AdminStudentSurnameEditTextBox.Text = String.Empty;
                AdminStudentLoginEditTextBox.Text = String.Empty;
                AdminStudentPassEditTextBox.Password = String.Empty;
                AdminStudentGroupEditCombobox.Items.Clear();
                AdminStudentsIdCombobox.Items.Clear();
                ShowStudents();
                AdminPanelStudentsEditStudent_Click(sender, e);
            }
            catch (Exception ex)
            {
                StatusBar.Content = ex.Message;
            }
        }

    }
}
