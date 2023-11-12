using AutoMapper;
using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using StudentManagement.Helper;
using StudentManagement.Models;
using StudentManagement.ModelsView;
using StudentManagement.ModelView;
using StudentManagement.Services.IStudent;
using System.Linq;
using System.Net.WebSockets;
using System.Runtime.Intrinsics.Arm;
using System.Security.Claims;

namespace StudentManagement.Areas.Admin.Controllers
{
    [Area("Admin")]
    public class HomeController : Controller
    {
        private readonly DbStuManageContext _db;
        private readonly IMapper _mapper;
        private readonly IStudent _studentService;
        public HomeController(DbStuManageContext db, IMapper mapper, IStudent studentService)
        {
            _db = db;
            _mapper = mapper;
            _studentService = studentService;
        }
        [HttpGet]
        public async Task<IActionResult> Index(string? condition, int? page, int pageSize)
        {
            try
            {

                ClaimsPrincipal claimUser = HttpContext.User;
                if (!claimUser.Identity.IsAuthenticated) return RedirectToAction("Login", "Accounts", new { Area = "Admin" });

                var accountId = HttpContext.Session.GetString("IdAccount");
                if (accountId == null) return RedirectToAction("Login", "Accounts", new { Area = "Admin" });

                var getAllStu = _studentService.GetAllStudent();

                page = page.HasValue ? page.Value > 0 ? page.Value : 1 : 1;
                pageSize = pageSize == 0 ? 5 : pageSize;

                var getSearchStu = _studentService.SearchStudent(condition, page, pageSize);
                TempData["message"] = "Search Student Success !!!";

                ViewBag.TotalPage = getSearchStu.Pagings.TotalPage;
                ViewBag.StartPage = getSearchStu.Pagings.StartPage;
                ViewBag.EndPage = getSearchStu.Pagings.EndPage;
                ViewBag.CurrentPage = getSearchStu.Pagings.CurrentPage;
                ViewBag.Condition = condition;


                return View(getSearchStu);
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message.ToString());
            }
        }
        [HttpGet]
        public async Task<IActionResult> AddStudent()
        {
            try
            {
                ViewData["Class"] = new SelectList(_db.Classes, "IdClass", "NameClass");
                ViewData["Depart"] = new SelectList(_db.Departments, "IdDepart", "NameDepart");
                ViewData["StuPro"] = new SelectList(_db.StudyPrograms, "IdStuPro", "NameStuPro");
                ViewData["Course"] = new SelectList(_db.Courses, "IdCourse", "NameCourse");
                return View();
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message.ToString());
            }

        }
        [HttpPost]
        public async Task<IActionResult> AddStudent(StudentModelView model)
        {
            var autoGen = _studentService.GenerateMSSV(model.IdCourse, model.IdClass);
            var getNameClass = _db.Classes.Where(x => x.IdClass == model.IdClass).Select(x => x.NameClass).FirstOrDefault();
            model.StudentCode = autoGen;
            model.NameClass = getNameClass;
            var mapStu = _mapper.Map<Student>(model);
            await _db.Students.AddAsync(mapStu);
            await _db.SaveChangesAsync();
            TempData["message"] = "Add Student Success !!!";

            return RedirectToAction("Index", "Home", new { Area = "Admin" });
        }

        [HttpGet]
        public async Task<IActionResult> Update(int id)
        {
            var getIdStu = _studentService.GetIdStudent(id);
            if (getIdStu == null) return NotFound();

            ViewData["Class"] = new SelectList(_db.Classes, "IdClass", "NameClass");
            ViewData["Depart"] = new SelectList(_db.Departments, "IdDepart", "NameDepart");
            ViewData["StuPro"] = new SelectList(_db.StudyPrograms, "IdStuPro", "NameStuPro");
            ViewData["Course"] = new SelectList(_db.Courses, "IdCourse", "NameCourse");

            return View(getIdStu);
        }

        [HttpPost]
        public async Task<IActionResult> Update(int id, StudentModelView model)
        {
            var getIdStu = _studentService.GetIdStudent(id);
            var getNameClass = _db.Classes.Where(x => x.IdClass == model.IdClass).Select(x => x.NameClass).FirstOrDefault();
            if (getIdStu == null) return NotFound();

            var mapStu = _mapper.Map<Student>(getIdStu);
            mapStu.IdStudent = id;
            mapStu.StudentCode = model.StudentCode;
            mapStu.NameClass = getNameClass;
            mapStu.FirstName = model.FirstName;
            mapStu.LastName = model.LastName;
            mapStu.Sex = model.Sex;
            mapStu.PhoneNumber = model.PhoneNumber;
            mapStu.Birthday = model.Birthday;
            mapStu.IdDepart = model.IdDepart;
            mapStu.IdClass = model.IdClass;
            mapStu.IdCourse = model.IdCourse;
            mapStu.IdStuPro = model.IdStuPro;

            _db.Students.Update(mapStu);
            await _db.SaveChangesAsync();

            return RedirectToAction("Index", "Home", new { Area = "Admin" });
        }

        [HttpGet]
        public async Task<IActionResult> Detail(int id)
        {
            var detailStu = _studentService.GetIdStudent(id);
            return View(detailStu);
        }

        [HttpDelete]
        public async Task<IActionResult> Delete(int id)
        {
            try
            {
                var getIdStu = _studentService.GetIdStudent(id);
                if (getIdStu == null) return NotFound();

                var mapStu = _mapper.Map<Student>(getIdStu);
                _db.Students.Remove(mapStu);
                await _db.SaveChangesAsync();

                return Json(Ok());
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message.ToString());
            }
        }


    }
}
