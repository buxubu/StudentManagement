using System;
using System.Collections.Generic;

namespace StudentManagement.Models
{
    public partial class Fee
    {
        public Fee()
        {
            StudentFees = new HashSet<StudentFee>();
        }

        public int IdFees { get; set; }
        public int IdSub { get; set; }
        public string? Description { get; set; }
        public double? Amount { get; set; }

        public virtual Subject IdSubNavigation { get; set; } = null!;
        public virtual ICollection<StudentFee> StudentFees { get; set; }
    }
}
