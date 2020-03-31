using System;
using System.Collections;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.IO;
using System.Linq;
using System.Threading.Tasks;
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


                // if ordered, can also include if not  payed or if no invioce..
                //partial,  can also include if not  payed or if no invioce..
                //clarify,  can also include if not  payed or if no invioce..

            }

            Dictionary<string, int> titleList = new Dictionary<string, int>
            {
                { "To Pay", NotPayedList.Count },
                { "No Invoice", NoInvoiceList.Count },
                { "Didn't Arrive", DidntArriveList.Count },
                { "Partial Delivery", PartialDeliveryList.Count },
                { "For Clarification", ForClarification.Count }
            };

            //foreach (var list in nameOfLists)
            //{
            //    ReturnParentRequestList = ReturnParentRequestList.Add(list); 
            //}
            ReturnParentRequestList.Add(NotPayedList);
            ReturnParentRequestList.Add(NoInvoiceList);
            ReturnParentRequestList.Add(DidntArriveList);
            ReturnParentRequestList.Add(PartialDeliveryList);
            ReturnParentRequestList.Add(ForClarification);
            //ReturnParentRequestList = ReturnParentRequestList.Add(NoInvoiceList);

            PaymentNotificationsViewModel paymentNotificationsViewModel = new PaymentNotificationsViewModel
            {
                TitleList = titleList,
                ParentRequests = await ReturnParentRequestList.ToListAsync()
            };


            return View(paymentNotificationsViewModel);
            //}
            //    else if (PageType == AppUtility.PaymentPageTypeEnum.General)
            //    {
            //       var fullParentRequestsListByDate = fullParentRequestsList
            //            .OrderByDescending(f => f.Time.Date)
            //            .ThenBy(f => f.Time.TimeOfDay);
            //        return View(fullParentRequestsListByDate);
            //    }

            //    return View(paymentNotificationsViewModel);
            //}
        }
        [HttpGet]
        public async Task<IActionResult> GeneralPaymentList()
        {

            IEnumerable<ParentRequest> fullParentRequestsList = _context.ParentRequests.Include(pr => pr.ApplicationUser).Include(pr => pr.Requests).ThenInclude(pr => pr.Product).ThenInclude(pr => pr.ProductSubcategory).ThenInclude(pr => pr.ParentCategory).Include(pr => pr.Requests).ThenInclude(pr => pr.Product).ThenInclude(pr => pr.Vendor);

            var fullParentRequestsListByDate = fullParentRequestsList
                        .OrderByDescending(f => f.OrderDate.Date)
                        .ThenBy(f => f.OrderDate.TimeOfDay);
            return View(await fullParentRequestsListByDate.ToListAsync());
        }
        //public async Task<IActionResult> ExpensesPaymentList2()
        //{
        //IEnumerable<ParentRequest> fullParentRequestsList = _context.ParentRequests.Include(pr => pr.Requests).ThenInclude(pr => pr.Product).ThenInclude(pr => pr.ProductSubcategory).ThenInclude(pr => pr.ParentCategory);
        //var fullParentRequestsListByCategoryAndDescendingDate = fullParentRequestsList

        //        .OrderByDescending(fPR => fPR.OrderDate.Date)
        //        .ThenBy(fPR => fPR.OrderDate.TimeOfDay);
        //     var orders = db.Orders.Include(o => o.Product)
        //.GroupBy(o => o.Product.Category.Name)
        //.Select(cat => cat.FirstOrDefault());

        //var products = db.Orders.Include(o => o.Product);

        //return View(Tuple.Create(orders.ToList(), products.ToList()));

        //var fullParentRequestsListByMonth = _context.ParentRequests.Include(pr => pr.Requests).ThenInclude(pr => pr.Product).ThenInclude(pr => pr.ProductSubcategory).ThenInclude(pr => pr.ParentCategory);
        //fullParentRequestsListByMonth
        //    .GroupBy(fPR => fPR.OrderDate.Month);
        //    ExpensesPaymentListViewModel expensesPaymentListViewModel = new ExpensesPaymentListViewModel
        //    {
        //        ParentCategories = /*await*/ _context.ParentCategories/*.ToListAsync()*/,
        //        ParentRequests = /*await */fullParentRequestsListByMonth/*.ToListAsync()*/
        //    };
        //)
        //.ToList();

        //ExpensesPaymentListViewModel expensesPaymentListViewModel = new ExpensesPaymentListViewModel
        //{
        //    ParentCategories = await _context.ParentCategories.ToListAsync(),
        //    ParentRequests = await fullParentRequestsListByCategoryAndDescendingDate.ToListAsync()
        //};

        /*        return View(expensesPaymentListViewModel*/  /* await fullParentRequestsListByCategoryAndDescendingDate.ToListAsync()/*await fullParentRequestsListByMonth*//*);*/



        //}
        //public ActionResult ExpensesPaymentList()
        //{
        //    var model = _context.Requests.Include(r => r.Product).ThenInclude(r => r.ProductSubcategory).ThenInclude(r => r.ParentCategory)
        //        .AsEnumerable()
        //        .GroupBy(r => new
        //        {
        //            ParentCategory = r.Product.ProductSubcategory.ParentCategory,


        //        })
        //        .Select(g => new ExpensesPaymentListViewModel
        //        {

        //            ParentCategory = g.Key.ParentCategory,
        //            Total = g.Sum(r => r.Cost)

        //        })

        //        .ToList();

        //    return View(model);
        //}




        public async Task<IActionResult> ExpensesPaymentList()
        {
            var requests = _context.Requests.Include(r => r.ParentRequest).Include(r => r.Product).ThenInclude(r => r.ProductSubcategory).ThenInclude(r => r.ParentCategory);
            var res = from request in requests
                      group request by new { request.ParentRequest.OrderDate.Month, request.Product.ProductSubcategory.ParentCategory };

            return View(await res.ToListAsync()/*await model.ToListAsync()*/);
        }


        public async Task<IActionResult> ExpensesList()
        {
            //get them all by date month

            var requestsSortedByDate = _context.Requests.Include(r => r.ParentRequest).Include(r => r.Product).ThenInclude(r => r.ProductSubcategory)
                .GroupBy(r =>  r.ParentRequest).Select(r => new List<Request>(r));
            foreach(var r in requestsSortedByDate)
            {
                r.ToString();
            }
            return View();
        }



        //public async Task<IActionResult> ExpensesPaymentList()
        //{

        //    var model = _context.Requests.Include(r => r.Product).ThenInclude(r => r.ProductSubcategory).ThenInclude(r => r.ParentCategory)
        //        .GroupBy(r => new
        //        {
        //            Month = r.ParentRequest.OrderDate.Month,
        //            Year = r.ParentRequest.OrderDate.Year,


        //        })

        //        .Select(g => new MonthlyTotalsViewModel
        //        {
        //            Month = g.Key.Month,
        //            Year = g.Key.Year,
        //            GrandTotal = g.Sum(r => r.Cost)

        //        })
        //        .OrderByDescending(a => a.Year)
        //        .ThenByDescending(a => a.Month)
        //        .ToList();

        //    return View(await model.ToListAsync());
        //}

        //       public async Task<IActionResult> ExpensesPaymentList()
        //       {
        //           var categoryGrouping = _context.Requests.Include(pr => pr.Product).ThenInclude(pr => pr.ProductSubcategory).ThenInclude(pr => pr.ParentCategory)
        //               .GroupBy(r => new
        //               {
        //                   Category = r.Product.ProductSubcategory.ParentCategory,
        //                   Month = r.ParentRequest.OrderDate.Month,
        //                   Year = r.ParentRequest.OrderDate.Year
        //               })
        //               .Select
        //               (x => new 
        //               {
        //                   Category = x.First().Product.ProductSubcategory.ParentCategory,
        //                   Amount = x.Sum(y => y.Cost),
        //                   Month = x.Key.Month,
        //                   Year = x.Key.Year



        //               });
        //           var categories = _context.ParentCategories;
        //           var categoryCount = categories.Count();
        //           var categoryIDs = categories.Select(x => x.ParentCategoryID).ToList();

        //var model = new MonthlyTotalsViewModel
        //{
        //   Month 
        //}

        //           return View(await model.ToListAsync());
        //       }

        //public ActionResult ExpensesPaymentList(DateTime startDate, int months)
        //{
        //    // Get the start and end dates
        //    startDate = new DateTime(startDate.Year, startDate.Month, 1); // ensure first day of month
        //    DateTime endDate = startDate.AddMonths(months + 1);
        //    // Initialize the model
        //    List<ExpensesPaymentListViewModel> model = new List<ExpensesPaymentListViewModel>();
        //    // Filter the data and group by client
        //    var data = _context.Requests/*.Include(pr => pr.Requests).ThenInclude(pr => pr.Product).ThenInclude(pr => pr.ProductSubcategory).ThenInclude(pr => pr.ParentCategory)*/
        //        .Where(r => r.ParentRequest.OrderDate >= startDate && r.ParentRequest.OrderDate < endDate)
        //        .GroupBy(r => r.Product.ProductSubcategory.ParentCategory);
        //    // Create a ExpensesPaymentListViewModel for each parentCategory group
        //    foreach (var parentCategoryGroup in data)
        //    {
        //        ExpensesPaymentListViewModel summary = new ExpensesPaymentListViewModel(startDate, months)
        //        {
        //            ParentCategory = parentCategoryGroup.Key,
        //            GrandTotal = (decimal)parentCategoryGroup.Sum(x => x.Cost)
        //        };
        //        // Group by month/year
        //        foreach (var monthGroup in parentCategoryGroup.GroupBy(x => new { Month = x.ParentRequest.OrderDate.Month, Year = x.ParentRequest.OrderDate.Year }))
        //        {
        //            // Get the index of the month
        //            int index = ((monthGroup.Key.Year - startDate.Year) * 12) + monthGroup.Key.Month - startDate.Month;
        //            summary.MonthlyTotals[index].Amount = (decimal)monthGroup.First().Cost; // or .Sum(m => m.First().Requests.FirstOrDefault().Cost) if there are multiple invoives per month
        //        }
        //        model.Add(summary);
        //    }
        //    return View(model);
        //}

        //public ActionResult ExpensesPaymentList()
        //{
        //    var data = _context.ParentRequests.Include(pr => pr.Requests).ThenInclude(pr => pr.Product).ThenInclude(pr => pr.ProductSubcategory).ThenInclude(pr => pr.ParentCategory)
        //       .GroupBy(pr => pr.Requests.FirstOrDefault().Product.ProductSubcategory.ParentCategory);
        //    foreach
        //}

        //    public ActionResult ExpensesPaymentList()
        //    {
        //        var parentCategoriesIDs = _context.ParentCategories;
        //        //var date = new DateTime(0001, 1, 1);
        //        var list = _context.ParentRequests.Include(pr => pr.Requests).ThenInclude(pr => pr.Product).ThenInclude(pr => pr.ProductSubcategory).ThenInclude(pr => pr.ParentCategory);
        //        //var filtered = list.Where(x => x.OrderDate >= date && x.OrderDate < date.AddMonths(1));
        //        var grouped = list.GroupBy(x => new { category = x.Requests.FirstOrDefault().Product.ProductSubcategory.ParentCategory }).Select(x => new
        //        {
        //            Category = x.First().Requests.First().Product.ProductSubcategory.ParentCategory,
        //            Amount = x.Sum(y => y.Requests.First().Cost)
        //        });
        //        var categories = _context.ParentCategories;
        //        var categoryCount = categories.Count();
        //        var categoryIDs = categories.Select(x => x.ParentCategoryID).ToList();
        //        var model = new MonthlyTotalsViewModel()
        //        {
        //            //Date = date,
        //            Categories = categories.Select(x => x.ParentCategoryDescription),
        //            Payments = new List<ExpensesPaymentListViewModel>()
        //        };

        //            ExpensesPaymentListViewModel payment = new ExpensesPaymentListViewModel (categoryCount);
        //            foreach (var item in grouped)
        //            {
        //                int index = categoryIDs.IndexOf(item.Category.ParentCategoryID);
        //                payment.Amounts[index] = (decimal)item.Amount;
        //                payment.Total += (decimal)item.Amount;
        //            }
        //            model.Payments.Add(payment);


        //        return View(model);
        //}


        // GET: ParentRequests/Details/5
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

            return View(parentRequest);
        }

        // GET: ParentRequests/Create
        public IActionResult Create()
        {
            ViewData["ApplicationUserID"] = new SelectList(_context.Users, "Id", "Id");
            return View();
        }

        // POST: ParentRequests/Create
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
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
            return View(parentRequest);
        }

        // POST: ParentRequests/Edit/5
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
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
    }
}
