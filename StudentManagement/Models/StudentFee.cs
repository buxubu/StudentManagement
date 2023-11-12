using System;
using System.Collections.Generic;

namespace StudentManagement.Models
{
    public partial class StudentFee
    {
        public StudentFee()
        {
            Payments = new HashSet<Payment>();
        }

        public int IdStuFee { get; set; }
        public int IdStudent { get; set; }
        public int IdFees { get; set; }
        public double? TotalFee { get; set; }
        public DateTime? DateCreate { get; set; }

        public virtual Fee IdFeesNavigation { get; set; } = null!;
        public virtual Student IdStudentNavigation { get; set; } = null!;
        public virtual ICollection<Payment> Payments { get; set; }
    }
}
