using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;

namespace ElectronicDiary
{
    partial class MainWindow : Window
    {

        private void AdminPanelGroupsAddGroup_Click(object sender, RoutedEventArgs e)
        {
            ShowAdminGrid(AdminGroupsAdd);
            AdminColName.Content = "Add group";
        }

        private void AdminPanelBackAddGroup_Click(object sender, RoutedEventArgs e)
        {
            ShowAdminGrid(AdminGroupsGrid);
            AdminColName.Content = String.Empty;
            StatusBar.Content = String.Empty;
        }


        private void AdminPanelGroupsAdd_Click(object sender, RoutedEventArgs e)
        {
            string name = GroupNameTextBoxAdd.Text;
            int year;
            if (!int.TryParse(GroupYearTextBoxAdd.Text, out year))
            {
                StatusBar.Content = "Year must be numeric";
            }
            else
            {
                var g = new Group()
                {
                    Name = name,
                    Year = year
                };
                if (year < System.DateTime.Now.Year || year > 9999)
                {
                    StatusBar.Content = "Wrong year set";
                    return;
                }
                if (IsExist(g))
                {
                    StatusBar.Content = "This group already exists";
                    return;
                }
                model.Groups.Add(g);
                try
                {
                    model.SaveChanges();
                    ShowGroups();
                    AdminPanelGroupsAddGroup_Click(sender, e);
                    StatusBar.Content = "Group added";
                    GroupNameTextBoxAdd.Text = String.Empty;
                    GroupYearTextBoxAdd.Text = String.Empty;
                }
                catch (Exception ex)
                {
                    StatusBar.Content = ex.Message;
                }

            }
        }

        private void AdminPanelGroupsEditGroup_Click(object sender, RoutedEventArgs e)
        {
            ShowAdminGrid(AdminGroupsEdit);
            AdminColName.Content = "Edit group";
            AdminPanelGroupIdCombobox.Items.Clear();
            GroupNameTextBoxEdit.Text = String.Empty;
            GroupYearTextBoxEdit.Text = String.Empty;
            var groupsIds = (from groups in model.Groups
                             select groups.Id).ToList();
            foreach (var item in groupsIds)
            {
                AdminPanelGroupIdCombobox.Items.Add(item);
            }
            StatusBar.Content = String.Empty;
        }

        private void AdminPanelGroupIdCombobox_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            if (AdminPanelGroupIdCombobox.SelectedIndex != -1)
            {
                try
                {
                    int index = int.Parse(AdminPanelGroupIdCombobox.SelectedValue.ToString());
                    var group = (from groups in model.Groups
                                 where groups.Id == index
                                 select groups).FirstOrDefault();
                    GroupNameTextBoxEdit.Text = group.Name;
                    GroupYearTextBoxEdit.Text = group.Year.ToString();
                    StatusBar.Content = String.Empty;
                }
                catch (Exception ex)
                {
                    StatusBar.Content = ex.Message;
                }

            }
        }
        private void AdminPanelBackEditGroup_Click(object sender, RoutedEventArgs e)
        {
            ShowAdminGrid(AdminGroupsGrid);
            AdminColName.Content = String.Empty;
        }

        private void AdminPanelSaveEditGroup_Click(object sender, RoutedEventArgs e)
        {
            if (AdminPanelGroupIdCombobox.SelectedIndex == -1)
            {
                StatusBar.Content = "Id isn't selected";
                return;
            }
            try
            {
                int index = int.Parse(AdminPanelGroupIdCombobox.SelectedValue.ToString());
                string name = GroupNameTextBoxEdit.Text;
                int year = int.Parse(GroupYearTextBoxEdit.Text);
                var group = (from groups in model.Groups
                             where
                                 groups.Id == index
                             select groups).FirstOrDefault();
                if (year < System.DateTime.Now.Year || year > 9999)
                {
                    StatusBar.Content = "Wrong year set";
                    return;
                }
                var reservGroup = new Group() { Name = name, Year = year };
                if (IsExist(reservGroup))
                {
                    StatusBar.Content = "This group already exists";
                    return;
                }
                group.Name = name;
                group.Year = year;
                model.SaveChanges();
                ShowGroups();
                AdminPanelGroupsEditGroup_Click(sender, e);
                StatusBar.Content = "Group saved";
            }
            catch (Exception ex)
            {
                StatusBar.Content = ex.Message;
            }

        }
        private void AdminPanelGroupsRemoveGroup_Click(object sender, RoutedEventArgs e)
        {
            ShowAdminGrid(AdminGroupsRemove);
            AdminColName.Content = "Remove group";
            AdminPanelGroupIdRemoveCombobox.Items.Clear();
            var groupNames = (from groups in model.Groups
                             select groups.Id).ToList();
            foreach (var item in groupNames)
            {
                AdminPanelGroupIdRemoveCombobox.Items.Add(item);
            }
            StatusBar.Content = String.Empty;

        }

        private void AdminPanelRemoveGroup_Click(object sender, RoutedEventArgs e)
        {
            if (AdminPanelGroupIdRemoveCombobox.SelectedIndex == -1)
            {
                StatusBar.Content = "Id isn't selected";
                return;
            }
            try
            {
                int groupids = int.Parse(AdminPanelGroupIdRemoveCombobox.SelectedValue.ToString());
                var group = (from groups in model.Groups
                             where
                                 groups.Id == groupids
                             select groups).FirstOrDefault();
                model.Schedules.RemoveRange(group.Schedules);
                model.Students.RemoveRange(group.Students);
                model.Groups.Remove(group);
                model.SaveChanges();
                ShowGroups();
                AdminPanelGroupsRemoveGroup_Click(sender, e);
                StatusBar.Content = "Group removed";
            }
            catch (System.Data.Entity.Infrastructure.DbUpdateException)
            {
                StatusBar.Content = "This group is used in another table";
            }
            catch (Exception ex)
            {
                StatusBar.Content = ex.Message;
            }

        }
        private void AdminPanelBackRemoveGroup_Click(object sender, RoutedEventArgs e)
        {
            ShowAdminGrid(AdminGroupsGrid);
            AdminColName.Content = String.Empty;
        }
    }
}
