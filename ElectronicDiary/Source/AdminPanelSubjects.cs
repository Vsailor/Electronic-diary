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
        private void AdminPanelAddSubject_Click(object sender, RoutedEventArgs e)
        {
            string toAdd = SubjectNameTextBox.Text;
            if (toAdd == String.Empty)
            {
                StatusBar.Content = "Name field is empty";
                return;
            }
            var newSubject = new Subject() { Name = toAdd };
            if (IsExist(newSubject))
            {
                StatusBar.Content = "This subject is already exist";
                return;
            }
            model.Subjects.Add(newSubject);
            try
            {
                model.SaveChanges();
                StatusBar.Content = "Subject added";
                SubjectNameTextBox.Text = String.Empty;
                ShowSubjects();
            }
            catch (Exception ex)
            {
                StatusBar.Content = ex.Message;
            }
        }

        private void AdminPanelRemoveSubject_Click(object sender, RoutedEventArgs e)
        {
            string toRemove = SubjectNameTextBox.Text;
            var subject = (from subjects in model.Subjects where
                           subjects.Name == toRemove select subjects).FirstOrDefault();
            if (subject == null)
            {
                StatusBar.Content = "Subject not found";
                return;
            }
            model.Subjects.Remove(subject);
            try
            {
                model.SaveChanges();
                StatusBar.Content = "Subject deleted";
                ShowSubjects();
            }
            catch (System.Data.Entity.Infrastructure.DbUpdateException)
            {
                StatusBar.Content = "This subject is used in another table";
            }
            catch (Exception ex)
            {
                StatusBar.Content = ex.Message;
            }
            
        }

    }
}
