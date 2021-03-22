﻿using Microsoft.AspNetCore.Http;
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
using System.Globalization;
using System.IO;
using System.Linq;
using System.Net;
using System.Net.NetworkInformation;
using System.Net.Sockets;
using System.Text.RegularExpressions;
using System.Threading.Tasks;

namespace PrototypeWithAuth.AppData
{
    public static class AppUtility
    {
        public enum PriceSortEnum
        {
            Unit = 1, Total = 2,
            [Display(Name = "VAT")]
            Vat = 3,
            [Display(Name = "Total + VAT")]
            TotalVat = 4
        }
        public enum TermsModalEnum { PayNow, PayWithInMonth, Installments, Paid }
        public enum PageTypeEnum
        {
            None, RequestRequest, RequestInventory, RequestCart, RequestSearch, RequestLocation, RequestSummary,
            AccountingNotifications, AccountingGeneral, AccountingExpenses, AccountingSuppliers, AccountingPayments,
            LabManagementSuppliers, LabManagementLocations, LabManagementEquipment, LabManagementQuotes, LabManagementSearch,
            TimeKeeperReport, TimekeeperSummary,
            UsersUser, UsersWorkers,
            OperationsRequest, OperationsInventory, OperationsSearch,
            ExpensesSummary, ExpensesStatistics, ExpensesCost, ExpensesWorkers,

        }
        public enum SidebarEnum
        {
            None, Type, Vendors, Owner, Search, General, AllSuppliers, NewSupplier, Orders,
            Quotes, List, Calibrate, Location, Cart, Notifications,
            ReportHours, SummaryHours, ReportDaysOff, SummaryDaysOff, Documents, CompanyAbsences,
            PieCharts, Tables, Graphs, Project, Item, Worker,
            Category, Details, Hours, Salary,
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
            Add, AwaitingApproval,
            [Display(Name = "Specify Payment")]
            SpecifyPayment
        }
        public enum FilterEnum { None, Price, Category, Amount }
        public enum YearlyMonthlyEnum { Yearly, Monthly }
        public enum EntryExitEnum { Entry1, Exit1, Entry2, Exit2, None }
        public enum CommentTypeEnum { Warning, Comment }
        public enum TempDataTypes { MenuType, PageType, SidebarType }
        public enum RequestFolderNamesEnum { Orders, Invoices, Shipments, Quotes, Info, Pictures, Returns, Credits, More, Warranty, Manual, S, Map, Details } //Listed in the site.js (if you change here must change there)
        public enum MenuItems { Requests, Protocols, Operations, Biomarkers, TimeKeeper, LabManagement, Accounting, Reports, Income, Users }
        public enum RoleItems { Admin, CEO }
        public enum CurrencyEnum { NIS, USD }
        public enum PaymentsPopoverEnum
        {
            //Share,
            // Order,
            [Display(Name = "Monthly Payment")]
            MonthlyPayment = 2,
            [Display(Name = "Pay Now")]
            PayNow = 3,
            [Display(Name = "Pay Later")]
            PayLater = 4,
            Installments = 5,
            SpecifyPayment
        }
        public enum PaymentsEnum { ToPay, PayNow }
        public enum SuppliersEnum { All, NewSupplier, Search }
        public enum CategoryTypeEnum { Operations, Lab }
        public enum ParentCategoryEnum { Plastics, ReagentsAndChemicals, Proprietary, Reusables, Equipment, Operation, Cells }
        public enum RequestModalType { Create, Edit, Summary }
        public enum OrderTypeEnum { RequestPriceQuote, OrderNow, AddToCart, AskForPermission, AlreadyPurchased, Save, SaveOperations }
        public enum OffDayTypeEnum { VacationDay, SickDay, MaternityLeave }
        public static string GetDisplayNameOfEnumValue(string EnumValueName)
        {
            string[] splitEnumValue = Regex.Split(EnumValueName, @"(?<!^)(?=[A-Z])");
            return String.Join(' ', splitEnumValue);
        }
        public static int GetCountOfRequestsByRequestStatusIDVendorIDSubcategoryIDApplicationUserID(IQueryable<Request> RequestsList, int RequestStatusID, SidebarEnum sidebarType, String filterID)
        {
            int ReturnList = 0;
            int id = 0;
            if (sidebarType != SidebarEnum.Owner)
            {
                int.TryParse(filterID, out id);
            }
            switch (sidebarType)
            {
                case SidebarEnum.Vendors:
                    ReturnList = RequestsList
                   .Where(r => r.RequestStatusID == RequestStatusID)
                   .Where(r => r.Product.VendorID == id)
                   .Count();
                    break;
                case SidebarEnum.Type:
                    ReturnList = RequestsList
                    .Where(r => r.RequestStatusID == RequestStatusID)
                    .Where(r => r.Product.ProductSubcategoryID == id)
                    .Count();
                    break;
                case SidebarEnum.Owner:
                    ReturnList = RequestsList
                    .Where(r => r.RequestStatusID == RequestStatusID)
                    .Where(r => r.ApplicationUserCreatorID == filterID)
                    .Count();
                    break;
                default:
                    ReturnList = RequestsList.Where(r => r.RequestStatusID == RequestStatusID).Count();
                    break;
            }

            return ReturnList;
        }

        public static double ExchangeRateIfNull = 3.5;
        public static int YearStartedTimeKeeper = 2021;
        public static DateTime DateSoftwareLaunched = new DateTime(2021, 1, 1);
        public static decimal GetExchangeRateFromApi()
        {
            var client = new RestClient("http://api.currencylayer.com/live?access_key=8a8f7defe393388b7249ffcdb09d6a34");
            var request = new RestRequest(Method.GET);
            IRestResponse response = client.Execute(request);
            decimal rate = 0.0m;
            try
            {
                dynamic tmp = JsonConvert.DeserializeObject(response.Content);
                String stringRate = (string)tmp.quotes.USDILS;
                stringRate = stringRate.Replace("{", "");
                stringRate = stringRate.Replace("}", "");
                decimal.TryParse(stringRate, out rate);

                return Math.Round(rate, 3);
            }
            catch (Exception ex)
            {
                return 0.0m;
            }

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

        public static List<String> GetPriceColumn(List<String> priceFilterEnums, Request request, CurrencyEnum currency)
        {
            List<String> priceColumn = new List<String>();
            var currencyFormat = "he-IL";
            var pricePerUnit = request.PricePerUnit;
            var cost = request.Cost;
            var total = request.TotalWithVat;
            var vat = request.VAT;
            var exchangeRate = request.ExchangeRate;
            if (currency == AppUtility.CurrencyEnum.USD)
            {
                currencyFormat = "en-US";
                pricePerUnit = request.PricePerUnit / exchangeRate;
                cost = request.Cost / exchangeRate;
                total = request.TotalWithVat / exchangeRate;
                vat = request.VAT / exchangeRate;
            }
            foreach (var p in priceFilterEnums)
            {
                switch (Enum.Parse(typeof(PriceSortEnum), p))
                {
                    case PriceSortEnum.Unit:
                        priceColumn.Add("U: " + string.Format(new CultureInfo(currencyFormat), "{0:c}", pricePerUnit));
                        break;
                    case PriceSortEnum.Total:
                        priceColumn.Add("T: " + string.Format(new CultureInfo(currencyFormat), "{0:c}", cost));
                        break;
                    case PriceSortEnum.Vat:
                        priceColumn.Add("V: " + string.Format(new CultureInfo(currencyFormat), "{0:c}", vat));
                        break;
                    case PriceSortEnum.TotalVat:
                        priceColumn.Add("P: " + string.Format(new CultureInfo(currencyFormat), "{0:c}", total));
                        break;
                }
            }
            return priceColumn;
        }

        public static List<String> GetAmountColumn(Request request, UnitType unitType, UnitType subUnitType, UnitType subSubUnitType)
        {
            List<String> amountColumn = new List<String>();
            if (request.Unit != null)
            {
                amountColumn.Add(request.Unit + " " + unitType.UnitTypeDescription);
                if (request.SubUnit != null)
                {
                    amountColumn.Add(request.SubUnit + " " + subUnitType.UnitTypeDescription);
                    if (request.SubSubUnit != null)
                    {
                        amountColumn.Add(request.SubSubUnit + " " + subSubUnitType.UnitTypeDescription);
                    }

                }

            }
            return amountColumn;
        }
        public static string GetNote(SidebarEnum sidebarEnum, Request request)
        {
           if(sidebarEnum == SidebarEnum.PartialDelivery)
            {
                return request.NoteForPartialDelivery;
            }
            else
            {
                return request.NoteForClarifyDelivery;
            }
        }

        public static string GetDocumentIcon(RequestFolderNamesEnum folderName)
        {
            var iconClass = "";
            switch (folderName)
            {
                case RequestFolderNamesEnum.Quotes:
                    iconClass = "icon-centarix-icons-03";
                    break;
                case RequestFolderNamesEnum.Orders:
                    iconClass = "icon-chrome_reader_mode-24px";
                    break;
                case RequestFolderNamesEnum.Invoices:
                    iconClass = "icon-book-24px";
                    break;
                case RequestFolderNamesEnum.Shipments:
                    iconClass = "icon-local_shipping-24px";
                    break;
                case RequestFolderNamesEnum.Info:
                    iconClass = "icon-info-24px-2";
                    break;
                case RequestFolderNamesEnum.Pictures:
                    iconClass = "icon-camera_alt-24px";
                    break;
                case RequestFolderNamesEnum.Returns:
                    iconClass = "icon-remove_shopping_cart-24px";
                    break;
                case RequestFolderNamesEnum.Credits:
                    iconClass = "icon-insert_drive_file-24px-1";
                    break;
                case RequestFolderNamesEnum.S:
                    iconClass = "icon-chrome_reader_mode-24px";
                    break;
                case RequestFolderNamesEnum.Map:
                    iconClass = "icon-chrome_reader_mode-24px";
                    break;
            }
            return iconClass;
        }
        public static string GetEmployeeCentarixID(IEnumerable<CentarixID> centarixIDs)
        {
            string centarixID = "";
            foreach (var c in centarixIDs)
            {
                centarixID += c.CentarixIDNumber;
            }
            return centarixID;
        }
        public static double GetTotalWorkingDaysThisMonth(DateTime firstOfTheMonth, IQueryable<CompanyDayOff> companyDayOffs, double vacationSickCount)
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

        public static string GetMyIPAddress()
        {
            var myIpAddress = "";
            IPAddress ipAddress;
            var ipAddressList = Dns.GetHostEntry(Dns.GetHostName()).AddressList;
            foreach (var address in ipAddressList)
            {
                if (myIpAddress == "")
                {
                    if (IPAddress.TryParse(address.ToString(), out ipAddress))
                    {
                        switch (ipAddress.AddressFamily)
                        {
                            case AddressFamily.InterNetwork:
                                myIpAddress = address.ToString();
                                break;
                            default:
                                break;
                        }
                    }
                }
            }

            return myIpAddress;
        }

        public static string PhysicalAddress = NetworkInterface
                           .GetAllNetworkInterfaces()
                           .Where(nic => nic.OperationalStatus == OperationalStatus.Up && nic.NetworkInterfaceType != NetworkInterfaceType.Loopback)
                           .Select(nic => nic.GetPhysicalAddress().ToString())
                           .FirstOrDefault();

        public static string GetExceptionMessage(Exception ex)
        {
            if (ex.InnerException != null)
            {
                return ex.InnerException.Message;
            }
            else
            {
                return ex.Message;
            }
        }

        public static void DirectoryCopy(string sourceDirName, string destDirName, bool copySubDirs)
        {
            // Get the subdirectories for the specified directory.
            DirectoryInfo dir = new DirectoryInfo(sourceDirName);

            if (!dir.Exists)
            {
                throw new DirectoryNotFoundException(
                    "Source directory does not exist or could not be found: "
                    + sourceDirName);
            }

            DirectoryInfo[] dirs = dir.GetDirectories();
            if(dirs.Count()<=0)
            {
                return;
            }
            // If the destination directory doesn't exist, create it.       
            Directory.CreateDirectory(destDirName);

            // Get the files in the directory and copy them to the new location.
            FileInfo[] files = dir.GetFiles();
            foreach (FileInfo file in files)
            {
                string tempPath = Path.Combine(destDirName, file.Name);
                file.CopyTo(tempPath, false);
            }

            // If copying subdirectories, copy them and their contents to new location.
            if (copySubDirs)
            {
                foreach (DirectoryInfo subdir in dirs)
                {
                    string tempPath = Path.Combine(destDirName, subdir.Name);
                    DirectoryCopy(subdir.FullName, tempPath, copySubDirs);
                }
            }
        }
    }
}
