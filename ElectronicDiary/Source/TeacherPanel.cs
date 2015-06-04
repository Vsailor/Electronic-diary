using ElectronicDiary.ServiceReference1;
using Newtonsoft.Json;
using System;
using System.Collections;
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
        private Teacher selectedTeacher;
        ServiceReference1.RequestsClient client = new RequestsClient();
        void ShowSelectedGroup()
        {
            var selectedDate = TeacherCalendar.SelectedDate.Value.Date;
            TeacherMarkSetter.ItemsSource = JsonConvert.DeserializeObject<ArrayList>(client.GetSelectedGroup(groupname, selectedDate, subject));
        }



        private void LoadTeacherSchedule(object sender, RoutedEventArgs e)
        {
            TeacherCalendar.SelectedDate = DateTime.Now.Date;
        }
        private void AddMarks(object sender, DataGridRowEditEndingEventArgs e)
        {
            var chosenRow = e.Row.DataContext;
            var response = client.AddMarkToGroup(JsonConvert.SerializeObject(chosenRow), TeacherCalendar.SelectedDate.Value.Date, subject, groupname);
            StatusBar.Content = response;
        }

        private void ShowTeacherSchedule(object sender, SelectionChangedEventArgs e)
        {
            var selectedDate = TeacherCalendar.SelectedDate;
            if (selectedDate != null && selectedTeacher != null)
            {
                var DayOfWeek = selectedDate.Value.DayOfWeek.ToString();
                TeacherSchedule.ItemsSource = JsonConvert.DeserializeObject<ArrayList>(client.GetTeacherSchedule(DayOfWeek, selectedTeacher.Id));
            }
        }

        private string groupname = "";
        private string subject = "";
        private void SelectedRow(object sender, SelectionChangedEventArgs e)
        {

            DataGrid _DataGrid = sender as DataGrid;
            try
            {
                if (_DataGrid != null)
                {
                    string strEID = _DataGrid.SelectedCells[0].Item.ToString().Replace("\r\n", "").Replace(" ", "").Replace("\"", "");
                    int lpos = strEID.IndexOf("Group:", StringComparison.Ordinal);
                    int rpos = strEID.IndexOf(",Description", StringComparison.Ordinal);
                    groupname = strEID.Substring(lpos + 6, rpos - lpos - 6);
                    lpos = strEID.IndexOf("Subject:", StringComparison.Ordinal);
                    rpos = strEID.IndexOf(",Teacher", StringComparison.Ordinal);
                    subject = strEID.Substring(lpos + 8, rpos - lpos - 8);
                };
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
                ShowSelectedGroup();
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
