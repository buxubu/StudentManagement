namespace StudentManagement.ModelsView
{
    public class AccountModelView
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
        public string? NameStudent { get; set; }
    }
}
