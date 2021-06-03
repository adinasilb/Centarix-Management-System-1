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
        protected SharedController(ApplicationDbContext context, UserManager<ApplicationUser> userManager = null, IHostingEnvironment hostingEnvironment = null)
        {
            _context = context;
            _hostingEnvironment = hostingEnvironment;
            _userManager = userManager;
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

        [Authorize(Roles = "Requests Protocols")]
        protected async Task<RequestItemViewModel> editModalViewFunction(int? id, int? Tab = 0, AppUtility.MenuItems SectionType = AppUtility.MenuItems.Requests,
           bool isEditable = true)
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

            var productId = _context.Requests.Where(r => r.RequestID == id).Select(r => r.ProductID).FirstOrDefault();

            var request = _context.Requests.Include(r => r.Product)
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

            var requestsByProduct = _context.Requests.Where(r => r.ProductID == productId && (r.RequestStatusID == 3))
                 .Include(r => r.Product.ProductSubcategory).Include(r => r.Product.ProductSubcategory.ParentCategory)
                    .Include(r => r.ApplicationUserCreator) //do we have to have a separate list of payments to include the inside things (like company account and payment types?)
                    .Include(r => r.ParentRequest)
                    .Where(r => r.Product.ProductSubcategory.ParentCategory.CategoryTypeID == categoryType).Include(r => r.ApplicationUserReceiver)
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

            ModalViewType = "Edit";
            requestItemViewModel.Requests.Add(request);

            //load the correct list of subprojects
            //var subprojects = await _context.SubProjects
            //    .Where(sp => sp.ProjectID == requestItemViewModel.Request.SubProject.ProjectID)
            //    .ToListAsync();
            //requestItemViewModel.SubProjects = subprojects;
            //may be able to do this together - combining the path for the orders folders
            string uploadFolder1 = Path.Combine(_hostingEnvironment.WebRootPath, AppUtility.ParentFolderName.Requests.ToString());
            string uploadFolder2 = Path.Combine(uploadFolder1, requestItemViewModel.Requests.FirstOrDefault().RequestID.ToString());
            requestItemViewModel.DocumentsInfo = new List<DocumentFolder>();

            //the partial file name that we will search for (1- because we want the first one)
            //creating the directory from the path made earlier
            var productSubcategory = requestItemViewModel.Requests.FirstOrDefault().Product.ProductSubcategory;

            FillDocumentsInfo(requestItemViewModel, uploadFolder2, productSubcategory);

            //locations:
            //get the list of requestLocationInstances in this request
            //can't look for _context.RequestLocationInstances b/c it's a join table and doesn't have a dbset
            var request1 = _context.Requests.Where(r => r.RequestID == id).Include(r => r.RequestLocationInstances).ThenInclude(rli => rli.LocationInstance).FirstOrDefault();
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
                    //int? locationInstanceParentID = _context.LocationInstances.Where(li => li.LocationInstanceID == requestLocationInstances[0].LocationInstanceID).FirstOrDefault().LocationInstanceParentID;
                    LocationInstance parentLocationInstance = _context.LocationInstances.Where(li => li.LocationInstanceID == requestLocationInstances[0].LocationInstance.LocationInstanceParentID).Include(li => li.LocationType).FirstOrDefault();
                    //requestItemViewModel.ParentLocationInstance = _context.LocationInstances.Where(li => li.LocationInstanceID == requestLocationInstances[0].LocationInstance.LocationInstanceParentID).FirstOrDefault();
                    //need to test b/c the model is int? which is nullable

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
                        locationInstancesDepthZero = _context.LocationInstances.Where(li => li.LocationTypeID == locationType.LocationTypeID),
                        locationTypeNames = new List<string>(),
                        locationInstancesSelected = new List<LocationInstance>()
                    };
                    bool finished = false;
                    int locationTypeIDLoop = locationType.LocationTypeID;
                    var parent = parentLocationInstance;
                    receivedModalSublocationsViewModel.locationInstancesSelected.Add(parent);
                    requestItemViewModel.ChildrenLocationInstances = new List<List<LocationInstance>>();
                    requestItemViewModel.ChildrenLocationInstances.Add(_context.LocationInstances.Where(l => l.LocationInstanceParentID == parent.LocationInstanceParentID).ToList());
                    while (parent.LocationInstanceParentID != null)
                    {
                        parent = _context.LocationInstances.Where(li => li.LocationInstanceID == parent.LocationInstanceParentID).FirstOrDefault();
                        requestItemViewModel.ChildrenLocationInstances.Add(_context.LocationInstances.Where(l => l.LocationInstanceParentID == parent.LocationInstanceParentID).ToList());
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
                        ParentLocationInstance = _context.LocationInstances.Where(m => m.LocationInstanceID == parentLocationInstance.LocationInstanceID).FirstOrDefault()
                    };

                    if (receivedModalVisualViewModel.ParentLocationInstance != null)
                    {
                        receivedModalVisualViewModel.RequestChildrenLocationInstances =
                            _context.LocationInstances.Where(m => m.LocationInstanceParentID == parentLocationInstance.LocationInstanceID)
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

        protected bool SetFavorite<T1, T2>(T1 ModelInstanceID, T2 FavoriteTable, bool IsFavorite)
        {
            return true;
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

        public static double GetTotalWorkingDaysByInterval(DateTime startDate, List<CompanyDayOff> companyDayOffs, DateTime endDate)
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

        protected async Task<RequestIndexPartialViewModel> GetIndexViewModel(RequestIndexObject requestIndexObject, List<int> Months = null, List<int> Years = null, SelectedFilters selectedFilters = null, string searchText = "", int numFilters = 0)
        {
            int categoryID = 1;
            if (requestIndexObject.SectionType == AppUtility.MenuItems.Operations)
            {
                categoryID = 2;
            }
            IQueryable<Request> RequestsPassedIn = Enumerable.Empty<Request>().AsQueryable();
            IQueryable<Request> fullRequestsList = _context.Requests.Where(r => r.Product.ProductName.Contains(searchText ?? "")).Include(r => r.ApplicationUserCreator)
         .Where(r => r.Product.ProductSubcategory.ParentCategory.CategoryTypeID == categoryID).Where(r=> r.IsArchived == requestIndexObject.IsArchive);

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
                RequestsPassedIn = _context.Requests.Where(r=>r.RequestStatusID==3).Where(r => Years.Contains(r.ParentRequest.OrderDate.Year)).Where(r => !r.IsClarify && !r.IsPartial && r.Payments.Where(p => p.IsPaid && p.HasInvoice).Count() == r.Payments.Count());
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

            RequestPassedInWithInclude = RequestPassedInWithInclude.Include(r => r.RequestLocationInstances).ThenInclude(li => li.LocationInstance).ThenInclude(l=>l.LocationInstanceParent);

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



        protected async Task<IPagedList<RequestIndexPartialRowViewModel>> GetColumnsAndRows(RequestIndexObject requestIndexObject, IPagedList<RequestIndexPartialRowViewModel> onePageOfProducts, IQueryable<Request> RequestPassedInWithInclude)
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
                            onePageOfProducts =  await RequestPassedInWithInclude.OrderByDescending(r => r.ParentRequest.OrderDate).Select(r => new RequestIndexPartialRowViewModel(AppUtility.IndexTableTypes.Ordered,
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
                            onePageOfProducts =  await RequestPassedInWithInclude.OrderByDescending(r => r.ParentRequest.OrderDate).Select(r =>
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
      
        protected void SetViewModelCounts(RequestIndexObject requestIndexObject, RequestIndexPartialViewModel viewmodel, SelectedFilters selectedFilters = null, string searchText = "")
        {
            int categoryID = 0;
            if (requestIndexObject.SectionType == AppUtility.MenuItems.Requests)
            {
                categoryID = 1;
            }
            else if (requestIndexObject.SectionType == AppUtility.MenuItems.Operations)
            {
                categoryID = 2;
            }
            IQueryable<Request> fullRequestsList = _context.Requests
              .Where(r => r.Product.ProductSubcategory.ParentCategory.CategoryTypeID == categoryID)
              .Include(r => r.ApplicationUserCreator).Include(r => r.Product).ThenInclude(p => p.Vendor);
            IQueryable<Request> changingList = _context.Requests.Where(r => r.Product.ProductName.Contains(searchText ?? ""))
                .Where(r => r.Product.ProductSubcategory.ParentCategory.CategoryTypeID == categoryID)
                .Include(r => r.ApplicationUserCreator).Include(r => r.Product).ThenInclude(p => p.Vendor);
            changingList = filterListBySelectFilters(selectedFilters, changingList);

            int[] requestStatusIds = { 1, 2, 3, 6 };
            int[] newRequestStatusIds = new int[2];
            if (requestIndexObject.RequestStatusID != 6)
            {
                newRequestStatusIds[0] = requestIndexObject.RequestStatusID;
            }
            else //for approval and approved are combined
            {
                newRequestStatusIds[0] = 1;
                newRequestStatusIds[1] = 6;
            }
            requestStatusIds = requestStatusIds.Where(id => !newRequestStatusIds.Contains(id)).ToArray();
            foreach (int statusId in requestStatusIds)
            {
                SetCountByStatusId(requestIndexObject, viewmodel, fullRequestsList, statusId);
            }
            foreach (int statusId in newRequestStatusIds)
            {
                SetCountByStatusId(requestIndexObject, viewmodel, changingList, statusId);
            }

            /*int newCount = AppUtility.GetCountOfRequestsByRequestStatusIDVendorIDSubcategoryIDApplicationUserID(fullRequestsList, 1, requestIndexObject.SidebarType, requestIndexObject.SidebarFilterID);
            int orderedCount = AppUtility.GetCountOfRequestsByRequestStatusIDVendorIDSubcategoryIDApplicationUserID(fullRequestsList, 2, requestIndexObject.SidebarType, requestIndexObject.SidebarFilterID);
            int receivedCount = AppUtility.GetCountOfRequestsByRequestStatusIDVendorIDSubcategoryIDApplicationUserID(fullRequestsList, 3, requestIndexObject.SidebarType, requestIndexObject.SidebarFilterID);
            int approvedCount = AppUtility.GetCountOfRequestsByRequestStatusIDVendorIDSubcategoryIDApplicationUserID(fullRequestsList, 6, requestIndexObject.SidebarType, requestIndexObject.SidebarFilterID);
            viewmodel.NewCount = newCount;
            viewmodel.ApprovedCount = approvedCount;
            viewmodel.OrderedCount = orderedCount;
            viewmodel.ReceivedCount = receivedCount;*/
        }
        protected static void SetCountByStatusId(RequestIndexObject requestIndexObject, RequestIndexPartialViewModel viewmodel, IQueryable<Request> requestsList, int statusId)
        {
            int count = AppUtility.GetCountOfRequestsByRequestStatusIDVendorIDSubcategoryIDApplicationUserID(requestsList, statusId, requestIndexObject.SidebarType, requestIndexObject.SidebarFilterID);
            switch (statusId)
            {
                case 1:
                    viewmodel.ApprovedCount += count;
                    break;
                case 2:
                    viewmodel.OrderedCount = count;
                    break;
                case 3:
                    viewmodel.ReceivedCount = count;
                    break;
                case 6:
                    viewmodel.ApprovedCount += count;
                    break;
                default:
                    break;
            }
        }


        [HttpGet]
        [HttpPost]
        [Authorize(Roles = "Requests, Operations")] //redo this later
        public async Task<IActionResult> _IndexTableWithCounts(RequestIndexObject requestIndexObject, SelectedFilters selectedFilters = null, string searchText = "", int numFilters = 0)
        {
            RequestIndexPartialViewModel viewModel = await GetIndexViewModel(requestIndexObject, selectedFilters: selectedFilters, searchText: searchText, numFilters: numFilters);
            SetViewModelCounts(requestIndexObject, viewModel, selectedFilters, searchText);
            if (TempData["RequestStatus"]?.ToString() == "1")
            {
                Response.StatusCode = 210;
            }
            return PartialView(viewModel);
        }


        [HttpGet]
        [HttpPost]
        [Authorize(Roles = "Requests")]
        public async Task<IActionResult> _IndexTable(RequestIndexObject requestIndexObject, SelectedFilters selectedFilters = null, string searchText = "", int numFilters = 0)
        {
            RequestIndexPartialViewModel viewModel = await GetIndexViewModel(requestIndexObject, selectedFilters: selectedFilters, searchText: searchText, numFilters: numFilters);
            return PartialView(viewModel);
        }


        [HttpGet]
        [HttpPost]
        [Authorize(Roles = "Requests")]
        public async Task<IActionResult> _IndexTableWithProprietaryTabs(RequestIndexObject requestIndexObject, List<int> months, List<int> years, SelectedFilters selectedFilters = null, string searchText = "", int numFilters = 0)
        {
            RequestIndexPartialViewModel viewModel = await GetIndexViewModel(requestIndexObject, months, years, selectedFilters, searchText, numFilters);
            SetViewModelProprietaryCounts(requestIndexObject, viewModel, selectedFilters, searchText);
            return PartialView(viewModel);
        }


        [HttpGet]
        [Authorize(Roles = "Requests")]
        public async Task<IActionResult> _IndexTableData(RequestIndexObject requestIndexObject, List<int> months, List<int> years)
        {
            RequestIndexPartialViewModel viewModel = await GetIndexViewModel(requestIndexObject, months, years);

            return PartialView(viewModel);
        }



        protected void SetViewModelProprietaryCounts(RequestIndexObject requestIndexObject, RequestIndexPartialViewModel viewmodel, SelectedFilters selectedFilters = null, string searchText = "")
        {
            int categoryID = 0;
            if (requestIndexObject.SectionType == AppUtility.MenuItems.Requests)
            {
                categoryID = 1;
            }
            else if (requestIndexObject.SectionType == AppUtility.MenuItems.Operations)
            {
                categoryID = 2;
            }
            IQueryable<Request> fullRequestsList = _context.Requests.Include(r => r.ApplicationUserCreator).Where(r => r.Product.ProductSubcategory.ParentCategory.CategoryTypeID == categoryID)
                .Where(r => r.RequestStatus.RequestStatusID == 3).Include(r => r.Product).ThenInclude(p => p.Vendor).ToList().GroupBy(r => r.ProductID).Select(e => e.First()).AsQueryable();
            IQueryable<Request> fullRequestsListProprietary = _context.Requests.Include(r => r.ApplicationUserCreator).Include(r => r.Product).ThenInclude(p => p.Vendor)
                    .Where(r => r.Product.ProductSubcategory.ParentCategory.CategoryTypeID == categoryID);
            if (requestIndexObject.RequestStatusID == 7)
            {
                fullRequestsListProprietary = filterListBySelectFilters(selectedFilters, fullRequestsListProprietary);
                fullRequestsListProprietary = fullRequestsListProprietary.Where(r => r.Product.ProductName.Contains(searchText ?? ""));
            }
            else
            {
                fullRequestsList = filterListBySelectFilters(selectedFilters, fullRequestsList);
                fullRequestsList = fullRequestsList.Where(r => r.Product.ProductName.Contains(searchText ?? ""));
            }

            int nonProprietaryCount = AppUtility.GetCountOfRequestsByRequestStatusIDVendorIDSubcategoryIDApplicationUserID(fullRequestsList, 3, requestIndexObject.SidebarType, requestIndexObject.SidebarFilterID);
            int proprietaryCount = AppUtility.GetCountOfRequestsByRequestStatusIDVendorIDSubcategoryIDApplicationUserID(fullRequestsListProprietary, 7, requestIndexObject.SidebarType, requestIndexObject.SidebarFilterID);
            viewmodel.ProprietaryCount = proprietaryCount;
            viewmodel.NonProprietaryCount = nonProprietaryCount;
        }


        [HttpGet]
        [Authorize(Roles = "Requests")]
        public async Task<IActionResult> _IndexSharedTable()
        {
            RequestIndexPartialViewModel viewModel = await GetSharedRequestIndexObjectAsync();
            return PartialView(viewModel);
        }

        [Authorize(Roles = "Requests")]
        public async Task<RequestIndexPartialViewModel> GetSharedRequestIndexObjectAsync()
        {
            RequestIndexObject requestIndexObject = new RequestIndexObject()
            {
                PageType = AppUtility.PageTypeEnum.RequestCart,
                SidebarType = AppUtility.SidebarEnum.SharedRequests
            };
            RequestIndexPartialViewModel viewModel = await GetIndexViewModel(requestIndexObject);
            return viewModel;
        }

        [Authorize(Roles = "Requests,Operations")]
        public async Task<RedirectToActionResult> SaveAddItemView(RequestItemViewModel requestItemViewModel, AppUtility.OrderTypeEnum OrderType, ReceivedModalVisualViewModel receivedModalVisualViewModel = null)
        {
            try
            {
                RemoveRequestWithCommentsAndEmailSessions();
                var vendor = _context.Vendors.FirstOrDefault(v => v.VendorID == requestItemViewModel.Requests.FirstOrDefault().Product.VendorID);
                var categoryType = 1;
                var serialLetter = "L";
                var exchangeRate = requestItemViewModel.Requests.FirstOrDefault().ExchangeRate;
                var currency = requestItemViewModel.Requests.FirstOrDefault().Currency;
                if (OrderType == AppUtility.OrderTypeEnum.SaveOperations)
                {
                    categoryType = 2;
                    serialLetter = "P";
                }
                var productSubcategories = _context.ProductSubcategories.Include(ps => ps.ParentCategory).Where(ps => ps.ParentCategory.CategoryTypeID == categoryType).ToList();
                //in case we need to return to the modal view
                //requestItemViewModel.ParentCategory = await _context.ParentCategories.Where(pc => pc.ParentCategoryID == requestItemViewModel.Request.Product.ProductSubcategory.ParentCategory.ParentCategoryID).FirstOrDefaultAsync();

                //declared outside the if b/c it's used farther down too 
                var currentUser = _context.Users.FirstOrDefault(u => u.Id == _userManager.GetUserId(User));
                var lastSerialNumber = Int32.Parse((_context.Products.Where(p => p.ProductSubcategory.ParentCategory.CategoryTypeID == categoryType).ToList().OrderBy(p => p.ProductCreationDate).LastOrDefault()?.SerialNumber ?? serialLetter + "0").Substring(1));

                var RequestNum = 1;
                var i = 1;
                var additionalRequests = false;
                foreach (var request in requestItemViewModel.Requests)
                {
                    if (!request.Ignore)
                    {
                        request.ApplicationUserCreatorID = currentUser.Id;
                        if (!requestItemViewModel.IsProprietary)
                        {
                            request.Product.VendorID = vendor.VendorID;
                            request.Product.Vendor = vendor;
                        }

                        request.Product.ProductSubcategory = productSubcategories.FirstOrDefault(ps => ps.ProductSubcategoryID == request.Product.ProductSubcategory.ProductSubcategoryID);
                        request.CreationDate = DateTime.Now;
                        var isInBudget = false;
                        if (!request.Product.ProductSubcategory.ParentCategory.IsProprietary)
                        {
                            if (request.Currency == null)
                            {
                                request.Currency = AppUtility.CurrencyEnum.NIS.ToString();
                            }
                            isInBudget = checkIfInBudget(request);
                        }
                        request.ExchangeRate = exchangeRate;
                        request.Product.SerialNumber = serialLetter + (lastSerialNumber + 1);
                        lastSerialNumber++;

                        using (var transaction = _context.Database.BeginTransaction())
                        {
                            try
                            {
                                await AddItemAccordingToOrderType(request, OrderType, isInBudget, requestNum: RequestNum);
                                var requestName = AppData.SessionExtensions.SessionNames.Request.ToString() + RequestNum;
                                var isSavedUsingSession = HttpContext.Session.GetObject<Request>(requestName) != null;

                                if (requestItemViewModel.Comments != null)
                                {
                                    var x = 1; //to name the comments in session
                                    foreach (var comment in requestItemViewModel.Comments)
                                    {
                                        if (comment.CommentText.Length != 0)
                                        {
                                            //save the new comment
                                            comment.ApplicationUserID = currentUser.Id;

                                            comment.RequestID = request.RequestID;

                                            if (!isSavedUsingSession)
                                            {
                                                _context.Add(comment);
                                            }
                                            else
                                            {
                                                var SessionCommentName = AppData.SessionExtensions.SessionNames.Comment.ToString() + x;
                                                HttpContext.Session.SetObject(SessionCommentName, comment);
                                            }
                                        }

                                        x++; //to name the comments in session
                                    }
                                }
                                if (!isSavedUsingSession)
                                {
                                    await _context.SaveChangesAsync();
                                    if (receivedModalVisualViewModel.LocationInstancePlaces != null)
                                    {
                                        await SaveLocations(receivedModalVisualViewModel, request);
                                    }
                                    if (i < requestItemViewModel.Requests.Count)
                                    {
                                        additionalRequests = true;
                                    }
                                    else
                                    {
                                        additionalRequests = false;
                                    }
                                    MoveDocumentsOutOfTempFolder(request.RequestID, AppUtility.ParentFolderName.Requests, additionalRequests);
                                    await transaction.CommitAsync();
                                    RemoveRequestWithCommentsAndEmailSessions();
                                }
                                else if (OrderType != AppUtility.OrderTypeEnum.SaveOperations)
                                {
                                    var emailNum = 1;
                                    foreach (var e in requestItemViewModel.EmailAddresses)
                                    {
                                        var SessionEmailName = AppData.SessionExtensions.SessionNames.Email.ToString() + emailNum;
                                        HttpContext.Session.SetObject(SessionEmailName, e);
                                        emailNum++;
                                    }

                                }
                            }
                            catch (Exception ex)
                            {
                                await transaction.RollbackAsync();
                                RemoveRequestWithCommentsAndEmailSessions();
                                throw ex;
                            }
                        }
                        RequestNum++;
                    }
                    i++;
                }
            }
            catch (Exception ex)
            {
                //Redirect Results Need to be checked here
                requestItemViewModel.ErrorMessage += AppUtility.GetExceptionMessage(ex);
                Response.StatusCode = 500;
                //Response.WriteAsync(ex.Message?.ToString());
                if (requestItemViewModel.RequestStatusID == 7)
                {
                    return new RedirectToActionResult(actionName: "CreateItemTabs", controllerName: "Requests", routeValues: new { RequestItemViewModel = requestItemViewModel });
                }
                return new RedirectToActionResult(actionName: "_OrderTab", controllerName: "Requests", routeValues: new { RequestItemViewMOdel = requestItemViewModel });
            }
            switch (OrderType)
            {
                case AppUtility.OrderTypeEnum.AlreadyPurchased:
                    return new RedirectToActionResult("UploadOrderModal", "Requests", new { OrderType = OrderType, SectionType = requestItemViewModel.SectionType });
                case AppUtility.OrderTypeEnum.OrderNow:
                    return new RedirectToActionResult("UploadQuoteModal", "Requests", new { OrderType = OrderType });
                case AppUtility.OrderTypeEnum.AddToCart:
                    return new RedirectToActionResult("UploadQuoteModal", "Requests", new { OrderType = OrderType });
                case AppUtility.OrderTypeEnum.SaveOperations:
                    return new RedirectToActionResult("UploadOrderModal", "Requests", new { OrderType = OrderType, SectionType = requestItemViewModel.SectionType });
                default:
                    if (requestItemViewModel.PageType == AppUtility.PageTypeEnum.RequestSummary)
                    {
                        return new RedirectToActionResult("IndexInventory", "Requests", new
                        {
                            PageType = requestItemViewModel.PageType,
                            SectionType = requestItemViewModel.SectionType,
                            SidebarType = AppUtility.SidebarEnum.List,
                            RequestStatusID = requestItemViewModel.Requests.FirstOrDefault().RequestStatusID,
                        });
                    }
                    return new RedirectToActionResult("Index", "Requests", new
                    {
                        PageType = requestItemViewModel.PageType,
                        SectionType = requestItemViewModel.SectionType,
                        SidebarType = AppUtility.SidebarEnum.List,
                        RequestStatusID = requestItemViewModel.Requests.FirstOrDefault().RequestStatusID,
                    });
            }
        }

        protected async Task SaveLocations(ReceivedModalVisualViewModel receivedModalVisualViewModel, Request requestReceived)
        {
            foreach (var place in receivedModalVisualViewModel.LocationInstancePlaces)
            {
                if (place.Placed)
                {
                    //getting the parentlocationinstanceid
                    var liParent = _context.LocationInstances.Where(li => li.LocationInstanceID == receivedModalVisualViewModel.ParentLocationInstance.LocationInstanceID).FirstOrDefault();
                    var mayHaveParent = true;
                    while (mayHaveParent)
                    {
                        if (liParent.LocationInstanceParentID != null)
                        {
                            liParent = _context.LocationInstances.Where(li => li.LocationInstanceID == liParent.LocationInstanceParentID).FirstOrDefault();
                        }
                        else
                        {
                            mayHaveParent = false;
                        }
                    }

                    //adding the requestlocationinstance
                    var rli = new RequestLocationInstance()
                    {
                        LocationInstanceID = place.LocationInstanceId,
                        RequestID = requestReceived.RequestID,
                        ParentLocationInstanceID = liParent.LocationInstanceID
                    };
                    _context.Add(rli);
                    try
                    {
                        await _context.SaveChangesAsync();
                    }
                    catch (Exception ex)
                    {
                        throw ex;
                    }

                    //updating the locationinstance
                    var locationInstance = _context.LocationInstances.Where(li => li.LocationInstanceID == place.LocationInstanceId).FirstOrDefault();
                    if (locationInstance.LocationTypeID == 103 || locationInstance.LocationTypeID == 205)
                    {
                        locationInstance.IsFull = true;
                    }
                    else
                    {
                        locationInstance.ContainsItems = true;
                    }
                    _context.Update(locationInstance);
                    try
                    {
                        await _context.SaveChangesAsync();
                    }
                    catch (Exception ex)
                    {
                        throw ex;
                    }
                }
            }
        }

        protected bool checkIfInBudget(Request request, Product oldProduct = null)
        {
            if (oldProduct == null)
            {
                oldProduct = request.Product;
            }
            var user = _context.Users.Where(u => u.Id == request.ApplicationUserCreatorID).FirstOrDefault();
            DateTime firstOfMonth = new DateTime(DateTime.Now.Year, DateTime.Now.Month, 1);
            if (oldProduct.ProductSubcategory.ParentCategory.CategoryTypeID == 1)
            { //lab
                var pricePerUnit = request.Cost / request.Unit;
                if (pricePerUnit > user.LabUnitLimit)
                {
                    return false;
                }
                if (request.TotalWithVat > user.LabOrderLimit)
                {
                    return false;
                }
                var monthsSpending = _context.Requests
                      .Where(r => r.Product.ProductSubcategory.ParentCategory.CategoryTypeID == 1)
                      .Where(r => r.ApplicationUserCreatorID == request.ApplicationUserCreatorID && r.Product.VendorID == oldProduct.VendorID)
                      .Where(r => r.ParentRequest.OrderDate >= firstOfMonth).AsEnumerable()
                      .Sum(r => r.TotalWithVat);
                if (monthsSpending + request.TotalWithVat > user.LabMonthlyLimit)
                {
                    return false;
                }
                return true;
            }

            else
            {
                var pricePerUnit = request.Cost;
                if (pricePerUnit > user.OperationUnitLimit)
                {
                    return false;
                }
                if (request.Cost > user.OperationOrderLimit)
                {
                    return false;
                }

                var monthsSpending = _context.Requests
                    .Where(r => r.Product.ProductSubcategory.ParentCategory.CategoryTypeID == 2)
                    .Where(r => r.ApplicationUserCreatorID == request.ApplicationUserCreatorID)
                    .Where(r => r.ParentRequest.OrderDate >= firstOfMonth)
                    .Sum(r => r.Cost);
                if (monthsSpending + request.Cost > user.OperationMonthlyLimit)
                {
                    return false;
                }
                return true;
            }
        }

        protected async Task AddItemAccordingToOrderType(Request newRequest, AppUtility.OrderTypeEnum OrderTypeEnum, bool isInBudget, int requestNum = 1)
        {

            var context = new ValidationContext(newRequest, null, null);
            var results = new List<ValidationResult>();
            var validatorCreate = Validator.TryValidateObject(newRequest, context, results, true);
            if (validatorCreate)
            {
                try
                {
                    switch (OrderTypeEnum)
                    {
                        case AppUtility.OrderTypeEnum.AddToCart:
                            await AddToCart(newRequest, isInBudget);
                            break;
                        case AppUtility.OrderTypeEnum.AlreadyPurchased:
                            AlreadyPurchased(newRequest);
                            break;
                        case AppUtility.OrderTypeEnum.OrderNow:
                            OrderNow(newRequest, isInBudget);
                            break;
                        case AppUtility.OrderTypeEnum.RequestPriceQuote:
                            await RequestItem(newRequest, isInBudget);
                            break;
                        case AppUtility.OrderTypeEnum.Save:
                            await SaveItem(newRequest);
                            break;
                        case AppUtility.OrderTypeEnum.SaveOperations:
                            await SaveOperationsItem(newRequest, requestNum);
                            break;
                    }

                }
                catch (DbUpdateException ex)
                {
                    throw ex;
                }
                catch (Exception ex)
                {
                    throw ex;
                }
            }

        }
        protected async Task<bool> AddToCart(Request request, bool isInBudget)
        {
            try
            {
                if (isInBudget)
                {
                    request.RequestStatusID = 6;
                }
                else
                {
                    request.RequestStatusID = 1;
                }
                request.OrderType = AppUtility.OrderTypeEnum.AddToCart.ToString();
                var requestNum = AppData.SessionExtensions.SessionNames.Request.ToString() + 1;
                HttpContext.Session.SetObject(requestNum, request);
                return true;
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }
        private void AlreadyPurchased(Request request)
        {
            try
            {
                request.RequestStatusID = 2;
                request.ParentQuoteID = null;
                request.OrderType = AppUtility.OrderTypeEnum.AlreadyPurchased.ToString();
                var requestNum = AppData.SessionExtensions.SessionNames.Request.ToString() + 1;
                HttpContext.Session.SetObject(requestNum, request);
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }
        private void OrderNow(Request request, bool isInBudget)
        {
            try
            {
                if (isInBudget)
                {
                    request.RequestStatusID = 6;
                }
                else
                {
                    request.RequestStatusID = 1;
                }
                request.OrderType = AppUtility.OrderTypeEnum.OrderNow.ToString();
                var requestNum = AppData.SessionExtensions.SessionNames.Request.ToString() + 1;
                HttpContext.Session.SetObject(requestNum, request);
            }
            catch (Exception ex)
            {
                throw ex;
            }

        }
        private async Task<bool> RequestItem(Request newRequest, bool isInBudget)
        {

            try
            {
                if (isInBudget)
                {
                    newRequest.RequestStatusID = 6;
                }
                else
                {
                    newRequest.RequestStatusID = 1;
                }
                newRequest.Cost = 0;
                newRequest.ParentQuote = new ParentQuote();
                newRequest.ParentQuote.QuoteStatusID = 1;
                newRequest.OrderType = AppUtility.OrderTypeEnum.RequestPriceQuote.ToString();
                _context.Add(newRequest);
                await _context.SaveChangesAsync();
            }
            catch (DbUpdateException ex)
            {
                throw ex;
            }
            catch (Exception ex)
            {
                throw ex;
            }

            return true;
        }
        private async Task<bool> SaveItem(Request newRequest)
        {

            try
            {
                newRequest.RequestStatusID = 7;
                newRequest.OrderType = AppUtility.OrderTypeEnum.Save.ToString();
                newRequest.Unit = 1;
                newRequest.UnitTypeID = 5;
                _context.Add(newRequest);
                await _context.SaveChangesAsync();
                //var commentExists = true;
                //var n = 1;
                //do
                //{
                //    var commentNumber = AppData.SessionExtensions.SessionNames.Comment.ToString() + n;
                //    var comment = HttpContext.Session.GetObject<Comment>(commentNumber);
                //    if (comment != null)
                //    //will only go in here if there are comments so will only work if it's there
                //    //IMPT look how to clear the session information if it fails somewhere...
                //    {
                //        comment.RequestID = newRequest.RequestID;
                //        _context.Add(comment);
                //    }
                //    else
                //    {
                //        commentExists = false;
                //    }
                //    n++;
                //} while (commentExists);
                //await _context.SaveChangesAsync();
                MoveDocumentsOutOfTempFolder(newRequest.RequestID, AppUtility.ParentFolderName.Requests);

                newRequest.Product = await _context.Products.Where(p => p.ProductID == newRequest.ProductID).FirstOrDefaultAsync();
                RequestNotification requestNotification = new RequestNotification();
                requestNotification.RequestID = newRequest.RequestID;
                requestNotification.IsRead = false;
                requestNotification.RequestName = newRequest.Product.ProductName;
                requestNotification.ApplicationUserID = newRequest.ApplicationUserCreatorID;
                requestNotification.Description = "item created";
                requestNotification.NotificationStatusID = 2;
                requestNotification.TimeStamp = DateTime.Now;
                requestNotification.Controller = "Requests";
                requestNotification.Action = "NotificationsView";
                requestNotification.OrderDate = DateTime.Now;
                _context.Update(requestNotification);
                await _context.SaveChangesAsync();
            }
            catch (DbUpdateException ex)
            {
                throw ex;
            }
            catch (Exception ex)
            {
                throw ex;
            }

            return true;
        }
        private async Task<bool> SaveOperationsItem(Request request, int requestNum)
        {

            try
            {
                if (request.IsReceived)
                {
                    request.RequestStatusID = 3;
                    request.ApplicationUserReceiverID = _userManager.GetUserId(User);
                    request.ArrivalDate = DateTime.Now;
                }
                else
                {
                    request.RequestStatusID = 2;
                }
                request.UnitTypeID = 5;
                request.OrderType = AppUtility.OrderTypeEnum.SaveOperations.ToString();
                var requestName = AppData.SessionExtensions.SessionNames.Request.ToString() + requestNum;
                HttpContext.Session.SetObject(requestName, request);
            }
            catch (DbUpdateException ex)
            {
                throw ex;
            }
            catch (Exception ex)
            {
                throw ex;
            }

            return true;
        }

        [Authorize(Roles ="Requests, Operations")]
        public async Task<TermsViewModel> GetTermsViewModelAsync(int vendorID, RequestIndexObject requestIndexObject)
        {
            var requ = HttpContext.Session.GetObject<Request>("Request1");
            List<Request> requests = new List<Request>();
            if (vendorID != 0)
            {
                if (requestIndexObject.SidebarType == AppUtility.SidebarEnum.Cart)
                {
                    requests = await _context.Requests.Where(r => r.Product.ProductSubcategory.ParentCategory.CategoryTypeID == 1)
          .Where(r => r.Product.VendorID == vendorID && r.RequestStatusID == 6 && r.OrderType == AppUtility.OrderTypeEnum.AddToCart.ToString() && r.ParentQuote.QuoteStatusID == 4)
          .Where(r => r.ApplicationUserCreatorID == _userManager.GetUserId(User))
                .Include(r => r.Product).ThenInclude(r => r.Vendor)
                .Include(r => r.Product.ProductSubcategory).ThenInclude(ps => ps.ParentCategory).ToListAsync();
                }
                else if (requestIndexObject.SidebarType == AppUtility.SidebarEnum.Orders)
                {
                    requests = await _context.Requests.Where(r => r.Product.ProductSubcategory.ParentCategory.CategoryTypeID == 1)
          .Where(r => r.Product.VendorID == vendorID && r.RequestStatusID == 6 && r.OrderType == AppUtility.OrderTypeEnum.RequestPriceQuote.ToString() && r.ParentQuote.QuoteStatusID == 4)
                .Include(r => r.Product).ThenInclude(r => r.Vendor)
                .Include(r => r.Product.ProductSubcategory).ThenInclude(ps => ps.ParentCategory).ToListAsync();
                }
                RemoveRequestWithCommentsAndEmailSessions();
            }
            else
            {
                var isRequests = true;
                var RequestNum = 1;
                while (isRequests)
                {
                    var requestName = AppData.SessionExtensions.SessionNames.Request.ToString() + RequestNum;
                    if (HttpContext.Session.GetObject<Request>(requestName) != null)
                    {
                        requests.Add(HttpContext.Session.GetObject<Request>(requestName));

                    }
                    else
                    {
                        isRequests = false;
                    }
                    RequestNum++;
                }

            }
            var requestNum = 1;
            foreach (var req in requests)
            {
                var requestName = AppData.SessionExtensions.SessionNames.Request.ToString() + requestNum;
                HttpContext.Session.SetObject(requestName, req);
                requestNum++;
            }
            var termsList = new List<SelectListItem>() { };
            await _context.PaymentStatuses.ForEachAsync(ps =>
            {
                if (ps.PaymentStatusID != 7)//don't have standing orders as an option
                {
                    termsList.Add(new SelectListItem() { Value = ps.PaymentStatusID + "", Text = ps.PaymentStatusDescription });
                }
            });
            TermsViewModel termsViewModel = new TermsViewModel()
            {
                ParentRequest = new ParentRequest(),
                TermsList = termsList,
                InstallmentDate = DateTime.Now
            };
            requestIndexObject.SelectedCurrency = (AppUtility.CurrencyEnum)Enum.Parse(typeof(AppUtility.CurrencyEnum), requests[0].Currency);
            termsViewModel.RequestIndexObject = requestIndexObject;
            return termsViewModel;
        }

        public async Task<RedirectAndModel> SaveTermsModalAsync(TermsViewModel termsViewModel)
        {
            var controller = "Requests";
            if (termsViewModel.RequestIndexObject.PageType == AppUtility.PageTypeEnum.RequestSummary)
            {
                controller = "Operations";
            }
            try
            {
                var requests = new List<Request>();
                var isRequests = true;
                var RequestNum = 1;
                while (isRequests)
                {
                    var requestName = AppData.SessionExtensions.SessionNames.Request.ToString() + RequestNum;
                    if (HttpContext.Session.GetObject<Request>(requestName) != null)
                    {
                        requests.Add(HttpContext.Session.GetObject<Request>(requestName));
                    }
                    else
                    {
                        isRequests = false;
                    }
                    RequestNum++;
                }

                RequestNum = 1;
                var PaymentNum = 1;
                var SaveUsingSessions = true;
                using (var transaction = _context.Database.BeginTransaction())
                {
                    try
                    {
                        foreach (var req in requests)
                        {
                            if (req.Product == null)
                            {
                                req.Product = _context.Products.Where(p => p.ProductID == req.ProductID).Include(p => p.ProductSubcategory).FirstOrDefault();
                            }

                            if (req.OrderType == AppUtility.OrderTypeEnum.AlreadyPurchased.ToString() || req.OrderType == AppUtility.OrderTypeEnum.SaveOperations.ToString())
                            {
                                SaveUsingSessions = false;
                            }
                            if (req.ParentRequest == null)
                            {
                                req.ParentRequest = termsViewModel.ParentRequest;
                            }
                            req.ParentRequest.Shipping = termsViewModel.ParentRequest.Shipping;
                            req.PaymentStatusID = termsViewModel.SelectedTerm;
                            req.Installments = (uint)termsViewModel.Installments;
                            if (termsViewModel.Installments == 0)
                            {
                                req.Installments = 1;
                            }
                            if (SaveUsingSessions)
                            {
                                var requestName = AppData.SessionExtensions.SessionNames.Request.ToString() + RequestNum;
                                HttpContext.Session.SetObject(requestName, req);
                            }
                            else
                            {

                                //if (req.PaymentStatusID == 7)
                                //{
                                //    req.RequestStatusID = 3;
                                //    req.ApplicationUserReceiverID = _userManager.GetUserId(User);
                                //    req.ArrivalDate = DateTime.Now;
                                //}
                                if (req.Product.ProductID == 0)
                                {
                                    _context.Entry(req.Product).State = EntityState.Added;
                                }
                                else
                                {
                                    _context.Entry(req.Product).State = EntityState.Unchanged;
                                }
                                _context.Entry(req).State = EntityState.Added;
                                _context.Entry(req.ParentRequest).State = EntityState.Added;


                                await _context.SaveChangesAsync();
                            }
                            for (int i = 0; i < req.Installments; i++)
                            {
                                var payment = new Payment() { InstallmentNumber = i + 1 };
                                if (req.PaymentStatusID == 5)
                                {
                                    payment.PaymentDate = termsViewModel.InstallmentDate.AddMonths(i);
                                    payment.Sum = ((req.Cost ?? 0) / (req.Installments ?? 0));
                                }
                                else
                                {
                                    payment.PaymentDate = new DateTime(DateTime.Now.Year, DateTime.Now.Month, DateTime.Now.Day);
                                    payment.Sum = req.Cost ?? 0;
                                }
                                if (SaveUsingSessions)
                                {
                                    var paymentName = AppData.SessionExtensions.SessionNames.Payment.ToString() + (PaymentNum);
                                    HttpContext.Session.SetObject(paymentName, payment);
                                }
                                else
                                {
                                    payment.RequestID = req.RequestID;
                                    _context.Add(payment);
                                    await _context.SaveChangesAsync();
                                }
                                PaymentNum++;
                            }
                            RequestNum++;
                        }
                        if (!SaveUsingSessions)
                        {

                            int i = 1;
                            var additionalRequests = false;
                            foreach (var request in requests)
                            {
                                var commentExists = true;
                                var n = 1;
                                do
                                {
                                    var commentNumber = AppData.SessionExtensions.SessionNames.Comment.ToString() + n;
                                    var comment = HttpContext.Session.GetObject<Comment>(commentNumber);
                                    if (comment != null)
                                    //will only go in here if there are comments so will only work if it's there
                                    //IMPT look how to clear the session information if it fails somewhere...
                                    {
                                        comment.RequestID = request.RequestID;
                                        _context.Add(comment);
                                    }
                                    else
                                    {
                                        commentExists = false;
                                    }
                                    n++;
                                } while (commentExists);
                                await _context.SaveChangesAsync();
                                if (i < requests.Count)
                                {
                                    additionalRequests = true;
                                }
                                else
                                {
                                    additionalRequests = false;
                                }
                                MoveDocumentsOutOfTempFolder(request.RequestID, AppUtility.ParentFolderName.Requests, additionalRequests);
                                request.Product.Vendor = _context.Vendors.Where(v => v.VendorID == request.Product.VendorID).FirstOrDefault();
                                RequestNotification requestNotification = new RequestNotification();
                                requestNotification.RequestID = request.RequestID;
                                requestNotification.IsRead = false;
                                requestNotification.RequestName = request.Product.ProductName;
                                requestNotification.ApplicationUserID = request.ApplicationUserCreatorID;
                                requestNotification.Description = "item ordered";
                                requestNotification.NotificationStatusID = 2;
                                requestNotification.TimeStamp = DateTime.Now;
                                requestNotification.Controller = "Requests";
                                requestNotification.Action = "NotificationsView";
                                requestNotification.OrderDate = DateTime.Now;
                                requestNotification.Vendor = request.Product.Vendor.VendorEnName;
                                _context.Add(requestNotification);
                                i++;
                            }
                            await _context.SaveChangesAsync();
                            await transaction.CommitAsync();
                            return new RedirectAndModel() { RedirectToActionResult = new RedirectToActionResult("Index", controller, termsViewModel.RequestIndexObject) };
                        }

                    }
                    catch (Exception ex)
                    {
                        transaction.Rollback();
                        throw ex;
                    }

                }
                return new RedirectAndModel() {RedirectToActionResult = new RedirectToActionResult("ConfirmEmailModal", controller, termsViewModel.RequestIndexObject) };
            }
            catch (Exception ex)
            {
                termsViewModel.ErrorMessage = AppUtility.GetExceptionMessage(ex);
                Response.StatusCode = 500;
                var termsList = new List<SelectListItem>() { };
                await _context.PaymentStatuses.ForEachAsync(ps => termsList.Add(new SelectListItem() { Value = ps.PaymentStatusID + "", Text = ps.PaymentStatusDescription }));
                termsViewModel.TermsList = termsList;
            return new RedirectAndModel() { RedirectToActionResult = new RedirectToActionResult("", "", ""), TermsViewModel = termsViewModel };
            }
        }

        public async Task<IActionResult> RedirectRequestsToShared(string action, RequestIndexObject requestIndexObject)
        {
            return RedirectToAction(action, requestIndexObject);
        }

        //[HttpPost]
        //[Authorize(Roles = "Requests")]
        //public async Task<IActionResult> TermsModal(TermsViewModel termsViewModel)
        //{
        //    return new EmptyResult();
        //}
    }
}
