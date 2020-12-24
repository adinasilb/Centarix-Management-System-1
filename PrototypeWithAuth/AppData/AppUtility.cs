using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Newtonsoft.Json;
using PrototypeWithAuth.AppData.UtilityModels;
using PrototypeWithAuth.Models;
using PrototypeWithAuth.ViewModels;
using RestSharp;
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
        public enum PriceSortEnum {Unit=1, Total=2,
            [Display(Name = "VAT")] 
            Vat=3,
            [Display(Name = "Total + VAT")]
            TotalVat=4
        }
        public enum PageTypeEnum {None, RequestRequest, RequestInventory, RequestCart, RequestSearch, RequestLocation, RequestSummary, 
            AccountingNotifications, AccountingGeneral, AccountingExpenses, AccountingSuppliers, AccountingPayments, 
            LabManagementSuppliers, LabManagementLocations, LabManagementEquipment, LabManagementQuotes, LabManagementSearch,
            TimeKeeperReport, TimekeeperSummary, 
            UsersUser, UsersWorkers,
            OperationsRequest, OperationsInventory, OperationsSearch,
            ExpensesSummary, ExpensesStatistics, ExpensesCost, ExpensesWorkers,

        }
        public enum SidebarEnum {
            None, LastItem, Type, Vendors, Owner, Search, General, AllSuppliers, NewSupplier, Orders,
            Quotes, List,  Calibrate, Categories,  Location, Cart, Notifications,
            ReportHours, SummaryHours, ReportDaysOff, SummaryDaysOff, Documents, CompanyAbsences,
            PieCharts, Tables, Graphs, Project, Item, Worker, 
            Category,  Details, Hours, Salary, 
            [Display(Name = "Monthly Payment")]
            MonthlyPayment,
            [Display(Name = "Pay Now")]
            PayNow,
            [Display(Name = "Pay Later")]
            PayLater, Installments,
            [Display(Name = "Standing Orders")]
            StandingOrders, 
            [Display(Name = "No Invoice")]
            NoInvoice,
            [Display(Name = "Didnt Arrive")]
            DidntArrive,
            [Display(Name = "Partial Delivery")]
            PartialDelivery,
            [Display(Name = "For Clarification")]
            ForClarification,
            Add,  AwaitingApproval,
        }
        public enum YearlyMonthlyEnum { Yearly, Monthly }
        public enum EntryExitEnum { Entry1, Exit1, Entry2, Exit2, None }
        public enum CommentTypeEnum { Warning, Comment }
        public enum TempDataTypes { MenuType, PageType, SidebarType }
        public enum RequestFolderNamesEnum { Orders, Invoices, Shipments, Quotes, Info, Pictures, Returns, Credits, More, Warranty, Manual } //Listed in the site.js (if you change here must change there)
        public enum MenuItems { Requests, Protocols, Operations, Biomarkers, TimeKeeper, LabManagement, Accounting, Reports, Income, Users }
        public enum RoleItems { Admin, CEO }
        public enum CurrencyEnum { USD, NIS }
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

        public static double ExchangeRateIfNull = 3.5;
        public static double _GetExchangeRateFromApi = GetExchangeRateFromApi();
        public static double GetExchangeRateFromApi()
        {
            var client = new RestClient("https://v6.exchangerate-api.com/v6/96ffcdbcf4b24b1bdf2dc9be/latest/USD");
            var request = new RestRequest(Method.GET);
            IRestResponse response = client.Execute(request);
            double rate=0.0;
            dynamic tmp = JsonConvert.DeserializeObject(response.Content);
            String stringRate = (string)tmp.conversion_rates.ILS;
            stringRate = stringRate.Replace("{", "");
            stringRate = stringRate.Replace("}", "");
            Double.TryParse(stringRate, out rate);
            return Math.Round(rate, 2);
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

        public static List<AccountingPopoverLink> GetPaymentsPopoverLinks(AppUtility.SidebarEnum CurrentEnum)
        {
            List<AccountingPopoverLink> list = new List<AccountingPopoverLink>();
            List<PaymentsPopoverEnum> enums = Enum.GetValues(typeof(PaymentsPopoverEnum)).Cast<PaymentsPopoverEnum>().ToList();
            if (!CurrentEnum.Equals(AppUtility.SidebarEnum.StandingOrders.ToString()))
            {
                foreach (PaymentsPopoverEnum e in enums)
                {

                    if (CurrentEnum.ToString() != e.ToString() && CurrentEnum != AppUtility.SidebarEnum.None)
                    {
                        AccountingPopoverLink accountingPopoverLink = new AccountingPopoverLink();
                        accountingPopoverLink.CurrentLocation = (PaymentsPopoverEnum)Enum.Parse(typeof(PaymentsPopoverEnum), CurrentEnum.ToString());
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
                                accountingPopoverLink.Icon = "icon-monetization_on-24px";
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

            }
        

            return list;
        }
        public static int GetTotalWorkingDaysThisMonth(DateTime firstOfTheMonth, IQueryable<CompanyDayOff> companyDayOffs, int vacationSickCount)
        {
            DateTime nextDay = firstOfTheMonth;
            DateTime endOfTheMonth = firstOfTheMonth.AddMonths(1);
            int totalDays = 0;
            int companyDaysOffCount = companyDayOffs.Where(d => d.Date.Year == firstOfTheMonth.Year && firstOfTheMonth.Month == d.Date.Month).Count();
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
            DateTime endofTheYear = firstOfTheYear.AddYears(1);
            int totalDays = 0;
            int companyDaysOffCount = companyDayOffs.Where(d => d.Date.Year == firstOfTheYear.Year).Count();
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

        public static List<String> GetChartColors()
        {
            return new List<string> { "#00BCD4", "#3F51B5", "#009688", "#607D8B",  "#FF9800", "#F44336", "#795548", "#673AB7", "#9E9E9E", "#4CAF50", "#2196F3",
 "#FFCDD2" , "#E91E63", "#9C27B0",
           "#03A9F4", "#8BC34A", "#CDDC39",
                "#FF5722",     "#FFEB3B", "#FFC107",};
        }
        public static string GetChartUnderZeroColor()
        {
            return "#000000";
        }
    }

}
