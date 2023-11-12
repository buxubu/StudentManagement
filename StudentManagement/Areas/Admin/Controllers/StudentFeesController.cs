using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using StudentManagement.Models;
using StudentManagement.Services.IStudentFee;

namespace StudentManagement.Areas.Admin.Controllers
{
    [Area("Admin")]
    public class StudentFeesController : Controller
    {
        private readonly DbStuManageContext _db;
        private readonly IStudentFee _studentFeeService;

        public StudentFeesController(DbStuManageContext context, IStudentFee studentFeeService)
        {
            _db = context;
            _studentFeeService = studentFeeService;
        }

        // GET: Admin/StudentFees
        public async Task<IActionResult> Index()
        {
            return View(_studentFeeService.GetAllStudentFee());
        }

        // GET: Admin/StudentFees/Details/5
        public async Task<IActionResult> Details(int? id)
        {
            if (id == null || _db.StudentFees == null)
            {
                return NotFound();
            }

            var studentFee = await _db.StudentFees
                .Include(s => s.IdFeesNavigation)
                .Include(s => s.IdStudentNavigation)
                .FirstOrDefaultAsync(m => m.IdStuFee == id);
            if (studentFee == null)
            {
                return NotFound();
            }

            return View(studentFee);
        }

        // GET: Admin/StudentFees/Create
        public IActionResult Create()
        {
            ViewData["IdFees"] = new SelectList(_db.Fees, "IdFees", "IdFees");
            ViewData["IdStudent"] = new SelectList(_db.Students, "IdStudent", "IdStudent");
            return View();
        }

        // POST: Admin/StudentFees/Create
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("IdStuFee,IdStudent,IdFees,TotalFee,DateCreate")] StudentFee studentFee)
        {
            if (ModelState.IsValid)
            {
                _db.Add(studentFee);
                await _db.SaveChangesAsync();
                return RedirectToAction(nameof(Index));
            }
            ViewData["IdFees"] = new SelectList(_db.Fees, "IdFees", "IdFees", studentFee.IdFees);
            ViewData["IdStudent"] = new SelectList(_db.Students, "IdStudent", "IdStudent", studentFee.IdStudent);
            return View(studentFee);
        }

        // GET: Admin/StudentFees/Edit/5
        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null || _db.StudentFees == null)
            {
                return NotFound();
            }

            var studentFee = await _db.StudentFees.FindAsync(id);
            if (studentFee == null)
            {
                return NotFound();
            }
            ViewData["IdFees"] = new SelectList(_db.Fees, "IdFees", "IdFees", studentFee.IdFees);
            ViewData["IdStudent"] = new SelectList(_db.Students, "IdStudent", "IdStudent", studentFee.IdStudent);
            return View(studentFee);
        }

        // POST: Admin/StudentFees/Edit/5
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, [Bind("IdStuFee,IdStudent,IdFees,TotalFee,DateCreate")] StudentFee studentFee)
        {
            if (id != studentFee.IdStuFee)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                try
                {
                    _db.Update(studentFee);
                    await _db.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!StudentFeeExists(studentFee.IdStuFee))
                    {
                        return NotFound();
                    }
                    else
                    {
                        throw;
                    }
                }
                return RedirectToAction(nameof(Index));
            }
            ViewData["IdFees"] = new SelectList(_db.Fees, "IdFees", "IdFees", studentFee.IdFees);
            ViewData["IdStudent"] = new SelectList(_db.Students, "IdStudent", "IdStudent", studentFee.IdStudent);
            return View(studentFee);
        }

        // GET: Admin/StudentFees/Delete/5
        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null || _db.StudentFees == null)
            {
                return NotFound();
            }

            var studentFee = await _db.StudentFees
                .Include(s => s.IdFeesNavigation)
                .Include(s => s.IdStudentNavigation)
                .FirstOrDefaultAsync(m => m.IdStuFee == id);
            if (studentFee == null)
            {
                return NotFound();
            }

            return View(studentFee);
        }

        // POST: Admin/StudentFees/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            if (_db.StudentFees == null)
            {
                return Problem("Entity set 'DbStuManageContext.StudentFees'  is null.");
            }
            var studentFee = await _db.StudentFees.FindAsync(id);
            if (studentFee != null)
            {
                _db.StudentFees.Remove(studentFee);
            }

            await _db.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }

        private bool StudentFeeExists(int id)
        {
            return (_db.StudentFees?.Any(e => e.IdStuFee == id)).GetValueOrDefault();
        }
    }
}
