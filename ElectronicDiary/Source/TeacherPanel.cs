using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;

namespace ElectronicDiary
{
    partial class MainWindow : Window
    {
        public class MyData
        {
            public string StudentName { set; get; }
            public string StudentSurname { set; get; }
            public string Mark { set; get; }
            public string Note { set; get; }
        }


        private Teacher selectedTeacher;
        private ObservableCollection<MyData> MyCollection;
        void SetMarks()
        {
            MyCollection = new ObservableCollection<MyData>();
            var date = TeacherCalendar.SelectedDate.Value.Date;
            var data = from t in model.Students
                       where t.Group.Name == groupname
                       select new
                       {
                           StudentName = t.Name,
                           StudentSurname = t.Surname,
                           Mark = "",
                           Note=""
                       };
            var list = data.ToList().OrderBy(a => a.StudentName);
            //list.First().
            foreach (var l in list)
            {
                var view = new MyData() { StudentName = l.StudentName, StudentSurname = l.StudentSurname, Mark = l.Mark.ToString(), Note = l.Note};
                var record = model.Marks.Where(m => m.Student.Name == l.StudentName && m.Student.Surname == l.StudentSurname && m.Date == date && m.Subject.Name == subject);
                if (record.Any())
                {
                    view.Mark = record.First().Mark1.ToString();
                    view.Note = record.First().Description;
                }
                MyCollection.Add(view);
            }
           
            TeacherMarkSetter.ItemsSource = MyCollection;

        }
        private void AddMarks(object sender, DataGridRowEditEndingEventArgs e)
        {
            var s = e.Row.DataContext as MyData;
            var date = TeacherCalendar.SelectedDate.Value.Date;
            var record = model.Marks.Where(m => m.Student.Name == s.StudentName && m.Student.Surname == s.StudentSurname && m.Date == date&&m.Subject.Name==subject);
            if (record.Any())
                try
                {
                    record.First().Mark1 = int.Parse(s.Mark);
                    record.First().Description = s.Note;
                    model.SaveChanges();
                }
                catch (Exception)
                {


                }
            else
            {
                var student = model.Students.First(m => m.Name == s.StudentName && m.Surname == s.StudentSurname);
                var sbj = model.Subjects.First(a => a.Name == subject);
                var mark = new Mark()
                {
                    Date = TeacherCalendar.SelectedDate.Value.Date,
                    Mark1 = int.Parse(s.Mark),
                    Student_Id = student.Id,
                    Subject_Id = sbj.Id,
                    Description = s.Note
                };
                model.Marks.Add(mark);
                try
                {
                    model.SaveChanges();
                }
                catch (Exception ex)
                {
                    
                }
              
            }
        }

        private void ShowTeacherSchedule(object sender, SelectionChangedEventArgs e)
        {
            var DayOfWeek = ((Calendar)sender).SelectedDate.Value.DayOfWeek.ToString();
            var View = from asd in model.Schedules
                       join ast in model.Subjects on asd.Subjects_Id equals ast.Id
                       join ask in model.Teachers on asd.Teachers_Id equals ask.Id
                       join ass in model.Groups on asd.Groups_Id equals ass.Id
                       where asd.WeekDay == DayOfWeek
                       select new
                       {
                           Num = asd.LessonNumber,
                           Subject = ast.Name,
                           Teacher = ask.Surname,
                           Group = ass.Name,
                           Auditory = asd.Description

                       };
            TeacherSchedule.ItemsSource = View.ToList().OrderBy(o => o.Num);

        }
        private string groupname;
        private string subject;
        private void test(object sender, SelectionChangedEventArgs e)
        {

            DataGrid _DataGrid = sender as DataGrid;
            try
            {
                string strEID = _DataGrid.SelectedCells[0].Item.ToString();
                int t = strEID.IndexOf("Group = ");
                int p = strEID.IndexOf(", Auditory");
                groupname = strEID.Substring(t + 8, p - t - 8);
                t = strEID.IndexOf("Subject = ");
                p = strEID.IndexOf(", Teacher");
                subject = strEID.Substring(t + 10, p - t - 10); ;
            }
            catch (Exception)
            {
                groupname = "";
                subject = "";
            }


        }

        private void SetMarks(object sender, RoutedEventArgs e)
        {
            if (groupname != "" && subject != "")
            {
                SetMarks();
                TeacherCalendar.Visibility = Visibility.Hidden;
                TeacherSchedule.Visibility = Visibility.Hidden;
                SetMarksButton.Visibility = Visibility.Hidden;
                BackToTeacherScheduleButton.Visibility = Visibility.Visible;
            }
            else
            {
                MessageBox.Show("Select lesson");
            }
        }
        private void BackToTeacherSchedule(object sender, RoutedEventArgs e)
        {
            TeacherCalendar.Visibility = Visibility.Visible;
            TeacherSchedule.Visibility = Visibility.Visible;
            SetMarksButton.Visibility = Visibility.Visible;
            BackToTeacherScheduleButton.Visibility = Visibility.Hidden;
            groupname = "";
            subject = "";
        }


    }
}
