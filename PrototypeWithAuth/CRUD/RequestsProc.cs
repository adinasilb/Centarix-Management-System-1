using LinqToExcel;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Query;
using OfficeOpenXml;
using PrototypeWithAuth.AppData;
using PrototypeWithAuth.AppData.UtilityModels;
using PrototypeWithAuth.Data;
using PrototypeWithAuth.Models;
using PrototypeWithAuth.ViewModels;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Linq.Expressions;
using System.Threading.Tasks;

namespace PrototypeWithAuth.CRUD
{
    public class RequestsProc : ApplicationDbContextProc<Request>
    {
        public RequestsProc(ApplicationDbContext context, bool FromBase = false) : base(context)
        {
            if (!FromBase)
            {
                base.InstantiateProcs();
            }
        }

        public override IQueryable<Request> ReadWithIgnoreQueryFilters(List<Expression<Func<Request, bool>>> wheres = null, List<ComplexIncludes<Request, ModelBase>> includes = null)
        {
            wheres.Add(r => !r.IsDeleted);
            return base.ReadWithIgnoreQueryFilters(wheres, includes);
        }


        public override IQueryable<Request> ReadOneWithIgnoreQueryFilters(List<Expression<Func<Request, bool>>> wheres = null, List<ComplexIncludes<Request, ModelBase>> includes = null)
        {
            wheres.Add(r => !r.IsDeleted);
            return base.ReadOneWithIgnoreQueryFilters(wheres, includes);
        }

        public override async Task<Request> ReadOneWithIgnoreQueryFiltersAsync(List<Expression<Func<Request, bool>>> wheres = null, List<ComplexIncludes<Request, ModelBase>> includes = null)
        {
            wheres.Add(r => !r.IsDeleted);
            return await base.ReadOneWithIgnoreQueryFiltersAsync(wheres, includes);
        }

        public async Task ArchiveRequestAsync(Request request)
        {
            request.IsArchived = true;
            _context.Entry(request).State = EntityState.Modified;
            await _context.SaveChangesAsync();
        }


        public async Task<StringWithBool> DeleteAsync(int requestID)
        {
            StringWithBool ReturnVal = new StringWithBool();
            try
            {
                var request = await ReadOneAsync(new List<Expression<Func<Request, bool>>> { r => r.RequestID == requestID },
                    new List<ComplexIncludes<Request, ModelBase>>
                    {
                        new ComplexIncludes<Request, ModelBase>{ Include = r => r.ParentRequest },
                        new ComplexIncludes<Request, ModelBase>{ Include = r => r.ParentQuote },
                        new ComplexIncludes<Request, ModelBase>{ Include = r => r.RequestLocationInstances},
                        new ComplexIncludes<Request, ModelBase>{ Include = r => r.Product, ThenInclude = new ComplexIncludes<ModelBase, ModelBase>{Include =p => ((Product)p).ProductSubcategory,
                        ThenInclude = new ComplexIncludes<ModelBase, ModelBase>{ Include = ps => ((ProductSubcategory)ps).ParentCategory} } }
                    });

                using (var transaction = _context.Database.BeginTransaction())
                {
                    try
                    {
                        request.IsDeleted = true;
                        _context.Update(request);
                        await _context.SaveChangesAsync();

                        if (request.ParentRequest != null)
                        {
                            await _parentRequestsProc.DeleteAsync(request.ParentRequest, request.RequestID);
                        }
                        if (request.ParentQuote != null)
                        {
                            await _parentQuotesProc.DeleteAsync(request.ParentQuote);
                        }
                        await _productsProc.DeleteAsync(request.Product);
                        var requestLocationInstances = request.RequestLocationInstances.ToList();
                        await _requestLocationInstancesProc.DeleteWithoutTransactionAsync(requestLocationInstances);
                        await _requestCommentsProc.DeleteWithoutTransactionAsync(request.RequestID);
                        var notifications = _requestNotificationsProc.Read(new List<Expression<Func<RequestNotification, bool>>> { rn => rn.RequestID == request.RequestID }).ToList();
                        await _requestNotificationsProc.DeleteWithoutTransactionAsync(notifications);
                        //throw new Exception();
                        await transaction.CommitAsync();
                    }
                    catch (Exception e)
                    {
                        transaction.Rollback();
                        throw new Exception(AppUtility.GetExceptionMessage(e));
                    }
                    ReturnVal.SetStringAndBool(true, null);
                }
            }
            catch (Exception ex)
            {
                ReturnVal.SetStringAndBool(false, AppUtility.GetExceptionMessage(ex));
            }
            return ReturnVal;

        }



        public async Task<string> GetSerialNumberAsync(bool isOperations)
        {
            var categoryType = 1;
            var serialLetter = "L";
            int lastSerialNumberInt = 0;
            if (isOperations)
            {
                categoryType = 2;
                serialLetter = "P";
            }
            var serialnumberList = _productsProc.ReadWithIgnoreQueryFilters(new List<Expression<Func<Product, bool>>> { p => p.ProductSubcategory.ParentCategory.CategoryTypeID == categoryType })
                .Select(p => int.Parse(p.SerialNumber.Substring(1))).ToList();

            lastSerialNumberInt = serialnumberList.OrderBy(s => s).LastOrDefault();

            return serialLetter + (lastSerialNumberInt + 1);
        }

        public async Task UpdateRequestInvoiceInfoAsync(AddInvoiceViewModel addInvoiceViewModel, Request request)
        {
            var RequestToSave = await ReadOneAsync(new List<Expression<Func<Request, bool>>> { r => r.RequestID == request.RequestID }, new List<ComplexIncludes<Request, ModelBase>> { new ComplexIncludes<Request, ModelBase> { Include = r => r.Payments } });
            RequestToSave.Cost = request.Cost;
            RequestToSave.Payments.FirstOrDefault().InvoiceID = addInvoiceViewModel.Invoice.InvoiceID;
            RequestToSave.Payments.FirstOrDefault().HasInvoice = true;
            _context.Update(RequestToSave);
        }

        public async Task<StringWithBool> UpdatePartialClarifyStatusAsync(AppUtility.SidebarEnum type, int requestID)
        {
            StringWithBool ReturnVal = new StringWithBool();
            try
            {
                
                using (var transaction = _context.Database.BeginTransaction())
                {
                    try
                    {
                        var request = await ReadOneAsync( new List<Expression<Func<Request, bool>>> { r => r.RequestID == requestID });
                        switch (type)
                        {

                            case AppUtility.SidebarEnum.DidntArrive:

                                break;
                            case AppUtility.SidebarEnum.PartialDelivery:
                                request.IsPartial = false;
                                break;
                            case AppUtility.SidebarEnum.ForClarification:
                                request.IsClarify = false;
                                break;
                        }
                        _context.Update(request);
                        await _context.SaveChangesAsync();
                        await transaction.CommitAsync();
                    }
                    catch (Exception e)
                    {
                        transaction.Rollback();
                        throw new Exception(AppUtility.GetExceptionMessage(e));
                    }
                    ReturnVal.SetStringAndBool(true, null);
                }
            }
            catch (Exception ex)
            {
                ReturnVal.SetStringAndBool(false, AppUtility.GetExceptionMessage(ex));
            }
            return ReturnVal;

        }

        public async Task<StringWithBool> UpdateQuoteStatusAsync(IEnumerable<Request> requests, int statusNumber)
        {
            StringWithBool ReturnVal = new StringWithBool();
            try
            {
                using (var transaction = _context.Database.BeginTransaction())
                {
                    try
                    {
                        foreach (var request in requests)
                        {
                            request.QuoteStatusID = statusNumber;
                            _context.Entry(request).State = EntityState.Modified;
                            _context.SaveChanges();
                        }
                        await transaction.CommitAsync();
                    }
                    catch (Exception e)
                    {
                        transaction.Rollback();
                        throw new Exception(AppUtility.GetExceptionMessage(e));
                    }
                    ReturnVal.SetStringAndBool(true, null);
                }
            }
            catch (Exception ex)
            {
                ReturnVal.SetStringAndBool(false,"Failed to update quote status- "+ AppUtility.GetExceptionMessage(ex));
            }
            return ReturnVal;

        }

        public async Task UpdateQuoteDetailsAsync(List<Request> requests, ParentQuote parentQuote)
        {
            try
            {
                foreach (var req in requests)
                {
                    //throw new Exception();
                    var request = await _requestsProc.ReadOneAsync(new List<Expression<Func<Request, bool>>> { r => r.RequestID == req.RequestID });
                    request.ParentQuote = parentQuote;
                    request.QuoteStatusID = 4;
                    request.Cost = req.Cost;
                    request.Currency = requests[0].Currency;
                    request.ExchangeRate = requests[0].ExchangeRate;
                    request.IncludeVAT = requests[0].IncludeVAT;
                    request.ExpectedSupplyDays = req.ExpectedSupplyDays;
                    _context.Entry(request).State = EntityState.Modified;
                }
                await _context.SaveChangesAsync();
            }
            catch(Exception ex)
            {
                throw ex;
            }
        }


        public async Task<StringWithBool> UpdatePaymentStatusAsync(AppUtility.PaymentsPopoverEnum newStatus, int requestID)
        {
            StringWithBool ReturnVal = new StringWithBool();
            try
            {

                using (var transaction = _context.Database.BeginTransaction())
                {
                    try
                    {
                        var request = await ReadOneAsync( new List<Expression<Func<Request, bool>>> { r => r.RequestID == requestID });

                        request.PaymentStatusID = (int)newStatus;
                        _context.Update(request);
                        await _context.SaveChangesAsync();
                        await transaction.CommitAsync();
                    }
                    catch (Exception e)
                    {
                        transaction.Rollback();
                        throw new Exception(AppUtility.GetExceptionMessage(e));
                    }
                    ReturnVal.SetStringAndBool(true, null);
                }
            }
            catch (Exception ex)
            {
                ReturnVal.SetStringAndBool(false, AppUtility.GetExceptionMessage(ex));
            }
            return ReturnVal;

        }

        public async Task RemoveFromInventoryAsync(int requestId)
        {

            var productID = ReadOneAsync( new List<Expression<Func<Request, bool>>> { r => r.RequestID == requestId }).Result.ProductID;
            var oldRequest = await ReadOneAsync( new List<Expression<Func<Request, bool>>> { r => r.ProductID == productID && r.RequestID != requestId, r => r.IsInInventory });
            if (oldRequest != null)
            {
                oldRequest.IsInInventory = false;
                _context.Update(oldRequest);
                await _context.SaveChangesAsync();
            }
        }

        public async Task ReceiveRequestWithoutTransactionAsync(ReceivedLocationViewModel receivedLocationViewModel, ReceivedModalVisualViewModel receivedModalVisualViewModel, Request requestReceived)
        {
          
            if (receivedLocationViewModel.CategoryType == 1)
            {
                if (receivedLocationViewModel.TemporaryLocation)
                {
                    await _requestLocationInstancesProc.SaveTempLocationWithoutTransactionAsync(receivedLocationViewModel, requestReceived);
                }
                else
                {
                    await _requestLocationInstancesProc.SaveLocationsWithoutTransactionAsync(receivedModalVisualViewModel, requestReceived, false);
                }
            }

            requestReceived.RequestStatusID = 3;
            requestReceived.IsPartial = false;
            if (receivedLocationViewModel.Request.ArrivalDate == DateTime.Today)
            {
                requestReceived.ArrivalDate = DateTime.Now;
            }
            else
            {
                requestReceived.ArrivalDate = receivedLocationViewModel.Request.ArrivalDate;
            }
            requestReceived.ApplicationUserReceiverID = receivedLocationViewModel.Request.ApplicationUserReceiverID;
            requestReceived.ApplicationUserReceiver = await _employeesProc.ReadOneAsync(new List<Expression<Func<Employee, bool>>> { u => u.Id == receivedLocationViewModel.Request.ApplicationUserReceiverID });

            requestReceived.NoteForClarifyDelivery = receivedLocationViewModel.Request.NoteForClarifyDelivery;
            requestReceived.IsClarify = receivedLocationViewModel.Request.IsClarify;
            requestReceived.IsInInventory = true;
            if (receivedLocationViewModel.Request.Batch != null || receivedLocationViewModel.Request.BatchExpiration != null)
            {
                requestReceived.Batch = receivedLocationViewModel.Request.Batch;
                requestReceived.BatchExpiration = receivedLocationViewModel.Request.BatchExpiration;
            }
            if (requestReceived.PaymentStatusID == 4)
            {
                requestReceived.PaymentStatusID = 3;
            }

            _context.Update(requestReceived);
            await _context.SaveChangesAsync();

            await RemoveFromInventoryAsync(requestReceived.RequestID);

            RequestNotification requestNotification = new RequestNotification();
            requestNotification.RequestID = requestReceived.RequestID;
            requestNotification.IsRead = false;
            requestNotification.ApplicationUserID = requestReceived.ApplicationUserCreatorID;
            requestNotification.RequestName = requestReceived.Product.ProductName;
            requestNotification.NotificationStatusID = 4;
            var FName = _employeesProc.ReadOneAsync(new List<Expression<Func<Employee, bool>>> { u => u.Id == requestReceived.ApplicationUserReceiverID }).Result.FirstName;
            requestNotification.Description = "received by " + FName;
            requestNotification.NotificationDate = DateTime.Now;
            requestNotification.Controller = "Requests";
            requestNotification.Action = "NotificationsView";
            requestNotification.Vendor = requestReceived.Product.Vendor.VendorEnName;
            await _requestNotificationsProc.CreateWithoutTransactionAsync(requestNotification);

            var didntArriveNotification = await _requestNotificationsProc.ReadOneAsync(new List<Expression<Func<RequestNotification, bool>>> { r => r.RequestID == requestReceived.RequestID && r.NotificationStatusID == 1 });
            if (didntArriveNotification != null)
            {
                await _requestNotificationsProc.DeleteWithoutTransactionAsync(new List<RequestNotification> { didntArriveNotification });
            }
        }

        public void CreatePartialRequest(ReceivedLocationViewModel receivedLocationViewModel, Request requestReceived, out decimal pricePerUnit)
        {
            requestReceived.RequestID = 0;
            requestReceived.SerialNumber =0;
            pricePerUnit = requestReceived.PricePerUnit;
            requestReceived.Unit = (uint)(requestReceived.Unit - receivedLocationViewModel.AmountArrived);
            requestReceived.Cost = pricePerUnit * requestReceived.Unit;
            requestReceived.IsPartial = true;
            _context.Entry(requestReceived).State = EntityState.Added;
            _context.SaveChanges();
        }

        public async Task<StringWithBool> UpdateExchangeRateByHistory()
        {
            StringWithBool ReturnVal = new StringWithBool();
            try
            {

                using (var transaction = _context.Database.BeginTransaction())
                {
                    try
                    {
                        await Read(new List<Expression<Func<Request, bool>>> { r => r.OrderMethod.DescriptionEnum.ToString() == AppUtility.OrderMethod.ExcelUpload.ToString() })
                            .ForEachAsync(r => { 
                                var rate = AppUtility.GetExchangeRateByDate(r.CreationDate);
                                r.ExchangeRate = rate;
                                r.Cost = (r.Cost/3.2m)*rate;
                                _context.Entry(r).State = EntityState.Modified;
                        });          
                        await _context.SaveChangesAsync();
                        await transaction.CommitAsync();
                    }
                    catch (Exception e)
                    {
                        transaction.Rollback();
                        throw new Exception(AppUtility.GetExceptionMessage(e));
                    }
                    ReturnVal.SetStringAndBool(true, null);
                }
            }
            catch (Exception ex)
            {
                ReturnVal.SetStringAndBool(false, AppUtility.GetExceptionMessage(ex));
            }
            return ReturnVal;
        }


        //private async Task<StringWithBool> MarkInventory()
        //{
        //    //before running this function, run the following in ssms:
        //    //update requests set IsInInventory = 'false'

        //    StringWithBool ReturnVal = new StringWithBool();
        //    try
        //    {
        //        using (var transaction = _context.Database.BeginTransaction())
        //        {
        //            try
        //            {
        //                var requests = ReadOneWithIgnoreQueryFilters( new List<Expression<Func<Request, bool>>> { r => r.RequestStatusID == 3 && r.OrderType != AppUtility.OrderTypeEnum.Save.ToString() });
        //                var requestsInInventory = requests.OrderByDescending(r => r.ArrivalDate).ToLookup(r => r.ProductID).Select(e => e.First());

        //                foreach (var r in requestsInInventory)
        //                {
        //                    r.IsInInventory = true;
        //                    _context.Update(r);
        //                }
        //                await _context.SaveChangesAsync();
        //                await transaction.CommitAsync();
        //            }
        //            catch (DbUpdateException ex)
        //            {
        //                await transaction.RollbackAsync();
        //                throw ex;
        //            }
        //            catch (Exception ex)
        //            {
        //                await transaction.RollbackAsync();
        //                throw ex;
        //            }

        //        }
        //    }
        //    catch (Exception ex)
        //    {
        //        ReturnVal.SetStringAndBool(false, AppUtility.GetExceptionMessage(ex));
        //    }
        //    return ReturnVal;


        //}
        //



        //private async Task UploadRequestsFromExcel()
        //{
        //    var InventoryFileName = @"C:\Users\debbie\OneDrive - Centarix\Desktop\inventoryexcel2.csv";
        //    var POFileName = @"C:\Users\debbie\OneDrive - Centarix\Desktop\ExcelForTesting\_2019.xlsx";

        //    var lineNumber = 0;

        //    var excelRequests = new ExcelQueryFactory(InventoryFileName);
        //    var excelInvoices = new ExcelQueryFactory(POFileName);
        //    try
        //    {
        //        var requests = from r in excelRequests.Worksheet<UploadExcelModel>("inventory excel") select r;
        //        var requestsInvoice = from i in excelInvoices.Worksheet<UploadInvoiceExcelModel>("orders") select i;
        //        var lastSerialNumber = int.Parse(_context.Products.IgnoreQueryFilters().Where(p => p.ProductSubcategory.ParentCategory.CategoryTypeID == 1).OrderBy(p => p).LastOrDefault()?.SerialNumber?.Substring(1) ?? "1");
        //        var currency = AppUtility.CurrencyEnum.USD;
        //        long lastParentRequestOrderNum = 1500;
        //        var requestInvoiceList = requestsInvoice.ToList();
        //        foreach (var r in requests)
        //        {
        //            lineNumber++;
        //            using (var transaction = _context.Database.BeginTransaction())
        //            {
        //                try
        //                {
        //                    var categories = await _context.ProductSubcategories.IgnoreQueryFilters().Include(pc => pc.ParentCategory).Where(ps => ps.ParentCategory.CategoryTypeID == 1).ToListAsync();
        //                    var requestedBy = _context.Employees.Where(e => e.Email == r.RequstedBy).FirstOrDefault()?.Id;
        //                    var receivedBy = _context.Employees.Where(e => e.Email == r.ReceivedBy).FirstOrDefault()?.Id;
        //                    var orderedBy = _context.Employees.Where(e => e.Email == r.OrderedBy).FirstOrDefault()?.Id;
        //                    var vendorID = _context.Vendors.Where(v => v.VendorEnName == r.VendorName).Select(v => v.VendorID).FirstOrDefault();
        //                    //check if product exists based on vendor catalog number
        //                    var productID = _context.Products.Where(p => p.VendorID == vendorID && p.CatalogNumber.ToLower() == r.CatalogNumber.ToLower()).Select(p => p.ProductID).FirstOrDefault();
        //                    var request = new Request() { };
        //                    if (vendorID == 0)
        //                    {

        //                        WriteErrorToFile("Row " + lineNumber + " did not have a proper vendor");
        //                        _context.ChangeTracker.Entries()
        //                            .Where(e => e.Entity != null).ToList();


        //                        throw new Exception("failed to find vendor");
        //                    }
        //                    if (productID == 0)
        //                    {
        //                        var product = new Product()
        //                        {
        //                            ProductName = r.ItemName,
        //                            VendorID = vendorID,
        //                            ProductSubcategoryID = categories.Where(ps => new string(ps.ProductSubcategoryDescription.ToLower().Where(c => char.IsLetterOrDigit(c)).ToArray()) == new string(r.ProductSubCategoryName.ToLower().Where(c => char.IsLetterOrDigit(c)).ToArray()) && ps.ParentCategory.ParentCategoryDescription.ToLower() == r.ParentCategoryName.ToLower()).Select(ps => ps.ProductSubcategoryID).FirstOrDefault(),
        //                            CatalogNumber = r.CatalogNumber,
        //                            SerialNumber = "L" + lastSerialNumber++,
        //                            ProductCreationDate = DateTime.Now,
        //                            UnitTypeID = -1,
        //                        };
        //                        if (lineNumber > 548)
        //                        {
        //                            lineNumber = lineNumber;
        //                        }
        //                        _context.Entry(product).State = EntityState.Added;
        //                        await _context.SaveChangesAsync();
        //                        request.ProductID = product.ProductID;
        //                    }
        //                    else
        //                    {
        //                        request.ProductID = productID;
        //                    }

        //                    var orderType = AppUtility.OrderTypeEnum.ExcelUpload.ToString();


        //                    var exchangeRate = AppUtility.GetExchangeRateByDate(r.DateOrdered);
        //                    //cost = cost * exchangeRate; //always from quartzy in dollars        
        //                    int parentRequestID = 0;
        //                    if (r.OrderNumber != "")
        //                    {
        //                        parentRequestID = _context.ParentRequests.Where(pr => pr.QuartzyOrderNumber == r.OrderNumber).Select(pr => pr.ParentRequestID).FirstOrDefault();

        //                    }
        //                    if (parentRequestID != 0)
        //                    {
        //                        request.ParentRequestID = parentRequestID;
        //                    }
        //                    else
        //                    {
        //                        ParentRequest parentRequest = null;
        //                        if (r.OrderNumber != null && r.OrderNumber.StartsWith("1") && r.OrderNumber.Length == 8)
        //                        {
        //                            parentRequest = new ParentRequest() { QuartzyOrderNumber = r.OrderNumber, OrderNumber = int.Parse(r.OrderNumber.Substring(3)), ApplicationUserID = orderedBy, OrderDate = r.DateOrdered };
        //                        }
        //                        else
        //                        {
        //                            parentRequest = new ParentRequest() { QuartzyOrderNumber = r.OrderNumber, OrderNumber = lastParentRequestOrderNum++, ApplicationUserID = orderedBy, OrderDate = r.DateOrdered };
        //                        }
        //                        _context.Entry(parentRequest).State = EntityState.Added;
        //                        await _context.SaveChangesAsync();
        //                        request.ParentRequestID = parentRequest.ParentRequestID;
        //                    }
        //                    request.ApplicationUserCreatorID = requestedBy;
        //                    request.RequestStatusID = 3;
        //                    request.ApplicationUserReceiverID = receivedBy;
        //                    request.ArrivalDate = r.DateReceived;
        //                    request.Cost = r.TotalPrice * 3.2M;
        //                    request.Currency = currency.ToString();
        //                    request.Unit = r.Unit;
        //                    request.ExchangeRate = 3.2M;
        //                    request.CreationDate = r.DateRequested;
        //                    request.ParentQuoteID = null;
        //                    request.OrderType = orderType;
        //                    request.IncludeVAT = true;
        //                    request.URL = r.Url;
        //                    request.PaymentStatusID = 2;
        //                    request.Installments = 1;
        //                    _context.Entry(request).State = EntityState.Added;
        //                    await _context.SaveChangesAsync();

        //                    var requestLocationInstance = new RequestLocationInstance() { RequestID = request.RequestID, LocationInstanceID = -1 };
        //                    _context.Entry(requestLocationInstance).State = EntityState.Added;
        //                    await _context.SaveChangesAsync();
        //                    try
        //                    {
        //                        try
        //                        {
        //                            if (r.OrderNumber != null && r.OrderNumber != "")
        //                            {

        //                                var invoiceRow = requestInvoiceList.Where(i => i.OrderNumber == r.OrderNumber && new string(i.CatalogNumber.ToLower().Where(c => char.IsLetterOrDigit(c)).ToArray()) == new string(r.CatalogNumber.ToLower().Where(c => char.IsLetterOrDigit(c)).ToArray()) && i.DocumentNumber != 0 && i.InvoiceNumber != "").FirstOrDefault();
        //                                // var invoiceNumbers = invoiceRows.Select(ir => ir.InvoiceNumber).ToList();
        //                                if (invoiceRow != null)
        //                                {

        //                                    WriteErrorToFile("this row went in perfectly");
        //                                    await SetInvoiceAndPaymentsAccordingToResultsFromDB(r, request, invoiceRow);
        //                                }


        //                                //var invoiceIds= invoices.Select(i=>i.InvoiceID);
        //                                //if(invoiceIds.Count()==invoiceNumbers.Count() && invoiceNumbers.Count()>0)
        //                                //{
        //                                //    foreach(var invoiceID in invoiceIds)
        //                                //    {
        //                                //        var payment = new Payment() { InvoiceID = invoiceID, HasInvoice = true, IsPaid = true, PaymentTypeID = 3, RequestID = request.RequestID, PaymentDate = r.DateOrdered, CompanyAccountID = 5 };
        //                                //        _context.Entry(payment).State = EntityState.Added;
        //                                //    }

        //                                //}
        //                                //else if (invoiceIds.Count() != invoiceNumbers.Count() && invoiceNumbers.Count() > 0)
        //                                //{
        //                                //    foreach (var invoiceID in invoiceIds)
        //                                //    {
        //                                //        var payment = new Payment() { InvoiceID = invoiceID, HasInvoice = true, IsPaid = true, PaymentTypeID = 3, RequestID = request.RequestID, PaymentDate = r.DateOrdered, CompanyAccountID = 5 };
        //                                //        _context.Entry(payment).State = EntityState.Added;
        //                                //    }
        //                                //    var invoiceNumbersNotInDataBaseYet = invoiceRows.Where(i => !invoices.Select(i=>i.InvoiceNumber).Contains(i.InvoiceNumber));
        //                                //    foreach(var ie in invoiceNumbersNotInDataBaseYet)
        //                                //    {
        //                                //        var invoice = new Invoice() {InvoiceNumber = ie.InvoiceNumber, InvoiceDate = ie.InvoiceDate };
        //                                //        _context.Entry(invoice).State = EntityState.Added;
        //                                //        await _context.SaveChangesAsync();
        //                                //        var payment = new Payment() { InvoiceID = invoice.InvoiceID, HasInvoice = true, IsPaid = true, PaymentTypeID =3 , RequestID = request.RequestID, PaymentDate = r.DateOrdered, CompanyAccountID = 5 };
        //                                //        _context.Entry(payment).State = EntityState.Added;

        //                                //    }
        //                                //    //upload the invoice documents ---
        //                                //}
        //                                else // no invoice in the excel
        //                                {
        //                                    var invoiceRows = requestInvoiceList.Where(i => i.OrderNumber == r.OrderNumber && i.DocumentNumber > 0 && i.InvoiceNumber != "");
        //                                    if (invoiceRows.Count() == 0)
        //                                    {
        //                                        WriteErrorToFile("There is not matching catalog number and order number to row" + lineNumber + " - added default invoice for requestID: " + request.RequestID);

        //                                        var emptyInvoice = new Invoice() { InvoiceNumber = "000000000", InvoiceDate = r.DateOrdered };
        //                                        _context.Entry(emptyInvoice).State = EntityState.Added;
        //                                        await _context.SaveChangesAsync();
        //                                        var payment = new Payment() { InvoiceID = emptyInvoice.InvoiceID, HasInvoice = true, IsPaid = true, PaymentTypeID = 3, RequestID = request.RequestID, PaymentDate = r.DateOrdered, CompanyAccountID = 5 };
        //                                        _context.Entry(payment).State = EntityState.Added;
        //                                        //no docments either....

        //                                    }
        //                                    else if (invoiceRows.Count() == 1)
        //                                    {

        //                                        WriteErrorToFile("this row went in okay - there was one row for the po # but not matching catlog number");
        //                                        await SetInvoiceAndPaymentsAccordingToResultsFromDB(r, request, invoiceRows.FirstOrDefault());
        //                                    }
        //                                    else if (invoiceRows.Select(ir => ir.InvoiceNumber).Distinct().Count() < 2 && invoiceRows.Select(ir => ir.DocumentNumber).Distinct().Count() < 2)
        //                                    {
        //                                        WriteErrorToFile("this row went in okay - there was one row for the po # but not matching catlog number");

        //                                        await SetInvoiceAndPaymentsAccordingToResultsFromDB(r, request, invoiceRows.FirstOrDefault());
        //                                    }
        //                                    else
        //                                    {
        //                                        WriteErrorToFile("There are duplicate matching invoices for " + lineNumber + " - added default invoice for requestID: " + request.RequestID);

        //                                        var emptyInvoice = new Invoice() { InvoiceNumber = "000000000", InvoiceDate = r.DateOrdered };
        //                                        _context.Entry(emptyInvoice).State = EntityState.Added;
        //                                        await _context.SaveChangesAsync();
        //                                        var payment = new Payment() { InvoiceID = emptyInvoice.InvoiceID, HasInvoice = true, IsPaid = true, PaymentTypeID = 3, RequestID = request.RequestID, PaymentDate = r.DateOrdered, CompanyAccountID = 5 };
        //                                        _context.Entry(payment).State = EntityState.Added;
        //                                        //no docments either....
        //                                    }
        //                                }
        //                                await _context.SaveChangesAsync();
        //                            }
        //                            else
        //                            {
        //                                var emptyInvoice = new Invoice() { InvoiceNumber = "000000000", InvoiceDate = r.DateOrdered };
        //                                _context.Entry(emptyInvoice).State = EntityState.Added;
        //                                await _context.SaveChangesAsync();
        //                                var payment = new Payment() { InvoiceID = emptyInvoice.InvoiceID, HasInvoice = true, IsPaid = true, PaymentTypeID = 3, RequestID = request.RequestID, PaymentDate = r.DateOrdered, CompanyAccountID = 5 };
        //                                _context.Entry(payment).State = EntityState.Added;
        //                                //no docments either....
        //                                WriteErrorToFile("This item has no order number" + lineNumber + " - added default invoice for requestID: " + request.RequestID);

        //                            }
        //                        }
        //                        catch (Exception ex)
        //                        {
        //                            WriteErrorToFile("Row " + lineNumber + " failed to enter database: " + AppUtility.GetExceptionMessage(ex));
        //                            _context.ChangeTracker.Entries()
        //                                .Where(e => e.Entity != null).ToList()
        //                                .ForEach(e => e.State = EntityState.Detached);
        //                        }

        //                    }
        //                    catch (Exception ex)
        //                    {
        //                        WriteErrorToFile("Error reading invoice file: " + AppUtility.GetExceptionMessage(ex));
        //                    }
        //                    await transaction.CommitAsync();
        //                }
        //                catch (Exception ex)
        //                {
        //                    WriteErrorToFile("Row " + lineNumber + " failed to enter database: " + AppUtility.GetExceptionMessage(ex));
        //                }

        //            }
        //        }
        //    }
        //    catch (Exception ex)
        //    {
        //        WriteErrorToFile("Error reading requests file: " + AppUtility.GetExceptionMessage(ex));
        //    }
        //}

        //private async Task SetInvoiceAndPaymentsAccordingToResultsFromDB(UploadExcelModel r, Request request, UploadInvoiceExcelModel invoiceRow)
        //{
        //    string sourceFile = @"C:\Users\debbie\OneDrive - Centarix\Desktop\DocumentsInvoices\" + invoiceRow.DocumentNumber + ".pdf";

        //    string uploadFolder = Path.Combine(_hostingEnvironment.WebRootPath, AppUtility.ParentFolderName.Requests.ToString());
        //    string requestFolderTo = Path.Combine(uploadFolder, request.RequestID.ToString());
        //    string uploadFolderPathTo = Path.Combine(requestFolderTo, AppUtility.FolderNamesEnum.Invoices.ToString());

        //    try
        //    {
        //        if (!Directory.Exists(requestFolderTo))
        //        {
        //            Directory.CreateDirectory(requestFolderTo);
        //            Directory.CreateDirectory(uploadFolderPathTo);
        //        }
        //        else
        //        {
        //            if (!Directory.Exists(uploadFolderPathTo))
        //            {
        //                Directory.CreateDirectory(uploadFolderPathTo);
        //            }
        //        }
        //        System.IO.File.Copy(sourceFile, uploadFolderPathTo + @"\" + invoiceRow.DocumentNumber + ".pdf", true);
        //        WriteErrorToFile("file was addeded for request id:" + request.RequestID);
        //    }
        //    catch (IOException iox)
        //    {
        //        WriteErrorToFile("error adding file" + request.RequestID);
        //    }
        //    var invoiceDB = _context.Invoices.Where(i => i.InvoiceNumber == invoiceRow.InvoiceNumber).AsNoTracking().FirstOrDefault();
        //    if (invoiceDB != null)
        //    {
        //        var payment = new Payment() { InvoiceID = invoiceDB.InvoiceID, HasInvoice = true, IsPaid = true, PaymentTypeID = 3, RequestID = request.RequestID, PaymentDate = r.DateOrdered, CompanyAccountID = 5 };
        //        _context.Entry(payment).State = EntityState.Added;
        //    }
        //    else
        //    {
        //        var invoice = new Invoice() { InvoiceNumber = invoiceRow.InvoiceNumber, InvoiceDate = invoiceRow.InvoiceDate };
        //        _context.Entry(invoice).State = EntityState.Added;
        //        await _context.SaveChangesAsync();
        //        var payment = new Payment() { InvoiceID = invoice.InvoiceID, HasInvoice = true, IsPaid = true, PaymentTypeID = 3, RequestID = request.RequestID, PaymentDate = r.DateOrdered, CompanyAccountID = 5 };
        //        _context.Entry(payment).State = EntityState.Added;
        //    }
        //}

        //private static void WriteErrorToFile(string message)
        //{
        //    var errorFilePath = "InventoryError.txt";
        //    if (!System.IO.File.Exists(errorFilePath))
        //    {
        //        var errorFile = System.IO.File.Create(errorFilePath);
        //        errorFile.Close();

        //    }
        //    var sw = System.IO.File.AppendText(errorFilePath);
        //    sw.WriteLine("\n" + message);
        //    sw.Close();
        //}
    }



}