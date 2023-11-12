using StudentManagement.Models;
using StudentManagement.ModelView;

namespace StudentManagement.ModelsView
{
    public class StudentModelView
    {
        public int IdStudent { get; set; }
        public int IdDepart { get; set; }
        public string NameDepart { get; set; }
        public int IdCourse { get; set; }
        public int StartYear { get; set; }
        public int EndYear { get; set; }
        public int IdStuPro { get; set; }
        public string NameStuPro { get; set; }
        public int IdClass { get; set; }
        public string? NameClass { get; set; }
        public string? StudentCode { get; set; }
        public string? FirstName { get; set; }
        public string? LastName { get; set; }
        public string? Sex { get; set; }
        public string? PhoneNumber { get; set; }
        public DateTime? Birthday { get; set; }
    }
    public class Search
    {
        public int? page { get; set; }
        public int pageSize { get; set; }
        public string? nameMovie { get; set; }
    }
    public class StudentsReponse
    {
        public IEnumerable<StudentModelView> ListStudent { get; set; }
        public Paging Pagings { get; set; }
    }
}
