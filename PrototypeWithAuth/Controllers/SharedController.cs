using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using PrototypeWithAuth.AppData;
using PrototypeWithAuth.AppData.UtilityModels;
using PrototypeWithAuth.Data;
using PrototypeWithAuth.Models;
using PrototypeWithAuth.ViewModels;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.IO;
using System.Linq;
using System.Threading.Tasks;
using X.PagedList;

namespace PrototypeWithAuth.Controllers
{
    public class SharedController : Controller
    {
        private readonly ApplicationDbContext _context;
        private readonly IHostingEnvironment _hostingEnvironment;
        private readonly UserManager<ApplicationUser> _userManager;
        protected string AccessDeniedPath = "~/Identity/Account/AccessDenied";
        protected SharedController(ApplicationDbContext context, UserManager<ApplicationUser> userManager = null, IHostingEnvironment hostingEnvironment = null)
        {
            _context = context;
            _hostingEnvironment = hostingEnvironment;
            _userManager = userManager;
        }

        protected async Task<bool> IsAuthorizedAsync(AppUtility.MenuItems SectionType)
        {
            var user = await _userManager.GetUserAsync(User);
            bool Allowed = false;
            if (
                (SectionType.Equals(AppUtility.MenuItems.Requests) && await _userManager.IsInRoleAsync(user, AppUtility.MenuItems.Requests.ToString()))/*User.IsInRole(AppUtility.MenuItems.Requests.ToString()))*/ ||
                (SectionType.Equals(AppUtility.MenuItems.Protocols) && await _userManager.IsInRoleAsync(user, AppUtility.MenuItems.Protocols.ToString())) ||
                (SectionType.Equals(AppUtility.MenuItems.Operations) && await _userManager.IsInRoleAsync(user, AppUtility.MenuItems.Operations.ToString())) ||
                (SectionType.Equals(AppUtility.MenuItems.Biomarkers) && await _userManager.IsInRoleAsync(user, AppUtility.MenuItems.Biomarkers.ToString())) ||
                (SectionType.Equals(AppUtility.MenuItems.TimeKeeper) && await _userManager.IsInRoleAsync(user, AppUtility.MenuItems.TimeKeeper.ToString())) ||
                (SectionType.Equals(AppUtility.MenuItems.LabManagement) && await _userManager.IsInRoleAsync(user, AppUtility.MenuItems.LabManagement.ToString())) ||
                (SectionType.Equals(AppUtility.MenuItems.Accounting) && await _userManager.IsInRoleAsync(user, AppUtility.MenuItems.Accounting.ToString())) ||
                (SectionType.Equals(AppUtility.MenuItems.Reports) && await _userManager.IsInRoleAsync(user, AppUtility.MenuItems.Reports.ToString())) ||
                (SectionType.Equals(AppUtility.MenuItems.Income) && await _userManager.IsInRoleAsync(user, AppUtility.MenuItems.Income.ToString())) ||
                (SectionType.Equals(AppUtility.MenuItems.Users) && await _userManager.IsInRoleAsync(user, AppUtility.MenuItems.Users.ToString()))
                )
            {
                Allowed = true;
            }
            return Allowed;
        }
        private List<EmployeeHoursAndAwaitingApprovalViewModel> GetHours(int year, int month, Employee user)
        {
            var hours = _context.EmployeeHours.Include(eh => eh.OffDayType).Include(eh => eh.EmployeeHoursStatusEntry1)
               .Include(eh => eh.CompanyDayOff).ThenInclude(cdo => cdo.CompanyDayOffType)
               .Include(eh => eh.PartialOffDayType).Where(eh => eh.EmployeeID == user.Id)
               .Where(eh => eh.Date.Month == month && eh.Date.Year == year && eh.Date.Date <= DateTime.Now.Date)
               .OrderByDescending(eh => eh.Date).ToList();

            List<EmployeeHoursAndAwaitingApprovalViewModel> hoursList = new List<EmployeeHoursAndAwaitingApprovalViewModel>();
            foreach (var hour in hours)
            {
                var ehaaavm = new EmployeeHoursAndAwaitingApprovalViewModel()
                {
                    EmployeeHours = hour
                };
                var eha = _context.EmployeeHoursAwaitingApprovals.Where(ehaa => ehaa.EmployeeHoursID == hour.EmployeeHoursID).FirstOrDefault();
                if (eha != null)
                {
                    ehaaavm.EmployeeHoursAwaitingApproval = eha;
                }
                hoursList.Add(ehaaavm);
            }
            return hoursList;
        }
        protected SummaryHoursViewModel SummaryHoursFunction(int month, int year, Employee user, string errorMessage = null)
        {
            var hours = GetHours(year, month, user);
            var CurMonth = new DateTime(year, month, 1);
            double? totalhours;
            double totalDays = 0;
            double workingDays = 0;
            double vacationDaysTaken = 0;
            double sickDaysTaken = 0;
            var companyDaysOff = _context.CompanyDayOffs.Include(co => co.CompanyDayOffType).ToList();
            var employeeHours = _context.EmployeeHours.Where(eh => eh.EmployeeID == user.Id);
            if (user.EmployeeStatusID != 1)
            {
                totalhours = null;
            }
            else
            {
                var sickHours = employeeHours.Where(eh => eh.Date.Year == year && eh.PartialOffDayTypeID == 1 && eh.Date <= DateTime.Now.Date && eh.Date.Month == month).Select(eh => (eh.PartialOffDayHours == null ? TimeSpan.Zero : ((TimeSpan)eh.PartialOffDayHours)).TotalHours).ToList().Sum(p => p);
                sickDaysTaken = employeeHours.Where(eh => eh.Date.Month == month && eh.Date.Year == year && eh.OffDayTypeID == 1 && eh.Date <= DateTime.Now.Date).Count();
                sickDaysTaken = Math.Round(sickDaysTaken + (sickHours / user.SalariedEmployee.HoursPerDay), 2);

                var vacationHours = employeeHours.Where(eh => eh.Date.Year == year && eh.PartialOffDayTypeID == 2 && eh.Date <= DateTime.Now.Date && eh.Date.Month == month).Select(eh => (eh.PartialOffDayHours == null ? TimeSpan.Zero : ((TimeSpan)eh.PartialOffDayHours)).TotalHours).ToList().Sum(p => p);
                vacationDaysTaken = employeeHours.Where(eh => eh.Date.Year == year && eh.OffDayTypeID == 2 && eh.Date <= DateTime.Now.Date && eh.Date.Month == month).Count();
                vacationDaysTaken = Math.Round(vacationDaysTaken + (vacationHours / user.SalariedEmployee.HoursPerDay), 2);
                var specialDays = employeeHours.Where(eh => eh.OffDayTypeID == 4).Count();
                var unpaidLeave = employeeHours.Where(eh => eh.OffDayTypeID == 5).Count();
                totalDays = GetTotalWorkingDaysThisMonth(new DateTime(year, month, 1), companyDaysOff);
                totalhours = (totalDays - (vacationDaysTaken + sickDaysTaken + unpaidLeave + specialDays)) * user.SalariedEmployee.HoursPerDay;
                workingDays = user.EmployeeHours.Where(eh => (eh.OffDayTypeID == null) || (eh.IsBonus && eh.OffDayTypeID != null)).Where(eh => (eh.Exit1 != null || eh.TotalHours != null)
                && eh.Date.Date < DateTime.Now.Date).Count();
                workingDays = workingDays - Math.Round((sickHours + vacationHours) / user.SalariedEmployee?.HoursPerDay ?? 1, 2);
            }
            SummaryHoursViewModel summaryHoursViewModel = new SummaryHoursViewModel()
            {
                EmployeeHours = hours,
                CurrentMonth = CurMonth,
                TotalHoursInMonth = totalhours,
                SelectedYear = year,
                TotalHolidaysInMonth = _context.CompanyDayOffs.Where(cdo => cdo.Date.Year == year && cdo.Date.Month == month).Count(),
                VacationDayInThisMonth = vacationDaysTaken,
                SickDayInThisMonth = sickDaysTaken,
                User = user,
                TotalWorkingDaysInThisMonth = totalDays,
                WorkingDays = workingDays
            };
            if (errorMessage != null)
            {
                summaryHoursViewModel.ErrorMessage += errorMessage;
            }
            return summaryHoursViewModel;
        }
        protected double GetUsersOffDaysLeft(Employee user, int offDayTypeID, int thisYear)
        {
            var year = AppUtility.YearStartedTimeKeeper;
            var offDaysLeft = 0.0;
            while (year <= thisYear)
            {
                double vacationDays = 0;
                double sickDays = 0;
                double offDaysTaken = _context.EmployeeHours.Where(eh => eh.EmployeeID == user.Id && eh.Date.Year == year && eh.OffDayTypeID == offDayTypeID && eh.Date <= DateTime.Now.Date && eh.IsBonus == false).Count();
                if (user.EmployeeStatusID == 1 && offDayTypeID == 2)
                {
                    var vacationHours = _context.EmployeeHours.Where(eh => eh.EmployeeID == user.Id && eh.Date.Year == year && eh.PartialOffDayTypeID == 2 && eh.Date <= DateTime.Now.Date && eh.IsBonus == false).Select(eh => (eh.PartialOffDayHours == null ? TimeSpan.Zero : ((TimeSpan)eh.PartialOffDayHours)).TotalHours).ToList().Sum(p => p);
                    offDaysTaken = Math.Round(offDaysTaken + (vacationHours / user.SalariedEmployee.HoursPerDay), 2);
                }
                if (year == AppUtility.YearStartedTimeKeeper && year == DateTime.Now.Year)
                {
                    int month = DateTime.Now.Month;
                    vacationDays = (user.VacationDaysPerMonth * month) + user.RollOverVacationDays;
                    sickDays = (user.SickDaysPerMonth * month) + user.RollOverSickDays;
                }
                else if (year == AppUtility.YearStartedTimeKeeper)
                {
                    int month = DateTime.Now.Month;
                    vacationDays = user.VacationDays + user.RollOverVacationDays;
                    sickDays = user.SickDays + user.RollOverSickDays;
                }
                else if (year == user.StartedWorking.Year)
                {
                    int month = 12 - user.StartedWorking.Month + 1;
                    vacationDays = user.VacationDaysPerMonth * month;
                    sickDays = user.SickDays * month;
                }
                else if (year == DateTime.Now.Year)
                {
                    int month = DateTime.Now.Month;
                    vacationDays = (user.VacationDaysPerMonth * month);
                    sickDays = (user.SickDaysPerMonth * month);
                }
                else
                {
                    vacationDays = user.VacationDays;
                    sickDays = user.SickDays;
                }
                if (offDayTypeID == 2)
                {
                    offDaysLeft += vacationDays - offDaysTaken;
                }
                else
                {
                    offDaysLeft += sickDays - offDaysTaken;
                }
                year = year + 1;
            }
            return offDaysLeft;
        }

            protected void RemoveRequestWithCommentsAndEmailSessions()
        {
            var requiredKeys = HttpContext.Session.Keys.Where(x => x.StartsWith(AppData.SessionExtensions.SessionNames.Request.ToString()) ||
                x.StartsWith(AppData.SessionExtensions.SessionNames.Comment.ToString()) ||
                 x.StartsWith(AppData.SessionExtensions.SessionNames.Email.ToString()));
            foreach (var k in requiredKeys)
            {
                HttpContext.Session.Remove(k); //will clear the session for the future
            }

        }
        protected decimal GetExchangeRate()
        {
            decimal rate;
            var parsed = decimal.TryParse(_context.GlobalInfos.Where(gi => gi.GlobalInfoType == AppUtility.GlobalInfoType.ExchangeRate.ToString()).Select(er => er.Description).FirstOrDefault(), out rate);
            if (!parsed)
            {
                rate = 0;
            }
            return rate;
        }

        protected void GetExistingFileStrings(List<DocumentFolder> DocumentsInfo, AppUtility.FolderNamesEnum folderName, string uploadFolderParent)
        {
            string uploadFolder = Path.Combine(uploadFolderParent, folderName.ToString());
            DocumentFolder folder = new DocumentFolder()
            {
                FolderName = folderName
            };
            if (Directory.Exists(uploadFolder))
            {
                DirectoryInfo DirectoryToSearch = new DirectoryInfo(uploadFolder);
                //searching for the partial file name in the directory
                FileInfo[] orderfilesfound = DirectoryToSearch.GetFiles("*.*");
                folder.FileStrings = new List<string>();
                foreach (var orderfile in orderfilesfound)
                {
                    string newFileString = AppUtility.GetLastFiles(orderfile.FullName, 4);
                    folder.FileStrings.Add(newFileString);
                }
            }
            folder.Icon = AppUtility.GetDocumentIcon(folderName);

            DocumentsInfo.Add(folder);
        }
        protected virtual void DocumentsModal(DocumentsModalViewModel documentsModalViewModel)
        {
            string uploadFolder = Path.Combine(_hostingEnvironment.WebRootPath, documentsModalViewModel.ParentFolderName.ToString());
            string folder = Path.Combine(uploadFolder, documentsModalViewModel.ObjectID.ToString());
            Directory.CreateDirectory(folder);
            if (documentsModalViewModel.FilesToSave != null) //test for more than one???
            {
                var x = 1;
                foreach (IFormFile file in documentsModalViewModel.FilesToSave)
                {
                    //create file
                    string folderPath = Path.Combine(folder, documentsModalViewModel.FolderName.ToString());
                    Directory.CreateDirectory(folderPath);
                    string uniqueFileName = x + file.FileName;
                    string filePath = Path.Combine(folderPath, uniqueFileName);
                    FileStream filestream = new FileStream(filePath, FileMode.Create);
                    file.CopyTo(filestream);
                    filestream.Close();
                    x++;
                }
            }
        }
        protected void DeleteTemporaryDocuments(AppUtility.ParentFolderName parentFolderName, int ObjectID = 0)
        {
            string uploadFolder = Path.Combine(_hostingEnvironment.WebRootPath, parentFolderName.ToString());
            string requestFolder = Path.Combine(uploadFolder, ObjectID.ToString());

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
        }
        protected void FillDocumentsViewModel(DocumentsModalViewModel documentsModalViewModel)
        {
            string uploadFolder1 = Path.Combine(_hostingEnvironment.WebRootPath, documentsModalViewModel.ParentFolderName.ToString());
            string uploadFolder2 = Path.Combine(uploadFolder1, documentsModalViewModel.ObjectID.ToString());
            string uploadFolder3 = Path.Combine(uploadFolder2, documentsModalViewModel.FolderName.ToString());

            if (Directory.Exists(uploadFolder3))
            {
                DirectoryInfo DirectoryToSearch = new DirectoryInfo(uploadFolder3);
                //searching for the partial file name in the directory
                FileInfo[] docfilesfound = DirectoryToSearch.GetFiles("*.*");
                documentsModalViewModel.FileStrings = new List<String>();
                documentsModalViewModel.FileStrings = new List<String>();
                foreach (var docfile in docfilesfound)
                {
                    string newFileString = AppUtility.GetLastFiles(docfile.FullName, 4);
                    documentsModalViewModel.FileStrings.Add(newFileString);
                    //documentsModalViewModel.Files.Add(docfile);
                }
            }

        }


        public ShareModalViewModel GetShareModalViewModel(int ID, AppUtility.ModelsEnum ModelsEnum)
        {
            ShareModalViewModel shareModalViewModel = new ShareModalViewModel()
            {
                ID = ID,
                ModelsEnum = ModelsEnum,
                ApplicationUsers = _context.Users
                              .Where(u => u.Id != _userManager.GetUserId(User))
                              .Select(
                                  u => new SelectListItem
                                  {
                                      Text = u.FirstName + " " + u.LastName,
                                      Value = u.Id
                                  }
                              ).ToList()
            };

            return shareModalViewModel;
        }

         protected void FillDocumentsInfo(RequestItemViewModel requestItemViewModel, string uploadFolder, ProductSubcategory productSubcategory)
        {
            requestItemViewModel.DocumentsInfo = new List<DocumentFolder>();

            if (productSubcategory.ParentCategory.IsProprietary)
            {
                if (productSubcategory.ProductSubcategoryDescription == "Blood" || productSubcategory.ProductSubcategoryDescription == "Serum")
                {
                    GetExistingFileStrings(requestItemViewModel.DocumentsInfo, AppUtility.FolderNamesEnum.S, uploadFolder);

                }
                if (productSubcategory.ProductSubcategoryDescription != "Blood" && productSubcategory.ProductSubcategoryDescription != "Serum"
                    && productSubcategory.ProductSubcategoryDescription != "Cells")
                {
                    GetExistingFileStrings(requestItemViewModel.DocumentsInfo, AppUtility.FolderNamesEnum.Info, uploadFolder);

                }
                if (productSubcategory.ProductSubcategoryDescription != "Blood" && productSubcategory.ProductSubcategoryDescription != "Serum"
                    && productSubcategory.ProductSubcategoryDescription != "Cells" && productSubcategory.ProductSubcategoryDescription != "Probes")
                {
                    GetExistingFileStrings(requestItemViewModel.DocumentsInfo, AppUtility.FolderNamesEnum.Map, uploadFolder);

                }
            }
            else if (requestItemViewModel.ParentCategories.FirstOrDefault().CategoryTypeID == 2)
            {
                GetExistingFileStrings(requestItemViewModel.DocumentsInfo, AppUtility.FolderNamesEnum.Orders, uploadFolder);
                GetExistingFileStrings(requestItemViewModel.DocumentsInfo, AppUtility.FolderNamesEnum.Invoices, uploadFolder);
                GetExistingFileStrings(requestItemViewModel.DocumentsInfo, AppUtility.FolderNamesEnum.Details, uploadFolder);
                GetExistingFileStrings(requestItemViewModel.DocumentsInfo, AppUtility.FolderNamesEnum.Quotes, uploadFolder);
            }
            else
            {
                GetExistingFileStrings(requestItemViewModel.DocumentsInfo, AppUtility.FolderNamesEnum.Quotes, uploadFolder);
                GetExistingFileStrings(requestItemViewModel.DocumentsInfo, AppUtility.FolderNamesEnum.Orders, uploadFolder);
                GetExistingFileStrings(requestItemViewModel.DocumentsInfo, AppUtility.FolderNamesEnum.Invoices, uploadFolder);
                GetExistingFileStrings(requestItemViewModel.DocumentsInfo, AppUtility.FolderNamesEnum.Shipments, uploadFolder);

                GetExistingFileStrings(requestItemViewModel.DocumentsInfo, AppUtility.FolderNamesEnum.Info, uploadFolder);
                GetExistingFileStrings(requestItemViewModel.DocumentsInfo, AppUtility.FolderNamesEnum.Pictures, uploadFolder);
                //GetExistingFileStrings(requestItemViewModel, AppUtility.RequestFolderNamesEnum.Returns, uploadFolder);
                //GetExistingFileStrings(requestItemViewModel, AppUtility.RequestFolderNamesEnum.Credits, uploadFolder);
            }
        }
        protected async Task<RequestIndexPartialViewModel> GetIndexViewModel(RequestIndexObject requestIndexObject, List<int> Months = null, List<int> Years = null, SelectedFilters selectedFilters = null, string searchText = "", int numFilters = 0)
        {
            int categoryID = 1;
            if (requestIndexObject.SectionType == AppUtility.MenuItems.Operations)
            {
                categoryID = 2;
            }
            IQueryable<Request> RequestsPassedIn = Enumerable.Empty<Request>().AsQueryable();
            IQueryable<Request> fullRequestsList = _context.Requests.Where(r => r.Product.ProductName.Contains(searchText ?? "")).Include(r => r.ApplicationUserCreator)
         .Where(r => r.Product.ProductSubcategory.ParentCategory.CategoryTypeID == categoryID).Where(r => r.IsArchived == requestIndexObject.IsArchive);

            int sideBarID = 0;
            if (requestIndexObject.SidebarType != AppUtility.SidebarEnum.Owner)
            {
                int.TryParse(requestIndexObject.SidebarFilterID, out sideBarID);
            }

            if (requestIndexObject.PageType == AppUtility.PageTypeEnum.LabManagementEquipment)
            {
                if (requestIndexObject.SidebarType == AppUtility.SidebarEnum.Category)
                {
                    RequestsPassedIn = _context.Requests.Where(r => r.RequestStatusID == 3).Where(r => r.Product.ProductSubcategory.ParentCategoryID == 5)
                     .Where(r => r.Product.ProductSubcategoryID == sideBarID).Include(r => r.ParentRequest);

                }
                else
                {
                    RequestsPassedIn = _context.Requests.Where(r => r.RequestStatusID == 3).Where(r => r.Product.ProductSubcategory.ParentCategoryID == 5);

                }
                RequestsPassedIn.OrderByDescending(e => e.ParentRequest.OrderDate);
            }
            else if (ViewData["ReturnRequests"] != null)
            {
                RequestsPassedIn = TempData["ReturnRequests"] as IQueryable<Request>;
            }
            else if (requestIndexObject.PageType == AppUtility.PageTypeEnum.RequestRequest || requestIndexObject.PageType == AppUtility.PageTypeEnum.OperationsRequest)
            {
                /*
                 * In order to combine all the requests each one needs to be in a separate list
                 * Then you need to union them one at a time into separate variables
                 * you only need the separate union variable in order for the union to work
                 * and the original queries are on separate lists because each is querying the full database with a separate where
                 */
                //var filteredRequests = fullRequestsList;
                IQueryable<Request> TempRequestList = Enumerable.Empty<Request>().AsQueryable();
                if (requestIndexObject.RequestStatusID == 0 || requestIndexObject.RequestStatusID == 1 || requestIndexObject.RequestStatusID == 6)
                {
                    TempRequestList = AppUtility.GetRequestsListFromRequestStatusID(fullRequestsList, 1);
                    var TempRequestList2 = AppUtility.GetRequestsListFromRequestStatusID(fullRequestsList, 6);
                    RequestsPassedIn = AppUtility.CombineTwoRequestsLists(TempRequestList, TempRequestList2);
                }

                if (requestIndexObject.RequestStatusID == 0 || requestIndexObject.RequestStatusID == 2)
                {
                    TempRequestList = AppUtility.GetRequestsListFromRequestStatusID(fullRequestsList, 2);
                    RequestsPassedIn = AppUtility.CombineTwoRequestsLists(RequestsPassedIn, TempRequestList);
                }

                if (requestIndexObject.RequestStatusID == 0 || requestIndexObject.RequestStatusID == 3)
                {
                    TempRequestList = AppUtility.GetRequestsListFromRequestStatusID(fullRequestsList, 3, 50);
                    RequestsPassedIn = AppUtility.CombineTwoRequestsLists(RequestsPassedIn, TempRequestList);
                    TempRequestList = AppUtility.GetRequestsListFromRequestStatusID(fullRequestsList, 4);
                    RequestsPassedIn = AppUtility.CombineTwoRequestsLists(RequestsPassedIn, TempRequestList);
                    TempRequestList = AppUtility.GetRequestsListFromRequestStatusID(fullRequestsList, 5);
                    RequestsPassedIn = AppUtility.CombineTwoRequestsLists(RequestsPassedIn, TempRequestList);
                }

            }
            else if (requestIndexObject.PageType == AppUtility.PageTypeEnum.RequestSummary)
            {
                if (requestIndexObject.RequestStatusID == 7)
                {
                    RequestsPassedIn = fullRequestsList.Where(r => r.RequestStatus.RequestStatusID == 7);
                }
                else
                {
                    RequestsPassedIn = fullRequestsList.Where(r => r.RequestStatus.RequestStatusID == 3);
                    RequestsPassedIn = RequestsPassedIn.Include(r => r.Product).ThenInclude(p => p.ProductSubcategory).ThenInclude(ps => ps.ParentCategory)
                .Include(r => r.ParentRequest).Include(r => r.ApplicationUserCreator)
                .Include(r => r.Product.Vendor).Include(r => r.RequestStatus)
                .Include(r => r.UnitType).Include(r => r.SubUnitType).Include(r => r.SubSubUnitType);
                    RequestsPassedIn = RequestsPassedIn.Include(r => r.RequestLocationInstances).ThenInclude(li => li.LocationInstance).ThenInclude(l => l.LocationInstanceParent).ToList().GroupBy(r => r.ProductID).Select(e => e.Last()).AsQueryable();
                }
            }
            else if (requestIndexObject.PageType == AppUtility.PageTypeEnum.AccountingGeneral)
            {
                //we need both categories
                RequestsPassedIn = _context.Requests.Where(r => r.RequestStatusID == 3).Where(r => Years.Contains(r.ParentRequest.OrderDate.Year)).Where(r => !r.IsClarify && !r.IsPartial && r.Payments.Where(p => p.IsPaid && p.HasInvoice).Count() == r.Payments.Count());
                if (Months != null && Months.Count() > 0)
                {
                    RequestsPassedIn = RequestsPassedIn.Where(r => Months.Contains(r.ParentRequest.OrderDate.Month));
                }
            }
            else if (requestIndexObject.PageType == AppUtility.PageTypeEnum.RequestCart && requestIndexObject.SidebarType == AppUtility.SidebarEnum.Favorites)
            {
                var usersFavoriteRequests = _context.FavoriteRequests.Where(fr => fr.ApplicationUserID == _userManager.GetUserId(User))
                    .Select(fr => fr.RequestID).ToList();
                RequestsPassedIn = fullRequestsList.Where(frl => usersFavoriteRequests.Contains(frl.RequestID));
                //RequestsPassedIn = fullRequestsList.Where(frl =>
                //_context.FavoriteRequests.Where(fr => fr.ApplicationUserID == _userManager.GetUserId(User)).Select(fr => fr.RequestID)
                //.Contains(frl.RequestID));

            }
            else if (requestIndexObject.PageType == AppUtility.PageTypeEnum.RequestCart && requestIndexObject.SidebarType == AppUtility.SidebarEnum.SharedRequests)
            {
                var sharedWithMe = _context.ShareRequests.Where(fr => fr.ToApplicationUserID == _userManager.GetUserId(User))
                    .Select(sr => sr.RequestID).ToList();
                RequestsPassedIn = fullRequestsList.Where(frl => sharedWithMe.Contains(frl.RequestID));
            }
            else //we just want what is in inventory
            {
                RequestsPassedIn = fullRequestsList.Where(r => r.RequestStatus.RequestStatusID == 3 || r.RequestStatus.RequestStatusID == 4 || r.RequestStatus.RequestStatusID == 5);
            }
            AppUtility.SidebarEnum SidebarTitle = AppUtility.SidebarEnum.List;
            //now that the lists are created sort by vendor or subcategory
            var sidebarFilterDescription = "";
            switch (requestIndexObject.SidebarType)
            {
                case AppUtility.SidebarEnum.Vendors:
                    sidebarFilterDescription = _context.Vendors.Where(v => v.VendorID == sideBarID).Select(v => v.VendorEnName).FirstOrDefault();
                    RequestsPassedIn = RequestsPassedIn
                     .OrderByDescending(r => r.ProductID)
                     .Where(r => r.Product.VendorID == sideBarID);
                    break;
                case AppUtility.SidebarEnum.Type:
                    sidebarFilterDescription = _context.ProductSubcategories.Where(p => p.ProductSubcategoryID == sideBarID).Select(p => p.ProductSubcategoryDescription).FirstOrDefault();
                    RequestsPassedIn = RequestsPassedIn
                   .OrderByDescending(r => r.ProductID)
                   .Where(r => r.Product.ProductSubcategoryID == sideBarID);
                    break;
                case AppUtility.SidebarEnum.Owner:
                    var owner = _context.Employees.Where(e => e.Id.Equals(requestIndexObject.SidebarFilterID)).FirstOrDefault();
                    sidebarFilterDescription = owner.FirstName + " " + owner.LastName;
                    RequestsPassedIn = RequestsPassedIn
                    .OrderByDescending(r => r.ProductID)
                    .Where(r => r.ApplicationUserCreatorID == requestIndexObject.SidebarFilterID);
                    break;
            }



            RequestIndexPartialViewModel requestIndexViewModel = new RequestIndexPartialViewModel();
            requestIndexViewModel.PricePopoverViewModel = new PricePopoverViewModel();
            if (!requestIndexObject.CategorySelected && !requestIndexObject.SubcategorySelected)
            {
                requestIndexObject.SubcategorySelected = true;
            }
            requestIndexViewModel.CategoryPopoverViewModel = new CategoryPopoverViewModel()
            {
                SelectedCategoryOption = new List<bool>()
                {
                    requestIndexObject.CategorySelected,
                    requestIndexObject.SubcategorySelected
                }
            };
            requestIndexViewModel.PageNumber = requestIndexObject.PageNumber;
            requestIndexViewModel.RequestStatusID = requestIndexObject.RequestStatusID;
            requestIndexViewModel.PageType = requestIndexObject.PageType;
            requestIndexViewModel.SidebarFilterID = requestIndexObject.SidebarFilterID;
            requestIndexViewModel.ErrorMessage = requestIndexObject.ErrorMessage;
            var onePageOfProducts = Enumerable.Empty<RequestIndexPartialRowViewModel>().ToPagedList();

            var RequestPassedInWithInclude = RequestsPassedIn
                .Include(r => r.Product).ThenInclude(p => p.ProductSubcategory).ThenInclude(ps => ps.ParentCategory)
                .Include(r => r.ParentRequest).Include(r => r.ApplicationUserCreator)
                .Include(r => r.Product.Vendor).Include(r => r.RequestStatus)
                .Include(r => r.UnitType).Include(r => r.SubUnitType).Include(r => r.SubSubUnitType).AsQueryable();

            RequestPassedInWithInclude = RequestPassedInWithInclude.Include(r => r.RequestLocationInstances).ThenInclude(li => li.LocationInstance).ThenInclude(l => l.LocationInstanceParent);

            RequestPassedInWithInclude = filterListBySelectFilters(selectedFilters, RequestPassedInWithInclude);

            onePageOfProducts = await GetColumnsAndRows(requestIndexObject, onePageOfProducts, RequestPassedInWithInclude);

            requestIndexViewModel.PagedList = onePageOfProducts;
            List<PriceSortViewModel> priceSorts = new List<PriceSortViewModel>();
            Enum.GetValues(typeof(AppUtility.PriceSortEnum)).Cast<AppUtility.PriceSortEnum>().ToList().ForEach(p => priceSorts.Add(new PriceSortViewModel { PriceSortEnum = p, Selected = requestIndexObject.SelectedPriceSort.Contains(p.ToString()) }));
            requestIndexViewModel.PricePopoverViewModel.PriceSortEnums = priceSorts;
            requestIndexViewModel.PricePopoverViewModel.SelectedCurrency = requestIndexObject.SelectedCurrency;
            requestIndexViewModel.PricePopoverViewModel.PopoverSource = 1;
            requestIndexViewModel.SidebarFilterName = sidebarFilterDescription;
            bool isProprietary = requestIndexObject.RequestStatusID == 7 ? true : false;
            requestIndexViewModel.InventoryFilterViewModel = GetInventoryFilterViewModel(selectedFilters, numFilters, requestIndexObject.SectionType, isProprietary);
            return requestIndexViewModel;
        }
        protected static IQueryable<Request> filterListBySelectFilters(SelectedFilters selectedFilters, IQueryable<Request> fullRequestsListProprietary)
        {
            if (selectedFilters != null)
            {
                if (selectedFilters.SelectedCategoriesIDs.Count() > 0)
                {
                    fullRequestsListProprietary = fullRequestsListProprietary.Where(r => selectedFilters.SelectedCategoriesIDs.Contains(r.Product.ProductSubcategory.ParentCategoryID));
                }
                if (selectedFilters.SelectedSubcategoriesIDs.Count() > 0)
                {
                    fullRequestsListProprietary = fullRequestsListProprietary.Where(r => selectedFilters.SelectedSubcategoriesIDs.Contains(r.Product.ProductSubcategoryID));
                }
                if (selectedFilters.SelectedVendorsIDs.Count() > 0)
                {
                    fullRequestsListProprietary = fullRequestsListProprietary.Where(r => selectedFilters.SelectedVendorsIDs.Contains(r.Product.VendorID ?? 0));
                }
                if (selectedFilters.SelectedLocationsIDs.Count() > 0)
                {
                    fullRequestsListProprietary = fullRequestsListProprietary.Where(r => selectedFilters.SelectedLocationsIDs.Contains((int)(Math.Floor(r.RequestLocationInstances.FirstOrDefault().LocationInstance.LocationTypeID / 100.0) * 100)));
                }
                if (selectedFilters.SelectedOwnersIDs.Count() > 0)
                {
                    fullRequestsListProprietary = fullRequestsListProprietary.Where(r => selectedFilters.SelectedOwnersIDs.Contains(r.ApplicationUserCreatorID));
                }
            }

            return fullRequestsListProprietary;
        }

        private async Task<IPagedList<RequestIndexPartialRowViewModel>> GetColumnsAndRows(RequestIndexObject requestIndexObject, IPagedList<RequestIndexPartialRowViewModel> onePageOfProducts, IQueryable<Request> RequestPassedInWithInclude)
        {
            List<IconColumnViewModel> iconList = new List<IconColumnViewModel>();
            var reorderIcon = new IconColumnViewModel(" icon-add_circle_outline-24px1 ", "#00CA72", "load-order-details", "Reorder");
            var orderOperations = new IconColumnViewModel(" icon-add_circle_outline-24px1 ", "#00CA72", "order-approved-operation", "Order");
            var deleteIcon = new IconColumnViewModel(" icon-delete-24px ", "black", "load-confirm-delete", "Delete");
            var receiveIcon = new IconColumnViewModel(" icon-done-24px ", "#00CA72", "load-receive-and-location", "Receive");
            var approveIcon = new IconColumnViewModel(" icon-centarix-icons-03 ", "#00CA72", "approve-order", "Approve");
            var equipmentIcon = new IconColumnViewModel(" icon-settings-24px-1 ", "var(--lab-man-color);", "create-calibration", "Create Calibration");
            var favoriteIcon = new IconColumnViewModel(" icon-favorite_border-24px", "#5F79E2", "request-favorite", "Favorite");

            var popoverMoreIcon = new IconColumnViewModel("icon-more_vert-24px", "black", "popover-more", "More");
            var popoverDelete = new IconPopoverViewModel(" icon-delete-24px  ", "black", AppUtility.PopoverDescription.Delete, "Delete", "Requests", AppUtility.PopoverEnum.None, "load-confirm-delete");
            var popoverReorder = new IconPopoverViewModel(" icon-add_circle_outline-24px1 ", "#00CA72", AppUtility.PopoverDescription.Reorder, "Reorder", "Requests", AppUtility.PopoverEnum.None, "load-order-details");
            var popoverRemoveShare = new IconPopoverViewModel("icon-share-24px1", "black", AppUtility.PopoverDescription.RemoveShare, ajaxcall: "remove-share");
            var popoverShare = new IconPopoverViewModel("icon-share-24px1", "black", AppUtility.PopoverDescription.Share, "ShareModal", "Requests", AppUtility.PopoverEnum.None, "share-request-fx");
            var defaultImage = "/images/css/CategoryImages/placeholder.png";
            var user = await _userManager.GetUserAsync(User);
            switch (requestIndexObject.PageType)
            {
                case AppUtility.PageTypeEnum.RequestRequest:
                    switch (requestIndexObject.RequestStatusID)
                    {
                        case 6:
                            iconList.Add(approveIcon);
                            iconList.Add(deleteIcon);
                            onePageOfProducts = await RequestPassedInWithInclude.OrderByDescending(r => r.CreationDate).Select(r =>
                                    new RequestIndexPartialRowViewModel(AppUtility.IndexTableTypes.Approved,
                                             r, r.Product, r.Product.Vendor, r.Product.ProductSubcategory, r.Product.ProductSubcategory.ParentCategory,
                                             r.UnitType, r.SubUnitType, r.SubSubUnitType, requestIndexObject, iconList, defaultImage)
                                    ).ToPagedListAsync(requestIndexObject.PageNumber == 0 ? 1 : requestIndexObject.PageNumber, 25);
                            break;
                        case 2:
                            iconList.Add(receiveIcon);
                            iconList.Add(deleteIcon);
                            onePageOfProducts = await RequestPassedInWithInclude.OrderByDescending(r => r.ParentRequest.OrderDate).Select(r => new RequestIndexPartialRowViewModel(AppUtility.IndexTableTypes.Ordered,
                               r, r.Product, r.Product.Vendor, r.Product.ProductSubcategory, r.Product.ProductSubcategory.ParentCategory,
                                            r.UnitType, r.SubUnitType, r.SubSubUnitType, requestIndexObject, iconList, defaultImage, r.ParentRequest)).ToPagedListAsync(requestIndexObject.PageNumber == 0 ? 1 : requestIndexObject.PageNumber, 25);
                            break;
                        case 3:
                            //iconList.Add(reorderIcon);
                            iconList.Add(favoriteIcon);
                            popoverMoreIcon.IconPopovers = new List<IconPopoverViewModel>() { popoverShare, popoverReorder, popoverDelete };
                            iconList.Add(popoverMoreIcon);
                            onePageOfProducts = await RequestPassedInWithInclude.OrderByDescending(r => r.ParentRequest.OrderDate).Select(r =>
                            new RequestIndexPartialRowViewModel(AppUtility.IndexTableTypes.ReceivedInventory,
                             r, r.Product, r.Product.Vendor, r.Product.ProductSubcategory, r.Product.ProductSubcategory.ParentCategory,
                                          r.UnitType, r.SubUnitType, r.SubSubUnitType, requestIndexObject, iconList, defaultImage, _context.FavoriteRequests.Where(fr => fr.RequestID == r.RequestID).Where(fr => fr.ApplicationUserID == user.Id).FirstOrDefault(),
                                            _context.ShareRequests.Where(sr => sr.RequestID == r.RequestID).Where(sr => sr.ToApplicationUserID == user.Id).Include(sr => sr.FromApplicationUser).FirstOrDefault(), user,
                                            r.RequestLocationInstances.FirstOrDefault().LocationInstance, r.RequestLocationInstances.FirstOrDefault().LocationInstance.LocationInstanceParent, r.ParentRequest)).ToPagedListAsync(requestIndexObject.PageNumber == 0 ? 1 : requestIndexObject.PageNumber, 25);
                            break;

                    }
                    break;
                case AppUtility.PageTypeEnum.OperationsRequest:
                    switch (requestIndexObject.RequestStatusID)
                    {
                        case 2:
                            iconList.Add(receiveIcon);
                            iconList.Add(deleteIcon);
                            onePageOfProducts = onePageOfProducts = await RequestPassedInWithInclude.OrderByDescending(r => r.ParentRequest.OrderDate).Select(r => new RequestIndexPartialRowViewModel(AppUtility.IndexTableTypes.OrderedOperations,
                            r, r.Product, r.Product.Vendor, r.Product.ProductSubcategory, r.Product.ProductSubcategory.ParentCategory,
                                         r.UnitType, r.SubUnitType, r.SubSubUnitType, requestIndexObject, iconList, defaultImage, r.ParentRequest, user)
                                       ).ToPagedListAsync(requestIndexObject.PageNumber == 0 ? 1 : requestIndexObject.PageNumber, 25);

                            break;
                        case 3:
                            iconList.Add(deleteIcon);
                            onePageOfProducts = onePageOfProducts = await RequestPassedInWithInclude.OrderByDescending(r => r.ParentRequest.OrderDate).Select(r => new RequestIndexPartialRowViewModel(AppUtility.IndexTableTypes.ReceivedInventoryOperations,
                             r, r.Product, r.Product.Vendor, r.Product.ProductSubcategory, r.Product.ProductSubcategory.ParentCategory,
                                          r.UnitType, r.SubUnitType, r.SubSubUnitType, requestIndexObject, iconList, defaultImage, r.ParentRequest, user)
                                        ).ToPagedListAsync(requestIndexObject.PageNumber == 0 ? 1 : requestIndexObject.PageNumber, 25);
                            break;
                    }
                    break;
                case AppUtility.PageTypeEnum.RequestInventory:
                    popoverMoreIcon.IconPopovers = new List<IconPopoverViewModel>() { popoverShare, popoverReorder };
                    iconList.Add(deleteIcon);
                    iconList.Add(popoverMoreIcon);
                    onePageOfProducts = onePageOfProducts = onePageOfProducts = await RequestPassedInWithInclude.OrderByDescending(r => r.ArrivalDate).Select(r => new RequestIndexPartialRowViewModel(AppUtility.IndexTableTypes.ReceivedInventory,
                             r, r.Product, r.Product.Vendor, r.Product.ProductSubcategory, r.Product.ProductSubcategory.ParentCategory,
                             r.UnitType, r.SubUnitType, r.SubSubUnitType, requestIndexObject, iconList, defaultImage, _context.FavoriteRequests.Where(fr => fr.RequestID == r.RequestID).Where(fr => fr.ApplicationUserID == user.Id).FirstOrDefault(),
                               _context.ShareRequests.Where(sr => sr.RequestID == r.RequestID).Where(sr => sr.ToApplicationUserID == user.Id).Include(sr => sr.FromApplicationUser).FirstOrDefault(), user,
                         r.RequestLocationInstances.FirstOrDefault().LocationInstance, r.RequestLocationInstances.FirstOrDefault().LocationInstance.LocationInstanceParent, r.ParentRequest)).ToPagedListAsync(requestIndexObject.PageNumber == 0 ? 1 : requestIndexObject.PageNumber, 25);
                    break;
                case AppUtility.PageTypeEnum.RequestSummary:


                    switch (requestIndexObject.RequestStatusID)
                    {
                        case 7:
                            popoverMoreIcon.IconPopovers = new List<IconPopoverViewModel>() { popoverShare, popoverReorder, popoverDelete };
                            iconList.Add(favoriteIcon);
                            iconList.Add(deleteIcon);
                            onePageOfProducts = onePageOfProducts = await RequestPassedInWithInclude.OrderByDescending(r => r.CreationDate).Select(r => new RequestIndexPartialRowViewModel(AppUtility.IndexTableTypes.SummaryProprietary,
                            r, r.Product, r.Product.Vendor, r.Product.ProductSubcategory, r.Product.ProductSubcategory.ParentCategory,
                                         r.UnitType, r.SubUnitType, r.SubSubUnitType, requestIndexObject, iconList, defaultImage, _context.FavoriteRequests.Where(fr => fr.RequestID == r.RequestID).Where(fr => fr.ApplicationUserID == user.Id).FirstOrDefault(),
                                          _context.ShareRequests.Where(sr => sr.RequestID == r.RequestID).Where(sr => sr.ToApplicationUserID == user.Id).Include(sr => sr.FromApplicationUser).FirstOrDefault(), user,
                                          r.RequestLocationInstances.FirstOrDefault().LocationInstance, r.RequestLocationInstances.FirstOrDefault().LocationInstance.LocationInstanceParent, r.ParentRequest)
                                       ).ToPagedListAsync(requestIndexObject.PageNumber == 0 ? 1 : requestIndexObject.PageNumber, 25);

                            break;
                        default:
                            popoverMoreIcon.IconPopovers = new List<IconPopoverViewModel>() { popoverShare, popoverReorder, popoverDelete };
                            iconList.Add(favoriteIcon);
                            iconList.Add(popoverMoreIcon);
                            onePageOfProducts = onePageOfProducts = await RequestPassedInWithInclude.OrderByDescending(r => r.ParentRequest.OrderDate).Select(r => new RequestIndexPartialRowViewModel(AppUtility.IndexTableTypes.Summary,
                             r, r.Product, r.Product.Vendor, r.Product.ProductSubcategory, r.Product.ProductSubcategory.ParentCategory,
                                    r.UnitType, r.SubUnitType, r.SubSubUnitType, requestIndexObject, iconList, defaultImage, _context.FavoriteRequests.Where(fr => fr.RequestID == r.RequestID).Where(fr => fr.ApplicationUserID == user.Id).FirstOrDefault(),
                                    _context.ShareRequests.Where(sr => sr.RequestID == r.RequestID).Where(sr => sr.ToApplicationUserID == user.Id).Include(sr => sr.FromApplicationUser).FirstOrDefault(), user,
                                    r.RequestLocationInstances.FirstOrDefault().LocationInstance, r.RequestLocationInstances.FirstOrDefault().LocationInstance.LocationInstanceParent, r.ParentRequest)
                           ).ToPagedListAsync(requestIndexObject.PageNumber == 0 ? 1 : requestIndexObject.PageNumber, 25);
                            break;
                    }

                    break;
                case AppUtility.PageTypeEnum.OperationsInventory:
                    iconList.Add(orderOperations);
                    iconList.Add(deleteIcon);
                    onePageOfProducts = onePageOfProducts = await RequestPassedInWithInclude.OrderByDescending(r => r.ParentRequest.OrderDate).Select(r => new RequestIndexPartialRowViewModel(AppUtility.IndexTableTypes.ReceivedInventoryOperations,
                             r, r.Product, r.Product.Vendor, r.Product.ProductSubcategory, r.Product.ProductSubcategory.ParentCategory,
                                          r.UnitType, r.SubUnitType, r.SubSubUnitType, requestIndexObject, iconList, defaultImage, r.ParentRequest)
                                        ).ToPagedListAsync(requestIndexObject.PageNumber == 0 ? 1 : requestIndexObject.PageNumber, 25);
                    break;
                case AppUtility.PageTypeEnum.LabManagementEquipment:
                    iconList.Add(equipmentIcon);
                    popoverMoreIcon.IconPopovers = new List<IconPopoverViewModel>() { popoverShare, popoverReorder };
                    iconList.Add(deleteIcon);
                    iconList.Add(popoverMoreIcon);
                    onePageOfProducts = await RequestPassedInWithInclude.OrderByDescending(r => r.ParentRequest.OrderDate).Select(r =>
                            new RequestIndexPartialRowViewModel(AppUtility.IndexTableTypes.ReceivedInventory,
                             r, r.Product, r.Product.Vendor, r.Product.ProductSubcategory, r.Product.ProductSubcategory.ParentCategory,
                                          r.UnitType, r.SubUnitType, r.SubSubUnitType, requestIndexObject, iconList, defaultImage, _context.FavoriteRequests.Where(fr => fr.RequestID == r.RequestID).Where(fr => fr.ApplicationUserID == user.Id).FirstOrDefault(), user, r.ParentRequest)).ToPagedListAsync(requestIndexObject.PageNumber == 0 ? 1 : requestIndexObject.PageNumber, 25);
                    break;
                case AppUtility.PageTypeEnum.ExpensesStatistics:
                    break;
                case AppUtility.PageTypeEnum.AccountingGeneral:
                    onePageOfProducts = onePageOfProducts = await RequestPassedInWithInclude.OrderByDescending(r => r.ParentRequest.OrderDate).Select(r => new RequestIndexPartialRowViewModel(AppUtility.IndexTableTypes.AccountingGeneral,
                             r, r.Product, r.Product.Vendor, r.Product.ProductSubcategory, r.Product.ProductSubcategory.ParentCategory,
                                          r.UnitType, r.SubUnitType, r.SubSubUnitType, requestIndexObject, iconList, defaultImage, r.ParentRequest)).ToPagedListAsync(requestIndexObject.PageNumber == 0 ? 1 : requestIndexObject.PageNumber, 25);

                    break;
                case AppUtility.PageTypeEnum.RequestCart:
                    switch (requestIndexObject.SidebarType)
                    {
                        case AppUtility.SidebarEnum.Favorites:
                            iconList.Add(reorderIcon);
                            iconList.Add(favoriteIcon);
                            onePageOfProducts = await RequestPassedInWithInclude.OrderByDescending(r => r.ParentRequest.OrderDate).Select(r =>
                           new RequestIndexPartialRowViewModel(AppUtility.IndexTableTypes.ReceivedInventoryFavorites,
                            r, r.Product, r.Product.Vendor, r.Product.ProductSubcategory, r.Product.ProductSubcategory.ParentCategory,
                                         r.UnitType, r.SubUnitType, r.SubSubUnitType, requestIndexObject, iconList, defaultImage, _context.FavoriteRequests.Where(fr => fr.RequestID == r.RequestID).Where(fr => fr.ApplicationUserID == user.Id).FirstOrDefault(),
                                         user, r.RequestLocationInstances.FirstOrDefault().LocationInstance, r.RequestLocationInstances.FirstOrDefault().LocationInstance.LocationInstanceParent, r.ParentRequest)).ToPagedListAsync(requestIndexObject.PageNumber == 0 ? 1 : requestIndexObject.PageNumber, 25);
                            break;
                        case AppUtility.SidebarEnum.SharedRequests:
                            iconList.Add(reorderIcon);
                            iconList.Add(favoriteIcon);
                            onePageOfProducts = await RequestPassedInWithInclude.OrderByDescending(r => r.ParentRequest.OrderDate).Select(r =>
                            new RequestIndexPartialRowViewModel(AppUtility.IndexTableTypes.ReceivedInventoryShared,
                             r, r.Product, r.Product.Vendor, r.Product.ProductSubcategory, r.Product.ProductSubcategory.ParentCategory,
                                          r.UnitType, r.SubUnitType, r.SubSubUnitType, requestIndexObject, iconList, defaultImage,
                                          _context.FavoriteRequests.Where(fr => fr.RequestID == r.RequestID).Where(fr => fr.ApplicationUserID == user.Id).FirstOrDefault(),
                                          _context.ShareRequests
                .Where(sr => sr.RequestID == r.RequestID).Where(sr => sr.ToApplicationUserID == user.Id).Include(sr => sr.FromApplicationUser).FirstOrDefault(), user,
                                          r.RequestLocationInstances.FirstOrDefault().LocationInstance, r.RequestLocationInstances.FirstOrDefault().LocationInstance.LocationInstanceParent, r.ParentRequest)).ToPagedListAsync(requestIndexObject.PageNumber == 0 ? 1 : requestIndexObject.PageNumber, 25);
                            break;
                    }
                    break;
            }

            return onePageOfProducts;
        }


        protected InventoryFilterViewModel GetInventoryFilterViewModel(SelectedFilters selectedFilters = null, int numFilters = 0, AppUtility.MenuItems sectionType = AppUtility.MenuItems.Requests, bool isProprietary = false)
        {
            int categoryType = sectionType == AppUtility.MenuItems.Requests ? 1 : 2;
            if (selectedFilters != null)
            {
                InventoryFilterViewModel inventoryFilterViewModel = new InventoryFilterViewModel()
                {
                    //Types = _context.CategoryTypes.Where(ct => !selectedFilters.SelectedTypesIDs.Contains(ct.CategoryTypeID)).ToList(),
                    Owners = _context.Employees.Where(o => !selectedFilters.SelectedOwnersIDs.Contains(o.Id)).ToList(),
                    Locations = _context.LocationTypes.Where(l => l.Depth == 0).Where(l => !selectedFilters.SelectedLocationsIDs.Contains(l.LocationTypeID)).ToList(),
                    Categories = _context.ParentCategories.Where(c => c.CategoryTypeID == categoryType && c.IsProprietary == isProprietary).Where(c => !selectedFilters.SelectedCategoriesIDs.Contains(c.ParentCategoryID)).ToList(),
                    Subcategories = _context.ProductSubcategories.Distinct().Where(sc => sc.ParentCategory.CategoryTypeID == categoryType && sc.ParentCategory.IsProprietary == isProprietary)
                        .Where(v => !selectedFilters.SelectedSubcategoriesIDs.Contains(v.ProductSubcategoryID)).ToList(),
                    Vendors = _context.Vendors.Where(v => v.VendorCategoryTypes.Select(vc => vc.CategoryTypeID).Contains(categoryType)).Where(v => !selectedFilters.SelectedVendorsIDs.Contains(v.VendorID)).ToList(),
                    //SelectedTypes = _context.CategoryTypes.Where(ct => selectedFilters.SelectedTypesIDs.Contains(ct.CategoryTypeID)).ToList(),
                    SelectedVendors = _context.Vendors.Where(v => selectedFilters.SelectedVendorsIDs.Contains(v.VendorID)).ToList(),
                    SelectedOwners = _context.Employees.Where(o => selectedFilters.SelectedOwnersIDs.Contains(o.Id)).ToList(),
                    SelectedLocations = _context.LocationTypes.Where(l => l.Depth == 0).Where(l => selectedFilters.SelectedLocationsIDs.Contains(l.LocationTypeID)).ToList(),
                    SelectedCategories = _context.ParentCategories.Where(c => selectedFilters.SelectedCategoriesIDs.Contains(c.ParentCategoryID)).ToList(),
                    SelectedSubcategories = _context.ProductSubcategories.Distinct().Where(v => selectedFilters.SelectedSubcategoriesIDs.Contains(v.ProductSubcategoryID)).ToList(),
                    //Projects = _context.Projects.ToList(),
                    //SubProjects = _context.SubProjects.ToList()
                    NumFilters = numFilters,
                    SectionType = sectionType
                };
                if (inventoryFilterViewModel.SelectedCategories.Count() > 0)
                {
                    inventoryFilterViewModel.Subcategories = inventoryFilterViewModel.Subcategories.Where(ps => inventoryFilterViewModel.SelectedCategories.Contains(ps.ParentCategory)).ToList();
                }

                return inventoryFilterViewModel;
            }
            else
            {
                return new InventoryFilterViewModel()
                {
                    //Types = _context.CategoryTypes.ToList(),
                    //Vendors = _context.Vendors.ToList(),
                    Vendors = _context.Vendors.Where(v => v.VendorCategoryTypes.Select(vc => vc.CategoryTypeID).Contains(categoryType)).ToList(),
                    Owners = _context.Employees.ToList(),
                    Locations = _context.LocationTypes.Where(r => r.Depth == 0).ToList(),
                    Categories = _context.ParentCategories.Where(c => c.CategoryTypeID == categoryType && c.IsProprietary == isProprietary).ToList(),
                    Subcategories = _context.ProductSubcategories.Distinct().Where(sc => sc.ParentCategory.CategoryTypeID == categoryType && sc.ParentCategory.IsProprietary == isProprietary).ToList(),
                    //SelectedTypes = new List<CategoryType>(),
                    SelectedVendors = new List<Vendor>(),
                    SelectedOwners = new List<Employee>(),
                    SelectedLocations = new List<LocationType>(),
                    SelectedCategories = new List<ParentCategory>(),
                    SelectedSubcategories = new List<ProductSubcategory>(),
                    //Projects = _context.Projects.ToList(),
                    //SubProjects = _context.SubProjects.ToList()
                    NumFilters = numFilters,
                    SectionType = sectionType
                };
            }
        }

        public static double GetTotalWorkingDaysThisMonth(DateTime firstOfTheMonth, List<CompanyDayOff> companyDayOffs)
        {
            DateTime endOfTheMonth = firstOfTheMonth.AddMonths(1);
            return GetTotalWorkingDaysByInterval(firstOfTheMonth, companyDayOffs, endOfTheMonth);
        }

        public static double GetTotalWorkingDaysThisYear(DateTime firstOfTheYear, List<CompanyDayOff> companyDayOffs)
        {
            DateTime endOfTheYear = firstOfTheYear.AddYears(1);
            return GetTotalWorkingDaysByInterval(firstOfTheYear, companyDayOffs, endOfTheYear);
        }

        private static double GetTotalWorkingDaysByInterval(DateTime startDate, List<CompanyDayOff> companyDayOffs, DateTime endDate)
        {
            int companyDaysOffCount = companyDayOffs.Where(d => d.Date.Date >= startDate.Date && d.Date.Date < endDate.Date).Count();
            DateTime nextDay = startDate;
            int totalDays = 0;
            while (nextDay.Date < endDate)
            {
                if (nextDay.DayOfWeek != DayOfWeek.Friday && nextDay.DayOfWeek != DayOfWeek.Saturday)
                {
                    totalDays += 1;
                }
                nextDay = nextDay.AddDays(1);
            }
            return totalDays - companyDaysOffCount;
        }

        protected void MoveDocumentsOutOfTempFolder(int id, AppUtility.ParentFolderName parentFolderName, bool additionalRequests = false)
        {
            //rename temp folder to the request id
            string uploadFolder = Path.Combine(_hostingEnvironment.WebRootPath, parentFolderName.ToString());
            string requestFolderFrom = Path.Combine(uploadFolder, "0");
            string requestFolderTo = Path.Combine(uploadFolder, id.ToString());
            if (Directory.Exists(requestFolderFrom))
            {
                if (Directory.Exists(requestFolderTo))
                {
                    Directory.Delete(requestFolderTo);
                }
                if (additionalRequests)
                {
                    AppUtility.DirectoryCopy(requestFolderFrom, requestFolderTo, true);
                }
                else
                {
                    Directory.Move(requestFolderFrom, requestFolderTo);
                }
            }
        }
        //[HttpPost]
        //[Authorize(Roles = "Requests")]
        //public async Task<IActionResult> TermsModal(TermsViewModel termsViewModel)
        //{
        //    return new EmptyResult();
        //}
    }
}
