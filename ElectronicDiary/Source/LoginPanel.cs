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
                MessageBox.Show("Логин или пароль не были введены");
                return;
            }

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
                    UserName.Content = "admin";
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
                                            
                    UserName.Content = selectedStudent.Name+" "+selectedStudent.Surname;
                    var groups = model.Groups.ToList();
                    foreach (var @group in groups)
                    {
                        StudentGroupList.Items.Add(@group.Name);
                    }
                    StudentGroupList.SelectedItem=selectedStudent.Group.Name;
                    var subjects = model.Subjects.ToList();
                    foreach (var @subject in subjects)
                    {
                        SubjectList.Items.Add(subject.Name);
                    }
                    //StudentGroupList.SelectedItem = selectedStudent.Group.Name;

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

                        UserName.Content = selectedTeacher.Name + " " + selectedTeacher.Surname;
                    }
            }
            else
            {
                MessageBox.Show("Логин или пароль были введены некоректно");
            }
        }
    }
}
