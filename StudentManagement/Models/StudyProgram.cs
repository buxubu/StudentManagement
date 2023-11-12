using System;
using System.Collections.Generic;

namespace StudentManagement.Models
{
    public partial class StudyProgram
    {
        public StudyProgram()
        {
            Students = new HashSet<Student>();
        }

        public int IdStuPro { get; set; }
        public string? NameStuPro { get; set; }
        public string? Level { get; set; }
        public int? Year { get; set; }

        public virtual ICollection<Student> Students { get; set; }
    }
}
