using System;
using System.Collections.Generic;

namespace StudentManagement.Models
{
    public partial class Role
    {
        public Role()
        {
            Accounts = new HashSet<Account>();
        }

        public int IdRole { get; set; }
        public string? NameRole { get; set; }
        public string? Descrip { get; set; }

        public virtual ICollection<Account> Accounts { get; set; }
    }
}
