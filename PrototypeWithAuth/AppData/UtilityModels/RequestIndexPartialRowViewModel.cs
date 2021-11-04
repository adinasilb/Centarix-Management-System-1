﻿using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Threading.Tasks;
using PrototypeWithAuth.AppData;
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
        public Request partialRequest;

        public RequestIndexPartialRowViewModel() { }
        public RequestIndexPartialRowViewModel(AppUtility.IndexTableTypes indexTableTypes, Request request, Product product, Vendor vendor, ProductSubcategory productSubcategory, 
            ParentCategory parentCategory, UnitType unitType, UnitType subUnitType, UnitType subSubUnitType, RequestIndexObject requestIndexObject, List<IconColumnViewModel> iconList, 
            string defaultImage)
            : this(indexTableTypes, request, product, vendor, productSubcategory, parentCategory, unitType, subUnitType, subSubUnitType, requestIndexObject, iconList, defaultImage, 
                  parentRequest: null)
        { 
        }
        public RequestIndexPartialRowViewModel(AppUtility.IndexTableTypes indexTableTypes, Request request, Product product, Vendor vendor, ProductSubcategory productSubcategory, 
            ParentCategory parentCategory, UnitType unitType, UnitType subUnitType, UnitType subSubUnitType, RequestIndexObject requestIndexObject, List<IconColumnViewModel> iconList, 
            string defaultImage, ParentRequest parentRequest)
            : this(indexTableTypes, request, product, vendor, productSubcategory, parentCategory, unitType, subUnitType, subSubUnitType, requestIndexObject, iconList, defaultImage, 
                favoriteRequest: null, null, parentRequest)
        {
        }
        public RequestIndexPartialRowViewModel(AppUtility.IndexTableTypes indexTableTypes, Request request, Product product, Vendor vendor, ProductSubcategory productSubcategory, 
            ParentCategory parentCategory, UnitType unitType, UnitType subUnitType, UnitType subSubUnitType, RequestIndexObject requestIndexObject, List<IconColumnViewModel> iconList, 
            string defaultImage, string checkboxString)
            : this(indexTableTypes, request, product, vendor, productSubcategory, parentCategory, unitType, subUnitType, subSubUnitType, requestIndexObject, iconList, defaultImage, 
                null, checkboxString)
        {
        }
        public RequestIndexPartialRowViewModel(AppUtility.IndexTableTypes indexTableTypes, Request request, Product product, Vendor vendor, ProductSubcategory productSubcategory, 
            ParentCategory parentCategory, UnitType unitType, UnitType subUnitType, UnitType subSubUnitType, RequestIndexObject requestIndexObject, List<IconColumnViewModel> iconList, 
            string defaultImage, ParentRequest parentRequest, ApplicationUser user)
            : this(indexTableTypes, request, product, vendor, productSubcategory, parentCategory, unitType, subUnitType, subSubUnitType, requestIndexObject, iconList, defaultImage, 
                null, user,  parentRequest)
        {
        }
        public RequestIndexPartialRowViewModel(AppUtility.IndexTableTypes indexTableTypes, Request request, Product product, Vendor vendor, ProductSubcategory productSubcategory, 
            ParentCategory parentCategory, UnitType unitType, UnitType subUnitType, UnitType subSubUnitType, RequestIndexObject requestIndexObject, List<IconColumnViewModel> iconList, 
            string defaultImage, ParentRequest parentRequest, String checkboxString, Request partialRequest)
            : this(indexTableTypes, request, product, vendor, productSubcategory, parentCategory, unitType, subUnitType, subSubUnitType, requestIndexObject, iconList, defaultImage, null, null, null,null,null,
                parentRequest, checkboxString, null, null, partialRequest)
        {
        }
        public RequestIndexPartialRowViewModel(AppUtility.IndexTableTypes indexTableTypes, Request request, Product product, Vendor vendor, ProductSubcategory productSubcategory,
          ParentCategory parentCategory, UnitType unitType, UnitType subUnitType, UnitType subSubUnitType, RequestIndexObject requestIndexObject, List<IconColumnViewModel> iconList,
          string defaultImage, ParentRequest parentRequest, String checkboxString)
          : this(indexTableTypes, request, product, vendor, productSubcategory, parentCategory, unitType, subUnitType, subSubUnitType, requestIndexObject, iconList, defaultImage,
              parentRequest, checkboxString, new List<Payment>())
        {
        }
        public RequestIndexPartialRowViewModel(AppUtility.IndexTableTypes indexTableTypes, Request request, Product product, Vendor vendor, ProductSubcategory productSubcategory, 
            ParentCategory parentCategory, UnitType unitType, UnitType subUnitType, UnitType subSubUnitType, RequestIndexObject requestIndexObject, List<IconColumnViewModel> iconList, 
            string defaultImage, ParentRequest parentRequest, String checkboxString, List<Payment> payments)
            : this(indexTableTypes, request, product, vendor, productSubcategory, parentCategory, unitType, subUnitType, subSubUnitType, requestIndexObject, iconList, defaultImage, 
                null, null, null, null, null, parentRequest, checkboxString, null, payments, null)
        {
        }
        public RequestIndexPartialRowViewModel(AppUtility.IndexTableTypes indexTableTypes, Request request, Product product, Vendor vendor, ProductSubcategory productSubcategory, 
            ParentCategory parentCategory, UnitType unitType, UnitType subUnitType, UnitType subSubUnitType, RequestIndexObject requestIndexObject, List<IconColumnViewModel> iconList, 
            string defaultImage, String checkboxString, ParentQuote parentQuote)
            : this(indexTableTypes, request, product, vendor, productSubcategory, parentCategory, unitType, subUnitType, subSubUnitType, requestIndexObject, iconList, defaultImage, 
                null, null, null, null, null, null, checkboxString, parentQuote, null, null)
        {
        }
        public RequestIndexPartialRowViewModel(AppUtility.IndexTableTypes indexTableTypes, Request request, Product product, Vendor vendor, ProductSubcategory productSubcategory, 
            ParentCategory parentCategory, UnitType unitType, UnitType subUnitType, UnitType subSubUnitType, RequestIndexObject requestIndexObject, List<IconColumnViewModel> iconList, 
            string defaultImage, FavoriteRequest favoriteRequest, ShareRequest shareRequest, ApplicationUser user, LocationInstance locationInstance, 
            LocationInstance parentLocationInstance, ParentRequest parentRequest, string checkboxString, ParentQuote parentQuote, List<Payment> payments, Request partialRequest)
        {
            r = request;
            r.Product = product;
            r.Product.Vendor = vendor;
            r.Product.ProductSubcategory = productSubcategory;
            r.Product.ProductSubcategory.ParentCategory = parentCategory;
            r.Product.UnitType = unitType;
            r.Product.SubUnitType = subUnitType;
            r.Product.SubSubUnitType = subSubUnitType;
            Vendor = vendor;
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
                r.RequestLocationInstances = new List<RequestLocationInstance>() { new RequestLocationInstance { Request = r, LocationInstance = locationInstance } };
                r.RequestLocationInstances.FirstOrDefault().LocationInstance.LocationInstanceParent = parentLocationInstance;
            }
            this.user = user;
            this.favoriteRequest = favoriteRequest;
            this.shareRequest = shareRequest;
            this.checkboxString = checkboxString;
            this.partialRequest = partialRequest;
            switch (indexTableTypes)
            {
                case AppUtility.IndexTableTypes.Approved:
                    Columns = GetApproveColumns();
                    break;
                case AppUtility.IndexTableTypes.ReceivedInventoryFavorites:
                    Columns = GetReceivedInventoryFavoriteColumns();
                    break;
                case AppUtility.IndexTableTypes.Ordered:
                    Columns = GetOrderedColumns();
                    break;
                case AppUtility.IndexTableTypes.ReceivedInventoryShared:
                    Columns = GetReceivedInventorySharedColumns();
                    break;
                case AppUtility.IndexTableTypes.ReceivedInventory:
                    Columns = GetReceivedInventoryColumns();
                    break;
                case AppUtility.IndexTableTypes.Summary:
                    Columns = GetSummaryColumns();
                    break;
                case AppUtility.IndexTableTypes.SummaryProprietary:
                    Columns = GetSummaryProprietaryColumns();
                    break;
                case AppUtility.IndexTableTypes.OrderedOperations:
                    Columns = GetOrderedOperationsColumns();
                    break;
                case AppUtility.IndexTableTypes.ReceivedInventoryOperations:
                    Columns = GetReceivedInventoryOperationsColumns();
                    break;
                case AppUtility.IndexTableTypes.AccountingGeneral:
                    Columns = GetAccountingGeneralColumns();
                    break;
                case AppUtility.IndexTableTypes.Cart:
                    Columns = GetCartColumns();
                    break;
                case AppUtility.IndexTableTypes.AccountingNotifications:
                    Columns = GetAccountingNotificationsColumns();
                    break;
                case AppUtility.IndexTableTypes.AccountingPaymentsDefault:
                    Columns = GetAccountingPaymentsDefaultColumns();
                    break;
                case AppUtility.IndexTableTypes.AccountingPaymentsInstallments:
                    Columns = GetAccountingPaymentsInstallmentsColumns();
                    break;
                case AppUtility.IndexTableTypes.LabQuotes:
                    Columns = GetLabQuotesColumns();
                    break;
                case AppUtility.IndexTableTypes.LabOrders:
                    Columns = GetLabOrdersColumns();
                    break;
            }

        }


        public RequestIndexPartialRowViewModel(AppUtility.IndexTableTypes indexTableTypes, Request request, Product product, Vendor vendor, ProductSubcategory productSubcategory, ParentCategory parentCategory, UnitType unitType, UnitType subUnitType, UnitType subSubUnitType, RequestIndexObject requestIndexObject, List<IconColumnViewModel> iconList, string defaultImage, FavoriteRequest favoriteRequest, ShareRequest shareRequest, ApplicationUser user, LocationInstance locationInstance, LocationInstance locationInstanceParent, ParentRequest parentRequest)
            : this(indexTableTypes, request, product, vendor, productSubcategory, parentCategory, unitType, subUnitType, subSubUnitType, requestIndexObject, iconList, defaultImage, favoriteRequest, shareRequest, user, locationInstance, locationInstanceParent, parentRequest, null, null, null, null)
        {
        }
        public RequestIndexPartialRowViewModel(AppUtility.IndexTableTypes indexTableTypes, Request request, Product product, Vendor vendor, ProductSubcategory productSubcategory, ParentCategory parentCategory, UnitType unitType, UnitType subUnitType, UnitType subSubUnitType, RequestIndexObject requestIndexObject, List<IconColumnViewModel> iconList, string defaultImage, FavoriteRequest favoriteRequest, ApplicationUser user,  ParentRequest parentRequest)
           : this(indexTableTypes, request, product, vendor, productSubcategory, parentCategory, unitType, subUnitType, subSubUnitType, requestIndexObject, iconList, defaultImage, favoriteRequest, null, user, null, null, parentRequest, null, null, null, null)
        {
        }

        public RequestIndexPartialRowViewModel(AppUtility.IndexTableTypes indexTableTypes, Request request, Product product, Vendor vendor, ProductSubcategory productSubcategory, ParentCategory parentCategory, UnitType unitType, UnitType subUnitType, UnitType subSubUnitType, RequestIndexObject requestIndexObject, List<IconColumnViewModel> iconList, string defaultImage, FavoriteRequest favoriteRequest, ApplicationUser user, LocationInstance locationInstance, LocationInstance locationInstanceParent, ParentRequest parentRequest)
            : this(indexTableTypes, request, product, vendor, productSubcategory, parentCategory, unitType, subUnitType, subSubUnitType, requestIndexObject, iconList, defaultImage, favoriteRequest, null, user, locationInstance, locationInstanceParent, parentRequest, null, null, null, null)
        
        {          
        }

        public IEnumerable<RequestIndexPartialColumnViewModel> Columns { get; set; }
        public Vendor Vendor { get; set; }
        public decimal TotalCost { get; set; }
        [Column(TypeName = "decimal(18, 3)")]
        public decimal ExchangeRate { get; set; }
        public string ButtonClasses { get; set; }
        public string ButtonText { get; set; }
        private static List<IconColumnViewModel> GetIconsByIndividualRequest(int RequestID, List<IconColumnViewModel> iconList, bool needsPlaceholder, FavoriteRequest favoriteRequest = null, Request request = null, ApplicationUser user = null)
        {
            var newIconList = AppUtility.DeepClone<List<IconColumnViewModel>>(iconList);
            //favorite icon
            var favIconIndex = newIconList.FindIndex(ni => ni.IconAjaxLink?.Contains("request-favorite") ?? false);

            if (favIconIndex != -1 && favoriteRequest != null) 
            {
                var unLikeIcon = new IconColumnViewModel(" icon-favorite-24px", "var(--order-inv-color);", "request-favorite request-unlike", "Unfavorite");
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
               
                var reorderIconIndex = newIconList.FindIndex(ni => ni.TooltipTitle=="Reorder");
                if(reorderIconIndex !=-1)
                {
                    if(request.Product.UnitTypeID ==-1 || request.Product.ProductSubcategory.IsOldSubCategory)
                    {
                        newIconList.RemoveAt(reorderIconIndex);
                        newIconList.Insert(reorderIconIndex, placeholder);
                    }
                }
                var morePopoverIndex = newIconList.FindIndex(ni => ni.IconAjaxLink=="popover-more");
                if (morePopoverIndex != -1)
                {
                    var popoverReorder = newIconList.ElementAt(morePopoverIndex).IconPopovers.FindIndex(ni => ni.Action =="Reorder");
                    if (popoverReorder != -1)
                    {
                        if (request.Product.UnitTypeID == -1 || request.Product.ProductSubcategory.IsOldSubCategory)
                        {
                            newIconList[morePopoverIndex].IconPopovers.RemoveAt(popoverReorder);
                        }
                    }
                }
            }
            return newIconList;
        }
        private static int GetOrderTypeEnum(string orderType)
        {
            return (int)Enum.Parse(typeof(AppUtility.OrderTypeEnum), orderType);
        }
        private String GetSharedBy(Request request, ShareRequest shareRequest)
        {
            try
            {
                var applicationUser = shareRequest.FromApplicationUser;
                return applicationUser.FirstName + " " + applicationUser.LastName;
            }
            catch(Exception ex)
            {
                return "No Person Specified";
            }
        }
        private List<string> GetApplicationUserName()
        {
            try
            {
                return new List<string>() { r.ApplicationUserCreator.FirstName + " " + r.ApplicationUserCreator.LastName } ;
            }
            catch (Exception ex)
            {
                return new List<string>() { "No Person Specified" };
            }
        }
        private string GetLocationInstanceNameBefore()
        {
            try
            {
                var newLIName = "";
                if (r.RequestLocationInstances.FirstOrDefault().LocationInstance.LocationInstanceParentID == null)//is temporary location
                {
                    newLIName = r.RequestLocationInstances.FirstOrDefault().LocationInstance.LocationInstanceName;
                }
                else
                {
                    newLIName = r.RequestLocationInstances.FirstOrDefault().LocationInstance.LocationInstanceParent.LocationInstanceName;
                }
                return newLIName;
            }
            catch (Exception ex)
            {
                return "location has an error";
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
        private List<string> GetProductName()
        {
            try
            {
                return new List<string>() { r.Product.ProductName };
            }
            catch (Exception ex)
            {
                return new List<string>() { "item name has an error" };
            }
        }
        private List<string> GetVendorName()
        {
            try
            {
                if(r.OrderType == AppUtility.OrderTypeEnum.Save.ToString())
                {
                    return new List<string>() { "Centarix" };
                }
                return new List<string>() { r.Product.Vendor.VendorEnName };
            }
            catch (Exception ex)
            {
                return new List<string>() { "vendor has an error" };
            }
        }
        private List<string> GetPaymentDate()
        {
            try
            {
                return new List<string>() { payments.FirstOrDefault().PaymentDate.GetElixirDateFormat() };
            }
            catch (Exception ex)
            {
                return new List<string>() { "payment date has an error" };
            }
        }
        private List<string> GetDateForFavoriteRequest()
        {
                if(r.OrderType == AppUtility.OrderTypeEnum.Save.ToString())
                {
                    return new List<string>() { r.CreationDate.GetElixirDateFormat() };
                }
                return new List<string>() { r.ArrivalDate.GetElixirDateFormat() };
        }
        private IEnumerable<RequestIndexPartialColumnViewModel> GetApproveColumns()
        {
            yield return new RequestIndexPartialColumnViewModel() { Title = "", Width = 10, Image = GetImageURL() };
            yield return new RequestIndexPartialColumnViewModel() { Title = "Item Name", Width = 15, Value =  GetProductName() , AjaxLink = "load-product-details", AjaxID = r.RequestID, ShowTooltip = true };
            yield return new RequestIndexPartialColumnViewModel() { Title = "Vendor", Width = 10, Value = GetVendorName() , ShowTooltip = true };
            yield return new RequestIndexPartialColumnViewModel() { Title = "Amount", Width = 10, Value = AppUtility.GetAmountColumn(r) };
            yield return new RequestIndexPartialColumnViewModel() { Title = "Category", Width = 11, Value = AppUtility.GetCategoryColumn(requestIndexObject.CategorySelected, requestIndexObject.SubcategorySelected, r.Product),
                                                                        FilterEnum = AppUtility.FilterEnum.Category, ShowTooltip = true };
            yield return new RequestIndexPartialColumnViewModel() { Title = "Owner", Width = 12, Value = GetApplicationUserName() };
            yield return new RequestIndexPartialColumnViewModel() { Title = "Price", Width = 10, Value = AppUtility.GetPriceColumn(requestIndexObject.SelectedPriceSort, r, requestIndexObject.SelectedCurrency), FilterEnum = AppUtility.FilterEnum.Price };
            yield return new RequestIndexPartialColumnViewModel() { Title = "Date Created", Width = 12, Value = new List<string>() { r.CreationDate.GetElixirDateFormat() } };
            yield return new RequestIndexPartialColumnViewModel() { Title = "", Width = 10, Icons = GetIconsByIndividualRequest(r.RequestID, iconList, false, null, r, null), AjaxID = r.RequestID };
            yield return new RequestIndexPartialColumnViewModel() { Width = 0, AjaxLink = "p-0 d-none order-type" + r.RequestID, AjaxID = GetOrderTypeEnum(r.OrderType), Value = new List<string>() { r.OrderType.ToString() } };
        }
        private IEnumerable<RequestIndexPartialColumnViewModel> GetOrderedColumns()
        {
            yield return new RequestIndexPartialColumnViewModel() { Title = "", Width = 10, Image = GetImageURL() };
            yield return new RequestIndexPartialColumnViewModel() { Title = "Item Name", Width = 15, Value = GetProductName(), AjaxLink = "load-product-details", AjaxID = r.RequestID, ShowTooltip = true };
            yield return new RequestIndexPartialColumnViewModel() { Title = "Vendor", Width = 10, Value = GetVendorName(), ShowTooltip = true };
            yield return new RequestIndexPartialColumnViewModel() { Title = "Amount", Width = 10, Value = AppUtility.GetAmountColumn(r) };
            yield return new RequestIndexPartialColumnViewModel() { Title = "Category", Width = 11, Value = AppUtility.GetCategoryColumn(requestIndexObject.CategorySelected, requestIndexObject.SubcategorySelected, r.Product), 
                                                                        FilterEnum = AppUtility.FilterEnum.Category, ShowTooltip = true };
            yield return new RequestIndexPartialColumnViewModel() { Title = "Owner", Width = 12, Value = GetApplicationUserName() };
            yield return new RequestIndexPartialColumnViewModel() { Title = "Price", Width = 10, Value = AppUtility.GetPriceColumn(requestIndexObject.SelectedPriceSort, r, requestIndexObject.SelectedCurrency), FilterEnum = AppUtility.FilterEnum.Price };
            yield return new RequestIndexPartialColumnViewModel() { Title = "Date Ordered", Width = 12, Value = new List<string>() { AppUtility.GetDateOrderedString(r.ParentRequest) } };
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
            yield return new RequestIndexPartialColumnViewModel() { Title = "Item Name", Width = 14, Value = GetProductName(), AjaxLink = "load-product-details", AjaxID = r.RequestID, ShowTooltip = true };
            yield return new RequestIndexPartialColumnViewModel() { Title = "Vendor", Width = 9, Value = GetVendorName(), ShowTooltip = true };
            yield return new RequestIndexPartialColumnViewModel() { Title = "Amount", Width = 9, Value = AppUtility.GetAmountColumn(r) };
            yield return new RequestIndexPartialColumnViewModel() { Title = "Location", Width = 9, Value = new List<string>() { GetLocationInstanceNameBefore() } };
            yield return new RequestIndexPartialColumnViewModel() { Title = "Category", Width = 11, Value = AppUtility.GetCategoryColumn(requestIndexObject.CategorySelected, requestIndexObject.SubcategorySelected, r.Product), FilterEnum = AppUtility.FilterEnum.Category, ShowTooltip = true };
            yield return new RequestIndexPartialColumnViewModel() { Title = "Owner", Width = 10, Value = GetApplicationUserName() };
            yield return new RequestIndexPartialColumnViewModel() { Title = "Price", Width = 10, Value = AppUtility.GetPriceColumn(requestIndexObject.SelectedPriceSort, r, requestIndexObject.SelectedCurrency), FilterEnum = AppUtility.FilterEnum.Price };
            yield return new RequestIndexPartialColumnViewModel() { Title = "Arrival Date", Width = 10, Value = GetDateForFavoriteRequest() };
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
            yield return new RequestIndexPartialColumnViewModel() { Title = "Item Name", Width = 14, Value = GetProductName(), AjaxLink = "load-product-details", AjaxID = r.RequestID, ShowTooltip = true };
            yield return new RequestIndexPartialColumnViewModel() { Title = "Vendor", Width = 9, Value = GetVendorName(), ShowTooltip = true };
            yield return new RequestIndexPartialColumnViewModel() { Title = "Amount", Width = 9, Value = AppUtility.GetAmountColumn(r) };
            yield return new RequestIndexPartialColumnViewModel() { Title = "Location", Width = 9, Value = new List<string>() { GetLocationInstanceNameBefore() } };
            yield return new RequestIndexPartialColumnViewModel() { Title = "Category", Width = 11, Value = AppUtility.GetCategoryColumn(requestIndexObject.CategorySelected, requestIndexObject.SubcategorySelected, r.Product), FilterEnum = AppUtility.FilterEnum.Category, ShowTooltip = true };
            yield return new RequestIndexPartialColumnViewModel() { Title = "Owner", Width = 10, Value = GetApplicationUserName() };
            yield return new RequestIndexPartialColumnViewModel() { Title = "Price", Width = 10, Value = AppUtility.GetPriceColumn(requestIndexObject.SelectedPriceSort, r, requestIndexObject.SelectedCurrency), FilterEnum = AppUtility.FilterEnum.Price };
            yield return new RequestIndexPartialColumnViewModel() { Title = "Shared By", Width = 10, Value = new List<string>() { GetSharedBy(r, shareRequest) } };
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
            yield return new RequestIndexPartialColumnViewModel() { Title = "Item Name", Width = 14, Value = GetProductName(), AjaxLink = "load-product-details", AjaxID = r.RequestID, ShowTooltip = true };
            yield return new RequestIndexPartialColumnViewModel() { Title = "Vendor", Width = 9, Value = GetVendorName(), ShowTooltip = true };
            yield return new RequestIndexPartialColumnViewModel() { Title = "Amount", Width = 9, Value = AppUtility.GetAmountColumn(r) };
            yield return new RequestIndexPartialColumnViewModel() { Title = "Location", Width = 9, Value = new List<string>() { GetLocationInstanceNameBefore() } };
            yield return new RequestIndexPartialColumnViewModel() { Title = "Category", Width = 11, Value = AppUtility.GetCategoryColumn(requestIndexObject.CategorySelected, requestIndexObject.SubcategorySelected, r.Product), FilterEnum = AppUtility.FilterEnum.Category, ShowTooltip = true };
            yield return new RequestIndexPartialColumnViewModel() { Title = "Owner", Width = 9, Value = GetApplicationUserName()  };
            yield return new RequestIndexPartialColumnViewModel() { Title = "Price", Width = 10, Value = AppUtility.GetPriceColumn(requestIndexObject.SelectedPriceSort, r, requestIndexObject.SelectedCurrency), FilterEnum = AppUtility.FilterEnum.Price };
            yield return new RequestIndexPartialColumnViewModel() { Title = "Arrival Date", Width = 10, Value = new List<string>() { r.ArrivalDate.GetElixirDateFormat() } };
            yield return new RequestIndexPartialColumnViewModel()
            {
                Title = "",
                Width = 10,
                Icons = GetIconsByIndividualRequest(r.RequestID, iconList, false, favoriteRequest, request:r, user),
                AjaxID = r.RequestID
            };
        }
        
        private IEnumerable<RequestIndexPartialColumnViewModel> GetSummaryColumns()
        {
            yield return new RequestIndexPartialColumnViewModel() { Title = "", Width = 9, Image = GetImageURL() };
            yield return new RequestIndexPartialColumnViewModel() { Title = "Item Name", Width = 14, Value = GetProductName(), AjaxLink = "load-product-details", AjaxID = r.RequestID, ShowTooltip = true };
            yield return new RequestIndexPartialColumnViewModel() { Title = "Vendor", Width = 9, Value = GetVendorName(), ShowTooltip = true };
            yield return new RequestIndexPartialColumnViewModel() { Title = "Amount", Width = 9, Value = AppUtility.GetAmountColumn(r) };
            yield return new RequestIndexPartialColumnViewModel() { Title = "Location", Width = 9, Value = new List<string>() { GetLocationInstanceNameBefore() } };
            yield return new RequestIndexPartialColumnViewModel() { Title = "Category", Width = 11, Value = AppUtility.GetCategoryColumn(requestIndexObject.CategorySelected, requestIndexObject.SubcategorySelected, r.Product), FilterEnum = AppUtility.FilterEnum.Category, ShowTooltip = true };
            yield return new RequestIndexPartialColumnViewModel() { Title = "Owner", Width = 9, Value = GetApplicationUserName() };
            yield return new RequestIndexPartialColumnViewModel() { Title = "Price", Width = 10, Value = AppUtility.GetPriceColumn(requestIndexObject.SelectedPriceSort, r, requestIndexObject.SelectedCurrency), FilterEnum = AppUtility.FilterEnum.Price };
            yield return new RequestIndexPartialColumnViewModel() { Title = "Date Ordered", Width = 10, Value = new List<string>() { AppUtility.GetDateOrderedString(r.ParentRequest) } };
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

            yield return new RequestIndexPartialColumnViewModel() { Title = "", Width = 10, Image = GetImageURL() };
            yield return new RequestIndexPartialColumnViewModel() { Title = "Item Name", Width = 15, Value = GetProductName(), AjaxLink = "load-product-details-summary", AjaxID = r.RequestID, ShowTooltip = true };
            yield return new RequestIndexPartialColumnViewModel() { Title = "Amount", Width = 10, Value = AppUtility.GetAmountColumn(r) };
            yield return new RequestIndexPartialColumnViewModel() { Title = "Price", Width = 10, Value = AppUtility.GetPriceColumn(requestIndexObject.SelectedPriceSort, r, requestIndexObject.SelectedCurrency), FilterEnum = AppUtility.FilterEnum.Price };
            yield return new RequestIndexPartialColumnViewModel() { Title = "Vendor", Width = 10, Value = GetVendorName(), ShowTooltip = true };
            yield return new RequestIndexPartialColumnViewModel() { Title = "Category", Width = 11, Value = AppUtility.GetCategoryColumn(requestIndexObject.CategorySelected, requestIndexObject.SubcategorySelected, r.Product), FilterEnum = AppUtility.FilterEnum.Category };
            yield return new RequestIndexPartialColumnViewModel() { Title = "Date Ordered", Width = 12, Value = new List<string>() { AppUtility.GetDateOrderedString(r.ParentRequest) } };

        }
        private IEnumerable<RequestIndexPartialColumnViewModel> GetSummaryProprietaryColumns()
        {
            yield return new RequestIndexPartialColumnViewModel() { Title = "", Width = 9, Image = GetImageURL() };
            yield return new RequestIndexPartialColumnViewModel() { Title = "Item Name", Width = 14, Value = GetProductName(), AjaxLink = "load-product-details-summary", AjaxID = r.RequestID , ShowTooltip = true };
            yield return new RequestIndexPartialColumnViewModel() { Title = "Amount", Width = 9, Value = AppUtility.GetAmountColumn(r) };
            yield return new RequestIndexPartialColumnViewModel() { Title = "Location", Width = 9, Value = new List<string>() { GetLocationInstanceNameBefore() } };
            yield return new RequestIndexPartialColumnViewModel() { Title = "Category", Width = 11, Value = AppUtility.GetCategoryColumn(requestIndexObject.CategorySelected, requestIndexObject.SubcategorySelected, r.Product), FilterEnum = AppUtility.FilterEnum.Category, ShowTooltip = true };
            yield return new RequestIndexPartialColumnViewModel() { Title = "Owner", Width = 10, Value = GetApplicationUserName() };
            yield return new RequestIndexPartialColumnViewModel() { Title = "Date Created", Width = 10, Value = new List<string>() { r.CreationDate.GetElixirDateFormat() } };
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
            yield return new RequestIndexPartialColumnViewModel() { Title = "Item Name", Width = 14, Value = GetProductName(), AjaxLink = "load-product-details", AjaxID = r.RequestID, ShowTooltip = true };
            yield return new RequestIndexPartialColumnViewModel() { Title = "Vendor", Width = 9, Value = GetVendorName(), ShowTooltip = true };
            yield return new RequestIndexPartialColumnViewModel() { Title = "Category", Width = 11, Value = AppUtility.GetCategoryColumn(requestIndexObject.CategorySelected, requestIndexObject.SubcategorySelected, r.Product), FilterEnum = AppUtility.FilterEnum.Category, ShowTooltip = true };
            yield return new RequestIndexPartialColumnViewModel() { Title = "Owner", Width = 10, Value = GetApplicationUserName() };
            yield return new RequestIndexPartialColumnViewModel() { Title = "Price", Width = 10, Value = AppUtility.GetPriceColumn(requestIndexObject.SelectedPriceSort, r, requestIndexObject.SelectedCurrency), FilterEnum = AppUtility.FilterEnum.Price };
            yield return new RequestIndexPartialColumnViewModel() { Title = "Date Ordered", Width = 10, Value = new List<string>() { AppUtility.GetDateOrderedString(r.ParentRequest) } };
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
            yield return new RequestIndexPartialColumnViewModel() { Title = "Item Name", Width = 15, Value = GetProductName(), AjaxLink = "load-product-details", AjaxID = r.RequestID, ShowTooltip = true };
            yield return new RequestIndexPartialColumnViewModel() { Title = "Vendor", Width = 10, Value = GetVendorName(), ShowTooltip = true };
            yield return new RequestIndexPartialColumnViewModel() { Title = "Category", Width = 11, Value = AppUtility.GetCategoryColumn(requestIndexObject.CategorySelected, requestIndexObject.SubcategorySelected, r.Product), FilterEnum = AppUtility.FilterEnum.Category, ShowTooltip = true };
            yield return new RequestIndexPartialColumnViewModel() { Title = "Owner", Width = 12, Value = GetApplicationUserName() };
            yield return new RequestIndexPartialColumnViewModel() { Title = "Price", Width = 10, Value = AppUtility.GetPriceColumn(requestIndexObject.SelectedPriceSort, r, requestIndexObject.SelectedCurrency), FilterEnum = AppUtility.FilterEnum.Price };
            yield return new RequestIndexPartialColumnViewModel() { Title = "Date Ordered", Width = 12, Value = new List<string>() { AppUtility.GetDateOrderedString(r.ParentRequest) } };
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
            yield return new RequestIndexPartialColumnViewModel() { Title = "", Width = 5, Value = new List<string>() { checkboxString }, AjaxID = r.RequestID };
            yield return new RequestIndexPartialColumnViewModel() { Title = "", Width = 10, Image = GetImageURL() };
            yield return new RequestIndexPartialColumnViewModel() { Title = "Item Name", Width = 15, Value = GetProductName(), AjaxLink = "load-product-details", AjaxID = r.RequestID, ShowTooltip = true };
            yield return new RequestIndexPartialColumnViewModel() { Title = "Vendor", Width = 10, Value = GetVendorName(), ShowTooltip = true };
            yield return new RequestIndexPartialColumnViewModel() { Title = "Category", Width = 11, Value = AppUtility.GetCategoryColumn(requestIndexObject.CategorySelected, requestIndexObject.SubcategorySelected, r.Product), FilterEnum = AppUtility.FilterEnum.Category, ShowTooltip = true };
            yield return new RequestIndexPartialColumnViewModel() { Title = "Amount", Width = 10, Value = AppUtility.GetAmountColumn(r) };
            yield return new RequestIndexPartialColumnViewModel() { Title = "Price", Width = 10, Value = AppUtility.GetPriceColumn(requestIndexObject.SelectedPriceSort, r, requestIndexObject.SelectedCurrency), FilterEnum = AppUtility.FilterEnum.Price };
            yield return new RequestIndexPartialColumnViewModel()
            {
                Title = "",
                Width = 10,
                Icons = GetIconsByIndividualRequest(r.RequestID, iconList, false, favoriteRequest, request: r, user),
                AjaxID = r.RequestID,
            };

        }
        private IEnumerable<RequestIndexPartialColumnViewModel> GetAccountingNotificationsColumns()
        {
            yield return new RequestIndexPartialColumnViewModel() { Title = "", Width = 5, Value = new List<string>() { checkboxString}, AjaxID = r.RequestID };
            yield return new RequestIndexPartialColumnViewModel() { Title = "", Width = 10, Image = GetImageURL() };
            yield return new RequestIndexPartialColumnViewModel() { Title = "Item Name", Width = 15, Value = GetProductName(), AjaxLink = "load-product-details-summary", AjaxID = r.RequestID, ShowTooltip = true };
            yield return new RequestIndexPartialColumnViewModel() { Title = "Category", Width = 11, Value = AppUtility.GetCategoryColumn(requestIndexObject.CategorySelected, requestIndexObject.SubcategorySelected, r.Product), FilterEnum = AppUtility.FilterEnum.Category, ShowTooltip = true };
            yield return new RequestIndexPartialColumnViewModel() { Title = "Amount", Width = 10, Value = AppUtility.GetAmountColumn(r) };
            yield return new RequestIndexPartialColumnViewModel() { Title = "Price", Width = 10, Value = AppUtility.GetPriceColumn(requestIndexObject.SelectedPriceSort, r, requestIndexObject.SelectedCurrency), FilterEnum = AppUtility.FilterEnum.Price };
            yield return new RequestIndexPartialColumnViewModel() { Title = "Date Ordered", Width = 12, Value = new List<string>() { AppUtility.GetDateOrderedString(r.ParentRequest) } };
            yield return new RequestIndexPartialColumnViewModel()
            {
                Title = "",
                Width = 10,
                Icons = iconList,
                AjaxID = r.RequestID,
                Note = AppUtility.GetNote(requestIndexObject.SidebarType, r )
            };
        }
        private IEnumerable<RequestIndexPartialColumnViewModel> GetAccountingPaymentsDefaultColumns()
        {
            yield return new RequestIndexPartialColumnViewModel() { Title = "", Width = 5, Value = new List<string>() { checkboxString }, AjaxID = r.RequestID };
            yield return new RequestIndexPartialColumnViewModel() { Title = "", Width = 10, Image = GetImageURL() };
            yield return new RequestIndexPartialColumnViewModel() { Title = "Item Name", Width = 15, Value = GetProductName(), AjaxLink = "load-product-details-summary", AjaxID = r.RequestID, ShowTooltip = true };
            yield return new RequestIndexPartialColumnViewModel() { Title = "Category", Width = 11, Value = AppUtility.GetCategoryColumn(requestIndexObject.CategorySelected, requestIndexObject.SubcategorySelected, r.Product), FilterEnum = AppUtility.FilterEnum.Category, ShowTooltip = true };
            yield return new RequestIndexPartialColumnViewModel() { Title = "Amount", Width = 10, Value = AppUtility.GetAmountColumn(r) };
            yield return new RequestIndexPartialColumnViewModel() { Title = "Price", Width = 10, Value = AppUtility.GetPriceColumn(requestIndexObject.SelectedPriceSort, r, requestIndexObject.SelectedCurrency), FilterEnum = AppUtility.FilterEnum.Price };
            yield return new RequestIndexPartialColumnViewModel() { Title = "Date Ordered", Width = 12, Value = new List<string>() { AppUtility.GetDateOrderedString(r.ParentRequest) } };
            yield return new RequestIndexPartialColumnViewModel()
            {
                Title = "",
                Width = 10,
                Icons = iconList,
                AjaxID = r.RequestID
            };
        }
        private IEnumerable<RequestIndexPartialColumnViewModel> GetAccountingPaymentsInstallmentsColumns()
        {
            yield return new RequestIndexPartialColumnViewModel() { Title = "", Width = 5, Value = new List<string>() { checkboxString }, AjaxID = r.RequestID };
            yield return new RequestIndexPartialColumnViewModel() { Title = "", Width = 10, Image = GetImageURL() };
            yield return new RequestIndexPartialColumnViewModel() { Title = "Item Name", Width = 15, Value = GetProductName(), AjaxLink = "load-product-details-summary", 
                                                                    AjaxID = r.RequestID, ShowTooltip = true };
            yield return new RequestIndexPartialColumnViewModel() { Title = "Category", Width = 11, Value = AppUtility.GetCategoryColumn(requestIndexObject.CategorySelected, requestIndexObject.SubcategorySelected, 
                                                                    r.Product), FilterEnum = AppUtility.FilterEnum.Category, ShowTooltip = true};
            yield return new RequestIndexPartialColumnViewModel() { Title = "Amount", Width = 10, Value = AppUtility.GetAmountColumn(r) };
            yield return new RequestIndexPartialColumnViewModel() { Title = "Price", Width = 10, Value = AppUtility.GetPriceColumn(requestIndexObject.SelectedPriceSort, r, 
                                                                    requestIndexObject.SelectedCurrency), FilterEnum = AppUtility.FilterEnum.Price };
            yield return new RequestIndexPartialColumnViewModel() { Title = "Payment Date", Width = 12, Value = GetPaymentDate() };
            yield return new RequestIndexPartialColumnViewModel()
            {
                Title = "",
                Width = 10,
                Icons = iconList,
                AjaxID = payments.FirstOrDefault().PaymentID
            };
        }

        private IEnumerable<RequestIndexPartialColumnViewModel> GetLabQuotesColumns()
        {
            yield return new RequestIndexPartialColumnViewModel() { Title = "", Width = 3, Value = new List<string>() { checkboxString }, AjaxID = r.RequestID };
            yield return new RequestIndexPartialColumnViewModel() { Title = "", Width = 5, Image = GetImageURL() };
            yield return new RequestIndexPartialColumnViewModel() { Title = "Item Name", Width = 15, Value = GetProductName(), AjaxLink = "load-product-details", AjaxID = r.RequestID, ShowTooltip = true };
            //yield return new RequestIndexPartialColumnViewModel() { Title = "Vendor", Width = 12, Value = GetVendorName(), ShowTooltip = true };
            yield return new RequestIndexPartialColumnViewModel() { Title = "Category", Width = 10, Value = AppUtility.GetCategoryColumn(requestIndexObject.CategorySelected, requestIndexObject.SubcategorySelected, r.Product), FilterEnum = AppUtility.FilterEnum.Category, ShowTooltip = true };
            yield return new RequestIndexPartialColumnViewModel() { Title = "Amount", Width = 8, Value = AppUtility.GetAmountColumn(r) };
            yield return new RequestIndexPartialColumnViewModel() { Title = "Price", Width = 9, Value = AppUtility.GetPriceColumn(requestIndexObject.SelectedPriceSort, r, requestIndexObject.SelectedCurrency), FilterEnum = AppUtility.FilterEnum.Price };
            yield return new RequestIndexPartialColumnViewModel() { Title = "Owner", Width = 8, Value = GetApplicationUserName() };
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
            yield return new RequestIndexPartialColumnViewModel() { Title = "", Width = 5, Value = new List<string>() { checkboxString }, AjaxID = r.RequestID };
            yield return new RequestIndexPartialColumnViewModel() { Title = "", Width = 7, Image = GetImageURL() };
            yield return new RequestIndexPartialColumnViewModel() { Title = "Item Name", Width = 15, Value = GetProductName(), AjaxLink = "load-product-details", AjaxID = r.RequestID, ShowTooltip = true };
            //yield return new RequestIndexPartialColumnViewModel() { Title = "Vendor", Width = 10, Value = GetVendorName(), ShowTooltip = true };
            yield return new RequestIndexPartialColumnViewModel() { Title = "Category", Width = 11, Value = AppUtility.GetCategoryColumn(requestIndexObject.CategorySelected, requestIndexObject.SubcategorySelected, r.Product), FilterEnum = AppUtility.FilterEnum.Category, ShowTooltip = true };
            yield return new RequestIndexPartialColumnViewModel() { Title = "Amount", Width = 10, Value = AppUtility.GetAmountColumn(r) };
            yield return new RequestIndexPartialColumnViewModel() { Title = "Price", Width = 10, Value = AppUtility.GetPriceColumn(requestIndexObject.SelectedPriceSort, r, requestIndexObject.SelectedCurrency), FilterEnum = AppUtility.FilterEnum.Price };
            yield return new RequestIndexPartialColumnViewModel() { Title = "Owner", Width = 12, Value = GetApplicationUserName() };
            yield return new RequestIndexPartialColumnViewModel()
            {
                Title = "",
                Width = 10,
                Icons = GetIconsByIndividualRequest(r.RequestID, iconList, false),
                AjaxID = r.RequestID
            };
        }
    }
}
