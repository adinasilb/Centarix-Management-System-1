using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
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
        public enum YearlyMonthlyEnum { Yearly, Monthly }
        public enum EntryExitEnum { Entry1, Exit1, Entry2, Exit2, None }
        public enum CommentTypeEnum { Warning, Comment }
        public enum TempDataTypes { MenuType, PageType, SidebarType }
        public enum RequestPageTypeEnum { None, Request, Inventory, Cart, Search, Location, Summary }
        public enum PaymentPageTypeEnum { None, Notifications, General, Expenses, SuppliersAC, Payments } //these are all going to the ParentRequestIndex
        public enum AccountingSidebarEnum { General, AllSuppliersAC, NewSupplierAC, SearchSupplierAC }
        public enum LabManagementPageTypeEnum { None, Suppliers, Locations, Equipment, Quotes, SearchLM }
        public enum LabManagementSidebarEnum { None, Orders, Quotes, AllSuppliers, NewSupplier, SearchSupplier, LocationsList, SearchRequests }
        public enum OrdersAndInventorySidebarEnum { None, LastItem, Type, Vendor, Owner, Location, Cart, AddItem, Notifications }
        public enum RequestFolderNamesEnum { Orders, Invoices, Shipments, Quotes, Info, Pictures, Returns, Credits } //Listed in the site.js (if you change here must change there)
        public enum UserPageTypeEnum { None, User, Workers }
        public enum UserSideBarEnum { None, Index, Add, Details, AddWorker, AwaitingApproval, Salary, Hours }
        public enum OperationsPageTypeEnum { RequestOperations, InventoryOperations, SearchOperations }
        public enum OperationsSidebarEnum { LastItem, AddItem, Type, Vendors, Owner, Search }
        public enum TimeKeeperPageTypeEnum { None, Report, Summary }
        public enum TimeKeeperSidebarEnum { None, ReportHours, Days, Hours, DaysOff, Documents, CompanyAbsences }
        public enum MenuItems { Admin, OrdersAndInventory, Protocols, Operation, Biomarkers, TimeKeeper, LabManagement, Accounting, Expenses, Income, Users }
        public enum AccountingNotificationsEnum { NoInvoice, DidntArrive, PartialDelivery, ForClarification }
        public enum AccountingPaymentsEnum { MonthlyPayment, PayNow, PayLater, Installments, StandingOrders }
        public enum PaymentsEnum { ToPay, PayNow }
        public enum SuppliersEnum { All, NewSupplier, Search }
        public enum CategoryTypeEnum { Operations, Lab }
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
                new SelectListItem() { Value="-1", Text="Paid" },
                new SelectListItem() { Value="30", Text="30" },
                new SelectListItem() { Value="45", Text="45" },
                new SelectListItem() { Value="60", Text="60" }
            };
            //SelectList termsSelectList = new SelectList(dictSelectList);
            return termsSelectList;
        }

        public static string GetLastFourFiles(string longFileName)
        {
            bool fourthFound = false;
            int counter = 0;
            int place = longFileName.Length - 1;
            while (!fourthFound)
            {
                string ch = longFileName.Substring(place, 1);
                if (ch == "\\")
                {
                    counter++;
                    if (counter == 4)
                    {
                        fourthFound = true;
                    }
                }
                place--;
            }
            string newFileName = longFileName.Substring(place + 2, longFileName.Length - place - 2);
            return newFileName;
        }


        public static DateTime ZeroSeconds(this DateTime value)
        {
            return new DateTime(value.Year, value.Month, value.Day,
              value.Hour, value.Minute, 0);
        }

    }

}
