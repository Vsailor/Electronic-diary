using Newtonsoft.Json;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Serialization;
using System.ServiceModel;
using System.Text;

namespace WcfServiceLibraryED
{
    // NOTE: You can use the "Rename" command on the "Refactor" menu to change the class name "Requests" in both code and config file together.
    public class Requests : IRequests
    {
        public string GetStudentSchedule(string dayOfWeek,string NameOfGroup){

            ElectronicDiaryDBEntities model = new ElectronicDiaryDBEntities();
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
    }
}
