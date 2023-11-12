using BlogMVC.Enums;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Caching.Memory;
using StudentManagement.Models;
using StudentManagement.ModelsView;
using StudentManagement.Services.IAccount;

namespace StudentManagement.Controllers.Components
{
    public class AccountStudentViewComponent : ViewComponent
    {
        private readonly DbStuManageContext _db;
        private readonly IAccount _accountService;
        private IMemoryCache _memoryCache;

        public AccountStudentViewComponent(DbStuManageContext db, IMemoryCache memoryCache, IAccount accountService)
        {
            _db = db;
            _memoryCache = memoryCache;
            _accountService = accountService;
        }

        public IViewComponentResult Invoke()
        {
            //var lstCate = _memoryCache.GetOrCreate(CacheKeys.Account, entry =>
            //{
            //    entry.SlidingExpiration = TimeSpan.MaxValue;
            //    return GetAccountStu();
            //});
            return View(GetAccountStu());
        }

        public AccountModelView GetAccountStu()
        {
            var accountId = HttpContext.Session.GetString("IdAccount");
            int idAccount = int.Parse(accountId);

            return _accountService.GetIdAccount(idAccount);
        }
    }
}
