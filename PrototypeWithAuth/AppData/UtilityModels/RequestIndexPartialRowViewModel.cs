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
        private static List<IconColumnViewModel> iconList;
        private static Request r;
        private static ApplicationUser user;
        private static FavoriteRequest favoriteRequest;
        public RequestIndexPartialRowViewModel(AppUtility.IndexTableTypes indexTableTypes, Request request, Product product, Vendor vendor, ProductSubcategory productSubcategory, ParentCategory parentCategory, UnitType unitType, UnitType subUnitType, UnitType subSubUnitType, 
            RequestIndexObject requestIndexObject, List<IconColumnViewModel> iconList, string defaultImage, FavoriteRequest favoriteRequest, ApplicationUser user, LocationInstance locationInstance, LocationInstance parentLocationInstance, ParentRequest parentRequest)
        {
            r = request;
            r.Product = product;
            r.Product.Vendor = vendor;
            r.Product.ProductSubcategory = productSubcategory;
            r.Product.ProductSubcategory.ParentCategory = parentCategory;
            r.UnitType = unitType;
            r.SubUnitType = subUnitType;
            r.SubSubUnitType = subSubUnitType;
            r.ParentRequest = parentRequest;
            RequestIndexPartialRowViewModel.requestIndexObject = requestIndexObject;
            RequestIndexPartialRowViewModel.defaultImage = defaultImage;
            RequestIndexPartialRowViewModel.iconList = iconList;
            RequestIndexPartialRowViewModel.user = user;
            RequestIndexPartialRowViewModel.favoriteRequest = favoriteRequest;
            switch (indexTableTypes)
            {
                case AppUtility.IndexTableTypes.Approved:
                    Columns = GetApproveColumns();
                    break;
                case AppUtility.IndexTableTypes.ReceivedInventoryFavorites:
                    Columns = GetReceivedInventoryFavoriteColumns();
                    break;
                case AppUtility.IndexTableTypes.Ordered:
                    GetOrderedColumns();
                    break;
            }
            
        }
        public RequestIndexPartialRowViewModel() {}
        private static int GetOrderTypeEnum(string orderType)
        {
            return (int)Enum.Parse(typeof(AppUtility.OrderTypeEnum), orderType);
        }
        public IEnumerable<RequestIndexPartialColumnViewModel> Columns {  get; private set; }

        private static IEnumerable<RequestIndexPartialColumnViewModel> GetApproveColumns()
        {
            yield return new RequestIndexPartialColumnViewModel() { Title = "", Width = 10, Image = r.Product.ProductSubcategory.ImageURL == null ? defaultImage : r.Product.ProductSubcategory.ImageURL };
            yield return new RequestIndexPartialColumnViewModel() { Title = "Item Name", Width = 15, Value = new List<string>() { r.Product.ProductName }, AjaxLink = "load-product-details", AjaxID = r.RequestID };
            yield return new RequestIndexPartialColumnViewModel() { Title = "Vendor", Width = 10, Value = new List<string>() { r.Product.Vendor.VendorEnName } };
            yield return new RequestIndexPartialColumnViewModel() { Title = "Amount", Width = 10, Value = AppUtility.GetAmountColumn(r, r.UnitType, r.SubUnitType, r.SubSubUnitType) };
            yield return new RequestIndexPartialColumnViewModel() { Title = "Category", Width = 11, Value = AppUtility.GetCategoryColumn(requestIndexObject.CategorySelected, requestIndexObject.SubcategorySelected, r.Product.ProductSubcategory, r.Product.ProductSubcategory.ParentCategory), FilterEnum = AppUtility.FilterEnum.Category };
            yield return new RequestIndexPartialColumnViewModel() { Title = "Owner", Width = 12, Value = new List<string>() { r.ApplicationUserCreator.FirstName + " " + r.ApplicationUserCreator.LastName } };
            yield return new RequestIndexPartialColumnViewModel() { Title = "Price", Width = 10, Value = AppUtility.GetPriceColumn(requestIndexObject.SelectedPriceSort, r, requestIndexObject.SelectedCurrency), FilterEnum = AppUtility.FilterEnum.Price };
            yield return new RequestIndexPartialColumnViewModel() { Title = "Date Created", Width = 12, Value = new List<string>() { r.CreationDate.ToString("dd'/'MM'/'yyyy") } };
            yield return new RequestIndexPartialColumnViewModel() { Title = "", Width = 10, Icons = GetIconsByIndividualRequest(r.RequestID, iconList, false, null, r, null), AjaxID = r.RequestID };
            yield return new RequestIndexPartialColumnViewModel() { Width = 0, AjaxLink = " d-none order-type" + r.RequestID, AjaxID = GetOrderTypeEnum(r.OrderType), Value = new List<string>() { r.OrderType.ToString() } };
        }
        private static IEnumerable<RequestIndexPartialColumnViewModel> GetOrderedColumns()
        {
            yield return new RequestIndexPartialColumnViewModel() { Title = "", Width = 10, Image = r.Product.ProductSubcategory.ImageURL == null ? defaultImage : r.Product.ProductSubcategory.ImageURL };
            yield return new RequestIndexPartialColumnViewModel() { Title = "Item Name", Width = 15, Value = new List<string>() { r.Product.ProductName }, AjaxLink = "load-product-details", AjaxID = r.RequestID };
            yield return new RequestIndexPartialColumnViewModel() { Title = "Vendor", Width = 10, Value = new List<string>() { r.Product.Vendor.VendorEnName } };
            yield return new RequestIndexPartialColumnViewModel() { Title = "Amount", Width = 10, Value = AppUtility.GetAmountColumn(r, r.UnitType, r.SubUnitType, r.SubSubUnitType) };
            yield return new RequestIndexPartialColumnViewModel() { Title = "Category", Width = 11, Value = AppUtility.GetCategoryColumn(requestIndexObject.CategorySelected, requestIndexObject.SubcategorySelected, r.Product.ProductSubcategory, r.Product.ProductSubcategory.ParentCategory), FilterEnum = AppUtility.FilterEnum.Category };
            yield return new RequestIndexPartialColumnViewModel() { Title = "Owner", Width = 12, Value = new List<string>() { r.ApplicationUserCreator.FirstName + " " + r.ApplicationUserCreator.LastName } };
            yield return new RequestIndexPartialColumnViewModel() { Title = "Price", Width = 10, Value = AppUtility.GetPriceColumn(requestIndexObject.SelectedPriceSort, r, requestIndexObject.SelectedCurrency), FilterEnum = AppUtility.FilterEnum.Price };
            yield return new RequestIndexPartialColumnViewModel() { Title = "Date Ordered", Width = 12, Value = new List<string>() { r.ParentRequest.OrderDate.ToString("dd'/'MM'/'yyyy") } };
            yield return new RequestIndexPartialColumnViewModel()
            {
                Title = "",
                Width = 10,
                Icons = iconList,
                AjaxID = r.RequestID
            };
        }
        private static IEnumerable<RequestIndexPartialColumnViewModel> GetReceivedInventoryFavoriteColumns()
        {
            yield return new RequestIndexPartialColumnViewModel() { Title = "", Width = 9, Image = r.Product.ProductSubcategory.ImageURL == null ? defaultImage : r.Product.ProductSubcategory.ImageURL };
            yield return new RequestIndexPartialColumnViewModel() { Title = "Item Name", Width = 14, Value = new List<string>() { r.Product.ProductName }, AjaxLink = "load-product-details", AjaxID = r.RequestID };
            yield return new RequestIndexPartialColumnViewModel() { Title = "Vendor", Width = 9, Value = new List<string>() { r.Product.Vendor.VendorEnName } };
            yield return new RequestIndexPartialColumnViewModel() { Title = "Amount", Width = 9, Value = AppUtility.GetAmountColumn(r, r.UnitType, r.SubUnitType, r.SubSubUnitType) };
            yield return new RequestIndexPartialColumnViewModel() { Title = "Location", Width = 9, Value = new List<string>() { GetLocationInstanceNameBefore(r.RequestLocationInstances.FirstOrDefault().LocationInstance, r.RequestLocationInstances.FirstOrDefault().LocationInstance.LocationInstanceParent) } };
            yield return new RequestIndexPartialColumnViewModel() { Title = "Category", Width = 11, Value = AppUtility.GetCategoryColumn(requestIndexObject.CategorySelected, requestIndexObject.SubcategorySelected, r.Product.ProductSubcategory, r.Product.ProductSubcategory.ParentCategory), FilterEnum = AppUtility.FilterEnum.Category };
            yield return new RequestIndexPartialColumnViewModel() { Title = "Owner", Width = 10, Value = new List<string>() { r.ApplicationUserCreator.FirstName + " " + r.ApplicationUserCreator.LastName } };
            yield return new RequestIndexPartialColumnViewModel() { Title = "Price", Width = 10, Value = AppUtility.GetPriceColumn(requestIndexObject.SelectedPriceSort, r, requestIndexObject.SelectedCurrency), FilterEnum = AppUtility.FilterEnum.Price };
            yield return new RequestIndexPartialColumnViewModel() { Title = "Arrival Date", Width = 10, Value = new List<string>() { r.ArrivalDate.ToString("dd'/'MM'/'yyyy") } };
            yield return new RequestIndexPartialColumnViewModel()
            {
                Title = "",
                Width = 10,
                Icons = GetIconsByIndividualRequest(r.RequestID, iconList, false, favoriteRequest, null, user),
                AjaxID = r.RequestID
            };
        }

    

       

        public Vendor Vendor { get; set; }
        public decimal TotalCost { get; set; }
        [Column(TypeName = "decimal(18, 3)")]
        public decimal ExchangeRate { get; set; }
        public string ButtonClasses {get ;set;}
        public string ButtonText { get; set; }

        private static string GetLocationInstanceNameBefore(LocationInstance locationInstance, LocationInstance parentLocationInstance)
        {
            var newLIName = "";
            if (locationInstance.LocationInstanceParentID == null)//is temporary location
            {
                newLIName = locationInstance.LocationInstanceName;
            }
            else
            {
                newLIName = parentLocationInstance.LocationInstanceName;
            }
            return newLIName;
        }

        private static List<IconColumnViewModel> GetIconsByIndividualRequest(int RequestID, List<IconColumnViewModel> iconList, bool needsPlaceholder, FavoriteRequest favoriteRequest = null, Request request = null, ApplicationUser user = null)
        {
            var newIconList = AppUtility.DeepClone(iconList);
            //favorite icon
            var favIconIndex = newIconList.FindIndex(ni => ni.IconAjaxLink.Contains("request-favorite"));

            if (favIconIndex != -1 && favoriteRequest != null) //check these checks
            {
                var unLikeIcon = new IconColumnViewModel(" icon-favorite-24px", "#5F79E2", "request-favorite request-unlike", "Unfavorite");
                newIconList[favIconIndex] = unLikeIcon;
            }
            //for approval icon
            if (request != null)
            {
                var forApprovalIconIndex = newIconList.FindIndex(ni => ni.IconAjaxLink.Contains("approve-order"));
                if (request.RequestStatusID != 1 && forApprovalIconIndex != -1)
                {
                    newIconList.RemoveAt(forApprovalIconIndex);
                    needsPlaceholder = true;
                }
                //resend icon
                var resendIcon = new IconColumnViewModel("Resend");
                var placeholder = new IconColumnViewModel("Placeholder");
                if (request.ParentQuote?.QuoteStatusID == 2)
                {
                    newIconList.Insert(0, resendIcon);
                }
                else if (needsPlaceholder)
                {
                    newIconList.Insert(0, placeholder);
                }
            }
            return newIconList;
        }

    }
}
