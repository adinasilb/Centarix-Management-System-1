using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.IO;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using PrototypeWithAuth.AppData;
using PrototypeWithAuth.Data;
using PrototypeWithAuth.Models;
using PrototypeWithAuth.ViewModels;
using X.PagedList;
using Microsoft.AspNetCore.Hosting;
using MailKit.Net.Smtp;
using MimeKit;
using Microsoft.AspNetCore.Mvc.ViewEngines;
using Microsoft.AspNetCore.Mvc.ViewFeatures;
using SelectPdf;
using Microsoft.AspNetCore.Authorization;
using Org.BouncyCastle.Ocsp;
using System.Xml.Linq;
using System.Diagnostics;
using Abp.Extensions;
using SQLitePCL;
using Microsoft.AspNetCore.Localization;
using PrototypeWithAuth.AppData.UtilityModels;
//using Org.BouncyCastle.Asn1.X509;
//using System.Data.Entity.Validation;
//using System.Data.Entity.Infrastructure;

namespace PrototypeWithAuth.Controllers
{
    public class OperationsController : SharedController
    {
        public OperationsController(ApplicationDbContext context, UserManager<ApplicationUser> userManager, IHostingEnvironment hostingEnvironment, IHttpContextAccessor httpContextAccessor, ICompositeViewEngine viewEngine)
           : base(context, userManager, hostingEnvironment, viewEngine, httpContextAccessor)
        
            {
           
        }

        public async Task<IActionResult> Index(RequestIndexObject requestIndexObject)
        {
            TempData[AppUtility.TempDataTypes.PageType.ToString()] = requestIndexObject.PageType;
            TempData[AppUtility.TempDataTypes.MenuType.ToString()] = requestIndexObject.SectionType;
            TempData[AppUtility.TempDataTypes.SidebarType.ToString()] = requestIndexObject.SidebarType;

            var viewmodel = await base.GetIndexViewModel(requestIndexObject);

            //SetViewModelCounts(requestIndexObject, viewmodel);

            return View(viewmodel);
        }


        public async Task<IActionResult> AddItemView()
        {
            TempData[AppUtility.TempDataTypes.PageType.ToString()] = AppUtility.PageTypeEnum.OperationsRequest;
            TempData[AppUtility.TempDataTypes.SidebarType.ToString()] = AppUtility.SidebarEnum.Add;
            TempData[AppUtility.TempDataTypes.MenuType.ToString()] = AppUtility.MenuItems.Operations;

            return View();
        }




        public IActionResult Order(int id, RequestIndexObject requestIndexObject)
        {
            var request = _context.Requests.Where(r => r.RequestID == id).Include(r => r.Product).ThenInclude(p => p.ProductSubcategory).ThenInclude(px => px.ParentCategory).FirstOrDefault();
            try
            {
                request.RequestStatusID = 2; //ordered
                ParentRequest parentRequest = new ParentRequest();
                parentRequest.ApplicationUserID = _userManager.GetUserId(User);
                long lastParentRequestOrderNum = 0;
                if (_context.ParentRequests.Any())
                {
                    lastParentRequestOrderNum = _context.ParentRequests.AsNoTracking().OrderByDescending(x => x.OrderNumber).FirstOrDefault().OrderNumber.Value;
                }
                parentRequest.OrderDate = DateTime.Now;
                parentRequest.OrderNumber = lastParentRequestOrderNum;
                request.ParentRequest = parentRequest;
                _context.Update(request);
                _context.SaveChanges();
            }
            catch (Exception ex)
            {
                TempData["ErrorMessage"] = ex.Message;
                TempData["InnerMessage"] = ex.InnerException;
                return View("~/Views/Shared/RequestError.cshtml");
            }
            return RedirectToAction("_IndexTableWithCounts", "Requests", requestIndexObject);
        }


        private bool checkIfInBudget(Request request)
        {
            DateTime firstOfMonth = new DateTime(DateTime.Now.Year, DateTime.Now.Month, 1);
            if (request.Product.ProductSubcategory.ParentCategory.CategoryTypeID == 2)
            {//operational
                var pricePerUnit = request.Cost;
                if (pricePerUnit > request.ApplicationUserCreator.OperationUnitLimit)
                {
                    return false;
                }
                if (request.Cost > request.ApplicationUserCreator.OperationOrderLimit)
                {
                    return false;
                }

                var monthsSpending = _context.Requests
                    .Where(r => request.Product.ProductSubcategory.ParentCategory.CategoryTypeID == 2)
                    .Where(r => r.ApplicationUserCreatorID == request.ApplicationUserCreatorID)
                    .Where(r => r.ParentRequest.OrderDate >= firstOfMonth)
                    .Sum(r => r.Cost);
                if (monthsSpending + request.Cost > request.ApplicationUserCreator.OperationMonthlyLimit)
                {
                    return false;
                }
                return true;
            }
            else
            {
                //probably will never happen should not have any lab here
                return false; //not any type of operation and therefore cannot be ordered without being approved
            }
        }
    }

}
