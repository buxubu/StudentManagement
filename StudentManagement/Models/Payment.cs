using System;
using System.Collections.Generic;

namespace StudentManagement.Models
{
    public partial class Payment
    {
        public int IdPay { get; set; }
        public int IdStuFee { get; set; }
        public string? Description { get; set; }
        public double? Amount { get; set; }

        public virtual StudentFee IdStuFeeNavigation { get; set; } = null!;
    }
}
