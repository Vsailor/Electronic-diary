using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Serialization;
using System.ServiceModel;
using System.Text;

namespace WcfServiceLibraryED
{
    // NOTE: You can use the "Rename" command on the "Refactor" menu to change the interface name "IRequests" in both code and config file together.
    [ServiceContract]
    public interface IRequests
    {
        [OperationContract]
        string GetStudentSchedule(string dayOfWeek, string NameOfGroup);
        [OperationContract]
        string GetStudentMarks(string subjectName, int studentId, int groupId);
        [OperationContract]
        string GetSelectedGroup(string groupName, DateTime selectedDate, string subject);
        [OperationContract]
        string AddMarkToGroup(string selectedrow, DateTime selectedDate, string subject, string groupname);
        [OperationContract]
        string GetTeacherSchedule(string DayOfWeek, int teacherId);
    }
}
