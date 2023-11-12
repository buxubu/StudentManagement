using NuGet.Common;
using StudentManagement.Models;
using StudentManagement.ModelsView;

namespace StudentManagement.Services.IStudentFee
{
    public interface IStudentFee
    {
        IEnumerable<StudentFeeModelView> GetAllStudentFee();
    }
    public class RepoStudentFee : IStudentFee
    {
        private readonly DbStuManageContext _db;

        public RepoStudentFee(DbStuManageContext db)
        {
            _db = db;
        }
        public IEnumerable<StudentFeeModelView> GetAllStudentFee()
        {
            var getAllStuFee = (from stuF in _db.StudentFees
                                join stu in _db.Students on stuF.IdStudent equals stu.IdStudent
                                join f in _db.Fees on stuF.IdFees equals f.IdFees
                                join sub in _db.Subjects on f.IdSub equals sub.IdSub
                                select new StudentFeeModelView
                                {
                                    IdFees = f.IdFees,
                                    IdSub = sub.IdSub,
                                    IdStudent = stu.IdStudent,
                                    NameStudent = stu.FirstName + " " + stu.LastName,
                                    NameSub = sub.SubjectName,
                                    Description = f.Description,
                                    TotalFee = f.Amount,
                                    DateCreate = stuF.DateCreate,
                                    TotalCreditsOfSub = sub.TotalCredits,
                                    TheoryCreditsOfSub = sub.TheoryCredits,
                                    PracticeCreditsOfSub = sub.PracticeCredits,
                                    DefaultMoneyOfSub = sub.DefaultMoney,
                                    Amount = sub.DefaultMoney * sub.TotalCredits
                                }).AsEnumerable().ToList();
            return getAllStuFee;
        }
    }
}
