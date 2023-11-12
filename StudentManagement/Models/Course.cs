using System;
using System.Collections.Generic;

namespace StudentManagement.Models
{
    public partial class Course
    {
        public Course()
        {
            Students = new HashSet<Student>();
        }

        public int IdCourse { get; set; }
        public string? NameCourse { get; set; }
        public int? StartYear { get; set; }
        public int? EndYear { get; set; }
        public string? CourseCode { get; set; }

        public virtual ICollection<Student> Students { get; set; }
    }
}
