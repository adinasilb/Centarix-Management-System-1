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
        public JsonResult GetAccountsByPaymentType(int PaymentTypeID)
        {
            var companyAccountLists = _context.CompanyAccounts
                .Select(ca => new { ca.CompanyAccountID, ca.CompanyAccountNum, ca.PaymentTypeID, ca.CompanyBankName })
                .Where(ca => ca.PaymentTypeID == PaymentTypeID).ToList();
            return Json(companyAccountLists);
        }
    }
}