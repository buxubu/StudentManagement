using System;
using System.Collections.Generic;

namespace StudentManagement.Models
{
    public partial class Class
    {
        public Class()
        {
            Students = new HashSet<Student>();
        }

        public int IdClass { get; set; }
        public string? NameClass { get; set; }
        public string? Descrip { get; set; }

        public virtual ICollection<Student> Students { get; set; }
    }
}
