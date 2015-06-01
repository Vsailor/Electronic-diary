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
        private void ShowName(object sender, SelectionChangedEventArgs e)
        {
            StudentDay.Columns.Clear();
            var DayOfWeek = ((Calendar)sender).SelectedDate.Value.DayOfWeek.ToString();
            var studentgroup = StudentGroupList.SelectedItem.ToString();
            var View = from asd in model.Schedules 
                       join ast in model.Subjects on asd.Subjects_Id equals ast.Id
                       join ask in model.Teachers on asd.Teachers_Id equals ask.Id
                       join ass in model.Groups on asd.Groups_Id equals ass.Id
                       where asd.WeekDay == DayOfWeek
                       where studentgroup == ass.Name
                       select new
                       {
                           Num = asd.LessonNumber,
                           Subject = ast.Name,
                           Teacher = ask.Surname,
                           Group = ass.Name,
                           Auditory = asd.Description
                       };
            StudentDay.ItemsSource = View.ToList().OrderBy(o=>o.Num);
        }
        private void ShowSubjects(object sender, SelectionChangedEventArgs e)
        {
            var name=SubjectList.SelectedItem.ToString();
            var view = from m in model.Marks
                where m.Subject.Name == name
                where m.Student.Id==selectedStudent.Id
                select new
                {
                    Date = m.Date,
                    Mark = m.Mark1,
                    Note=m.Description
                };
            MarkTable.ItemsSource = view.ToList().OrderBy(m=>m.Date);
        }

    }
}
