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

        // GET: ParentRequests
        [HttpGet]
        [Authorize(Roles = "Admin, Accounting")]
        public async Task<IActionResult> Index(int? subcategoryID, int? vendorID, int? RequestStatusID, int? page, AppUtility.PaymentPageTypeEnum PageType = AppUtility.PaymentPageTypeEnum.Notifications)
        {
            IEnumerable<ParentRequest> fullParentRequestsList = _context.ParentRequests.Include(pr => pr.ApplicationUser).Include(pr => pr.Requests).ThenInclude(pr => pr.Product).ThenInclude(pr => pr.ProductSubcategory).ThenInclude(pr => pr.ParentCategory).Include(pr => pr.Requests).ThenInclude(pr => pr.Product).ThenInclude(pr => pr.Vendor);

            //if (PageType == AppUtility.PaymentPageTypeEnum.Notifications)
            //{

            List<List<ParentRequest>> ReturnParentRequestList = new List<List<ParentRequest>>();
            List<ParentRequest> NotPayedList = new List<ParentRequest>();
            List<ParentRequest> NoInvoiceList = new List<ParentRequest>();
            List<ParentRequest> DidntArriveList = new List<ParentRequest>();
            List<ParentRequest> PartialDeliveryList = new List<ParentRequest>();
            List<ParentRequest> ForClarification = new List<ParentRequest>();
            List<ParentRequest> PayNow = new List<ParentRequest>();


            foreach (var parentRequest in fullParentRequestsList)
            {
                if (parentRequest.Payed != true)
                {
                    NotPayedList.Add(parentRequest);
                }
                if (parentRequest.InvoiceNumber == null)
                {
                    NoInvoiceList.Add(parentRequest);
                }
                //need to test and have code review, is this the best way to do it?? have O^2 run time
                foreach (var request in parentRequest.Requests)
                {
                    if (request.RequestStatusID == 2) //Request Stat ID of 2 equals Ordered
                    {
                        DidntArriveList.Add(request.ParentRequest);
                        break; //make sure break doesnt leave the outer-foreach
                    }

                }
                foreach (var request in parentRequest.Requests)
                {
                    if (request.RequestStatusID == 4) //Request Stat ID of 4 equals Partial
                    {
                        PartialDeliveryList.Add(request.ParentRequest);
                        break;
                    }

                }
                foreach (var request in parentRequest.Requests)
                {
                    if (request.RequestStatusID == 5) //Request Stat ID of 5 equals Clarify
                    {
                        ForClarification.Add(request.ParentRequest);
                        break;
                    }

                }
                foreach (var request in parentRequest.Requests)
                {
                    if (request.RequestStatusID == 6) //Request Stat ID of 6 equals Pay Now
                    {
                        PayNow.Add(request.ParentRequest);
                        break;
                    }

                }

                // if ordered, can also include if not  payed or if no invioce..
                //partial,  can also include if not  payed or if no invioce..
                //clarify,  can also include if not  payed or if no invioce..

            }

            Dictionary<string, int> titleList = new Dictionary<string, int>
            {
                { "To Pay", NotPayedList.Count },
                { "No Invoice", NoInvoiceList.Count },
                { "Didn't Arrive", DidntArriveList.Count },
                { "Pay Now", PayNow.Count },
                { "Partial Delivery", PartialDeliveryList.Count },
                { "For Clarification", ForClarification.Count }

            };

            ReturnParentRequestList.Add(NotPayedList);
            ReturnParentRequestList.Add(NoInvoiceList);
            ReturnParentRequestList.Add(DidntArriveList);
            ReturnParentRequestList.Add(PartialDeliveryList);
            ReturnParentRequestList.Add(ForClarification);
            ReturnParentRequestList.Add(PayNow);

            PaymentNotificationsViewModel paymentNotificationsViewModel = new PaymentNotificationsViewModel
            {
                TitleList = titleList,
                ParentRequests = await ReturnParentRequestList.ToListAsync()
            };

            //tempdata page type for active tab link
            TempData["PageType"] = AppUtility.PaymentPageTypeEnum.Notifications;


            return View(paymentNotificationsViewModel);
        }
        [HttpGet]
        [Authorize(Roles = "Admin, Accounting")]
        public async Task<IActionResult> GeneralPaymentList()
        {

            IEnumerable<ParentRequest> fullParentRequestsList = _context.ParentRequests
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
            TempData["PageType"] = AppUtility.PaymentPageTypeEnum.General;

            return View(await fullParentRequestsListByDate.ToListAsync());
        }


        [Authorize(Roles = "Admin, Accounting")]
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
            TempData["PageType"] = AppUtility.PaymentPageTypeEnum.Expenses;


            return View(expensesListViewModel);
        }


        // GET: ParentRequests/Details/5
        [Authorize(Roles = "Admin, Accounting")]
        public async Task<IActionResult> Details(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var parentRequest = await _context.ParentRequests
                .Include(p => p.ApplicationUser)
                .FirstOrDefaultAsync(m => m.ParentRequestID == id);
            if (parentRequest == null)
            {
                return NotFound();
            }

            //Need tempdata for request nav view???

            return View(parentRequest);
        }

        // GET: ParentRequests/Create
        [Authorize(Roles = "Admin, Accounting")]
        public IActionResult Create()
        {
            ViewData["ApplicationUserID"] = new SelectList(_context.Users, "Id", "Id");

            //need tempdata for requestnavview??

            return View();
        }

        // POST: ParentRequests/Create
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        [Authorize(Roles = "Admin, Accounting")]
        public async Task<IActionResult> Create([Bind("ParentRequestID,ApplicationUserID,OrderDate,OrderNumber,InvoiceNumber,InvoiceDate")] ParentRequest parentRequest)
        {
            if (ModelState.IsValid)
            {
                _context.Add(parentRequest);
                await _context.SaveChangesAsync();
                return RedirectToAction(nameof(Index));
            }
            ViewData["ApplicationUserID"] = new SelectList(_context.Users, "Id", "Id", parentRequest.ApplicationUserID);

            return View(parentRequest);
        }

        // GET: ParentRequests/Edit/5
        [Authorize(Roles = "Admin, Accounting")]
        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var parentRequest = await _context.ParentRequests.FindAsync(id);
            if (parentRequest == null)
            {
                return NotFound();
            }
            ViewData["ApplicationUserID"] = new SelectList(_context.Users, "Id", "Id", parentRequest.ApplicationUserID);


            //tempdata page type for active tab link
            //put it in


            return View(parentRequest);
        }

        // POST: ParentRequests/Edit/5
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        [Authorize(Roles = "Admin, Accounting")]
        public async Task<IActionResult> Edit(int id, [Bind("ParentRequestID,ApplicationUserID,OrderDate,OrderNumber,InvoiceNumber,InvoiceDate")] ParentRequest parentRequest)
        {
            if (id != parentRequest.ParentRequestID)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                try
                {
                    _context.Update(parentRequest);
                    await _context.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!ParentRequestExists(parentRequest.ParentRequestID))
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
            ViewData["ApplicationUserID"] = new SelectList(_context.Users, "Id", "Id", parentRequest.ApplicationUserID);
            return View(parentRequest);
        }

        // GET: ParentRequests/Delete/5
        [Authorize(Roles = "Admin, Accounting")]
        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var parentRequest = await _context.ParentRequests
                .Include(p => p.ApplicationUser)
                .FirstOrDefaultAsync(m => m.ParentRequestID == id);
            if (parentRequest == null)
            {
                return NotFound();
            }

            return View(parentRequest);
        }

        // POST: ParentRequests/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        [Authorize(Roles = "Admin, Accounting")]
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
        [Authorize(Roles = "Admin, Accounting")]
        public async Task<IActionResult> Payments(AppUtility.PaymentsEnum id)
        {
            TempData["PageType"] = AppUtility.PaymentPageTypeEnum.Payments;
            NotificationsListViewModel notificationsListViewModel = new NotificationsListViewModel();
            Dictionary<Vendor, List<ParentRequestListViewModel>> ParentRequestsFiltered;
            switch (id)
            {
                case AppUtility.PaymentsEnum.ToPay:
                    TempData["Action"] = "ToPay";
                    //var test = new DateTime(2014, 3, 15, 5, 4, 9) - DateTime.Today;
                    //var begDT = new DateTime();
                    var fullListOfParentRequests = await _context.ParentRequests
                        //.Where(pr => pr.OrderDate!= null && pr.Requests.FirstOrDefault().Terms != null)
                        .Select(pr => new ParentRequestWithPayByDateViewModel
                        {
                            ParentRequest = pr,
                            //Terms = pr.Requests.FirstOrDefault().Terms,
                            PayByDate = (DateTime?)pr.OrderDate.AddDays((double)pr.Requests.FirstOrDefault().Terms)
                        })
                        .ToListAsync();
                    var parentRequestIds = fullListOfParentRequests
                        .Where(pr => /*pr.DateToBePaid != null &&*/ pr.PayByDate > DateTime.Today || pr.PayByDate == null)
                        /*
                         * Right now To Pay is all requests with a NULL PayByDate or where the PayByDate is in the future
                         * 1. should we take out the nulls?
                         * 2. should we put in if the PayByDate is today?
                         */
                        .Select(pr => pr.ParentRequest.ParentRequestID).ToList();
                    ParentRequestsFiltered = _context.ParentRequests
                        .Where(pr => parentRequestIds.Contains(pr.ParentRequestID))
                        .Select(pr => new ParentRequestListViewModel
                        {
                            ParentRequest = pr,
                            Request = pr.Requests.FirstOrDefault(),
                            Product = pr.Requests.FirstOrDefault().Product,
                            ProductSubcategory = pr.Requests.FirstOrDefault().Product.ProductSubcategory,
                            Vendor = pr.Requests.FirstOrDefault().Product.Vendor,
                            ParentCategory = pr.Requests.FirstOrDefault().Product.ProductSubcategory.ParentCategory,
                            UnitType = pr.Requests.FirstOrDefault().UnitType,
                            SubUnitType = pr.Requests.FirstOrDefault().SubUnitType,
                            SubSubUnitType = pr.Requests.FirstOrDefault().SubSubUnitType
                        }).ToList().GroupBy(pr => pr.Vendor).ToDictionary(pr => pr.Key,
                               pr => pr.ToList()
                            );
                    notificationsListViewModel.ParentRequestList = ParentRequestsFiltered;
                    break;

                case AppUtility.PaymentsEnum.PayNow:
                    TempData["Action"] = "PayNow";
                    var fullListOfParentRequestsDidNotArrive = await _context.ParentRequests
                       //.Where(pr => pr.OrderDate!= null && pr.Requests.FirstOrDefault().Terms != null)
                       .Select(pr => new ParentRequestWithPayByDateViewModel
                       {
                           ParentRequest = pr,
                           //Terms = pr.Requests.FirstOrDefault().Terms,
                           PayByDate = (DateTime?)pr.OrderDate.AddDays((double)pr.Requests.FirstOrDefault().Terms)
                       })
                       .ToListAsync();
                    var parentRequestIdsDidNotArrive = fullListOfParentRequestsDidNotArrive
                        .Where(pr => /*pr.DateToBePaid != null &&*/ pr.PayByDate == DateTime.Today)
                        /*
                         * Right now PAY NOW is taking all requests that are due today. Should it be tomorrow also or sometime this week?
                         */
                        .Select(pr => pr.ParentRequest.ParentRequestID).ToList();
                    ParentRequestsFiltered = _context.ParentRequests
                        .Where(pr => parentRequestIdsDidNotArrive.Contains(pr.ParentRequestID))
                        .Select(pr => new ParentRequestListViewModel
                        {
                            ParentRequest = pr,
                            Request = pr.Requests.FirstOrDefault(),
                            Product = pr.Requests.FirstOrDefault().Product,
                            ProductSubcategory = pr.Requests.FirstOrDefault().Product.ProductSubcategory,
                            Vendor = pr.Requests.FirstOrDefault().Product.Vendor,
                            ParentCategory = pr.Requests.FirstOrDefault().Product.ProductSubcategory.ParentCategory,
                            UnitType = pr.Requests.FirstOrDefault().UnitType,
                            SubUnitType = pr.Requests.FirstOrDefault().SubUnitType,
                            SubSubUnitType = pr.Requests.FirstOrDefault().SubSubUnitType
                        }).ToList().GroupBy(pr => pr.Vendor).ToDictionary(pr => pr.Key,
                               pr => pr.Select(r => r).ToList()
                            );
                    notificationsListViewModel.ParentRequestList = ParentRequestsFiltered;
                    break;
            }
            return View(notificationsListViewModel);

        }
        [HttpGet]
        [Authorize(Roles = "Admin, Accounting")]
        public async Task<IActionResult> Notifications(AppUtility.NotificationsEnum id)
        {
            TempData["PageType"] = AppUtility.PaymentPageTypeEnum.Notifications;
            NotificationsListViewModel notificationsListViewModel = new NotificationsListViewModel();
            Dictionary<Vendor, List<ParentRequestListViewModel>> ParentRequestsFiltered;
            switch (id)
            {            
                case AppUtility.NotificationsEnum.NoInvoice:
                    TempData["Action"] = "NoInvoice";
                    ParentRequestsFiltered =  _context.ParentRequests
                        .Where(pr => pr.InvoiceNumber == null)
                        .Select(pr => new ParentRequestListViewModel
                        {
                            ParentRequest = pr,
                            Request = pr.Requests.FirstOrDefault(),
                            Product = pr.Requests.FirstOrDefault().Product,
                            ProductSubcategory = pr.Requests.FirstOrDefault().Product.ProductSubcategory,
                            Vendor = pr.Requests.FirstOrDefault().Product.Vendor,
                            ParentCategory = pr.Requests.FirstOrDefault().Product.ProductSubcategory.ParentCategory,
                            UnitType = pr.Requests.FirstOrDefault().UnitType,
                            SubUnitType = pr.Requests.FirstOrDefault().SubUnitType,
                            SubSubUnitType = pr.Requests.FirstOrDefault().SubSubUnitType
                        }).ToList().GroupBy(pr => pr.Vendor).ToDictionary(pr => pr.Key,
                               pr => pr.Select(r => r).ToList()
                            );
                    notificationsListViewModel.ParentRequestList = ParentRequestsFiltered;
                    break;

                case AppUtility.NotificationsEnum.DidntArrive:
                    TempData["Action"] = "DidntArrive";
                    ParentRequestsFiltered = _context.ParentRequests
                       .Where(pr => pr.Requests.FirstOrDefault().RequestStatusID == 2)
                       .Select(pr => new ParentRequestListViewModel
                       {
                           ParentRequest = pr,
                           Request = pr.Requests.FirstOrDefault(),
                           Product = pr.Requests.FirstOrDefault().Product,
                           ProductSubcategory = pr.Requests.FirstOrDefault().Product.ProductSubcategory,
                           Vendor = pr.Requests.FirstOrDefault().Product.Vendor,
                           ParentCategory = pr.Requests.FirstOrDefault().Product.ProductSubcategory.ParentCategory,
                           UnitType = pr.Requests.FirstOrDefault().UnitType,
                           SubUnitType = pr.Requests.FirstOrDefault().SubUnitType,
                           SubSubUnitType = pr.Requests.FirstOrDefault().SubSubUnitType
                       }).ToList().GroupBy(pr => pr.Vendor).ToDictionary(pr => pr.Key,
                               pr => pr.Select(r => r).ToList()
                            );
                    notificationsListViewModel.ParentRequestList = ParentRequestsFiltered;
                    break;
               
                case AppUtility.NotificationsEnum.PartialDelivery:
                    TempData["Action"] = "PartialDelivery";
                    ParentRequestsFiltered = _context.ParentRequests
                       .Where(pr => pr.Requests.FirstOrDefault().RequestStatusID == 4)
                       .Select(pr => new ParentRequestListViewModel
                       {
                           ParentRequest = pr,
                           Request = pr.Requests.FirstOrDefault(),
                           Product = pr.Requests.FirstOrDefault().Product,
                           ProductSubcategory = pr.Requests.FirstOrDefault().Product.ProductSubcategory,
                           Vendor = pr.Requests.FirstOrDefault().Product.Vendor,
                           ParentCategory = pr.Requests.FirstOrDefault().Product.ProductSubcategory.ParentCategory,
                           UnitType = pr.Requests.FirstOrDefault().UnitType,
                           SubUnitType = pr.Requests.FirstOrDefault().SubUnitType,
                           SubSubUnitType = pr.Requests.FirstOrDefault().SubSubUnitType
                       }).ToList().GroupBy(pr => pr.Vendor).ToDictionary(pr => pr.Key,
                               pr => pr.Select(r => r).ToList()
                            );
                    notificationsListViewModel.ParentRequestList = ParentRequestsFiltered;
                    break;
                case AppUtility.NotificationsEnum.ForClarification:
                    TempData["Action"] = "ForClarification";
                    ParentRequestsFiltered = _context.ParentRequests
                       .Where(pr => pr.Requests.FirstOrDefault().RequestStatusID == 5)
                       .Select(pr => new ParentRequestListViewModel
                       {
                           ParentRequest = pr,
                           Request = pr.Requests.FirstOrDefault(),
                           Product = pr.Requests.FirstOrDefault().Product,
                           ProductSubcategory = pr.Requests.FirstOrDefault().Product.ProductSubcategory,
                           Vendor = pr.Requests.FirstOrDefault().Product.Vendor,
                           ParentCategory = pr.Requests.FirstOrDefault().Product.ProductSubcategory.ParentCategory,
                           UnitType = pr.Requests.FirstOrDefault().UnitType,
                           SubUnitType = pr.Requests.FirstOrDefault().SubUnitType,
                           SubSubUnitType = pr.Requests.FirstOrDefault().SubSubUnitType
                       }).ToList().GroupBy(pr => pr.Vendor).ToDictionary(pr => pr.Key,
                               pr => pr.Select(r => r).ToList()
                            );
                    notificationsListViewModel.ParentRequestList = ParentRequestsFiltered;
                    break;
            }
            return View(notificationsListViewModel);

        }


  
        //this is here b/c the ajax call on the payment view is not working and I didn't have time to debug it
        [HttpGet]
        [Authorize(Roles = "Admin, Accounting")]
        public IActionResult DetailsModalView(int id)
        {
            return RedirectToAction("DetailsModalView", "Requests", new { id = id });
        }

    }
}
