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
        private void ShowExistLessons()
        {
            AdminDataGrid.ItemsSource = (from schedules in model.Schedules
                                         group schedules by schedules.Subject.Name into schedules
                                         select new { Lesson = schedules.Key }).ToList();
        }
        private void UpdateGroupsComboboxList()
        {             
            AdminPanelScheduleGroupCombobox.Items.Clear();
            var groupList = (from groups in model.Groups
                             select groups.Name).ToList();
            foreach (var item in groupList)
            {
                AdminPanelScheduleGroupCombobox.Items.Add(item);
            }
        }


        private void ShowAllLessons()
        {
            AdminDataGrid.ItemsSource = (from schedules in model.Schedules
                                         select new
                                         {
                                             Number = schedules.LessonNumber,
                                             Week_Day = schedules.WeekDay,
                                             Group_Name = schedules.Group.Name,
                                             Subject = schedules.Subject.Name,
                                             Teacher_Name = schedules.Teacher.Name,
                                             Teacher_Surname = schedules.Teacher.Surname,
                                             Description = schedules.Description
                                         }).ToList();
        }

        private void UpdateLessonsByGroupSelectedValue()
        {
            AdminDataGrid.ItemsSource = (from schedules in model.Schedules
                                         where schedules.Group.Name == AdminPanelScheduleGroupCombobox.SelectedValue.ToString()
                                         select new
                                         {
                                             Number = schedules.LessonNumber,
                                             Week_Day = schedules.WeekDay,
                                             Group_Name = schedules.Group.Name,
                                             Subject = schedules.Subject.Name,
                                             Teacher_Name = schedules.Teacher.Name,
                                             Teacher_Surname = schedules.Teacher.Surname,
                                             Description = schedules.Description
                                         }).ToList();
        }
        private void AdminPanelSchedulesAddSchedule_Click(object sender, RoutedEventArgs e)
        {
            ShowAdminGrid(AdminSchedulesPanel, AddScheduleForm);
            AdminColName.Content = "Add lesson";
            ShowAllLessons();
            UpdateGroupsComboboxList();

        }

        private void AdminPanelSchedulesEditSchedule_Click(object sender, RoutedEventArgs e)
        {
            ShowAdminGrid(AdminSchedulesPanel, EditScheduleButtonsForm);
            AdminColName.Content = "Edit lesson";
            ShowExistLessons();
            UpdateGroupsComboboxList();
        }

        private void AdminPanelSchedulesRemoveSchedule_Click(object sender, RoutedEventArgs e)
        {
            ShowAdminGrid(AdminSchedulesPanel, RemoveScheduleForm);

            AdminColName.Content = "Remove lesson";
            AdminHeaderLabel.Content = "Lessons";
            ShowExistLessons();
            UpdateGroupsComboboxList();
        }
        private void AdminPanelScheduleGroupCombobox_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            UpdateLessonsByGroupSelectedValue();
        }
        private void EditScheduleButtonBackPanel_Click(object sender, RoutedEventArgs e)
        {
            ShowAdminGrid(AdminSchedulesGrid);
            AdminColName.Content = String.Empty;
        }



        private void AddScheduleButtonBack_Click(object sender, RoutedEventArgs e)
        {
            ShowAdminGrid(AdminSchedulesGrid);
            AdminColName.Content = String.Empty;
        }

        private void AddScheduleButtonAdd_Click(object sender, RoutedEventArgs e)
        {
            // Add schedule
        }

        private void EditScheduleButtonBack_Click(object sender, RoutedEventArgs e)
        {
            ShowAdminGrid(AdminSchedulesGrid);
            AdminColName.Content = String.Empty;
        }

        private void EditScheduleButtonSave_Click(object sender, RoutedEventArgs e)
        {
            // Save schedule
        }

        private void RemoveScheduleButtonBack_Click(object sender, RoutedEventArgs e)
        {
            ShowAdminGrid(AdminSchedulesGrid);
            AdminColName.Content = String.Empty;
        }

        private void RemoveScheduleButtonDelete_Click(object sender, RoutedEventArgs e)
        {
            // Remove schedule
        }
    }
}
