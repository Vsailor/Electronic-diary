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
    }
}
