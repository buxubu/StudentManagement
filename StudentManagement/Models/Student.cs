using System;
using System.Collections.Generic;

namespace StudentManagement.Models
{
    public partial class Student
    {
        public Student()
        {
            Accounts = new HashSet<Account>();
            StudentFees = new HashSet<StudentFee>();
            Subjects = new HashSet<Subject>();
            TestResults = new HashSet<TestResult>();
        }

        public int IdStudent { get; set; }
        public int IdDepart { get; set; }
        public int IdCourse { get; set; }
        public int IdStuPro { get; set; }
        public int IdClass { get; set; }
        public string? NameClass { get; set; }
        public string? StudentCode { get; set; }
        public string? FirstName { get; set; }
        public string? LastName { get; set; }
        public string? Sex { get; set; }
        public string? PhoneNumber { get; set; }
        public DateTime? Birthday { get; set; }

        public virtual Class IdClassNavigation { get; set; } = null!;
        public virtual Course IdCourseNavigation { get; set; } = null!;
        public virtual Department IdDepartNavigation { get; set; } = null!;
        public virtual StudyProgram IdStuProNavigation { get; set; } = null!;
        public virtual ICollection<Account> Accounts { get; set; }
        public virtual ICollection<StudentFee> StudentFees { get; set; }
        public virtual ICollection<Subject> Subjects { get; set; }
        public virtual ICollection<TestResult> TestResults { get; set; }
    }
}
