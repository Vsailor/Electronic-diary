//------------------------------------------------------------------------------
// <auto-generated>
//    This code was generated from a template.
//
//    Manual changes to this file may cause unexpected behavior in your application.
//    Manual changes to this file will be overwritten if the code is regenerated.
// </auto-generated>
//------------------------------------------------------------------------------

namespace WcfServiceLibraryED
{
    using System;
    using System.Collections.Generic;
    
    public partial class Groups
    {
        public Groups()
        {
            this.Schedules = new HashSet<Schedules>();
            this.Students = new HashSet<Students>();
        }
    
        public int Id { get; set; }
        public string Name { get; set; }
        public int Year { get; set; }
    
        public virtual ICollection<Schedules> Schedules { get; set; }
        public virtual ICollection<Students> Students { get; set; }
    }
}
