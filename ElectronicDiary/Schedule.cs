//------------------------------------------------------------------------------
// <auto-generated>
//    This code was generated from a template.
//
//    Manual changes to this file may cause unexpected behavior in your application.
//    Manual changes to this file will be overwritten if the code is regenerated.
// </auto-generated>
//------------------------------------------------------------------------------

namespace ElectronicDiary
{
    using System;
    using System.Collections.Generic;
    
    public partial class Schedule
    {
        public int Id { get; set; }
        public int Subject_Id { get; set; }
        public int Group_Id { get; set; }
        public int LessonNumber { get; set; }
        public string WeekDay { get; set; }
        public int Teacher_Id { get; set; }
        public string Description { get; set; }
    
        public virtual Group Group { get; set; }
        public virtual Subject Subject { get; set; }
        public virtual Teacher Teacher { get; set; }
    }
}
