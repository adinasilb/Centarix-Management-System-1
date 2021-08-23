using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.AspNetCore.Mvc.ViewEngines;
using Microsoft.AspNetCore.Mvc.ViewFeatures;
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
        protected readonly ApplicationDbContext _context;
        protected readonly UserManager<ApplicationUser> _userManager;
        protected readonly IHostingEnvironment _hostingEnvironment;
        protected readonly IHttpContextAccessor _httpContextAccessor;
        protected string AccessDeniedPath = "~/Identity/Account/AccessDenied";
        protected ICompositeViewEngine _viewEngine;
        protected SharedController(ApplicationDbContext context, UserManager<ApplicationUser> userManager, IHostingEnvironment hostingEnvironment, ICompositeViewEngine viewEngine, IHttpContextAccessor httpContextAccessor)
        {
            _context = context;
            _hostingEnvironment = hostingEnvironment;
            _userManager = userManager;
            _viewEngine = viewEngine;
            _httpContextAccessor = httpContextAccessor;
        }

        protected async Task<bool> IsAuthorizedAsync(AppUtility.MenuItems SectionType, string innerRole = null)
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
                if (innerRole == null)
                {
                    Allowed = true;
                }
                else
                {
                    if (await _userManager.IsInRoleAsync(user, SectionType + innerRole))
                    {
                        Allowed = true;
                    }
                }
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
            var employeeHours = _context.EmployeeHours.Where(eh => eh.EmployeeID == user.Id && eh.Date.Month == month && eh.Date.Year == year && eh.Date <= DateTime.Now.Date);
            int unpaidLeave = 0;
            if (user.EmployeeStatusID != 1)
            {
                totalhours = null;
            }
            else
            {
                var sickHours = employeeHours.Where(eh => eh.PartialOffDayTypeID == 1)
                    .Select(eh => (eh.PartialOffDayHours == null ? TimeSpan.Zero : ((TimeSpan)eh.PartialOffDayHours)).TotalHours).ToList().Sum(p => p);
                sickDaysTaken = employeeHours.Where(eh =>eh.OffDayTypeID == 1).Count();
                sickDaysTaken = sickDaysTaken + (sickHours / user.SalariedEmployee.HoursPerDay);

                var vacationHours = employeeHours.Where(eh => eh.PartialOffDayTypeID == 2)
                    .Select(eh => (eh.PartialOffDayHours == null ? TimeSpan.Zero : ((TimeSpan)eh.PartialOffDayHours)).TotalHours).ToList().Sum(p => p);
                vacationDaysTaken = employeeHours.Where(eh => eh.OffDayTypeID == 2).Count();
                vacationDaysTaken = vacationDaysTaken + (vacationHours / user.SalariedEmployee.HoursPerDay);
                var specialDays = employeeHours.Where(eh => eh.OffDayTypeID == 4 ).Count();
                unpaidLeave = employeeHours.Where(eh => eh.OffDayTypeID == 5).Count();
                totalDays = GetTotalWorkingDaysThisMonth(new DateTime(year, month, 1), companyDaysOff);
                totalhours = (totalDays - (vacationDaysTaken + sickDaysTaken + unpaidLeave + specialDays)) * user.SalariedEmployee.HoursPerDay;
                workingDays = employeeHours.Where(eh => (eh.OffDayTypeID == null) || (eh.IsBonus && eh.OffDayTypeID != null))
                    .Where(eh => eh.Exit1 != null || eh.TotalHours != null).Count();
                workingDays = workingDays - Math.Round((sickHours + vacationHours) / user.SalariedEmployee?.HoursPerDay ?? 1, 2);
            }
            SummaryHoursViewModel summaryHoursViewModel = new SummaryHoursViewModel()
            {
                EmployeeHours = hours,
                CurrentMonth = CurMonth,
                TotalHoursInMonth = totalhours,
                SelectedYear = year,
                TotalHolidaysInMonth = companyDaysOff.Where(cdo => cdo.Date.Year == year && cdo.Date.Month == month).Count(),
                VacationDayInThisMonth = Math.Round(vacationDaysTaken, 2),
                SickDayInThisMonth = Math.Round(sickDaysTaken, 2),
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

        protected void GetExistingFileStrings(List<DocumentFolder> DocumentsInfo, AppUtility.FolderNamesEnum folderName, AppUtility.ParentFolderName parentFolderName, 
                                                                                                                                string uploadFolderParent, string objectID)
        {
            string uploadFolder = Path.Combine(uploadFolderParent, folderName.ToString());
            DocumentFolder folder = new DocumentFolder()
            {
                FolderName = folderName, 
                ParentFolderName = parentFolderName,
                ObjectID = objectID
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
            var MiddleFolderName = documentsModalViewModel.ObjectID == "0" ? documentsModalViewModel.Guid.ToString() : documentsModalViewModel.ObjectID;
            string folder = Path.Combine(uploadFolder, MiddleFolderName);
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
        protected void DeleteTemporaryDocuments(AppUtility.ParentFolderName parentFolderName, Guid? guid = null, int ObjectID = 0)
        {
            string uploadFolder = Path.Combine(_hostingEnvironment.WebRootPath, parentFolderName.ToString());
            String FolderName = guid == Guid.Empty ? ObjectID.ToString() : guid.ToString();
            string requestFolder = Path.Combine(uploadFolder, FolderName);

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
                Directory.Delete(requestFolder, true);
            }
            Directory.CreateDirectory(requestFolder);
        }
        protected void FillDocumentsViewModel(DocumentsModalViewModel documentsModalViewModel)
        {
            string uploadFolder1 = Path.Combine(_hostingEnvironment.WebRootPath, documentsModalViewModel.ParentFolderName.ToString());
            var MiddleFolderName = documentsModalViewModel.ObjectID == "0" ? documentsModalViewModel.Guid.ToString() : documentsModalViewModel.ObjectID;
            string uploadFolder2 = Path.Combine(uploadFolder1, MiddleFolderName);
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


        [Authorize(Roles = "Requests, Protocols")]
        public async Task<RequestItemViewModel> editModalViewFunction(int? id, int? Tab = 0, AppUtility.MenuItems SectionType = AppUtility.MenuItems.Requests,
            bool isEditable = true, List<string> selectedPriceSort = null, string selectedCurrency = null, bool isProprietary = false)
        {

            var categoryType = 1;
            if (SectionType == AppUtility.MenuItems.Operations)
            {
                categoryType = 2;
            }

            string ModalViewType = "";
            if (id == null)
            {
                return null;
            }

            var productId = _context.Requests.Where(r => r.RequestID == id).IgnoreQueryFilters().Where(r => !r.IsDeleted).Select(r => r.ProductID).FirstOrDefault();

            var request = _context.Requests.IgnoreQueryFilters().Where(r => !r.IsDeleted).Include(r => r.Product)
                .Include(r => r.ParentQuote)
                .Include(r => r.ParentRequest)
                .Include(r => r.Product.ProductSubcategory)
                .Include(r => r.Product.ProductSubcategory.ParentCategory)
                .Include(r => r.Product.Vendor)
                .Include(r => r.RequestStatus)
                .Include(r => r.ApplicationUserCreator).Include(r => r.PaymentStatus)
                .Include(r => r.Payments).ThenInclude(p => p.CompanyAccount)
                .Include(r => r.Payments).ThenInclude(p => p.CreditCard)
                .Include(r => r.Payments).ThenInclude(p => p.Invoice)
                .Include(r => r.ApplicationUserReceiver)
                //.Include(r => r.Payments) //do we have to have a separate list of payments to include thefix c inside things (like company account and payment types?)
                .SingleOrDefault(x => x.RequestID == id);
            
          
            if (request.RequestStatusID == 7)
            {
                isProprietary = true;
            }

            var requestsByProduct = _context.Requests.IgnoreQueryFilters().Where(r=>!r.IsDeleted).Where(r => r.ProductID == productId)
                 .Include(r => r.Product.ProductSubcategory).Include(r => r.Product.ProductSubcategory.ParentCategory)
                    .Include(r => r.ApplicationUserCreator) //do we have to have a separate list of payments to include the inside things (like company account and payment types?)
                    .Include(r => r.ParentRequest)
                    .Where(r => r.Product.ProductSubcategory.ParentCategory.CategoryTypeID == categoryType).Include(r => r.ApplicationUserReceiver)
                    .OrderByDescending(r => r.CreationDate)
                    .ToList();

            RequestItemViewModel requestItemViewModel = new RequestItemViewModel();
            await FillRequestDropdowns(requestItemViewModel, request.Product.ProductSubcategory, categoryType);

            requestItemViewModel.Tab = Tab ?? 0;
            requestItemViewModel.Comments = await _context.Comments
                .Include(r => r.ApplicationUser)
                .Where(r => r.Request.RequestID == id).ToListAsync();
            requestItemViewModel.SectionType = SectionType;
            requestItemViewModel.RequestsByProduct = requestsByProduct;
            requestItemViewModel.Requests = new List<Request>();
            if (isEditable)
            {
                requestItemViewModel.ModalType = AppUtility.RequestModalType.Edit;
            }
            else
            {
                requestItemViewModel.ModalType = AppUtility.RequestModalType.Summary;
            }

            if (_context.Requests.Where(r => r.ProductID == request.ProductID).Count() > 1)
            {
                requestItemViewModel.IsReorder = true;
            }

            ModalViewType = "Edit";
            requestItemViewModel.Requests.Add(request);

            //load the correct list of subprojects
            //var subprojects = await _context.SubProjects
            //    .Where(sp => sp.ProjectID == requestItemViewModel.Request.SubProject.ProjectID)
            //    .ToListAsync();
            //requestItemViewModel.SubProjects = subprojects;
            //may be able to do this together - combining the path for the orders folders
            string uploadFolder1 = Path.Combine(_hostingEnvironment.WebRootPath, AppUtility.ParentFolderName.Requests.ToString());
            string requestId = requestItemViewModel.Requests.FirstOrDefault().RequestID.ToString();
            string parentQuoteId = requestItemViewModel.Requests.FirstOrDefault().ParentQuoteID?.ToString();
            requestItemViewModel.DocumentsInfo = new List<DocumentFolder>();

            //the partial file name that we will search for (1- because we want the first one)
            //creating the directory from the path made earlier
            var productSubcategory = requestItemViewModel.Requests.FirstOrDefault().Product.ProductSubcategory;

            FillDocumentsInfo(requestItemViewModel, productSubcategory, requestId, parentQuoteId);

            //locations:
            //get the list of requestLocationInstances in this request
            //can't look for _context.RequestLocationInstances b/c it's a join table and doesn't have a dbset
            var request1 = _context.Requests.IgnoreQueryFilters().Where(r => r.RequestID == id).Include(r => r.RequestLocationInstances).ThenInclude(rli => rli.LocationInstance).FirstOrDefault();
            var requestLocationInstances = request1.RequestLocationInstances.ToList();
            //if it has => (which it should once its in a details view)
            requestItemViewModel.LocationInstances = new List<LocationInstance>();
            requestLocationInstances.ForEach(rli => requestItemViewModel.LocationInstances.Add(rli.LocationInstance));
            if (request1.RequestStatusID == 3 || request1.RequestStatusID == 5 || request1.RequestStatusID == 4 || request1.RequestStatusID == 7)
            {
                ReceivedLocationViewModel receivedLocationViewModel = new ReceivedLocationViewModel()
                {
                    Request = _context.Requests.Where(r => r.RequestID == request1.RequestID).Include(r => r.Product).ThenInclude(p => p.ProductSubcategory).ThenInclude(ps => ps.ParentCategory)
                   .FirstOrDefault(),
                    locationTypesDepthZero = _context.LocationTypes.Where(lt => lt.Depth == 0),
                    locationInstancesSelected = new List<LocationInstance>(),
                };


                if (requestLocationInstances.Any())
                {
                    //get the parent location instances of the first one
                    //can do this now b/c can only be in one box - later on will have to be a list or s/t b/c will have more boxes
                    //int? locationInstanceParentID = _context.LocationInstances.OfType<LocationInstance>().Where(li => li.LocationInstanceID == requestLocationInstances[0].LocationInstanceID).FirstOrDefault().LocationInstanceParentID;
                    LocationInstance parentLocationInstance = _context.LocationInstances.OfType<LocationInstance>().Where(li => li.LocationInstanceID == requestLocationInstances[0].LocationInstance.LocationInstanceParentID).Include(li => li.LocationType).FirstOrDefault();
                    //requestItemViewModel.ParentLocationInstance = _context.LocationInstances.OfType<LocationInstance>().Where(li => li.LocationInstanceID == requestLocationInstances[0].LocationInstance.LocationInstanceParentID).FirstOrDefault();
                    //need to test b/c the model is int? which is nullable

                    if (parentLocationInstance == null)
                    {
                        var locationType = _context.TemporaryLocationInstances.IgnoreQueryFilters().Where(l => l.LocationInstanceID == requestLocationInstances[0].LocationInstance.LocationInstanceID).Select(li => li.LocationType).FirstOrDefault();
                        requestItemViewModel.ReceivedLocationViewModel = receivedLocationViewModel;
                        ReceivedModalSublocationsViewModel receivedModalSublocationsViewModel = new ReceivedModalSublocationsViewModel()
                        {
                            locationInstancesDepthZero = new List<LocationInstance>(),
                            locationTypeNames = new List<string>(),
                            locationInstancesSelected = _context.LocationInstances.IgnoreQueryFilters().Where(l => l.LocationInstanceID == requestLocationInstances[0].LocationInstance.LocationInstanceID).ToList()

                        };
                        requestItemViewModel.ReceivedModalSublocationsViewModel = receivedModalSublocationsViewModel;
                        requestItemViewModel.ChildrenLocationInstances = new List<List<LocationInstance>>();
                        requestItemViewModel.ChildrenLocationInstances.Add(receivedModalSublocationsViewModel.locationInstancesSelected);

                        requestItemViewModel.ParentDepthZeroOfSelected = locationType;
                        //requestItemViewModel.ReceivedModalVisualViewModel = receivedModalVisualViewModel;
                    }
                    else
                    {
                        var locationType = parentLocationInstance.LocationType;
                        while (locationType.Depth != 0)
                        {
                            locationType = _context.LocationTypes.Where(l => l.LocationTypeID == locationType.LocationTypeParentID).FirstOrDefault();
                        }

                        receivedLocationViewModel.locationInstancesSelected.Add(parentLocationInstance);
                        requestItemViewModel.ParentDepthZeroOfSelected = locationType;
                        requestItemViewModel.ReceivedLocationViewModel = receivedLocationViewModel;

                        ReceivedModalSublocationsViewModel receivedModalSublocationsViewModel = new ReceivedModalSublocationsViewModel()
                        {
                            locationInstancesDepthZero = _context.LocationInstances.Where(li => li.LocationTypeID == locationType.LocationTypeID && !(li is TemporaryLocationInstance)),
                            locationTypeNames = new List<string>(),
                            locationInstancesSelected = new List<LocationInstance>()
                        };
                        bool finished = false;
                        int locationTypeIDLoop = locationType.LocationTypeID;
                        var parent = parentLocationInstance;
                        receivedModalSublocationsViewModel.locationInstancesSelected.Add(parent);
                        requestItemViewModel.ChildrenLocationInstances = new List<List<LocationInstance>>();
                        requestItemViewModel.ChildrenLocationInstances.Add(_context.LocationInstances.OfType<LocationInstance>().Where(l => l.LocationInstanceParentID == parent.LocationInstanceParentID).OrderBy(l => l.LocationNumber).ToList());
                        while (parent.LocationInstanceParentID != null)
                        {
                            parent = _context.LocationInstances.OfType<LocationInstance>().Where(li => li.LocationInstanceID == parent.LocationInstanceParentID).FirstOrDefault();
                            requestItemViewModel.ChildrenLocationInstances.Add(_context.LocationInstances.OfType<LocationInstance>().Where(l => l.LocationInstanceParentID == parent.LocationInstanceParentID).OrderBy(l => l.LocationNumber).ToList());
                            receivedModalSublocationsViewModel.locationInstancesSelected.Add(parent);
                        }
                        while (!finished)
                        {
                            //need to get the whole thing b/c need both the name and the child id so it's instead of looping through the list twice
                            var nextType = _context.LocationTypes.Where(lt => lt.LocationTypeID == locationTypeIDLoop).FirstOrDefault();
                            string nextTYpeName = nextType.LocationTypeName;
                            int? tryNewLocationType = nextType.LocationTypeChildID;
                            //add it to the list in the viewmodel
                            receivedModalSublocationsViewModel.locationTypeNames.Add(nextTYpeName);

                            //while we're still looping through we'll instantiate the locationInstancesSelected so we can have dropdownlistfors on the view
                            //receivedModalSublocationsViewModel.locationInstancesSelected.Add(new LocationInstance());

                            if (tryNewLocationType == null)
                            {
                                //if its not null we can convert it and pass it in
                                finished = true;
                            }
                            else
                            {
                                locationTypeIDLoop = (Int32)tryNewLocationType;
                            }
                        }
                        requestItemViewModel.ReceivedModalSublocationsViewModel = receivedModalSublocationsViewModel;
                        ReceivedModalVisualViewModel receivedModalVisualViewModel = new ReceivedModalVisualViewModel()
                        {
                            IsEditModalTable = true,
                            ParentLocationInstance = _context.LocationInstances.OfType<LocationInstance>().Where(m => m.LocationInstanceID == parentLocationInstance.LocationInstanceID).FirstOrDefault()
                        };

                        if (receivedModalVisualViewModel.ParentLocationInstance != null)
                        {
                            receivedModalVisualViewModel.RequestChildrenLocationInstances =
                                _context.LocationInstances.OfType<LocationInstance>().Where(m => m.LocationInstanceParentID == parentLocationInstance.LocationInstanceID)
                                .Include(m => m.RequestLocationInstances)
                                .Select(li => new RequestChildrenLocationInstances()
                                {
                                    LocationInstance = li,
                                    IsThisRequest = li.RequestLocationInstances.Select(rli => rli.RequestID).Where(i => i == id).Any()
                                }).OrderBy(m => m.LocationInstance.LocationNumber).ToList();

                            List<LocationInstancePlace> liPlaces = new List<LocationInstancePlace>();
                            foreach (var cli in receivedModalVisualViewModel.RequestChildrenLocationInstances)
                            {
                                liPlaces.Add(new LocationInstancePlace()
                                {
                                    LocationInstanceId = cli.LocationInstance.LocationInstanceID,
                                    Placed = cli.IsThisRequest
                                });
                            }
                            receivedModalVisualViewModel.LocationInstancePlaces = liPlaces;
                            //return NotFound();
                        }
                        requestItemViewModel.ReceivedModalVisualViewModel = receivedModalVisualViewModel;
                    }

                }


            }
            if (selectedPriceSort != null && !isProprietary)
            {
                requestItemViewModel.PricePopoverViewModel = new PricePopoverViewModel();
                List<PriceSortViewModel> priceSorts = new List<PriceSortViewModel>();
                Enum.GetValues(typeof(AppUtility.PriceSortEnum)).Cast<AppUtility.PriceSortEnum>().ToList().ForEach(p => priceSorts.Add(new PriceSortViewModel { PriceSortEnum = p, Selected = selectedPriceSort.Contains(p.ToString()) }));
                requestItemViewModel.PricePopoverViewModel.PriceSortEnums = priceSorts;
                requestItemViewModel.PricePopoverViewModel.SelectedCurrency = (AppUtility.CurrencyEnum)Enum.Parse(typeof(AppUtility.CurrencyEnum), selectedCurrency ?? requestItemViewModel.Requests[0].Currency);
                requestItemViewModel.PricePopoverViewModel.PopoverSource = 2;
            }
            requestItemViewModel.IsProprietary = isProprietary;

            if (requestItemViewModel.Requests.FirstOrDefault() == null)
            {
                TempData["InnerMessage"] = "The request sent in was null";
            }

            ViewData["ModalViewType"] = ModalViewType;
            //ViewData["ApplicationUserID"] = new SelectList(_context.Users, "Id", "Id", addNewItemViewModel.Request.ParentRequest.ApplicationUserID);
            //ViewData["ProductID"] = new SelectList(_context.Products, "ProductID", "ProductName", addNewItemViewModel.Request.ProductID);
            //ViewData["RequestStatusID"] = new SelectList(_context.RequestStatuses, "RequestStatusID", "RequestStatusID", addNewItemViewModel.Request.RequestStatusID);
            return requestItemViewModel;


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
            shareModalViewModel.ApplicationUsers.Insert(0, new SelectListItem() { Selected = true, Disabled = true, Text = "Select User" });

            return shareModalViewModel;
        }

        protected void FillDocumentsInfo(RequestItemViewModel requestItemViewModel, ProductSubcategory productSubcategory, string requestId = null, string parentQuoteId = null)
        {
            requestItemViewModel.DocumentsInfo = new List<DocumentFolder>();
            string quoteFolder = "";
            string requestFolder = "";
            string ordersFolder = "";
            AppUtility.ParentFolderName quoteParentFolderName = AppUtility.ParentFolderName.ParentQuote;
            AppUtility.ParentFolderName requestParentFolderName = AppUtility.ParentFolderName.Requests;
            AppUtility.ParentFolderName parentRequestFolderName = AppUtility.ParentFolderName.ParentRequest;

            if (parentQuoteId != null)
            {
                string quoteParentFolder = Path.Combine(_hostingEnvironment.WebRootPath, quoteParentFolderName.ToString());
                quoteFolder = Path.Combine(quoteParentFolder, parentQuoteId.ToString());
            }
            if (requestItemViewModel.Requests.FirstOrDefault().ParentRequestID != null)
            {
                string parentRequestFolder = Path.Combine(_hostingEnvironment.WebRootPath, parentRequestFolderName.ToString());
                ordersFolder = Path.Combine(parentRequestFolder, requestItemViewModel.Requests.FirstOrDefault().ParentRequestID.ToString());
            }
            if (requestId != null) //eventually change to/add? parent request...
            {
                string requestParentFolder = Path.Combine(_hostingEnvironment.WebRootPath, requestParentFolderName.ToString());
                requestFolder = Path.Combine(requestParentFolder, requestId.ToString());
            }
            if (productSubcategory.ParentCategory.IsProprietary)
            {
                GetExistingFileStrings(requestItemViewModel.DocumentsInfo, AppUtility.FolderNamesEnum.Info, requestParentFolderName, requestFolder, requestId);
                if (productSubcategory.ProductSubcategoryDescription == "Blood" || productSubcategory.ProductSubcategoryDescription == "Serum")
                {
                    GetExistingFileStrings(requestItemViewModel.DocumentsInfo, AppUtility.FolderNamesEnum.S, requestParentFolderName, requestFolder, requestId);
                }
                else if (new List<string>() { "Virus", "Plasmid", "Bacteria with Plasmids" }.Contains(productSubcategory.ProductSubcategoryDescription))
                {
                    GetExistingFileStrings(requestItemViewModel.DocumentsInfo, AppUtility.FolderNamesEnum.Map, requestParentFolderName, requestFolder, requestId);
                }
            }
            else if (requestItemViewModel.ParentCategories.FirstOrDefault().CategoryTypeID == 2)
            {
                
                if(requestItemViewModel.Requests.FirstOrDefault().ParentRequestID !=null)
                {
                    GetExistingFileStrings(requestItemViewModel.DocumentsInfo, AppUtility.FolderNamesEnum.Orders, AppUtility.ParentFolderName.ParentRequest, ordersFolder, requestItemViewModel.Requests.FirstOrDefault().ParentRequestID.ToString());
                }
                if (requestItemViewModel.Requests.FirstOrDefault().Payments != null && requestItemViewModel.Requests.FirstOrDefault().Payments.Count() != 0)
                {
                    if (requestItemViewModel.Requests.FirstOrDefault().Payments.OrderBy(p => p.PaymentDate)?.FirstOrDefault().InvoiceID != null)
                    {
                        GetExistingFileStrings(requestItemViewModel.DocumentsInfo, AppUtility.FolderNamesEnum.Invoices, requestParentFolderName, requestFolder, requestId);
                    }
                }
                GetExistingFileStrings(requestItemViewModel.DocumentsInfo, AppUtility.FolderNamesEnum.Details, requestParentFolderName, requestFolder, requestId);
                if (requestItemViewModel.Requests.FirstOrDefault().ParentQuoteID != null)
                {
                    GetExistingFileStrings(requestItemViewModel.DocumentsInfo, AppUtility.FolderNamesEnum.Quotes, quoteParentFolderName, quoteFolder, parentQuoteId);
                }
            }
            else
            {
                if (requestItemViewModel.Requests.FirstOrDefault().ParentQuoteID != null)
                {
                    GetExistingFileStrings(requestItemViewModel.DocumentsInfo, AppUtility.FolderNamesEnum.Quotes, quoteParentFolderName, quoteFolder, parentQuoteId);
                }
                if (requestItemViewModel.Requests.FirstOrDefault().ParentRequestID != null)
                {
                    GetExistingFileStrings(requestItemViewModel.DocumentsInfo, AppUtility.FolderNamesEnum.Orders, AppUtility.ParentFolderName.ParentRequest, ordersFolder, requestItemViewModel.Requests.FirstOrDefault().ParentRequestID.ToString());
                }
                if (requestItemViewModel.Requests.FirstOrDefault().Payments != null && requestItemViewModel.Requests.FirstOrDefault().Payments.Count() != 0)
                {
                    if (requestItemViewModel.Requests.FirstOrDefault().Payments.OrderBy(p => p.PaymentDate)?.FirstOrDefault().InvoiceID != null)
                    {
                        GetExistingFileStrings(requestItemViewModel.DocumentsInfo, AppUtility.FolderNamesEnum.Invoices, requestParentFolderName, requestFolder, requestId);
                    }
                }
                GetExistingFileStrings(requestItemViewModel.DocumentsInfo, AppUtility.FolderNamesEnum.Shipments, requestParentFolderName, requestFolder, requestId);
                GetExistingFileStrings(requestItemViewModel.DocumentsInfo, AppUtility.FolderNamesEnum.Info, requestParentFolderName, requestFolder, requestId);
                GetExistingFileStrings(requestItemViewModel.DocumentsInfo, AppUtility.FolderNamesEnum.Pictures, requestParentFolderName, requestFolder, requestId);
                //GetExistingFileStrings(requestItemViewModel.DocumentsInfo, AppUtility.RequestFolderNamesEnum.Returns, requestParentFolderName, requestFolder, requestId);
                //GetExistingFileStrings(requestItemViewModel.DocumentsInfo, AppUtility.RequestFolderNamesEnum.Credits, requestParentFolderName, requestFolder, requestId);
            }
        }
        protected async Task<RequestIndexPartialViewModel> GetIndexViewModel(RequestIndexObject requestIndexObject, List<int> Months = null, List<int> Years = null, SelectedFilters selectedFilters = null, int numFilters = 0)
        {
            int categoryID = 1;
            if (requestIndexObject.SectionType == AppUtility.MenuItems.Operations)
            {
                categoryID = 2;
            }
            IQueryable<Request> RequestsPassedIn = Enumerable.Empty<Request>().AsQueryable();
            IQueryable<Request> fullRequestsList = _context.Requests.IgnoreQueryFilters().Where(r=>!r.IsDeleted).Where(r => r.Product.ProductName.Contains(requestIndexObject.SearchText)).Include(r => r.ApplicationUserCreator)
         .Where(r => r.Product.ProductSubcategory.ParentCategory.CategoryTypeID == categoryID)/*.Where(r => r.IsArchived == requestIndexObject.IsArchive)*/;

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
                    TempRequestList = AppUtility.GetRequestsListFromRequestStatusID(fullRequestsList, 3);
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
                .Include(r => r.Product.UnitType).Include(r => r.Product.SubUnitType).Include(r => r.Product.SubSubUnitType);
                    RequestsPassedIn = RequestsPassedIn.Include(r => r.RequestLocationInstances).ThenInclude(li => li.LocationInstance).ThenInclude(l => l.LocationInstanceParent);
                }
            }
            else if (requestIndexObject.PageType == AppUtility.PageTypeEnum.AccountingGeneral)
            {
                //we need both categories
                RequestsPassedIn = _context.Requests.IgnoreQueryFilters().Where(r => !r.IsDeleted).Where(r => r.RequestStatusID == 3)
                    .Where(r => !r.IsClarify && !r.IsPartial && r.Payments.Where(p => p.IsPaid && p.HasInvoice).Count() == r.Payments.Count())
                    .Where(r => Years.Contains(r.ParentRequest.OrderDate.Year));
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
                     .Where(r => r.Product.VendorID == sideBarID);
                    break;
                case AppUtility.SidebarEnum.Type:
                    sidebarFilterDescription = _context.ProductSubcategories.Where(p => p.ProductSubcategoryID == sideBarID).Select(p => p.ProductSubcategoryDescription).FirstOrDefault();
                    RequestsPassedIn = RequestsPassedIn
                   .Where(r => r.Product.ProductSubcategoryID == sideBarID);
                    break;
                case AppUtility.SidebarEnum.Owner:
                    var owner = _context.Employees.Where(e => e.Id.Equals(requestIndexObject.SidebarFilterID)).FirstOrDefault();
                    sidebarFilterDescription = owner.FirstName + " " + owner.LastName;
                    RequestsPassedIn = RequestsPassedIn
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
                .Include(r => r.Product.UnitType).Include(r => r.Product.SubUnitType).Include(r => r.Product.SubSubUnitType).AsQueryable();

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
        protected static IQueryable<Request> filterListBySelectFilters(SelectedFilters selectedFilters, IQueryable<Request> fullRequestsList)
        {
            if (selectedFilters != null)
            {
                if (selectedFilters.SelectedCategoriesIDs.Count() > 0)
                {
                    fullRequestsList = fullRequestsList.Where(r => selectedFilters.SelectedCategoriesIDs.Contains(r.Product.ProductSubcategory.ParentCategoryID));
                }
                if (selectedFilters.SelectedSubcategoriesIDs.Count() > 0)
                {
                    fullRequestsList = fullRequestsList.Where(r => selectedFilters.SelectedSubcategoriesIDs.Contains(r.Product.ProductSubcategoryID));
                }
                if (selectedFilters.SelectedVendorsIDs.Count() > 0)
                {
                    fullRequestsList = fullRequestsList.Where(r => selectedFilters.SelectedVendorsIDs.Contains(r.Product.VendorID ?? 0));
                }
                if (selectedFilters.SelectedLocationsIDs.Count() > 0)
                {
                    fullRequestsList = fullRequestsList.Where(r => selectedFilters.SelectedLocationsIDs.Contains((int)(Math.Floor(r.RequestLocationInstances.FirstOrDefault().LocationInstance.LocationTypeID / 100.0) * 100)));
                }
                if (selectedFilters.SelectedOwnersIDs.Count() > 0)
                {
                    fullRequestsList = fullRequestsList.Where(r => selectedFilters.SelectedOwnersIDs.Contains(r.ApplicationUserCreatorID));
                }
                
            }
            if (selectedFilters?.Archived == true)
            {
                fullRequestsList = fullRequestsList.Where(r => r.IsArchived == true);
            }
            else
            {
                fullRequestsList = fullRequestsList.Where(r => r.IsArchived == false);
            }
            return fullRequestsList;
        }

        private async Task<IPagedList<RequestIndexPartialRowViewModel>> GetColumnsAndRows(RequestIndexObject requestIndexObject, IPagedList<RequestIndexPartialRowViewModel> onePageOfProducts, IQueryable<Request> RequestPassedInWithInclude)
        {
            List<IconColumnViewModel> iconList = new List<IconColumnViewModel>();
            var reorderIcon = new IconColumnViewModel(" icon-add_circle_outline-24px1 ", "#00CA72", "load-order-details", "Reorder");
            var orderOperations = new IconColumnViewModel(" icon-add_circle_outline-24px1 ", "#00CA72", "order-approved-operation", "Order");
            var deleteIcon = new IconColumnViewModel(" icon-delete-24px ", "black", "load-confirm-delete", "Delete");
            var receiveIcon = new IconColumnViewModel(" icon-done-24px ", "#00CA72", "load-receive-and-location", "Receive");
            var approveIcon = new IconColumnViewModel(" icon-centarix-icons-03 ", "#00CA72", "approve-order", "Approve");
            var CantApproveIcon = new IconColumnViewModel(" icon-centarix-icons-03 ", "#E5E5E5", "", "Needs Approval");
            var equipmentIcon = new IconColumnViewModel(" icon-settings-24px-1 ", "var(--lab-man-color);", "create-calibration", "Create Calibration");
            var favoriteIcon = new IconColumnViewModel(" icon-favorite_border-24px", "var(--order-inv-color);", "request-favorite", "Favorite");

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
                            if (await this.IsAuthorizedAsync(requestIndexObject.SectionType, "ApproveOrders"))
                            {
                                iconList.Add(approveIcon);
                            }
                            else
                            {
                                iconList.Add(CantApproveIcon);
                            }
                            popoverMoreIcon.IconPopovers = new List<IconPopoverViewModel>();
                            popoverMoreIcon.IconPopovers.Add(popoverDelete);
                            iconList.Add(popoverMoreIcon);
                            onePageOfProducts = await RequestPassedInWithInclude.OrderByDescending(r => r.CreationDate).Select(r =>
                                    new RequestIndexPartialRowViewModel(AppUtility.IndexTableTypes.Approved,
                                             r, r.Product, r.Product.Vendor, r.Product.ProductSubcategory, r.Product.ProductSubcategory.ParentCategory,
                                             r.Product.UnitType, r.Product.SubUnitType, r.Product.SubSubUnitType, requestIndexObject, iconList, defaultImage)
                                    ).ToPagedListAsync(requestIndexObject.PageNumber == 0 ? 1 : requestIndexObject.PageNumber, 20);
                            break;
                        case 2:
                            iconList.Add(receiveIcon);
                            popoverMoreIcon.IconPopovers = new List<IconPopoverViewModel>();
                            popoverMoreIcon.IconPopovers.Add(popoverDelete);
                            iconList.Add(popoverMoreIcon);
                            onePageOfProducts = await RequestPassedInWithInclude.OrderByDescending(r => r.ParentRequest.OrderDate).Select(r => new RequestIndexPartialRowViewModel(AppUtility.IndexTableTypes.Ordered,
                               r, r.Product, r.Product.Vendor, r.Product.ProductSubcategory, r.Product.ProductSubcategory.ParentCategory,
                                            r.Product.UnitType, r.Product.SubUnitType, r.Product.SubSubUnitType, requestIndexObject, iconList, defaultImage, r.ParentRequest)).ToPagedListAsync(requestIndexObject.PageNumber == 0 ? 1 : requestIndexObject.PageNumber, 20);
                            break;
                        case 3:
                            //iconList.Add(reorderIcon);
                            iconList.Add(favoriteIcon);
                            popoverMoreIcon.IconPopovers = new List<IconPopoverViewModel>() { popoverShare, popoverReorder/*, popoverDelete*/ };
                            iconList.Add(popoverMoreIcon);
                            onePageOfProducts = await RequestPassedInWithInclude.OrderByDescending(r => r.ArrivalDate).Select(r =>
                            new RequestIndexPartialRowViewModel(AppUtility.IndexTableTypes.ReceivedInventory,
                             r, r.Product, r.Product.Vendor, r.Product.ProductSubcategory, r.Product.ProductSubcategory.ParentCategory,
                                          r.Product.UnitType, r.Product.SubUnitType, r.Product.SubSubUnitType, requestIndexObject, iconList, defaultImage, _context.FavoriteRequests.Where(fr => fr.RequestID == r.RequestID).Where(fr => fr.ApplicationUserID == user.Id).FirstOrDefault(),
                                            _context.ShareRequests.Where(sr => sr.RequestID == r.RequestID).Where(sr => sr.ToApplicationUserID == user.Id).Include(sr => sr.FromApplicationUser).FirstOrDefault(), user,
                                            r.RequestLocationInstances.FirstOrDefault().LocationInstance, r.RequestLocationInstances.FirstOrDefault().LocationInstance.LocationInstanceParent, r.ParentRequest)).ToPagedListAsync(requestIndexObject.PageNumber == 0 ? 1 : requestIndexObject.PageNumber, 20);
                            break;

                    }
                    break;
                case AppUtility.PageTypeEnum.OperationsRequest:
                    switch (requestIndexObject.RequestStatusID)
                    {
                        case 2:
                            iconList.Add(receiveIcon);
                            popoverMoreIcon.IconPopovers = new List<IconPopoverViewModel>();
                            popoverMoreIcon.IconPopovers.Add(popoverDelete);
                            iconList.Add(popoverMoreIcon);
                            onePageOfProducts = onePageOfProducts = await RequestPassedInWithInclude.OrderByDescending(r => r.ParentRequest.OrderDate).Select(r => new RequestIndexPartialRowViewModel(AppUtility.IndexTableTypes.OrderedOperations,
                            r, r.Product, r.Product.Vendor, r.Product.ProductSubcategory, r.Product.ProductSubcategory.ParentCategory,
                                         r.Product.UnitType, r.Product.SubUnitType, r.Product.SubSubUnitType, requestIndexObject, iconList, defaultImage, r.ParentRequest, user)
                                       ).ToPagedListAsync(requestIndexObject.PageNumber == 0 ? 1 : requestIndexObject.PageNumber, 20);

                            break;
                        case 3:
                            popoverMoreIcon.IconPopovers = new List<IconPopoverViewModel>();
                            popoverMoreIcon.IconPopovers.Add(popoverDelete);
                            iconList.Add(popoverMoreIcon);
                            onePageOfProducts = onePageOfProducts = await RequestPassedInWithInclude.OrderByDescending(r => r.ParentRequest.OrderDate).Select(r => new RequestIndexPartialRowViewModel(AppUtility.IndexTableTypes.ReceivedInventoryOperations,
                             r, r.Product, r.Product.Vendor, r.Product.ProductSubcategory, r.Product.ProductSubcategory.ParentCategory,
                                          r.Product.UnitType, r.Product.SubUnitType, r.Product.SubSubUnitType, requestIndexObject, iconList, defaultImage, r.ParentRequest, user)
                                        ).ToPagedListAsync(requestIndexObject.PageNumber == 0 ? 1 : requestIndexObject.PageNumber, 20);
                            break;
                    }
                    break;
                case AppUtility.PageTypeEnum.RequestInventory:
                    popoverMoreIcon.IconPopovers = new List<IconPopoverViewModel>() { popoverShare, popoverReorder, popoverDelete };
                    iconList.Add(popoverMoreIcon);
                    onePageOfProducts = onePageOfProducts = onePageOfProducts = await RequestPassedInWithInclude.OrderByDescending(r => r.ArrivalDate).Select(r => new RequestIndexPartialRowViewModel(AppUtility.IndexTableTypes.ReceivedInventory,
                             r, r.Product, r.Product.Vendor, r.Product.ProductSubcategory, r.Product.ProductSubcategory.ParentCategory,
                             r.Product.UnitType, r.Product.SubUnitType, r.Product.SubSubUnitType, requestIndexObject, iconList, defaultImage, _context.FavoriteRequests.Where(fr => fr.RequestID == r.RequestID).Where(fr => fr.ApplicationUserID == user.Id).FirstOrDefault(),
                               _context.ShareRequests.Where(sr => sr.RequestID == r.RequestID).Where(sr => sr.ToApplicationUserID == user.Id).Include(sr => sr.FromApplicationUser).FirstOrDefault(), user,
                         r.RequestLocationInstances.FirstOrDefault().LocationInstance, r.RequestLocationInstances.FirstOrDefault().LocationInstance.LocationInstanceParent, r.ParentRequest)).ToPagedListAsync(requestIndexObject.PageNumber == 0 ? 1 : requestIndexObject.PageNumber, 20);
                    break;
                case AppUtility.PageTypeEnum.RequestSummary:


                    switch (requestIndexObject.RequestStatusID)
                    {
                        case 7:
                            popoverMoreIcon.IconPopovers = new List<IconPopoverViewModel>() { /*popoverShare, popoverReorder,*/ popoverDelete };
                            iconList.Add(favoriteIcon);
                            iconList.Add(popoverMoreIcon);
                            onePageOfProducts = onePageOfProducts = await RequestPassedInWithInclude.OrderByDescending(r => r.CreationDate).Select(r => new RequestIndexPartialRowViewModel(AppUtility.IndexTableTypes.SummaryProprietary,
                            r, r.Product, r.Product.Vendor, r.Product.ProductSubcategory, r.Product.ProductSubcategory.ParentCategory,
                                         r.Product.UnitType, r.Product.SubUnitType, r.Product.SubSubUnitType, requestIndexObject, iconList, defaultImage, _context.FavoriteRequests.Where(fr => fr.RequestID == r.RequestID).Where(fr => fr.ApplicationUserID == user.Id).FirstOrDefault(),
                                          _context.ShareRequests.Where(sr => sr.RequestID == r.RequestID).Where(sr => sr.ToApplicationUserID == user.Id).Include(sr => sr.FromApplicationUser).FirstOrDefault(), user,
                                          r.RequestLocationInstances.FirstOrDefault().LocationInstance, r.RequestLocationInstances.FirstOrDefault().LocationInstance.LocationInstanceParent, r.ParentRequest)
                                       ).ToPagedListAsync(requestIndexObject.PageNumber == 0 ? 1 : requestIndexObject.PageNumber, 20);

                            break;
                        default:
                            popoverMoreIcon.IconPopovers = new List<IconPopoverViewModel>() { popoverShare, popoverReorder, /*popoverDelete*/ };
                            iconList.Add(favoriteIcon);
                            iconList.Add(popoverMoreIcon);
                            onePageOfProducts = onePageOfProducts = await RequestPassedInWithInclude.OrderByDescending(r => r.ParentRequest.OrderDate).Select(r => new RequestIndexPartialRowViewModel(AppUtility.IndexTableTypes.Summary,
                             r, r.Product, r.Product.Vendor, r.Product.ProductSubcategory, r.Product.ProductSubcategory.ParentCategory,
                                    r.Product.UnitType, r.Product.SubUnitType, r.Product.SubSubUnitType, requestIndexObject, iconList, defaultImage, _context.FavoriteRequests.Where(fr => fr.RequestID == r.RequestID).Where(fr => fr.ApplicationUserID == user.Id).FirstOrDefault(),
                                    _context.ShareRequests.Where(sr => sr.RequestID == r.RequestID).Where(sr => sr.ToApplicationUserID == user.Id).Include(sr => sr.FromApplicationUser).FirstOrDefault(), user,
                                    r.RequestLocationInstances.FirstOrDefault().LocationInstance, r.RequestLocationInstances.FirstOrDefault().LocationInstance.LocationInstanceParent, r.ParentRequest)
                           ).ToLookup(r => r.r.ProductID).Select(e => e.First()).ToPagedListAsync(requestIndexObject.PageNumber == 0 ? 1 : requestIndexObject.PageNumber, 20);
                           /// .GroupBy(r => r.ProductID, (key, value) => value.OrderByDescending(v => v.ParentRequest.OrderDate).First()).AsQueryable();
                            break;
                    }

                    break;
                case AppUtility.PageTypeEnum.OperationsInventory:
                    iconList.Add(orderOperations);
                    popoverMoreIcon.IconPopovers = new List<IconPopoverViewModel>();
                    popoverMoreIcon.IconPopovers.Add(popoverDelete);
                    iconList.Add(popoverMoreIcon);
                    onePageOfProducts = onePageOfProducts = await RequestPassedInWithInclude.OrderByDescending(r => r.ParentRequest.OrderDate).Select(r => new RequestIndexPartialRowViewModel(AppUtility.IndexTableTypes.ReceivedInventoryOperations,
                             r, r.Product, r.Product.Vendor, r.Product.ProductSubcategory, r.Product.ProductSubcategory.ParentCategory,
                                          r.Product.UnitType, r.Product.SubUnitType, r.Product.SubSubUnitType, requestIndexObject, iconList, defaultImage, r.ParentRequest)
                                        ).ToPagedListAsync(requestIndexObject.PageNumber == 0 ? 1 : requestIndexObject.PageNumber, 20);
                    break;
                case AppUtility.PageTypeEnum.LabManagementEquipment:
                    iconList.Add(equipmentIcon);
                    popoverMoreIcon.IconPopovers = new List<IconPopoverViewModel>() { popoverShare, popoverReorder, popoverDelete };
                    iconList.Add(popoverMoreIcon);
                    onePageOfProducts = await RequestPassedInWithInclude.OrderByDescending(r => r.ParentRequest.OrderDate).Select(r =>
                            new RequestIndexPartialRowViewModel(AppUtility.IndexTableTypes.ReceivedInventory,
                             r, r.Product, r.Product.Vendor, r.Product.ProductSubcategory, r.Product.ProductSubcategory.ParentCategory,
                                          r.Product.UnitType, r.Product.SubUnitType, r.Product.SubSubUnitType, requestIndexObject, iconList, defaultImage, _context.FavoriteRequests.Where(fr => fr.RequestID == r.RequestID).Where(fr => fr.ApplicationUserID == user.Id).FirstOrDefault(), user, r.ParentRequest)).ToPagedListAsync(requestIndexObject.PageNumber == 0 ? 1 : requestIndexObject.PageNumber, 20);
                    break;
                case AppUtility.PageTypeEnum.ExpensesStatistics:
                    break;
                case AppUtility.PageTypeEnum.AccountingGeneral:
                    onePageOfProducts = onePageOfProducts = await RequestPassedInWithInclude.OrderByDescending(r => r.ParentRequest.OrderDate).Select(r => new RequestIndexPartialRowViewModel(AppUtility.IndexTableTypes.AccountingGeneral,
                             r, r.Product, r.Product.Vendor, r.Product.ProductSubcategory, r.Product.ProductSubcategory.ParentCategory,
                                          r.Product.UnitType, r.Product.SubUnitType, r.Product.SubSubUnitType, requestIndexObject, iconList, defaultImage, r.ParentRequest)).ToPagedListAsync(requestIndexObject.PageNumber == 0 ? 1 : requestIndexObject.PageNumber, 20);

                    break;
                case AppUtility.PageTypeEnum.RequestCart:
                    switch (requestIndexObject.SidebarType)
                    {
                        case AppUtility.SidebarEnum.Favorites:
                            iconList.Add(favoriteIcon);
                            popoverMoreIcon.IconPopovers = new List<IconPopoverViewModel>() { popoverReorder, popoverShare/*, popoverDelete*/ };
                            iconList.Add(popoverMoreIcon);
                            onePageOfProducts = await RequestPassedInWithInclude.OrderByDescending(r => r.ParentRequest.OrderDate).Select(r =>
                           new RequestIndexPartialRowViewModel(AppUtility.IndexTableTypes.ReceivedInventoryFavorites,
                            r, r.Product, r.Product.Vendor, r.Product.ProductSubcategory, r.Product.ProductSubcategory.ParentCategory,
                                         r.Product.UnitType, r.Product.SubUnitType, r.Product.SubSubUnitType, requestIndexObject, iconList, defaultImage, _context.FavoriteRequests.Where(fr => fr.RequestID == r.RequestID).Where(fr => fr.ApplicationUserID == user.Id).FirstOrDefault(),
                                         user, r.RequestLocationInstances.FirstOrDefault().LocationInstance, r.RequestLocationInstances.FirstOrDefault().LocationInstance.LocationInstanceParent, r.ParentRequest)).ToPagedListAsync(requestIndexObject.PageNumber == 0 ? 1 : requestIndexObject.PageNumber, 20);
                            break;
                        case AppUtility.SidebarEnum.SharedRequests:
                            //iconList.Add(reorderIcon);
                            iconList.Add(favoriteIcon);
                            popoverMoreIcon.IconPopovers = new List<IconPopoverViewModel>() { popoverReorder, popoverShare, popoverRemoveShare/*, popoverDelete*/ };
                            iconList.Add(popoverMoreIcon);
                            onePageOfProducts = await RequestPassedInWithInclude.OrderByDescending(r => r.ParentRequest.OrderDate).Select(r =>
                            new RequestIndexPartialRowViewModel(AppUtility.IndexTableTypes.ReceivedInventoryShared,
                             r, r.Product, r.Product.Vendor, r.Product.ProductSubcategory, r.Product.ProductSubcategory.ParentCategory,
                                          r.Product.UnitType, r.Product.SubUnitType, r.Product.SubSubUnitType, requestIndexObject, iconList, defaultImage,
                                          _context.FavoriteRequests.Where(fr => fr.RequestID == r.RequestID).Where(fr => fr.ApplicationUserID == user.Id).FirstOrDefault(),
                                          _context.ShareRequests
                .Where(sr => sr.RequestID == r.RequestID).Where(sr => sr.ToApplicationUserID == user.Id).Include(sr => sr.FromApplicationUser).FirstOrDefault(), user,
                                          r.RequestLocationInstances.FirstOrDefault().LocationInstance, r.RequestLocationInstances.FirstOrDefault().LocationInstance.LocationInstanceParent, r.ParentRequest)).ToPagedListAsync(requestIndexObject.PageNumber == 0 ? 1 : requestIndexObject.PageNumber, 20);
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
                    SectionType = sectionType,
                    Archive = selectedFilters.Archived, 
                    IsProprietary = isProprietary
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
                    SectionType = sectionType,
                    IsProprietary = isProprietary
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

        protected void RevertDocuments(int id, AppUtility.ParentFolderName parentFolderName, Guid? guid, bool additionalRequests = false)
        {
            string uploadFolder = Path.Combine(_hostingEnvironment.WebRootPath, parentFolderName.ToString());
            var TempFolderName = guid == null ? "0" : guid.ToString();
            string requestFolderTo = Path.Combine(uploadFolder, TempFolderName);
            string requestFolderFrom = Path.Combine(uploadFolder, id.ToString());
            if (Directory.Exists(requestFolderFrom))
            {
                if (Directory.Exists(requestFolderTo))
                {
                    Directory.Delete(requestFolderTo, true);
                }
                if (additionalRequests)
                {
                    AppUtility.DirectoryCopy(requestFolderFrom, requestFolderTo, true);
                }
                else if (requestFolderFrom != requestFolderTo)
                {
                    Directory.Move(requestFolderFrom, requestFolderTo);
                }
            }
        }

        protected void MoveDocumentsOutOfTempFolder(int id, AppUtility.ParentFolderName parentFolderName, bool additionalRequests = false, Guid? guid = null)
        {
            //rename temp folder to the request id
            string uploadFolder = Path.Combine(_hostingEnvironment.WebRootPath, parentFolderName.ToString());
            var TempFolderName = guid == null ? "0" : guid.ToString();
            string requestFolderFrom = Path.Combine(uploadFolder, TempFolderName);
            string requestFolderTo = Path.Combine(uploadFolder, id.ToString());
            if (Directory.Exists(requestFolderFrom))
            {
                if (Directory.Exists(requestFolderTo))
                {
                    Directory.Delete(requestFolderTo, true);
                }
                if (additionalRequests)
                {
                    AppUtility.DirectoryCopy(requestFolderFrom, requestFolderTo, true);
                }
                else if(requestFolderFrom != requestFolderTo)
                {
                    Directory.Move(requestFolderFrom, requestFolderTo);
                }
            }
        }

        protected void MoveDocumentsOutOfTempFolder(int id, AppUtility.ParentFolderName parentFolderName, AppUtility.FolderNamesEnum folderName, bool additionalRequests = false, Guid? guid = null)
        {
            //rename temp folder to the request id
            string uploadFolder = Path.Combine(_hostingEnvironment.WebRootPath, parentFolderName.ToString());   
            var TempFolderName = guid == null ? "0" : guid.ToString();
            string requestFolderFrom = Path.Combine(uploadFolder, TempFolderName);
            string uplaodFolderPathFrom = Path.Combine(requestFolderFrom, folderName.ToString());
            string requestFolderTo = Path.Combine(uploadFolder, id.ToString());
            string uplaodFolderPathTo = Path.Combine(requestFolderTo, folderName.ToString());
            if (Directory.Exists(uplaodFolderPathFrom))
            {
                if (Directory.Exists(uplaodFolderPathTo))
                {
                    //for now we are overwriting becuase it will never enter this if
                    Directory.Delete(uplaodFolderPathTo, true);
                }
                if (additionalRequests)
                {
                    AppUtility.DirectoryCopy(uplaodFolderPathFrom, uplaodFolderPathTo, true);
                }
                else if (uplaodFolderPathFrom != uplaodFolderPathTo)
                {
                    Directory.Move(requestFolderFrom, uplaodFolderPathTo);
                }
            }
        }


        protected async Task<RequestItemViewModel> FillRequestDropdowns(RequestItemViewModel requestItemViewModel, ProductSubcategory productSubcategory, int categoryTypeId)
        {
            var parentcategories = new List<ParentCategory>();
            var productsubcategories = new List<ProductSubcategory>();
            var unittypes = _context.UnitTypes.Include(u => u.UnitParentType).OrderBy(u => u.UnitParentType.UnitParentTypeID).ThenBy(u => u.UnitTypeDescription);
            if (productSubcategory != null)
            {
                if (categoryTypeId == 1)
                {
                    parentcategories = await _context.ParentCategories.Where(pc => pc.ParentCategoryID == productSubcategory.ParentCategoryID).ToListAsync();
                }
                else
                {
                    parentcategories = await _context.ParentCategories.Where(pc => pc.CategoryTypeID == 2).ToListAsync();
                }
                productsubcategories = await _context.ProductSubcategories.Where(ps => ps.ParentCategoryID == productSubcategory.ParentCategoryID).ToListAsync();
                unittypes = _context.UnitTypes.Where(ut => ut.UnitTypeParentCategory.Where(up => up.ParentCategoryID == productSubcategory.ParentCategoryID).Count() > 0).Include(u => u.UnitParentType).OrderBy(u => u.UnitParentType.UnitParentTypeID).ThenBy(u => u.UnitTypeDescription);
            }
            else
            {
                if (requestItemViewModel.IsProprietary)
                {
                    var proprietarycategory = await _context.ParentCategories.Where(pc => pc.ParentCategoryDescription == AppUtility.ParentCategoryEnum.Samples.ToString()).FirstOrDefaultAsync();
                    productsubcategories = await _context.ProductSubcategories.Where(ps => ps.ParentCategoryID == proprietarycategory.ParentCategoryID).ToListAsync();
                }
                else
                {
                    parentcategories = await _context.ParentCategories.Where(pc => pc.CategoryTypeID == categoryTypeId && !pc.IsProprietary).ToListAsync();
                }
            }
            var vendors = await _context.Vendors.Where(v => v.VendorCategoryTypes.Where(vc => vc.CategoryTypeID == categoryTypeId).Count() > 0).ToListAsync();
            var projects = await _context.Projects.ToListAsync();
            var subprojects = await _context.SubProjects.ToListAsync();
            var unittypeslookup = unittypes.ToLookup(u => u.UnitParentType);
            var paymenttypes = await _context.PaymentTypes.ToListAsync();
            var companyaccounts = await _context.CompanyAccounts.ToListAsync();
            List<AppUtility.CommentTypeEnum> commentTypes = Enum.GetValues(typeof(AppUtility.CommentTypeEnum)).Cast<AppUtility.CommentTypeEnum>().ToList();

            requestItemViewModel.ParentCategories = parentcategories;
            requestItemViewModel.ProductSubcategories = productsubcategories;
            requestItemViewModel.Vendors = vendors;
            requestItemViewModel.Projects = projects;
            requestItemViewModel.SubProjects = subprojects;

            requestItemViewModel.UnitTypeList = new SelectList(unittypes, "UnitTypeID", "UnitTypeDescription", null, "UnitParentType.UnitParentTypeDescription");
            requestItemViewModel.UnitTypes = unittypeslookup;
            requestItemViewModel.CommentTypes = commentTypes;
            requestItemViewModel.PaymentTypes = paymenttypes;
            requestItemViewModel.CompanyAccounts = companyaccounts;
            return requestItemViewModel;
        }

        [Authorize(Roles = "Requests")]
        protected async Task<string> RenderPartialViewToString(string viewName, object model)
        {
            if (string.IsNullOrEmpty(viewName))
                viewName = ControllerContext.ActionDescriptor.ActionName;

            ViewData.Model = model;

            using (var writer = new StringWriter())
            {
                ViewEngineResult viewResult =
                    _viewEngine.FindView(ControllerContext, viewName, false);

                ViewContext viewContext = new ViewContext(
                    ControllerContext,
                    viewResult.View,
                    ViewData,
                    TempData,
                    writer,
                    new HtmlHelperOptions()
                );

                await viewResult.View.RenderAsync(viewContext);

                return writer.GetStringBuilder().ToString();
            }
        }
        public string GetSerialNumber(bool isOperations)
        {
            var categoryType = 1;
            var serialLetter = "L";
            int lastSerialNumberInt = 0;
            if (isOperations)
            {
                categoryType = 2;
                serialLetter = "P";
            }
            var serialnumberList = _context.Products.IgnoreQueryFilters().Where(p => p.ProductSubcategory.ParentCategory.CategoryTypeID == categoryType)
                .Select(p => int.Parse(p.SerialNumber.Substring(1))).ToList();

            lastSerialNumberInt = serialnumberList.OrderBy(s => s).LastOrDefault();
            
            return serialLetter + (lastSerialNumberInt + 1);
        }


        //[HttpPost]
        //[Authorize(Roles = "Requests")]
        //public async Task<IActionResult> TermsModal(TermsViewModel termsViewModel)
        //{
        //    return new EmptyResult();
        //}
    }
}
