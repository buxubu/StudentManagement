using System;
using System.Collections.Generic;

namespace StudentManagement.Models
{
    public partial class Account
    {
        public int IdAccount { get; set; }
        public int IdRole { get; set; }
        public int IdStudent { get; set; }
        public string? Username { get; set; }
        public string? Password { get; set; }
        public string? Salt { get; set; }
        public bool? Active { get; set; }
        public DateTime? CreateDate { get; set; }
        public DateTime? LastLogin { get; set; }
        public string? NameRole { get; set; }

        public virtual Role IdRoleNavigation { get; set; } = null!;
        public virtual Student IdStudentNavigation { get; set; } = null!;
    }
}
