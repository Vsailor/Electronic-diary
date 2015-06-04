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
        private List<Grid> AdminGrids;
        private void AdminPanelShow()
        {
            StatusBar.Content = "Administration panel";
            Admin.Visibility = Visibility.Visible;
            AdminGrids = new List<Grid>();
            AdminGrids.Add(AdminStudentsGrid);
            AdminGrids.Add(AdminSubjectsGrid);
            AdminGrids.Add(AdminGroupsGrid);
            AdminGrids.Add(AdminStudentsAdd);
            AdminGrids.Add(AdminStudentsDelete);
            AdminGrids.Add(AdminStudentsEdit);
            AdminGrids.Add(AdminGroupsGrid);
            AdminGrids.Add(AdminGroupsAdd);
            AdminGrids.Add(AdminGroupsEdit);
            AdminGrids.Add(AdminGroupsRemove);
            AdminGrids.Add(AdminTeachersGrid);
            AdminGrids.Add(AdminTeachersAdd);
            AdminGrids.Add(AdminTeachersRemove);
            AdminGrids.Add(AdminTeachersEdit);
            AdminGrids.Add(AdminSchedulesGrid);
            AdminGrids.Add(AdminSchedulesPanel);
            AdminGrids.Add(EditScheduleForm);
            AdminGrids.Add(AddScheduleForm);
            AdminGrids.Add(RemoveScheduleForm);
            AdminGrids.Add(EditScheduleButtonsForm);
            ShowAdminGrid(null);
            ShowAllUsers();
            AdminComboBox.Items.Clear();
            AdminComboBox.Items.Add("All users");
            AdminComboBox.Items.Add("Students");
            AdminComboBox.Items.Add("Teachers");
            AdminComboBox.Items.Add("Groups");
            AdminComboBox.Items.Add("Subjects");
            AdminComboBox.Items.Add("Schedules");
        }
        private void ShowAdminGrid(params Grid[] g)
        {
            foreach (Grid item in AdminGrids)
            {
                item.Visibility = Visibility.Hidden;
            }
            if (g != null)
            {
                foreach (Grid item in g)
                {
                    item.Visibility = Visibility.Visible;
                }
            }
        }


        private void ShowAllUsers()
        {
            AdminHeaderLabel.Content = "All users";
            AdminDataGrid.ItemsSource = (from users in model.Users
                                         select new { users.Login, users.Role }).ToList();
            ShowAdminGrid(null);
            AdminColName.Content = String.Empty;
        }

        private void ShowStudents()
        {
            AdminHeaderLabel.Content = "Students";
            AdminDataGrid.ItemsSource = (from students in model.Students
                                         join groups in model.Groups on students.Group_Id equals groups.Id
                                         join users in model.Users on students.User_Id equals users.Id
                                         select new {Id = students.Id, Name = students.Name, Surname = students.Surname, Group = groups.Name, Login = users.Login}).ToList();
            ShowAdminGrid(AdminStudentsGrid);
        }
        private void ShowTeachers()
        {
            AdminHeaderLabel.Content = "Teachers";
            AdminDataGrid.ItemsSource = (from teachers
                                             in model.Teachers
                                         select new
                                         {
                                             Id = teachers.Id,
                                             Name = teachers.Name,
                                             Surname = teachers.Surname,
                                             Login = teachers.User.Login
                                         }).ToList();
            ShowAdminGrid(AdminTeachersGrid);
        }
        private void ShowGroups()
        {
            AdminHeaderLabel.Content = "Groups";
            AdminDataGrid.ItemsSource = (from groups in model.Groups
                                         select new { Id = groups.Id, Name = groups.Name, Year = groups.Year }).ToList();
            ShowAdminGrid(AdminGroupsGrid);
        }
        private void ShowSubjects()
        {
            AdminHeaderLabel.Content = "Subjects";
            AdminDataGrid.ItemsSource = (from subjects in model.Subjects
                                         select new { Name = subjects.Name }).ToList();
            ShowAdminGrid(AdminSubjectsGrid);
        }

        private void ShowSchedules()
        {
            AdminHeaderLabel.Content = "Schedules";

            AdminDataGrid.ItemsSource = (from schedules in model.Schedules
                                         where schedules.Group.Name != null
                                         group schedules.Group.Name by schedules.Group.Name
                                             into schedules
                                             select new { Name = schedules.Key }).ToList();
            ShowAdminGrid(AdminSchedulesGrid);
        }

        private bool IsExist(Group g)
        {
            var items = (from groups in model.Groups
                         where g.Name == groups.Name && g.Year == groups.Year
                         select groups).FirstOrDefault();
            if (items != null)
            {
                return true;
            }
            return false;
        }
        private bool IsExist(Subject s)
        {
            var items = (from subjects in model.Subjects
                         where s.Name == subjects.Name
                         select subjects).FirstOrDefault();
            if (items != null)
            {
                return true;
            }
            return false;
        }
        private bool IsExist(User u)
        {
            var items = (from users in model.Users
                         where u.Login == users.Login
                         select users).FirstOrDefault();
            if (items != null)
            {
                return true;
            }
            return false;
        }

        private bool IsExist(Teacher t)
        {
            var items = (from teachers in model.Teachers
                         where t.Name == teachers.Name
                         && t.Surname == teachers.Surname
                         select teachers).FirstOrDefault();
            if (items != null)
            {
                return true;
            }
            return false;
        }

        private bool IsExist(Student s)
        {
            var items = (from students in model.Students
                         where s.Name == students.Name
                         && s.Surname == students.Surname
                         && s.Group_Id == students.Group_Id
                         select students).FirstOrDefault();
            if (items != null)
            {
                return true;
            }
            return false;
        }


        private void AdminComboBox_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            AdminColName.Content = String.Empty;
            switch (AdminComboBox.SelectedIndex)
            {
                case 0:
                    ShowAllUsers();
                    break;
                case 1:
                    ShowStudents();
                    break;
                case 2:
                    ShowTeachers();
                    break;
                case 3:
                    ShowGroups();
                    break;
                case 4:
                    ShowSubjects();
                    break;
                case 5:
                    ShowSchedules();
                    break;
            }
        }
    }
}
