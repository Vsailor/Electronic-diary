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
    
    public partial class Students
    {
        public Students()
        {
            this.Marks = new HashSet<Marks>();
        }
    
        public int Id { get; set; }
        public string Name { get; set; }
        public string Surname { get; set; }
        public int Group_Id { get; set; }
        public int User_Id { get; set; }
    
        public virtual Groups Groups { get; set; }
        public virtual ICollection<Marks> Marks { get; set; }
        public virtual Users Users { get; set; }
    }
}
