using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;

namespace ElectronicDiary
{
    partial class MainWindow : Window
    {
        enum role
        {
            administrator, student, teacher
        }
        private void Entering(object sender, RoutedEventArgs e)
        {
            if (Login.Text.Length == 0 || Password.Password.Length == 0)
            {
                StatusBar.Content="Login or password is empty";
                return;
            }
            try
            {

         
            var z = ((from asd in model.Users
                      where asd.Login == Login.Text
                      where asd.Password == Password.Password
                      select asd));
            if (z.LongCount() > 0)
            {
                if (z.First().Role == role.administrator.ToString())
                {
                    Logining.Visibility = Visibility.Hidden;
                    Exit.Visibility = Visibility.Visible;
                    AdminPanelShow();
                    UserName.Content = "Hello, admin";
                }
                else if (z.First().Role == role.student.ToString())
                {
                    Logining.Visibility = Visibility.Hidden;
                    Student.Visibility = Visibility.Visible;
                    Client.Visibility = Visibility.Visible;
                    Exit.Visibility = Visibility.Visible;
                    Teacher.Visibility = Visibility.Hidden;
                    int id = z.First().Id;
                    selectedStudent = (from st in model.Students
                        where st.User_Id == id
                        select st).FirstOrDefault();
                                            
                    UserName.Content = "Hello, " + selectedStudent.Name+" "+selectedStudent.Surname;
                    var groups = model.Groups.ToList();
                    foreach (var @group in groups)
                    {
                        StudentGroupList.Items.Add(@group.Name);
                    }
                    StudentGroupList.SelectedItem=selectedStudent.Group.Name;
                    SubjectList.Items.Clear();
                    var subjects = model.Subjects.ToList();
                    foreach (var @subject in subjects)
                    {
                        SubjectList.Items.Add(subject.Name);
                    }

                    StudentSchedule.ItemsSource = null;
                    StudentMarkTable.ItemsSource = null;
                    SubjectList.SelectedItem = null;
             

                    StudentCalendar.SelectedDate = DateTime.Now.Date;
                    ShowSchedule(sender, null);
                }
                else
                    if (z.First().Role == role.teacher.ToString())
                    {
                        Logining.Visibility = Visibility.Hidden;
                        Client.Visibility = Visibility.Visible;
                        Student.Visibility = Visibility.Hidden;
                        Teacher.Visibility = Visibility.Visible;
                        Exit.Visibility = Visibility.Visible;
                        //SetMarks();
                        int id = z.First().Id;
                        selectedTeacher = (from st in model.Teachers
                                           where st.UserId == id
                                           select st).FirstOrDefault();

                        UserName.Content = "Hello, " + selectedTeacher.Name + " " + selectedTeacher.Surname;

                        TeacherCalendar.Visibility = Visibility.Visible;
                        TeacherSchedule.Visibility = Visibility.Visible;
                        SetMarksButton.Visibility = Visibility.Visible;
                        BackToTeacherScheduleButton.Visibility = Visibility.Hidden;
                        groupname = "";
                        subject = "";
                        TeacherSchedule.ItemsSource = null;
                        TeacherMarkSetter.ItemsSource = null;

                        TeacherCalendar.SelectedDate = DateTime.Now.Date;
                        ShowTeacherSchedule(e, null);
                    }
            }
            else
            {
                StatusBar.Content = "Login or password is wrong";
            }
            }
            catch (Exception)
            {

                StatusBar.Content = "Make sure server is running";
            }
        }
           
    }
}
