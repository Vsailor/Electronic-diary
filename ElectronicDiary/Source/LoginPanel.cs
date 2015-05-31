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
                    Admin.Visibility = Visibility.Visible;
                }
                else if (z.First().Role == role.student.ToString())
                {
                    Logining.Visibility = Visibility.Hidden;
                    Student.Visibility = Visibility.Visible;


                }
                else
                    if (z.First().Role == role.teacher.ToString())
                    {
                        Logining.Visibility = Visibility.Hidden;
                        Teacher.Visibility = Visibility.Visible;
                    }
            }
            else
            {
                MessageBox.Show("Логин или пароль были введены некоректно");
            }
        }
    }
}
