using System;
using System.Collections.Generic;

namespace StudentManagement.Models
{
    public partial class TestResult
    {
        public int IdTestRe { get; set; }
        public int IdSub { get; set; }
        public int IdStudent { get; set; }
        public double? TotalMarkTen { get; set; }
        public double? TotalMarkFour { get; set; }
        public string? TotalMarkString { get; set; }
        public bool? Result { get; set; }
        public string? ResultDes { get; set; }

        public virtual Student IdStudentNavigation { get; set; } = null!;
        public virtual Subject IdSubNavigation { get; set; } = null!;
    }
}
