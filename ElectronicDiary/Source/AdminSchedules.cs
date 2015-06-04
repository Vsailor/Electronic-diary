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
                                         orderby schedules.Group.Name
                                         select new
                                         {
                                             Number = schedules.LessonNumber,
                                             Week_Day = schedules.WeekDay,
                                             Group = schedules.Group.Name,
                                             Subject = schedules.Subject.Name,
                                             Teacher_Name = schedules.Teacher.Name,
                                             Teacher_Surname = schedules.Teacher.Surname,
                                             Description = schedules.Description
                                         }).ToList();
        }

        private void UpdateLessonsByGroupSelectedValue()
        {
            try
            {
                string combobox = AdminPanelScheduleGroupCombobox.SelectedItem.ToString();
                if (EditScheduleForm.Visibility == Visibility.Visible)
                {
                    AdminDataGrid.ItemsSource = (from schedules in model.Schedules
                                                 where schedules.Group.Name == combobox
                                                 orderby schedules.LessonNumber
                                                 select new
                                                 {
                                                     Id = schedules.Id,
                                                     Number = schedules.LessonNumber,
                                                     Week_Day = schedules.WeekDay,
                                                     Group_Name = schedules.Group.Name,
                                                     Subject = schedules.Subject.Name,
                                                     Teacher_Name = schedules.Teacher.Name,
                                                     Teacher_Surname = schedules.Teacher.Surname,
                                                     Description = schedules.Description
                                                 }).ToList();

                }
                else
                {
                    AdminDataGrid.ItemsSource = (from schedules in model.Schedules
                                                 where schedules.Group.Name == combobox
                                                 orderby schedules.LessonNumber
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
            }
            catch
            { }
        }
        private void FillWeekDaysCombobox(ComboBox cb)
        {
            string[] week = { "Monday", "Tuesday", "Wednesday", "Thursday", "Friday", "Saturday", "Sunday" };
            foreach (string day in week)
            {
                cb.Items.Add(day);
            }
        }
        private void ClearAddScheduleForm()
        {
            StatusBar.Content = String.Empty;
            AdminPanelAddScheduleWeekDayCombobox.Items.Clear();
            AdminPanelScheduleGroupCombobox.Items.Clear();
            AdminPanelAddScheduleTeacherCombobox.Items.Clear();
            AdminPanelAddScheduleSubjectCombobox.Items.Clear();
            AdminScheduleAddDescriptionTextBox.Text = String.Empty;
            AdminScheduleNumber.Text = String.Empty;
            UpdateGroupsComboboxList();
        }
        private void AdminPanelSchedulesAddSchedule_Click(object sender, RoutedEventArgs e)
        {
            ClearAddScheduleForm();
            ShowAdminGrid(AdminSchedulesPanel, AddScheduleForm);
            AdminColName.Content = "Add lesson";
            ShowAllLessons();
            UpdateGroupsComboboxList();

            FillWeekDaysCombobox(AdminPanelAddScheduleWeekDayCombobox);

            var teachers = (from users in model.Teachers select users).ToList();
            foreach (var item in teachers)
            {
                AdminPanelAddScheduleTeacherCombobox.Items.Add(String.Format("{0} {1}", item.Name, item.Surname));
            }
            var subjects = (from items in model.Subjects select items.Name).ToList();
            foreach (var item in subjects)
            {
                AdminPanelAddScheduleSubjectCombobox.Items.Add(item);
            }
        }
        private void UpdateLessonsByWeekDaySelectedValue()
        {
            string groupName = String.Empty;
            string weekDay = String.Empty;
            try
            {

                groupName = AdminPanelScheduleGroupCombobox.SelectedItem.ToString();
                weekDay = AdminPanelAddScheduleWeekDayCombobox.SelectedItem.ToString();
                AdminDataGrid.ItemsSource = (from schedules in model.Schedules
                                             where schedules.Group.Name == groupName
                                             && schedules.WeekDay == weekDay
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
            catch (NullReferenceException)
            {
                AdminDataGrid.ItemsSource = (from schedules in model.Schedules
                                             where schedules.WeekDay == weekDay
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
        }

        private void FillLessonNumberComboboxByGroupOrWeekDay(ComboBox cb, string groupName, string weekDay)
        {
            if (cb.Items.Count != 0)
            {
                cb.Items.Clear();
            }
            if (groupName == String.Empty || weekDay == String.Empty)
            {
                for (int i = 1; i <= 8; i++)
                {
                    cb.Items.Add(i.ToString());
                }
            }
            else
            {
                var usedNumbers = (from numbers in model.Schedules
                                   where
                                   numbers.WeekDay == weekDay
                                   && numbers.Group.Name == groupName
                                   select numbers.LessonNumber).ToList();
                for (int i = 1; i <= 8; i++)
                {
                    if (!usedNumbers.Contains(i))
                    {
                        cb.Items.Add(i.ToString());
                    }
                }
            }
        }
        private void FillIdNumberComboboxEditSchedule()
        {
            if (AdminPanelScheduleGroupCombobox.Items.Count == 0)
            {
                return;
            }
            string groupName = AdminPanelScheduleGroupCombobox.SelectedValue.ToString();
            var ids = (from schedules in model.Schedules
                       where groupName == schedules.Group.Name
                       select schedules.Id).ToList();
            EditScheduleLessonIdCombobox.Items.Clear();
            foreach (var item in ids)
            {
                EditScheduleLessonIdCombobox.Items.Add(item);
            }



        }
        private void ClearEditScheduleForm()
        {
            EditScheduleLessonIdCombobox.Items.Clear();
            AdminScheduleEditTextBox.Text = String.Empty;
            AdminPanelEditScheduleWeekDayCombobox.Items.Clear();
            AdminPanelEditScheduleTeacherCombobox.Items.Clear();
            AdminPanelEditScheduleSubjectCombobox.Items.Clear();
            AdminScheduleEditDescriptionTextBox.Text = String.Empty;
        }
        private void EditScheduleLessonIdCombobox_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            if (EditScheduleLessonIdCombobox.Items.Count == 0)
            {
                return;
            }
            int index = int.Parse(EditScheduleLessonIdCombobox.SelectedItem.ToString());
            var schedule = (from schedules in model.Schedules
                            where index == schedules.Id
                            select schedules).FirstOrDefault();
            AdminScheduleEditTextBox.Text = schedule.LessonNumber.ToString();
            FillWeekDaysCombobox(AdminPanelEditScheduleWeekDayCombobox);
            AdminPanelEditScheduleWeekDayCombobox.SelectedItem = schedule.WeekDay;
            var teachersdb = (from teachers in model.Teachers
                              select teachers).ToList();
            foreach (var item in teachersdb)
            {
                AdminPanelEditScheduleTeacherCombobox.Items.Add(String.Format("{0} {1}", item.Name, item.Surname));
            }
            AdminPanelEditScheduleTeacherCombobox.SelectedItem = String.Format("{0} {1}", schedule.Teacher.Name, schedule.Teacher.Surname);
            var subjectsdb = (from items in model.Subjects
                              select items.Name).ToList();
            foreach (var item in subjectsdb)
            {
                AdminPanelEditScheduleSubjectCombobox.Items.Add(item);
            }
            AdminPanelEditScheduleSubjectCombobox.SelectedItem = schedule.Subject.Name;
            AdminScheduleEditDescriptionTextBox.Text = schedule.Description;
        }

        private void AdminPanelAddScheduleWeekDayCombobox_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            try
            {
                FillLessonNumberComboboxByGroupOrWeekDay(AdminScheduleNumber, AdminPanelScheduleGroupCombobox.SelectedItem.ToString(), AdminPanelAddScheduleWeekDayCombobox.SelectedItem.ToString());
            }
            catch { }

            UpdateLessonsByWeekDaySelectedValue();

        }
        private void AdminPanelScheduleGroupCombobox_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            if (EditScheduleButtonsForm.Visibility == Visibility.Visible)
            {
                ClearEditScheduleForm();
                ShowAdminGrid(AdminSchedulesPanel, EditScheduleForm);

            }
            FillIdNumberComboboxEditSchedule();
            try
            {
                FillLessonNumberComboboxByGroupOrWeekDay(AdminScheduleNumber, AdminPanelScheduleGroupCombobox.SelectedItem.ToString(), AdminPanelAddScheduleWeekDayCombobox.SelectedItem.ToString());

            }
            catch { }
            UpdateLessonsByGroupSelectedValue();



        }


        private void AddScheduleButtonAdd_Click(object sender, RoutedEventArgs e)
        {
            int lessonNumber;
            string groupName;
            string weekDay;
            string teacher;
            string subject;
            string description;
            try
            {
                lessonNumber = int.Parse(AdminScheduleNumber.SelectedItem.ToString());
                groupName = AdminPanelScheduleGroupCombobox.SelectedItem.ToString();
                weekDay = AdminPanelAddScheduleWeekDayCombobox.SelectedItem.ToString();
                teacher = AdminPanelAddScheduleTeacherCombobox.SelectedItem.ToString();
                subject = AdminPanelAddScheduleSubjectCombobox.SelectedItem.ToString();
                description = AdminScheduleAddDescriptionTextBox.Text;
            }
            catch
            {
                StatusBar.Content = "At least one combobox isn't fill";
                return;
            }
            string teacherName = teacher.Remove(teacher.IndexOf(" "));
            string teacherSurname = teacher.Remove(0, teacher.IndexOf(" ") + 1);
            var subjectdb = (from subjects in model.Subjects
                             where subjects.Name == subject
                             select subjects).FirstOrDefault();
            var groupdb = (from groups in model.Groups
                           where groups.Name == groupName
                           select groups).FirstOrDefault();
            var teacherdb = (from teachers in model.Teachers
                             where teachers.Name == teacherName &&
                             teachers.Surname == teacherSurname
                             select teachers).FirstOrDefault();
            try
            {


                Schedule s = new Schedule()
                {
                    Subject_Id = subjectdb.Id,
                    Group_Id = groupdb.Id,
                    LessonNumber = lessonNumber,
                    WeekDay = weekDay,
                    Teacher_Id = teacherdb.Id,
                    Description = description
                };
                model.Schedules.Add(s);
                model.SaveChanges();
                StatusBar.Content = "Lesson added";
                ClearAddScheduleForm();
                AdminPanelSchedulesAddSchedule_Click(sender, e);
                AdminPanelScheduleGroupCombobox.SelectedValue = groupName;
            }
            catch (Exception ex)
            {
                StatusBar.Content = ex.Message;
            }
        }

        private void AdminPanelSchedulesEditSchedule_Click(object sender, RoutedEventArgs e)
        {
            ShowAdminGrid(AdminSchedulesPanel, EditScheduleButtonsForm);
            AdminColName.Content = "Edit lesson";
            ShowAllLessons();
            UpdateGroupsComboboxList();

        }

        private void AdminPanelSchedulesRemoveSchedule_Click(object sender, RoutedEventArgs e)
        {
            ShowAdminGrid(AdminSchedulesPanel, RemoveScheduleForm);

            AdminColName.Content = "Remove lesson";
            ShowAllLessons();
            UpdateGroupsComboboxList();
        }

        private void EditScheduleButtonBackPanel_Click(object sender, RoutedEventArgs e)
        {
            ShowAdminGrid(AdminSchedulesGrid);
            ShowSchedules();
            ClearEditScheduleForm();
            AdminColName.Content = String.Empty;
        }



        private void AddScheduleButtonBack_Click(object sender, RoutedEventArgs e)
        {
            ShowAdminGrid(AdminSchedulesGrid);
            AdminColName.Content = String.Empty;
            ShowAllLessons();

        }


        private void EditScheduleButtonBack_Click(object sender, RoutedEventArgs e)
        {
            ShowAdminGrid(AdminSchedulesGrid);
            AdminColName.Content = String.Empty;
            ShowAllLessons();
        }

        private void EditScheduleButtonSave_Click(object sender, RoutedEventArgs e)
        {
            string groupName = String.Empty;
            int scheduleId = 0;
            int lessonNumber = 0;
            string weekDay = String.Empty;
            string teacher = String.Empty;
            string subject = String.Empty;
            string description = String.Empty;
            try
            {
                groupName = AdminPanelScheduleGroupCombobox.SelectedItem.ToString();
                scheduleId = int.Parse(EditScheduleLessonIdCombobox.SelectedItem.ToString());
                lessonNumber = int.Parse(AdminScheduleEditTextBox.Text);
                weekDay = AdminPanelEditScheduleWeekDayCombobox.SelectedItem.ToString();
                teacher = AdminPanelEditScheduleTeacherCombobox.SelectedItem.ToString();
                subject = AdminPanelEditScheduleSubjectCombobox.SelectedItem.ToString();
                description = AdminScheduleEditDescriptionTextBox.Text;
            }
            catch
            { 
                
            }
            if (groupName == String.Empty ||
                weekDay == String.Empty || teacher == String.Empty ||
                subject == String.Empty)
            {
                StatusBar.Content = "At least one textbox is emty";
                return;
            }
            string teacherName = teacher.Remove(teacher.IndexOf(" "));
            string teacherSurname = teacher.Remove(0, teacher.IndexOf(" ") + 1);

            var subjectdb = (from subjects in model.Subjects
                             where subjects.Name == subject select subjects).FirstOrDefault();
            var groupdb = (from groups in model.Groups
                           where groups.Name == groupName
                           select groups).FirstOrDefault();
            var teacherdb = (from teachers in model.Teachers
                             where teachers.Name == teacherName &&
                             teachers.Surname == teacherSurname
                             select teachers).FirstOrDefault();
            Schedule s = new Schedule()
            {
                Subject = subjectdb,
                Group = groupdb,
                LessonNumber = lessonNumber,
                WeekDay = weekDay,
                Teacher = teacherdb,
                Description = description
            };
            var clone = (from schedules in model.Schedules
                               where schedules.LessonNumber == lessonNumber
                               && schedules.WeekDay == weekDay
                             select schedules).FirstOrDefault();
            if (clone != null)
            {
                if (scheduleId != clone.Id)
                {
                    StatusBar.Content = "This teacher is busy or lesson for this group is already scheduled";
                    return;
                }
            }
            try
            {
                var item = (from schedules in model.Schedules
                            where schedules.Id == scheduleId
                            select schedules).FirstOrDefault();
                item.LessonNumber = lessonNumber;
                item.WeekDay = weekDay;
                item.Teacher = s.Teacher;
                item.Subject = s.Subject;
                item.Description = s.Description;
                model.SaveChanges();
                ClearEditScheduleForm();
                AdminPanelScheduleGroupCombobox_SelectionChanged(null, null);
                StatusBar.Content = "Changes saved";
            }
            catch (Exception ex)
            {
                StatusBar.Content = ex.Message;
            }
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
