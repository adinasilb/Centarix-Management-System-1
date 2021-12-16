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
using System.Net.Http;
using System.Linq.Expressions;

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
        protected readonly CRUD.VendorsProc _vendorsProc;
        protected readonly CRUD.CategoryTypesProc _categoryType;
        protected readonly CRUD.CountriesProc _country;
        protected readonly CRUD.CommentTypesProc _commentType;
        protected readonly CRUD.RequestsProc _requestsProc;
        protected readonly CRUD.EmployeeHoursProc _employeeHoursProc;
        protected readonly CRUD.TimekeeperNotificationsProc _timekeeperNotificationsProc;
        protected readonly CRUD.EmployeesProc _employeesProc;
        protected readonly CRUD.EmployeeHoursAwaitingApprovalProc _employeeHoursAwaitingApprovalProc;
        protected readonly CRUD.EmployeeHoursStatusesProc _employeeHoursStatuesProc;
        protected readonly CRUD.OffDayTypesProc _offDayTypesProc;
        protected readonly CRUD.CompanyDaysOffProc _companyDaysOffProc;
        protected readonly CRUD.ParticipantsProc _participantsProc;
        protected readonly CRUD.GendersProc _gendersProc;
        protected readonly CRUD.ParticipantStatusesProc _participantStatusesProc;
        protected readonly CRUD.ExperimentEntriesProc _experimentEntriesProc;
        protected readonly CRUD.SitesProc _sitesProc;
        protected readonly CRUD.ExperimentsProc _experimentsProc;
        protected readonly CRUD.TestsProc _testsProc;
        protected readonly CRUD.TestValuesProc _testValuesProc;
        protected readonly CRUD.ProductsProc _productsProc;
        protected readonly CRUD.GlobalInfosProc _globalInfosProc;
        protected readonly CRUD.RequestCommentsProc _requestCommentsProc;
        protected readonly CRUD.ProductCommentsProc _productCommentsProc;
        protected readonly CRUD.LocationInstancesProc _locationInstancesProc;
        protected readonly CRUD.TemporaryLocationInstancesProc _temporaryLocationInstancesProc;
        protected readonly CRUD.LocationTypesProc _locationTypesProc;
        protected readonly CRUD.LabPartsProc _lapPartsProc;
        protected readonly CRUD.ShareRequestsProc _shareRequestsProc;
        protected readonly CRUD.FavoriteRequestsProc _favoriteRequestsProc;
        protected readonly CRUD.ProductSubcategoriesProc _productSubcategoriesProc;
        protected readonly CRUD.ParentCategoriesProc _parentCategoriesProc;
        protected readonly CRUD.UnitTypesProc _unitTypesProc;
        protected readonly CRUD.RequestListRequestsProc _requestListRequestsProc;
        protected readonly CRUD.ShareRequestListsProc _shareRequestListsProc;
        protected readonly CRUD.PaymentTypesProc _paymentTypesProc;
        protected readonly CRUD.CompanyAccountsProc _companyAccountsProc;
        protected readonly CRUD.ProjectsProc _projectsProc;
        protected readonly CRUD.SubProjectsProc _subProjectsProc;
        public SharedController(ApplicationDbContext context, UserManager<ApplicationUser> userManager, IHostingEnvironment hostingEnvironment, ICompositeViewEngine viewEngine, IHttpContextAccessor httpContextAccessor)

        {
            _context = context;
            _hostingEnvironment = hostingEnvironment;
            _userManager = userManager;
            _viewEngine = viewEngine;
            _httpContextAccessor = httpContextAccessor;
            _vendorsProc = new CRUD.VendorsProc(context);
            _categoryType = new CRUD.CategoryTypesProc(context);
            _country = new CRUD.CountriesProc(context);
            _commentType = new CRUD.CommentTypesProc(context);
            _requestsProc = new CRUD.RequestsProc(context);
            _employeeHoursProc = new CRUD.EmployeeHoursProc(context);
            _timekeeperNotificationsProc = new CRUD.TimekeeperNotificationsProc(context);
            _employeesProc = new CRUD.EmployeesProc(context);
            _employeeHoursAwaitingApprovalProc = new CRUD.EmployeeHoursAwaitingApprovalProc(context);
            _employeeHoursStatuesProc = new CRUD.EmployeeHoursStatusesProc(context);
            _offDayTypesProc = new CRUD.OffDayTypesProc(context);
            _companyDaysOffProc = new CRUD.CompanyDaysOffProc(context);
            _participantsProc = new CRUD.ParticipantsProc(context);
            _gendersProc = new CRUD.GendersProc(context);
            _participantStatusesProc = new CRUD.ParticipantStatusesProc(context);
            _experimentEntriesProc = new CRUD.ExperimentEntriesProc(context);
            _productsProc = new CRUD.ProductsProc(context);
            _globalInfosProc = new CRUD.GlobalInfosProc(context);
            _requestCommentsProc = new CRUD.RequestCommentsProc(context);
            _productCommentsProc = new CRUD.ProductCommentsProc(context);
            _sitesProc = new CRUD.SitesProc(context);
            _experimentsProc = new CRUD.ExperimentsProc(context);
            _testsProc = new CRUD.TestsProc(context);
            _testValuesProc = new CRUD.TestValuesProc(context);
            _locationTypesProc = new CRUD.LocationTypesProc(context);
            _locationInstancesProc = new CRUD.LocationInstancesProc(context);
            _temporaryLocationInstancesProc = new CRUD.TemporaryLocationInstancesProc(context);
            _lapPartsProc = new CRUD.LabPartsProc(context);
            _shareRequestsProc = new CRUD.ShareRequestsProc(context);
            _favoriteRequestsProc = new CRUD.FavoriteRequestsProc(context);
            _productSubcategoriesProc = new CRUD.ProductSubcategoriesProc(context);
            _parentCategoriesProc = new CRUD.ParentCategoriesProc(context);
            _unitTypesProc = new CRUD.UnitTypesProc(context);
            _requestListRequestsProc = new CRUD.RequestListRequestsProc(context);
            _shareRequestListsProc = new CRUD.ShareRequestListsProc(context);
            _companyAccountsProc = new CRUD.CompanyAccountsProc(context);
            _paymentTypesProc = new CRUD.PaymentTypesProc(context);
            _projectsProc = new CRUD.ProjectsProc(context);
            _subProjectsProc = new CRUD.SubProjectsProc(context);

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
        private async Task<List<EmployeeHoursAndAwaitingApprovalViewModel>> GetHoursAsync(int year, int month, Employee user)
        {
            var hours = _employeeHoursProc.Read(new List<Expression<Func<EmployeeHours, bool>>> { eh => eh.Date.Month == month && eh.Date.Year == year && eh.Date.Date <= DateTime.Now.Date, eh => eh.EmployeeID == user.Id },
            new List<ComplexIncludes<EmployeeHours, ModelBase>>{
                new ComplexIncludes<EmployeeHours, ModelBase> {Include = eh => eh.Employee },
                new ComplexIncludes<EmployeeHours, ModelBase> {Include = eh => eh.OffDayType },
                new ComplexIncludes<EmployeeHours, ModelBase> {Include = eh => eh.EmployeeHoursStatusEntry1 },
                new ComplexIncludes<EmployeeHours, ModelBase> {Include = eh => eh.CompanyDayOff,
                    ThenInclude = new ComplexIncludes<ModelBase, ModelBase> {Include = cdo => ((CompanyDayOff)cdo).CompanyDayOffType }},
                new ComplexIncludes<EmployeeHours, ModelBase> {Include = eh => eh.PartialOffDayType }
            }).OrderByDescending(eh => eh.Date).ToList();

            List<EmployeeHoursAndAwaitingApprovalViewModel> hoursList = new List<EmployeeHoursAndAwaitingApprovalViewModel>();
            foreach (var hour in hours)
            {
                var ehaaavm = new EmployeeHoursAndAwaitingApprovalViewModel()
                {
                    EmployeeHours = hour
                };
                var eha = await _employeeHoursAwaitingApprovalProc.ReadOneAsync(new List<Expression<Func<EmployeeHoursAwaitingApproval, bool>>> { ehaa => ehaa.EmployeeHoursID == hour.EmployeeHoursID });
                if (eha != null)
                {
                    ehaaavm.EmployeeHoursAwaitingApproval = eha;
                }
                hoursList.Add(ehaaavm);
            }
            return hoursList;
        }
        protected async Task<SummaryHoursViewModel> SummaryHoursFunctionAsync(int month, int year, Employee user, string errorMessage = null)
        {
            var hours = await GetHoursAsync(year, month, user);
            var CurMonth = new DateTime(year, month, 1);
            double? totalhours;
            double totalDays = 0;
            double workingDays = 0;
            double vacationDaysTaken = 0;
            double sickDaysTaken = 0;
            var companyDaysOff = _companyDaysOffProc.Read(includes: new List<ComplexIncludes<CompanyDayOff, ModelBase>> { new ComplexIncludes<CompanyDayOff, ModelBase> { Include = co => co.CompanyDayOffType } }).ToList();
            var employeeHours = _employeeHoursProc.Read(new List<Expression<Func<EmployeeHours, bool>>> { eh => eh.EmployeeID == user.Id && eh.Date.Month == month && eh.Date.Year == year && eh.Date <= DateTime.Now.Date });
            int unpaidLeave = 0;
            if (user.EmployeeStatusID != 1)
            {
                totalhours = null;
            }
            else
            {
                var sickHours = employeeHours.Where(eh => eh.PartialOffDayTypeID == 1)
                    .Select(eh => (eh.PartialOffDayHours == null ? TimeSpan.Zero : ((TimeSpan)eh.PartialOffDayHours)).TotalHours).ToList().Sum(p => p);
                sickDaysTaken = employeeHours.Where(eh => eh.OffDayTypeID == 1).Count();
                sickDaysTaken = sickDaysTaken + (sickHours / user.SalariedEmployee.HoursPerDay);

                var vacationHours = employeeHours.Where(eh => eh.PartialOffDayTypeID == 2)
                    .Select(eh => (eh.PartialOffDayHours == null ? TimeSpan.Zero : ((TimeSpan)eh.PartialOffDayHours)).TotalHours).ToList().Sum(p => p);
                vacationDaysTaken = employeeHours.Where(eh => eh.OffDayTypeID == 2).Count();
                vacationDaysTaken = vacationDaysTaken + (vacationHours / user.SalariedEmployee.HoursPerDay);
                var specialDays = employeeHours.Where(eh => eh.OffDayTypeID == 4).Count();
                unpaidLeave = employeeHours.Where(eh => eh.OffDayTypeID == 5).Count();
                totalDays = GetTotalWorkingDaysThisMonth(new DateTime(year, month, 1), companyDaysOff);
                totalhours = (totalDays - (vacationDaysTaken + sickDaysTaken + unpaidLeave + specialDays)) * user.SalariedEmployee.HoursPerDay;
                workingDays = employeeHours.Where(eh => (eh.OffDayTypeID == null))
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

        protected async Task<decimal> GetExchangeRateAsync()
        {
            decimal rate;
            var globalInfo = await _globalInfosProc.ReadOneAsync(new List<Expression<Func<GlobalInfo, bool>>> { gi => gi.GlobalInfoType == AppUtility.GlobalInfoType.ExchangeRate.ToString() });
            var parsed = decimal.TryParse(globalInfo.Description, out rate);
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
            var MiddleFolderName = "";
            if (documentsModalViewModel.FolderName == AppUtility.FolderNamesEnum.Custom && documentsModalViewModel.ParentFolderName == AppUtility.ParentFolderName.ExperimentEntries)
            {
                MiddleFolderName = documentsModalViewModel.Guid.ToString();
            }
            else
            {
                MiddleFolderName = documentsModalViewModel.ObjectID == "0" ? documentsModalViewModel.Guid.ToString() : documentsModalViewModel.ObjectID;
            }
            var folder = Path.Combine(uploadFolder, MiddleFolderName);
            Directory.CreateDirectory(folder);
            if (documentsModalViewModel.FilesToSave != null) //test for more than one???
            {
                var x = 1;
                foreach (IFormFile file in documentsModalViewModel.FilesToSave)
                {
                    //create file
                    var folderName = documentsModalViewModel.FolderName.ToString();
                    if (documentsModalViewModel.FolderName == AppUtility.FolderNamesEnum.Custom && documentsModalViewModel.ParentFolderName == AppUtility.ParentFolderName.ExperimentEntries)
                    {
                        folderName = documentsModalViewModel.ObjectID.ToString();
                    }
                    string folderPath = Path.Combine(folder, folderName);
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

        [HttpPost]
        public string UploadFile(DocumentsModalViewModel documentsModalViewModel)
        {
            string uploadFolder = Path.Combine(_hostingEnvironment.WebRootPath, documentsModalViewModel.ParentFolderName.ToString());
            var MiddleFolderName = "";
            var fileName = "";
            if (documentsModalViewModel.FolderName == AppUtility.FolderNamesEnum.Custom && documentsModalViewModel.ParentFolderName == AppUtility.ParentFolderName.ExperimentEntries)
            {
                MiddleFolderName = documentsModalViewModel.Guid.ToString();
            }
            else
            {
                MiddleFolderName = documentsModalViewModel.ObjectID == "0" ? documentsModalViewModel.Guid.ToString() : documentsModalViewModel.ObjectID;
            }
            var folder = Path.Combine(uploadFolder, MiddleFolderName);
            Directory.CreateDirectory(folder);
            if (documentsModalViewModel.FilesToSave != null)
            {
                fileName = documentsModalViewModel.FilesToSave[0].FileName;
                var folderName = documentsModalViewModel.FolderName.ToString();
                if (documentsModalViewModel.FolderName == AppUtility.FolderNamesEnum.Custom && documentsModalViewModel.ParentFolderName == AppUtility.ParentFolderName.ExperimentEntries)
                {
                    folderName = documentsModalViewModel.ObjectID.ToString();
                }
                string folderPath = Path.Combine(folder, folderName);
                Directory.CreateDirectory(folderPath);
                var uniqueFilePath = Path.Combine(folderPath, "_Part2"+ fileName);
                var uniqueFilePathOld = Path.Combine(folderPath, fileName);

                var files = Directory.GetFiles(folderPath).Where(f => f==uniqueFilePathOld).ToList();
                if (!documentsModalViewModel.IsFirstPart)
                {
                    foreach (var file in files)
                    {
                        FileStream fileStreamOld = new FileStream(file, FileMode.Append);
                        FileStream fileStream = new FileStream(uniqueFilePath, FileMode.OpenOrCreate);
                        documentsModalViewModel.FilesToSave[0].CopyTo(fileStream);
                        fileStream.Close();
                        var fileBytes = System.IO.File.ReadAllBytes(uniqueFilePath);
                        fileStreamOld.Write(fileBytes);
                        fileStreamOld.Close();

                        System.IO.File.Delete(uniqueFilePath);

                    }

                }
                else
                {
                    if (files.Count>0)
                    {
                        fileName=(files.Count+1)+ fileName;
                    }
                    uniqueFilePathOld =Path.Combine(folderPath, fileName);
                    FileStream filestream = new FileStream(uniqueFilePathOld, FileMode.Create);
                    documentsModalViewModel.FilesToSave[0].CopyTo(filestream);
                    filestream.Close();
                }

            }
            return fileName;
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
            if (documentsModalViewModel.FolderName == AppUtility.FolderNamesEnum.Custom && documentsModalViewModel.ParentFolderName == AppUtility.ParentFolderName.ExperimentEntries)
            {
                string uploadFolder1 = Path.Combine(_hostingEnvironment.WebRootPath, documentsModalViewModel.ParentFolderName.ToString());
                var MiddleFolderName = documentsModalViewModel.Guid.ToString();
                string uploadFolder2 = Path.Combine(uploadFolder1, MiddleFolderName);
                var FolderName = documentsModalViewModel.ObjectID.ToString();
                string uploadFolder3 = Path.Combine(uploadFolder2, FolderName);

                if (Directory.Exists(uploadFolder3))
                {
                    documentsModalViewModel = SaveDocuments(uploadFolder3, documentsModalViewModel);
                }
                string uploadFolderA = Path.Combine(_hostingEnvironment.WebRootPath, documentsModalViewModel.ParentFolderName.ToString());
                var MiddleFolderNameA = documentsModalViewModel.CustomMainObjectID.ToString();
                string uploadFolderB = Path.Combine(uploadFolderA, MiddleFolderNameA);
                var FolderNameB = documentsModalViewModel.ObjectID.ToString();
                string uploadFolderC = Path.Combine(uploadFolderB, FolderNameB);

                if (Directory.Exists(uploadFolderC))
                {
                    documentsModalViewModel = SaveDocuments(uploadFolderC, documentsModalViewModel);
                }
            }
            else
            {
                string uploadFolder1 = Path.Combine(_hostingEnvironment.WebRootPath, documentsModalViewModel.ParentFolderName.ToString());
                var MiddleFolderName = documentsModalViewModel.ObjectID == "0" ? documentsModalViewModel.Guid.ToString() : documentsModalViewModel.ObjectID;
                string uploadFolder2 = Path.Combine(uploadFolder1, MiddleFolderName);
                var FolderName = documentsModalViewModel.FolderName == AppUtility.FolderNamesEnum.Custom && documentsModalViewModel.ParentFolderName == AppUtility.ParentFolderName.ExperimentEntries ? documentsModalViewModel.ObjectID.ToString() : documentsModalViewModel.FolderName.ToString();
                string uploadFolder3 = Path.Combine(uploadFolder2, FolderName);

                if (Directory.Exists(uploadFolder3))
                {
                    documentsModalViewModel = SaveDocuments(uploadFolder3, documentsModalViewModel);
                }
            }

        }

        protected DocumentsModalViewModel SaveDocuments(string FinalUploadFolder, DocumentsModalViewModel documentsModalViewModel)
        {
            DirectoryInfo DirectoryToSearch = new DirectoryInfo(FinalUploadFolder);
            //searching for the partial file name in the directory
            FileInfo[] docfilesfound = DirectoryToSearch.GetFiles("*.*");
            if (documentsModalViewModel.FileStrings == null)
            {
                documentsModalViewModel.FileStrings = new List<String>();
            }
            foreach (var docfile in docfilesfound)
            {

                string newFileString = AppUtility.GetLastFiles(docfile.FullName, 4);
                documentsModalViewModel.FileStrings.Add(newFileString);
                //documentsModalViewModel.Files.Add(docfile);
            }
            return documentsModalViewModel;
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

            var productId = await _requestsProc.ReadOneWithIgnoreQueryFilters(new List<Expression<Func<Request, bool>>> { r => r.RequestID == id }).Select(r => r.ProductID).FirstOrDefaultAsync();

            var request = await _requestsProc.ReadOneWithIgnoreQueryFiltersAsync(new List<Expression<Func<Request, bool>>> { r=>r.RequestID == id},  new List<ComplexIncludes<Request, ModelBase>>
            {
                new ComplexIncludes<Request, ModelBase> { Include = r => r.Product },
                new ComplexIncludes<Request, ModelBase> { Include = r => r.ParentQuote },
                new ComplexIncludes<Request, ModelBase> { Include = r => r.ParentRequest },
                new ComplexIncludes<Request, ModelBase> { Include = r => r.Product.ProductSubcategory },
                new ComplexIncludes<Request, ModelBase> { Include = r => r.Product.ProductSubcategory.ParentCategory },
                new ComplexIncludes<Request, ModelBase>
                {
                    Include = r => r.Product.Vendor,
                    ThenInclude=  new ComplexIncludes<ModelBase, ModelBase> { Include = v => ((Vendor)v).Country }
                },
                new ComplexIncludes<Request, ModelBase> { Include = r => r.RequestStatus},
                new ComplexIncludes<Request, ModelBase> { Include = r => r.ApplicationUserCreator},
                new ComplexIncludes<Request, ModelBase> { Include = r => r.PaymentStatus },
                new ComplexIncludes<Request, ModelBase>
                {
                    Include = r => r.Payments,
                    ThenInclude= new ComplexIncludes<ModelBase, ModelBase> { Include = p => ((Payment)p).CompanyAccount }
                },
                new ComplexIncludes<Request, ModelBase>
                {
                    Include = r => r.Payments,
                    ThenInclude = new ComplexIncludes<ModelBase, ModelBase> { Include = p => ((Payment)p).CreditCard}
                },
                new ComplexIncludes<Request, ModelBase>
                {
                    Include = r => r.Payments,
                    ThenInclude = new ComplexIncludes<ModelBase, ModelBase> { Include = p => ((Payment)p).Invoice}
                },
                new ComplexIncludes<Request, ModelBase> { Include = r => r.ApplicationUserReceiver }
            });


if (request.RequestStatusID == 7)
{
    isProprietary = true;
}

var requestsByProduct = _requestsProc.ReadWithIgnoreQueryFilters( new List<Expression<Func<Request, bool>>> { r => r.ProductID == productId, r => r.Product.ProductSubcategory.ParentCategory.CategoryTypeID == categoryType },
    new List<ComplexIncludes<Request, ModelBase>>
    {
        new ComplexIncludes<Request, ModelBase>{Include = r => r.Product.ProductSubcategory },
        new ComplexIncludes<Request, ModelBase>{Include =r => r.Product.ProductSubcategory.ParentCategory },
        new ComplexIncludes<Request, ModelBase>{Include = r => r.ApplicationUserCreator },
        new ComplexIncludes<Request, ModelBase>{Include = r => r.ParentRequest },
        new ComplexIncludes<Request, ModelBase>{Include = r => r.ApplicationUserReceiver },
    }).OrderByDescending(r => r.CreationDate).ToList();

RequestItemViewModel requestItemViewModel = new RequestItemViewModel();
await FillRequestDropdowns(requestItemViewModel, request.Product.ProductSubcategory, categoryType);

requestItemViewModel.Tab = Tab ?? 0;
var requestComments = _requestCommentsProc.Read( new List<Expression<Func<RequestComment, bool>>> { r => r.ObjectID == request.RequestID },
    new List<ComplexIncludes<RequestComment, ModelBase>> { 
        new ComplexIncludes<RequestComment, ModelBase> { Include = r => r.ApplicationUser },
        new ComplexIncludes<RequestComment, ModelBase> { Include = r => r.CommentType } } ).ToList();
var productComments = _productCommentsProc.Read(new List<Expression<Func<ProductComment, bool>>> { r => r.ObjectID == request.ProductID },
    new List<ComplexIncludes<ProductComment, ModelBase>> {
        new ComplexIncludes<ProductComment, ModelBase> { Include = r => r.ApplicationUser },
        new ComplexIncludes<ProductComment, ModelBase> { Include = r => r.CommentType } }).ToList();
            requestItemViewModel.Comments = requestComments.Concat<CommentBase>(productComments).OrderByDescending(rc => rc.CommentTimeStamp).ToList();
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

if (_requestsProc.Read( new List<Expression<Func<Request, bool>>> { r => r.ProductID == request.ProductID }).Count() > 1)
{
    requestItemViewModel.IsReorder = true;
}

ModalViewType = "Edit";
requestItemViewModel.Requests.Add(request);

//load the correct list of subprojects
//var subprojects = await proc.SubProjects
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
//can't look for proc.RequestLocationInstances b/c it's a join table and doesn't have a dbset]
var request1 = await _requestsProc.ReadOneWithIgnoreQueryFiltersAsync( new List<Expression<Func<Request, bool>>> { r => r.RequestID == id }, new List<ComplexIncludes<Request, ModelBase>>{
    new ComplexIncludes<Request, ModelBase>{Include = r => r.RequestLocationInstances, ThenInclude = new  ComplexIncludes<ModelBase, ModelBase>{ Include = rli => ((RequestLocationInstance)rli).LocationInstance } } });
var requestLocationInstances = request1.RequestLocationInstances.ToList();
//if it has => (which it should once its in a details view)
requestItemViewModel.LocationInstances = new List<LocationInstance>();
requestLocationInstances.ForEach(rli => requestItemViewModel.LocationInstances.Add(rli.LocationInstance));
if (request1.RequestStatusID == 3 || request1.RequestStatusID == 5 || request1.RequestStatusID == 4 || request1.RequestStatusID == 7)
{
    ReceivedLocationViewModel receivedLocationViewModel = new ReceivedLocationViewModel()
    {
        Request = await _requestsProc.ReadOneAsync(new List<Expression<Func<Request, bool>>> { r => r.RequestID == request1.RequestID }, new List<ComplexIncludes<Request, ModelBase>>
        {

            new ComplexIncludes<Request, ModelBase> { Include = r => r.Product },
            new ComplexIncludes<Request, ModelBase> { Include = r => r.Product.ProductSubcategory },
            new ComplexIncludes<Request, ModelBase> { Include =r => r.Product.ProductSubcategory.ParentCategory }
        }),
        locationTypesDepthZero = _locationTypesProc.Read( new List<Expression<Func<LocationType, bool>>> { lt => lt.Depth == 0 }),
        locationInstancesSelected = new List<LocationInstance>(),
    };
    requestItemViewModel.ReceivedLocationViewModel = receivedLocationViewModel;

    if (requestLocationInstances.Any())
    {
        //get the parent location instances of the first one
        //can do this now b/c can only be in one box - later on will have to be a list or s/t b/c will have more boxes
        //int? locationInstanceParentID = proc.LocationInstances.OfType<LocationInstance>().Where(li => li.LocationInstanceID == requestLocationInstances[0].LocationInstanceID).FirstOrDefault().LocationInstanceParentID;
        LocationInstance parentLocationInstance = await _locationInstancesProc.ReadOneAsync( new List<Expression<Func<LocationInstance, bool>>> { li => li.LocationInstanceID == requestLocationInstances[0].LocationInstance.LocationInstanceParentID }, new List<ComplexIncludes<LocationInstance, ModelBase>> { new ComplexIncludes<LocationInstance, ModelBase> { Include = li => li.LocationType} });
        //requestItemViewModel.ParentLocationInstance = proc.LocationInstances.OfType<LocationInstance>().Where(li => li.LocationInstanceID == requestLocationInstances[0].LocationInstance.LocationInstanceParentID).FirstOrDefault();
        //need to test b/c the model is int? which is nullable

        if (parentLocationInstance == null)
        {
            var locationType = await _temporaryLocationInstancesProc.ReadOneWithIgnoreQueryFilters( new List<Expression<Func<TemporaryLocationInstance, bool>>> { l => l.LocationInstanceID == requestLocationInstances[0].LocationInstance.LocationInstanceID }).Select(li => li.LocationType).FirstOrDefaultAsync();

            ReceivedModalSublocationsViewModel receivedModalSublocationsViewModel = new ReceivedModalSublocationsViewModel()
            {
                locationInstancesDepthZero = new List<LocationInstance>(),
                locationTypeNames = new List<string>(),
                locationInstancesSelected = await _locationInstancesProc.ReadWithIgnoreQueryFilters( new List<Expression<Func<LocationInstance, bool>>> { l => l.LocationInstanceID == requestLocationInstances[0].LocationInstance.LocationInstanceID }).ToListAsync()

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
                locationType = await _locationTypesProc.ReadOneAsync( new List<Expression<Func<LocationType, bool>>> { l => l.LocationTypeID == locationType.LocationTypeParentID });
            }

            receivedLocationViewModel.locationInstancesSelected.Add(parentLocationInstance);
            requestItemViewModel.ParentDepthZeroOfSelected = locationType;
            requestItemViewModel.ReceivedLocationViewModel = receivedLocationViewModel;

            ReceivedModalSublocationsViewModel receivedModalSublocationsViewModel = new ReceivedModalSublocationsViewModel()
            {
                locationInstancesDepthZero = _locationInstancesProc.Read( new List<Expression<Func<LocationInstance, bool>>> { li => li.LocationTypeID == locationType.LocationTypeID && !(li is TemporaryLocationInstance)}).AsEnumerable(),
                locationTypeNames = new List<string>(),
                locationInstancesSelected = new List<LocationInstance>()
            };
            bool finished = false;
            int locationTypeIDLoop = locationType.LocationTypeID;
            var parent = parentLocationInstance;
            receivedModalSublocationsViewModel.locationInstancesSelected.Add(parent);
            requestItemViewModel.ChildrenLocationInstances = new List<List<LocationInstance>>();
            requestItemViewModel.ChildrenLocationInstances.Add(_locationInstancesProc.Read( new List<Expression<Func<LocationInstance, bool>>> { l => l.LocationInstanceParentID == parent.LocationInstanceParentID }).OrderBy(l => l.LocationNumber).ToList());

            while (parent.LocationInstanceParentID != null)
            {
                parent = await _locationInstancesProc.ReadOneAsync(new List<Expression<Func<LocationInstance, bool>>> { li => li.LocationInstanceID == parent.LocationInstanceParentID });
                requestItemViewModel.ChildrenLocationInstances.Add(_locationInstancesProc.Read(new List<Expression<Func<LocationInstance, bool>>> { l => l.LocationInstanceParentID == parent.LocationInstanceParentID }).OrderBy(l => l.LocationNumber).ToList());
                receivedModalSublocationsViewModel.locationInstancesSelected.Add(parent);
            }
            if (parent.LocationTypeID == 500)
            {

                if (parentLocationInstance.LocationTypeID == 500)
                {
                    receivedModalSublocationsViewModel.locationInstancesSelected.Insert(0, requestLocationInstances[0].LocationInstance);
                    requestItemViewModel.ChildrenLocationInstances = new List<List<LocationInstance>>();
                    requestItemViewModel.ChildrenLocationInstances.Add(_locationInstancesProc.Read(new List<Expression<Func<LocationInstance, bool>>> { l => l.LocationInstanceParentID == parent.LocationInstanceID }).Include(l => l.LabPart).OrderBy(l => l.LocationNumber).ToList());
                }
                receivedModalSublocationsViewModel.locationInstancesSelected.First().LabPart = await _lapPartsProc.ReadOneAsync( new List<Expression<Func<LabPart, bool>>> { lp => lp.LabPartID == receivedModalSublocationsViewModel.locationInstancesSelected.First().LabPartID });
                receivedModalSublocationsViewModel.LabPartTypes = _lapPartsProc.Read().AsEnumerable();
            }
            while (!finished)
            {
                //need to get the whole thing b/c need both the name and the child id so it's instead of looping through the list twice
                var nextType = await _locationTypesProc.ReadOneAsync(new List<Expression<Func<LocationType, bool>>> { lt => lt.LocationTypeID == locationTypeIDLoop });
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
                ParentLocationInstance = await _locationInstancesProc.ReadOneAsync( new List<Expression<Func<LocationInstance, bool>>> { m => m.LocationInstanceID == parentLocationInstance.LocationInstanceID })
            };

            if (receivedModalVisualViewModel.ParentLocationInstance != null)
            {
                receivedModalVisualViewModel.RequestChildrenLocationInstances =
                    _locationInstancesProc.Read(new List<Expression<Func<LocationInstance, bool>>> { m => m.LocationInstanceParentID == parentLocationInstance.LocationInstanceID }, new List<ComplexIncludes<LocationInstance, ModelBase>>{
                     new ComplexIncludes<LocationInstance, ModelBase>{Include = m => m.RequestLocationInstances } })
                    .Select(li => new RequestChildrenLocationInstances()
                    {
                        LocationInstance = li,
                        IsThisRequest = li.RequestLocationInstances.Select(rli => rli.RequestID).Where(i => i == id).Any()
                    }).OrderBy(m => m.LocationInstance.LocationNumber).ToList();

                List<LocationInstancePlace> liPlaces = new List<LocationInstancePlace>();
                var emptyshelf25 = receivedModalVisualViewModel.RequestChildrenLocationInstances.Where(rcli => rcli.IsThisRequest && rcli.LocationInstance.IsEmptyShelf).FirstOrDefault();
                if (parentLocationInstance.LocationTypeID == 500
                    && emptyshelf25 != null)
                {
                    liPlaces.Add(new LocationInstancePlace()
                    {
                        LocationInstanceId = emptyshelf25.LocationInstance.LocationInstanceID,
                        Placed = true
                    });
                    receivedModalVisualViewModel.RequestChildrenLocationInstances= new List<RequestChildrenLocationInstances>() { emptyshelf25 };
                }
                else
                {
                    foreach (var cli in receivedModalVisualViewModel.RequestChildrenLocationInstances)
                    {
                        liPlaces.Add(new LocationInstancePlace()
                        {
                            LocationInstanceId = cli.LocationInstance.LocationInstanceID,
                            Placed = cli.IsThisRequest
                        });
                    }
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
return requestItemViewModel;


        }
        public ShareModalViewModel GetShareModalViewModel(int ID, AppUtility.ModelsEnum ModelsEnum)
{
    ShareModalViewModel shareModalViewModel = new ShareModalViewModel()
    {
        ID = ID,
        ModelsEnum = ModelsEnum,
        ApplicationUsers =_employeesProc.Read(new List<Expression<Func<Employee, bool>>> { u => u.Id != _userManager.GetUserId(User) })
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

        if (requestItemViewModel.Requests.FirstOrDefault().ParentRequestID !=null)
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
protected async Task<RequestIndexPartialViewModel> GetIndexViewModel(RequestIndexObject requestIndexObject, List<int> Months = null, List<int> Years = null,
                                                                        SelectedRequestFilters selectedFilters = null, int numFilters = 0, RequestsSearchViewModel? requestsSearchViewModel = null)
{
    int categoryID = 1;
    if (requestIndexObject.SectionType == AppUtility.MenuItems.Operations)
    {
        categoryID = 2;
    }
    IQueryable<Request> RequestsPassedIn = Enumerable.Empty<Request>().AsQueryable().AsNoTracking();

    List<System.Linq.Expressions.Expression<Func<Request, bool>>> wheres = new List<System.Linq.Expressions.Expression<Func<Request, bool>>>();
    List<ComplexIncludes<Request, ModelBase>> includes = new List<ComplexIncludes<Request, ModelBase>>();

    wheres.Add(r => r.Product.ProductName.Contains(selectedFilters == null ? "" : selectedFilters.SearchText));
    wheres.Add(r => r.Product.ProductSubcategory.ParentCategory.CategoryTypeID == categoryID);

    int sideBarID = 0;
    if (requestIndexObject.SidebarType != AppUtility.SidebarEnum.Owner)
    {
        int.TryParse(requestIndexObject.SidebarFilterID, out sideBarID);
    }

    if (requestIndexObject.PageType == AppUtility.PageTypeEnum.RequestRequest || requestIndexObject.PageType == AppUtility.PageTypeEnum.OperationsRequest)
    {

        if (requestIndexObject.RequestStatusID == 1 || requestIndexObject.RequestStatusID == 6)
        {
            wheres.Add(r => r.RequestStatusID == 1|| r.RequestStatusID==6);
        }
        else
        {
            wheres.Add(r => r.RequestStatusID == requestIndexObject.RequestStatusID);
        }

    }
    else if (requestIndexObject.PageType == AppUtility.PageTypeEnum.RequestSummary)
    {
        if (requestIndexObject.RequestStatusID == 7)
        {
            wheres.Add(r => r.RequestStatus.RequestStatusID == 7);
        }
        else
        {
            wheres.Add(r => r.IsInInventory);
        }
    }
    else if (requestIndexObject.PageType == AppUtility.PageTypeEnum.AccountingGeneral)
    {
        //ignore all the wheres got so far HERE it used to take from context so i reinstantiated the wheres list


        ///REMEMBER TO IGNORE QUERY FILTERS
        wheres.Clear();
        //we need both categories
        wheres.Add(r => r.RequestStatusID == 3);
        wheres.Add(r => !r.IsClarify && r.Payments.Where(p => p.IsPaid && p.HasInvoice).Count() == r.Payments.Count());
        wheres.Add(r => Years.Contains(r.ParentRequest.OrderDate.Year));
        if (Months != null && Months.Count() > 0)
        {
            wheres.Add(r => Months.Contains(r.ParentRequest.OrderDate.Month));
        }
    }
    else if (requestIndexObject.PageType == AppUtility.PageTypeEnum.RequestCart && requestIndexObject.SidebarType == AppUtility.SidebarEnum.Favorites)
    {
                var usersFavoriteRequests = _favoriteRequestsProc.Read(new List<Expression<Func<FavoriteRequest, bool>>> { fr => fr.ApplicationUserID == _userManager.GetUserId(User) })
            .Select(fr => fr.RequestID).ToList();
        wheres.Add(frl => usersFavoriteRequests.Contains(frl.RequestID));
    }
    else if (requestIndexObject.PageType == AppUtility.PageTypeEnum.RequestCart && requestIndexObject.SidebarType == AppUtility.SidebarEnum.SharedRequests)
    {
        var sharedWithMe = _shareRequestsProc.Read( new List<Expression<Func<ShareRequest, bool>>> { fr => fr.ToApplicationUserID == _userManager.GetUserId(User) })
            .Select(sr => sr.RequestID).ToList();
        wheres.Add(frl => sharedWithMe.Contains(frl.RequestID));
    }
    else
    {
        //do not think it is ever supposed to get here
        //wheres.Add(r => r.RequestStatus.RequestStatusID == 3);              
        //wheres.Add(r => r.IsInInventory);
    }
    AppUtility.SidebarEnum SidebarTitle = AppUtility.SidebarEnum.List;
    //now that the lists are created sort by vendor or subcategory
    var sidebarFilterDescription = "";
    switch (requestIndexObject.SidebarType)
    {
        case AppUtility.SidebarEnum.Vendors:
            sidebarFilterDescription = await _vendorsProc.ReadOne( new List<Expression<Func<Vendor, bool>>> { v => v.VendorID == sideBarID }).Select(v => v.VendorEnName).FirstOrDefaultAsync();
            wheres.Add(r => r.Product.VendorID == sideBarID);
            break;
        case AppUtility.SidebarEnum.Type:
            sidebarFilterDescription = await _productSubcategoriesProc.ReadOne( new List<Expression<Func<ProductSubcategory, bool>>> { p => p.ProductSubcategoryID == sideBarID }).Select(p => p.ProductSubcategoryDescription).FirstOrDefaultAsync();
            wheres.Add(r => r.Product.ProductSubcategoryID == sideBarID);
            break;
        case AppUtility.SidebarEnum.Owner:
            var owner = await _employeesProc.ReadOneAsync( new List<Expression<Func<Employee, bool>>> { e => e.Id.Equals(requestIndexObject.SidebarFilterID) });
            sidebarFilterDescription = owner.FirstName + " " + owner.LastName;
            wheres.Add(r => r.ApplicationUserCreatorID == requestIndexObject.SidebarFilterID);
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
    requestIndexViewModel.SideBarType = requestIndexObject.SidebarType;
    requestIndexViewModel.ErrorMessage = requestIndexObject.ErrorMessage;
    var onePageOfProducts = Enumerable.Empty<RequestIndexPartialRowViewModel>().ToPagedList();

    includes.Add(new ComplexIncludes<Request, ModelBase> { Include= r => r.Product });
    includes.Add(new ComplexIncludes<Request, ModelBase> { Include= r => r.Product.ProductSubcategory });
    includes.Add(new ComplexIncludes<Request, ModelBase> { Include= r => r.Product.ProductSubcategory.ParentCategory });
    includes.Add(new ComplexIncludes<Request, ModelBase> { Include= r => r.ParentRequest });
    includes.Add(new ComplexIncludes<Request, ModelBase> { Include= r => r.ApplicationUserCreator });
    includes.Add(new ComplexIncludes<Request, ModelBase> { Include= r => r.Product.Vendor });
    includes.Add(new ComplexIncludes<Request, ModelBase> { Include= r => r.RequestStatus });
    includes.Add(new ComplexIncludes<Request, ModelBase> { Include= r => r.Product.UnitType });
    includes.Add(new ComplexIncludes<Request, ModelBase> { Include= r => r.Product.SubUnitType });
    includes.Add(new ComplexIncludes<Request, ModelBase> { Include= r => r.Product.SubSubUnitType });
    includes.Add(new ComplexIncludes<Request, ModelBase>
    {
        Include= r => r.RequestLocationInstances,
        ThenInclude = new ComplexIncludes<ModelBase, ModelBase>
        {
            Include = rli => ((RequestLocationInstance)rli).LocationInstance,
            ThenInclude = new ComplexIncludes<ModelBase, ModelBase> { Include = li => ((LocationInstance)li).LocationInstanceParent }
        }
    });

    FilterListBySelectFilters(selectedFilters, wheres);


    //if ((requestsSearchViewModel.PageType != AppUtility.PageTypeEnum.AccountingGeneral && 
    //    requestsSearchViewModel.SidebarEnum != AppUtility.SidebarEnum.Search)
    //    && (Request.Method == "GET" && !AppUtility.IsAjaxRequest(Request)))
    if (requestsSearchViewModel == null)
    {
        requestsSearchViewModel = new RequestsSearchViewModel();
    }
    if (requestsSearchViewModel.Payment == null)
    {
        requestsSearchViewModel.Payment = new Payment();
    }

    ApplySearchToRequestList(requestsSearchViewModel, wheres);
    var RequestPassedInWithInclude = _requestsProc.ReadWithIgnoreQueryFilters(wheres, includes);

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
protected static void FilterListBySelectFilters(SelectedRequestFilters selectedFilters, List<Expression<Func<Request, bool>>> wheres)
{
    if (selectedFilters != null)
    {
        if (selectedFilters.SelectedCategoriesIDs.Count() > 0)
        {
            wheres.Add(r => selectedFilters.SelectedCategoriesIDs.Contains(r.Product.ProductSubcategory.ParentCategoryID));
        }
        if (selectedFilters.SelectedSubcategoriesIDs.Count() > 0)
        {
            wheres.Add(r => selectedFilters.SelectedSubcategoriesIDs.Contains(r.Product.ProductSubcategoryID));
        }
        if (selectedFilters.SelectedVendorsIDs.Count() > 0)
        {
            wheres.Add(r => selectedFilters.SelectedVendorsIDs.Contains(r.Product.VendorID ?? 0));
        }
        if (selectedFilters.SelectedLocationsIDs.Count() > 0)
        {
            wheres.Add(r => selectedFilters.SelectedLocationsIDs.Contains((int)(Math.Floor(r.RequestLocationInstances.FirstOrDefault().LocationInstance.LocationTypeID / 100.0) * 100)));
        }
        if (selectedFilters.SelectedOwnersIDs.Count() > 0)
        {
            wheres.Add(r => selectedFilters.SelectedOwnersIDs.Contains(r.ApplicationUserCreatorID));
        }
        if (selectedFilters.CatalogNumber != null && selectedFilters.CatalogNumber != "")
        {
            wheres.Add(r => r.Product.CatalogNumber.ToUpper().Contains(selectedFilters.CatalogNumber.ToUpper()));
        }

    }
    if (selectedFilters?.Archived == true)
    {
        wheres.Add(r => r.IsArchived == true);
    }
    else
    {
        wheres.Add(r => r.IsArchived == false);
    }
}
protected static void ApplySearchToRequestList(RequestsSearchViewModel requestsSearchViewModel, List<Expression<Func<Request, bool>>> wheres)
{
    if (requestsSearchViewModel != null)
    {
        wheres.Add(r =>
            (requestsSearchViewModel.ParentCategoryID == null || r.Product.ProductSubcategory.ParentCategoryID == requestsSearchViewModel.ParentCategoryID)
            && (requestsSearchViewModel.ProductSubcategoryID == null || r.Product.ProductSubcategory.ProductSubcategoryID == requestsSearchViewModel.ProductSubcategoryID)
            && (requestsSearchViewModel.VendorID == null || r.Product.VendorID == requestsSearchViewModel.VendorID)
            && (String.IsNullOrEmpty(requestsSearchViewModel.ItemName) || r.Product.ProductName.ToLower().Contains(requestsSearchViewModel.ItemName.ToLower()))
            && (String.IsNullOrEmpty(requestsSearchViewModel.ProductHebrewName) || r.Product.ProductHebrewName.ToLower().Contains(requestsSearchViewModel.ProductHebrewName.ToLower()))
            && (requestsSearchViewModel.InvoiceDate == null || r.Payments.Where(p => p.Invoice.InvoiceDate.Date == requestsSearchViewModel.InvoiceDate.GetJustDateOnNullableDateTime()).Any())
            && (requestsSearchViewModel.Batch == null || r.Batch == requestsSearchViewModel.Batch)
            && (requestsSearchViewModel.ExpirationDate == null || r.BatchExpiration.Equals(requestsSearchViewModel.ExpirationDate.GetJustDateOnNullableDateTime()))
            && (requestsSearchViewModel.CreationDate == null || r.CreationDate.Date.Equals(requestsSearchViewModel.CreationDate.GetJustDateOnNullableDateTime()))
            && (requestsSearchViewModel.ArrivalDate == null || r.ArrivalDate.Date.Equals(requestsSearchViewModel.ArrivalDate.GetJustDateOnNullableDateTime()))
            && (requestsSearchViewModel.ApplicationUserReceiverID == null || r.ApplicationUserReceiverID == requestsSearchViewModel.ApplicationUserReceiverID)
            && (requestsSearchViewModel.ApplicationUserOwnerID == null || r.ApplicationUserCreatorID == requestsSearchViewModel.ApplicationUserOwnerID)
            && (String.IsNullOrEmpty(requestsSearchViewModel.QuoteNumber) || r.ParentQuote.QuoteNumber.ToLower().Contains(requestsSearchViewModel.QuoteNumber.ToLower()))
            && (String.IsNullOrEmpty(requestsSearchViewModel.CatalogNumber) || r.Product.CatalogNumber.ToLower().Contains(requestsSearchViewModel.CatalogNumber.ToLower()))
            && (String.IsNullOrEmpty(requestsSearchViewModel.SerialNumber) || r.Product.SerialNumber.ToLower().Contains(requestsSearchViewModel.SerialNumber.ToLower()))
            && (requestsSearchViewModel.OrderDate == null || r.ParentRequest.OrderDate.Date.Equals(requestsSearchViewModel.OrderDate.GetJustDateOnNullableDateTime()))
            && (requestsSearchViewModel.OrderNumber == null || r.ParentRequest.OrderNumber == requestsSearchViewModel.OrderNumber)
            && (String.IsNullOrEmpty(requestsSearchViewModel.Currency) || r.Currency.Equals(requestsSearchViewModel.Currency))
            && (String.IsNullOrEmpty(requestsSearchViewModel.SupplierOrderNumber) || r.ParentRequest.SupplierOrderNumber.ToLower().Contains(requestsSearchViewModel.SupplierOrderNumber.ToLower()))
            && (String.IsNullOrEmpty(requestsSearchViewModel.InvoiceNumber) || r.Payments.Where(p => p.Invoice.InvoiceNumber.ToLower() == requestsSearchViewModel.InvoiceNumber.ToLower()).Any())
            && (requestsSearchViewModel.Payment.CompanyAccountID == null || r.Payments.Where(p => p.CompanyAccountID.Equals(requestsSearchViewModel.Payment.CompanyAccountID)).Any())
            && (requestsSearchViewModel.Payment.PaymentTypeID == null || r.Payments.Where(p => p.PaymentTypeID.Equals(requestsSearchViewModel.Payment.PaymentTypeID)).Any())
            && (requestsSearchViewModel.Payment.CreditCardID == null || r.Payments.Where(p => p.CreditCardID.Equals(requestsSearchViewModel.Payment.CreditCardID)).Any())
            && (requestsSearchViewModel.Payment.PaymentReferenceDate == new DateTime() || r.Payments.Where(p => p.PaymentReferenceDate.Equals(requestsSearchViewModel.Payment.PaymentReferenceDate.Date)).Any())
            && (String.IsNullOrEmpty(requestsSearchViewModel.Payment.CheckNumber) || r.Payments.Where(p => p.CheckNumber.ToLower().Contains(requestsSearchViewModel.Payment.CheckNumber.ToLower())).Any())
            && (String.IsNullOrEmpty(requestsSearchViewModel.Payment.Reference) || r.Payments.Where(p => p.Reference.ToLower().Contains(requestsSearchViewModel.Payment.Reference.ToLower())).Any())
            );

    }
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
    var popoverAddToList = new IconPopoverViewModel("icon-centarix-icons-04", "black", AppUtility.PopoverDescription.AddToList, "MoveToListModal", "Requests", AppUtility.PopoverEnum.None, "move-to-list");
    var popoverMoveList = new IconPopoverViewModel("icon-entry-24px", "black", AppUtility.PopoverDescription.MoveToList, "MoveToListModal", "Requests", AppUtility.PopoverEnum.None, "move-to-list");
    var popoverDeleteFromList = new IconPopoverViewModel("icon-delete-24px", "black", AppUtility.PopoverDescription.DeleteFromList, "DeleteFromListModal", "Requests", AppUtility.PopoverEnum.None, "remove-from-list");

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
                    iconList.Add(reorderIcon);
                    iconList.Add(favoriteIcon);
                    popoverMoreIcon.IconPopovers = new List<IconPopoverViewModel>() { popoverShare, popoverAddToList /*, popoverReorder*//*, popoverDelete*/ };
                    iconList.Add(popoverMoreIcon);
                    onePageOfProducts = await RequestPassedInWithInclude.OrderByDescending(r => r.ArrivalDate).ThenBy(r => r.Product.ProductName).Select(r =>
                    new RequestIndexPartialRowViewModel(AppUtility.IndexTableTypes.ReceivedInventory,
                     r, r.Product, r.Product.Vendor, r.Product.ProductSubcategory, r.Product.ProductSubcategory.ParentCategory,
                                  r.Product.UnitType, r.Product.SubUnitType, r.Product.SubSubUnitType, requestIndexObject, iconList, defaultImage, _favoriteRequestsProc.ReadOne( new List<Expression<Func<FavoriteRequest, bool>>> { fr => fr.RequestID == r.RequestID, fr => fr.ApplicationUserID == user.Id }, null).FirstOrDefault(),
                                    _shareRequestsProc.ReadOne(new List<Expression<Func<ShareRequest, bool>>> { sr => sr.RequestID == r.RequestID, sr => sr.ToApplicationUserID == user.Id }, new List<ComplexIncludes<ShareRequest, ModelBase>> {
                                     new ComplexIncludes<ShareRequest, ModelBase>{Include = sr => sr.FromApplicationUser }}).FirstOrDefault(), user,
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
                    onePageOfProducts = await RequestPassedInWithInclude.OrderByDescending(r => r.ParentRequest.OrderDate).Select(r => new RequestIndexPartialRowViewModel(AppUtility.IndexTableTypes.OrderedOperations,
                    r, r.Product, r.Product.Vendor, r.Product.ProductSubcategory, r.Product.ProductSubcategory.ParentCategory,
                                 r.Product.UnitType, r.Product.SubUnitType, r.Product.SubSubUnitType, requestIndexObject, iconList, defaultImage, r.ParentRequest, user)
                               ).ToPagedListAsync(requestIndexObject.PageNumber == 0 ? 1 : requestIndexObject.PageNumber, 20);

                    break;
                case 3:
                    popoverMoreIcon.IconPopovers = new List<IconPopoverViewModel>();
                    popoverMoreIcon.IconPopovers.Add(popoverDelete);
                    iconList.Add(popoverMoreIcon);
                    onePageOfProducts = await RequestPassedInWithInclude.OrderByDescending(r => r.ParentRequest.OrderDate).Select(r => new RequestIndexPartialRowViewModel(AppUtility.IndexTableTypes.ReceivedInventoryOperations,
                     r, r.Product, r.Product.Vendor, r.Product.ProductSubcategory, r.Product.ProductSubcategory.ParentCategory,
                                  r.Product.UnitType, r.Product.SubUnitType, r.Product.SubSubUnitType, requestIndexObject, iconList, defaultImage, r.ParentRequest, user)
                                ).ToPagedListAsync(requestIndexObject.PageNumber == 0 ? 1 : requestIndexObject.PageNumber, 20);
                    break;
            }
            break;
        case AppUtility.PageTypeEnum.RequestInventory:
            popoverMoreIcon.IconPopovers = new List<IconPopoverViewModel>() { popoverShare, /*popoverReorder,*/ popoverDelete };
            iconList.Add(popoverMoreIcon);
            onePageOfProducts = await RequestPassedInWithInclude.OrderByDescending(r => r.ArrivalDate).Select(r => new RequestIndexPartialRowViewModel(AppUtility.IndexTableTypes.ReceivedInventory,
                     r, r.Product, r.Product.Vendor, r.Product.ProductSubcategory, r.Product.ProductSubcategory.ParentCategory,
                     r.Product.UnitType, r.Product.SubUnitType, r.Product.SubSubUnitType, requestIndexObject, iconList, defaultImage, _favoriteRequestsProc.ReadOne(new List<Expression<Func<FavoriteRequest, bool>>> { fr => fr.RequestID == r.RequestID, fr => fr.ApplicationUserID == user.Id }, null).FirstOrDefault(),
                                    _shareRequestsProc.ReadOne(new List<Expression<Func<ShareRequest, bool>>> { sr => sr.RequestID == r.RequestID, sr => sr.ToApplicationUserID == user.Id }, new List<ComplexIncludes<ShareRequest, ModelBase>> {
                                     new ComplexIncludes<ShareRequest, ModelBase>{Include = sr => sr.FromApplicationUser }}).FirstOrDefault(), user,
                 r.RequestLocationInstances.FirstOrDefault().LocationInstance, r.RequestLocationInstances.FirstOrDefault().LocationInstance.LocationInstanceParent, r.ParentRequest)).ToPagedListAsync(requestIndexObject.PageNumber == 0 ? 1 : requestIndexObject.PageNumber, 20);
            break;
        case AppUtility.PageTypeEnum.RequestSummary:


            switch (requestIndexObject.RequestStatusID)
            {
                case 7:
                    popoverMoreIcon.IconPopovers = new List<IconPopoverViewModel>() { /*popoverShare, popoverReorder,*/ popoverDelete };
                    iconList.Add(favoriteIcon);
                    iconList.Add(popoverMoreIcon);
                    onePageOfProducts = await RequestPassedInWithInclude.OrderByDescending(r => r.CreationDate).Select(r => new RequestIndexPartialRowViewModel(AppUtility.IndexTableTypes.SummaryProprietary,
                    r, r.Product, r.Product.Vendor, r.Product.ProductSubcategory, r.Product.ProductSubcategory.ParentCategory,
                                 r.Product.UnitType, r.Product.SubUnitType, r.Product.SubSubUnitType, requestIndexObject, iconList, defaultImage, _favoriteRequestsProc.ReadOne(new List<Expression<Func<FavoriteRequest, bool>>> { fr => fr.RequestID == r.RequestID, fr => fr.ApplicationUserID == user.Id }, null).FirstOrDefault(),
                                    _shareRequestsProc.ReadOne(new List<Expression<Func<ShareRequest, bool>>> { sr => sr.RequestID == r.RequestID, sr => sr.ToApplicationUserID == user.Id }, new List<ComplexIncludes<ShareRequest, ModelBase>> {
                                     new ComplexIncludes<ShareRequest, ModelBase>{Include = sr => sr.FromApplicationUser }}).FirstOrDefault(), user,
                                  r.RequestLocationInstances.FirstOrDefault().LocationInstance, r.RequestLocationInstances.FirstOrDefault().LocationInstance.LocationInstanceParent, r.ParentRequest)
                               ).ToPagedListAsync(requestIndexObject.PageNumber == 0 ? 1 : requestIndexObject.PageNumber, 20);

                    break;
                default:
                    iconList.Add(reorderIcon);
                    popoverMoreIcon.IconPopovers = new List<IconPopoverViewModel>() { popoverShare, popoverAddToList /*, popoverReorder,*/ /*popoverDelete*/ };
                    iconList.Add(favoriteIcon);
                    iconList.Add(popoverMoreIcon);
                    onePageOfProducts = await RequestPassedInWithInclude.OrderByDescending(r => r.ParentRequest.OrderDate).ThenBy(r => r.Product.ProductName).Select(r => new RequestIndexPartialRowViewModel(AppUtility.IndexTableTypes.Summary,
                     r, r.Product, r.Product.Vendor, r.Product.ProductSubcategory, r.Product.ProductSubcategory.ParentCategory,
                            r.Product.UnitType, r.Product.SubUnitType, r.Product.SubSubUnitType, requestIndexObject, iconList, defaultImage,  _favoriteRequestsProc.ReadOne(new List<Expression<Func<FavoriteRequest, bool>>> { fr => fr.RequestID == r.RequestID, fr => fr.ApplicationUserID == user.Id }, null).FirstOrDefault(),
                                    _shareRequestsProc.ReadOne(new List<Expression<Func<ShareRequest, bool>>> { sr => sr.RequestID == r.RequestID, sr => sr.ToApplicationUserID == user.Id }, new List<ComplexIncludes<ShareRequest, ModelBase>> {
                                     new ComplexIncludes<ShareRequest, ModelBase>{Include = sr => sr.FromApplicationUser }}).FirstOrDefault(), user, r.RequestLocationInstances.FirstOrDefault().LocationInstance, r.RequestLocationInstances.FirstOrDefault().LocationInstance.LocationInstanceParent, r.ParentRequest)
                   )
                        //.ToLookup(r => r.r.ProductID).Select(e => e.First())
                        .ToPagedListAsync(requestIndexObject.PageNumber == 0 ? 1 : requestIndexObject.PageNumber, 20);
                    /// .GroupBy(r => r.ProductID, (key, value) => value.OrderByDescending(v => v.ParentRequest.OrderDate).First()).AsQueryable();
                    break;
            }

            break;
        case AppUtility.PageTypeEnum.OperationsInventory:
            iconList.Add(orderOperations);
            popoverMoreIcon.IconPopovers = new List<IconPopoverViewModel>();
            popoverMoreIcon.IconPopovers.Add(popoverDelete);
            iconList.Add(popoverMoreIcon);
            onePageOfProducts = await RequestPassedInWithInclude.OrderByDescending(r => r.ParentRequest.OrderDate).Select(r => new RequestIndexPartialRowViewModel(AppUtility.IndexTableTypes.ReceivedInventoryOperations,
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
                                  r.Product.UnitType, r.Product.SubUnitType, r.Product.SubSubUnitType, requestIndexObject, iconList, defaultImage, _favoriteRequestsProc.ReadOne(new List<Expression<Func<FavoriteRequest, bool>>> { fr => fr.RequestID == r.RequestID, fr => fr.ApplicationUserID == user.Id }, null).FirstOrDefault(),
                                user, r.ParentRequest)).ToPagedListAsync(requestIndexObject.PageNumber == 0 ? 1 : requestIndexObject.PageNumber, 20);
            break;
        case AppUtility.PageTypeEnum.ExpensesStatistics:
            break;
        case AppUtility.PageTypeEnum.AccountingGeneral:
            onePageOfProducts = await RequestPassedInWithInclude.OrderByDescending(r => r.ParentRequest.OrderDate).Select(r => new RequestIndexPartialRowViewModel(AppUtility.IndexTableTypes.AccountingGeneral,
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
                                 r.Product.UnitType, r.Product.SubUnitType, r.Product.SubSubUnitType, requestIndexObject, iconList, defaultImage, _favoriteRequestsProc.ReadOne(new List<Expression<Func<FavoriteRequest, bool>>> { fr => fr.RequestID == r.RequestID, fr => fr.ApplicationUserID == user.Id }, null).FirstOrDefault(),
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
                              _favoriteRequestsProc.ReadOne(new List<Expression<Func<FavoriteRequest, bool>>> { fr => fr.RequestID == r.RequestID, fr => fr.ApplicationUserID == user.Id }, null).FirstOrDefault(),
                                    _shareRequestsProc.ReadOne(new List<Expression<Func<ShareRequest, bool>>> { sr => sr.RequestID == r.RequestID, sr => sr.ToApplicationUserID == user.Id }, new List<ComplexIncludes<ShareRequest, ModelBase>> {
                                     new ComplexIncludes<ShareRequest, ModelBase>{Include = sr => sr.FromApplicationUser }}).FirstOrDefault(), user,
                                  r.RequestLocationInstances.FirstOrDefault().LocationInstance, r.RequestLocationInstances.FirstOrDefault().LocationInstance.LocationInstanceParent, r.ParentRequest)).ToPagedListAsync(requestIndexObject.PageNumber == 0 ? 1 : requestIndexObject.PageNumber, 20);
                    break;
                case AppUtility.SidebarEnum.MyLists:
                case AppUtility.SidebarEnum.SharedLists:
                    iconList.Add(reorderIcon);
                    iconList.Add(favoriteIcon);
                    popoverMoreIcon.IconPopovers = new List<IconPopoverViewModel>() { popoverMoveList, popoverShare, popoverDeleteFromList };
                    iconList.Add(popoverMoreIcon);
                    onePageOfProducts = await _requestListRequestsProc.Read( new List<Expression<Func<RequestListRequest, bool>>> { rlr => rlr.ListID == requestIndexObject.ListID }).OrderByDescending(rlr => rlr.TimeStamp)
                        .Select(rlr => rlr.Request).Select(r =>
                    new RequestIndexPartialRowViewModel(AppUtility.IndexTableTypes.RequestLists,
                     r, r.Product, r.Product.Vendor, r.Product.ProductSubcategory, r.Product.ProductSubcategory.ParentCategory,
                                  r.Product.UnitType, r.Product.SubUnitType, r.Product.SubSubUnitType, requestIndexObject, iconList, defaultImage,
                                 _favoriteRequestsProc.ReadOne(new List<Expression<Func<FavoriteRequest, bool>>> { fr => fr.RequestID == r.RequestID, fr => fr.ApplicationUserID == user.Id }, null).FirstOrDefault(),
                                    _shareRequestsProc.ReadOne(new List<Expression<Func<ShareRequest, bool>>> { sr => sr.RequestID == r.RequestID, sr => sr.ToApplicationUserID == user.Id }, new List<ComplexIncludes<ShareRequest, ModelBase>> {
                                     new ComplexIncludes<ShareRequest, ModelBase>{Include = sr => sr.FromApplicationUser }}).FirstOrDefault(), user,
                                  r.RequestLocationInstances.FirstOrDefault().LocationInstance, r.RequestLocationInstances.FirstOrDefault().LocationInstance.LocationInstanceParent, r.ParentRequest,
                                  _shareRequestListsProc.Read( new List<Expression<Func<ShareRequestList, bool>>> { srl => srl.RequestListID == requestIndexObject.ListID && srl.ToApplicationUserID == user.Id }, null).FirstOrDefault().ViewOnly
                                  )).ToPagedListAsync(requestIndexObject.PageNumber == 0 ? 1 : requestIndexObject.PageNumber, 20);
                    break;
            }
            break;
    }

    return onePageOfProducts;
}


protected InventoryFilterViewModel GetInventoryFilterViewModel(SelectedRequestFilters selectedFilters = null, int numFilters = 0, AppUtility.MenuItems sectionType = AppUtility.MenuItems.Requests, bool isProprietary = false)
{
    int categoryType = sectionType == AppUtility.MenuItems.Requests ? 1 : 2;
    if (selectedFilters != null)
    {
        InventoryFilterViewModel inventoryFilterViewModel = new InventoryFilterViewModel()
        {
            Owners = _employeesProc.Read( new List<Expression<Func<Employee, bool>>> { o => !selectedFilters.SelectedOwnersIDs.Contains(o.Id) }).ToList(),
            Locations = _locationTypesProc.Read( new List<Expression<Func<LocationType, bool>>> { l => l.Depth == 0, l => !selectedFilters.SelectedLocationsIDs.Contains(l.LocationTypeID) }).ToList(),
            Categories = _parentCategoriesProc.Read( new List<Expression<Func<ParentCategory, bool>>> { c => c.CategoryTypeID == categoryType && c.IsProprietary == isProprietary, c => !selectedFilters.SelectedCategoriesIDs.Contains(c.ParentCategoryID) }).ToList(),
            Subcategories = _productSubcategoriesProc.Read( new List<Expression<Func<ProductSubcategory, bool>>> { sc => sc.ParentCategory.CategoryTypeID == categoryType && sc.ParentCategory.IsProprietary == isProprietary, v => !selectedFilters.SelectedSubcategoriesIDs.Contains(v.ProductSubcategoryID) }).Distinct().ToList(),
            Vendors = _vendorsProc.Read( new List<Expression<Func<Vendor, bool>>> { v => v.VendorCategoryTypes.Select(vc => vc.CategoryTypeID).Contains(categoryType), v => !selectedFilters.SelectedVendorsIDs.Contains(v.VendorID) }).ToList(),
            SelectedVendors = _vendorsProc.Read( new List<Expression<Func<Vendor, bool>>> { v => selectedFilters.SelectedVendorsIDs.Contains(v.VendorID) }).ToList(),
            SelectedOwners = _employeesProc.Read( new List<Expression<Func<Employee, bool>>> { o => selectedFilters.SelectedOwnersIDs.Contains(o.Id) }).ToList(),
            SelectedLocations = _locationTypesProc.Read( new List<Expression<Func<LocationType, bool>>> { l => l.Depth == 0, l => selectedFilters.SelectedLocationsIDs.Contains(l.LocationTypeID) }).ToList(),
            SelectedCategories = _parentCategoriesProc.Read( new List<Expression<Func<ParentCategory, bool>>> { c => selectedFilters.SelectedCategoriesIDs.Contains(c.ParentCategoryID) }).ToList(),
            SelectedSubcategories = _productSubcategoriesProc.Read( new List<Expression<Func<ProductSubcategory, bool>>> { v => selectedFilters.SelectedSubcategoriesIDs.Contains(v.ProductSubcategoryID) }).Distinct().ToList(),
            NumFilters = numFilters,
            SectionType = sectionType,
            Archive = selectedFilters.Archived,
            IsProprietary = isProprietary,
            CatalogNumber = selectedFilters.CatalogNumber
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
            Vendors = _vendorsProc.Read( new List<Expression<Func<Vendor, bool>>> { v => v.VendorCategoryTypes.Select(vc => vc.CategoryTypeID).Contains(categoryType) }).ToList(),
            Owners = _employeesProc.Read().ToList(),
            Locations = _locationTypesProc.Read(new List<Expression<Func<LocationType, bool>>> { r => r.Depth == 0 }).ToList(),
            Categories = _parentCategoriesProc.Read( new List<Expression<Func<ParentCategory, bool>>> { c => c.CategoryTypeID == categoryType && c.IsProprietary == isProprietary }).ToList(),
            Subcategories = _productSubcategoriesProc.Read( new List<Expression<Func<ProductSubcategory, bool>>> { sc => sc.ParentCategory.CategoryTypeID == categoryType && sc.ParentCategory.IsProprietary == isProprietary }).Distinct().ToList(),
            SelectedVendors = new List<Vendor>(),
            SelectedOwners = new List<Employee>(),
            SelectedLocations = new List<LocationType>(),
            SelectedCategories = new List<ParentCategory>(),
            SelectedSubcategories = new List<ProductSubcategory>(),
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
    MoveDocumentsOutOfTempFolder(id, parentFolderName, additionalRequests, guid.ToString());
}


protected void MoveDocumentsOutOfTempFolder(int id, AppUtility.ParentFolderName parentFolderName, int oldID, bool additionalRequests = false)
{
    MoveDocumentsOutOfTempFolder(id, parentFolderName, additionalRequests, oldID.ToString());
}


private void MoveDocumentsOutOfTempFolder(int id, AppUtility.ParentFolderName parentFolderName, bool additionalRequests, string oldID)
{
    //rename temp folder to the request id
    string uploadFolder = Path.Combine(_hostingEnvironment.WebRootPath, parentFolderName.ToString());
    var TempFolderName = oldID.ToString();
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
        else if (requestFolderFrom != requestFolderTo)
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
    var unittypes = _unitTypesProc.Read(includes: new List<ComplexIncludes<UnitType, ModelBase>>{ new ComplexIncludes<UnitType, ModelBase> { Include = u => u.UnitParentType } }).OrderBy(u => u.UnitParentType.UnitParentTypeID).ThenBy(u => u.UnitTypeDescription);
    if (productSubcategory != null)
    {
        if (categoryTypeId == 1)
        {
            parentcategories = await _parentCategoriesProc.Read( new List<Expression<Func<ParentCategory, bool>>> { pc => pc.ParentCategoryID == productSubcategory.ParentCategoryID }).ToListAsync();
        }
        else
        {
            parentcategories = await _parentCategoriesProc.Read( new List<Expression<Func<ParentCategory, bool>>> { pc => pc.CategoryTypeID == 2 }).ToListAsync();
        }
        productsubcategories = await _productSubcategoriesProc.Read( new List<Expression<Func<ProductSubcategory, bool>>> { ps => ps.ParentCategoryID == productSubcategory.ParentCategoryID }).ToListAsync();
        unittypes = _unitTypesProc.Read( new List<Expression<Func<UnitType, bool>>> { ut => ut.UnitTypeParentCategory.Where(up => up.ParentCategoryID == productSubcategory.ParentCategoryID).Count() > 0 }, 
            new List<ComplexIncludes<UnitType, ModelBase>>{
            new ComplexIncludes<UnitType, ModelBase> { Include = u => u.UnitParentType } }).OrderBy(u => u.UnitParentType.UnitParentTypeID).ThenBy(u => u.UnitTypeDescription);
    }
    else
    {
        if (requestItemViewModel.IsProprietary)
        {
            var proprietarycategory = await _parentCategoriesProc.Read( new List<Expression<Func<ParentCategory, bool>>> { pc => pc.ParentCategoryDescription == AppUtility.ParentCategoryEnum.Samples.ToString() }).FirstOrDefaultAsync();
            productsubcategories = await _productSubcategoriesProc.Read( new List<Expression<Func<ProductSubcategory, bool>>> { ps => ps.ParentCategoryID == proprietarycategory.ParentCategoryID }).ToListAsync();
        }
        else
        {
            parentcategories = await _parentCategoriesProc.Read( new List<Expression<Func<ParentCategory, bool>>> { pc => pc.CategoryTypeID == categoryTypeId && !pc.IsProprietary }).ToListAsync();
        }
    }
    var vendors = await _vendorsProc.Read( new List<Expression<Func<Vendor, bool>>> { v => v.VendorCategoryTypes.Where(vc => vc.CategoryTypeID == categoryTypeId).Count() > 0 }).ToListAsync();
    var projects = await _projectsProc.Read().ToListAsync();
    var subprojects = await _subProjectsProc.Read().ToListAsync();
    var unittypeslookup = unittypes.ToLookup(u => u.UnitParentType);
    var paymenttypes = await _paymentTypesProc.Read().ToListAsync();
    var companyaccounts = await _companyAccountsProc.Read().ToListAsync();

    requestItemViewModel.ParentCategories = parentcategories;
    requestItemViewModel.ProductSubcategories = productsubcategories;
    requestItemViewModel.Vendors = vendors;
    requestItemViewModel.Projects = projects;
    requestItemViewModel.SubProjects = subprojects;

    requestItemViewModel.UnitTypeList = new SelectList(unittypes, "UnitTypeID", "UnitTypeDescription", null, "UnitParentType.UnitParentTypeDescription");
    requestItemViewModel.UnitTypes = unittypeslookup;
    requestItemViewModel.CommentTypes = _commentType.Read().AsEnumerable();
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
    var serialnumberList = _productsProc.ReadWithIgnoreQueryFilters(new List<Expression<Func<Product, bool>>> { p => p.ProductSubcategory.ParentCategory.CategoryTypeID == categoryType })
        .Select(p => int.Parse(p.SerialNumber.Substring(1))).ToList();

    lastSerialNumberInt = serialnumberList.OrderBy(s => s).LastOrDefault();

    return serialLetter + (lastSerialNumberInt + 1);
}

[HttpGet]
[Authorize(Roles = "Requests")]
protected virtual async Task<IActionResult> _CommentInfoPartialView(int typeID, int index)
{

    CommentsInfoViewModel commentsInfoViewModel = await GetCommentInfoViewModelAsync(typeID, index);
    return PartialView(commentsInfoViewModel);
}

private async Task<CommentsInfoViewModel> GetCommentInfoViewModelAsync(int typeID, int index)
{
    CommentBase comment = new CommentBase();
    comment.ApplicationUser = await _employeesProc.ReadOneAsync(new List<Expression<Func<Employee, bool>>> { e => e.Id ==_userManager.GetUserId(User) });
    comment.ApplicationUserID = comment.ApplicationUser.Id;
    comment.CommentTypeID = typeID;
    comment.CommentType = await _commentType.ReadOneAsync(new List<Expression<Func<CommentType, bool>>> { ct => ct.TypeID ==typeID });
    CommentsInfoViewModel commentsInfoViewModel = new CommentsInfoViewModel { Comment = comment, Index = index };
    return commentsInfoViewModel;
}

protected async Task<CreateSupplierViewModel> GetCreateSupplierViewModel(AppUtility.MenuItems SectionType, int VendorID = 0, int Tab = 0)
{
    CreateSupplierViewModel createSupplierViewModel = new CreateSupplierViewModel()
    {
        SectionType = SectionType,
        CategoryTypes = new List<SelectListItem>(),
        Tab = Tab==0 ? 1 : Tab,
        CommentTypes = _commentType.Read(),
        Countries = new List<SelectListItem>()
    };
    createSupplierViewModel.Countries = await _country.Read().Select(c => new SelectListItem() { Text = c.CountryName, Value = c.CountryID.ToString() }).ToListAsync();

    await _categoryType.Read().ForEachAsync(ct =>
    {
        createSupplierViewModel.CategoryTypes.Add(new SelectListItem() { Text = ct.CategoryTypeDescription, Value = ct.CategoryTypeID.ToString() });
    });
    createSupplierViewModel.CategoryTypes.Insert(0, new SelectListItem() { Text = "Select Category", Disabled = true, Selected = true });
    if (VendorID == 0)
    {
        createSupplierViewModel.VendorContacts = new List<VendorContactWithDeleteViewModel>() { new VendorContactWithDeleteViewModel()
                    {
                        VendorContact = new VendorContact(),
                        Delete = false
                    }
                };
    }
    else
    {
        createSupplierViewModel.Vendor = await _vendorsProc.ReadOneAsync(
           new List<Expression<Func<Vendor, bool>>> { v => v.VendorID == VendorID },
           new List<ComplexIncludes<Vendor, ModelBase>> {
                            new ComplexIncludes<Vendor, ModelBase> {Include = v => v.VendorCategoryTypes },
                            new ComplexIncludes<Vendor, ModelBase> {Include = v => v.VendorComments, ThenInclude  = new ComplexIncludes<ModelBase, ModelBase>{ Include = c=>((VendorComment)c).CommentType } },
                            new ComplexIncludes<Vendor, ModelBase> {Include = v => v.VendorComments, ThenInclude  = new ComplexIncludes<ModelBase, ModelBase>{ Include = c=>((VendorComment)c).ApplicationUser } },
                            new ComplexIncludes<Vendor, ModelBase> {Include = v => v.VendorContacts}
           });
        createSupplierViewModel.Comments = createSupplierViewModel.Vendor.VendorComments;
        createSupplierViewModel.VendorContacts = createSupplierViewModel.Vendor.VendorContacts.Select(c => new VendorContactWithDeleteViewModel()
        {
            VendorContact = c,
            Delete = false
        }).ToList();

        createSupplierViewModel.VendorCategoryTypes = createSupplierViewModel.Vendor.VendorCategoryTypes.Select(vc => vc.CategoryTypeID).ToList();
    }
    return createSupplierViewModel;
}

    }
}
