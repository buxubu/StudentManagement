using StudentManagement.Helper;
using StudentManagement.Models;
using StudentManagement.ModelsView;
using StudentManagement.ModelView;
using StudentManagement.Services.ICourse;
using System.Text.RegularExpressions;

namespace StudentManagement.Services.IStudent
{
    public interface IStudent
    {
        public string GenerateMSSV(int idCourse, int idClass);
        IEnumerable<StudentModelView> GetAllStudent();
        StudentModelView GetIdStudent(int idStu);
        StudentsReponse SearchStudent(string? condition, int? page, int pageSize);
        IEnumerable<StudentFeeModelView> GettAllFeeStudent(int idStu);
    }
    public class RepoStudent : IStudent
    {
        private readonly DbStuManageContext _db;

        public RepoStudent(DbStuManageContext db)
        {
            _db = db;
        }
        public string GenerateMSSV(int idCourse, int idClass)
        {
            string studentCode = string.Empty;
            //MSSV = "Khoa_Code + Year_Code = Y2021 loại bỏ = substring + 5 STT của sinh viên "
            string course = _db.Courses.Find(idCourse).NameCourse.Substring(0, 1);
            string year_code = _db.Courses.Find(idCourse).StartYear.ToString();
            string countSTT = (_db.Students.Where(x => x.StudentCode.Substring(0, 5) == year_code + course.ToUpper()).ToList().Count() + 1).ToString();
            switch (countSTT.Length)
            {
                case 1:
                    return studentCode += year_code + course.ToUpper() + "0000" + countSTT;
                case 2:
                    return studentCode += year_code + course.ToUpper() + "000" + countSTT;
                case 3:
                    return studentCode += year_code + course.ToUpper() + "00" + countSTT;
                case 4:
                    return studentCode += year_code + course.ToUpper() + "0" + countSTT;
                default:
                    return studentCode += year_code + course.ToUpper() + countSTT;
            }
        }

        public IEnumerable<StudentModelView> GetAllStudent()
        {
            var getAllStu = (from stu in _db.Students
                             join c in _db.Classes on stu.IdClass equals c.IdClass
                             join dp in _db.Departments on stu.IdDepart equals dp.IdDepart
                             join course in _db.Courses on stu.IdCourse equals course.IdCourse
                             join stuPro in _db.StudyPrograms on stu.IdStuPro equals stuPro.IdStuPro
                             select new StudentModelView
                             {
                                 IdStudent = stu.IdStudent,
                                 IdDepart = dp.IdDepart,
                                 IdStuPro = stuPro.IdStuPro,
                                 IdClass = c.IdClass,
                                 IdCourse = course.IdCourse,
                                 NameDepart = dp.NameDepart,
                                 NameStuPro = stuPro.NameStuPro,
                                 NameClass = c.NameClass,
                                 StartYear = course.StartYear.Value,
                                 EndYear = course.EndYear.Value,
                                 FirstName = stu.FirstName,
                                 LastName = stu.LastName,
                                 StudentCode = stu.StudentCode,
                                 Sex = stu.Sex,
                                 PhoneNumber = stu.PhoneNumber,
                                 Birthday = stu.Birthday
                             }).AsEnumerable().ToList();
            return getAllStu;
        }

        public StudentModelView GetIdStudent(int idStu)
        {
            var getAllStu = (from stu in _db.Students
                             join c in _db.Classes on stu.IdClass equals c.IdClass
                             join dp in _db.Departments on stu.IdDepart equals dp.IdDepart
                             join course in _db.Courses on stu.IdCourse equals course.IdCourse
                             join stuPro in _db.StudyPrograms on stu.IdStuPro equals stuPro.IdStuPro
                             select new StudentModelView
                             {
                                 IdStudent = stu.IdStudent,
                                 IdDepart = dp.IdDepart,
                                 IdStuPro = stuPro.IdStuPro,
                                 IdClass = c.IdClass,
                                 IdCourse = course.IdCourse,
                                 NameDepart = dp.NameDepart,
                                 NameStuPro = stuPro.NameStuPro,
                                 NameClass = c.NameClass,
                                 StartYear = course.StartYear.Value,
                                 EndYear = course.EndYear.Value,
                                 FirstName = stu.FirstName,
                                 LastName = stu.LastName,
                                 StudentCode = stu.StudentCode,
                                 Sex = stu.Sex,
                                 PhoneNumber = stu.PhoneNumber,
                                 Birthday = stu.Birthday
                             }).AsEnumerable().Where(x => x.IdStudent == idStu).FirstOrDefault();
            return getAllStu;

        }

        public IEnumerable<StudentFeeModelView> GettAllFeeStudent(int idStu)
        {
            var getAllStuFee = (from stuF in _db.StudentFees
                                join stu in _db.Students on stuF.IdStudent equals stu.IdStudent
                                join acc in _db.Accounts on stu.IdStudent equals acc.IdStudent
                                join f in _db.Fees on stuF.IdFees equals f.IdFees
                                join sub in _db.Subjects on f.IdSub equals sub.IdSub
                                where stuF.IdStudent == idStu
                                select new StudentFeeModelView
                                {
                                    IdStuFee = stuF.IdStuFee,
                                    IdFees = f.IdFees,
                                    IdSub = sub.IdSub,
                                    IdStudent = stu.IdStudent,
                                    NameSub = sub.SubjectName,
                                    Description = f.Description,
                                    TotalFee = stuF.TotalFee,
                                    DateCreate = stuF.DateCreate,
                                    TotalCreditsOfSub = sub.TotalCredits,
                                    TheoryCreditsOfSub = sub.TheoryCredits,
                                    PracticeCreditsOfSub = sub.PracticeCredits,
                                    DefaultMoneyOfSub = sub.DefaultMoney
                                }).AsEnumerable().ToList();

            return getAllStuFee;
        }

        public StudentsReponse SearchStudent(string? condition, int? page, int pageSize)
        {
            var getAllStu = this.GetAllStudent();

            int totalItems;


            IEnumerable<StudentModelView> objStu = new List<StudentModelView>();
            if (!string.IsNullOrEmpty(condition))
            {
                objStu = getAllStu.Where(z => ShareHelp.RemoveUnicode(z.FirstName.ToLower()).Contains(ShareHelp.RemoveUnicode(condition).ToLower()) ||
                                              ShareHelp.RemoveUnicode(z.LastName.ToLower()).Contains(ShareHelp.RemoveUnicode(condition).ToLower()) ||
                                                                      z.PhoneNumber.Contains(condition))
                                   .OrderBy(x => x.IdStudent)
                                   .Skip((int)(page - 1) * pageSize)
                                   .Take(pageSize);


                totalItems = getAllStu.Where(z => ShareHelp.RemoveUnicode(z.FirstName.ToLower()).Contains(ShareHelp.RemoveUnicode(condition).ToLower()) ||
                                                        ShareHelp.RemoveUnicode(z.LastName.ToLower()).Contains(ShareHelp.RemoveUnicode(condition).ToLower()) ||
                                                                                z.PhoneNumber.Contains(condition)).Count();
            }
            else
            {
                objStu = getAllStu.OrderBy(x => x.IdStudent)
                              .Skip((int)(page - 1) * pageSize)
                              .Take(pageSize);
                totalItems = getAllStu.Count();
            }

            StudentsReponse mapStu = new StudentsReponse
            {
                ListStudent = objStu,
                Pagings = new Paging(totalItems, page, pageSize)
            };
            return mapStu;
        }
    }
}
