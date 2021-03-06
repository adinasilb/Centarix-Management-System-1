using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.ViewEngines;
using PrototypeWithAuth.Data;

namespace PrototypeWithAuth.Controllers
{
    public class CompanyAccountsController : SharedController
    {

        public CompanyAccountsController(ApplicationDbContext context, UserManager<ApplicationUser> userManager, IHostingEnvironment hostingEnvironment, IHttpContextAccessor httpContextAccessor, ICompositeViewEngine viewEngine)
            : base(context, userManager, hostingEnvironment, viewEngine, httpContextAccessor)
        {
        }
        public JsonResult GetAccountsByBank(int CompanyAccountID)
        {
            var companyAccountLists = _context.CreditCards
                .Select(cc => new { cc.CreditCardID, cc.CardNumber, cc.CompanyAccountID})
                .Where(cc => cc.CompanyAccountID == CompanyAccountID).ToList();
            return Json(companyAccountLists);
        }
    }
}