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

       
        [Authorize(Roles = "Operations")]
        public async Task<IActionResult> CreateModalView()
        {
            var parentcategories = await _context.ParentCategories.Where(pr => pr.CategoryTypeID == 2).ToListAsync();
            var productsubactegories = await _context.ProductSubcategories.Where(pr => pr.ParentCategory.CategoryTypeID == 2).ToListAsync();
            var vendors = await _context.Vendors.Where(v => v.VendorCategoryTypes.Where(vc => vc.CategoryTypeID == 2).Count() > 0).ToListAsync();
            var paymenttypes = await _context.PaymentTypes.ToListAsync();
            var companyaccounts = await _context.CompanyAccounts.ToListAsync();
            List<AppUtility.CommentTypeEnum> commentTypes = Enum.GetValues(typeof(AppUtility.CommentTypeEnum)).Cast<AppUtility.CommentTypeEnum>().ToList();

            RequestItemViewModel requestItemViewModel = new RequestItemViewModel()
            {
                //ParentCategories = parentcategories,
                ProductSubcategories = productsubactegories,
                Vendors = vendors,
                PaymentTypes = paymenttypes,
                CompanyAccounts = companyaccounts,
                CommentTypes = commentTypes,
                Comments = new List<Comment>(),
            };
            string uploadFolder = Path.Combine(_hostingEnvironment.WebRootPath, "files");
            string requestFolder = Path.Combine(uploadFolder, "0");

            if (Directory.Exists(requestFolder))
            {
                System.IO.DirectoryInfo di = new DirectoryInfo(requestFolder);
                foreach (FileInfo file in di.GetFiles())
                {
                    file.Delete();
                }
                foreach (DirectoryInfo dir in di.EnumerateDirectories())
                {
                    dir.Delete(true);
                }
                Directory.Delete(requestFolder);
            }
            Directory.CreateDirectory(requestFolder);
            requestItemViewModel.Request = new Request();
            requestItemViewModel.Request.Product = new Product();
            requestItemViewModel.Request.Product = new Product();
            requestItemViewModel.Request.ParentQuote = new ParentQuote();
            requestItemViewModel.Request.ExchangeRate =  _context.ExchangeRates.FirstOrDefault().LatestExchangeRate;

            requestItemViewModel.Request.ParentQuote.QuoteDate = DateTime.Now;
            requestItemViewModel.Request.CreationDate = DateTime.Now;
            TempData[AppUtility.TempDataTypes.PageType.ToString()] = AppUtility.PageTypeEnum.OperationsRequest;
            TempData[AppUtility.TempDataTypes.MenuType.ToString()] = AppUtility.MenuItems.Operations;
            TempData[AppUtility.TempDataTypes.SidebarType.ToString()] = AppUtility.SidebarEnum.Add;
            TempData[AppUtility.TempDataTypes.SidebarType.ToString()] = AppUtility.SidebarEnum.Add;
            if (AppUtility.IsAjaxRequest(this.Request))
            {
                return PartialView(requestItemViewModel);
            }
            else
            {
                return View(requestItemViewModel);
            }
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        [Authorize(Roles = "Operations")]
        public async Task<IActionResult> CreateModalView(RequestItemViewModel requestItemViewModel, string OrderType)
        {
            //why do we need this here?
            requestItemViewModel.Request.Product.Vendor = _context.Vendors.FirstOrDefault(v => v.VendorID == requestItemViewModel.Request.Product.VendorID);
            requestItemViewModel.Request.Product.ProductSubcategory = _context.ProductSubcategories.Include(ps => ps.ParentCategory).FirstOrDefault(ps => ps.ProductSubcategoryID == requestItemViewModel.Request.Product.ProductSubcategoryID);

            //in case we need to return to the modal view
            //requestItemViewModel.ParentCategories = await _context.ParentCategories.ToListAsync();
            requestItemViewModel.ProductSubcategories = await _context.ProductSubcategories.ToListAsync();
            requestItemViewModel.Vendors = await _context.Vendors.Where(v => v.VendorCategoryTypes.Where(vc => vc.CategoryTypeID == 2).Count() > 0).ToListAsync();
            requestItemViewModel.RequestStatuses = await _context.RequestStatuses.ToListAsync();
            //formatting the select list of the unit types

            //declared outside the if b/c it's used farther down too 
            var currentUser = _context.Users.FirstOrDefault(u => u.Id == _userManager.GetUserId(User));

            requestItemViewModel.Request.ApplicationUserCreatorID = currentUser.Id;
            requestItemViewModel.Request.ApplicationUserCreator = currentUser;
            requestItemViewModel.Request.CreationDate = DateTime.Now;
            requestItemViewModel.Request.ParentQuote.ApplicationUser = currentUser;
            requestItemViewModel.Request.ParentQuote.QuoteDate = DateTime.Now;

            //all new ones will be "new" until actually ordered after the confirm email
            requestItemViewModel.Request.RequestStatusID = 1;

            //in case we need to redirect to action
            //TempData["ModalView"] = true;
            //todo why is this here?
            //todo terms and installements and paid
            var context = new ValidationContext(requestItemViewModel.Request, null, null);
            var results = new List<ValidationResult>();
            if (Validator.TryValidateObject(requestItemViewModel.Request, context, results, true))
            {
                /*
                 * the viewmodel loads the request.product with a primary key of 0
                 * so if you don't insert the request.productid into the request.product.productid
                 * it will create a new one instead of updating the existing one
                 * only need this if using an existing product
                 */
                //CREATE MODAL - may need to take this out? shouldn't it always create a new product??
                //requestItemViewModel.Request.Product.ProductID = requestItemViewModel.Request.ProductID;

                //if it is out of the budget get sent to get approved automatically and user is not in role admin !User.IsInRole("Admin")
                if (/*!User.IsInRole("Admin") &&*/ OrderType.Equals("Ask For Permission") || !checkIfInBudget(requestItemViewModel.Request))
                {
                    try
                    {
                        requestItemViewModel.Request.ParentQuote.QuoteStatusID = 4;
                        requestItemViewModel.Request.RequestStatusID = 1; //new request
                        _context.Update(requestItemViewModel.Request);
                        _context.SaveChanges();
                    }
                    catch (Exception ex)
                    {
                        TempData["ErrorMessage"] = ex.Message;
                        TempData["InnerMessage"] = ex.InnerException;
                        return View("~/Views/Shared/RequestError.cshtml");
                    }
                }
                else
                {
                    if (OrderType.Equals("Add To Cart"))
                    {
                        try
                        {
                            requestItemViewModel.Request.RequestStatusID = 6; //approved
                            requestItemViewModel.Request.ParentQuote.QuoteStatusID = 4;
                            _context.Update(requestItemViewModel.Request);
                            _context.SaveChanges();
                        }
                        catch (Exception ex)
                        {
                            TempData["ErrorMessage"] = ex.Message;
                            TempData["InnerMessage"] = ex.InnerException;
                            return View("~/Views/Shared/RequestError.cshtml");
                        }
                    }
                    try
                    {
                        if (OrderType.Equals("Without Order"))
                        {
                            requestItemViewModel.Request.ParentRequest = new ParentRequest();
                            int lastParentRequestOrderNum = 0;
                            requestItemViewModel.Request.ParentRequest.ApplicationUserID = currentUser.Id;
                            if (_context.ParentRequests.Any())
                            {
                                lastParentRequestOrderNum = _context.ParentRequests.OrderByDescending(x => x.OrderNumber).FirstOrDefault().OrderNumber ?? 0;
                            }
                            requestItemViewModel.Request.ParentRequest.OrderNumber = lastParentRequestOrderNum + 1;
                            requestItemViewModel.Request.ParentRequest.OrderDate = DateTime.Now;
                           // requestItemViewModel.Request.ParentRequest.WithoutOrder = true;
                            requestItemViewModel.Request.RequestStatusID = 2;
                            requestItemViewModel.RequestStatusID = 2;
                            requestItemViewModel.Request.ParentQuote = null;
                            _context.Update(requestItemViewModel.Request);
                            _context.SaveChanges();
                        }
                        else if (OrderType.Equals("Order"))
                        {
                            requestItemViewModel.Request.RequestStatusID = 1; //new request
                            requestItemViewModel.Request.ParentQuote.QuoteStatusID = 4;
                            requestItemViewModel.RequestStatusID = 1;
                            requestItemViewModel.Request.ParentRequest = new ParentRequest();
                            requestItemViewModel.Request.ParentRequest.ApplicationUserID = currentUser.Id;

                            _context.Update(requestItemViewModel.Request);
                            _context.SaveChanges();
                            TempData["OpenTermsModal"] = "Single";
                            //TempData["OpenConfirmEmailModal"] = true; //now we want it to go to the terms instead
                            TempData["RequestID"] = requestItemViewModel.Request.RequestID;
                        }
                        else
                        {
                            requestItemViewModel.Request.RequestStatusID = 1; //needs approvall
                            requestItemViewModel.Request.ParentQuote.QuoteStatusID = 4;
                            _context.Update(requestItemViewModel.Request);
                            _context.SaveChanges();
                        }

                    }
                    catch (Exception ex)
                    {
                        TempData["ErrorMessage"] = ex.Message;
                        TempData["InnerMessage"] = ex.InnerException;
                        return View("~/Views/Shared/RequestError.cshtml");
                    }
                }
                try
                {

                    try
                    {
                        foreach (var comment in requestItemViewModel.Comments)
                        {
                            if (comment.CommentText.Length != 0)
                            {
                                //save the new comment
                                comment.ApplicationUserID = currentUser.Id;
                                comment.CommentTimeStamp = DateTime.Now; //check if we actually need this line
                                comment.RequestID = requestItemViewModel.Request.RequestID;
                                _context.Add(comment);
                            }


                        }
                        await _context.SaveChangesAsync();
                    }
                    catch (Exception ex)
                    {
                        //do something here. comment didn't save
                    }

                    //rename temp folder to the request id
                    string uploadFolder = Path.Combine(_hostingEnvironment.WebRootPath, "files");
                    string requestFolderFrom = Path.Combine(uploadFolder, "0");
                    string requestFolderTo = Path.Combine(uploadFolder, requestItemViewModel.Request.RequestID.ToString());
                    Directory.Move(requestFolderFrom, requestFolderTo);

                    return RedirectToAction("Index", new
                    {
                        requestStatusID = requestItemViewModel.Request.RequestStatusID,
                        PageType = AppUtility.PageTypeEnum.RequestRequest
                    });
                }
                catch (DbUpdateException ex)
                {
                    //ModelState.AddModelError();
                    ViewData["ModalViewType"] = "Create";
                    TempData["ErrorMessage"] = ex.Message.ToString();
                    TempData["InnerMessage"] = ex.InnerException.ToString();
                    return View("~/Views/Shared/RequestError.cshtml");
                }
                catch (Exception ex)
                {
                    //ModelState.AddModelError();
                    ViewData["ModalViewType"] = "Create";
                    TempData["ErrorMessage"] = ex.Message.ToString();
                    TempData["InnerMessage"] = ex.InnerException.ToString();
                    return View("~/Views/Shared/RequestError.cshtml");
                }
            }
            else
            {
                TempData["InnerMessage"] = "The request model failed to validate. Please ensure that all fields were filled in correctly";
                return View("~/Views/Shared/RequestError.cshtml");
            }
        }


        [HttpGet]
        [Authorize(Roles = "Operations")]
        public async Task<IActionResult> EditModalView(int? id, bool NewRequestFromProduct = false)
        {
            return await editModalViewFunction(id);
        }

        [HttpGet]
        [Authorize(Roles = "Operations")]
        public async Task<IActionResult> EditModalViewPartial(int? id, int? Tab)
        {
            return await editModalViewFunction(id, Tab);
        }
        public async Task<IActionResult> editModalViewFunction(int? id, int? Tab = 0)
        {
            string ModalViewType = "";
            if (id == null)
            {
                return NotFound();
            }

            var parentcategories = await _context.ParentCategories.Where(pc => pc.CategoryTypeID == 2).ToListAsync();
            var productsubactegories = await _context.ProductSubcategories.Where(ps => ps.ParentCategory.CategoryTypeID == 2).ToListAsync();
            var vendors = await _context.Vendors.Where(v => v.VendorCategoryTypes.Where(vc => vc.CategoryTypeID == 2).Count() > 0).ToListAsync();
            var paymenttypes = await _context.PaymentTypes.ToListAsync();
            var companyaccounts = await _context.CompanyAccounts.ToListAsync();
            List<AppUtility.CommentTypeEnum> commentTypes = Enum.GetValues(typeof(AppUtility.CommentTypeEnum)).Cast<AppUtility.CommentTypeEnum>().ToList();
            RequestItemViewModel requestItemViewModel = new RequestItemViewModel()
            {
                //ParentCategories = parentcategories,
                ProductSubcategories = productsubactegories,
                Vendors = vendors,
                PaymentTypes = paymenttypes,
                CompanyAccounts = companyaccounts,
                Tab = Tab ?? 0,
                Comments = await _context.Comments
                .Include(r => r.ApplicationUser)
                .Where(r => r.Request.RequestID == id).ToListAsync(),
                CommentTypes = commentTypes
            };

            ModalViewType = "Edit";

            requestItemViewModel.Request = _context.Requests.Include(r => r.Product)
                .Include(r => r.ParentQuote)
                .Include(r => r.ParentRequest)
                .Include(r => r.Product.ProductSubcategory)
                .Include(r => r.Product.ProductSubcategory.ParentCategory)
                .Include(r => r.Product.Vendor)
                .Include(r => r.RequestStatus)
                .Include(r => r.ApplicationUserCreator)
                //.Include(r => r.Payments) //do we have to have a separate list of payments to include thefix c inside things (like company account and payment types?)
                .SingleOrDefault(r => r.RequestID == id);

            //may be able to do this together - combining the path for the orders folders
            string uploadFolder1 = Path.Combine(_hostingEnvironment.WebRootPath, "files");
            string uploadFolder2 = Path.Combine(uploadFolder1, requestItemViewModel.Request.RequestID.ToString());
            //the partial file name that we will search for (1- because we want the first one)
            //creating the directory from the path made earlier

            GetExistingFileStrings(requestItemViewModel, AppUtility.RequestFolderNamesEnum.Invoices, uploadFolder2);
            GetExistingFileStrings(requestItemViewModel, AppUtility.RequestFolderNamesEnum.Shipments, uploadFolder2);
            GetExistingFileStrings(requestItemViewModel, AppUtility.RequestFolderNamesEnum.Quotes, uploadFolder2);
            GetExistingFileStrings(requestItemViewModel, AppUtility.RequestFolderNamesEnum.Info, uploadFolder2);
            GetExistingFileStrings(requestItemViewModel, AppUtility.RequestFolderNamesEnum.Pictures, uploadFolder2);
            GetExistingFileStrings(requestItemViewModel, AppUtility.RequestFolderNamesEnum.Returns, uploadFolder2);
            GetExistingFileStrings(requestItemViewModel, AppUtility.RequestFolderNamesEnum.Credits, uploadFolder2);


            //GET PAYMENTS HERE
            //var payments = _context.Payments
            //    .Include(p => p.CompanyAccount).ThenInclude(ca => ca.PaymentType)
            //    .Where(p => p.RequestID == requestItemViewModel.Request.RequestID).ToList();
            //requestItemViewModel.NewPayments = payments;

            //if (payments.Count > 0)
            //{
            //    var amountPerPayment = requestItemViewModel.Request.Cost / payments.Count; //shekel
            //    var totalPaymentsToDate = 0;
            //    foreach (var payment in payments)
            //    {
            //        if (payment.PaymentDate <= DateTime.Now)
            //        {
            //            totalPaymentsToDate++;
            //        }
            //        else
            //        {
            //            break;
            //        }
            //    }
            //    requestItemViewModel.Debt = requestItemViewModel.Request.Cost - (totalPaymentsToDate * amountPerPayment);
            //}
            //else
            //{
            //    requestItemViewModel.Debt = requestItemViewModel.Request.Cost;
            //}

            //setting the lists of companyaccounts by payment type id (so easy filtering on the frontend)

            //first get the list of payment types there are
            var paymentTypeIds = _context.CompanyAccounts.Select(ca => ca.PaymentTypeID).Distinct().ToList();
            //initialize the dictionary
            requestItemViewModel.CompanyAccountListsByPaymentTypeID = new Dictionary<int, IEnumerable<CompanyAccount>>();
            //foreach paymenttype
            foreach (var paymentTypeID in paymentTypeIds)
            {
                var caList = _context.CompanyAccounts.Where(ca => ca.PaymentTypeID == paymentTypeID);
                requestItemViewModel.CompanyAccountListsByPaymentTypeID.Add(paymentTypeID, caList);
            }



            //locations:
            //get the list of requestLocationInstances in this request

            if (requestItemViewModel.Request == null)
            {
                TempData["InnerMessage"] = "The request sent in was null";
            }

            ViewData["ModalViewType"] = ModalViewType;
            //ViewData["ApplicationUserID"] = new SelectList(_context.Users, "Id", "Id", addNewItemViewModel.Request.ParentRequest.ApplicationUserID);
            //ViewData["ProductID"] = new SelectList(_context.Products, "ProductID", "ProductName", addNewItemViewModel.Request.ProductID);
            //ViewData["RequestStatusID"] = new SelectList(_context.RequestStatuses, "RequestStatusID", "RequestStatusID", addNewItemViewModel.Request.RequestStatusID);

            return PartialView(requestItemViewModel);

        }
        //[Authorize(Roles = "Admin, Operation")]
        //public async Task<IActionResult> EditSummaryModalView(int? id, bool NewRequestFromProduct = false)
        //{

        //    //not imlemented yet
        //}

        [HttpPost]
        [ValidateAntiForgeryToken]
        [Authorize(Roles = "Operations")]
        public async Task<IActionResult> EditModalView(RequestItemViewModel requestItemViewModel, string OrderType)
        {
            //fill the request.parentrequestid with the request.parentrequets.parentrequestid (otherwise it creates a new not used parent request)
            requestItemViewModel.Request.ParentRequest = null;
            // requestItemViewModel.Request.ParentQuote.ParentQuoteID = (Int32)requestItemViewModel.Request.ParentQuoteID;
            //  var parentQuote = _context.ParentQuotes.Where(pq => pq.ParentQuoteID == requestItemViewModel.Request.ParentQuoteID).FirstOrDefault();
            // parentQuote.QuoteNumber = requestItemViewModel.Request.ParentQuote.QuoteNumber;
            // parentQuote.QuoteDate = requestItemViewModel.Request.ParentQuote.QuoteDate;
            // requestItemViewModel.Request.ParentQuote = parentQuote;

            var product = _context.Products.Include(p => p.Vendor).Include(p => p.ProductSubcategory).FirstOrDefault(v => v.ProductID == requestItemViewModel.Request.ProductID);
            product.ProductSubcategoryID = requestItemViewModel.Request.Product.ProductSubcategoryID;
            product.VendorID = requestItemViewModel.Request.Product.VendorID;
            product.ProductName = requestItemViewModel.Request.Product.ProductName;
            //in case we need to return to the modal view
            //requestItemViewModel.ParentCategories = await _context.ParentCategories.ToListAsync();
            requestItemViewModel.ProductSubcategories = await _context.ProductSubcategories.ToListAsync();
            requestItemViewModel.Vendors = await _context.Vendors.ToListAsync();

            //declared outside the if b/c it's used farther down to (for parent request the new comment too)
            var currentUser = _context.Users.FirstOrDefault(u => u.Id == _userManager.GetUserId(User));

            //in case we need to redirect to action
            //TempData["ModalView"] = true;
            TempData["RequestID"] = requestItemViewModel.Request.RequestID;

            //todo figure out payments
            //if (requestItemViewModel.Request.Terms == -1)
            //{
            //    requestItemViewModel.Request.Payed = true;
            //}


            var context = new ValidationContext(requestItemViewModel.Request, null, null);
            var results = new List<ValidationResult>();
            if (Validator.TryValidateObject(requestItemViewModel.Request, context, results, true))
            {

                // requestItemViewModel.Request.Product.ProductID = requestItemViewModel.Request.ProductID;
                try
                {
                    //_context.Update(requestItemViewModel.Request.Product.SubProject);
                    //_context.Update(requestItemViewModel.Request.Product);
                    requestItemViewModel.Request.Product = product;
                    _context.Update(requestItemViewModel.Request);
                    await _context.SaveChangesAsync();


                    try
                    {
                        foreach (var comment in requestItemViewModel.Comments)
                        {
                            if (!String.IsNullOrEmpty(comment.CommentText))
                            {
                                //save the new comment
                                comment.ApplicationUserID = currentUser.Id;
                                comment.CommentTimeStamp = DateTime.Now; //check if we actually need this line
                                comment.RequestID = requestItemViewModel.Request.RequestID;
                                _context.Update(comment);
                            }
                        }
                        await _context.SaveChangesAsync();
                    }
                    catch (Exception ex)
                    {
                        //Tell the user that the comment didn't save here
                    }


                    //Saving the Payments - each one should come in with a 1) date 2) companyAccountID
                    //if (requestItemViewModel.NewPayments != null)
                    //{
                    //    foreach (Payment payment in requestItemViewModel.NewPayments)
                    //    {
                    //        payment.RequestID = (Int32)requestItemViewModel.Request.RequestID;
                    //        payment.CompanyAccount = null;
                    //        //payment.Reference = "TEST";
                    //        try
                    //        {
                    //            _context.Payments.Update(payment);
                    //            await _context.SaveChangesAsync();
                    //        }
                    //        catch (Exception ex)
                    //        {

                    //        }
                    //    }
                    //}
                }
                catch (DbUpdateException ex)
                {
                    TempData["ErrorMessage"] = ex.Message.ToString();
                    TempData["InnerMessage"] = ex.InnerException.ToString();
                    return View("~/Views/Shared/RequestError.cshtml");
                }
                catch (Exception ex)
                {
                    TempData["ErrorMessage"] = ex.Message.ToString();
                    TempData["InnerMessage"] = ex.InnerException.ToString();
                    return View("~/Views/Shared/RequestError.cshtml");
                }
            }
            else
            {
                foreach (var result in results) Debug.WriteLine(result.ErrorMessage);
                return View("~/Views/Shared/RequestError.cshtml");
            }
            //return RedirectToAction("Index");
            //AppUtility.RequestPageTypeEnum requestPageTypeEnum = (AppUtility.RequestPageTypeEnum)requestItemViewModel.PageType;
            return RedirectToAction("Index", new
            {
                requestStatusID = requestItemViewModel.RequestStatusID,
            });
        }

        [HttpGet]
        //[ValidateAntiForgeryToken]
        [Authorize(Roles = "Operations")]
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
