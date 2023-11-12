using StudentManagement.Models;
using StudentManagement.ModelsView;

namespace StudentManagement.Services.IFee
{
    public interface IFee
    {
        IEnumerable<FeeModelView> GetAllFees();
    }
    public class RepoFee : IFee
    {
        private readonly DbStuManageContext _db;

        public RepoFee(DbStuManageContext db)
        {
            _db = db;
        }
        public IEnumerable<FeeModelView> GetAllFees()
        {
            var getAllFee = (from f in _db.Fees
                             join sub in _db.Subjects on f.IdSub equals sub.IdSub
                             select new FeeModelView
                             {
                                 IdFees = f.IdFees,
                                 IdSub = sub.IdSub,
                                 NameSub = sub.SubjectName,
                                 Description = f.Description,
                                 TotalCreditsOfSub = sub.TotalCredits,
                                 TheoryCreditsOfSub = sub.TheoryCredits,
                                 PracticeCreditsOfSub = sub.PracticeCredits,
                                 DefaultMoneyOfSub = sub.DefaultMoney,
                                 Amount = sub.DefaultMoney * sub.TotalCredits
                             }).AsEnumerable().ToList();

            return getAllFee;
        }
    }
}
