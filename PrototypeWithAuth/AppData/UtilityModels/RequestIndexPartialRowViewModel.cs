using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using PrototypeWithAuth.AppData;
using PrototypeWithAuth.AppData.UtilityModels;
using PrototypeWithAuth.Data;
using PrototypeWithAuth.Models;

namespace PrototypeWithAuth.ViewModels
{
    public class RequestIndexPartialRowViewModel
    {
        private static RequestIndexObject requestIndexObject;
        private static string defaultImage;
        private List<IconColumnViewModel> iconList;
        public Request r;
        private ApplicationUser user;
        private FavoriteRequest favoriteRequest;
        private ShareRequest shareRequest;
        private string checkboxString;
        private LocationInstance locationInstance;
        private LocationInstance locationInstanceParent;
        private ParentRequest parentRequest;
        private List<Payment> payments;
        private Request partialRequest;
        private TimePeriod timePeriod;
        public bool viewOnly;
        private string currentCurrency = "currencyEnum";

        public RequestIndexPartialRowViewModel() { }
        public RequestIndexPartialRowViewModel(AppUtility.IndexTableTypes indexTableTypes, Request request, OrderMethod orderMethod, Employee requestUser, Product product, Vendor vendor, ProductSubcategory productSubcategory,
            ParentCategory parentCategory, UnitType unitType, UnitType subUnitType, UnitType subSubUnitType, RequestIndexObject requestIndexObject, List<IconColumnViewModel> iconList,
            string defaultImage)
            : this(indexTableTypes, request, orderMethod, requestUser, product, vendor, productSubcategory, parentCategory, unitType, subUnitType, subSubUnitType, requestIndexObject, iconList, defaultImage,
                  parentRequest: null)
        {
        }
        public RequestIndexPartialRowViewModel(AppUtility.IndexTableTypes indexTableTypes, Request request, OrderMethod orderMethod, Employee requestUser, Product product, Vendor vendor, ProductSubcategory productSubcategory,
            ParentCategory parentCategory, UnitType unitType, UnitType subUnitType, UnitType subSubUnitType, RequestIndexObject requestIndexObject, List<IconColumnViewModel> iconList,
            string defaultImage, ParentRequest parentRequest)
            : this(indexTableTypes, request, orderMethod, requestUser, product, vendor, productSubcategory, parentCategory, unitType, subUnitType, subSubUnitType, requestIndexObject, iconList, defaultImage,
                favoriteRequest: null, null, parentRequest)
        {
        }
        public RequestIndexPartialRowViewModel(AppUtility.IndexTableTypes indexTableTypes, Request request, OrderMethod orderMethod, Employee requestUser, Product product, Vendor vendor, ProductSubcategory productSubcategory,
            ParentCategory parentCategory, UnitType unitType, UnitType subUnitType, UnitType subSubUnitType, RequestIndexObject requestIndexObject, List<IconColumnViewModel> iconList,
            string defaultImage, string checkboxString)
            : this(indexTableTypes, request, orderMethod, requestUser, product, vendor, productSubcategory, parentCategory, unitType, subUnitType, subSubUnitType, requestIndexObject, iconList, defaultImage,
                null, checkboxString)
        {
        }
        public RequestIndexPartialRowViewModel(AppUtility.IndexTableTypes indexTableTypes, Request request, OrderMethod orderMethod, Employee requestUser, Product product, Vendor vendor, ProductSubcategory productSubcategory,
            ParentCategory parentCategory, UnitType unitType, UnitType subUnitType, UnitType subSubUnitType, RequestIndexObject requestIndexObject, List<IconColumnViewModel> iconList,
            string defaultImage, ParentRequest parentRequest, ApplicationUser user)
            : this(indexTableTypes, request, orderMethod, requestUser, product, vendor, productSubcategory, parentCategory, unitType, subUnitType, subSubUnitType, requestIndexObject, iconList, defaultImage,
                null, user, parentRequest)
        {
        }
        public RequestIndexPartialRowViewModel(AppUtility.IndexTableTypes indexTableTypes, Request request, OrderMethod orderMethod, Product product, Vendor vendor, ProductSubcategory productSubcategory, 
            ParentCategory parentCategory, UnitType unitType, UnitType subUnitType, UnitType subSubUnitType, RequestIndexObject requestIndexObject, List<IconColumnViewModel> iconList, 
            string defaultImage, ParentRequest parentRequest, String checkboxString, Request partialRequest)
            : this(indexTableTypes, request, orderMethod, null, product, vendor, productSubcategory, parentCategory, unitType, subUnitType, subSubUnitType, requestIndexObject, iconList, defaultImage, null, null, null, null,null,null,
                parentRequest, checkboxString, null, null, partialRequest, false)
        {
        }        
        public RequestIndexPartialRowViewModel(AppUtility.IndexTableTypes indexTableTypes, Request request, OrderMethod orderMethod, Employee requestUser, Product product, Vendor vendor, ProductSubcategory productSubcategory,
          ParentCategory parentCategory, UnitType unitType, UnitType subUnitType, UnitType subSubUnitType, RequestIndexObject requestIndexObject, List<IconColumnViewModel> iconList,
          string defaultImage, ParentRequest parentRequest, String checkboxString)
          : this(indexTableTypes, request, orderMethod, requestUser, product, vendor, productSubcategory, parentCategory, unitType, subUnitType, subSubUnitType, requestIndexObject, iconList, defaultImage,
              parentRequest, checkboxString, new List<Payment>())
        {
        }
        public RequestIndexPartialRowViewModel(AppUtility.IndexTableTypes indexTableTypes, Request request, OrderMethod orderMethod, Product product, Vendor vendor, ProductSubcategory productSubcategory,
          ParentCategory parentCategory, UnitType unitType, UnitType subUnitType, UnitType subSubUnitType, RequestIndexObject requestIndexObject, List<IconColumnViewModel> iconList,
          string defaultImage, ParentRequest parentRequest, String checkboxString, List<Payment> payments)
          : this(indexTableTypes, request, orderMethod, null, product, vendor, productSubcategory, parentCategory, unitType, subUnitType, subSubUnitType, requestIndexObject, iconList, defaultImage,
              parentRequest, checkboxString, payments)
        {
        }
        public RequestIndexPartialRowViewModel(AppUtility.IndexTableTypes indexTableTypes, Request request, OrderMethod orderMethod, Employee requestUser, Product product, Vendor vendor, ProductSubcategory productSubcategory,
            ParentCategory parentCategory, UnitType unitType, UnitType subUnitType, UnitType subSubUnitType, RequestIndexObject requestIndexObject, List<IconColumnViewModel> iconList,
            string defaultImage, ParentRequest parentRequest, String checkboxString, List<Payment> payments)
            : this(indexTableTypes, request, orderMethod, requestUser, product, vendor, productSubcategory, parentCategory, unitType, subUnitType, subSubUnitType, requestIndexObject, iconList, defaultImage, 
                null, null, null, null, null, null, parentRequest, checkboxString, null, payments, null, false)
        {
        }
        public RequestIndexPartialRowViewModel(AppUtility.IndexTableTypes indexTableTypes, Request request, OrderMethod orderMethod, Employee requestUser, Product product, Vendor vendor, ProductSubcategory productSubcategory,
            ParentCategory parentCategory, UnitType unitType, UnitType subUnitType, UnitType subSubUnitType, RequestIndexObject requestIndexObject, List<IconColumnViewModel> iconList,
            string defaultImage, String checkboxString, ParentQuote parentQuote)
            : this(indexTableTypes, request, orderMethod, requestUser, product, vendor, productSubcategory, parentCategory, unitType, subUnitType, subSubUnitType, requestIndexObject, iconList, defaultImage, 
                null, null, null, null, null, null, null, checkboxString, parentQuote, null, null, false)
        {
        }
        public RequestIndexPartialRowViewModel(AppUtility.IndexTableTypes indexTableTypes, Request request, OrderMethod orderMethod, Employee requestUser, Product product, Vendor vendor, ProductSubcategory productSubcategory,
            ParentCategory parentCategory, UnitType unitType, UnitType subUnitType, UnitType subSubUnitType, RequestIndexObject requestIndexObject, List<IconColumnViewModel> iconList,
            string defaultImage, FavoriteRequest favoriteRequest, ShareRequest shareRequest, Employee shareRequestFromUser, ApplicationUser user, LocationInstance locationInstance,
            LocationInstance parentLocationInstance, ParentRequest parentRequest, string checkboxString, ParentQuote parentQuote, List<Payment> payments, Request partialRequest, bool viewOnly)
            : this(indexTableTypes, request, orderMethod, requestUser, product, vendor, productSubcategory,parentCategory, unitType, subUnitType, subSubUnitType, requestIndexObject, iconList,
            defaultImage, favoriteRequest, shareRequest, shareRequestFromUser, user, locationInstance, parentLocationInstance, parentRequest, checkboxString, parentQuote, payments,
            partialRequest, viewOnly, null)
        { 
        }
        public RequestIndexPartialRowViewModel(AppUtility.IndexTableTypes indexTableTypes, Request request, OrderMethod orderMethod, Employee requestUser, Product product, Vendor vendor, ProductSubcategory productSubcategory, 
            ParentCategory parentCategory, UnitType unitType, UnitType subUnitType, UnitType subSubUnitType, RequestIndexObject requestIndexObject, List<IconColumnViewModel> iconList, 
            string defaultImage, FavoriteRequest favoriteRequest, ShareRequest shareRequest, Employee shareRequestFromUser, ApplicationUser user, LocationInstance locationInstance, 
            LocationInstance parentLocationInstance, ParentRequest parentRequest, string checkboxString, ParentQuote parentQuote, List<Payment> payments, Request partialRequest, bool viewOnly, TimePeriod timePeriod)
        {
            r = request;
            r.OrderMethod = orderMethod;
            r.ApplicationUserCreator = requestUser;
            r.Product = product;
            r.Product.Vendor = vendor;
            r.Product.ProductSubcategory = productSubcategory;
            r.Product.ProductSubcategory.ParentCategory = parentCategory;
            r.Product.UnitType = unitType;
            r.Product.SubUnitType = subUnitType;
            r.Product.SubSubUnitType = subSubUnitType;
            if(!(r.Product is SingleOrder))
            {
                this.timePeriod = timePeriod;
            }
            if (vendor != null) //for samples
            {
                vendor.Products = null;
                Vendor = vendor;
            }
            TotalCost = (r.Cost ?? 0) + r.VAT;
            ExchangeRate = r.ExchangeRate;
            RequestIndexPartialRowViewModel.requestIndexObject = requestIndexObject;
            RequestIndexPartialRowViewModel.defaultImage = defaultImage;
            this.iconList = iconList;
            r.ParentRequest = parentRequest;
            r.ParentQuote = parentQuote;
            this.payments = payments;
            if (locationInstance != null)
            {
                r.RequestLocationInstances = new ListImplementsModelBase<RequestLocationInstance>() { new RequestLocationInstance { Request = r, LocationInstance = locationInstance } };
                r.RequestLocationInstances.FirstOrDefault().LocationInstance.LocationInstanceParent = parentLocationInstance;
            }
            this.user = user;
            this.favoriteRequest = favoriteRequest;
            this.shareRequest = shareRequest;
            if (shareRequest != null)
            {
                this.shareRequest.FromApplicationUser = shareRequestFromUser;
            }
            this.checkboxString = checkboxString;
            this.partialRequest = partialRequest;
            this.viewOnly = viewOnly;
            switch (indexTableTypes)
            {
                case AppUtility.IndexTableTypes.Approved:
                    Columns = GetApproveColumns().ToList();
                    break;
                case AppUtility.IndexTableTypes.ReceivedInventoryFavorites:
                    Columns = GetReceivedInventoryFavoriteColumns().ToList();
                    break;
                case AppUtility.IndexTableTypes.Ordered:
                    Columns = GetOrderedColumns().ToList();
                    break;
                case AppUtility.IndexTableTypes.ReceivedInventoryShared:
                    Columns = GetReceivedInventorySharedColumns().ToList();
                    break;
                case AppUtility.IndexTableTypes.ReceivedInventory:
                    Columns = GetReceivedInventoryColumns().ToList();
                    break;
                case AppUtility.IndexTableTypes.Summary:
                    Columns = GetSummaryColumns().ToList();
                    break;
                case AppUtility.IndexTableTypes.SummaryProprietary:
                    Columns = GetSummaryProprietaryColumns().ToList();
                    break;
                case AppUtility.IndexTableTypes.OrderedOperations:
                    Columns = GetOrderedOperationsColumns().ToList();
                    break;
                case AppUtility.IndexTableTypes.ReceivedInventoryOperations:
                    Columns = GetReceivedInventoryOperationsColumns().ToList();
                    break;
                case AppUtility.IndexTableTypes.AccountingGeneral:
                    Columns = GetAccountingGeneralColumns().ToList();
                    break;
                case AppUtility.IndexTableTypes.Cart:
                    Columns = GetCartColumns().ToList();
                    break;
                case AppUtility.IndexTableTypes.AccountingNotifications:
                    Columns = GetAccountingNotificationsColumns().ToList();
                    break;
                case AppUtility.IndexTableTypes.AccountingPaymentsInstallments:
                    //Columns = GetAccountingPaymentsInstallmentsColumns().ToList();
                    //break;
                case AppUtility.IndexTableTypes.AccountingPaymentsDefault:
                    Columns = GetAccountingPaymentsDefaultColumns().ToList();
                    break;
                case AppUtility.IndexTableTypes.LabQuotes:
                    Columns = GetLabQuotesColumns().ToList();
                    break;
                case AppUtility.IndexTableTypes.LabOrders:
                    Columns = GetLabOrdersColumns().ToList();
                    break;
                case AppUtility.IndexTableTypes.RequestLists:
                    Columns = GetRequestListColumns().ToList();
                    break;
                case AppUtility.IndexTableTypes.RecurringExpensesOperations:
                    Columns = GetRecurringExpensesOperationsColumns().ToList();
                    break;
                case AppUtility.IndexTableTypes.CartOperations:
                    Columns = GetCartOperationsColumns().ToList();
                    break;
            }
            //nullify r for json purposes because it is not needed anymore
            r=null;
            r= new Request();
            r.RequestID= request.RequestID;
        }

        

        public RequestIndexPartialRowViewModel(AppUtility.IndexTableTypes indexTableTypes, Request request, OrderMethod orderMethod, Employee requestUser, Product product, Vendor vendor, ProductSubcategory productSubcategory, ParentCategory parentCategory, UnitType unitType, UnitType subUnitType, UnitType subSubUnitType, RequestIndexObject requestIndexObject, List<IconColumnViewModel> iconList, string defaultImage, FavoriteRequest favoriteRequest, ShareRequest shareRequest, ApplicationUser user, LocationInstance locationInstance, LocationInstance locationInstanceParent, ParentRequest parentRequest)
            : this(indexTableTypes, request, orderMethod, requestUser, product, vendor, productSubcategory, parentCategory, unitType, subUnitType, subSubUnitType, requestIndexObject, iconList, defaultImage, favoriteRequest, shareRequest, null, user, locationInstance, locationInstanceParent, parentRequest, null, null, null, null, false)
        {
        }
        public RequestIndexPartialRowViewModel(AppUtility.IndexTableTypes indexTableTypes, Request request, OrderMethod orderMethod, Employee requestUser,  Product product, Vendor vendor, ProductSubcategory productSubcategory, ParentCategory parentCategory, UnitType unitType, UnitType subUnitType, UnitType subSubUnitType, RequestIndexObject requestIndexObject, List<IconColumnViewModel> iconList, string defaultImage, FavoriteRequest favoriteRequest, ShareRequest shareRequest, Employee shareRequestFromUser, ApplicationUser user, LocationInstance locationInstance, LocationInstance locationInstanceParent, ParentRequest parentRequest)
            : this(indexTableTypes, request, orderMethod, requestUser, product, vendor, productSubcategory, parentCategory, unitType, subUnitType, subSubUnitType, requestIndexObject, iconList, defaultImage, favoriteRequest, shareRequest, shareRequestFromUser, user, locationInstance, locationInstanceParent, parentRequest, null, null, null, null, false)
        {
        }
        public RequestIndexPartialRowViewModel(AppUtility.IndexTableTypes indexTableTypes, Request request, OrderMethod orderMethod, Product product, Vendor vendor, ProductSubcategory productSubcategory, ParentCategory parentCategory, UnitType unitType, UnitType subUnitType, UnitType subSubUnitType, RequestIndexObject requestIndexObject, List<IconColumnViewModel> iconList, string defaultImage, FavoriteRequest favoriteRequest, ShareRequest shareRequest, ApplicationUser user, LocationInstance locationInstance, LocationInstance locationInstanceParent, ParentRequest parentRequest, bool viewOnly)
            : this(indexTableTypes, request, orderMethod, null, product, vendor, productSubcategory, parentCategory, unitType, subUnitType, subSubUnitType, requestIndexObject, iconList, defaultImage, favoriteRequest, shareRequest, null, user, locationInstance, locationInstanceParent, parentRequest, null, null, null, null, viewOnly)
        {
        }
        public RequestIndexPartialRowViewModel(AppUtility.IndexTableTypes indexTableTypes, Request request, OrderMethod orderMethod, Employee requestUser, Product product, Vendor vendor, ProductSubcategory productSubcategory, ParentCategory parentCategory, UnitType unitType, UnitType subUnitType, UnitType subSubUnitType, RequestIndexObject requestIndexObject, List<IconColumnViewModel> iconList, string defaultImage, FavoriteRequest favoriteRequest, ApplicationUser user,  ParentRequest parentRequest)
           : this(indexTableTypes, request, orderMethod, requestUser, product, vendor, productSubcategory, parentCategory, unitType, subUnitType, subSubUnitType, requestIndexObject, iconList, defaultImage, favoriteRequest, null, null, user, null, null, parentRequest, null, null, null, null, false)
        {
        }

        public RequestIndexPartialRowViewModel(AppUtility.IndexTableTypes indexTableTypes, Request request, OrderMethod orderMethod, Employee requestUser, Product product, Vendor vendor, ProductSubcategory productSubcategory, ParentCategory parentCategory, UnitType unitType, UnitType subUnitType, UnitType subSubUnitType, RequestIndexObject requestIndexObject, List<IconColumnViewModel> iconList, string defaultImage, FavoriteRequest favoriteRequest, ApplicationUser user, LocationInstance locationInstance, LocationInstance locationInstanceParent, ParentRequest parentRequest)
            : this(indexTableTypes, request, orderMethod, requestUser, product, vendor, productSubcategory, parentCategory, unitType, subUnitType, subSubUnitType, requestIndexObject, iconList, defaultImage, favoriteRequest, null, null, user, locationInstance, locationInstanceParent, parentRequest, null, null, null, null, false)
        
        {          
        }

        public List<RequestIndexPartialColumnViewModel> Columns { get; set; }
        public Vendor Vendor { get; set; }
        public decimal TotalCost { get; set; }
        [Column(TypeName = "decimal(18, 3)")]
        public decimal ExchangeRate { get; set; }
        public string ButtonClasses { get; set; }
        public string ButtonText { get; set; }
        private static List<IconColumnViewModel> GetIconsByIndividualRequest(int RequestID, List<IconColumnViewModel> iconList, bool needsPlaceholder, FavoriteRequest favoriteRequest = null, Request request = null, ApplicationUser user = null, bool viewOnly = false)
        {
            var newIconList = AppUtility.DeepClone<List<IconColumnViewModel>>(iconList);
            //favorite icon
            var favIconIndex = newIconList.FindIndex(ni => ni.IconAjaxLink?.Contains("request-favorite") ?? false);

            if (favIconIndex != -1 && favoriteRequest != null)
            {
                var unLikeIcon = new IconColumnViewModel(" icon-favorite-24px", "var(--order-inv-color)", "request-favorite request-unlike", "Unfavorite");
                newIconList[favIconIndex] = unLikeIcon;
            }

            if (request != null)
            {
                var placeholder = new IconColumnViewModel("Placeholder");

                //for approval icon
                var forApprovalIconIndex = newIconList.FindIndex(ni => ni.IconAjaxLink?.Contains("approve-order") ?? false);
                if (request.RequestStatusID != 1 && forApprovalIconIndex != -1)
                {
                    newIconList.RemoveAt(forApprovalIconIndex);
                    newIconList.Insert(forApprovalIconIndex, placeholder);
                }
                var cantApproveIconIndex = newIconList.FindIndex(ni => ni.TooltipTitle?.Contains("Needs Approval") ?? false);
                if (request.RequestStatusID != 1 && cantApproveIconIndex != -1)
                {
                    newIconList.RemoveAt(cantApproveIconIndex);
                    newIconList.Insert(cantApproveIconIndex, placeholder);
                }
                //resend icon
                var resendIconIndex = newIconList.FindIndex(ni => ni.IconClass.Equals("Resend"));
                if (request.QuoteStatusID == 1 && resendIconIndex != -1)
                {
                    newIconList.RemoveAt(resendIconIndex);
                    newIconList.Insert(resendIconIndex, placeholder);
                }

                var reorderIconIndex = newIconList.FindIndex(ni => ni.TooltipTitle == "Reorder");
                if (reorderIconIndex != -1)
                {
                    if (request.Product.UnitTypeID == -1 || request.Product.ProductSubcategory.IsOldSubCategory)
                    {
                        newIconList.RemoveAt(reorderIconIndex);
                        newIconList.Insert(reorderIconIndex, placeholder);
                    }
                }
                var morePopoverIndex = newIconList.FindIndex(ni => ni.IconAjaxLink == "popover-more");
                if (morePopoverIndex != -1)
                {
                    var popoverReorder = newIconList.ElementAt(morePopoverIndex).IconPopovers.FindIndex(ni => ni.Action == "Reorder");
                    if (popoverReorder != -1)
                    {
                        if (request.Product.UnitTypeID == -1 || request.Product.ProductSubcategory.IsOldSubCategory)
                        {
                            newIconList[morePopoverIndex].IconPopovers.RemoveAt(popoverReorder);
                        }
                    }
                }
                //var deleteIconIndex = newIconList.FindIndex(ni => ni.IconAjaxLink?.Contains("load-confirm-delete") ?? false);
                //if (request.RequestStatusID != 3 && deleteIconIndex != -1)
                //{
                //    newIconList.RemoveAt(deleteIconIndex);
                //    newIconList.Insert(deleteIconIndex, placeholder);
                //}
                if (viewOnly)
                {
                    var popoverMoveList = newIconList.ElementAt(morePopoverIndex).IconPopovers.FindIndex(ni => ni.Description == AppUtility.PopoverDescription.MoveToList);
                    newIconList[morePopoverIndex].IconPopovers.RemoveAt(popoverMoveList);
                    var popoverDeleteFromList = newIconList.ElementAt(morePopoverIndex).IconPopovers.FindIndex(ni => ni.Description == AppUtility.PopoverDescription.DeleteFromList);
                    newIconList[morePopoverIndex].IconPopovers.RemoveAt(popoverDeleteFromList);
                }
            }
            return newIconList;
        }
        private static int GetOrderMethodEnum(string orderMethod)
        {
            var id = (int)Enum.Parse(typeof(AppUtility.OrderMethod), orderMethod);
            return id;
        }
        private String GetSharedBy(Request request, ShareRequest shareRequest)
        {
            try
            {
                var applicationUser = shareRequest.FromApplicationUser;
                return applicationUser.FirstName + " " + applicationUser.LastName;
            }
            catch (Exception ex)
            {
                return "No Person Specified";
            }
        }
        private List<StringWithBool> GetApplicationUserName()
        {
            try
            {
                return new List<StringWithBool>() { new StringWithBool { String = r.ApplicationUserCreator.FirstName + " " + r.ApplicationUserCreator.LastName, Bool = false } };
            }
            catch (Exception ex)
            {
                return new List<StringWithBool>() { new StringWithBool { String = "No Person Specified", Bool = true } };
            }
        }
        private StringWithBool GetLocationInstanceNameBefore()
        {
            try
            {
                var newLIName = "";
                if (r.RequestLocationInstances.FirstOrDefault().LocationInstance.LocationInstanceParentID == null)//is temporary location
                {
                    newLIName = r.RequestLocationInstances.FirstOrDefault().LocationInstance.LocationInstanceName;
                }
                else if (r.RequestLocationInstances.FirstOrDefault().LocationInstance.LocationInstanceParent.LocationTypeID == 500)
                {
                    newLIName = r.RequestLocationInstances.FirstOrDefault().LocationInstance.LocationInstanceName;
                }
                else
                {
                    newLIName = r.RequestLocationInstances.FirstOrDefault().LocationInstance.LocationInstanceParent.LocationInstanceName;
                }
                return new StringWithBool { String = newLIName };
            }
            catch (Exception ex)
            {
                return new StringWithBool { String = "location has an error", Bool = true };
            }

        }
        private string GetImageURL()
        {
            try
            {
                return r.Product.ProductSubcategory.ImageURL == null ? defaultImage : r.Product.ProductSubcategory.ImageURL;
            }
            catch (Exception ex)
            {
                return "image has an error";
            }
        }
        private List<StringWithBool> GetProductName()
        {
            try
            {
                return new List<StringWithBool>() { new StringWithBool { String = r.Product.ProductName, Bool = false } };
            }
            catch (Exception ex)
            {
                return new List<StringWithBool>() { new StringWithBool { String = "item name has an error", Bool = true } };
            }
        }
        private List<StringWithBool> GetVendorName()
        {
            try
            {
                if (r.OrderMethod.DescriptionEnum.ToString() == AppUtility.OrderMethod.Save.ToString())
                {
                    return new List<StringWithBool>() { new StringWithBool { String = "Centarix", Bool = false } };
                }
                return new List<StringWithBool>() { new StringWithBool { String = r.Product.Vendor.VendorEnName, Bool = false } };
            }
            catch (Exception ex)
            {
                return new List<StringWithBool>() { new StringWithBool { String = "vendor has an error", Bool = true } };
            }
        }
        private List<StringWithBool> GetPaymentDate()
        {
            try
            {
                return new List<StringWithBool>() { new StringWithBool { String = payments.FirstOrDefault().PaymentDate.GetElixirDateFormat() } };
            }
            catch (Exception ex)
            {
                return new List<StringWithBool>() { new StringWithBool { String = "payment date has an error", Bool = true } };
            }
        }
        /* private List<StringWithBool> GetInstallmentColumn()
        {
            try
            {
                return new List<StringWithBool>() { new StringWithBool { String = payments.FirstOrDefault().InstallmentNumber + " / " + r.Installments } };
            }
            catch (Exception ex)
            {
                return new List<StringWithBool>() { new StringWithBool { String = "installment number has an error", Bool = true } };
            }
        }*/
        private List<StringWithBool> GetStatusColumn()
        {
            var statusColumn = new List<StringWithBool>();
            try
            {
                if(r.RequestStatusID == 3)
                {
                    statusColumn.Add( new StringWithBool { String = "received" });
                } 
                else if(r.RequestStatusID == 2)
                {
                    statusColumn.Add(new StringWithBool { String = "ordered" });
                }

                if(r.Payments.Where(p=> p.HasInvoice == true).Count() >= 1)
                {
                    statusColumn.Add(new StringWithBool { String = "invoiced" });
                }
                else
                {
                    statusColumn.Add(new StringWithBool { String = "no invoice" });
                }
            }
            catch (Exception ex)
            {
                statusColumn.Add(new StringWithBool { String = "status has an error", Bool = true } );
            }
            return statusColumn;
        }
        private List<StringWithBool> GetOrderDetails()
        {
            var orderDetails = new List<StringWithBool>();
            try
            {
                orderDetails.Add(new StringWithBool { String = "# " + r.ParentRequest.OrderNumber });
            }
            catch (Exception ex)
            {
                orderDetails.Add( new StringWithBool { String = "order number has an error", Bool = true } );
            }
            try 
            { 
                orderDetails.Add( AppUtility.GetDateOrderedString(r.ParentRequest) );
            }
            catch (Exception ex)
            {
                orderDetails.Add(new StringWithBool { String = "order date has an error", Bool = true } );
            }
            return orderDetails;
        }
        private List<StringWithBool> GetOrderTypeColumn()
        {
            var orderTypeColumn =  new List<StringWithBool>();
            try
            {
                string orderType;
                if (r.PaymentStatusID == 7)
                {
                    orderType = "standing order";
                } 
                else if (r.Product is RecurringOrder) 
                {
                    orderType = "recurring order";
                } 
                else if (r.PaymentStatusID == 5)
                {
                    orderType = "installments";
                    try
                    {
                        orderTypeColumn.Add(new StringWithBool { String = payments.FirstOrDefault().InstallmentNumber + " / " + r.Installments });
                    }
                    catch (Exception ex)
                    {
                        orderTypeColumn.Add( new StringWithBool { String = "installment number has an error", Bool = true } );
                    }
                }
                else
                {
                    orderType = "single order";
                }
                orderTypeColumn.Add(new StringWithBool { String = orderType });
            }
            catch (Exception ex)
            {
                orderTypeColumn.Add( new StringWithBool { String = "order type has an error", Bool = true } );
            }
            return orderTypeColumn;
        }
        private List<StringWithBool> GetInvoiceColumn()
        {
            var invoiceColumn = new List<StringWithBool>();
            try
            {
                invoiceColumn.Add(new StringWithBool { String = payments.FirstOrDefault().Invoice.InvoiceNumber });
                invoiceColumn.Add(new StringWithBool { String = payments.FirstOrDefault().Invoice.InvoiceDate.GetElixirDateFormat() });
            }
            catch(Exception ex)
            {
                invoiceColumn.Add( new StringWithBool { String = "invoice column has an error", Bool= true } );
            }
            return invoiceColumn;
        }
        private List<StringWithBool> GetDateForFavoriteRequest()
        {
            if (r.OrderMethod.DescriptionEnum.ToString() == AppUtility.OrderMethod.Save.ToString())
            {
                return new List<StringWithBool>() { new StringWithBool { String = r.CreationDate.GetElixirDateFormat(), Bool = false } };
            }
            return new List<StringWithBool>() { new StringWithBool { String = r.ArrivalDate.GetElixirDateFormat(), Bool = false } };
        }
        private IEnumerable<RequestIndexPartialColumnViewModel> GetApproveColumns()
        {
            yield return new RequestIndexPartialColumnViewModel() { Title = "", Width = 10, Image = GetImageURL() };
            yield return new RequestIndexPartialColumnViewModel() { Title = "Item Name", Width = 15, ValueWithError = GetProductName(), AjaxLink = "load-product-details", AjaxID = r.RequestID, ShowTooltip = true };
            yield return new RequestIndexPartialColumnViewModel() { Title = "Vendor", Width = 10, ValueWithError = GetVendorName(), ShowTooltip = true };
            yield return new RequestIndexPartialColumnViewModel() { Title = "Amount", Width = 10, ValueWithError = AppUtility.GetAmountColumn(r) };
            yield return new RequestIndexPartialColumnViewModel()
            {
                Title = "Category",
                Width = 11,
                ValueWithError = AppUtility.GetCategoryColumn(requestIndexObject.CategorySelected, requestIndexObject.SubcategorySelected, r.Product),
                FilterEnum = AppUtility.FilterEnum.Category,
                ShowTooltip = true
            };
            yield return new RequestIndexPartialColumnViewModel() { Title = "Owner", Width = 12, ValueWithError = GetApplicationUserName() };
            yield return new RequestIndexPartialColumnViewModel() { Title = "Price", Width = 10, ValueWithError = AppUtility.GetPriceColumn(requestIndexObject.SelectedPriceSort, r, requestIndexObject.SelectedCurrency), FilterEnum = AppUtility.FilterEnum.Price };
            yield return new RequestIndexPartialColumnViewModel() { Title = "Date Created", Width = 12, ValueWithError = new List<StringWithBool>() { new StringWithBool { String = r.CreationDate.GetElixirDateFormat(), Bool = false } } };
            yield return new RequestIndexPartialColumnViewModel() { Title = "", Width = 10, Icons = GetIconsByIndividualRequest(r.RequestID, iconList, false, null, r, null), AjaxID = r.RequestID };
            yield return new RequestIndexPartialColumnViewModel() { Width = 0, AjaxLink = "p-0 d-none order-type" + r.RequestID, AjaxID = GetOrderMethodEnum(r.OrderMethod.DescriptionEnum.ToString()), ValueWithError = new List<StringWithBool>() { new StringWithBool { String = r.OrderMethod.DescriptionEnum.ToString().ToString(), Bool = false } } };
        }
        private IEnumerable<RequestIndexPartialColumnViewModel> GetOrderedColumns()
        {
            yield return new RequestIndexPartialColumnViewModel() { Title = "", Width = 10, Image = GetImageURL() };
            yield return new RequestIndexPartialColumnViewModel() { Title = "Item Name", Width = 15, ValueWithError = GetProductName(), AjaxLink = "load-product-details", AjaxID = r.RequestID, ShowTooltip = true };
            yield return new RequestIndexPartialColumnViewModel() { Title = "Vendor", Width = 10, ValueWithError = GetVendorName(), ShowTooltip = true };
            yield return new RequestIndexPartialColumnViewModel() { Title = "Amount", Width = 10, ValueWithError = AppUtility.GetAmountColumn(r) };
            yield return new RequestIndexPartialColumnViewModel()
            {
                Title = "Category",
                Width = 11,
                ValueWithError = AppUtility.GetCategoryColumn(requestIndexObject.CategorySelected, requestIndexObject.SubcategorySelected, r.Product),
                FilterEnum = AppUtility.FilterEnum.Category,
                ShowTooltip = true
            };
            yield return new RequestIndexPartialColumnViewModel() { Title = "Owner", Width = 12, ValueWithError = GetApplicationUserName() };
            yield return new RequestIndexPartialColumnViewModel() { Title = "Price", Width = 10, ValueWithError = AppUtility.GetPriceColumn(requestIndexObject.SelectedPriceSort, r, requestIndexObject.SelectedCurrency), FilterEnum = AppUtility.FilterEnum.Price };
            yield return new RequestIndexPartialColumnViewModel() { Title = "Date Ordered", Width = 12, ValueWithError = new List<StringWithBool>() { AppUtility.GetDateOrderedString(r.ParentRequest) } };
            yield return new RequestIndexPartialColumnViewModel()
            {
                Title = "",
                Width = 10,
                Icons = iconList,
                AjaxID = r.RequestID
            };
        }
        private IEnumerable<RequestIndexPartialColumnViewModel> GetReceivedInventoryFavoriteColumns()
        {
            yield return new RequestIndexPartialColumnViewModel() { Title = "", Width = 9, Image = GetImageURL() };
            yield return new RequestIndexPartialColumnViewModel() { Title = "Item Name", Width = 14, ValueWithError = GetProductName(), AjaxLink = "load-product-details", AjaxID = r.RequestID, ShowTooltip = true };
            yield return new RequestIndexPartialColumnViewModel() { Title = "Vendor", Width = 9, ValueWithError = GetVendorName(), ShowTooltip = true };
            yield return new RequestIndexPartialColumnViewModel() { Title = "Amount", Width = 9, ValueWithError = AppUtility.GetAmountColumn(r) };
            yield return new RequestIndexPartialColumnViewModel() { Title = "Location", Width = 9, ValueWithError = new List<StringWithBool>() { GetLocationInstanceNameBefore() } };
            yield return new RequestIndexPartialColumnViewModel() { Title = "Category", Width = 11, ValueWithError = AppUtility.GetCategoryColumn(requestIndexObject.CategorySelected, requestIndexObject.SubcategorySelected, r.Product), FilterEnum = AppUtility.FilterEnum.Category, ShowTooltip = true };
            yield return new RequestIndexPartialColumnViewModel() { Title = "Owner", Width = 10, ValueWithError = GetApplicationUserName() };
            yield return new RequestIndexPartialColumnViewModel() { Title = "Price", Width = 10, ValueWithError = AppUtility.GetPriceColumn(requestIndexObject.SelectedPriceSort, r, requestIndexObject.SelectedCurrency), FilterEnum = AppUtility.FilterEnum.Price };
            yield return new RequestIndexPartialColumnViewModel() { Title = "Arrival Date", Width = 10, ValueWithError = GetDateForFavoriteRequest() };
            yield return new RequestIndexPartialColumnViewModel()
            {
                Title = "",
                Width = 10,
                Icons = GetIconsByIndividualRequest(r.RequestID, iconList, false, favoriteRequest, null, user),
                AjaxID = r.RequestID
            };
        }
        private IEnumerable<RequestIndexPartialColumnViewModel> GetReceivedInventorySharedColumns()
        {
            yield return new RequestIndexPartialColumnViewModel() { Title = "", Width = 9, Image = GetImageURL() };
            yield return new RequestIndexPartialColumnViewModel() { Title = "Item Name", Width = 14, ValueWithError = GetProductName(), AjaxLink = "load-product-details", AjaxID = r.RequestID, ShowTooltip = true };
            yield return new RequestIndexPartialColumnViewModel() { Title = "Vendor", Width = 9, ValueWithError = GetVendorName(), ShowTooltip = true };
            yield return new RequestIndexPartialColumnViewModel() { Title = "Amount", Width = 9, ValueWithError = AppUtility.GetAmountColumn(r) };
            yield return new RequestIndexPartialColumnViewModel() { Title = "Location", Width = 9, ValueWithError = new List<StringWithBool>() { GetLocationInstanceNameBefore() } };
            yield return new RequestIndexPartialColumnViewModel() { Title = "Category", Width = 11, ValueWithError = AppUtility.GetCategoryColumn(requestIndexObject.CategorySelected, requestIndexObject.SubcategorySelected, r.Product), FilterEnum = AppUtility.FilterEnum.Category, ShowTooltip = true };
            yield return new RequestIndexPartialColumnViewModel() { Title = "Owner", Width = 10, ValueWithError = GetApplicationUserName() };
            yield return new RequestIndexPartialColumnViewModel() { Title = "Price", Width = 10, ValueWithError = AppUtility.GetPriceColumn(requestIndexObject.SelectedPriceSort, r, requestIndexObject.SelectedCurrency), FilterEnum = AppUtility.FilterEnum.Price };
            yield return new RequestIndexPartialColumnViewModel() { Title = "Shared By", Width = 10, ValueWithError = new List<StringWithBool>() { new StringWithBool { String = GetSharedBy(r, shareRequest), Bool = false } } };
            yield return new RequestIndexPartialColumnViewModel()
            {
                Title = "",
                Width = 10,
                Icons = GetIconsByIndividualRequest(r.RequestID, iconList, false, request: r),
                AjaxID = r.RequestID
            };
        }
        private IEnumerable<RequestIndexPartialColumnViewModel> GetReceivedInventoryColumns()
        {
            yield return new RequestIndexPartialColumnViewModel() { Title = "", Width = 9, Image = GetImageURL() };
            yield return new RequestIndexPartialColumnViewModel() { Title = "Item Name", Width = 14, ValueWithError = GetProductName(), AjaxLink = "load-product-details", AjaxID = r.RequestID, ShowTooltip = true };
            yield return new RequestIndexPartialColumnViewModel() { Title = "Vendor", Width = 9, ValueWithError = GetVendorName(), ShowTooltip = true };
            yield return new RequestIndexPartialColumnViewModel() { Title = "Amount", Width = 9, ValueWithError = AppUtility.GetAmountColumn(r) };
            yield return new RequestIndexPartialColumnViewModel() { Title = "Location", Width = 9, ValueWithError = new List<StringWithBool>() { GetLocationInstanceNameBefore() } };
            yield return new RequestIndexPartialColumnViewModel() { Title = "Category", Width = 11, ValueWithError = AppUtility.GetCategoryColumn(requestIndexObject.CategorySelected, requestIndexObject.SubcategorySelected, r.Product), FilterEnum = AppUtility.FilterEnum.Category, ShowTooltip = true };
            yield return new RequestIndexPartialColumnViewModel() { Title = "Owner", Width = 9, ValueWithError = GetApplicationUserName() };
            yield return new RequestIndexPartialColumnViewModel() { Title = "Price", Width = 10, ValueWithError = AppUtility.GetPriceColumn(requestIndexObject.SelectedPriceSort, r, requestIndexObject.SelectedCurrency), FilterEnum = AppUtility.FilterEnum.Price };
            yield return new RequestIndexPartialColumnViewModel() { Title = "Arrival Date", Width = 10, ValueWithError = new List<StringWithBool>() { new StringWithBool { String = r.ArrivalDate.GetElixirDateFormat(), Bool = false } } };
            yield return new RequestIndexPartialColumnViewModel()
            {
                Title = "",
                Width = 10,
                Icons = GetIconsByIndividualRequest(r.RequestID, iconList, false, favoriteRequest, request: r, user),
                AjaxID = r.RequestID
            };
        }

        private IEnumerable<RequestIndexPartialColumnViewModel> GetSummaryColumns()
        {
            yield return new RequestIndexPartialColumnViewModel() { Title = "", Width = 9, Image = GetImageURL() };
            yield return new RequestIndexPartialColumnViewModel() { Title = "Item Name", Width = 14, ValueWithError = GetProductName(), AjaxLink = "load-product-details", AjaxID = r.RequestID, ShowTooltip = true };
            yield return new RequestIndexPartialColumnViewModel() { Title = "Vendor", Width = 9, ValueWithError = GetVendorName(), ShowTooltip = true };
            yield return new RequestIndexPartialColumnViewModel() { Title = "Amount", Width = 9, ValueWithError = AppUtility.GetAmountColumn(r) };
            yield return new RequestIndexPartialColumnViewModel() { Title = "Location", Width = 9, ValueWithError = new List<StringWithBool>() { GetLocationInstanceNameBefore() } };
            yield return new RequestIndexPartialColumnViewModel() { Title = "Category", Width = 11, ValueWithError = AppUtility.GetCategoryColumn(requestIndexObject.CategorySelected, requestIndexObject.SubcategorySelected, r.Product), FilterEnum = AppUtility.FilterEnum.Category, ShowTooltip = true };
            yield return new RequestIndexPartialColumnViewModel() { Title = "Owner", Width = 9, ValueWithError = GetApplicationUserName() };
            yield return new RequestIndexPartialColumnViewModel() { Title = "Price", Width = 10, ValueWithError = AppUtility.GetPriceColumn(requestIndexObject.SelectedPriceSort, r, requestIndexObject.SelectedCurrency), FilterEnum = AppUtility.FilterEnum.Price };
            yield return new RequestIndexPartialColumnViewModel() { Title = "Date Ordered", Width = 10, ValueWithError = new List<StringWithBool>() { AppUtility.GetDateOrderedString(r.ParentRequest) } };
            yield return new RequestIndexPartialColumnViewModel()
            {
                Title = "",
                Width = 10,
                Icons = GetIconsByIndividualRequest(r.RequestID, iconList, false, favoriteRequest, r, user),
                AjaxID = r.RequestID
            };
        }

        private IEnumerable<RequestIndexPartialColumnViewModel> GetAccountingGeneralColumns()
        {
            yield return new RequestIndexPartialColumnViewModel() { Title = "", Width = 6, Image = GetImageURL() };
            yield return new RequestIndexPartialColumnViewModel() { Title = "Item Name", Width = 15, ValueWithError = GetProductName(), AjaxLink = "load-product-details-summary", AjaxID = r.RequestID, ShowTooltip = true };
            yield return new RequestIndexPartialColumnViewModel() { Title = "Category", Width = 11, ValueWithError = AppUtility.GetCategoryColumn(requestIndexObject.CategorySelected, requestIndexObject.SubcategorySelected, r.Product, requestIndexObject.SourceSelected), FilterEnum = AppUtility.FilterEnum.Category, ShowTooltip = true };
            yield return new RequestIndexPartialColumnViewModel() { Title = "Order Type", Width = 10, ValueWithError = GetOrderTypeColumn() };
            yield return new RequestIndexPartialColumnViewModel() { Title = "Status", Width = 9, ValueWithError = GetStatusColumn() };
            yield return new RequestIndexPartialColumnViewModel() { Title = "Invoice Details", Width = 9, ValueWithError = GetInvoiceColumn() };
            yield return new RequestIndexPartialColumnViewModel() { Title = "Order Details", Width = 10, ValueWithError = GetOrderDetails() };
            yield return new RequestIndexPartialColumnViewModel() { Title = "Units", Width = 8, ValueWithError = AppUtility.GetAmountColumn(r) };
            yield return new RequestIndexPartialColumnViewModel() { Title = "Price", Width = 9, ValueWithError = AppUtility.GetPriceColumn(requestIndexObject.SelectedPriceSort, r, requestIndexObject.SelectedCurrency), FilterEnum = AppUtility.FilterEnum.Price };
        }
        private IEnumerable<RequestIndexPartialColumnViewModel> GetSummaryProprietaryColumns()
        {
            yield return new RequestIndexPartialColumnViewModel() { Title = "", Width = 9, Image = GetImageURL() };
            yield return new RequestIndexPartialColumnViewModel() { Title = "Item Name", Width = 14, ValueWithError = GetProductName(), AjaxLink = "load-product-details-summary", AjaxID = r.RequestID, ShowTooltip = true };
            yield return new RequestIndexPartialColumnViewModel() { Title = "Amount", Width = 9, ValueWithError = AppUtility.GetAmountColumn(r) };
            yield return new RequestIndexPartialColumnViewModel() { Title = "Location", Width = 9, ValueWithError = new List<StringWithBool>() { GetLocationInstanceNameBefore() } };
            yield return new RequestIndexPartialColumnViewModel() { Title = "Category", Width = 11, ValueWithError = AppUtility.GetCategoryColumn(requestIndexObject.CategorySelected, requestIndexObject.SubcategorySelected, r.Product), FilterEnum = AppUtility.FilterEnum.Category, ShowTooltip = true };
            yield return new RequestIndexPartialColumnViewModel() { Title = "Owner", Width = 10, ValueWithError = GetApplicationUserName() };
            yield return new RequestIndexPartialColumnViewModel() { Title = "Date Created", Width = 10, ValueWithError = new List<StringWithBool>() { new StringWithBool { String = r.CreationDate.GetElixirDateFormat(), Bool = false } } };
            yield return new RequestIndexPartialColumnViewModel()
            {
                Title = "",
                Width = 10,
                Icons = GetIconsByIndividualRequest(r.RequestID, iconList, false, favoriteRequest, null, user),
                AjaxID = r.RequestID
            };
        }
        private IEnumerable<RequestIndexPartialColumnViewModel> GetReceivedInventoryOperationsColumns()
        {
            yield return new RequestIndexPartialColumnViewModel() { Title = "", Width = 9, Image = GetImageURL() };
            yield return new RequestIndexPartialColumnViewModel() { Title = "Item Name", Width = 14, ValueWithError = GetProductName(), AjaxLink = "load-product-details", AjaxID = r.RequestID, ShowTooltip = true };
            yield return new RequestIndexPartialColumnViewModel() { Title = "Vendor", Width = 9, ValueWithError = GetVendorName(), ShowTooltip = true };
            yield return new RequestIndexPartialColumnViewModel() { Title = "Category", Width = 11, ValueWithError = AppUtility.GetCategoryColumn(requestIndexObject.CategorySelected, requestIndexObject.SubcategorySelected, r.Product), FilterEnum = AppUtility.FilterEnum.Category, ShowTooltip = true };
            yield return new RequestIndexPartialColumnViewModel() { Title = "Owner", Width = 10, ValueWithError = GetApplicationUserName() };
            yield return new RequestIndexPartialColumnViewModel() { Title = "Price", Width = 10, ValueWithError = AppUtility.GetPriceColumn(requestIndexObject.SelectedPriceSort, r, requestIndexObject.SelectedCurrency), FilterEnum = AppUtility.FilterEnum.Price };
            yield return new RequestIndexPartialColumnViewModel() { Title = "Date Ordered", Width = 10, ValueWithError = new List<StringWithBool>() { AppUtility.GetDateOrderedString(r.ParentRequest) } };
            yield return new RequestIndexPartialColumnViewModel()
            {
                Title = "",
                Width = 10,
                Icons = iconList,
                AjaxID = r.RequestID
            };
        }

        private IEnumerable<RequestIndexPartialColumnViewModel> GetOrderedOperationsColumns()
        {
            yield return new RequestIndexPartialColumnViewModel() { Title = "", Width = 10, Image = GetImageURL() };
            yield return new RequestIndexPartialColumnViewModel() { Title = "Item Name", Width = 15, ValueWithError = GetProductName(), AjaxLink = "load-product-details", AjaxID = r.RequestID, ShowTooltip = true };
            yield return new RequestIndexPartialColumnViewModel() { Title = "Vendor", Width = 10, ValueWithError = GetVendorName(), ShowTooltip = true };
            yield return new RequestIndexPartialColumnViewModel() { Title = "Category", Width = 11, ValueWithError = AppUtility.GetCategoryColumn(requestIndexObject.CategorySelected, requestIndexObject.SubcategorySelected, r.Product), FilterEnum = AppUtility.FilterEnum.Category, ShowTooltip = true };
            yield return new RequestIndexPartialColumnViewModel() { Title = "Owner", Width = 12, ValueWithError = GetApplicationUserName() };
            yield return new RequestIndexPartialColumnViewModel() { Title = "Price", Width = 10, ValueWithError = AppUtility.GetPriceColumn(requestIndexObject.SelectedPriceSort, r, requestIndexObject.SelectedCurrency), FilterEnum = AppUtility.FilterEnum.Price };
            yield return new RequestIndexPartialColumnViewModel() { Title = "Date Ordered", Width = 12, ValueWithError = new List<StringWithBool>() { AppUtility.GetDateOrderedString(r.ParentRequest) } };
            yield return new RequestIndexPartialColumnViewModel()
            {
                Title = "",
                Width = 10,
                Icons = iconList,
                AjaxID = r.RequestID
            };
        }
        private IEnumerable<RequestIndexPartialColumnViewModel> GetCartColumns()
        {
            yield return new RequestIndexPartialColumnViewModel()
            {
                Title = "",
                Width = 5,
                ValueWithError = new List<StringWithBool>() { new StringWithBool { String = checkboxString, Bool = false } },
                AjaxID = r.RequestID,
                AjaxLink = currentCurrency + r.Currency + " "
            };
            yield return new RequestIndexPartialColumnViewModel() { Title = "", Width = 10, Image = GetImageURL() };
            yield return new RequestIndexPartialColumnViewModel() { Title = "Item Name", Width = 15, ValueWithError = GetProductName(), AjaxLink = "load-product-details", AjaxID = r.RequestID, ShowTooltip = true };
            yield return new RequestIndexPartialColumnViewModel() { Title = "Vendor", Width = 10, ValueWithError = GetVendorName(), ShowTooltip = true };
            yield return new RequestIndexPartialColumnViewModel() { Title = "Category", Width = 11, ValueWithError = AppUtility.GetCategoryColumn(requestIndexObject.CategorySelected, requestIndexObject.SubcategorySelected, r.Product), FilterEnum = AppUtility.FilterEnum.Category, ShowTooltip = true };
            yield return new RequestIndexPartialColumnViewModel() { Title = "Amount", Width = 10, ValueWithError = AppUtility.GetAmountColumn(r) };
            yield return new RequestIndexPartialColumnViewModel() { Title = "Price", Width = 10, ValueWithError = AppUtility.GetPriceColumn(requestIndexObject.SelectedPriceSort, r, requestIndexObject.SelectedCurrency), FilterEnum = AppUtility.FilterEnum.Price };
            yield return new RequestIndexPartialColumnViewModel()
            {
                Title = "",
                Width = 10,
                Icons = GetIconsByIndividualRequest(r.RequestID, iconList, false, favoriteRequest, request: r, user),
                AjaxID = r.RequestID,
            };

        }
        private IEnumerable<RequestIndexPartialColumnViewModel> GetCartOperationsColumns()
        {
            yield return new RequestIndexPartialColumnViewModel()
            {
                Title = "",
                Width = 5,
                ValueWithError = new List<StringWithBool>() { new StringWithBool { String = checkboxString, Bool = false } },
                AjaxID = r.RequestID,
                AjaxLink = currentCurrency + r.Currency + " "
            };
            yield return new RequestIndexPartialColumnViewModel() { Title = "", Width = 10, Image = GetImageURL() };
            yield return new RequestIndexPartialColumnViewModel() { Title = "Item Name", Width = 15, ValueWithError = GetProductName(), AjaxLink = "load-product-details", AjaxID = r.RequestID, ShowTooltip = true };
            yield return new RequestIndexPartialColumnViewModel() { Title = "Vendor", Width = 10, ValueWithError = GetVendorName(), ShowTooltip = true };
            yield return new RequestIndexPartialColumnViewModel() { Title = "Units", Width = 10, ValueWithError = AppUtility.GetAmountColumn(r) };
            yield return new RequestIndexPartialColumnViewModel() { Title = "Category", Width = 11, ValueWithError = AppUtility.GetCategoryColumn(requestIndexObject.CategorySelected, requestIndexObject.SubcategorySelected, r.Product), FilterEnum = AppUtility.FilterEnum.Category, ShowTooltip = true };
            yield return new RequestIndexPartialColumnViewModel() { Title = "Date Added", Width = 11, ValueWithError = new List<StringWithBool>() { new StringWithBool { String = r.CreationDate.GetElixirDateFormat(), Bool = false } } };
            yield return new RequestIndexPartialColumnViewModel() { Title = "Price", Width = 10, ValueWithError = AppUtility.GetPriceColumn(requestIndexObject.SelectedPriceSort, r, requestIndexObject.SelectedCurrency), FilterEnum = AppUtility.FilterEnum.Price };
            yield return new RequestIndexPartialColumnViewModel()
            {
                Title = "",
                Width = 10,
                Icons = GetIconsByIndividualRequest(r.RequestID, iconList, false, favoriteRequest, request: r, user),
                AjaxID = r.RequestID,
            };

        }
        private IEnumerable<RequestIndexPartialColumnViewModel> GetRecurringExpensesOperationsColumns()
        {
            yield return new RequestIndexPartialColumnViewModel() { Title = "", Width = 10, Image = GetImageURL() };
            yield return new RequestIndexPartialColumnViewModel() { Title = "Item Name", Width = 15, ValueWithError = GetProductName(), AjaxLink = "load-product-details", AjaxID = r.RequestID, ShowTooltip = true };
            yield return new RequestIndexPartialColumnViewModel() { Title = "Amount", Width = 10, ValueWithError = AppUtility.GetTimePeriodColumn(r, timePeriod) };
            yield return new RequestIndexPartialColumnViewModel() { Title = "Vendor", Width = 10, ValueWithError = GetVendorName(), ShowTooltip = true };
            yield return new RequestIndexPartialColumnViewModel() { Title = "Units", Width = 10, ValueWithError = AppUtility.GetAmountColumn(r, true) };
            yield return new RequestIndexPartialColumnViewModel() { Title = "Amount", Width = 10, ValueWithError = AppUtility.GetTimePeriodTypeColumn(timePeriod) };
            yield return new RequestIndexPartialColumnViewModel() { Title = "Category", Width = 11, ValueWithError = AppUtility.GetCategoryColumn(requestIndexObject.CategorySelected, requestIndexObject.SubcategorySelected, r.Product), FilterEnum = AppUtility.FilterEnum.Category, ShowTooltip = true };
            yield return new RequestIndexPartialColumnViewModel() { Title = "Owner", Width = 12, ValueWithError = GetApplicationUserName() };
            yield return new RequestIndexPartialColumnViewModel() { Title = "Price", Width = 10, ValueWithError = AppUtility.GetPriceColumn(requestIndexObject.SelectedPriceSort, r, requestIndexObject.SelectedCurrency, true), FilterEnum = AppUtility.FilterEnum.Price };
            yield return new RequestIndexPartialColumnViewModel() { Title = "Order Added", Width = 12, ValueWithError = new List<StringWithBool>() { AppUtility.GetDateOrderedString(r.ParentRequest) } };
            yield return new RequestIndexPartialColumnViewModel()
            {
                Title = "",
                Width = 10,
                Icons = iconList,
                AjaxID = r.RequestID
            };
        }
        private IEnumerable<RequestIndexPartialColumnViewModel> GetAccountingNotificationsColumns()
        {
            yield return new RequestIndexPartialColumnViewModel() { Title = "", Width = 3, ValueWithError = new List<StringWithBool>() { new StringWithBool { String = checkboxString, Bool = false } }, AjaxID = r.RequestID, AjaxLink = currentCurrency + r.Currency + " " };
            yield return new RequestIndexPartialColumnViewModel() { Title = "", Width = 6, Image = GetImageURL() };
            yield return new RequestIndexPartialColumnViewModel() { Title = "Item Name", Width = 15, ValueWithError = GetProductName(), AjaxLink = "load-product-details-summary", AjaxID = r.RequestID, ShowTooltip = true };
            yield return new RequestIndexPartialColumnViewModel() { Title = "Category", Width = 11, ValueWithError = AppUtility.GetCategoryColumn(requestIndexObject.CategorySelected, requestIndexObject.SubcategorySelected, r.Product, requestIndexObject.SourceSelected), FilterEnum = AppUtility.FilterEnum.Category, ShowTooltip = true };
            yield return new RequestIndexPartialColumnViewModel() { Title = "Units", Width = 8, ValueWithError = AppUtility.GetAmountColumn(r) };
            yield return new RequestIndexPartialColumnViewModel() { Title = "Order Type", Width = 10, ValueWithError = GetOrderTypeColumn() };
            yield return new RequestIndexPartialColumnViewModel() { Title = "Price", Width = 9, ValueWithError = AppUtility.GetPriceColumn(requestIndexObject.SelectedPriceSort, r, requestIndexObject.SelectedCurrency), FilterEnum = AppUtility.FilterEnum.Price };
            yield return new RequestIndexPartialColumnViewModel() { Title = "Status", Width = 9, ValueWithError = GetStatusColumn() };
            yield return new RequestIndexPartialColumnViewModel() { Title = "Order Details", Width = 10, ValueWithError = GetOrderDetails() };
            yield return new RequestIndexPartialColumnViewModel()
            {
                Title = "",
                Width = 10,
                Icons = iconList,
                AjaxID = r.RequestID,
                Note = AppUtility.GetNote(requestIndexObject.SidebarType, r)
            };
        }
        private IEnumerable<RequestIndexPartialColumnViewModel> GetAccountingPaymentsDefaultColumns()
        {
            yield return new RequestIndexPartialColumnViewModel() { Title = "", Width = 3, ValueWithError = new List<StringWithBool>() { new StringWithBool { String = checkboxString, Bool = false } }, AjaxID = payments.FirstOrDefault().PaymentID, AjaxLink=currentCurrency+r.Currency+" " };
            yield return new RequestIndexPartialColumnViewModel() { Title = "", Width = 6, Image = GetImageURL() };
            yield return new RequestIndexPartialColumnViewModel() { Title = "Item Name", Width = 15, ValueWithError = GetProductName(), AjaxLink = "load-product-details-summary", AjaxID = r.RequestID, ShowTooltip = true };
            yield return new RequestIndexPartialColumnViewModel() { Title = "Category", Width = 11, ValueWithError = AppUtility.GetCategoryColumn(requestIndexObject.CategorySelected, requestIndexObject.SubcategorySelected, r.Product, requestIndexObject.SourceSelected), FilterEnum = AppUtility.FilterEnum.Category, ShowTooltip = true };
            yield return new RequestIndexPartialColumnViewModel() { Title = "Units", Width = 8, ValueWithError = AppUtility.GetAmountColumn(r) };
            yield return new RequestIndexPartialColumnViewModel() { Title = "Order Type", Width = 10, ValueWithError = GetOrderTypeColumn()};
            yield return new RequestIndexPartialColumnViewModel() { Title = "Price", Width = 9, ValueWithError = AppUtility.GetPriceColumn(requestIndexObject.SelectedPriceSort, r, requestIndexObject.SelectedCurrency), FilterEnum = AppUtility.FilterEnum.Price };
            yield return new RequestIndexPartialColumnViewModel() { Title = "Status", Width = 9, ValueWithError = GetStatusColumn() };
            yield return new RequestIndexPartialColumnViewModel() { Title = "Invoice Details", Width = 9, ValueWithError = GetInvoiceColumn() };
            yield return new RequestIndexPartialColumnViewModel() { Title = "Order Details", Width = 10, ValueWithError = GetOrderDetails() };
            yield return new RequestIndexPartialColumnViewModel()
            {
                Title = "",
                Width = 10,
                Icons = iconList,
                //AjaxID = r.RequestID
                AjaxID = payments.FirstOrDefault().PaymentID

            };
        }
        private IEnumerable<RequestIndexPartialColumnViewModel> GetAccountingPaymentsInstallmentsColumns()
        {
            yield return new RequestIndexPartialColumnViewModel() { Title = "", Width = 5, ValueWithError = new List<StringWithBool>() { new StringWithBool { String = checkboxString, Bool = false } }, AjaxID = payments.FirstOrDefault().PaymentID };
            yield return new RequestIndexPartialColumnViewModel() { Title = "", Width = 10, Image = GetImageURL() };
            yield return new RequestIndexPartialColumnViewModel()
            {
                Title = "Item Name",
                Width = 15,
                ValueWithError = GetProductName(),
                AjaxLink = "load-product-details-summary",
                AjaxID = r.RequestID,
                ShowTooltip = true
            };
            yield return new RequestIndexPartialColumnViewModel()
            {
                Title = "Category",
                Width = 11,
                ValueWithError = AppUtility.GetCategoryColumn(requestIndexObject.CategorySelected, requestIndexObject.SubcategorySelected,
                                                                    r.Product),
                FilterEnum = AppUtility.FilterEnum.Category,
                ShowTooltip = true
            };
            yield return new RequestIndexPartialColumnViewModel() { Title = "Amount", Width = 10, ValueWithError = AppUtility.GetAmountColumn(r) };
            yield return new RequestIndexPartialColumnViewModel()
            {
                Title = "Price",
                Width = 10,
                ValueWithError = AppUtility.GetPriceColumn(requestIndexObject.SelectedPriceSort, r,
                                                                    requestIndexObject.SelectedCurrency),
                FilterEnum = AppUtility.FilterEnum.Price
            };
            yield return new RequestIndexPartialColumnViewModel() { Title = "Payment Date", Width = 12, ValueWithError = GetPaymentDate() };
            yield return new RequestIndexPartialColumnViewModel()
            {
                Title = "",
                Width = 10,
                Icons = iconList,
                AjaxID = payments.FirstOrDefault().PaymentID
                //AjaxID = r.RequestID
            };
        }

        private IEnumerable<RequestIndexPartialColumnViewModel> GetLabQuotesColumns()
        {
            yield return new RequestIndexPartialColumnViewModel() { Title = "", Width = 3, ValueWithError = new List<StringWithBool>() { new StringWithBool { String = checkboxString } }, AjaxID = r.RequestID };
            yield return new RequestIndexPartialColumnViewModel() { Title = "", Width = 5, Image = GetImageURL() };
            yield return new RequestIndexPartialColumnViewModel() { Title = "Item Name", Width = 15, ValueWithError = GetProductName(), AjaxLink = "load-product-details", AjaxID = r.RequestID, ShowTooltip = true };
            //yield return new RequestIndexPartialColumnViewModel() { Title = "Vendor", Width = 12, Value = GetVendorName(), ShowTooltip = true };
            yield return new RequestIndexPartialColumnViewModel() { Title = "Category", Width = 10, ValueWithError = AppUtility.GetCategoryColumn(requestIndexObject.CategorySelected, requestIndexObject.SubcategorySelected, r.Product), FilterEnum = AppUtility.FilterEnum.Category, ShowTooltip = true };
            yield return new RequestIndexPartialColumnViewModel() { Title = "Amount", Width = 8, ValueWithError = AppUtility.GetAmountColumn(r) };
            yield return new RequestIndexPartialColumnViewModel() { Title = "Price", Width = 9, ValueWithError = AppUtility.GetPriceColumn(requestIndexObject.SelectedPriceSort, r, requestIndexObject.SelectedCurrency), FilterEnum = AppUtility.FilterEnum.Price };
            yield return new RequestIndexPartialColumnViewModel() { Title = "Owner", Width = 8, ValueWithError = GetApplicationUserName() };
            yield return new RequestIndexPartialColumnViewModel()
            {
                Title = "",
                Width = 15,
                Icons = GetIconsByIndividualRequest(r.RequestID, iconList, true, null, r),
                AjaxID = r.RequestID
            };
        }
        private IEnumerable<RequestIndexPartialColumnViewModel> GetLabOrdersColumns()
        {
            yield return new RequestIndexPartialColumnViewModel() { Title = "", Width = 5, ValueWithError = new List<StringWithBool>() { new StringWithBool { String = checkboxString } }, AjaxID = r.RequestID, AjaxLink = currentCurrency + r.Currency + " " };
            yield return new RequestIndexPartialColumnViewModel() { Title = "", Width = 7, Image = GetImageURL() };
            yield return new RequestIndexPartialColumnViewModel() { Title = "Item Name", Width = 15, ValueWithError = GetProductName(), AjaxLink = "load-product-details", AjaxID = r.RequestID, ShowTooltip = true };
            //yield return new RequestIndexPartialColumnViewModel() { Title = "Vendor", Width = 10, Value = GetVendorName(), ShowTooltip = true };
            yield return new RequestIndexPartialColumnViewModel() { Title = "Category", Width = 11, ValueWithError = AppUtility.GetCategoryColumn(requestIndexObject.CategorySelected, requestIndexObject.SubcategorySelected, r.Product), FilterEnum = AppUtility.FilterEnum.Category, ShowTooltip = true };
            yield return new RequestIndexPartialColumnViewModel() { Title = "Amount", Width = 10, ValueWithError = AppUtility.GetAmountColumn(r) };
            yield return new RequestIndexPartialColumnViewModel() { Title = "Price", Width = 10, ValueWithError = AppUtility.GetPriceColumn(requestIndexObject.SelectedPriceSort, r, requestIndexObject.SelectedCurrency), FilterEnum = AppUtility.FilterEnum.Price };
            yield return new RequestIndexPartialColumnViewModel() { Title = "Owner", Width = 12, ValueWithError = GetApplicationUserName() };
            yield return new RequestIndexPartialColumnViewModel()
            {
                Title = "",
                Width = 10,
                Icons = GetIconsByIndividualRequest(r.RequestID, iconList, false),
                AjaxID = r.RequestID
            };

        }
        private IEnumerable<RequestIndexPartialColumnViewModel> GetRequestListColumns()
        {
            yield return new RequestIndexPartialColumnViewModel() { Title = "", Width = 7, Image = GetImageURL() };
            yield return new RequestIndexPartialColumnViewModel() { Title = "Item Name", Width = 15, ValueWithError = GetProductName(), AjaxLink = "load-product-details", AjaxID = r.RequestID, ShowTooltip = true };
            yield return new RequestIndexPartialColumnViewModel() { Title = "Vendor", Width = 10, ValueWithError = GetVendorName(), ShowTooltip = true };
            yield return new RequestIndexPartialColumnViewModel() { Title = "Location", Width = 9, ValueWithError = new List<StringWithBool>() { GetLocationInstanceNameBefore() } };
            yield return new RequestIndexPartialColumnViewModel() { Title = "Category", Width = 11, ValueWithError = AppUtility.GetCategoryColumn(requestIndexObject.CategorySelected, requestIndexObject.SubcategorySelected, r.Product), FilterEnum = AppUtility.FilterEnum.Category, ShowTooltip = true };
            yield return new RequestIndexPartialColumnViewModel() { Title = "Amount", Width = 10, ValueWithError = AppUtility.GetAmountColumn(r) };
            yield return new RequestIndexPartialColumnViewModel() { Title = "Price", Width = 10, ValueWithError = AppUtility.GetPriceColumn(requestIndexObject.SelectedPriceSort, r, requestIndexObject.SelectedCurrency), FilterEnum = AppUtility.FilterEnum.Price };
            yield return new RequestIndexPartialColumnViewModel() { Title = "Date Ordered", Width = 12, ValueWithError = new List<StringWithBool>() { AppUtility.GetDateOrderedString(r.ParentRequest) } };
            yield return new RequestIndexPartialColumnViewModel()
            {
                Title = "",
                Width = 10,
                Icons = GetIconsByIndividualRequest(r.RequestID, iconList, false, favoriteRequest, r, null, viewOnly),
                AjaxID = r.RequestID
            };

        }
    }
}
