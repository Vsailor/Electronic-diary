using Newtonsoft.Json;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Runtime.Serialization;
using System.ServiceModel;
using System.Text;

namespace WcfServiceLibraryED
{
    // NOTE: You can use the "Rename" command on the "Refactor" menu to change the class name "Requests" in both code and config file together.
    public class Requests : IRequests
    {
        ElectronicDiaryDBEntities model = new ElectronicDiaryDBEntities();
        public string GetStudentSchedule(string dayOfWeek, string NameOfGroup)
        {


            var view = from asd in model.Schedule
                       join ast in model.Subjects on asd.Subjects_Id equals ast.Id
                       join ask in model.Teachers on asd.Teachers_Id equals ask.Id
                       join ass in model.Groups on asd.Groups_Id equals ass.Id
                       where asd.WeekDay == dayOfWeek
                       where NameOfGroup == ass.Name
                       select new
                       {
                           Num = asd.LessonNumber,
                           Subject = ast.Name,
                           Teacher = ask.Surname,
                           Group = ass.Name,
                           Auditory = asd.Description
                       };
            var list = view.OrderBy(o => o.Num).ToList();
            return JsonConvert.SerializeObject(list);
        }
        public string GetStudentMarks(string subjectName, int studentId, int groupId)
        {
            var view = from mark in model.Marks
                       where mark.Subjects.Name == subjectName
                       where mark.Students.Id == studentId
                       where mark.Students.Group_Id == groupId
                       select new
                       {
                           Date = mark.Date,
                           Mark = mark.Mark,
                           Note = mark.Description
                       };
            var list = view.OrderBy(m => m.Date).ToList();
            return JsonConvert.SerializeObject(list);
        }
        [DataContract]
        public class MyData
        {
            [DataMember]
            public string StudentName { set; get; }
            [DataMember]
            public string StudentSurname { set; get; }
            [DataMember]
            public string Mark { set; get; }
            [DataMember]
            public string Note { set; get; }
        }

        public string GetSelectedGroup(string groupName, DateTime selectedDate, string subject)
        {
            ObservableCollection<MyData> MyCollection = new ObservableCollection<MyData>();
            var data = from t in model.Students
                       where t.Groups.Name == groupName
                       select new
                       {
                           StudentName = t.Name,
                           StudentSurname = t.Surname,
                           Mark = "",
                           Note = ""
                       };
            var studentList = data.ToList().OrderBy(a => a.StudentSurname);

            foreach (var student in studentList)
            {
                var view = new MyData() { StudentName = student.StudentName, StudentSurname = student.StudentSurname, Mark = student.Mark.ToString(),
                    Note = student.Note };
                var record = model.Marks.Where(s => s.Students.Name == student.StudentName && s.Students.Surname == student.StudentSurname &&
                    s.Date == selectedDate && s.Subjects.Name == subject && s.Students.Groups.Name == groupName);
                if (record.Any())
                {
                    view.Mark = record.First().Mark.ToString();
                    view.Note = record.First().Description;
                }
                MyCollection.Add(view);
            }
            return JsonConvert.SerializeObject(MyCollection);
        }
        public string AddMarkToGroup(string selectedrow, DateTime selectedDate, string subject, string groupname)
        {
            var chosenRow = JsonConvert.DeserializeObject<MyData>(selectedrow);
            if (selectedDate != null)
            {
                var date = selectedDate.Date;
                var record = model.Marks.Where(s => s.Students.Name == chosenRow.StudentName && s.Students.Surname == chosenRow.StudentSurname &&
                    s.Date == date && s.Subjects.Name == subject && s.Students.Groups.Name == groupname);
                if (record.Any())
                    try
                    {
                        record.First().Mark = int.Parse(chosenRow.Mark);
                        record.First().Description = chosenRow.Note;
                        model.SaveChanges();
                    }
                    catch (Exception ex)
                    {
                        return ex.Message;
                    }
                else
                {
                    var student = model.Students.First(m => m.Name == chosenRow.StudentName && m.Surname == chosenRow.StudentSurname);
                    var sbj = model.Subjects.First(a => a.Name == subject);
                    var mark = new Marks()
                    {
                        Date = selectedDate.Date,
                        Mark = int.Parse(chosenRow.Mark),
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
                        return ex.Message;
                    }
                }
            }
            return "";
        }
        public string GetTeacherSchedule(string DayOfWeek, int teacherId)
        {
            var View = from schedule in model.Schedule
                       join subj in model.Subjects on schedule.Subjects_Id equals subj.Id
                       join teacher in model.Teachers on schedule.Teachers_Id equals teacher.Id
                       join @group in model.Groups on schedule.Groups_Id equals @group.Id
                       where schedule.WeekDay == DayOfWeek
                       where teacher.Id == teacherId
                       select new
                       {
                           Num = schedule.LessonNumber,
                           Subject = subj.Name,
                           Teacher = teacher.Surname,
                           Group = @group.Name,
                           Auditory = schedule.Description

                       };
            var list = View.ToList().OrderBy(o => o.Num);
            return JsonConvert.SerializeObject(list);
        }
    }
}
