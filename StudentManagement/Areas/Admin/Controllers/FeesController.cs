using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using StudentManagement.Models;
using StudentManagement.Services.IFee;

namespace StudentManagement.Areas.Admin.Controllers
{
    [Area("Admin")]
    public class FeesController : Controller
    {
        private readonly DbStuManageContext _context;
        private readonly IFee _feeService;

        public FeesController(DbStuManageContext context, IFee feeService)
        {
            _context = context;
            _feeService = feeService;
        }

        // GET: Admin/Fees
        public async Task<IActionResult> Index()
        {
            return View(_feeService.GetAllFees());
        }

        // GET: Admin/Fees/Details/5
        public async Task<IActionResult> Details(int? id)
        {
            if (id == null || _context.Fees == null)
            {
                return NotFound();
            }

            var fee = await _context.Fees
                .Include(f => f.IdSubNavigation)
                .FirstOrDefaultAsync(m => m.IdFees == id);
            if (fee == null)
            {
                return NotFound();
            }

            return View(fee);
        }

        // GET: Admin/Fees/Create
        public IActionResult Create()
        {
            ViewData["IdSub"] = new SelectList(_context.Subjects, "IdSub", "IdSub");
            return View();
        }

        // POST: Admin/Fees/Create
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("IdFees,IdSub,Description,Amount")] Fee fee)
        {
            if (ModelState.IsValid)
            {
                _context.Add(fee);
                await _context.SaveChangesAsync();
                return RedirectToAction(nameof(Index));
            }
            ViewData["IdSub"] = new SelectList(_context.Subjects, "IdSub", "IdSub", fee.IdSub);
            return View(fee);
        }

        // GET: Admin/Fees/Edit/5
        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null || _context.Fees == null)
            {
                return NotFound();
            }

            var fee = await _context.Fees.FindAsync(id);
            if (fee == null)
            {
                return NotFound();
            }
            ViewData["IdSub"] = new SelectList(_context.Subjects, "IdSub", "IdSub", fee.IdSub);
            return View(fee);
        }

        // POST: Admin/Fees/Edit/5
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, [Bind("IdFees,IdSub,Description,Amount")] Fee fee)
        {
            if (id != fee.IdFees)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                try
                {
                    _context.Update(fee);
                    await _context.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!FeeExists(fee.IdFees))
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
            ViewData["IdSub"] = new SelectList(_context.Subjects, "IdSub", "IdSub", fee.IdSub);
            return View(fee);
        }

        // GET: Admin/Fees/Delete/5
        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null || _context.Fees == null)
            {
                return NotFound();
            }

            var fee = await _context.Fees
                .Include(f => f.IdSubNavigation)
                .FirstOrDefaultAsync(m => m.IdFees == id);
            if (fee == null)
            {
                return NotFound();
            }

            return View(fee);
        }

        // POST: Admin/Fees/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            if (_context.Fees == null)
            {
                return Problem("Entity set 'DbStuManageContext.Fees'  is null.");
            }
            var fee = await _context.Fees.FindAsync(id);
            if (fee != null)
            {
                _context.Fees.Remove(fee);
            }
            
            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }

        private bool FeeExists(int id)
        {
          return (_context.Fees?.Any(e => e.IdFees == id)).GetValueOrDefault();
        }
    }
}
