using System;
using System.Collections;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.IO;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using Newtonsoft.Json;
using PrototypeWithAuth.AppData;
using PrototypeWithAuth.Data;
using PrototypeWithAuth.Models;
using PrototypeWithAuth.ViewModels;
using X.PagedList;

namespace PrototypeWithAuth.Controllers
{
    public class ParentRequestsController : Controller
    {
        private readonly ApplicationDbContext _context;

        public ParentRequestsController(ApplicationDbContext context)
        {
            _context = context;
        }

     [HttpGet]
        [Authorize(Roles = "Accounting")]
        public async Task<IActionResult> GeneralPaymentList()
        {

            IEnumerable<ParentRequest> fullParentRequestsList = _context.ParentRequests.Where(pr=>pr.Requests.Count()!=0)
                .Include(pr => pr.ApplicationUser).Include(pr => pr.Requests).ThenInclude(r => r.Product).ThenInclude(p => p.ProductSubcategory)
                .ThenInclude(ps => ps.ParentCategory).Include(pr => pr.Requests).ThenInclude(r => r.Product).ThenInclude(pr => pr.Vendor)
                .Include(pr => pr.Requests).ThenInclude( r=> r.UnitType)
                .Include(pr => pr.Requests).ThenInclude(r => r.SubUnitType)
                .Include(pr => pr.Requests).ThenInclude(r => r.SubSubUnitType)
                ;

            var fullParentRequestsListByDate = fullParentRequestsList
                        .OrderByDescending(f => f.OrderDate.Date)
                        .ThenBy(f => f.OrderDate.TimeOfDay);


            //tempdata page type for active tab link
            TempData[AppUtility.TempDataTypes.MenuType.ToString()] = AppUtility.MenuItems.Accounting;
            TempData[AppUtility.TempDataTypes.PageType.ToString()] = AppUtility.PageTypeEnum.AccountingGeneral;
            TempData[AppUtility.TempDataTypes.SidebarType.ToString()] = AppUtility.SidebarEnum.General;

            return View(await fullParentRequestsListByDate.ToListAsync());
        }


        [Authorize(Roles = "Accounting")]
        public async Task<IActionResult> ExpensesList()
        {
            List<MonthlyTotalsViewModel> monthlyTotals = new List<MonthlyTotalsViewModel>();

            var requestsByMonthAndYear = _context.Requests.Select(r => new { Year = r.ParentRequest.OrderDate.Year, Month = r.ParentRequest.OrderDate.Month }).Distinct();
            int year;
            if (_context.Requests.IsEmpty())
            {
                year = 0;
            }
            else
            {
                year = requestsByMonthAndYear.FirstOrDefault().Year;
            }

            foreach (var req in requestsByMonthAndYear)
            {
                var requestsinMonthAndYear = _context.Requests.Include(r => r.ParentRequest).Include(r => r.Product).ThenInclude(p => p.ProductSubcategory).ThenInclude(p => p.ParentCategory).Where(r => r.ParentRequest.OrderDate.Year == req.Year).Where(r => r.ParentRequest.OrderDate.Month == req.Month).ToList();
                MonthlyTotalsViewModel monthlyTotalsViewModel = new MonthlyTotalsViewModel();
                monthlyTotalsViewModel.Month = req.Month;
                monthlyTotalsViewModel.Year = req.Year;
                monthlyTotalsViewModel.GrandTotal = requestsinMonthAndYear.Sum(r => r.Cost);
                monthlyTotalsViewModel.PlasticsTotal = requestsinMonthAndYear.Where(r => r.Product.ProductSubcategory.ParentCategoryID == 1).AsEnumerable().Sum(m => m.Cost);
                monthlyTotalsViewModel.ReagentsTotal = requestsinMonthAndYear.Where(r => r.Product.ProductSubcategory.ParentCategoryID == 2).AsEnumerable().Sum(m => m.Cost);
                monthlyTotalsViewModel.ProprietyTotal = requestsinMonthAndYear.Where(r => r.Product.ProductSubcategory.ParentCategoryID == 3).AsEnumerable().Sum(m => m.Cost);
                monthlyTotalsViewModel.ReusableTotal = requestsinMonthAndYear.Where(r => r.Product.ProductSubcategory.ParentCategoryID == 4).AsEnumerable().Sum(m => m.Cost);
                monthlyTotalsViewModel.EquipmentTotal = requestsinMonthAndYear.Where(r => r.Product.ProductSubcategory.ParentCategoryID == 5).AsEnumerable().Sum(m => m.Cost);
                monthlyTotalsViewModel.OperationsTotal = requestsinMonthAndYear.Where(r => r.Product.ProductSubcategory.ParentCategoryID == 6).AsEnumerable().Sum(m => m.Cost);
                monthlyTotals.Add(monthlyTotalsViewModel);
            }

            ExpensesListViewModel expensesListViewModel = new ExpensesListViewModel
            {
                monthlyTotals = monthlyTotals
            };
            //tempdata page type for active tab link
            TempData[AppUtility.TempDataTypes.PageType.ToString()] = AppUtility.PageTypeEnum.AccountingExpenses;


            return View(expensesListViewModel);
        }

        // POST: ParentRequests/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        [Authorize(Roles = "Accounting")]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            var parentRequest = await _context.ParentRequests.FindAsync(id);
            _context.ParentRequests.Remove(parentRequest);
            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }

        private bool ParentRequestExists(int id)
        {
            return _context.ParentRequests.Any(e => e.ParentRequestID == id);
        }
        [HttpGet]
        [Authorize(Roles = "Accounting")]
       
       


  
        //this is here b/c the ajax call on the payment view is not working and I didn't have time to debug it
        [HttpGet]
        [Authorize(Roles = "Accounting")]
        public IActionResult DetailsModalView(int id)
        {
            return RedirectToAction("DetailsModalView", "Requests", new { id = id });
        }

    }
}
