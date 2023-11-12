using StudentManagement.Models;

namespace StudentManagement.Services.ICourse
{
    public interface ICourse
    {
        Course GetCourseId(int idCourse);
    }
    public class RepoCourse : ICourse
    {
        private readonly DbStuManageContext _db;
        public RepoCourse(DbStuManageContext db)
        {
            _db = db;
        }
        public Course GetCourseId(int idCourse)
        {
            return _db.Courses.Find(idCourse);
        }
    }
}
