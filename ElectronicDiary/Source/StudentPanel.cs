using System;
using System.Collections.Generic;
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
        Student  SelectedStudent;
        private void ShowName(object sender, SelectionChangedEventArgs e)
        {
            StudentDay.Columns.Clear();
            var DayOfWeek = ((Calendar)sender).SelectedDate.Value.DayOfWeek.ToString();
            var lessons = model.Schedules.Where(a => a.WeekDay == DayOfWeek).ToList();
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
                        
                       };


            StudentDay.ItemsSource = View.ToList();
            DataGridColumn column = new DataGridTextColumn();
            column.Header = "Your Marks";
            StudentDay.Columns.Add(column);
        }

    }
}
