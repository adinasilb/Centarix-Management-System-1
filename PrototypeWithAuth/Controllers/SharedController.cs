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
                sickDaysTaken = employeeHours.Where(eh => eh.Date.Month == month && eh.Date.Year == year &&  eh.OffDayTypeID == 1 && eh.Date <= DateTime.Now.Date).Count();
                sickDaysTaken = Math.Round(sickDaysTaken + (sickHours / user.SalariedEmployee.HoursPerDay), 2);
                
                var vacationHours = employeeHours.Where(eh =>  eh.Date.Year == year && eh.PartialOffDayTypeID == 2 && eh.Date <= DateTime.Now.Date && eh.Date.Month == month).Select(eh => (eh.PartialOffDayHours == null ? TimeSpan.Zero : ((TimeSpan)eh.PartialOffDayHours)).TotalHours).ToList().Sum(p => p);
                vacationDaysTaken = employeeHours.Where(eh =>  eh.Date.Year == year && eh.OffDayTypeID == 2 && eh.Date <= DateTime.Now.Date && eh.Date.Month == month).Count();
                vacationDaysTaken = Math.Round(vacationDaysTaken + (vacationHours / user.SalariedEmployee.HoursPerDay), 2);
                var specialDays = employeeHours.Where(eh => eh.OffDayTypeID == 4).Count();
                var unpaidLeave =employeeHours.Where(eh => eh.OffDayTypeID == 5).Count();
                totalhours = GetTotalWorkingDaysThisMonth(new DateTime(year, month, 1), companyDaysOff) - (vacationDaysTaken + sickDaysTaken+unpaidLeave+specialDays);
                totalhours = totalhours * user.SalariedEmployee.HoursPerDay;

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
                User = user
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
            if(!parsed)
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
         .Include(r => r.ParentQuote)
         .Where(r => r.Product.ProductSubcategory.ParentCategory.CategoryTypeID == categoryID).Include(x => x.ParentRequest);

            int sideBarID = 0;
            if (requestIndexObject.SidebarType != AppUtility.SidebarEnum.Owner)
            {
                int.TryParse(requestIndexObject.SidebarFilterID, out sideBarID);
            }

            if (requestIndexObject.PageType == AppUtility.PageTypeEnum.LabManagementEquipment)
            {
                if (requestIndexObject.SidebarType == AppUtility.SidebarEnum.Category)
                {
                    RequestsPassedIn = _context.Requests.Where(r => r.RequestStatusID == 3).Where(r => r.Product.ProductSubcategory.ParentCategoryID == 5).Include(r => r.Product.ProductSubcategory)
                        .Include(r => r.Product.Vendor).Include(r => r.RequestStatus).Include(r => r.UnitType).Include(r => r.SubUnitType).Include(r => r.SubSubUnitType).Include(r => r.RequestLocationInstances).ThenInclude(rli => rli.LocationInstance)
                        .Include(r => r.ParentRequest).Where(r => r.Product.ProductSubcategoryID == sideBarID).Include(r => r.ParentRequest);

                }
                else
                {
                    RequestsPassedIn = _context.Requests.Where(r => r.RequestStatusID == 3).Where(r => r.Product.ProductSubcategory.ParentCategoryID == 5).Include(r => r.Product.ProductSubcategory)
                        .Include(r => r.Product.Vendor).Include(r => r.RequestStatus).Include(r => r.UnitType).Include(r => r.SubUnitType).Include(r => r.SubSubUnitType).Include(r => r.RequestLocationInstances).ThenInclude(rli => rli.LocationInstance)
                        .Include(r => r.ParentRequest);
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
                    RequestsPassedIn = fullRequestsList.Where(r => r.RequestStatus.RequestStatusID == 7).Include(r => r.Product.ProductSubcategory).ThenInclude(ps => ps.ParentCategory)
                 .Include(r => r.Product.Vendor).Include(r => r.RequestStatus).Include(r => r.UnitType).Include(r => r.SubUnitType).Include(r => r.SubSubUnitType);
                    RequestsPassedIn = RequestsPassedIn.Include(r => r.RequestLocationInstances).ThenInclude(r => r.LocationInstance);
                    RequestsPassedIn = RequestsPassedIn.ToList().GroupBy(r => r.ProductID).Select(e => e.Last()).AsQueryable();
                }
                else
                {
                    RequestsPassedIn = fullRequestsList.Where(r => r.RequestStatus.RequestStatusID == 3).Include(r => r.Product.ProductSubcategory).ThenInclude(ps => ps.ParentCategory)
                   .Include(r => r.Product.Vendor).Include(r => r.RequestStatus).Include(r => r.UnitType).Include(r => r.SubUnitType).Include(r => r.SubSubUnitType);
                    RequestsPassedIn = RequestsPassedIn.Include(r => r.RequestLocationInstances).ThenInclude(r => r.LocationInstance);
                    RequestsPassedIn = RequestsPassedIn.ToList().GroupBy(r => r.ProductID).Select(e => e.Last()).AsQueryable();
                }

            }
            else if (requestIndexObject.PageType == AppUtility.PageTypeEnum.AccountingGeneral)
            {
                //we need both categories
                RequestsPassedIn = _context.Requests.Include(r => r.ApplicationUserCreator)
                     .Include(r => r.RequestLocationInstances).ThenInclude(rli => rli.LocationInstance).Include(r => r.ParentQuote)
                     .Include(r => r.ParentRequest).Where(r => Years.Contains(r.ParentRequest.OrderDate.Year)).Where(r => !r.IsClarify && !r.IsPartial && r.Payments.Where(p => p.IsPaid && p.HasInvoice).Count() == r.Payments.Count());
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

            RequestPassedInWithInclude = RequestPassedInWithInclude.Include(r => r.RequestLocationInstances).ThenInclude(li => li.LocationInstance);

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
            var popoverShare = new IconPopoverViewModel("icon-share-24px1", "black", AppUtility.PopoverDescription.Share, "ShareRequest", "Requests", AppUtility.PopoverEnum.None, "share-request-fx");
            var defaultImage = "/images/css/CategoryImages/placeholder.png";
            switch (requestIndexObject.PageType)
            {
                case AppUtility.PageTypeEnum.RequestRequest:
                    switch (requestIndexObject.RequestStatusID)
                    {
                        case 1:
                            /*iconList.Add(approveIcon);
                            iconList.Add(deleteIcon);
                            onePageOfProducts = await GetForApprovalRows(requestIndexObject, onePageOfProducts, RequestPassedInWithInclude, iconList, defaultImage);
                            break;*/
                        case 6:
                            iconList.Add(approveIcon);
                            iconList.Add(deleteIcon);
                            //onePageOfProducts = await GetForApprovalRows(requestIndexObject, onePageOfProducts, RequestPassedInWithInclude, iconList, defaultImage);
                            //iconList.Add(reorderIcon);
                            //iconList.Add(deleteIcon);
                            onePageOfProducts = await GetApprovedRows(requestIndexObject, onePageOfProducts, RequestPassedInWithInclude, iconList, defaultImage);
                            break;
                        case 2:
                            iconList.Add(receiveIcon);
                            iconList.Add(deleteIcon);
                            onePageOfProducts = await GetOrderedRows(requestIndexObject, onePageOfProducts, RequestPassedInWithInclude, iconList, defaultImage);
                            break;
                        case 3:
                            //iconList.Add(reorderIcon);
                            iconList.Add(favoriteIcon);
                            popoverMoreIcon.IconPopovers = new List<IconPopoverViewModel>() { popoverShare, popoverReorder, popoverDelete };
                            iconList.Add(popoverMoreIcon);
                            onePageOfProducts = await GetReceivedInventoryRows(requestIndexObject, onePageOfProducts, RequestPassedInWithInclude, iconList, defaultImage);
                            break;

                    }
                    break;
                case AppUtility.PageTypeEnum.OperationsRequest:
                    switch (requestIndexObject.RequestStatusID)
                    {
                        /*case 1:
                            iconList.Add(approveIcon);
                            iconList.Add(deleteIcon);
                            onePageOfProducts = await GetForApprovalOperationsRows(requestIndexObject, onePageOfProducts, RequestPassedInWithInclude, iconList, defaultImage);
                            break;
                        case 6:
                            iconList.Add(orderOperations);
                            iconList.Add(deleteIcon);
                            onePageOfProducts = await GetApprovedOperationsRows(requestIndexObject, onePageOfProducts, RequestPassedInWithInclude, iconList, defaultImage);
                            break;*/
                        case 2:
                            iconList.Add(receiveIcon);
                            iconList.Add(deleteIcon);
                            onePageOfProducts = await GetOrderedOperationsRows(requestIndexObject, onePageOfProducts, RequestPassedInWithInclude, iconList, defaultImage);
                            break;
                        case 3:
                            iconList.Add(deleteIcon);
                            onePageOfProducts = await GetReceivedInventoryOperationsRows(requestIndexObject, onePageOfProducts, RequestPassedInWithInclude, iconList, defaultImage);
                            break;
                    }
                    break;
                case AppUtility.PageTypeEnum.RequestInventory:
                    popoverMoreIcon.IconPopovers = new List<IconPopoverViewModel>() { popoverShare, popoverReorder };
                    iconList.Add(deleteIcon);
                    iconList.Add(popoverMoreIcon);
                    onePageOfProducts = await GetReceivedInventoryRows(requestIndexObject, onePageOfProducts, RequestPassedInWithInclude, iconList, defaultImage);
                    break;
                case AppUtility.PageTypeEnum.RequestSummary:


                    switch (requestIndexObject.RequestStatusID)
                    {
                        case 7:
                            iconList.Add(favoriteIcon);
                            iconList.Add(deleteIcon);
                            onePageOfProducts = await GetSummaryProprietaryRows(requestIndexObject, onePageOfProducts, RequestPassedInWithInclude, iconList, defaultImage);
                            break;
                        default:
                            popoverMoreIcon.IconPopovers = new List<IconPopoverViewModel>() { popoverShare, popoverReorder, popoverDelete };
                            iconList.Add(favoriteIcon);
                            iconList.Add(popoverMoreIcon);
                            onePageOfProducts = await GetSummaryRows(requestIndexObject, onePageOfProducts, RequestPassedInWithInclude, iconList, defaultImage);
                            break;
                    }

                    break;
                case AppUtility.PageTypeEnum.OperationsInventory:
                    iconList.Add(orderOperations);
                    iconList.Add(deleteIcon);
                    onePageOfProducts = await GetReceivedInventoryOperationsRows(requestIndexObject, onePageOfProducts, RequestPassedInWithInclude, iconList, defaultImage);
                    break;
                case AppUtility.PageTypeEnum.LabManagementEquipment:
                    iconList.Add(equipmentIcon);
                    popoverMoreIcon.IconPopovers = new List<IconPopoverViewModel>() { popoverShare, popoverReorder };
                    iconList.Add(deleteIcon);
                    iconList.Add(popoverMoreIcon);
                    onePageOfProducts = await GetReceivedInventoryRows(requestIndexObject, onePageOfProducts, RequestPassedInWithInclude, iconList, defaultImage);
                    break;
                case AppUtility.PageTypeEnum.ExpensesStatistics:
                    break;
                case AppUtility.PageTypeEnum.AccountingGeneral:
                    onePageOfProducts = await GetAccountingGeneralRows(requestIndexObject, onePageOfProducts, RequestPassedInWithInclude, iconList, defaultImage);
                    break;
                case AppUtility.PageTypeEnum.RequestCart:
                    switch (requestIndexObject.SidebarType)
                    {
                        case AppUtility.SidebarEnum.Favorites:
                            iconList.Add(reorderIcon);
                            iconList.Add(favoriteIcon);
                            onePageOfProducts = await GetReceivedInventoryFavoriteRows(requestIndexObject, onePageOfProducts, RequestPassedInWithInclude, iconList, defaultImage);
                            break;
                        case AppUtility.SidebarEnum.SharedRequests:
                            iconList.Add(reorderIcon);
                            iconList.Add(favoriteIcon);
                            onePageOfProducts = await GetReceivedInventorySharedRows(requestIndexObject, onePageOfProducts, RequestPassedInWithInclude, iconList, defaultImage);
                            break;
                    }
                    break;
            }

            return onePageOfProducts;
        }

        protected async Task<IPagedList<RequestIndexPartialRowViewModel>> GetApprovedRows(RequestIndexObject requestIndexObject, IPagedList<RequestIndexPartialRowViewModel> onePageOfProducts, IQueryable<Request> RequestPassedInWithInclude, List<IconColumnViewModel> iconList, string defaultImage)
        {
            onePageOfProducts = await RequestPassedInWithInclude.OrderByDescending(r => r.CreationDate).ToList().Select(r => new RequestIndexPartialRowViewModel()
            {
                Columns = new List<RequestIndexPartialColumnViewModel>()
                            {
                                 new RequestIndexPartialColumnViewModel() { Title = "", Width=10, Image = r.Product.ProductSubcategory.ImageURL==null?defaultImage: r.Product.ProductSubcategory.ImageURL},
                                 new RequestIndexPartialColumnViewModel() { Title = "Item Name", Width=15, Value = new List<string>(){ r.Product.ProductName}, AjaxLink = "load-product-details", AjaxID=r.RequestID},
                                 new RequestIndexPartialColumnViewModel() { Title = "Vendor", Width=10, Value = new List<string>(){ r.Product.Vendor.VendorEnName} },
                                 new RequestIndexPartialColumnViewModel() { Title = "Amount", Width=10, Value = AppUtility.GetAmountColumn(r, r.UnitType, r.SubUnitType, r.SubSubUnitType)},
                                 new RequestIndexPartialColumnViewModel() { Title = "Category", Width=11, Value = AppUtility.GetCategoryColumn(requestIndexObject.CategorySelected, requestIndexObject.SubcategorySelected, r.Product.ProductSubcategory), FilterEnum=AppUtility.FilterEnum.Category },
                                 new RequestIndexPartialColumnViewModel() { Title = "Owner", Width=12, Value = new List<string>(){r.ApplicationUserCreator.FirstName + " " + r.ApplicationUserCreator.LastName} },
                                 new RequestIndexPartialColumnViewModel() { Title = "Price", Width=10, Value = AppUtility.GetPriceColumn(requestIndexObject.SelectedPriceSort, r,  requestIndexObject.SelectedCurrency), FilterEnum=AppUtility.FilterEnum.Price},
                                 new RequestIndexPartialColumnViewModel() { Title = "Date Created", Width=12, Value = new List<string>(){ r.CreationDate.ToString("dd'/'MM'/'yyyy") } },
                                 new RequestIndexPartialColumnViewModel() { Title = "", Width=10, Icons = GetIconsByIndividualRequest(r.RequestID, iconList, false), AjaxID = r.RequestID },
                                 new RequestIndexPartialColumnViewModel() { Width=0, AjaxLink = " d-none order-type"+r.RequestID, AjaxID = (int)Enum.Parse(typeof(AppUtility.OrderTypeEnum),r.OrderType), Value = new List<string>(){r.OrderType.ToString()} }

                            }
            }).ToPagedListAsync(requestIndexObject.PageNumber == 0 ? 1 : requestIndexObject.PageNumber, 25);
            return onePageOfProducts;
        }

        protected static async Task<IPagedList<RequestIndexPartialRowViewModel>> GetOrderedRows(RequestIndexObject requestIndexObject, IPagedList<RequestIndexPartialRowViewModel> onePageOfProducts, IQueryable<Request> RequestPassedInWithInclude, List<IconColumnViewModel> iconList, string defaultImage)
        {
            onePageOfProducts = await RequestPassedInWithInclude.OrderByDescending(r => r.ParentRequest.OrderDate).ToList().Select(r => new RequestIndexPartialRowViewModel()
            {
                Columns = new List<RequestIndexPartialColumnViewModel>()
                            {
                                 new RequestIndexPartialColumnViewModel() { Title = "", Width=10, Image = r.Product.ProductSubcategory.ImageURL==null?defaultImage: r.Product.ProductSubcategory.ImageURL},
                                 new RequestIndexPartialColumnViewModel() { Title = "Item Name", Width=15, Value = new List<string>(){ r.Product.ProductName}, AjaxLink = "load-product-details", AjaxID=r.RequestID},
                                 new RequestIndexPartialColumnViewModel() { Title = "Vendor", Width=10, Value = new List<string>(){ r.Product.Vendor.VendorEnName} },
                                 new RequestIndexPartialColumnViewModel() { Title = "Amount", Width=10, Value = AppUtility.GetAmountColumn(r, r.UnitType, r.SubUnitType, r.SubSubUnitType)},
                                 new RequestIndexPartialColumnViewModel() { Title = "Category", Width=11, Value = AppUtility.GetCategoryColumn(requestIndexObject.CategorySelected, requestIndexObject.SubcategorySelected, r.Product.ProductSubcategory), FilterEnum=AppUtility.FilterEnum.Category },
                                 new RequestIndexPartialColumnViewModel() { Title = "Owner", Width=12, Value = new List<string>(){r.ApplicationUserCreator.FirstName + " " + r.ApplicationUserCreator.LastName} },
                                 new RequestIndexPartialColumnViewModel() { Title = "Price", Width=10, Value = AppUtility.GetPriceColumn(requestIndexObject.SelectedPriceSort, r,  requestIndexObject.SelectedCurrency), FilterEnum=AppUtility.FilterEnum.Price},
                                 new RequestIndexPartialColumnViewModel() { Title = "Date Ordered", Width=12, Value = new List<string>(){ r.ParentRequest.OrderDate.ToString("dd'/'MM'/'yyyy") } },
                                 new RequestIndexPartialColumnViewModel()
                                 {
                                     Title = "", Width=10, Icons = iconList, AjaxID = r.RequestID
                                 }
                            }
            }).ToPagedListAsync(requestIndexObject.PageNumber == 0 ? 1 : requestIndexObject.PageNumber, 25);
            return onePageOfProducts;
        }


        protected async Task<IPagedList<RequestIndexPartialRowViewModel>> GetReceivedInventoryRows(RequestIndexObject requestIndexObject, IPagedList<RequestIndexPartialRowViewModel> onePageOfProducts, IQueryable<Request> RequestPassedInWithInclude, List<IconColumnViewModel> iconList, string defaultImage)
        {
            onePageOfProducts = await RequestPassedInWithInclude.OrderByDescending(r => r.ArrivalDate).ToList().Select(r => new RequestIndexPartialRowViewModel()
            {
                Columns = new List<RequestIndexPartialColumnViewModel>()
                        {
                             new RequestIndexPartialColumnViewModel() { Title = "", Width=9, Image = r.Product.ProductSubcategory.ImageURL==null?defaultImage: r.Product.ProductSubcategory.ImageURL},
                             new RequestIndexPartialColumnViewModel() { Title = "Item Name", Width=14, Value = new List<string>(){ r.Product.ProductName}, AjaxLink = "load-product-details", AjaxID=r.RequestID},
                             new RequestIndexPartialColumnViewModel() { Title = "Vendor", Width=9, Value = new List<string>(){ r.Product.Vendor.VendorEnName} },
                             new RequestIndexPartialColumnViewModel() { Title = "Amount", Width=9, Value = AppUtility.GetAmountColumn(r, r.UnitType, r.SubUnitType, r.SubSubUnitType)},
                             new RequestIndexPartialColumnViewModel() { Title = "Location", Width=9, Value = new List<string>(){ GetLocationInstanceNameBefore(r.RequestLocationInstances.FirstOrDefault().LocationInstance) } },
                             new RequestIndexPartialColumnViewModel() { Title = "Category", Width=11, Value = AppUtility.GetCategoryColumn(requestIndexObject.CategorySelected, requestIndexObject.SubcategorySelected, r.Product.ProductSubcategory), FilterEnum=AppUtility.FilterEnum.Category },
                             new RequestIndexPartialColumnViewModel() { Title = "Owner", Width=10, Value = new List<string>(){r.ApplicationUserCreator.FirstName + " " + r.ApplicationUserCreator.LastName} },
                             new RequestIndexPartialColumnViewModel() { Title = "Price", Width=10, Value = AppUtility.GetPriceColumn(requestIndexObject.SelectedPriceSort, r, requestIndexObject.SelectedCurrency), FilterEnum=AppUtility.FilterEnum.Price},
                             new RequestIndexPartialColumnViewModel() { Title = "Arrival Date", Width=10, Value = new List<string>(){ r.ArrivalDate.ToString("dd'/'MM'/'yyyy") } },
                             new RequestIndexPartialColumnViewModel()
                             {
                                 Title = "", Width=10, Icons = GetIconsByIndividualRequest(r.RequestID, iconList, false), AjaxID = r.RequestID
                             }
                        }
            }).ToPagedListAsync(requestIndexObject.PageNumber == 0 ? 1 : requestIndexObject.PageNumber, 25);
            return onePageOfProducts;
        }

        protected static async Task<IPagedList<RequestIndexPartialRowViewModel>> GetOrderedOperationsRows(RequestIndexObject requestIndexObject, IPagedList<RequestIndexPartialRowViewModel> onePageOfProducts, IQueryable<Request> RequestPassedInWithInclude, List<IconColumnViewModel> iconList, string defaultImage)
        {
            onePageOfProducts = await RequestPassedInWithInclude.OrderByDescending(r => r.ParentRequest.OrderDate).ToList().Select(r => new RequestIndexPartialRowViewModel()
            {
                Columns = new List<RequestIndexPartialColumnViewModel>()
                            {
                                 new RequestIndexPartialColumnViewModel() { Title = "", Width=10, Image = r.Product.ProductSubcategory.ImageURL==null?defaultImage: r.Product.ProductSubcategory.ImageURL},
                                 new RequestIndexPartialColumnViewModel() { Title = "Item Name", Width=15, Value = new List<string>(){ r.Product.ProductName}, AjaxLink = "load-product-details", AjaxID=r.RequestID},
                                 new RequestIndexPartialColumnViewModel() { Title = "Vendor", Width=10, Value = new List<string>(){ r.Product.Vendor.VendorEnName} },
                                 new RequestIndexPartialColumnViewModel() { Title = "Category", Width=11, Value = AppUtility.GetCategoryColumn(requestIndexObject.CategorySelected, requestIndexObject.SubcategorySelected, r.Product.ProductSubcategory), FilterEnum=AppUtility.FilterEnum.Category },
                                 new RequestIndexPartialColumnViewModel() { Title = "Owner", Width=12, Value = new List<string>(){r.ApplicationUserCreator.FirstName + " " + r.ApplicationUserCreator.LastName} },
                                 new RequestIndexPartialColumnViewModel() { Title = "Price", Width=10, Value = AppUtility.GetPriceColumn(requestIndexObject.SelectedPriceSort, r,  requestIndexObject.SelectedCurrency), FilterEnum=AppUtility.FilterEnum.Price},
                                 new RequestIndexPartialColumnViewModel() { Title = "Date Ordered", Width=12, Value = new List<string>(){ r.ParentRequest.OrderDate.ToString("dd'/'MM'/'yyyy") } },
                                 new RequestIndexPartialColumnViewModel()
                                 {
                                     Title = "", Width=10, Icons = iconList, AjaxID = r.RequestID
                                 }
                            }
            }).ToPagedListAsync(requestIndexObject.PageNumber == 0 ? 1 : requestIndexObject.PageNumber, 25);
            return onePageOfProducts;
        }

        protected async Task<IPagedList<RequestIndexPartialRowViewModel>> GetReceivedInventoryOperationsRows(RequestIndexObject requestIndexObject, IPagedList<RequestIndexPartialRowViewModel> onePageOfProducts, IQueryable<Request> RequestPassedInWithInclude, List<IconColumnViewModel> iconList, string defaultImage)
        {
            onePageOfProducts = await RequestPassedInWithInclude.OrderByDescending(r => r.ParentRequest.OrderDate).ToList().Select(r => new RequestIndexPartialRowViewModel()
            {
                Columns = new List<RequestIndexPartialColumnViewModel>()
                        {
                             new RequestIndexPartialColumnViewModel() { Title = "", Width=9, Image = r.Product.ProductSubcategory.ImageURL==null?defaultImage: r.Product.ProductSubcategory.ImageURL},
                             new RequestIndexPartialColumnViewModel() { Title = "Item Name", Width=14, Value = new List<string>(){ r.Product.ProductName}, AjaxLink = "load-product-details", AjaxID=r.RequestID},
                             new RequestIndexPartialColumnViewModel() { Title = "Vendor", Width=9, Value = new List<string>(){ r.Product.Vendor.VendorEnName} },
                             new RequestIndexPartialColumnViewModel() { Title = "Category", Width=11, Value = AppUtility.GetCategoryColumn(requestIndexObject.CategorySelected, requestIndexObject.SubcategorySelected, r.Product.ProductSubcategory), FilterEnum=AppUtility.FilterEnum.Category },
                             new RequestIndexPartialColumnViewModel() { Title = "Owner", Width=10, Value = new List<string>(){r.ApplicationUserCreator.FirstName + " " + r.ApplicationUserCreator.LastName} },
                             new RequestIndexPartialColumnViewModel() { Title = "Price", Width=10, Value = AppUtility.GetPriceColumn(requestIndexObject.SelectedPriceSort, r, requestIndexObject.SelectedCurrency), FilterEnum=AppUtility.FilterEnum.Price},
                             new RequestIndexPartialColumnViewModel() { Title = "Date Ordered", Width=10, Value = new List<string>(){ r.ParentRequest.OrderDate.ToString("dd'/'MM'/'yyyy") } },
                             new RequestIndexPartialColumnViewModel()
                             {
                                 Title = "", Width=10, Icons = iconList, AjaxID = r.RequestID
                             }
                        }
            }).ToPagedListAsync(requestIndexObject.PageNumber == 0 ? 1 : requestIndexObject.PageNumber, 25);
            return onePageOfProducts;
        }
        protected async Task<IPagedList<RequestIndexPartialRowViewModel>> GetSummaryProprietaryRows(RequestIndexObject requestIndexObject, IPagedList<RequestIndexPartialRowViewModel> onePageOfProducts, IQueryable<Request> RequestPassedInWithInclude, List<IconColumnViewModel> iconList, string defaultImage)
        {

            onePageOfProducts = await RequestPassedInWithInclude.OrderByDescending(r => r.CreationDate).ToList().Select(r => new RequestIndexPartialRowViewModel()
            {
                Columns = new List<RequestIndexPartialColumnViewModel>()
                        {
                             new RequestIndexPartialColumnViewModel() { Title = "", Width=9, Image = r.Product.ProductSubcategory.ImageURL==null?defaultImage: r.Product.ProductSubcategory.ImageURL},
                             new RequestIndexPartialColumnViewModel() { Title = "Item Name", Width=14, Value = new List<string>(){ r.Product.ProductName}, AjaxLink = "load-product-details-summary", AjaxID=r.RequestID},
                             new RequestIndexPartialColumnViewModel() { Title = "Amount", Width=9, Value = AppUtility.GetAmountColumn(r, r.UnitType, r.SubUnitType, r.SubSubUnitType)},
                             new RequestIndexPartialColumnViewModel() { Title = "Location", Width=9, Value = new List<string>(){ GetLocationInstanceNameBefore(r.RequestLocationInstances.FirstOrDefault().LocationInstance) } },
                             new RequestIndexPartialColumnViewModel() { Title = "Category", Width=11, Value = AppUtility.GetCategoryColumn(requestIndexObject.CategorySelected, requestIndexObject.SubcategorySelected, r.Product.ProductSubcategory), FilterEnum=AppUtility.FilterEnum.Category },
                             new RequestIndexPartialColumnViewModel() { Title = "Owner", Width=10, Value = new List<string>(){r.ApplicationUserCreator.FirstName + " " + r.ApplicationUserCreator.LastName} },
                             new RequestIndexPartialColumnViewModel() { Title = "Date Created", Width=10, Value = new List<string>(){ r.CreationDate.ToString("dd'/'MM'/'yyyy") } },
                             new RequestIndexPartialColumnViewModel()
                             {
                                 Title = "", Width=10, Icons = iconList, AjaxID = r.RequestID
                             }
                        }
            }).ToPagedListAsync(requestIndexObject.PageNumber == 0 ? 1 : requestIndexObject.PageNumber, 25);
            return onePageOfProducts;
        }
        protected async Task<IPagedList<RequestIndexPartialRowViewModel>> GetSummaryRows(RequestIndexObject requestIndexObject, IPagedList<RequestIndexPartialRowViewModel> onePageOfProducts, IQueryable<Request> RequestPassedInWithInclude, List<IconColumnViewModel> iconList, string defaultImage)
        {

            onePageOfProducts = await RequestPassedInWithInclude.OrderByDescending(r => r.ParentRequest.OrderDate).ToList().Select(r => new RequestIndexPartialRowViewModel()
            {
                Columns = new List<RequestIndexPartialColumnViewModel>()
                        {
                             new RequestIndexPartialColumnViewModel() { Title = "", Width=9, Image = r.Product.ProductSubcategory.ImageURL==null?defaultImage: r.Product.ProductSubcategory.ImageURL},
                             new RequestIndexPartialColumnViewModel() { Title = "Item Name", Width=14, Value = new List<string>(){ r.Product.ProductName}, AjaxLink = "load-product-details-summary", AjaxID=r.RequestID},
                             new RequestIndexPartialColumnViewModel() { Title = "Vendor", Width=9, Value = new List<string>(){ r.Product.Vendor.VendorEnName} },
                             new RequestIndexPartialColumnViewModel() { Title = "Amount", Width=9, Value = AppUtility.GetAmountColumn(r, r.UnitType, r.SubUnitType, r.SubSubUnitType)},
                             new RequestIndexPartialColumnViewModel() { Title = "Location", Width=9, Value = new List<string>(){ GetLocationInstanceNameBefore(r.RequestLocationInstances.FirstOrDefault().LocationInstance) } },
                             new RequestIndexPartialColumnViewModel() { Title = "Category", Width=11, Value = AppUtility.GetCategoryColumn(requestIndexObject.CategorySelected, requestIndexObject.SubcategorySelected, r.Product.ProductSubcategory), FilterEnum=AppUtility.FilterEnum.Category },
                             new RequestIndexPartialColumnViewModel() { Title = "Owner", Width=10, Value = new List<string>(){r.ApplicationUserCreator.FirstName + " " + r.ApplicationUserCreator.LastName} },
                             new RequestIndexPartialColumnViewModel() { Title = "Price", Width=10, Value = AppUtility.GetPriceColumn(requestIndexObject.SelectedPriceSort, r, requestIndexObject.SelectedCurrency), FilterEnum=AppUtility.FilterEnum.Price},
                             new RequestIndexPartialColumnViewModel() { Title = "Date Ordered", Width=10, Value = new List<string>(){ r.ParentRequest.OrderDate.ToString("dd'/'MM'/'yyyy") } },
                             new RequestIndexPartialColumnViewModel()
                             {
                                 Title = "", Width=10, Icons = iconList, AjaxID = r.RequestID
                             }
                        }
            }).ToPagedListAsync(requestIndexObject.PageNumber == 0 ? 1 : requestIndexObject.PageNumber, 25);
            return onePageOfProducts;
        }

        protected static async Task<IPagedList<RequestIndexPartialRowViewModel>> GetAccountingGeneralRows(RequestIndexObject requestIndexObject, IPagedList<RequestIndexPartialRowViewModel> onePageOfProducts, IQueryable<Request> RequestPassedInWithInclude, List<IconColumnViewModel> iconList, string defaultImage)
        {
            onePageOfProducts = await RequestPassedInWithInclude.OrderByDescending(r => r.ParentRequest.OrderDate).ToList().Select(r => new RequestIndexPartialRowViewModel()
            {
                Columns = new List<RequestIndexPartialColumnViewModel>()
                            {
                                 new RequestIndexPartialColumnViewModel() { Title = "", Width=10, Image = r.Product.ProductSubcategory.ImageURL==null?defaultImage: r.Product.ProductSubcategory.ImageURL},
                                 new RequestIndexPartialColumnViewModel() { Title = "Item Name", Width=15, Value = new List<string>(){ r.Product.ProductName}, AjaxLink = "load-product-details-summary", AjaxID=r.RequestID},
                                 new RequestIndexPartialColumnViewModel() { Title = "Amount", Width=10, Value = AppUtility.GetAmountColumn(r, r.UnitType, r.SubUnitType, r.SubSubUnitType)},
                                 new RequestIndexPartialColumnViewModel() { Title = "Price", Width=10, Value = AppUtility.GetPriceColumn(requestIndexObject.SelectedPriceSort, r,  requestIndexObject.SelectedCurrency), FilterEnum=AppUtility.FilterEnum.Price},
                                 new RequestIndexPartialColumnViewModel() { Title = "Vendor", Width=10, Value = new List<string>(){ r.Product.Vendor.VendorEnName} },
                                 new RequestIndexPartialColumnViewModel() { Title = "Category", Width=11, Value = new List<string>(){ r.Product.ProductSubcategory.ProductSubcategoryDescription} },
                                 new RequestIndexPartialColumnViewModel() { Title = "Date Ordered", Width=12, Value = new List<string>(){ r.ParentRequest.OrderDate.ToString("dd'/'MM'/'yyyy") } }
                            }
            }).ToPagedListAsync(requestIndexObject.PageNumber == 0 ? 1 : requestIndexObject.PageNumber, 25);
            return onePageOfProducts;
        }

        protected async Task<IPagedList<RequestIndexPartialRowViewModel>> GetReceivedInventoryFavoriteRows(RequestIndexObject requestIndexObject, IPagedList<RequestIndexPartialRowViewModel> onePageOfProducts, IQueryable<Request> RequestPassedInWithInclude, List<IconColumnViewModel> iconList, string defaultImage)
        {
            var newIconList = iconList;
            onePageOfProducts = await RequestPassedInWithInclude.OrderByDescending(r => r.ArrivalDate).ToList().Select(r => new RequestIndexPartialRowViewModel()
            {
                Columns = new List<RequestIndexPartialColumnViewModel>()
                        {
                             new RequestIndexPartialColumnViewModel() { Title = "", Width=9, Image = r.Product.ProductSubcategory.ImageURL==null?defaultImage: r.Product.ProductSubcategory.ImageURL},
                             new RequestIndexPartialColumnViewModel() { Title = "Item Name", Width=14, Value = new List<string>(){ r.Product.ProductName}, AjaxLink = "load-product-details", AjaxID=r.RequestID},
                             new RequestIndexPartialColumnViewModel() { Title = "Vendor", Width=9, Value = new List<string>(){ r.Product.Vendor?.VendorEnName ?? ""} },
                             new RequestIndexPartialColumnViewModel() { Title = "Amount", Width=9, Value = AppUtility.GetAmountColumn(r, r.UnitType, r.SubUnitType, r.SubSubUnitType)},
                             new RequestIndexPartialColumnViewModel() { Title = "Location", Width=9, Value = new List<string>(){ GetLocationInstanceNameBefore(r.RequestLocationInstances.FirstOrDefault().LocationInstance) } },
                             new RequestIndexPartialColumnViewModel() { Title = "Category", Width=11, Value = AppUtility.GetCategoryColumn(requestIndexObject.CategorySelected, requestIndexObject.SubcategorySelected, r.Product.ProductSubcategory), FilterEnum=AppUtility.FilterEnum.Category },
                             new RequestIndexPartialColumnViewModel() { Title = "Owner", Width=10, Value = new List<string>(){r.ApplicationUserCreator.FirstName + " " + r.ApplicationUserCreator.LastName} },
                             new RequestIndexPartialColumnViewModel() { Title = "Price", Width=10, Value = AppUtility.GetPriceColumn(requestIndexObject.SelectedPriceSort, r, requestIndexObject.SelectedCurrency), FilterEnum=AppUtility.FilterEnum.Price},
                             new RequestIndexPartialColumnViewModel() { Title = "Arrival Date", Width=10, Value = new List<string>(){ r.ArrivalDate.ToString("dd'/'MM'/'yyyy") } },
                             new RequestIndexPartialColumnViewModel()
                             {
                                 Title = "", Width=10, Icons = GetIconsByIndividualRequest(r.RequestID, newIconList, false), AjaxID = r.RequestID
                             }
                        }
            }).ToPagedListAsync(requestIndexObject.PageNumber == 0 ? 1 : requestIndexObject.PageNumber, 25);
            return onePageOfProducts;
        }
        protected async Task<IPagedList<RequestIndexPartialRowViewModel>> GetReceivedInventorySharedRows(RequestIndexObject requestIndexObject, IPagedList<RequestIndexPartialRowViewModel> onePageOfProducts, IQueryable<Request> RequestPassedInWithInclude, List<IconColumnViewModel> iconList, string defaultImage)
        {
            var newIconList = iconList;
            onePageOfProducts = await RequestPassedInWithInclude.Include(sr => sr.ShareRequests).ToList().Select(r => new RequestIndexPartialRowViewModel()
            {
                Columns = new List<RequestIndexPartialColumnViewModel>()
                        {
                             new RequestIndexPartialColumnViewModel() { Title = "", Width=9, Image = r.Product.ProductSubcategory.ImageURL==null?defaultImage: r.Product.ProductSubcategory.ImageURL},
                             new RequestIndexPartialColumnViewModel() { Title = "Item Name", Width=14, Value = new List<string>(){ r.Product.ProductName}, AjaxLink = "load-product-details", AjaxID=r.RequestID},
                             new RequestIndexPartialColumnViewModel() { Title = "Vendor", Width=9, Value = new List<string>(){ r.Product.Vendor.VendorEnName} },
                             new RequestIndexPartialColumnViewModel() { Title = "Amount", Width=9, Value = AppUtility.GetAmountColumn(r, r.UnitType, r.SubUnitType, r.SubSubUnitType)},
                             new RequestIndexPartialColumnViewModel() { Title = "Location", Width=9, Value = new List<string>(){ GetLocationInstanceNameBefore(r.RequestLocationInstances.FirstOrDefault().LocationInstance) } },
                             new RequestIndexPartialColumnViewModel() { Title = "Category", Width=11, Value = AppUtility.GetCategoryColumn(requestIndexObject.CategorySelected, requestIndexObject.SubcategorySelected, r.Product.ProductSubcategory), FilterEnum=AppUtility.FilterEnum.Category },
                             new RequestIndexPartialColumnViewModel() { Title = "Owner", Width=10, Value = new List<string>(){r.ApplicationUserCreator.FirstName + " " + r.ApplicationUserCreator.LastName} },
                             new RequestIndexPartialColumnViewModel() { Title = "Price", Width=10, Value = AppUtility.GetPriceColumn(requestIndexObject.SelectedPriceSort, r, requestIndexObject.SelectedCurrency), FilterEnum=AppUtility.FilterEnum.Price},
                             new RequestIndexPartialColumnViewModel() { Title = "Shared By", Width=10, Value = new List<string>(){ GetSharedBy(r) } },
                             new RequestIndexPartialColumnViewModel()
                             {
                                 Title = "", Width=10, Icons = GetIconsByIndividualRequest(r.RequestID, newIconList, false), AjaxID = r.RequestID
                             }
                        }
            }).ToPagedListAsync(requestIndexObject.PageNumber == 0 ? 1 : requestIndexObject.PageNumber, 25);
            return onePageOfProducts;
        }

        protected List<IconColumnViewModel> GetIconsByIndividualRequest(int RequestID, List<IconColumnViewModel> iconList, bool needsPlaceholder)
        {
            var newIconList = AppUtility.DeepClone(iconList);
            //favorite icon
            var favIconIndex = newIconList.FindIndex(ni => ni.IconAjaxLink.Contains("request-favorite"));
            var favoriteRequest = _context.FavoriteRequests.Where(fr => fr.RequestID == RequestID).Where(fr => fr.ApplicationUserID == _userManager.GetUserId(User)).FirstOrDefault();
            if (favIconIndex != -1 && favoriteRequest != null) //check these checks
            {
                var unLikeIcon = new IconColumnViewModel(" icon-favorite-24px", "#5F79E2", "request-favorite request-unlike", "Unfavorite");
                newIconList[favIconIndex] = unLikeIcon;
            }
            //for approval icon
            var forApprovalIconIndex = newIconList.FindIndex(ni => ni.IconAjaxLink.Contains("approve-order"));
            var request = _context.Requests.Where(r => r.RequestID == RequestID).Include(r => r.ParentQuote).FirstOrDefault();
            if (request.RequestStatusID != 1 && forApprovalIconIndex != -1)
            {
                newIconList.RemoveAt(forApprovalIconIndex);
                needsPlaceholder = true;
            }
            //resend icon
            if (request.RequestStatusID != 1 && request.ParentQuote?.QuoteStatusID == 2)
            {
                var resendIcon = new IconColumnViewModel("Resend");
                newIconList.Insert(0, resendIcon);
            }
            else if (needsPlaceholder) //for approval and resend placeholder
            {
                var placeholderString = request.RequestStatusID == 1 ? "ApprovePlaceholder" : "ResendPlaceholder";
                var placeholder = new IconColumnViewModel(placeholderString);
                newIconList.Insert(0, placeholder);
            }

            return newIconList;
        }


        protected string GetLocationInstanceNameBefore(LocationInstance locationInstance)
        {
            var newLIName = "";
            if (locationInstance.LocationInstanceParentID == null)//is temporary location
            {
                newLIName = _context.TemporaryLocationInstances.Where(li => li.LocationInstanceID == locationInstance.LocationInstanceID).FirstOrDefault().LocationInstanceAbbrev;
            }
            else
            {
                newLIName = _context.LocationInstances.Where(li => li.LocationInstanceID == locationInstance.LocationInstanceParentID).FirstOrDefault().LocationInstanceAbbrev;
                if (newLIName == null)
                {
                    while (locationInstance.LocationInstanceParentID != null)
                    {
                        newLIName = _context.LocationInstances.OfType<LocationInstance>().Where(li => li.LocationInstanceID == locationInstance.LocationInstanceParentID).Include(li => li.LocationInstanceParent).FirstOrDefault().LocationInstanceName + newLIName;
                        locationInstance = locationInstance.LocationInstanceParent;
                    }
                }
            }
            return newLIName;
        }

        protected String GetSharedBy(Request request)
        {
            var applicationUser = _context.ShareRequests
                .Where(sr => sr.RequestID == request.RequestID).Where(sr => sr.ToApplicationUserID == _userManager.GetUserId(User))
                .Include(sr => sr.FromApplicationUser)
                .FirstOrDefault().FromApplicationUser;
            return applicationUser.FirstName + " " + applicationUser.LastName;
            //return "";
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
            requestStatusIds = requestStatusIds.Where(id => id != requestIndexObject.RequestStatusID).ToArray();
            foreach (int statusId in requestStatusIds)
            {
                SetCountByStatusId(requestIndexObject, viewmodel, fullRequestsList, statusId);
            }

            SetCountByStatusId(requestIndexObject, viewmodel, changingList, requestIndexObject.RequestStatusID);


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


    }
}
