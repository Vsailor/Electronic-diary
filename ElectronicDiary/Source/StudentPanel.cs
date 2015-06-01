using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.ServiceModel.Activation;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;

namespace ElectronicDiary
{
    partial class MainWindow : Window
    {
        Student  selectedStudent;
        private void ShowSchedule(object sender, SelectionChangedEventArgs e)
        {
            StudentSchedule.Columns.Clear();
            if (StudentCalendar.SelectedDate != null)
            {
                var dayOfWeek = StudentCalendar.SelectedDate.Value.DayOfWeek.ToString();
                if (StudentGroupList.SelectedItem != null)
                {
                    var studentGroup = StudentGroupList.SelectedItem.ToString();
                    var view = from asd in model.Schedules
                        join ast in model.Subjects on asd.Subjects_Id equals ast.Id
                        join ask in model.Teachers on asd.Teachers_Id equals ask.Id
                        join ass in model.Groups on asd.Groups_Id equals ass.Id
                        where asd.WeekDay == dayOfWeek
                        where studentGroup == ass.Name
                        select new
                        {
                            Num = asd.LessonNumber,
                            Subject = ast.Name,
                            Teacher = ask.Surname,
                            Group = ass.Name,
                            Auditory = asd.Description
                        };
                    StudentSchedule.ItemsSource = view.ToList().OrderBy(o => o.Num);
                }
            }
        }

        private void GroupSelected(object sender, SelectionChangedEventArgs e)
        {
            ShowSchedule(sender,e);
        }
        private void displaySchedule(object sender, RoutedEventArgs e)
        {
            StudentCalendar.SelectedDate = DateTime.Now.Date;
        }
        private void ShowSubjects(object sender, SelectionChangedEventArgs e)
        {
            if (SubjectList.SelectedItem != null)
            {
                var subjectName = SubjectList.SelectedItem.ToString();
                var view = from mark in model.Marks
                    where mark.Subject.Name == subjectName
                    where mark.Student.Id == selectedStudent.Id
                    where mark.Student.Group_Id == selectedStudent.Group_Id
                    select new
                    {
                        Date = mark.Date,
                        Mark = mark.Mark1,
                        Note = mark.Description
                    };
                StudentMarkTable.ItemsSource = view.ToList().OrderBy(m => m.Date);
            }
        }


    }
}
