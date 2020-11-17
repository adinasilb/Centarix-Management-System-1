using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using PrototypeWithAuth.AppData.UtilityModels;
using PrototypeWithAuth.Models;
using PrototypeWithAuth.ViewModels;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
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
        public enum LabManagementSidebarEnum { None, Orders, Quotes, AllSuppliers, NewSupplier, SearchSupplier, LocationsList, SearchRequests, SearchEquipment, Calibrate, EquipmentList, EquipmentCategories }
        public enum OrdersAndInventorySidebarEnum { None, LastItem, Type, Vendor, Owner, Location, Cart, AddItem, Notifications }
        public enum RequestFolderNamesEnum { Orders, Invoices, Shipments, Quotes, Info, Pictures, Returns, Credits, More, Warranty, Manual } //Listed in the site.js (if you change here must change there)
        public enum UserPageTypeEnum { None, User, Workers }
        public enum UserSideBarEnum { UsersList, UsersAdd, WorkersDetails, WorkersHours, WorkersSalary, WorkersAwaitingApproval, AddWorker }
        public enum OperationsPageTypeEnum { RequestOperations, InventoryOperations, SearchOperations }
        public enum OperationsSidebarEnum { LastItem, AddItem, Type, Vendors, Owner, Search }
        public enum TimeKeeperPageTypeEnum { None, Report, TimekeeperSummary }
        public enum TimeKeeperSidebarEnum { ReportHours, SummaryHours, ReportDaysOff, SummaryDaysOff, Documents, CompanyAbsences }
        public enum ExpensesPageTypeEnum { ExpensesSummary, ExpensesStatistics, ExpensesCost, ExpensesWorkers }
        public enum ExpensesSidebarEnum { SummaryPieCharts, SummaryTables, SummaryGraphs, StatisticsProject, StatisticsItem, StatisticsWorker, StatisticsCategory, StatisticsVendor, CostsProject, CostsAdvancedSearch, CostsAdvancedLists, WorkersDetails, WorkersHours, WorkersSalary }
        public enum MenuItems { OrdersAndInventory, Protocols, Operation, Biomarkers, TimeKeeper, LabManagement, Accounting, Reports, Income, Users }
        public enum RoleItems { Admin, CEO }
        public enum CurrencyEnum { USD, NIS }
        public enum AccountingNotificationsEnum
        {
            [Display(Name = "No Invoice")]
            NoInvoice,
            [Display(Name = "Didnt Arrive")]
            DidntArrive,
            [Display(Name = "Partial Delivery")]
            PartialDelivery,
            [Display(Name = "For Clarification")]
            ForClarification
        }
        public enum AccountingPaymentsEnum
        {
            [Display(Name = "Monthly Payment")]
            MonthlyPayment,
            [Display(Name = "Pay Now")]
            PayNow,
            [Display(Name = "Pay Later")]
            PayLater, Installments,
            [Display(Name = "Standing Orders")]
            StandingOrders
        }
        public enum PaymentsPopoverEnum
        {
            //Share,
            // Order,
            [Display(Name = "Monthly Payment")]
            MonthlyPayment = 1,
            [Display(Name = "Pay Now")]
            PayNow = 3,
            [Display(Name = "Pay Later")]
            PayLater = 4,
            Installments = 5,
            // Clarification
        }
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

        public static string GetLastFiles(string longFileName, int amountOfFiles)
        {
            bool lastfound = false;
            int counter = 0;
            int place = longFileName.Length - 1;
            while (!lastfound)
            {
                string ch = longFileName.Substring(place, 1);
                if (ch == "\\")
                {
                    counter++;
                    if (counter == amountOfFiles)
                    {
                        lastfound = true;
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

        public static List<AccountingPopoverLink> GetPaymentsPopoverLinks(String CurrentEnum)
        {
            List<AccountingPopoverLink> list = new List<AccountingPopoverLink>();
            var enums = Enum.GetValues(typeof(PaymentsPopoverEnum)).Cast<PaymentsPopoverEnum>().ToList();

            foreach (var e in enums)
            {

                if (CurrentEnum != e.ToString())
                {
                    AccountingPopoverLink accountingPopoverLink = new AccountingPopoverLink();
                    accountingPopoverLink.CurrentLocation = (PaymentsPopoverEnum)Enum.Parse(typeof(PaymentsPopoverEnum), CurrentEnum);
                    accountingPopoverLink.Description = e;
                    switch (e)
                    {
                        //case PaymentsPopoverEnum.Share:
                        //    accountingPopoverLink.Action = "AccountingPayments";
                        //    accountingPopoverLink.Controller = "Requests";
                        //    accountingPopoverLink.Color = "#30BCC9";
                        //    accountingPopoverLink.Icon = "icon-share-24px-1";
                        //    break;
                        //case PaymentsPopoverEnum.Order:
                        //    accountingPopoverLink.Action = "ChangePaymentStatus";
                        //    accountingPopoverLink.Controller = "Requests";
                        //    accountingPopoverLink.Color = "#00CA72";
                        //    accountingPopoverLink.Icon = "icon-add_circle_outline-24px1";
                        //    break;
                        case PaymentsPopoverEnum.MonthlyPayment:
                            accountingPopoverLink.Action = "ChangePaymentStatus";
                            accountingPopoverLink.Controller = "Requests";
                            accountingPopoverLink.Color = "#90C939";
                            accountingPopoverLink.Icon = "icon-monetization_on-24px1";
                            break;
                        case PaymentsPopoverEnum.PayNow:
                            accountingPopoverLink.Action = "ChangePaymentStatus";
                            accountingPopoverLink.Controller = "Requests";
                            accountingPopoverLink.Color = "#D5A522";
                            accountingPopoverLink.Icon = "icon-credit_card-24px";
                            break;
                        case PaymentsPopoverEnum.PayLater:
                            accountingPopoverLink.Action = "ChangePaymentStatus";
                            accountingPopoverLink.Controller = "Requests";
                            accountingPopoverLink.Color = "#5F79E2";
                            accountingPopoverLink.Icon = "icon-centarix-icons-19";
                            break;
                        case PaymentsPopoverEnum.Installments:
                            accountingPopoverLink.Action = "ChangePaymentStatus";
                            accountingPopoverLink.Controller = "Requests";
                            accountingPopoverLink.Color = "#7D9BAA";
                            accountingPopoverLink.Icon = "icon-centarix-icons-20";
                            break;
                            //case PaymentsPopoverEnum.Clarification:
                            //    accountingPopoverLink.Action = "ChangePaymentStatus";
                            //    accountingPopoverLink.Controller = "Requests";
                            //    accountingPopoverLink.Color = "#E27933";
                            //    accountingPopoverLink.Icon = "icon-report_problem-24px-2";
                            //    break;
                    }
                    list.Add(accountingPopoverLink);
                }
            }

            return list;
        }
        public static int GetTotalWorkingDaysThisMonth(DateTime firstOfTheMonth, IQueryable<CompanyDayOff> companyDayOffs, int vacationSickCount)
        {
            DateTime nextDay = firstOfTheMonth;
            var endOfTheMonth = firstOfTheMonth.AddMonths(1);
            int totalDays = 0;
            var companyDaysOffCount = companyDayOffs.Where(d => d.Date.Year == firstOfTheMonth.Year && firstOfTheMonth.Month == d.Date.Month).Count();
            while (nextDay.Date < endOfTheMonth)
            {
                if (nextDay.DayOfWeek != DayOfWeek.Friday && nextDay.DayOfWeek != DayOfWeek.Saturday)
                {
                    totalDays += 1;
                }
                nextDay = nextDay.AddDays(1);
            }

            return totalDays - vacationSickCount - companyDaysOffCount;
        }

        public static int GetTotalWorkingDaysThisYear(DateTime firstOfTheYear, IQueryable<CompanyDayOff> companyDayOffs, int vacationSickCount)
        {
            DateTime nextDay = firstOfTheYear;
            var endofTheYear = firstOfTheYear.AddYears(1);
            int totalDays = 0;
            var companyDaysOffCount = companyDayOffs.Where(d => d.Date.Year == firstOfTheYear.Year).Count();
            while (nextDay.Date < endofTheYear)
            {
                if (nextDay.DayOfWeek != DayOfWeek.Friday && nextDay.DayOfWeek != DayOfWeek.Saturday)
                {
                    totalDays += 1;
                }
                nextDay = nextDay.AddDays(1);
            }

            return totalDays - vacationSickCount - companyDaysOffCount;
        }

    }

}
