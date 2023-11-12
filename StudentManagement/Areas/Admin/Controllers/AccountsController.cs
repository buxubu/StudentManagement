using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Claims;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using StudentManagement.Models;
using StudentManagement.ModelsView;
using StudentManagement.Services.IAccount;
using BlogMVC.Extentions;
using System.Security.Principal;
using Microsoft.AspNetCore.Authorization;

namespace StudentManagement.Areas.Admin.Controllers
{
    [Area("Admin")]
    [Authorize(Roles = "Admin")]
    public class AccountsController : Controller
    {
        private readonly DbStuManageContext _db;
        private readonly IAccount _accountService;

        public AccountsController(DbStuManageContext context, IAccount accountService)
        {
            _db = context;
            _accountService = accountService;
        }
        [HttpGet]
        [AllowAnonymous]
        public async Task<IActionResult> Login()
        {
            //var salt = Share.GetSailt();
            //var pass = Share.ToMD5("phucray1310");

            ClaimsPrincipal claimUser = HttpContext.User;
            if (claimUser.Identity.IsAuthenticated)
            {
                return RedirectToAction("Index", "Home", new { Area = "Admin" });
            }
            return View();

            /*var idAccount = HttpContext.Session.GetString("IdAccount");
            if (idAccount != null) return RedirectToAction("Login", "Accounts", new { Area = "Admin" });*/
        }
        [HttpPost]
        [AllowAnonymous]
        //[Route("/dang-nhap.html", Name = "Login")]
        public async Task<IActionResult> Login(LoginModelView model)
        {
            try
            {
                if (ModelState.IsValid)
                {
                    var checkAccount = await _db.Accounts.SingleOrDefaultAsync(x => x.Username == model.Username.ToLower().Trim());

                    if (checkAccount == null)
                    {
                        ViewData["ErrorClaims"] = "Login information is incorrect";
                        return View(model);
                    }

                    bool pass = (Share.ToMD5(model.Password.Trim()) + checkAccount.Salt) == checkAccount.Password;
                    if (!pass)
                    {
                        ViewData["ErrorClaims"] = "Login information is incorrect";
                        return View(model);
                    }

                    checkAccount.LastLogin = DateTime.Now;

                    _db.Accounts.Update(checkAccount);
                    await _db.SaveChangesAsync();

                    //save session
                    HttpContext.Session.SetString("IdAccount", checkAccount.IdAccount.ToString());

                    var getNameRole = _db.Roles.Where(x => x.IdRole == checkAccount.IdRole).FirstOrDefault().NameRole;

                    //identity
                    var accountClaims = new List<Claim>
                    {
                    new Claim(ClaimTypes.Name, model.Username,ClaimValueTypes.String),
                    new Claim("IdAccount", checkAccount.IdAccount.ToString(),ClaimValueTypes.Integer),
                    new Claim(ClaimTypes.Role, getNameRole)
                    };

                    ClaimsIdentity claimsIdentity = new ClaimsIdentity(accountClaims,
                        CookieAuthenticationDefaults.AuthenticationScheme
                        );

                    AuthenticationProperties properties = new AuthenticationProperties()
                    {
                        AllowRefresh = true,
                        IsPersistent = model.KeepMeLogin
                    };

                    await HttpContext.SignInAsync(CookieAuthenticationDefaults.AuthenticationScheme,
                        new ClaimsPrincipal(claimsIdentity), properties);


                    ViewData["ErrorClaims"] = "Error to save claims";
                    if (getNameRole == "Admin")
                    {
                        return RedirectToAction("Index", "HomeDashboard", new { Area = "Admin" });
                    }
                    else if (getNameRole == "Student")
                    {
                        return RedirectToAction("Index", "Students", new { Area = "" });
                    }

                }

                return RedirectToAction("Login", "Accounts", new { Area = "Admin" });
            }
            catch
            {
                return RedirectToAction("Login", "Accounts", new { Area = "Admin" });
            }
        }

        [HttpGet]
        public async Task<IActionResult> GetAllAccount(string? condition, int? page, int pageSize)
        {
            page = page.HasValue ? page.Value > 0 ? page.Value : 1 : 1;
            pageSize = pageSize == 0 ? 5 : pageSize;

            return View(_accountService.GetAllAccount());

        }

        public async Task<IActionResult> LogOut()
        {
            await HttpContext.SignOutAsync(CookieAuthenticationDefaults.AuthenticationScheme);
            HttpContext.Session.Remove("AccountId");
            return RedirectToAction("Login", "Accounts", new { Area = "Admin" });
        }

        // GET: Admin/Accounts/Details/5
        public async Task<IActionResult> Details(int? id)
        {
            if (id == null || _db.Accounts == null)
            {
                return NotFound();
            }

            var account = await _db.Accounts
                .Include(a => a.IdRoleNavigation)
                .Include(a => a.IdStudentNavigation)
                .FirstOrDefaultAsync(m => m.IdAccount == id);
            if (account == null)
            {
                return NotFound();
            }

            return View(account);
        }

        // GET: Admin/Accounts/Create
        public IActionResult Create()
        {
            ViewData["IdRole"] = new SelectList(_db.Roles, "IdRole", "IdRole");
            ViewData["IdStudent"] = new SelectList(_db.Students, "IdStudent", "IdStudent");
            return View();
        }

        // POST: Admin/Accounts/Create
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("IdAccount,IdRole,IdStudent,Username,Password,Salt,Active,CreateDate,LastLogin,NameRole")] Account account)
        {
            if (ModelState.IsValid)
            {
                _db.Add(account);
                await _db.SaveChangesAsync();
                return RedirectToAction(nameof(Index));
            }
            ViewData["IdRole"] = new SelectList(_db.Roles, "IdRole", "IdRole", account.IdRole);
            ViewData["IdStudent"] = new SelectList(_db.Students, "IdStudent", "IdStudent", account.IdStudent);
            return View(account);
        }

        // GET: Admin/Accounts/Edit/5
        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null || _db.Accounts == null)
            {
                return NotFound();
            }

            var account = await _db.Accounts.FindAsync(id);
            if (account == null)
            {
                return NotFound();
            }
            ViewData["IdRole"] = new SelectList(_db.Roles, "IdRole", "IdRole", account.IdRole);
            ViewData["IdStudent"] = new SelectList(_db.Students, "IdStudent", "IdStudent", account.IdStudent);
            return View(account);
        }

        // POST: Admin/Accounts/Edit/5
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, [Bind("IdAccount,IdRole,IdStudent,Username,Password,Salt,Active,CreateDate,LastLogin,NameRole")] Account account)
        {
            if (id != account.IdAccount)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                try
                {
                    _db.Update(account);
                    await _db.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!AccountExists(account.IdAccount))
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
            ViewData["IdRole"] = new SelectList(_db.Roles, "IdRole", "IdRole", account.IdRole);
            ViewData["IdStudent"] = new SelectList(_db.Students, "IdStudent", "IdStudent", account.IdStudent);
            return View(account);
        }

        // GET: Admin/Accounts/Delete/5
        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null || _db.Accounts == null)
            {
                return NotFound();
            }

            var account = await _db.Accounts
                .Include(a => a.IdRoleNavigation)
                .Include(a => a.IdStudentNavigation)
                .FirstOrDefaultAsync(m => m.IdAccount == id);
            if (account == null)
            {
                return NotFound();
            }

            return View(account);
        }

        // POST: Admin/Accounts/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            if (_db.Accounts == null)
            {
                return Problem("Entity set 'DbStuManageContext.Accounts'  is null.");
            }
            var account = await _db.Accounts.FindAsync(id);
            if (account != null)
            {
                _db.Accounts.Remove(account);
            }

            await _db.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }

        private bool AccountExists(int id)
        {
            return (_db.Accounts?.Any(e => e.IdAccount == id)).GetValueOrDefault();
        }
    }
}
