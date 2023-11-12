using System;
using System.Collections.Generic;

namespace StudentManagement.Models
{
    public partial class Department
    {
        public Department()
        {
            Students = new HashSet<Student>();
        }

        public int IdDepart { get; set; }
        public string? NameDepart { get; set; }

        public virtual ICollection<Student> Students { get; set; }
    }
}
