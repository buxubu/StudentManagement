using StudentManagement.Models;

namespace StudentManagement.Services.IClass
{
    public interface IClass
    {
    }
    public class RepoClass : IClass
    {
        private readonly DbStuManageContext _db;
        public RepoClass(DbStuManageContext db)
        {
            _db = db;
        }
    }
}
