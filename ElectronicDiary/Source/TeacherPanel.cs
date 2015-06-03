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
            var selectedDate = TeacherCalendar.SelectedDate.Value.Date;
            var data = from t in model.Students
                       where t.Group.Name == groupname
                       select new
                       {
                           StudentName = t.Name,
                           StudentSurname = t.Surname,
                           Mark = "",
                           Note=""
                       };
            var studentList = data.ToList().OrderBy(a => a.StudentSurname);

            foreach (var student in studentList)
            {
                var view = new MyData() { StudentName = student.StudentName, StudentSurname = student.StudentSurname, Mark = student.Mark.ToString(), Note = student.Note};
                var record = model.Marks.Where(s => s.Student.Name == student.StudentName && s.Student.Surname == student.StudentSurname && s.Date == selectedDate && s.Subject.Name == subject);
                if (record.Any())
                {
                    view.Mark = record.First().Mark1.ToString();
                    view.Note = record.First().Description;
                }
                MyCollection.Add(view);
            }           
            TeacherMarkSetter.ItemsSource = MyCollection;
        }

        private void ShowTeacherSchedule(object sender, RoutedEventArgs e)
        {
            TeacherCalendar.SelectedDate = DateTime.Now.Date;
        }
        private void AddMarks(object sender, DataGridRowEditEndingEventArgs e)
        {
            var chosenRow = e.Row.DataContext as MyData;
            if (TeacherCalendar.SelectedDate != null)
            {
                var date = TeacherCalendar.SelectedDate.Value.Date;
                var record = model.Marks.Where(s => s.Student.Name == chosenRow.StudentName && s.Student.Surname == chosenRow.StudentSurname && s.Date == date&&s.Subject.Name==subject);
                if (record.Any())
                    try
                    {
                        record.First().Mark1 = int.Parse(chosenRow.Mark);
                        record.First().Description = chosenRow.Note;
                        model.SaveChanges();
                    }
                    catch (Exception ex)
                    {
                        StatusBar.Content = ex.Message;
                    }
                else
                {
                    var student = model.Students.First(m => m.Name == chosenRow.StudentName && m.Surname == chosenRow.StudentSurname);
                    var sbj = model.Subjects.First(a => a.Name == subject);
                    var mark = new Mark()
                    {
                        Date = TeacherCalendar.SelectedDate.Value.Date,
                        Mark1 = int.Parse(chosenRow.Mark),
                        Student_Id = student.Id,
                        Subject_Id = sbj.Id,
                        Description = chosenRow.Note
                    };
                    model.Marks.Add(mark);
                    try
                    {
                        model.SaveChanges();
                    }
                    catch (Exception ex)
                    {
                        StatusBar.Content = ex.Message;
                    }
              
                }
            }
        }

        private void ShowTeacherSchedule(object sender, SelectionChangedEventArgs e)
        {
            var selectedDate = TeacherCalendar.SelectedDate;
            if (selectedDate != null)
            {
                var DayOfWeek = selectedDate.Value.DayOfWeek.ToString();
                var View = from schedule in model.Schedules
                    join subj in model.Subjects on schedule.Subject_Id equals subj.Id
                    join teacher in model.Teachers on schedule.Teacher_Id equals teacher.Id
                    join @group in model.Groups on schedule.Group_Id equals @group.Id
                    where schedule.WeekDay == DayOfWeek
					where teacher.Id==selectedTeacher.Id
                    select new
                    {
                        Num = schedule.LessonNumber,
                        Subject = subj.Name,
                        Teacher = teacher.Surname,
                        Group = @group.Name,
                        Auditory = schedule.Description

                    };
                
                ///Убрать при релизе
                try
                {
                    var list = View.ToList().OrderBy(o => o.Num);
                    TeacherSchedule.ItemsSource = list;
                }
                catch
                {

                }
            }
        }

        private string groupname="";
        private string subject="";
        private void SelectedRow(object sender, SelectionChangedEventArgs e)
        {

            DataGrid _DataGrid = sender as DataGrid;
            try
            {
                if (_DataGrid != null)
                {
                    string strEID = _DataGrid.SelectedCells[0].Item.ToString();
                    int lpos = strEID.IndexOf("Group = ", StringComparison.Ordinal);
                    int rpos = strEID.IndexOf(", Auditory", StringComparison.Ordinal);
                    groupname = strEID.Substring(lpos + 8, rpos - lpos - 8);
                    lpos = strEID.IndexOf("Subject = ", StringComparison.Ordinal);
                    rpos = strEID.IndexOf(", Teacher", StringComparison.Ordinal);
                    subject = strEID.Substring(lpos + 10, rpos - lpos - 10);
                }
                ;
            }
            catch (Exception ex)
            {
                groupname = "";
                subject = "";
            }


        }

        private void SetMarks(object sender, RoutedEventArgs e)
        {
            if (groupname != "" || subject != "")
            {
                SetMarks();
                TeacherCalendar.Visibility = Visibility.Hidden;
                TeacherSchedule.Visibility = Visibility.Hidden;
                SetMarksButton.Visibility = Visibility.Hidden;
                BackToTeacherScheduleButton.Visibility = Visibility.Visible;
            }
            else
            {
                StatusBar.Content = "Select lesson in the table";
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
            TeacherSchedule.SelectedItem = null;
            
        }


    }
}
