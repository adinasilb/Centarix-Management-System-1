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
            IEnumerable<ParentRequest> fullParentRequestsList = _context.ParentRequests.Include(pr => pr.ApplicationUser).Include(pr => pr.Requests).ThenInclude(pr => pr.Product).ThenInclude(pr => pr.ProductSubcategory).ThenInclude(pr => pr.ParentCategory).Include(pr => pr.Requests).ThenInclude(pr => pr.Product).ThenInclude(pr =>pr.Vendor);
            


            List<List<ParentRequest>> ReturnParentRequestList = new List<List<ParentRequest>>();
            List<ParentRequest> NotPayedList = new List<ParentRequest>();
            List<ParentRequest> NoInvoiceList = new List<ParentRequest>();
            List<ParentRequest> DidntArriveList = new List<ParentRequest>();
            List<ParentRequest> PartialDeliveryList = new List<ParentRequest>();
            List <ParentRequest> ForClarification = new List<ParentRequest>();
   

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

            Dictionary<string, int> titleList = new Dictionary<string, int>();
            titleList.Add("To Pay", NotPayedList.Count);
            titleList.Add("No Invoice", NoInvoiceList.Count);
            titleList.Add("Didn't Arrive", DidntArriveList.Count);
            titleList.Add("Partial Delivery", PartialDeliveryList.Count);
            titleList.Add("For Clarification", ForClarification.Count);

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
                ParentRequests = await ReturnParentRequestList.ToListAsync() }
            ;

            return View(paymentNotificationsViewModel);
 
        }

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
