using Abp.Extensions;
using Castle.DynamicProxy.Generators.Emitters.SimpleAST;
using JetBrains.Annotations;
using Microsoft.ApplicationInsights.AspNetCore;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.AspNetCore.Mvc.ViewEngines;
using Microsoft.CodeAnalysis.Differencing;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Internal;
using Microsoft.Net.Http.Headers;
using PrototypeWithAuth.AppData;
using PrototypeWithAuth.AppData.UtilityModels;
using PrototypeWithAuth.Data;
using PrototypeWithAuth.Models;
using PrototypeWithAuth.ViewModels;
using SQLitePCL;
using System;
using System.Collections.Generic;
using System.Drawing;
using System.Globalization;
using System.IO;
using System.Linq;
using System.Linq.Dynamic.Core;
using System.Threading.Tasks;
using X.PagedList;

namespace PrototypeWithAuth.Controllers
{
    public class ProtocolsController : SharedController
    {
        public enum ProtocolIconNamesEnum { Share, Favorite, MorePopover, Edit, RemoveShare }
        public ProtocolsController(ApplicationDbContext context, UserManager<ApplicationUser> userManager, IHostingEnvironment hostingEnvironment)
            : base(context, userManager, hostingEnvironment)
        {
        }

        [Authorize(Roles = "Protocols")]
        public async Task<IActionResult> Index()
        {
            TempData[AppUtility.TempDataTypes.MenuType.ToString()] = AppUtility.MenuItems.Protocols;
            TempData[AppUtility.TempDataTypes.SidebarType.ToString()] = AppUtility.SidebarEnum.List;
            TempData[AppUtility.TempDataTypes.PageType.ToString()] = AppUtility.PageTypeEnum.ProtocolsProtocols;
            var viewmodel = await GetProtocolsIndexViewModel(new ProtocolsIndexObject() { });

            return View(viewmodel);
        }

        [Authorize(Roles = "Protocols")]
        private static IQueryable<Protocol> filterListBySelectFilters(SelectedProtocolsFilters selectedFilters, IQueryable<Protocol> fullRequestsListProprietary)
        {
            if (selectedFilters != null)
            {
                if (selectedFilters.SelectedCategoriesIDs.Count() > 0)
                {
                    fullRequestsListProprietary = fullRequestsListProprietary.Where(p => selectedFilters.SelectedCategoriesIDs.Contains(p.ProtocolSubCategory.ProtocolCategoryTypeID));
                }
                if (selectedFilters.SelectedProtocolsSubcategoriesIDs.Count() > 0)
                {
                    fullRequestsListProprietary = fullRequestsListProprietary.Where(p => selectedFilters.SelectedProtocolsSubcategoriesIDs.Contains(p.ProtocolSubCategoryID));
                }
                if (selectedFilters.SelectedOwnersIDs.Count() > 0)
                {
                    fullRequestsListProprietary = fullRequestsListProprietary.Where(p => selectedFilters.SelectedOwnersIDs.Contains(p.ApplicationUserCreatorID));
                }
            }

            return fullRequestsListProprietary;
        }

        [Authorize(Roles = "Protocols")]
        private async Task<ProtocolsIndexViewModel> GetProtocolsIndexViewModel(ProtocolsIndexObject protocolsIndexObject, SelectedProtocolsFilters selectedFilters = null)
        {
            IQueryable<Protocol> ProtocolsPassedIn = Enumerable.Empty<Protocol>().AsQueryable();
            IQueryable<Protocol> fullProtocolsList = _context.Protocols.Include(p => p.ApplicationUserCreator).Include(p => p.ProtocolSubCategory)
                .ThenInclude(p => p.ProtocolCategoryType).Include(p => p.ProtocolType);

            switch (protocolsIndexObject.PageType)
            {
                case AppUtility.PageTypeEnum.ProtocolsProtocols:
                    switch (protocolsIndexObject.SidebarType)
                    {
                        case AppUtility.SidebarEnum.List:
                            break;
                        case AppUtility.SidebarEnum.MyProtocols:
                            break;
                        case AppUtility.SidebarEnum.Favorites:
                            break;
                        case AppUtility.SidebarEnum.SharedWithMe:
                            break;
                        case AppUtility.SidebarEnum.LastProtocol:
                            break;
                    }
                    break;
            }

            ProtocolsIndexViewModel protocolsIndexViewModel = new ProtocolsIndexViewModel();
            protocolsIndexViewModel.PageNumber = protocolsIndexObject.PageNumber;
            protocolsIndexViewModel.PageType = protocolsIndexObject.PageType;
            protocolsIndexViewModel.ErrorMessage = protocolsIndexObject.ErrorMessage;
            var onePageOfProducts = Enumerable.Empty<RequestIndexPartialRowViewModel>().ToPagedList();


            var ProtocolsPassedInWithInclude = filterListBySelectFilters(selectedFilters, fullProtocolsList);

            onePageOfProducts = await GetProtocolsColumnsAndRows(protocolsIndexObject, onePageOfProducts, ProtocolsPassedInWithInclude);

            protocolsIndexViewModel.PagedList = onePageOfProducts;
            List<PriceSortViewModel> priceSorts = new List<PriceSortViewModel>();
            protocolsIndexViewModel.ProtocolsFilterViewModel = GetProtocolFilterViewModel(selectedFilters);
            return protocolsIndexViewModel;
        }

        [Authorize(Roles = "Protocols")]
        private ProtocolsInventoryFilterViewModel GetProtocolFilterViewModel(SelectedProtocolsFilters selectedFilters = null, int numFilters = 0, AppUtility.MenuItems sectionType = AppUtility.MenuItems.Requests)
        {
            if (selectedFilters != null)
            {
                ProtocolsInventoryFilterViewModel inventoryFilterViewModel = new ProtocolsInventoryFilterViewModel()
                {
                    Owners = _context.Employees.Where(o => !selectedFilters.SelectedOwnersIDs.Contains(o.Id)).ToList(),
                    ProtocolCategories = _context.ProtocolCategories.Where(c => !selectedFilters.SelectedCategoriesIDs.Contains(c.ProtocolCategoryTypeID)).ToList(),
                    ProtocolSubCategories = _context.ProtocolSubCategories.Distinct().Where(v => !selectedFilters.SelectedProtocolsSubcategoriesIDs.Contains(v.ProtocolSubCategoryTypeID)).ToList(),
                    SelectedOwners = _context.Employees.Where(o => selectedFilters.SelectedOwnersIDs.Contains(o.Id)).ToList(),
                    SelectedProtocolCategories = _context.ProtocolCategories.Where(c => selectedFilters.SelectedCategoriesIDs.Contains(c.ProtocolCategoryTypeID)).ToList(),
                    SelectedProtocolSubCategories = _context.ProtocolSubCategories.Distinct().Where(v => selectedFilters.SelectedProtocolsSubcategoriesIDs.Contains(v.ProtocolSubCategoryTypeID)).ToList(),
                    NumFilters = numFilters
                };
                if (inventoryFilterViewModel.ProtocolCategories.Count() > 0)
                {
                    inventoryFilterViewModel.ProtocolSubCategories = inventoryFilterViewModel.ProtocolSubCategories.Where(ps => inventoryFilterViewModel.ProtocolCategories.Contains(ps.ProtocolCategoryType)).ToList();
                }

                return inventoryFilterViewModel;
            }
            else
            {
                return new ProtocolsInventoryFilterViewModel()
                {
                    Owners = _context.Employees.ToList(),
                    ProtocolCategories = _context.ProtocolCategories.ToList(),
                    ProtocolSubCategories = _context.ProtocolSubCategories.ToList(),
                    SelectedOwners = new List<Employee>(),
                    SelectedProtocolCategories = new List<ProtocolCategory>(),
                    SelectedProtocolSubCategories = new List<ProtocolSubCategory>(),
                    NumFilters = numFilters
                };
            }
        }

        [Authorize(Roles = "Protocols")]
        private async Task<IPagedList<RequestIndexPartialRowViewModel>> GetProtocolsColumnsAndRows(ProtocolsIndexObject protocolsIndexObject, IPagedList<RequestIndexPartialRowViewModel> onePageOfProtocols, IQueryable<Protocol> ProtocolPassedInWithInclude)
        {
            List<IconColumnViewModel> iconList = new List<IconColumnViewModel>();
            var defaultImage = "/images/css/CategoryImages/placeholder.png";
            switch (protocolsIndexObject.PageType)
            {
                case AppUtility.PageTypeEnum.ProtocolsProtocols:
                    switch (protocolsIndexObject.SidebarType)
                    {
                        case AppUtility.SidebarEnum.List:
                            onePageOfProtocols = await GetProtocolListRows(protocolsIndexObject, onePageOfProtocols, ProtocolPassedInWithInclude, iconList, defaultImage);
                            break;
                        case AppUtility.SidebarEnum.MyProtocols:
                            break;
                        case AppUtility.SidebarEnum.Favorites:
                            break;
                        case AppUtility.SidebarEnum.SharedWithMe:
                            break;
                        case AppUtility.SidebarEnum.LastProtocol:
                            break;
                    }
                    break;
            }
            return onePageOfProtocols;
        }
        private async Task<ReportsIndexViewModel> GetReportsIndexViewModel(ReportsIndexObject reportsIndexObject)
        {
            if(reportsIndexObject.Years.Count == 0)
            {
                reportsIndexObject.Years.Add(DateTime.Now.Year);
            }
            if (reportsIndexObject.Months.Count == 0)
            {
                reportsIndexObject.Months.Add(DateTime.Now.Month);
            }
            IQueryable<Report> ReportsPassedIn = Enumerable.Empty<Report>().AsQueryable();
            IQueryable<Report> ReportsPassedInWithInclude = _context.Reports.Where(r => reportsIndexObject.Years.Contains(r.DateCreated.Year) && reportsIndexObject.Months.Contains(r.DateCreated.Month) && r.ReportCategoryID == reportsIndexObject.ReportCategoryID)
                .Include(r => r.ReportType);

            switch (reportsIndexObject.PageType)
            {
                case AppUtility.PageTypeEnum.ProtocolsReports:
                    switch (reportsIndexObject.SidebarType)
                    {
                        case AppUtility.SidebarEnum.DailyReports:
                            break;
                        case AppUtility.SidebarEnum.WeeklyReports:
                            ReportsPassedInWithInclude = ReportsPassedInWithInclude.Where(r => r.ReportTypeID == 2);
                            break;
                        case AppUtility.SidebarEnum.MonthlyReports:
                            break;
                        case AppUtility.SidebarEnum.SharedWithMe:
                            break;
                        case AppUtility.SidebarEnum.LastProtocol:
                            break;
                    }
                    break;
            }

            ReportsIndexViewModel reportsIndexViewModel = new ReportsIndexViewModel();
            reportsIndexViewModel.ReportsIndexObject = reportsIndexObject;
            reportsIndexViewModel.ErrorMessage = reportsIndexObject.ErrorMessage;

            var onePageOfReports = Enumerable.Empty<ReportIndexPartialRowViewModel>().ToPagedList();

            onePageOfReports = await GetReportsColumnsAndRows(reportsIndexObject, onePageOfReports, ReportsPassedInWithInclude);

            reportsIndexViewModel.PagedList = onePageOfReports;

            return reportsIndexViewModel;
        }
        [Authorize(Roles = "Protocols")]
        private async Task<IPagedList<ReportIndexPartialRowViewModel>> GetReportsColumnsAndRows(ReportsIndexObject reportsIndexObject, IPagedList<ReportIndexPartialRowViewModel> onePageOfReports, IQueryable<Report> ReportPassedInWithInclude)
        {
            List<IconColumnViewModel> iconList = new List<IconColumnViewModel>();
            var defaultImage = "/images/css/CategoryImages/placeholder.png";
            switch (reportsIndexObject.PageType)
            {
                case AppUtility.PageTypeEnum.ProtocolsReports:
                    switch (reportsIndexObject.SidebarType)
                    {
                        case AppUtility.SidebarEnum.WeeklyReports:
                            onePageOfReports = await GetReportListRows(reportsIndexObject, onePageOfReports, ReportPassedInWithInclude);
                            break;
                        case AppUtility.SidebarEnum.DailyReports:
                            break;
                        case AppUtility.SidebarEnum.MonthlyReports:
                            break;
                        case AppUtility.SidebarEnum.SharedWithMe:
                            break;
                        case AppUtility.SidebarEnum.LastProtocol:
                            break;
                    }
                    break;
            }
            return onePageOfReports;
        }

        [Authorize(Roles = "Protocols")]
        private static async Task<IPagedList<RequestIndexPartialRowViewModel>> GetProtocolListRows(ProtocolsIndexObject requestIndexObject, IPagedList<RequestIndexPartialRowViewModel> onePageOfProtocols, IQueryable<Protocol> ProtocolPassedInWithInclude, List<IconColumnViewModel> iconList, string defaultImage)
        {
            onePageOfProtocols = await ProtocolPassedInWithInclude.OrderByDescending(r => r.CreationDate).ToList().Select(p => new RequestIndexPartialRowViewModel()
            {
                Columns = new List<RequestIndexPartialColumnViewModel>()
                            {
                                 new RequestIndexPartialColumnViewModel() { Title = "", Width=10, Image = defaultImage},
                                 new RequestIndexPartialColumnViewModel() { Title = "Name", AjaxLink=" load-protocol ", AjaxID=p.ProtocolID, Width=15, Value = new List<string>(){ p.Name}},
                                 new RequestIndexPartialColumnViewModel() { Title = "Version", Width=10, Value = new List<string>(){ p.VersionNumber} },
                                 new RequestIndexPartialColumnViewModel() { Title = "Creator", Width=10, Value = new List<string>(){p.ApplicationUserCreator.FirstName + " " + p.ApplicationUserCreator.LastName}},
                                 new RequestIndexPartialColumnViewModel() { Title = "Time", Width=11, Value = new List<string>(){ } },
                                 new RequestIndexPartialColumnViewModel() { Title = "Date Created", Width=12, Value = new List<string>(){ p.CreationDate.ToString("dd'/'MM'/'yyyy") }},
                                 new RequestIndexPartialColumnViewModel() { Title = "Type", Width=10, Value = new List<string>(){ p.ProtocolType.ProtocolTypeDescription } },
                                 new RequestIndexPartialColumnViewModel() { Title = "Category", Width=12, Value = new List<string>(){ p.ProtocolSubCategory.ProtocolSubCategoryTypeDescription } },
                                 new RequestIndexPartialColumnViewModel() { Title = "", Width=10, Icons = iconList, AjaxID = p.ProtocolID }
                            }
            }).ToPagedListAsync(requestIndexObject.PageNumber == 0 ? 1 : requestIndexObject.PageNumber, 25);
            return onePageOfProtocols;
        }

        private static async Task<IPagedList<ReportIndexPartialRowViewModel>> GetReportListRows(ReportsIndexObject reportsIndexObject, IPagedList<ReportIndexPartialRowViewModel> onePageOfReports, IQueryable<Report> ReportPassedInWithInclude)
        {
            var reports = ReportPassedInWithInclude.OrderByDescending(r => r.DateCreated).ToList();
            onePageOfReports = await ReportPassedInWithInclude.OrderByDescending(r => r.DateCreated).ToList().Select(r => new ReportIndexPartialRowViewModel(AppUtility.ReportTypes.Weekly, r, r.ReportCategory, reportsIndexObject)
            ).ToPagedListAsync(reportsIndexObject.PageNumber == 0 ? 1 : reportsIndexObject.PageNumber, 25);
            return onePageOfReports;
        }

        [Authorize(Roles = "Protocols")]
        public async Task<IActionResult> CurrentProtocols()
        {
            TempData[AppUtility.TempDataTypes.MenuType.ToString()] = AppUtility.MenuItems.Protocols;
            TempData[AppUtility.TempDataTypes.SidebarType.ToString()] = AppUtility.SidebarEnum.CurrentProtocols;
            TempData[AppUtility.TempDataTypes.PageType.ToString()] = AppUtility.PageTypeEnum.ProtocolsWorkflow;
            return View();
        }
        [Authorize(Roles = "Protocols")]
        public async Task<IActionResult> Projects()
        {
            TempData[AppUtility.TempDataTypes.MenuType.ToString()] = AppUtility.MenuItems.Protocols;
            TempData[AppUtility.TempDataTypes.SidebarType.ToString()] = AppUtility.SidebarEnum.Projects;
            TempData[AppUtility.TempDataTypes.PageType.ToString()] = AppUtility.PageTypeEnum.ProtocolsWorkflow;
            return View();
        }
        [Authorize(Roles = "Protocols")]
        public async Task<IActionResult> SharedProjects()
        {
            TempData[AppUtility.TempDataTypes.MenuType.ToString()] = AppUtility.MenuItems.Protocols;
            TempData[AppUtility.TempDataTypes.SidebarType.ToString()] = AppUtility.SidebarEnum.SharedProjects;
            TempData[AppUtility.TempDataTypes.PageType.ToString()] = AppUtility.PageTypeEnum.ProtocolsWorkflow;
            return View();
        }
        [Authorize(Roles = "Protocols")]
        public async Task<IActionResult> Calendar()
        {
            TempData[AppUtility.TempDataTypes.MenuType.ToString()] = AppUtility.MenuItems.Protocols;
            TempData[AppUtility.TempDataTypes.SidebarType.ToString()] = AppUtility.SidebarEnum.Calendar;
            TempData[AppUtility.TempDataTypes.PageType.ToString()] = AppUtility.PageTypeEnum.ProtocolsWorkflow;
            return View();
        }
        [Authorize(Roles = "Protocols")]
        public async Task<IActionResult> MyProtocols()
        {
            TempData[AppUtility.TempDataTypes.MenuType.ToString()] = AppUtility.MenuItems.Protocols;
            TempData[AppUtility.TempDataTypes.SidebarType.ToString()] = AppUtility.SidebarEnum.MyProtocols;
            TempData[AppUtility.TempDataTypes.PageType.ToString()] = AppUtility.PageTypeEnum.ProtocolsProtocols;
            return View();
        }

        [Authorize(Roles = "Protocols")]
        public async Task<IActionResult> ProtocolsFavorites()
        {
            TempData[AppUtility.TempDataTypes.MenuType.ToString()] = AppUtility.MenuItems.Protocols;
            TempData[AppUtility.TempDataTypes.SidebarType.ToString()] = AppUtility.SidebarEnum.MyProtocols;
            TempData[AppUtility.TempDataTypes.PageType.ToString()] = AppUtility.PageTypeEnum.ProtocolsProtocols;
            return View();
        }

        [Authorize(Roles = "Protocols")]
        public async Task<IActionResult> ProtocolsSharedWithMe()
        {
            TempData[AppUtility.TempDataTypes.MenuType.ToString()] = AppUtility.MenuItems.Protocols;
            TempData[AppUtility.TempDataTypes.SidebarType.ToString()] = AppUtility.SidebarEnum.SharedWithMe;
            TempData[AppUtility.TempDataTypes.PageType.ToString()] = AppUtility.PageTypeEnum.ProtocolsProtocols;
            return View();
        }
        [Authorize(Roles = "Protocols")]
        public async Task<IActionResult> LastProtocol()
        {
            TempData[AppUtility.TempDataTypes.MenuType.ToString()] = AppUtility.MenuItems.Protocols;
            TempData[AppUtility.TempDataTypes.SidebarType.ToString()] = AppUtility.SidebarEnum.LastProtocol;
            TempData[AppUtility.TempDataTypes.PageType.ToString()] = AppUtility.PageTypeEnum.ProtocolsProtocols;
            return View();
        }
        [Authorize(Roles = "Protocols")]
        public async Task<IActionResult> ResearchProtocol()
        {
            TempData[AppUtility.TempDataTypes.MenuType.ToString()] = AppUtility.MenuItems.Protocols;
            TempData[AppUtility.TempDataTypes.SidebarType.ToString()] = AppUtility.SidebarEnum.ResearchProtocol;
            TempData[AppUtility.TempDataTypes.PageType.ToString()] = AppUtility.PageTypeEnum.ProtocolsCreate;
            CreateProtocolsViewModel viewmodel = await FillCreateProtocolsViewModel(1);
            return View(viewmodel);
        }

        private async Task<CreateProtocolsViewModel> FillCreateProtocolsViewModel(int typeID, int protocolID = 0)
        {
            var protocol = _context.Protocols
                .Include(p => p.Urls).Include(p => p.Materials)
                .ThenInclude(m => m.Product).Where(p => p.ProtocolID == protocolID).FirstOrDefault() ?? new Protocol();
            protocol.Urls ??= new List<Link>();
            protocol.Materials ??= new List<Material>();

            if (protocol.Urls.Count() < 2)
            {
                while (protocol.Urls.Count() < 2)
                {
                    protocol.Urls.Add(new Link());
                }
            }
            if (typeID != 0)
            {
                protocol.ProtocolTypeID = typeID;
            }

            var viewmodel = new CreateProtocolsViewModel()
            {
                Protocol = protocol,
                ProtocolCategories = _context.ProtocolCategories,
                ProtocolSubCategories = _context.ProtocolSubCategories,
                MaterialCategories = _context.MaterialCategories,
                LineTypes = _context.LineTypes.ToList(),
                FunctionTypes = _context.FunctionTypes
            };
            await CopySelectedLinesToTempLineTable(protocol.ProtocolID);
            viewmodel.TempLines = OrderLinesForView(protocolID);
            string uploadProtocolsFolder = Path.Combine(_hostingEnvironment.WebRootPath, AppUtility.ParentFolderName.Materials.ToString());
            string uploadProtocolsFolder2 = Path.Combine(uploadProtocolsFolder, protocol.ProtocolID.ToString());
            FillDocumentsInfo(viewmodel, uploadProtocolsFolder2);
            Dictionary<Material, List<DocumentFolder>> MaterialFolders = FillMaterialDocumentsModel(protocol.Materials, uploadProtocolsFolder);
            viewmodel.MaterialDocuments = (Lookup<Material, List<DocumentFolder>>)MaterialFolders.ToLookup(o => o.Key, o => o.Value);
            return viewmodel;
        }
        private TempLine TurnLineIntoTempLine(Line line)
        {
            TempLine tempLine = new TempLine();
            tempLine.PermanentLineID = line.LineID;
            tempLine.Content = line.Content;
            tempLine.LineNumber = line.LineNumber;
            tempLine.LineTypeID = line.LineTypeID;
            tempLine.ProtocolID = line.ProtocolID;
            tempLine.ParentLineID = line.ParentLineID;
            return tempLine;
        }
        private Line TurnTempLineToLine(TempLine tempLine)
        {
            Line line = new Line();
            line.LineID = tempLine.PermanentLineID ?? tempLine.LineID;
            line.Content = tempLine.Content;
            line.LineNumber = tempLine.LineNumber;
            line.LineTypeID = tempLine.LineTypeID;
            line.ProtocolID = tempLine.ProtocolID;
            line.ParentLineID = tempLine.ParentLineID;
            return line;
        }
        private List<LineType> GetOrderLineTypeFromParentToChild()
        {
            List<LineType> orderedLineTypes = new List<LineType>();
            var lineTypes = _context.LineTypes;
            var parentLineType = lineTypes.Where(lt => lt.LineTypeParentID == null).FirstOrDefault();
            orderedLineTypes.Add(parentLineType);
            while (parentLineType.LineTypeChildID != null)
            {
                parentLineType = lineTypes.Where(lt => lt.LineTypeID == parentLineType.LineTypeChildID).FirstOrDefault();
                orderedLineTypes.Add(parentLineType);
            }
            return orderedLineTypes;
        }
        private List<LineType> GetOrderLineTypeFromChildToParent()
        {
            List<LineType> orderedLineTypes = new List<LineType>();
            var lineTypes = _context.LineTypes;
            var childLineType = lineTypes.Where(lt => lt.LineTypeChildID == null).FirstOrDefault();
            orderedLineTypes.Add(childLineType);
            while (childLineType.LineTypeParentID != null)
            {
                childLineType = lineTypes.Where(lt => lt.LineTypeID == childLineType.LineTypeParentID).FirstOrDefault();
                orderedLineTypes.Add(childLineType);
            }
            return orderedLineTypes;
        }

        private async Task CopySelectedLinesToTempLineTable(int protocolID)
        {
            await ClearTempLinesTableAsync();
            var lines = _context.Lines.Where(l => l.ProtocolID == protocolID);
            var lineTypes = GetOrderLineTypeFromParentToChild();
            foreach (var lineType in lineTypes)
            {
                var linesByType = lines.Where(l => l.LineTypeID == lineType.LineTypeID);
                foreach (var line in linesByType)
                {
                    _context.Add(TurnLineIntoTempLine(line));
                }
                await _context.SaveChangesAsync();
            }
        }
        public async Task SaveTempLines(List<TempLine> TempLines)
        {
            using (var transaction = _context.Database.BeginTransaction())
            {
                try
                {
                    await UpdateLineContentAsync(TempLines);
                    var tempLines = _context.TempLines;
                    var lines = _context.Lines.Where(l => l.ProtocolID == tempLines.FirstOrDefault().ProtocolID);
                    foreach (var line in tempLines)
                    {
                        _context.Update(TurnTempLineToLine(line));
                    }
                    await _context.SaveChangesAsync();
                    await _context.FunctionLines.Where(fl => fl.IsTemporary).ForEachAsync(fl => fl.IsTemporary = false);
                    await CopySelectedLinesToTempLineTable(tempLines.FirstOrDefault().ProtocolID);
                    await transaction.CommitAsync();
                }
                catch (Exception ex)
                {
                    Response.StatusCode = 500;
                    await transaction.RollbackAsync();
                    //  await Response.WriteAsync(AppUtility.GetExceptionMessage(ex));
                }
            }
        }
        private async Task ClearTempLinesTableAsync()
        {
            var lineTypes = GetOrderLineTypeFromChildToParent();

            await _context.FunctionLines.Where(fl => fl.IsTemporary).ForEachAsync(fl => { _context.Remove(fl); });
            await _context.SaveChangesAsync();
            foreach (var lineType in lineTypes)
            {
                var linesByType = _context.TempLines.Where(l => l.LineTypeID == lineType.LineTypeID);
                foreach (var line in linesByType)
                {
                    _context.Remove(line);
                }
                await _context.SaveChangesAsync();
                var tempLines = _context.Lines.Where(l => l.LineTypeID == lineType.LineTypeID).Where(tl => tl.IsTemporary);
                foreach (var tempLine in tempLines)
                {
                    _context.Remove(tempLine);
                }
                await _context.SaveChangesAsync();
            }         

        }
        private Dictionary<Material, List<DocumentFolder>> FillMaterialDocumentsModel(IEnumerable<Material> Materials, string uploadProtocolsFolder)
        {
            Dictionary<Material, List<DocumentFolder>> MaterialFolders = new Dictionary<Material, List<DocumentFolder>>();
            foreach (var material in Materials)
            {
                List<DocumentFolder> folders = new List<DocumentFolder>();
                string uploadMaterialFolder2 = Path.Combine(uploadProtocolsFolder, material.MaterialID.ToString());
                base.GetExistingFileStrings(folders, AppUtility.FolderNamesEnum.Pictures, uploadMaterialFolder2);
                MaterialFolders.Add(material, folders);
            }

            return MaterialFolders;
        }

        [Authorize(Roles = "Protocols")]
        public async Task<IActionResult> AddMaterialModal(int materialTypeID, int ProtocolID)
        {
            var MaterialCategory = _context.MaterialCategories.Where(mc => mc.MaterialCategoryID == materialTypeID).FirstOrDefault();

            var viewModel = new AddMaterialViewModel()
            {
                Material = new Material()
                {
                    MaterialCategoryID = materialTypeID,
                    MaterialCategory = MaterialCategory,
                    ProtocolID = ProtocolID
                }
            };
            return PartialView(viewModel);
        }
        [Authorize(Roles = "Protocols")]
        public async Task<IActionResult> MaterialInfoModal(int materialID)
        {
            var material = _context.Materials.Where(m => m.MaterialID == materialID).FirstOrDefault();
            return PartialView(new AddMaterialViewModel { Material = material });
        }



        [HttpPost]
        [Authorize(Roles = "Protocols")]
        public async Task<IActionResult> MaterialInfoModal(AddMaterialViewModel addMaterialViewModel)
        {
            var materialDB = _context.Materials.Where(m => m.MaterialID == addMaterialViewModel.Material.MaterialID).FirstOrDefault();
            using (var transaction = _context.Database.BeginTransaction())
            {
                try
                {
                    materialDB.Info = addMaterialViewModel.Material.Info;
                    _context.Update(materialDB);
                    await _context.SaveChangesAsync();
                    await transaction.CommitAsync();
                }
                catch (Exception ex)
                {
                    Response.StatusCode = 500;
                    await transaction.RollbackAsync();
                    return PartialView("MaterialInfoModal", new AddMaterialViewModel { Material = _context.Materials.Where(m => m.MaterialID == addMaterialViewModel.Material.MaterialID).FirstOrDefault(), ErrorMessage = AppUtility.GetExceptionMessage(ex) });
                }
                return redirectToMaterialTab(materialDB.ProtocolID);
            }
        }
        [HttpPost]
        [HttpGet]
        [Authorize(Roles = "Protocols")]
        public async Task<IActionResult> _Lines(List<TempLine> TempLines, int lineTypeID, int currentLineID, int protocolID)
        {
            using (var transaction = _context.Database.BeginTransaction())
            {
                try
                {
                    if(TempLines != null)
                    {
                        //save all temp line data 
                        await UpdateLineContentAsync(TempLines);
                    }              
                
                    var currentLine = _context.TempLines.Include(tl => tl.ParentLine).Where(l => l.PermanentLineID == currentLineID).FirstOrDefault();
                    var newLineType = _context.LineTypes.Where(lt => lt.LineTypeID == lineTypeID).FirstOrDefault();
                    var orderedLineTypes = GetOrderLineTypeFromParentToChild();
                    TempLine newLine = new TempLine();
                    newLine.LineTypeID = lineTypeID;
                    newLine.ProtocolID = protocolID;
                    if (newLine.LineNumber == 0)
                    {
                        newLine.LineNumber = 1;
                    }
                    _context.Add(newLine);
                    await _context.SaveChangesAsync();
                    _context.Add(new Line { LineID = newLine.LineID, LineTypeID = lineTypeID, ProtocolID = protocolID, IsTemporary = true });
                    await _context.SaveChangesAsync();
                    newLine.PermanentLineID = newLine.LineID;
                    _context.Update(newLine);
                    await _context.SaveChangesAsync();

                    if (currentLine != null)
                    {
                        var currentLineTypeIndex = orderedLineTypes.IndexOf(currentLine.LineType);
                        var newLineTypeIndex = orderedLineTypes.IndexOf(newLineType);
                        if (newLineTypeIndex <= currentLineTypeIndex)
                        {
                            //new line is parent of child
                            var parent = currentLine;

                            newLine.LineTypeID = newLineType.LineTypeID;

                            while (parent != null)
                            {
                                if (parent.LineTypeID == newLineType.LineTypeID)
                                {
                                    newLine.LineNumber = (parent.LineNumber + 1);
                                    newLine.ParentLineID = parent.ParentLineID;
                                    _context.Update(newLine);
                                    //we have to increment all the sibling parents
                                    var siblings = _context.TempLines.Where(tl => tl.LineNumber > parent.LineNumber && tl.ParentLineID == newLine.ParentLineID);
                                    await siblings.ForEachAsync(tl => { tl.LineNumber += 1; _context.Update(tl); });
                                    await _context.SaveChangesAsync();

                                    break;
                                }
                                parent = _context.TempLines.Where(tl => tl.PermanentLineID == parent.ParentLineID).FirstOrDefault();
                            }


                            if (newLineTypeIndex < currentLineTypeIndex)
                            {
                                //get currentline siblings and make their parent point to new line
                                var currentLineSiblings = _context.TempLines.Where(lt => lt.ParentLineID == currentLine.PermanentLineID && lt.LineNumber > currentLine.LineNumber);
                                await currentLineSiblings.ForEachAsync(tl =>
                                {
                                    tl.LineNumber -= currentLine.LineNumber;
                                    tl.ParentLineID = newLine.LineID;
                                    _context.Update(tl);
                                });
                                await _context.SaveChangesAsync();

                                currentLine.ParentLine = _context.TempLines.Where(tl => tl.PermanentLineID == currentLine.ParentLineID).Include(tl => tl.LineType).FirstOrDefault();
                                if (orderedLineTypes.IndexOf(currentLine.ParentLine.LineType) < newLineTypeIndex)
                                {
                                    //make new line currents parent
                                    newLine.ParentLineID = currentLine.ParentLineID;
                                    currentLine.ParentLineID = newLine.LineID;
                                    _context.Update(currentLine);
                                    _context.Update(newLine);
                                    await _context.SaveChangesAsync();
                                }
                            }
                            else // types are the same
                            {
                                //all curents children should pount to new line
                                await _context.TempLines.Where(tl => tl.ParentLineID == currentLine.PermanentLineID).ForEachAsync(tl => { tl.ParentLineID = newLine.LineID; _context.Update(tl); });
                                await _context.SaveChangesAsync();

                            }

                        }
                        else
                        {
                            newLine.LineNumber = 1;
                            newLine.ParentLineID = currentLine.PermanentLineID;
                            _context.Update(newLine);
                            var siblings = _context.TempLines.Where(tl => tl.ParentLineID == newLine.ParentLineID);
                            await siblings.ForEachAsync(tl => { tl.LineNumber += 1; _context.Update(tl); });
                            await _context.SaveChangesAsync();
                        }

                    }
                    await transaction.CommitAsync();
                }
                catch (Exception ex)
                {
                    Response.StatusCode = 500;
                    await transaction.RollbackAsync();
                    //  await Response.WriteAsync(AppUtility.GetExceptionMessage(ex));
                    return PartialView("_Lines", new ProtocolsLinesViewModel { Lines = OrderLinesForView(protocolID), ErrorMessage = AppUtility.GetExceptionMessage(ex) });
                }
            }

            List<ProtocolsLineViewModel> refreshedLines = OrderLinesForView(protocolID);

            return PartialView("_Lines", new ProtocolsLinesViewModel { Lines = refreshedLines });
        }


        public bool CheckIfSerialNumberExists(string serialNumber)
        {
            return _context.Products.Where(p => p.SerialNumber.Equals(serialNumber)).ToList().Any();
        }
        private async Task UpdateLineContentAsync(List<TempLine> TempLines)
        {
            foreach (var line in TempLines)
            {
                var temp = _context.TempLines.Where(tl => tl.PermanentLineID == line.PermanentLineID).FirstOrDefault();
                if (temp != null)
                {
                    temp.Content = line.Content ?? "";
                    _context.Update(temp);
                }
            }
            await _context.SaveChangesAsync();
        }

        private List<ProtocolsLineViewModel> OrderLinesForView(int protocolID)
        {
            var functionLine = _context.FunctionLines.Where(fl => fl.Line.ProtocolID == protocolID).Include(fl=>fl.FunctionType).ToList();
            List<ProtocolsLineViewModel> refreshedLines = new List<ProtocolsLineViewModel>();
            Stack<TempLine> parentNodes = new Stack<TempLine>();
            var lineTypes = _context.LineTypes.ToList();
            _context.TempLines.Where(tl => tl.ParentLineID == null).OrderByDescending(tl => tl.LineNumber).ToList().ForEach(tl => { parentNodes.Push(tl); });
            int count = 0;
            while (!parentNodes.IsEmpty())
            {
                var node = parentNodes.Pop();

                refreshedLines.Add(new ProtocolsLineViewModel()
                {
                    LineTypes = lineTypes,
                    TempLine = node,
                    Index = count++,
                    LineNumberString = refreshedLines.Where(rl => rl.TempLine.PermanentLineID == node.ParentLineID)?.FirstOrDefault()?.LineNumberString + node.LineNumber + ".",
                    Functions = functionLine.Where(fl => fl.LineID == node.PermanentLineID)
                }); 
                _context.TempLines.Where(c => c.ParentLineID == (node.PermanentLineID)).OrderByDescending(tl => tl.LineNumber).ToList().ForEach(c => { parentNodes.Push(c); });
            }
            if (refreshedLines.Count == 0)
            {
                refreshedLines.Add(new ProtocolsLineViewModel() { LineTypes = lineTypes, Index = 0, LineNumberString = 1 + "" });
            }
            return refreshedLines;
        }

        [Authorize(Roles = "Protocols")]
        public async Task<IActionResult> DeleteMaterial(int materialID)
        {
            var material = _context.Materials.Where(m => m.MaterialID == materialID).Include(m => m.Product).FirstOrDefault();
            return PartialView(new AddMaterialViewModel { Material = material });
        }


        [HttpPost]
        [Authorize(Roles = "Protocols")]
        public async Task<IActionResult> DeleteMaterial(AddMaterialViewModel addMaterialViewModel)
        {
            var materialDB = _context.Materials.Where(m => m.MaterialID == addMaterialViewModel.Material.MaterialID).FirstOrDefault();
            using (var transaction = _context.Database.BeginTransaction())
            {
                try
                {
                    materialDB.IsDeleted = true;
                    _context.Update(materialDB);
                    await _context.SaveChangesAsync();
                    await transaction.CommitAsync();
                }
                catch (Exception ex)
                {
                    Response.StatusCode = 500;
                    await transaction.RollbackAsync();
                    return PartialView("DeleteMaterial", new AddMaterialViewModel { Material = _context.Materials.Where(m => m.MaterialID == addMaterialViewModel.Material.MaterialID).FirstOrDefault(), ErrorMessage = AppUtility.GetExceptionMessage(ex) });
                }
                return redirectToMaterialTab(materialDB.ProtocolID);
            }
        }

        [Authorize(Roles = "Protocols")]
        public async Task<IActionResult> AddFunctionModal(int FunctionTypeID, int LineID, int functionLineID)
        {
            var functionType = _context.FunctionTypes.Where(ft => ft.FunctionTypeID == FunctionTypeID).FirstOrDefault();
            var tempLine = _context.TempLines.Where(tl => tl.PermanentLineID ==  LineID).FirstOrDefault();
            var line = TurnTempLineToLine(tempLine);
            FunctionLine functionLine = _context.FunctionLines.Where(fl => fl.FunctionLineID == functionLineID).FirstOrDefault();
            if(functionLine == null)
            {
                functionLine = new FunctionLine
                {
                    FunctionType = functionType,
                    FunctionTypeID = FunctionTypeID,
                    Line = line,
                    LineID = LineID
                };
            }
          
            var viewmodel = new AddFunctionViewModel
            {
                FunctionLine = functionLine
            };

            switch (Enum.Parse<AppUtility.ProtocolFunctionTypes>(functionType.DescriptionEnum))
            {
                case AppUtility.ProtocolFunctionTypes.AddLinkToProduct:
                    viewmodel.ParentCategories = _context.ParentCategories.ToList();
                    viewmodel.ProductSubcategories = _context.ProductSubcategories.ToList();
                    viewmodel.Products = _context.Products.ToList();
                    viewmodel.Vendors = _context.Vendors.ToList();
                    break;
                case AppUtility.ProtocolFunctionTypes.AddLinkToProtocol:
                    viewmodel.ProtocolCategories = _context.ProtocolCategories.ToList();
                    viewmodel.ProtocolSubCategories = _context.ProtocolSubCategories.ToList();
                    viewmodel.Creators = _context.Users.Select(u =>
                        new SelectListItem() { Value = u.Id, Text = u.FirstName + u.LastName }).ToList();
                    viewmodel.Protocols = _context.Protocols.ToList();
                    break;
            }
            return PartialView(viewmodel);
        }


        [HttpPost]
        [Authorize(Roles = "Protocols")]
        public async Task<IActionResult> AddFunctionModal(AddFunctionViewModel addFunctionViewModel)
        {
            using (var transaction = _context.Database.BeginTransaction())
            {
                var line = _context.TempLines.Where(l => l.PermanentLineID == addFunctionViewModel.FunctionLine.LineID).FirstOrDefault();
                try
                {
                   
                    var functionType = _context.FunctionTypes.Where(ft => ft.FunctionTypeID == addFunctionViewModel.FunctionLine.FunctionTypeID).FirstOrDefault();
                 
                    switch (Enum.Parse<AppUtility.ProtocolFunctionTypes>(functionType.DescriptionEnum))
                    {
                        case AppUtility.ProtocolFunctionTypes.AddLinkToProduct:
                            var product = _context.Products.Where(p => p.ProductID == addFunctionViewModel.FunctionLine.ProductID).FirstOrDefault();
                            line.Content += " <a href='#' contenteditable=false class='open-line-product' value='" + product.ProductID + "'>" + product.ProductName + "</a> ";
                            break;
                        case AppUtility.ProtocolFunctionTypes.AddLinkToProtocol:
                            var protocol = _context.Protocols.Include(p => p.Materials).Where(p => p.ProtocolID == addFunctionViewModel.FunctionLine.ProtocolID).FirstOrDefault();
                            line.Content += " <a href='#' contenteditable=false class='open-line-protocol' value='" + protocol.ProtocolID + "'>" + protocol.Name + " </a> ";
                            break;
                        case AppUtility.ProtocolFunctionTypes.AddFile:
                        case AppUtility.ProtocolFunctionTypes.AddImage:
                            MoveDocumentsOutOfTempFolder(addFunctionViewModel.FunctionLine.FunctionLineID, AppUtility.ParentFolderName.FunctionLine);
                            await SaveTempFunctionLineAsync(addFunctionViewModel);
                            break;
                        //case AppUtility.FuctionTypes.AddStop:
                        //    break;
                        case AppUtility.ProtocolFunctionTypes.AddTable:
                            await SaveTempFunctionLineAsync(addFunctionViewModel);
                            break;
                        case AppUtility.ProtocolFunctionTypes.AddTemplate:
                            await SaveTempFunctionLineAsync(addFunctionViewModel);
                            break;
                            //case AppUtility.FuctionTypes.AddTimer:
                            //    break;
                            //case AppUtility.FuctionTypes.AddTip:
                            //case AppUtility.FuctionTypes.AddWarning:
                            //case AppUtility.FuctionTypes.AddComment:
                            //    break;
                    }
                    _context.Update(line);
                    await _context.SaveChangesAsync();
                    await transaction.CommitAsync();
                }
                catch (Exception ex)
                {
                    Response.StatusCode = 500;
                    await transaction.RollbackAsync();
                    //  await Response.WriteAsync(AppUtility.GetExceptionMessage(ex));
                }
                return PartialView("_Lines", new ProtocolsLinesViewModel { Lines = OrderLinesForView(line.ProtocolID) });
                return PartialView("_Lines", new ProtocolsLinesViewModel { Lines = OrderLinesForView(line.ProtocolID) });
            }           
        }

        private async Task SaveTempFunctionLineAsync(AddFunctionViewModel addFunctionViewModel)
        {
            addFunctionViewModel.FunctionLine.Product = null;
            addFunctionViewModel.FunctionLine.Protocol = null;
            addFunctionViewModel.FunctionLine.IsTemporary = true;
            _context.Add(addFunctionViewModel.FunctionLine);
            await _context.SaveChangesAsync();
        }

        [HttpPost]
        [Authorize(Roles = "Protocols")]
        public async Task<IActionResult> AddMaterialModal(AddMaterialViewModel addMaterialViewModel)
        {
            var Protocol = _context.Protocols.Where(p => p.ProtocolID == addMaterialViewModel.Material.ProtocolID).FirstOrDefault();
            var product = _context.Products.Where(p => p.SerialNumber.Equals(addMaterialViewModel.Material.Product.SerialNumber)).FirstOrDefault();
            using (var transaction = _context.Database.BeginTransaction())
            {
                try
                {
                    if (product != null)
                    {
                        addMaterialViewModel.Material.ProductID = product.ProductID;
                    }

                    _context.Entry(addMaterialViewModel.Material).State = EntityState.Added;
                    await _context.SaveChangesAsync();
                    base.MoveDocumentsOutOfTempFolder(addMaterialViewModel.Material.MaterialID, AppUtility.ParentFolderName.Materials);
                    await transaction.CommitAsync();
                }
                catch (Exception ex)
                {
                    addMaterialViewModel.Material.MaterialCategory = _context.MaterialCategories.Where(mc => mc.MaterialCategoryID == addMaterialViewModel.Material.MaterialCategoryID).FirstOrDefault();
                    addMaterialViewModel.ErrorMessage = AppUtility.GetExceptionMessage(ex);
                    Response.StatusCode = 500;
                    await transaction.RollbackAsync();
                    return PartialView("AddMaterialModal", addMaterialViewModel);
                }
            }
            addMaterialViewModel.Material.Product = product;
            return redirectToMaterialTab(addMaterialViewModel.Material.ProtocolID);
        }

        private IActionResult redirectToMaterialTab(int protocolID)
        {
            string uploadProtocolsFolder = Path.Combine(_hostingEnvironment.WebRootPath, AppUtility.ParentFolderName.Materials.ToString());
            var materials = _context.Materials.Include(m => m.Product).Where(m => m.ProtocolID == protocolID);
            Dictionary<Material, List<DocumentFolder>> MaterialFolders = FillMaterialDocumentsModel(materials, uploadProtocolsFolder);
            return PartialView("_MaterialTab", new MaterialTabViewModel() { Materials = materials.ToList(), MaterialCategories = _context.MaterialCategories, Folders = (Lookup<Material, List<DocumentFolder>>)MaterialFolders.ToLookup(o => o.Key, o => o.Value) });
        }

        [Authorize(Roles = "Requests")]
        public async Task<IActionResult> ProtocolsProductDetails(int? productID)
        {
            var requestID = _context.Requests.Where(r => r.ProductID == productID).OrderByDescending(r => r.ParentRequest.OrderDate).Select(r => r.RequestID).FirstOrDefault();
            var requestItemViewModel = await editModalViewFunction(requestID, isEditable: false);
            requestItemViewModel.SectionType = AppUtility.MenuItems.Protocols;
            return PartialView(requestItemViewModel);
        }

        [HttpPost]
        [Authorize(Roles = "Protocols")]
        public async Task<IActionResult> CreateProtocol(CreateProtocolsViewModel createProtocolsViewModel)
        {

            using (var transaction = _context.Database.BeginTransaction())
            {
                try
                {
                    createProtocolsViewModel.Protocol.Urls = createProtocolsViewModel.Protocol.Urls.Where(u => u.LinkDescription != null && u.Url != null).ToList();

                    createProtocolsViewModel.Protocol.CreationDate = DateTime.Now;
                    createProtocolsViewModel.Protocol.ApplicationUserCreatorID = _userManager.GetUserId(User);
                    if (createProtocolsViewModel.Protocol.ProtocolID == 0)
                    {
                        _context.Entry(createProtocolsViewModel.Protocol).State = EntityState.Added;
                    }
                    else
                    {
                        _context.Entry(createProtocolsViewModel.Protocol).State = EntityState.Modified;
                    }
                    await _context.SaveChangesAsync();
                    foreach (var url in createProtocolsViewModel.Protocol.Urls)
                    {
                        if (url.LinkID == 0)
                        {
                            _context.Entry(url).State = EntityState.Added;
                            await _context.SaveChangesAsync();
                            base.MoveDocumentsOutOfTempFolder(createProtocolsViewModel.Protocol.ProtocolID, AppUtility.ParentFolderName.Protocols);

                        }
                        else
                        {
                            _context.Entry(url).State = EntityState.Modified;
                            await _context.SaveChangesAsync();
                        }
                    }
                    await transaction.CommitAsync();
                    createProtocolsViewModel = await FillCreateProtocolsViewModel(createProtocolsViewModel.Protocol.ProtocolTypeID, createProtocolsViewModel.Protocol.ProtocolID);
                  
                    return PartialView("_CreateProtocolTabs", createProtocolsViewModel);
                }
                catch (Exception ex)
                {
                    createProtocolsViewModel = await FillCreateProtocolsViewModel(createProtocolsViewModel.Protocol.ProtocolID, createProtocolsViewModel.Protocol.ProtocolTypeID);
                    createProtocolsViewModel.ErrorMessage = AppUtility.GetExceptionMessage(ex);
                    Response.StatusCode = 500;
                    await transaction.RollbackAsync();
                    //todo: delete the newly added documents
                    return PartialView("_CreateProtocol", createProtocolsViewModel);
                }

            }

        }

        [Authorize(Roles = "Protocols")]
        public async Task<IActionResult> _IndexTableWithEditProtocol(int protocolID)
        {
            CreateProtocolsViewModel viewmodel = await FillCreateProtocolsViewModel(0, protocolID);
            return PartialView(viewmodel);
        }


        [Authorize(Roles = "Protocols")]
        public async Task<IActionResult> KitProtocol()
        {
            TempData[AppUtility.TempDataTypes.MenuType.ToString()] = AppUtility.MenuItems.Protocols;
            TempData[AppUtility.TempDataTypes.SidebarType.ToString()] = AppUtility.SidebarEnum.KitProtocol;
            TempData[AppUtility.TempDataTypes.PageType.ToString()] = AppUtility.PageTypeEnum.ProtocolsCreate;
            CreateProtocolsViewModel viewmodel = await FillCreateProtocolsViewModel(2);
            return View(viewmodel);
        }

        [Authorize(Roles = "Protocols")]
        public async Task<IActionResult> SopProtocol()
        {
            TempData[AppUtility.TempDataTypes.MenuType.ToString()] = AppUtility.MenuItems.Protocols;
            TempData[AppUtility.TempDataTypes.SidebarType.ToString()] = AppUtility.SidebarEnum.SOPProtocol;
            TempData[AppUtility.TempDataTypes.PageType.ToString()] = AppUtility.PageTypeEnum.ProtocolsCreate;
            CreateProtocolsViewModel viewmodel = await FillCreateProtocolsViewModel(3);
            return View(viewmodel);
        }
        [Authorize(Roles = "Protocols")]
        public async Task<IActionResult> BufferCreating()
        {
            TempData[AppUtility.TempDataTypes.MenuType.ToString()] = AppUtility.MenuItems.Protocols;
            TempData[AppUtility.TempDataTypes.SidebarType.ToString()] = AppUtility.SidebarEnum.BufferCreating;
            TempData[AppUtility.TempDataTypes.PageType.ToString()] = AppUtility.PageTypeEnum.ProtocolsCreate;
            CreateProtocolsViewModel viewmodel = await FillCreateProtocolsViewModel(4);
            return View(viewmodel);
        }
        [Authorize(Roles = "Protocols")]
        public async Task<IActionResult> RobioticProtocol()
        {
            TempData[AppUtility.TempDataTypes.MenuType.ToString()] = AppUtility.MenuItems.Protocols;
            TempData[AppUtility.TempDataTypes.SidebarType.ToString()] = AppUtility.SidebarEnum.RoboticProtocol;
            TempData[AppUtility.TempDataTypes.PageType.ToString()] = AppUtility.PageTypeEnum.ProtocolsCreate;
            CreateProtocolsViewModel viewmodel = await FillCreateProtocolsViewModel(5);
            return View(viewmodel);
        }
        [Authorize(Roles = "Protocols")]
        public async Task<IActionResult> MaintenanceProtocol()
        {
            TempData[AppUtility.TempDataTypes.MenuType.ToString()] = AppUtility.MenuItems.Protocols;
            TempData[AppUtility.TempDataTypes.SidebarType.ToString()] = AppUtility.SidebarEnum.MaintenanceProtocol;
            TempData[AppUtility.TempDataTypes.PageType.ToString()] = AppUtility.PageTypeEnum.ProtocolsCreate;
            CreateProtocolsViewModel viewmodel = await FillCreateProtocolsViewModel(6);
            return View(viewmodel);
        }

        [Authorize(Roles = "Protocols")]
        public async Task<IActionResult> DailyReports()
        {
            TempData[AppUtility.TempDataTypes.MenuType.ToString()] = AppUtility.MenuItems.Protocols;
            TempData[AppUtility.TempDataTypes.SidebarType.ToString()] = AppUtility.SidebarEnum.DailyReports;
            TempData[AppUtility.TempDataTypes.PageType.ToString()] = AppUtility.PageTypeEnum.ProtocolsReports;
            return View();
        }
        [Authorize(Roles = "Protocols")]
        public async Task<IActionResult> ReportsCategories()
        {
            TempData[AppUtility.TempDataTypes.MenuType.ToString()] = AppUtility.MenuItems.Protocols;
            TempData[AppUtility.TempDataTypes.SidebarType.ToString()] = AppUtility.SidebarEnum.WeeklyReports;
            TempData[AppUtility.TempDataTypes.PageType.ToString()] = AppUtility.PageTypeEnum.ProtocolsReports;
            var user = await _userManager.GetUserAsync(User);
            var userRoles = await _userManager.GetRolesAsync(user);
            ReportCategoriesViewModel viewModel = new ReportCategoriesViewModel
            {
                PageType = AppUtility.PageTypeEnum.ProtocolsReports.ToString(),
                SidebarType = AppUtility.SidebarEnum.WeeklyReports.ToString(),
                ReportCategories = _context.ResourceCategories.Where(rc => rc.IsReportsCategory &&
                userRoles.Contains("Protocols" + rc.ResourceCategoryDescription.Replace(" ", ""))).ToList(),
            };
            return View(viewModel);
        }

        [Authorize(Roles = "Protocols")]
        public async Task<IActionResult> IndexReports(int ReportCategoryID, AppUtility.SidebarEnum SidebarType)
        {
            TempData[AppUtility.TempDataTypes.MenuType.ToString()] = AppUtility.MenuItems.Protocols;
            TempData[AppUtility.TempDataTypes.SidebarType.ToString()] = AppUtility.SidebarEnum.WeeklyReports;
            TempData[AppUtility.TempDataTypes.PageType.ToString()] = AppUtility.PageTypeEnum.ProtocolsReports;
            ReportsIndexObject reportsIndexObject = new ReportsIndexObject()
            {
                PageType = AppUtility.PageTypeEnum.ProtocolsReports,
                SidebarType = SidebarType,
                ReportCategoryID = ReportCategoryID

            };
            var reportsIndexViewModel = await GetReportsIndexViewModel(reportsIndexObject);
            var currentWeek = CultureInfo.CurrentCulture.Calendar.GetWeekOfYear(DateTime.Now, CalendarWeekRule.FirstFullWeek, DayOfWeek.Sunday);
            var currentWeekReport = _context.Reports.Where(r => r.WeekNumber == currentWeek && r.DateCreated.Year == DateTime.Now.Year && r.ReportCategoryID == ReportCategoryID).FirstOrDefault();
            if (currentWeekReport != null)
            {
                reportsIndexViewModel.CurrentReportCreated = true;
            }
            return View(reportsIndexViewModel);
        }

        public async Task<IActionResult> _ReportsIndexTable(ReportsIndexObject reportsIndex)
        {
            var reportsIndexViewModel = await GetReportsIndexViewModel(reportsIndex);

            return PartialView(reportsIndexViewModel);
        }

        [Authorize(Roles = "Protocols")]
        public async Task<IActionResult> MonthlyReports()
        {
            TempData[AppUtility.TempDataTypes.MenuType.ToString()] = AppUtility.MenuItems.Protocols;
            TempData[AppUtility.TempDataTypes.SidebarType.ToString()] = AppUtility.SidebarEnum.MonthlyReports;
            TempData[AppUtility.TempDataTypes.PageType.ToString()] = AppUtility.PageTypeEnum.ProtocolsReports;
            return View();
        }

        [Authorize(Roles = "Protocols")]
        public async Task<IActionResult> ReportsSharedWithMe()
        {
            TempData[AppUtility.TempDataTypes.MenuType.ToString()] = AppUtility.MenuItems.Protocols;
            TempData[AppUtility.TempDataTypes.SidebarType.ToString()] = AppUtility.SidebarEnum.SharedWithMe;
            TempData[AppUtility.TempDataTypes.PageType.ToString()] = AppUtility.PageTypeEnum.ProtocolsReports;
            return View();
        }

        [Authorize(Roles = "Protocols")]
        public async Task<IActionResult> Library(int? CategoryType = 1)
        {
            TempData[AppUtility.TempDataTypes.MenuType.ToString()] = AppUtility.MenuItems.Protocols;
            TempData[AppUtility.TempDataTypes.SidebarType.ToString()] = AppUtility.SidebarEnum.Library;
            TempData[AppUtility.TempDataTypes.PageType.ToString()] = AppUtility.PageTypeEnum.ProtocolsResources;

            var resourceLibraryViewModel = new ResourceLibraryViewModel();

            //switch (CategoryType)
            //{
            //    case 2:
            //        resourceLibraryViewModel.PageType = 2;
            //        resourceLibraryViewModel.ResourceCategories = _context.ResourceCategories.Where(rc => rc.IsResourceType == true);
            //        break;
            //    case 1:
            //    default:
            //        resourceLibraryViewModel.PageType = 1;
            //        resourceLibraryViewModel.ResourceCategories = _context.ResourceCategories.Where(rc => rc.IsResourceType != true);
            //        break;
            //}

            resourceLibraryViewModel.ResourceCategories = _context.ResourceCategories;
            return View(resourceLibraryViewModel);
        }

        [HttpGet]
        [Authorize(Roles = "Protocols")]
        public async Task<IActionResult> ResourcesList(int? ResourceCategoryID)
        {
            TempData[AppUtility.TempDataTypes.MenuType.ToString()] = AppUtility.MenuItems.Protocols;
            TempData[AppUtility.TempDataTypes.SidebarType.ToString()] = AppUtility.SidebarEnum.Library;
            TempData[AppUtility.TempDataTypes.PageType.ToString()] = AppUtility.PageTypeEnum.ProtocolsResources;

            ResourcesListIndexViewModel ResourcesListIndexViewModel = new ResourcesListIndexViewModel() { IsFavoritesPage = false };

            ResourcesListIndexViewModel.ResourcesWithFavorites = _context.Resources
                .Include(r => r.FavoriteResources)
                .Include(r => r.ResourceResourceCategories).ThenInclude(rrc => rrc.ResourceCategory)
                .Where(r => r.ResourceResourceCategories.Any(rrc => rrc.ResourceCategoryID == ResourceCategoryID))
                .Select(r => new ResourceWithFavorite
                {
                    Resource = r,
                    IsFavorite = r.FavoriteResources.Any(fr => fr.ApplicationUserID == _userManager.GetUserId(User))
                }).ToList();
            ResourcesListIndexViewModel.SidebarEnum = AppUtility.SidebarEnum.Library;
            ResourcesListIndexViewModel.IconColumnViewModels = GetIconColumnViewModels(new List<IconNamesEnumWithList>()
            {
                new IconNamesEnumWithList(){ IconNamesEnum = AppUtility.IconNamesEnum.Favorite },
                new IconNamesEnumWithList(){ IconNamesEnum = AppUtility.IconNamesEnum.Share },
                new IconNamesEnumWithList(){ IconNamesEnum = AppUtility.IconNamesEnum.Edit }
            });
            ResourcesListViewModel resourcesListViewModel = new ResourcesListViewModel()
            {
                ResourcesListIndexViewModel = ResourcesListIndexViewModel,
                PaginationTabs = new List<string>() { "Library", _context.ResourceCategories.Where(rc => rc.ResourceCategoryID == ResourceCategoryID).FirstOrDefault().ResourceCategoryDescription }
            };


            return View(resourcesListViewModel);
        }

        [HttpGet]
        [HttpPost]
        [Authorize(Roles = "Protocols")]
        public async Task<IActionResult> _ResourcesListIndex(ResourcesListIndexViewModel ResourcesListIndexViewModel = null, AppUtility.SidebarEnum? sidebarEnum = null, bool IsFavorites = false, bool IsShared = false)
        {
            if (sidebarEnum == AppUtility.SidebarEnum.Favorites)
            {
                ResourcesListIndexViewModel = GetFavoritesResourceListIndexViewModel();
            }
            else if (sidebarEnum == AppUtility.SidebarEnum.SharedWithMe)
            {
                ResourcesListIndexViewModel = await GetResourcesSharedWithMe();
            }
            return PartialView(ResourcesListIndexViewModel);
        }

        [HttpGet]
        [Authorize(Roles = "Protocols")]
        public async Task<IActionResult> ResourceNotesModal(int ResourceID)
        {
            var resourceNote = new ResourceNote() { ResourceID = ResourceID };
            return PartialView(resourceNote);
        }

        [HttpPost]
        [Authorize(Roles = "Protocols")]
        public async Task<bool> ResourceNotesModal(ResourceNote ResourceNote)
        {
            bool error = false;
            using (var transaction = _context.Database.BeginTransaction())
            {
                try
                {
                    _context.Update(ResourceNote);
                    await _context.SaveChangesAsync();
                    transaction.Commit();
                }
                catch (Exception e)
                {
                    error = true;
                    transaction.Rollback();
                }
            }
            return error;
        }

        [HttpGet]
        [Authorize(Roles = "Protocols")]
        public async Task<IActionResult> FavoriteResources(int ResourceID, bool Favorite = true)
        {
            //The system for checks is strict b/c the calls are dependent upon icon names in code and jquery that can break or be changed one day
            string retString = null;
            using (var transaction = _context.Database.BeginTransaction())
            {
                try
                {
                    if (Favorite)
                    {
                        FavoriteResource favoriteResource = _context.FavoriteResources.Where(fr => fr.ResourceID == ResourceID && fr.ApplicationUserID == _userManager.GetUserId(User)).FirstOrDefault();
                        if (favoriteResource != null) { _context.Remove(favoriteResource); } //check is here so it doesn't crash
                                                                                             //if it doesn't exist the jquery will then cont and leave an empty icon which is ok b/c its empty
                    }
                    else
                    {
                        //check for favorite
                        if (_context.FavoriteResources.Where(fr => fr.ResourceID == ResourceID && fr.ApplicationUserID == _userManager.GetUserId(User)) != null)
                        {
                            FavoriteResource favoriteResource = new FavoriteResource()
                            {
                                ResourceID = ResourceID,
                                ApplicationUserID = _userManager.GetUserId(User)
                            };
                            _context.Update(favoriteResource);
                        }
                        //if the favorite exists the jquery will then cont and leave a full icon which is ok b/c its full
                    }
                    await _context.SaveChangesAsync();
                    transaction.Commit();
                }
                catch (Exception e)
                {
                    transaction.Rollback();
                }
            }
            return new EmptyResult();
        }

        [HttpGet]
        [Authorize(Roles = "Protocols")]

        public async Task<IActionResult> AddResource(int? resourceType = 1)
        {

            var rc = _context.ResourceCategories.Where(rc => !rc.IsResourceType).ToList();
            ResourceCategoriesToAdd[] resourceCategoriesToAdds = new ResourceCategoriesToAdd[rc.Count];
            var addResourceViewModel = new AddResourceViewModel()
            {
                //ResourceType = Convert.ToInt32(resourceType),
                ResourceCategories = rc,
                ResourceCategoriesToAdd = resourceCategoriesToAdds,
                Resource = new Resource() { ResourceTypeID = Convert.ToInt32(resourceType) }
            };

            return PartialView(addResourceViewModel);
        }

        [HttpPost]
        [Authorize(Roles = "Protocols")]
        public async Task<IActionResult> AddResource(AddResourceViewModel addResourceViewModel)
        {
            using (var transaction = _context.Database.BeginTransaction())
            {
                try
                {
                    _context.Update(addResourceViewModel.Resource);
                    await _context.SaveChangesAsync();

                    var resourceCategoriesToAdd = addResourceViewModel.ResourceCategoriesToAdd.Where(rc => rc.Added).Select(rc => rc.ResourceCategoryID).ToList();
                    //var resourceCategoriesAdded = _context.ResourceCategories.Where(rc1 => resourceCategoriesToAdd.Contains(rc1.ResourceCategoryID)).ToList();
                    foreach (var resourceCategoryID in resourceCategoriesToAdd)
                    {
                        var resourceResourceCategory = new ResourceResourceCategory()
                        {
                            ResourceID = addResourceViewModel.Resource.ResourceID,
                            ResourceCategoryID = resourceCategoryID
                        };
                        _context.Add(resourceResourceCategory);
                    }
                    await _context.SaveChangesAsync(); //adding join table instances
                    await transaction.CommitAsync();

                    string uploadFolder = Path.Combine(_hostingEnvironment.WebRootPath, "ResourceImages");
                    string folder = Path.Combine(uploadFolder, addResourceViewModel.Resource.ResourceID.ToString());
                    Directory.CreateDirectory(folder);
                    if (addResourceViewModel.ResourceImage != null) //test for more than one???
                    {
                        string uniqueFileName = addResourceViewModel.ResourceImage.FileName;
                        string filePath = Path.Combine(folder, uniqueFileName);
                        FileStream filestream = new FileStream(filePath, FileMode.Create);
                        addResourceViewModel.ResourceImage.CopyTo(filestream);
                        filestream.Close();
                    }
                }
                catch (Exception e)
                {
                    await transaction.RollbackAsync();
                    //unsave file -- I saved file last so won't crash after the file save
                }

            }
            return RedirectToAction("Library");
        }

        [Authorize(Roles = "Protocols")]
        public async Task<JsonResult> GetPubMedFromAPI(String PubMedID)
        {
            var ResourceVM = AppUtility.GetResourceArticleFromNCBIPubMed(PubMedID);
            return Json(ResourceVM);
        }

        [Authorize(Roles = "Protocols")]
        public async Task<IActionResult> Personal()
        {
            TempData[AppUtility.TempDataTypes.MenuType.ToString()] = AppUtility.MenuItems.Protocols;
            TempData[AppUtility.TempDataTypes.SidebarType.ToString()] = AppUtility.SidebarEnum.Personal;
            TempData[AppUtility.TempDataTypes.PageType.ToString()] = AppUtility.PageTypeEnum.ProtocolsResources;
            return View();
        }

        [Authorize(Roles = "Protocols")]
        public async Task<IActionResult> ResourcesSharedWithMe()
        {
            TempData[AppUtility.TempDataTypes.MenuType.ToString()] = AppUtility.MenuItems.Protocols;
            TempData[AppUtility.TempDataTypes.SidebarType.ToString()] = AppUtility.SidebarEnum.SharedWithMe;
            TempData[AppUtility.TempDataTypes.PageType.ToString()] = AppUtility.PageTypeEnum.ProtocolsResources;

            return View(await GetResourcesSharedWithMe());
        }

        [Authorize(Roles = "Protocols")]
        public async Task<ResourcesListIndexViewModel> GetResourcesSharedWithMe()
        {
            ResourcesListIndexViewModel ResourcesListIndexViewModel = new ResourcesListIndexViewModel() { IsFavoritesPage = false };

            var shareresourcesreceivedResoureid = _context.Users.Include(u => u.ShareResourcesReceived)
                .Where(u => u.Id == _userManager.GetUserId(User)).FirstOrDefault()
                .ShareResourcesReceived.Select(srr => srr.ResourceID).ToList();

            var testNew = _context.Resources
            .Include(r => r.FavoriteResources)
            .Include(r => r.ResourceResourceCategories).ThenInclude(rrc => rrc.ResourceCategory)
            .Where(r => shareresourcesreceivedResoureid.Contains(r.ResourceID));

            ResourcesListIndexViewModel.SidebarEnum = AppUtility.SidebarEnum.SharedWithMe;
            ResourcesListIndexViewModel.ResourcesWithFavorites = testNew.Select(r => new ResourceWithFavorite
            {
                Resource = r,
                IsFavorite = r.FavoriteResources.Any(fr => fr.ApplicationUserID == _userManager.GetUserId(User))
            }).ToList();

            var tempResourcesWithFavorites = from r in _context.Resources
                                             join sr in _context.ShareResources on r.ResourceID equals sr.ResourceID
                                             join fr in _context.FavoriteResources on r.ResourceID equals fr.ResourceID into g
                                             from fr in g.DefaultIfEmpty()
                                             where sr.ToApplicationUserID == _userManager.GetUserId(User)
                                             select new ResourceWithFavorite { Resource = r, IsFavorite = fr.FavoriteResourceID == null ? false : true, SharedByApplicationUser = sr.FromApplicationUser, ShareResourceID = sr.ShareResourceID };

            ResourcesListIndexViewModel.ResourcesWithFavorites = tempResourcesWithFavorites.ToList();
            ResourcesListIndexViewModel.IconColumnViewModels = GetIconColumnViewModels(new List<IconNamesEnumWithList>()
            {
                new IconNamesEnumWithList(){ IconNamesEnum = AppUtility.IconNamesEnum.Favorite },
                new IconNamesEnumWithList(){ IconNamesEnum = AppUtility.IconNamesEnum.Share },
                new IconNamesEnumWithList(){ IconNamesEnum = AppUtility.IconNamesEnum.Edit },
                new IconNamesEnumWithList(){ IconNamesEnum = AppUtility.IconNamesEnum.MorePopover, IconNamesEnums = new List<AppUtility.IconNamesEnum>(){ AppUtility.IconNamesEnum.RemoveShare } }
            });

            return ResourcesListIndexViewModel;
        }

        [Authorize(Roles = "Protocols")]
        public async Task<IActionResult> ResourcesFavorites()
        {
            TempData[AppUtility.TempDataTypes.MenuType.ToString()] = AppUtility.MenuItems.Protocols;
            TempData[AppUtility.TempDataTypes.SidebarType.ToString()] = AppUtility.SidebarEnum.Favorites;
            TempData[AppUtility.TempDataTypes.PageType.ToString()] = AppUtility.PageTypeEnum.ProtocolsResources;

            return View(GetFavoritesResourceListIndexViewModel());
        }


        [Authorize(Roles = "Protocols")]
        public ResourcesListIndexViewModel GetFavoritesResourceListIndexViewModel()
        {
            ResourcesListIndexViewModel ResourcesListIndexViewModel = new ResourcesListIndexViewModel();
            ResourcesListIndexViewModel.ResourcesWithFavorites = _context.FavoriteResources
                .Include(fr => fr.Resource).ThenInclude(r => r.ResourceResourceCategories).ThenInclude(rrc => rrc.ResourceCategory)
                .Where(fr => fr.ApplicationUserID == _userManager.GetUserId(User))
                .Select(fr => new ResourceWithFavorite
                {
                    Resource = fr.Resource,
                    IsFavorite = true
                }).ToList();
            ResourcesListIndexViewModel.IsFavoritesPage = true;
            ResourcesListIndexViewModel.SidebarEnum = AppUtility.SidebarEnum.Favorites;
            ResourcesListIndexViewModel.IconColumnViewModels = GetIconColumnViewModels(new List<IconNamesEnumWithList>()
            {
                new IconNamesEnumWithList(){ IconNamesEnum = AppUtility.IconNamesEnum.Favorite },
                new IconNamesEnumWithList(){ IconNamesEnum = AppUtility.IconNamesEnum.Share },
                new IconNamesEnumWithList(){ IconNamesEnum = AppUtility.IconNamesEnum.Edit }
            });
            return ResourcesListIndexViewModel;
        }

        [Authorize(Roles = "Protocols")]
        public async Task<IActionResult> Active()
        {
            TempData[AppUtility.TempDataTypes.MenuType.ToString()] = AppUtility.MenuItems.Protocols;
            TempData[AppUtility.TempDataTypes.SidebarType.ToString()] = AppUtility.SidebarEnum.Active;
            TempData[AppUtility.TempDataTypes.PageType.ToString()] = AppUtility.PageTypeEnum.ProtocolsTask;
            return View();
        }


        [Authorize(Roles = "Protocols")]
        public async Task<IActionResult> Done()
        {
            TempData[AppUtility.TempDataTypes.MenuType.ToString()] = AppUtility.MenuItems.Protocols;
            TempData[AppUtility.TempDataTypes.SidebarType.ToString()] = AppUtility.SidebarEnum.Done;
            TempData[AppUtility.TempDataTypes.PageType.ToString()] = AppUtility.PageTypeEnum.ProtocolsTask;
            return View();

        }

        [HttpGet]
        [Authorize(Roles = "Protocols")]
        public ActionResult DocumentsModal(int? id, AppUtility.FolderNamesEnum RequestFolderNameEnum, bool IsEdittable, bool showSwitch,
    AppUtility.MenuItems SectionType = AppUtility.MenuItems.Protocols, AppUtility.ParentFolderName parentFolderName = AppUtility.ParentFolderName.Protocols)
        {
            DocumentsModalViewModel documentsModalViewModel = new DocumentsModalViewModel()
            {
                FolderName = RequestFolderNameEnum,
                ParentFolderName = parentFolderName,
                ObjectID = id ?? 0,
                SectionType = SectionType,
                IsEdittable = true
            };

            base.FillDocumentsViewModel(documentsModalViewModel);
            return PartialView(documentsModalViewModel);
        }

        [Authorize(Roles = "Protocols")]
        public async Task<IActionResult> Search()
        {
            TempData[AppUtility.TempDataTypes.MenuType.ToString()] = AppUtility.MenuItems.Protocols;
            TempData[AppUtility.TempDataTypes.SidebarType.ToString()] = AppUtility.SidebarEnum.Search;
            TempData[AppUtility.TempDataTypes.PageType.ToString()] = AppUtility.PageTypeEnum.ProtocolsSearch;
            return View();
        }


        [HttpGet] //send a json to that the subcategory list is filtered
        [Authorize(Roles = "Protocols")]
        public JsonResult GetSubCategoryList(int? ParentCategoryId)
        {
            var subCategoryList = _context.ProtocolSubCategories.ToList();
            if (ParentCategoryId != null)
            {
                subCategoryList = _context.ProtocolSubCategories.Where(c => c.ProtocolCategoryTypeID == ParentCategoryId).ToList();
            }
            return Json(subCategoryList);
        }

        [HttpPost]
        [Authorize(Roles = "Protocols")]
        public void DocumentsModal(/*[FromBody]*/ DocumentsModalViewModel documentsModalViewModel)
        {
            base.DocumentsModal(documentsModalViewModel);
        }


        [Authorize(Roles = "Protocols")]
        private void FillDocumentsInfo(CreateProtocolsViewModel createProtocolsViewModel, string uploadFolder)
        {
            createProtocolsViewModel.DocumentsInfo = new List<DocumentFolder>();

            base.GetExistingFileStrings(createProtocolsViewModel.DocumentsInfo, AppUtility.FolderNamesEnum.Info, uploadFolder);
            base.GetExistingFileStrings(createProtocolsViewModel.DocumentsInfo, AppUtility.FolderNamesEnum.Pictures, uploadFolder);
        }
        [Authorize(Roles = "Protocols")]
        public async void RemoveShare(int ShareID, AppUtility.ModelsEnum modelsEnum)
        {
            using (var transaction = _context.Database.BeginTransaction())
            {
                try
                {
                    switch (modelsEnum)
                    {
                        case AppUtility.ModelsEnum.Resource:
                            var sharedResource = _context.ShareResources.Where(sr => sr.ShareResourceID == ShareID).FirstOrDefault();
                            _context.Remove(sharedResource);
                            break;
                    }
                    _context.SaveChanges();
                    transaction.Commit();
                }
                catch (Exception ex)
                {
                    transaction.Rollback();
                }
            }
        }

        [Authorize(Roles = "Protocols")]
        public async Task<IActionResult> ShareModal(int ID, AppUtility.ModelsEnum ModelsEnum)
        {
            ShareModalViewModel shareModalViewModel = base.GetShareModalViewModel(ID, ModelsEnum);
            shareModalViewModel.MenuItem = AppUtility.MenuItems.Protocols;
            switch (ModelsEnum)
            {
                case AppUtility.ModelsEnum.Resource:
                    shareModalViewModel.ObjectDescription = _context.Resources.Where(r => r.ResourceID == ID).FirstOrDefault().Title;
                    break;
            }
            return PartialView(shareModalViewModel);
        }

        [HttpPost]
        [Authorize(Roles = "Protocols")]
        public async Task<bool> ShareModal(ShareModalViewModel shareModalViewModel)
        {
            var currentUserID = _userManager.GetUserId(User);
            bool error = false;
            using (var transaction = _context.Database.BeginTransaction())
            {
                try
                {
                    foreach (var userID in shareModalViewModel.ApplicationUserIDs)
                    {

                        switch (shareModalViewModel.ModelsEnum)
                        {
                            case AppUtility.ModelsEnum.Resource:
                                var PrevSharedResource = _context.ShareResources
                                    .Where(sr => sr.ResourceID == shareModalViewModel.ID && sr.FromApplicationUserID == currentUserID && sr.ToApplicationUserID == userID).FirstOrDefault();
                                if (PrevSharedResource != null)
                                {
                                    PrevSharedResource.TimeStamp = DateTime.Now;
                                    _context.Update(PrevSharedResource);
                                }
                                else
                                {
                                    var shareResource = new ShareResource()
                                    {
                                        ResourceID = shareModalViewModel.ID,
                                        FromApplicationUserID = currentUserID,
                                        ToApplicationUserID = userID,
                                        TimeStamp = DateTime.Now
                                    };
                                    _context.Update(shareResource);
                                }
                                break;
                        }
                    }
                    await _context.SaveChangesAsync();
                    await transaction.CommitAsync();
                }
                catch (Exception ex)
                {
                    error = true;
                }
            }

            return error;
        }

        public List<IconColumnViewModel> GetIconColumnViewModels(List<IconNamesEnumWithList> iconNamesEnumWithLists) //MUST USE THIS OVERRIDE WHEN FAVORITES ARE INCLUDED
        {
            var iconColumnViewModels = new List<IconColumnViewModel>();
            var editIcon = new IconColumnViewModel("icon-create-24px", null, "edit", "Edit");
            var favoriteIcon = new IconColumnViewModel(AppUtility.FavoriteIcons().Where(fi => fi.StringName == AppUtility.FavoriteIconTitle.Empty.ToString()).FirstOrDefault().StringDefinition, null, "favorite", "Favorite");
            var shareIcon = new IconColumnViewModel("icon-share-24px1", null, "share", "Share");
            var moreIcon = new IconColumnViewModel("icon-more_vert-24px", null, "popover-more", "More");

            var removeShareIcon = new IconPopoverViewModel("icon-share-24px1", "black", AppUtility.PopoverDescription.RemoveShare, ajaxcall: "remove-share");

            foreach (var iconNameEnum in iconNamesEnumWithLists)
            {
                switch (iconNameEnum.IconNamesEnum)
                {
                    case AppUtility.IconNamesEnum.Edit:
                        iconColumnViewModels.Add(editIcon);
                        break;
                    case AppUtility.IconNamesEnum.Favorite:
                        iconColumnViewModels.Add(favoriteIcon);
                        break;
                    case AppUtility.IconNamesEnum.Share:
                        iconColumnViewModels.Add(shareIcon);
                        break;
                    case AppUtility.IconNamesEnum.MorePopover:
                        var popoverMoreIcon = moreIcon;
                        popoverMoreIcon.IconPopovers = new List<IconPopoverViewModel>();
                        foreach (var iconPopoverName in iconNameEnum.IconNamesEnums)
                        {
                            switch (iconPopoverName)
                            {
                                case AppUtility.IconNamesEnum.RemoveShare:
                                    popoverMoreIcon.IconPopovers.Add(removeShareIcon);
                                    break;
                            }
                        }
                        //var popoverShare = new IconPopoverViewModel("icon-share-24px1", "black", AppUtility.PopoverDescription.Share, "ShareRequest", "Requests", AppUtility.PopoverEnum.None, "share-request-fx");
                        //popoverMoreIcon.IconPopovers = new List<IconPopoverViewModel>() { popoverShare };
                        iconColumnViewModels.Add(popoverMoreIcon);
                        break;
                };
            }

            return iconColumnViewModels;
        }

        [HttpGet]
        [Authorize(Roles = "Protocols")]
        public async Task<IActionResult> NewReportModal(int reportCategoryId, AppUtility.SidebarEnum sidebarType)
        {
            var reportTypeID = 0;
            switch (sidebarType)
            {
                case AppUtility.SidebarEnum.DailyReports:
                    reportTypeID = 1;
                    break;
                case AppUtility.SidebarEnum.WeeklyReports:
                    reportTypeID = 2;
                    break;
                case AppUtility.SidebarEnum.MonthlyReports:
                    reportTypeID = 3;
                    break;
            }

            var reportType = _context.ReportTypes.Where(rt => rt.ReportTypeID == reportTypeID).FirstOrDefault();
            var report = new Report()
            {
                DateCreated = DateTime.Now,
                ReportCategoryID = reportCategoryId,
                ReportType = reportType
            };

            CreateReportViewModel createReportViewModel = new CreateReportViewModel()
            {
                Report = report
            };

            return PartialView(createReportViewModel);
        }

        [HttpPost]
        [Authorize(Roles = "Protocols")]
        public async Task<IActionResult> NewReportModal(CreateReportViewModel createReportViewModel)
        {
            TempData[AppUtility.TempDataTypes.MenuType.ToString()] = AppUtility.MenuItems.Protocols;
            TempData[AppUtility.TempDataTypes.SidebarType.ToString()] = AppUtility.SidebarEnum.WeeklyReports;
            TempData[AppUtility.TempDataTypes.PageType.ToString()] = AppUtility.PageTypeEnum.ProtocolsReports;

            using (var transaction = _context.Database.BeginTransaction())
            {
                try
                {
                    var report = createReportViewModel.Report;
                    report.WeekNumber = CultureInfo.CurrentCulture.Calendar.GetWeekOfYear(report.DateCreated, CalendarWeekRule.FirstFullWeek, DayOfWeek.Sunday);

                    var reportNumber = "WR";
                    var lastReportNumber = _context.Reports.OrderByDescending(r => r.ReportNumber).FirstOrDefault()?.ReportNumber.Substring(2);
                    if(lastReportNumber == null)
                    {
                        reportNumber += 1;
                    }
                    else
                    {
                        reportNumber += (Int32.Parse(lastReportNumber) + 1);
                    }

                    report.ReportNumber = reportNumber;
                    _context.Update(report);
                    await _context.SaveChangesAsync();

                    createReportViewModel.ReportID = report.ReportID;

                    var functionTypes = new List<FunctionType>();
                    foreach (var functionType in Enum.GetValues(typeof(AppUtility.ReportsFunctionTypes)))
                    {
                        functionTypes.Add(_context.FunctionTypes.Where(ft => ft.DescriptionEnum == functionType.ToString()).FirstOrDefault());
                    }
                    createReportViewModel.FunctionTypes = functionTypes;
                    createReportViewModel.ReportDateRange = AppUtility.GetWeekStartEndDates(createReportViewModel.Report.DateCreated);

                    await transaction.CommitAsync();
                }
                catch (Exception ex)
                {
                    Response.StatusCode = 500;
                    await transaction.RollbackAsync();
                }
            }

            return View("CreateReport", createReportViewModel);
        }

        [HttpGet]
        [Authorize(Roles = "Protocols")]
        public async Task<IActionResult> CreateReport(int reportID)
        {
            TempData[AppUtility.TempDataTypes.MenuType.ToString()] = AppUtility.MenuItems.Protocols;
            TempData[AppUtility.TempDataTypes.SidebarType.ToString()] = AppUtility.SidebarEnum.WeeklyReports;
            TempData[AppUtility.TempDataTypes.PageType.ToString()] = AppUtility.PageTypeEnum.ProtocolsReports;

            var functionTypes = new List<FunctionType>();
            foreach (var functionType in Enum.GetValues(typeof(AppUtility.ReportsFunctionTypes)))
            {
                functionTypes.Add(_context.FunctionTypes.Where(ft => ft.DescriptionEnum == functionType.ToString()).FirstOrDefault());
            }
            var report = _context.Reports.Where(r => r.ReportID == reportID).FirstOrDefault();
            var createReportViewModel = new CreateReportViewModel()
            {
                Report = report,
                FunctionTypes = functionTypes,
                ReportDateRange = AppUtility.GetWeekStartEndDates(report.DateCreated)
            };

            return View(createReportViewModel);
        }

        [Authorize(Roles = "Protocols")]
        public async Task<IActionResult> AddReportFunctionModal(int FunctionTypeID, int ReportID, string reportTempText)
        {
            var functionType = _context.FunctionTypes.Where(ft => ft.FunctionTypeID == FunctionTypeID).FirstOrDefault();
            var viewmodel = new AddReportFunctionViewModel
            {
                FunctionType = functionType,
                ReportID = ReportID,
                ReportTempText = reportTempText
            };

            switch (Enum.Parse<AppUtility.ProtocolFunctionTypes>(functionType.DescriptionEnum))
            {
                case AppUtility.ProtocolFunctionTypes.AddLinkToProduct:
                    viewmodel.ParentCategories = _context.ParentCategories.ToList();
                    viewmodel.ProductSubcategories = _context.ProductSubcategories.ToList();
                    viewmodel.Products = _context.Products.ToList();
                    viewmodel.Vendors = _context.Vendors.ToList();
                    break;
                case AppUtility.ProtocolFunctionTypes.AddLinkToProtocol:
                    viewmodel.ProtocolCategories = _context.ProtocolCategories.ToList();
                    viewmodel.ProtocolSubCategories = _context.ProtocolSubCategories.ToList();
                    viewmodel.Creators = _context.Users.Select(u =>
                        new SelectListItem() { Value = u.Id, Text = u.FirstName + u.LastName }).ToList();
                    viewmodel.Protocols = _context.Protocols.ToList();
                    break;
            }
            return PartialView(viewmodel);
        }

        [HttpPost]
        [Authorize(Roles = "Protocols")]
        public async Task<IActionResult> AddReportFunctionModal(AddReportFunctionViewModel addReportsFunctionViewModel)
        {
            var functionType = _context.FunctionTypes.Where(ft => ft.FunctionTypeID == addReportsFunctionViewModel.FunctionType.FunctionTypeID).FirstOrDefault();
            var report = _context.Reports.Where(r => r.ReportID == addReportsFunctionViewModel.ReportID).FirstOrDefault();
            
            using (var transaction = _context.Database.BeginTransaction())
            {
                try
                {
                    switch (Enum.Parse<AppUtility.ProtocolFunctionTypes>(functionType.DescriptionEnum))
                    {
                        //case AppUtility.ProtocolFunctionTypes.AddLinkToProduct:
                        //    var product = _context.Products.Where(p => p.ProductID == addFunctionViewModel.FunctionLine.ProductID).FirstOrDefault();
                        //    line.Content += "<a href='#' class='open-line-product'>" + product.ProductName + "</>";
                        //    break;
                        //case AppUtility.ProtocolFunctionTypes.AddLinkToProtocol:
                        //    var protocol = _context.Protocols.Include(p => p.Materials).Where(p => p.ProtocolID == addFunctionViewModel.FunctionLine.ProtocolID).FirstOrDefault();
                        //    line.Content += "<a href='#' class='open-line-protocol'>" + protocol.Name + "</>";
                        //    break;
                        case AppUtility.ProtocolFunctionTypes.AddFile:
                        case AppUtility.ProtocolFunctionTypes.AddImage:
                            MoveDocumentsOutOfTempFolder(addReportsFunctionViewModel.ReportID, AppUtility.ParentFolderName.Reports);
                            report.TemporaryReportText = addReportsFunctionViewModel.ReportTempText + " <a href='#' class='open-document-modal mark-edditable' data-string='@AppUtility.FolderNamesEnum.Files.ToString() "
                                                + "data-id = '@Model.ReportID' "
                                                + "id = '@AppUtility.FolderNamesEnum.Files.ToString()' parentFolder = '@AppUtility.ParentFolderName.Reports' data-val = '@true' show-switch= '@false' >" + addReportsFunctionViewModel.FileName + "</>";
                            _context.Update(report);
                            await _context.SaveChangesAsync();
                            break;
                    }
                    await transaction.CommitAsync();
                }
                catch (Exception ex)
                {
                    Response.StatusCode = 500;
                    await transaction.RollbackAsync();
                }
            }
            return PartialView("_ReportText", report);
        }
    }

}
