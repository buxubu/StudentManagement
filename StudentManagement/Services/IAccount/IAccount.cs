using NuGet.Protocol;
using StudentManagement.Models;
using StudentManagement.ModelsView;

namespace StudentManagement.Services.IAccount
{
    public interface IAccount
    {
        IEnumerable<AccountModelView> GetAllAccount();
        AccountModelView GetIdAccount(int idAcc);
    }
    public class RepoAccount : IAccount
    {
        private readonly DbStuManageContext _db;

        public RepoAccount(DbStuManageContext db)
        {
            _db = db;
        }
        public IEnumerable<AccountModelView> GetAllAccount()
        {
            var getAll = (from acc in _db.Accounts
                          join rl in _db.Roles on acc.IdRole equals rl.IdRole
                          join stu in _db.Students on acc.IdStudent equals stu.IdStudent
                          select new AccountModelView
                          {
                              IdAccount = acc.IdAccount,
                              IdRole = rl.IdRole,
                              IdStudent = stu.IdStudent,
                              NameRole = rl.NameRole,
                              NameStudent = stu.FirstName + " " + stu.LastName,
                              Username = acc.Username,
                              Password = acc.Password,
                              Salt = acc.Salt,
                              Active = acc.Active,
                              CreateDate = acc.CreateDate,
                              LastLogin = acc.LastLogin
                          }).AsEnumerable().ToList();
            return getAll;
        }

        public AccountModelView GetIdAccount(int idAcc)
        {
            var getAll = (from acc in _db.Accounts
                          join rl in _db.Roles on acc.IdRole equals rl.IdRole
                          join stu in _db.Students on acc.IdStudent equals stu.IdStudent
                          where acc.IdAccount == idAcc
                          select new AccountModelView
                          {
                              IdAccount = acc.IdAccount,
                              IdRole = rl.IdRole,
                              IdStudent = stu.IdStudent,
                              NameRole = rl.NameRole,
                              NameStudent = stu.FirstName + " " + stu.LastName,
                              Username = acc.Username,
                              Password = acc.Password,
                              Salt = acc.Salt,
                              Active = acc.Active,
                              CreateDate = acc.CreateDate,
                              LastLogin = acc.LastLogin
                          }).AsEnumerable().FirstOrDefault();
            return getAll;
        }
    }
}
