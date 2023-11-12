using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Threading.Tasks;
using AutoMapper;
using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using StudentManagement.Models;
using StudentManagement.ModelsView;
using StudentManagement.Services.IStudent;
using Stripe.Checkout;

namespace StudentManagement.Controllers
{
    [Authorize(Roles = "Student")]
    public class StudentsController : Controller
    {
        private readonly DbStuManageContext _db;
        private readonly IMapper _mapper;
        private readonly IStudent _studentService;
        public StudentsController(DbStuManageContext db, IMapper mapper, IStudent studentService)
        {
            _db = db;
            _mapper = mapper;
            _studentService = studentService;
        }

        // GET: Students
        public async Task<IActionResult> Index()
        {
            var accountId = HttpContext.Session.GetString("IdAccount");
            if (accountId == null) return RedirectToAction("Login", "Accounts", new { Area = "Admin" });

            var parseId = int.Parse(accountId);
            var detailStu = _studentService.GetIdStudent(parseId);
            return View(detailStu);
        }

        public async Task<IActionResult> StudentFee()
        {
            var accountId = HttpContext.Session.GetString("IdAccount");
            if (accountId == null) return RedirectToAction("Login", "Accounts", new { Area = "Admin" });
            var parseId = int.Parse(accountId);

            return View(_studentService.GettAllFeeStudent(parseId));
        }

        public async Task<IActionResult> CheckOut()
        {
            var accountId = HttpContext.Session.GetString("IdAccount");
            if (accountId == null) return RedirectToAction("Login", "Accounts", new { Area = "Admin" });
            var parseId = int.Parse(accountId);
            var getStuFee = _studentService.GettAllFeeStudent(parseId);

            var domain = "https://localhost:7185/";
            var option = new SessionCreateOptions
            {
                SuccessUrl = domain + $"Students/StudentFee",
                CancelUrl = domain + $"Admin/Accounts/Login",
                LineItems = new List<SessionLineItemOptions>(),
                Mode = "payment"
            };

            double allTotal = 0;
            foreach (var item in getStuFee)
            {
                allTotal += (double)(item.TotalFee);
                var sessionList = new SessionLineItemOptions
                {
                    PriceData = new SessionLineItemPriceDataOptions
                    {
                        UnitAmount = (long)item.TotalFee,
                        Currency = "inr",
                        ProductData = new SessionLineItemPriceDataProductDataOptions
                        {
                            Name = item.NameSub
                        }
                    },
                    Quantity = 1
                };
                option.LineItems.Add(sessionList);
            }
            var service = new SessionService();
            Session session = service.Create(option);
            HttpContext.Response.Headers.Add("Location", session.Url);


            foreach (var item in getStuFee)
            {
                var getStuFees = _db.StudentFees.Where(x => x.IdStuFee == item.IdStuFee).FirstOrDefault();

                getStuFees.IdStuFee = item.IdStuFee;
                getStuFees.IdFees = item.IdFees;
                getStuFees.IdStudent = parseId;
                getStuFees.TotalFee = 0;
                getStuFees.DateCreate = DateTime.Now;

                _db.StudentFees.Update(getStuFees);
                _db.SaveChanges();
            }
            

            return new StatusCodeResult(303);
        }

        public async Task<IActionResult> LogOut()
        {
            await HttpContext.SignOutAsync(CookieAuthenticationDefaults.AuthenticationScheme);
            HttpContext.Session.Remove("AccountId");
            return RedirectToAction("Login", "Accounts", new { Area = "Admin" });
        }

        // GET: Students/Details/5
        public async Task<IActionResult> Details(int? id)
        {
            if (id == null || _db.Students == null)
            {
                return NotFound();
            }

            var student = await _db.Students
                .Include(s => s.IdClassNavigation)
                .Include(s => s.IdCourseNavigation)
                .Include(s => s.IdDepartNavigation)
                .Include(s => s.IdStuProNavigation)
                .FirstOrDefaultAsync(m => m.IdStudent == id);
            if (student == null)
            {
                return NotFound();
            }

            return View(student);
        }

        // GET: Students/Create
        public IActionResult Create()
        {
            ViewData["IdClass"] = new SelectList(_db.Classes, "IdClass", "IdClass");
            ViewData["IdCourse"] = new SelectList(_db.Courses, "IdCourse", "IdCourse");
            ViewData["IdDepart"] = new SelectList(_db.Departments, "IdDepart", "IdDepart");
            ViewData["IdStuPro"] = new SelectList(_db.StudyPrograms, "IdStuPro", "IdStuPro");
            return View();
        }

        // POST: Students/Create
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("IdStudent,IdDepart,IdCourse,IdStuPro,IdClass,NameClass,StudentCode,FirstName,LastName,Sex,PhoneNumber,Birthday")] Student student)
        {
            if (ModelState.IsValid)
            {
                _db.Add(student);
                await _db.SaveChangesAsync();
                return RedirectToAction(nameof(Index));
            }
            ViewData["IdClass"] = new SelectList(_db.Classes, "IdClass", "IdClass", student.IdClass);
            ViewData["IdCourse"] = new SelectList(_db.Courses, "IdCourse", "IdCourse", student.IdCourse);
            ViewData["IdDepart"] = new SelectList(_db.Departments, "IdDepart", "IdDepart", student.IdDepart);
            ViewData["IdStuPro"] = new SelectList(_db.StudyPrograms, "IdStuPro", "IdStuPro", student.IdStuPro);
            return View(student);
        }

        // GET: Students/Edit/5
        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null || _db.Students == null)
            {
                return NotFound();
            }

            var getIdStu = _studentService.GetIdStudent(id.Value);
            if (getIdStu == null) return NotFound();

            ViewData["Class"] = new SelectList(_db.Classes, "IdClass", "NameClass");
            ViewData["Depart"] = new SelectList(_db.Departments, "IdDepart", "NameDepart");
            ViewData["StuPro"] = new SelectList(_db.StudyPrograms, "IdStuPro", "NameStuPro");
            ViewData["Course"] = new SelectList(_db.Courses, "IdCourse", "NameCourse");

            return View(getIdStu);
        }

        // POST: Students/Edit/5
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, [Bind("IdStudent,IdDepart,IdCourse,IdStuPro,IdClass,NameClass,StudentCode,FirstName,LastName,Sex,PhoneNumber,Birthday")] StudentModelView model)
        {


            //if (ModelState.IsValid)
            //{
            try
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

                return RedirectToAction("Index", "Students", new { Area = "" });
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!StudentExists(model.IdStudent))
                {
                    return NotFound();
                }
            }
            return RedirectToAction(nameof(Index));
            //}
            //else
            //{
            //    return View(model);
            //}
            //ViewData["IdClass"] = new SelectList(_db.Classes, "IdClass", "IdClass", student.IdClass);
            //ViewData["IdCourse"] = new SelectList(_db.Courses, "IdCourse", "IdCourse", student.IdCourse);
            //ViewData["IdDepart"] = new SelectList(_db.Departments, "IdDepart", "IdDepart", student.IdDepart);
            //ViewData["IdStuPro"] = new SelectList(_db.StudyPrograms, "IdStuPro", "IdStuPro", student.IdStuPro);
        }

        // GET: Students/Delete/5
        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null || _db.Students == null)
            {
                return NotFound();
            }

            var student = await _db.Students
                .Include(s => s.IdClassNavigation)
                .Include(s => s.IdCourseNavigation)
                .Include(s => s.IdDepartNavigation)
                .Include(s => s.IdStuProNavigation)
                .FirstOrDefaultAsync(m => m.IdStudent == id);
            if (student == null)
            {
                return NotFound();
            }

            return View(student);
        }

        // POST: Students/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            if (_db.Students == null)
            {
                return Problem("Entity set 'DbStuManageContext.Students'  is null.");
            }
            var student = await _db.Students.FindAsync(id);
            if (student != null)
            {
                _db.Students.Remove(student);
            }

            await _db.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }

        private bool StudentExists(int id)
        {
            return (_db.Students?.Any(e => e.IdStudent == id)).GetValueOrDefault();
        }
    }
}
