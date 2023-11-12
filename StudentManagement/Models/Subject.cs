using System;
using System.Collections.Generic;

namespace StudentManagement.Models
{
    public partial class Subject
    {
        public Subject()
        {
            Fees = new HashSet<Fee>();
            TestResults = new HashSet<TestResult>();
        }

        public int IdSub { get; set; }
        public int IdStudent { get; set; }
        public string? SubjectCode { get; set; }
        public string? SubjectName { get; set; }
        public double? DefaultMoney { get; set; }
        public int? Semester { get; set; }
        public int? TotalCredits { get; set; }
        public int? TheoryCredits { get; set; }
        public int? PracticeCredits { get; set; }

        public virtual Student IdStudentNavigation { get; set; } = null!;
        public virtual ICollection<Fee> Fees { get; set; }
        public virtual ICollection<TestResult> TestResults { get; set; }
    }
}
