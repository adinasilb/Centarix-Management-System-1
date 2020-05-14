using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc.Rendering;
using PrototypeWithAuth.Models;
using PrototypeWithAuth.ViewModels;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace PrototypeWithAuth.AppData
{
    public static class AppUtility
    {

        public enum RequestPageTypeEnum { None, Request, Inventory, Cart, Search, Location }
        public enum PaymentPageTypeEnum { None, Notifications, General, Expenses, Suppliers } //these are all going to the ParentRequestIndex
        public enum RequestSidebarEnum { None, Type, Vendor, Owner, Location, Cart }

        public static int GetCountOfRequestsByRequestStatusIDVendorIDSubcategoryIDApplicationUserID(IQueryable<Request> RequestsList, int RequestStatusID, int VendorID = 0, int? SubcategoryID = 0, string ApplicationUserID = null)
        {
            int ReturnList = 0;
            if (VendorID > 0)
            {
                ReturnList = RequestsList
                    .Where(r => r.RequestStatusID == RequestStatusID)
                    .Where(r => r.Product.VendorID == VendorID)
                    .Count();
            }
            else if (SubcategoryID > 0)
            {
                ReturnList = RequestsList
                    .Where(r => r.RequestStatusID == RequestStatusID)
                    .Where(r => r.Product.ProductSubcategoryID == SubcategoryID)
                    .Count();
            }
            else if (ApplicationUserID != null)
            {
                ReturnList = RequestsList
                    .Where(r => r.RequestStatusID == RequestStatusID)
                    .Where(r => r.ParentRequest.ApplicationUserID == ApplicationUserID)
                    .Count();
            }
            else
            {
                ReturnList = RequestsList.Where(r => r.RequestStatusID == RequestStatusID).Count();
            }
            return ReturnList;
        }

        public static IQueryable<Request> GetRequestsListFromRequestStatusID(IQueryable<Request> FullRequestList, int RequestStatusID, int AmountToTake = 0)
        {
            IQueryable<Request> ReturnList = Enumerable.Empty<Request>().AsQueryable();
            if (AmountToTake > 0)
            {
                ReturnList = FullRequestList.Where(r => r.RequestStatusID == RequestStatusID).Take(AmountToTake);
            }
            else
            {
                ReturnList = FullRequestList.Where(r => r.RequestStatusID == RequestStatusID);
            }
            return ReturnList;
        }

        //this checks if a list is empty
        //right now used in the requestscontroller -> index
        public static Boolean IsEmpty<T>(this IEnumerable<T> source)
        {
            if (source == null)
                return true; // or throw an exception
            return !source.Any();
        }

        //combines two lists first checking if one is empty so it doesn't get an error
        public static IQueryable<Request> CombineTwoRequestsLists(IQueryable<Request> RequestListToCheck1, IQueryable<Request> RequestListToCheck2)
        {
            IQueryable<Request> ReturnList = Enumerable.Empty<Request>().AsQueryable();
            if (!RequestListToCheck1.IsEmpty() && RequestListToCheck2.IsEmpty())
            {
                ReturnList = RequestListToCheck1;
            }
            else if (RequestListToCheck1.IsEmpty() && !RequestListToCheck2.IsEmpty())
            {
                ReturnList = RequestListToCheck2;
            }
            else if (!RequestListToCheck1.IsEmpty() && !RequestListToCheck2.IsEmpty())
            {
                ReturnList = RequestListToCheck1.Concat(RequestListToCheck2).OrderByDescending(r => r.ParentRequest.OrderDate);
            }
            return ReturnList;
        }




        public static int ReplaceIntValueIfNull(int? value)
        {
            int iReturn = 0;
            if (value != null)
            {
                iReturn = (int)value;
            }
            return iReturn;
        }

        public static string ReplaceStringValueIfNull(string value)
        {
            string sReturn = "";
            if (value != null)
            {
                sReturn = value;
            }
            return sReturn;
        }

        struct PaymentNotificationValuePairs
        {
            public string NotificationListName;
            public int Amount;
        }

        //public static int InsertToLocationTierModelReturnForiegnKey() 
        //{
        // LocationsTier1Model locationsTier1Model = new LocationsTier1Model();
        //locationsTier1Model.LocationsTier1ModelDescription = addLocationTypeViewModel.Sublocations[i];
        //                locationsTier1Model.LocationsTier1ModelAbbreviation = addLocationTypeViewModel.Sublocations[i].Substring(0,1);
        //_context.Add(locationsTier1Model);
        //                _context.SaveChanges();
        //                var ltm = _context.LocationsTier1Models.Where(m => m.LocationsTier1ModelDescription == addLocationTypeViewModel.Sublocations[i]).FirstOrDefault();
        //foriegnKey = ltm.LocationsTier1ModelID;
        //}


        //.NetCore does not have the function .IsAjaxRequest so we took a similar function created online to do the same thing
        public static bool IsAjaxRequest(this HttpRequest request)
        {
            if (request == null)
                throw new ArgumentNullException(nameof(request));

            if (request.Headers != null)
                return request.Headers["X-Requested-With"] == "XMLHttpRequest";
            return false;
        }

        public static List<SelectListItem> TermsSelectList()
        {
            List<SelectListItem> termsSelectList = new List<SelectListItem>()
            {
                new SelectListItem() { Value="0", Text="Pay Now" },
                new SelectListItem() { Value="30", Text="30" },
                new SelectListItem() { Value="45", Text="45" },
                new SelectListItem() { Value="60", Text="60" }
            };
            //SelectList termsSelectList = new SelectList(dictSelectList);
            return termsSelectList;
        }

    }

}
