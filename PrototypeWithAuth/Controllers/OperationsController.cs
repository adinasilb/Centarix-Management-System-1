﻿using System;
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
        private readonly ApplicationDbContext _context;
        private readonly UserManager<ApplicationUser> _userManager;
        private readonly SignInManager<ApplicationUser> _signInManager;
        //take this out?
        private readonly IHostingEnvironment _hostingEnvironment;

        private ICompositeViewEngine _viewEngine;

        public OperationsController(ApplicationDbContext context, UserManager<ApplicationUser> userManager, SignInManager<ApplicationUser> signInManager,
            IHostingEnvironment hostingEnvironment, ICompositeViewEngine viewEngine /*IHttpContextAccessor Context*/) : base(context)
        {
            //_Context = Context;
            _context = context;
            _userManager = userManager;
            _signInManager = signInManager;
            //use the hosting environment for the file uploads
            _hostingEnvironment = hostingEnvironment;
            _viewEngine = viewEngine;
        }

       
       
        public IActionResult Order(int id, RequestIndexObject requestIndexObject)
        {
            var request = _context.Requests.Where(r => r.RequestID == id).Include(r => r.Product).ThenInclude(p => p.ProductSubcategory).ThenInclude(px => px.ParentCategory).FirstOrDefault();
            try
            {
                request.RequestStatusID = 2; //ordered
                ParentRequest parentRequest = new ParentRequest();
                parentRequest.ApplicationUserID = _userManager.GetUserId(User);
                int lastParentRequestOrderNum = 0;
                if (_context.ParentRequests.Any())
                {
                    lastParentRequestOrderNum = _context.ParentRequests.OrderByDescending(x => x.OrderNumber).FirstOrDefault().OrderNumber.Value;
                }
                parentRequest.OrderDate = AppUtility.ElixirDate();
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
            DateTime firstOfMonth = new DateTime(AppUtility.ElixirDate().Year, AppUtility.ElixirDate().Month, 1);
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
