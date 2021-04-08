using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using PrototypeWithAuth.Data;

namespace PrototypeWithAuth.Controllers
{
    public class CompanyAccountsController : SharedController
    {
        private readonly ApplicationDbContext _context;

        public CompanyAccountsController(ApplicationDbContext context) : base(context)
        {
            _context = context;
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