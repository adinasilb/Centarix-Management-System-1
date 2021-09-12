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
using Microsoft.AspNetCore.Mvc.ViewFeatures;
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
using System.Text;
using System.Threading.Tasks;
using System.Xml;
using X.PagedList;

namespace PrototypeWithAuth.Controllers
{
    public class ProtocolsController : SharedController
    {
        public enum ProtocolIconNamesEnum { Share, Favorite, MorePopover, Edit, RemoveShare }
        public ProtocolsController(ApplicationDbContext context, UserManager<ApplicationUser> userManager, IHostingEnvironment hostingEnvironment, ICompositeViewEngine viewEngine, IHttpContextAccessor httpContextAccessor)
            : base(context, userManager, hostingEnvironment, viewEngine, httpContextAccessor)
        {
        }

        [Authorize(Roles = "Protocols")]
        public async Task<IActionResult> Index()
        {
            TempData[AppUtility.TempDataTypes.MenuType.ToString()] = AppUtility.MenuItems.Protocols;
            TempData[AppUtility.TempDataTypes.SidebarType.ToString()] = AppUtility.SidebarEnum.List;
            TempData[AppUtility.TempDataTypes.PageType.ToString()] = AppUtility.PageTypeEnum.ProtocolsProtocols;
            var viewmodel = await GetProtocolsIndexViewModelAsync(new ProtocolsIndexObject() { });

            return View(viewmodel);
        }

        [Authorize(Roles = "Protocols")]
        private static IQueryable<ProtocolVersion> filterListBySelectFilters(SelectedProtocolsFilters selectedFilters, IQueryable<ProtocolVersion> fullRequestsListProprietary)
        {
            if (selectedFilters != null)
            {
                if (selectedFilters.SelectedCategoriesIDs.Count() > 0)
                {
                    fullRequestsListProprietary = fullRequestsListProprietary.Where(p => selectedFilters.SelectedCategoriesIDs.Contains(p.Protocol.ProtocolSubCategory.ProtocolCategoryTypeID));
                }
                if (selectedFilters.SelectedProtocolsSubcategoriesIDs.Count() > 0)
                {
                    fullRequestsListProprietary = fullRequestsListProprietary.Where(p => selectedFilters.SelectedProtocolsSubcategoriesIDs.Contains(p.Protocol.ProtocolSubCategoryID));
                }
                if (selectedFilters.SelectedOwnersIDs.Count() > 0)
                {
                    fullRequestsListProprietary = fullRequestsListProprietary.Where(p => selectedFilters.SelectedOwnersIDs.Contains(p.ApplicationUserCreatorID));
                }
            }

            return fullRequestsListProprietary;
        }

        [Authorize(Roles = "Protocols")]
        private async Task<ProtocolsIndexViewModel> GetProtocolsIndexViewModelAsync(ProtocolsIndexObject protocolsIndexObject, SelectedProtocolsFilters selectedFilters = null)
        {
            IQueryable<ProtocolVersion> ProtocolsPassedIn = Enumerable.Empty<ProtocolVersion>().AsQueryable();
            IQueryable<ProtocolVersion> fullProtocolsList = _context.ProtocolVersions.Include(p=>p.Protocol).Include(p => p.ApplicationUserCreator).Include(p => p.Protocol.ProtocolSubCategory)
                .ThenInclude(p => p.ProtocolCategoryType).Include(p => p.Protocol.ProtocolType).Include(p => p.ProtocolInstances);
            var user = await _userManager.GetUserAsync(User);
            switch (protocolsIndexObject.PageType)
            {
                case AppUtility.PageTypeEnum.ProtocolsProtocols:
                    switch (protocolsIndexObject.SidebarType)
                    {
                        case AppUtility.SidebarEnum.List:
                            break;
                        case AppUtility.SidebarEnum.MyProtocols:
                            fullProtocolsList = fullProtocolsList.Where(fl => fl.ApplicationUserCreatorID == user.Id);
                            break;
                        case AppUtility.SidebarEnum.Favorites:
                            var usersFavoriteProtocols = _context.FavoriteProtocols.Where(fr => fr.ApplicationUserID == _userManager.GetUserId(User))
                    .Select(fr => fr.ProtocolVersionID);
                            fullProtocolsList = fullProtocolsList.Where(frl => usersFavoriteProtocols.Contains(frl.ProtocolVersionID));
                            break;
                        case AppUtility.SidebarEnum.SharedWithMe:
                            var shareProtocols = _context.ShareProtocols.Where(fr => fr.ToApplicationUserID == _userManager.GetUserId(User))
                    .Select(fr => fr.ProtocolVersionID);
                            fullProtocolsList = fullProtocolsList.Where(frl => shareProtocols.Contains(frl.ProtocolVersionID));
                            break;
                        case AppUtility.SidebarEnum.LastProtocol:
                            fullProtocolsList = fullProtocolsList.Where(fl => fl.ApplicationUserCreatorID == user.Id).Where(p => p.ProtocolInstances.Count() > 0);
                            break;
                    }
                    break;
            }

            ProtocolsIndexViewModel protocolsIndexViewModel = new ProtocolsIndexViewModel();
            protocolsIndexViewModel.PageNumber = protocolsIndexObject.PageNumber;
            protocolsIndexViewModel.PageType = protocolsIndexObject.PageType;
            protocolsIndexViewModel.ErrorMessage = protocolsIndexObject.ErrorMessage;
            var onePageOfProducts = Enumerable.Empty<ProtocolsIndexPartialRowViewModel>().ToPagedList();


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
        private async Task<IPagedList<ProtocolsIndexPartialRowViewModel>> GetProtocolsColumnsAndRows(ProtocolsIndexObject protocolsIndexObject, IPagedList<ProtocolsIndexPartialRowViewModel> onePageOfProtocols, IQueryable<ProtocolVersion> ProtocolPassedInWithInclude)
        {
            List<IconColumnViewModel> iconList = new List<IconColumnViewModel>();
            var favoriteIcon = new IconColumnViewModel(" icon-favorite_border-24px", "var(--protocols-color)", "protocol-favorite", "Favorite");
            var popoverMoreIcon = new IconColumnViewModel("icon-more_vert-24px", "black", "popover-more", "More");
            var popoverRemoveShare = new IconPopoverViewModel("icon-share-24px1", "black", AppUtility.PopoverDescription.RemoveShare, ajaxcall: "remove-share");
            var popoverShare = new IconPopoverViewModel("icon-share-24px1", "black", AppUtility.PopoverDescription.Share, "ShareModal", "Protocols", AppUtility.PopoverEnum.None, "share-protocol-fx");
            var popoverStart = new IconPopoverViewModel("icon-play_circle_outline-24px-1", "#4CAF50", AppUtility.PopoverDescription.Start, "StartProtocol", "Protocols", AppUtility.PopoverEnum.None, "start-protocol-fx");
            var updateResultsIcon = new IconColumnViewModel("UpdateResults");
            var user = await _userManager.GetUserAsync(User);
            switch (protocolsIndexObject.PageType)
            {
                case AppUtility.PageTypeEnum.ProtocolsProtocols:
                    switch (protocolsIndexObject.SidebarType)
                    {
                        case AppUtility.SidebarEnum.List:
                            iconList.Add(favoriteIcon);
                            popoverMoreIcon.IconPopovers = new List<IconPopoverViewModel>() { popoverShare, popoverStart };
                            iconList.Add(popoverMoreIcon);
                            onePageOfProtocols = await ProtocolPassedInWithInclude.OrderByDescending(p => p.CreationDate)
    .Select(p => new ProtocolsIndexPartialRowViewModel(p, p.Protocol, p.Protocol.ProtocolType, p.Protocol.ProtocolSubCategory, p.ApplicationUserCreator, protocolsIndexObject, iconList, _context.FavoriteProtocols.Where(fr => fr.ProtocolVersionID == p.ProtocolVersionID).Where(fr => fr.ApplicationUserID == user.Id).FirstOrDefault(), user,
        _context.ProtocolInstances.Where(pi => pi.ProtocolVersionID == p.ProtocolVersionID && pi.ApplicationUserID == user.Id && !pi.IsFinished).OrderByDescending(pi => pi.StartDate).FirstOrDefault())).ToPagedListAsync(protocolsIndexObject.PageNumber == 0 ? 1 : protocolsIndexObject.PageNumber, 20);
                            break;
                        case AppUtility.SidebarEnum.MyProtocols:
                            iconList.Add(favoriteIcon);
                            popoverMoreIcon.IconPopovers = new List<IconPopoverViewModel>() { popoverShare, popoverStart };
                            iconList.Add(popoverMoreIcon);
                            onePageOfProtocols = await ProtocolPassedInWithInclude.OrderByDescending(p => p.CreationDate)
  .Select(p => new ProtocolsIndexPartialRowViewModel(p, p.Protocol, p.Protocol.ProtocolType, p.Protocol.ProtocolSubCategory, protocolsIndexObject, iconList,
     _context.FavoriteProtocols.Where(fr => fr.ProtocolVersionID == p.ProtocolVersionID).Where(fr => fr.ApplicationUserID == user.Id).FirstOrDefault(), user,
             _context.ProtocolInstances.Where(pi => pi.ProtocolVersionID == p.ProtocolVersionID && pi.ApplicationUserID == user.Id && !pi.IsFinished).OrderByDescending(pi => pi.StartDate).FirstOrDefault())).ToPagedListAsync(protocolsIndexObject.PageNumber == 0 ? 1 : protocolsIndexObject.PageNumber, 20);

                            break;
                        case AppUtility.SidebarEnum.Favorites:
                            iconList.Add(favoriteIcon);
                            popoverMoreIcon.IconPopovers = new List<IconPopoverViewModel>() { popoverShare, popoverStart };
                            iconList.Add(popoverMoreIcon);
                            onePageOfProtocols = await ProtocolPassedInWithInclude.OrderByDescending(p => p.CreationDate)
   .Select(p => new ProtocolsIndexPartialRowViewModel(p, p.Protocol, p.Protocol.ProtocolType, p.Protocol.ProtocolSubCategory, p.ApplicationUserCreator, protocolsIndexObject, iconList,
                                                 _context.ShareProtocols
                .Where(fr => fr.ProtocolVersionID == p.ProtocolVersionID).Where(sr => sr.ToApplicationUserID == user.Id).Include(sr => sr.FromApplicationUser).FirstOrDefault(),
                                                  _context.FavoriteProtocols.Where(fr => fr.ProtocolVersionID == p.ProtocolVersionID).Where(fr => fr.ApplicationUserID == user.Id).FirstOrDefault(), user,
                                                          _context.ProtocolInstances.Where(pi => pi.ProtocolVersionID == p.ProtocolVersionID && pi.ApplicationUserID == user.Id && !pi.IsFinished).OrderByDescending(pi => pi.StartDate).FirstOrDefault()
                                        )).ToPagedListAsync(protocolsIndexObject.PageNumber == 0 ? 1 : protocolsIndexObject.PageNumber, 20);
                            break;
                        case AppUtility.SidebarEnum.SharedWithMe:
                            popoverMoreIcon.IconPopovers = new List<IconPopoverViewModel>() { popoverShare, popoverRemoveShare, popoverStart };
                            iconList.Add(popoverMoreIcon);
                            onePageOfProtocols = await ProtocolPassedInWithInclude.OrderByDescending(p => p.CreationDate)
.Select(p => new ProtocolsIndexPartialRowViewModel(p, p.Protocol, p.Protocol.ProtocolType, p.Protocol.ProtocolSubCategory, protocolsIndexObject, iconList, p.ApplicationUserCreator,
                                              _context.ShareProtocols
             .Where(fr => fr.ProtocolVersionID == p.ProtocolVersionID).Where(sr => sr.ToApplicationUserID == user.Id).Include(sr => sr.FromApplicationUser).FirstOrDefault(), user, _context.ProtocolInstances.Where(pi => pi.ProtocolVersionID == p.ProtocolVersionID && pi.ApplicationUserID == user.Id && !pi.IsFinished).OrderByDescending(pi => pi.StartDate).FirstOrDefault()
                                     )).ToPagedListAsync(protocolsIndexObject.PageNumber == 0 ? 1 : protocolsIndexObject.PageNumber, 20);
                            break;
                        case AppUtility.SidebarEnum.LastProtocol:
                            iconList.Add(updateResultsIcon);
                            var protocolList = new List<ProtocolProtocolInstance>() { };

                            foreach (var protocol in ProtocolPassedInWithInclude)
                            {
                                var currentProtocolInstances = protocol.ProtocolInstances.Where(pi => !pi.ResultsReported && pi.IsFinished).ToList();
                                if (currentProtocolInstances.Count() > 0)
                                {
                                    for (var i = 0; i < currentProtocolInstances.Count(); i++)
                                    {
                                        protocolList.Add(new ProtocolProtocolInstance { ProtocolVersion = protocol, ProtocolInstance = currentProtocolInstances.ElementAt(i) });
                                    }
                                }
                            }
                            onePageOfProtocols = await protocolList.OrderByDescending(p => p.ProtocolInstance.EndDate)
.Select(p => new ProtocolsIndexPartialRowViewModel(p.ProtocolVersion, p.ProtocolVersion.Protocol, p.ProtocolVersion.Protocol.ProtocolType, p.ProtocolVersion.Protocol.ProtocolSubCategory, p.ProtocolVersion.ApplicationUserCreator, protocolsIndexObject, iconList, user, p.ProtocolInstance
                                     )).ToPagedListAsync(protocolsIndexObject.PageNumber == 0 ? 1 : protocolsIndexObject.PageNumber, 20);

                            break;
                    }
                    break;
            }
            return onePageOfProtocols;
        }
        private async Task<ReportsIndexViewModel> GetReportsIndexViewModel(ReportsIndexObject reportsIndexObject)
        {
            var currentWeek = CultureInfo.CurrentCulture.Calendar.GetWeekOfYear(DateTime.Now, CalendarWeekRule.FirstFullWeek, DayOfWeek.Sunday);

            if (reportsIndexObject.Years.Count == 0)
            {
                reportsIndexObject.Years.Add(DateTime.Now.Year);
            }
            if (reportsIndexObject.Months.Count == 0)
            {
                reportsIndexObject.Months.Add(DateTime.Now.Month);
            }
            reportsIndexObject.ReportCategory = _context.ResourceCategories.Where(rc => rc.ResourceCategoryID == reportsIndexObject.ReportCategoryID).FirstOrDefault();
            IQueryable<Report> ReportsPassedIn = Enumerable.Empty<Report>().AsQueryable();
            IQueryable<Report> ReportsPassedInWithInclude = _context.Reports.Where(r => r.ReportCategoryID == reportsIndexObject.ReportCategoryID)
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

            var currentWeekReport = _context.Reports.Where(r => r.WeekNumber == currentWeek && r.DateCreated.Year == DateTime.Now.Year && r.ReportCategoryID == reportsIndexObject.ReportCategoryID).FirstOrDefault();
            if (currentWeekReport != null)
            {
                reportsIndexViewModel.CurrentReportCreated = true;
            }

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

        private static async Task<IPagedList<ReportIndexPartialRowViewModel>> GetReportListRows(ReportsIndexObject reportsIndexObject, IPagedList<ReportIndexPartialRowViewModel> onePageOfReports, IQueryable<Report> ReportPassedInWithInclude)
        {
            var reports = ReportPassedInWithInclude.OrderByDescending(r => r.DateCreated);
            onePageOfReports = await ReportPassedInWithInclude.OrderByDescending(r => r.DateCreated).ToList().Select(r => new ReportIndexPartialRowViewModel(AppUtility.ReportTypes.Weekly, r, r.ReportCategory, reportsIndexObject)
            ).ToPagedListAsync(reportsIndexObject.PageNumber == 0 ? 1 : reportsIndexObject.PageNumber, 20);
            return onePageOfReports;
        }

        [Authorize(Roles = "Protocols")]
        public async Task<IActionResult> CurrentProtocols()
        {
            TempData[AppUtility.TempDataTypes.MenuType.ToString()] = AppUtility.MenuItems.Protocols;
            TempData[AppUtility.TempDataTypes.SidebarType.ToString()] = AppUtility.SidebarEnum.CurrentProtocols;
            TempData[AppUtility.TempDataTypes.PageType.ToString()] = AppUtility.PageTypeEnum.ProtocolsWorkflow;
            var user = await _userManager.GetUserAsync(User);
            var viewmodel = _context.ProtocolInstances.Where(p => p.ApplicationUserID == user.Id && !p.IsFinished).Include(p => p.ProtocolVersion).Include(p=>p.ProtocolVersion.Protocol).ToList().Select(p => new WorkFlowViewModel() { ProtocolInstance = p, CurrentLineString = GetLineNumberString(p.CurrentLineID) });
            return View(viewmodel);
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
            var viewmodel = await GetProtocolsIndexViewModelAsync(
                new ProtocolsIndexObject() { SectionType = AppUtility.MenuItems.Protocols, SidebarType = AppUtility.SidebarEnum.MyProtocols, PageType = AppUtility.PageTypeEnum.ProtocolsProtocols });

            return View(viewmodel);
        }

        [Authorize(Roles = "Protocols")]
        public async Task<IActionResult> ProtocolsFavorites()
        {
            TempData[AppUtility.TempDataTypes.MenuType.ToString()] = AppUtility.MenuItems.Protocols;
            TempData[AppUtility.TempDataTypes.SidebarType.ToString()] = AppUtility.SidebarEnum.Favorites;
            TempData[AppUtility.TempDataTypes.PageType.ToString()] = AppUtility.PageTypeEnum.ProtocolsProtocols;
            var viewmodel = await GetProtocolsIndexViewModelAsync(
                new ProtocolsIndexObject() { SectionType = AppUtility.MenuItems.Protocols, SidebarType = AppUtility.SidebarEnum.Favorites, PageType = AppUtility.PageTypeEnum.ProtocolsProtocols });

            return View(viewmodel);
        }

        [Authorize(Roles = "Protocols")]
        public async Task<IActionResult> ProtocolsSharedWithMe()
        {
            TempData[AppUtility.TempDataTypes.MenuType.ToString()] = AppUtility.MenuItems.Protocols;
            TempData[AppUtility.TempDataTypes.SidebarType.ToString()] = AppUtility.SidebarEnum.SharedWithMe;
            TempData[AppUtility.TempDataTypes.PageType.ToString()] = AppUtility.PageTypeEnum.ProtocolsProtocols;
            var viewmodel = await GetProtocolsIndexViewModelAsync(
                 new ProtocolsIndexObject() { SectionType = AppUtility.MenuItems.Protocols, SidebarType = AppUtility.SidebarEnum.SharedWithMe, PageType = AppUtility.PageTypeEnum.ProtocolsProtocols });

            return View(viewmodel);
        }
        [Authorize(Roles = "Protocols")]
        public async Task<IActionResult> LastProtocol()
        {
            TempData[AppUtility.TempDataTypes.MenuType.ToString()] = AppUtility.MenuItems.Protocols;
            TempData[AppUtility.TempDataTypes.SidebarType.ToString()] = AppUtility.SidebarEnum.LastProtocol;
            TempData[AppUtility.TempDataTypes.PageType.ToString()] = AppUtility.PageTypeEnum.ProtocolsProtocols;
            var viewmodel = await GetProtocolsIndexViewModelAsync(new ProtocolsIndexObject() { SectionType = AppUtility.MenuItems.Protocols, SidebarType = AppUtility.SidebarEnum.LastProtocol, PageType = AppUtility.PageTypeEnum.ProtocolsProtocols });
            return View(viewmodel);
        }
        [Authorize(Roles = "Protocols")]
        public async Task<IActionResult> ResearchProtocol()
        {
            TempData[AppUtility.TempDataTypes.MenuType.ToString()] = AppUtility.MenuItems.Protocols;
            TempData[AppUtility.TempDataTypes.SidebarType.ToString()] = AppUtility.SidebarEnum.ResearchProtocol;
            TempData[AppUtility.TempDataTypes.PageType.ToString()] = AppUtility.PageTypeEnum.ProtocolsCreate;
            CreateProtocolsViewModel viewmodel = new CreateProtocolsViewModel();
            await FillCreateProtocolsViewModel(viewmodel, 1);
            viewmodel.ModalType = AppUtility.ProtocolModalType.Create;
            return View(viewmodel);
        }

        [Authorize(Roles = "Protocols")]
        public async Task<IActionResult> StartProtocol(int ID, bool isContinue, int tab = 3)
        {
            try
            {
                using (var transaction = _context.Database.BeginTransaction())
                {
                    try
                    {
                        var user = await _userManager.GetUserAsync(User);
                        CreateProtocolsViewModel viewmodel = new CreateProtocolsViewModel();
                        viewmodel.Tab = tab;
                        ProtocolVersion protocolVersion = null;
                        if (isContinue)
                        {
                            viewmodel.ProtocolInstance = await _context.ProtocolInstances.Where(p => p.ProtocolInstanceID == ID).Include(p => p.ProtocolVersion).ThenInclude(pv=>pv.Protocol).FirstOrDefaultAsync();
                            viewmodel.ProtocolInstance.TemporaryResultDescription = viewmodel.ProtocolInstance.ResultDescription;
                            protocolVersion = viewmodel.ProtocolInstance.ProtocolVersion;
                        }
                        else
                        {
                            protocolVersion = _context.ProtocolVersions.Where(p => p.ProtocolVersionID == ID).Include(p=>p.Protocol).FirstOrDefault();
                            viewmodel.ProtocolInstance = new ProtocolInstance { ProtocolVersionID = ID, StartDate = DateTime.Now, ApplicationUserID = user.Id, CurrentLineID = _context.Lines.Where(l => l.ProtocolVersionID == ID && l.ParentLineID == null && l.LineNumber == 1).FirstOrDefault().LineID };
                            _context.Add(viewmodel.ProtocolInstance);
                            await _context.SaveChangesAsync();
                        }
                        viewmodel.ModalType = AppUtility.ProtocolModalType.CheckListMode;
                        await FillCreateProtocolsViewModel(viewmodel, viewmodel.ProtocolInstance.ProtocolVersion.Protocol.ProtocolTypeID,  viewmodel.ProtocolInstance.ProtocolVersionID);
                        await transaction.CommitAsync();
                        return PartialView("_IndexTableWithEditProtocol", viewmodel);
                    }
                    catch (Exception ex)
                    {
                        Response.StatusCode = 500;
                        await transaction.RollbackAsync();
                        await Response.WriteAsync(AppUtility.GetExceptionMessage(ex));
                        return new EmptyResult();
                    }
                }
            }
            catch (Exception ex)
            {
                Response.StatusCode = 500;
                await Response.WriteAsync(AppUtility.GetExceptionMessage(ex));
                return new EmptyResult();
            }
        
        }

        [Authorize(Roles = "Protocols")]
        public async Task<IActionResult> MoveToNextLine(int protocolInstanceID, int nextLineID, bool isLast, Guid guid)
        {
            var protocolInstance = await _context.ProtocolInstances.Where(pi => pi.ProtocolInstanceID == protocolInstanceID).FirstOrDefaultAsync();
            if (nextLineID > 0 || isLast)
            {
                if (isLast)
                {
                    protocolInstance.IsFinished = true;
                    protocolInstance.EndDate = DateTime.Now;
                }
                else
                {
                    protocolInstance.CurrentLineID = nextLineID;
                }
                _context.Update(protocolInstance);
                await _context.SaveChangesAsync();
            }

            return PartialView("_Lines", await OrderLinesForViewAsync(false, protocolInstance.ProtocolVersionID, AppUtility.ProtocolModalType.CheckListMode, guid, protocolInstance));
        }
        [HttpGet]
        [Authorize(Roles = "Protocols")]
        public async Task<IActionResult> AddChangeModal(int protocolInstanceID, int currentLineID)
        {
            var protocolInstance = await _context.ProtocolInstances.Where(pi => pi.ProtocolInstanceID == protocolInstanceID).FirstOrDefaultAsync();
            var lineChange = await _context.LineChanges.Where(lc => lc.LineID == currentLineID && lc.ProtocolInstanceID == protocolInstanceID).FirstOrDefaultAsync();
            if (lineChange == null)
            {
                lineChange = new LineChange() { ProtocolInstanceID = protocolInstanceID, LineID = currentLineID };
            }
            return PartialView(lineChange);
        }

        [HttpPost]
        [Authorize(Roles = "Protocols")]
        public async Task<IActionResult> AddChangeModal(LineChange lineChange, Guid guid)
        {
            var protocolInstance = await _context.ProtocolInstances.Where(pi => pi.ProtocolInstanceID == lineChange.ProtocolInstanceID).FirstOrDefaultAsync();

            if (lineChange.ChangeText != null && !lineChange.ChangeText.IsNullOrWhiteSpace())
            {
                var lineChangeDB = await _context.LineChanges.Where(lc => lc.LineID == lineChange.LineID && lc.ProtocolInstanceID == lineChange.ProtocolInstanceID).FirstOrDefaultAsync();
                using (var transaction = await _context.Database.BeginTransactionAsync())
                {
                    try
                    {
                        if (lineChangeDB == null)
                        {
                            _context.Entry(lineChange).State = EntityState.Added;
                        }
                        else
                        {
                            lineChangeDB.ChangeText = lineChange.ChangeText;
                            _context.Entry(lineChangeDB).State = EntityState.Modified;
                        }
                        await _context.SaveChangesAsync();
                        await transaction.CommitAsync();
                    }
                    catch (Exception ex)
                    {
                        await transaction.RollbackAsync();
                        var viewmodel = await OrderLinesForViewAsync(false, protocolInstance.ProtocolVersionID, AppUtility.ProtocolModalType.CheckListMode, guid, protocolInstance);
                        viewmodel.ErrorMessage = AppUtility.GetExceptionMessage(ex);
                        return PartialView("_Lines", viewmodel);
                    }
                }
            }

            return PartialView("_Lines", await OrderLinesForViewAsync(false, protocolInstance.ProtocolVersionID, AppUtility.ProtocolModalType.CheckListMode, guid, protocolInstance));

        }
        private async Task<CreateProtocolsViewModel> FillCreateProtocolsViewModel(CreateProtocolsViewModel createProtocolsViewModel, int typeID, int protocolVersionID = 0)
        {
            DeleteTemporaryDocuments(AppUtility.ParentFolderName.Protocols, Guid.Empty);
            var protocol = _context.ProtocolVersions.Include(p=>p.Protocol)
                .Include(p => p.Urls).Include(p => p.Materials)
                .ThenInclude(m => m.Product).Include(p => p.Protocol.ProtocolSubCategory).Where(p => p.ProtocolVersionID == protocolVersionID).FirstOrDefault() ?? new ProtocolVersion() {Protocol = new Protocol() { UniqueCode = GetUniqueNumber() }, VersionNumber = 1 };
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
                protocol.Protocol.ProtocolTypeID = typeID;
            }
            List<FunctionType> protocolsFunctionTypes = new List<FunctionType>();
            List<FunctionType> resultsFunctionTypes = new List<FunctionType>();

            createProtocolsViewModel.ProtocolVersion = protocol;
            createProtocolsViewModel.ProtocolCategories = _context.ProtocolCategories;
            createProtocolsViewModel.ProtocolSubCategories = _context.ProtocolSubCategories;
            createProtocolsViewModel.MaterialCategories = _context.MaterialCategories;
            createProtocolsViewModel.LineTypes = _context.LineTypes.ToList();
            foreach (var functionType in Enum.GetValues(typeof(AppUtility.ProtocolFunctionTypes)))
            {
                protocolsFunctionTypes.Add(_context.FunctionTypes.AsNoTracking().Where(ft => ft.DescriptionEnum == functionType.ToString()).FirstOrDefault());
            }
            createProtocolsViewModel.ProtocolFunctionTypes = protocolsFunctionTypes;
            foreach (var functionType in Enum.GetValues(typeof(AppUtility.ResultsFunctionTypes)))
            {
                resultsFunctionTypes.Add(_context.FunctionTypes.AsNoTracking().Where(ft => ft.DescriptionEnum == functionType.ToString()).FirstOrDefault());
            }
            createProtocolsViewModel.ResultsFunctionTypes = resultsFunctionTypes;
            if (createProtocolsViewModel.UniqueGuid == Guid.Empty)
            {
                createProtocolsViewModel.UniqueGuid = Guid.NewGuid();
                var functionLines = await _context.FunctionLines.Where(fl => fl.Line.ProtocolVersionID == protocolVersionID && fl.IsTemporaryDeleted == false).Include(fl => fl.FunctionType).Include(fl => fl.ProtocolVersion).ThenInclude(pv  =>pv.Protocol).ThenInclude(p=>p.ProtocolSubCategory).Include(fl => fl.Product).ToListAsync();

                var lines = await _context.Lines.Where(l => l.ProtocolVersionID == protocolVersionID && l.IsTemporaryDeleted == false).Select(l =>
               new ProtocolsLineViewModel { Line = l, Functions = AppUtility.GetFunctionsByLineID(l.LineID, functionLines) }).ToListAsync();
             
                if (lines.Count() == 0)
                {
                    var lineID = new TempLineID();
                    _context.Add(lineID);
                    await _context.SaveChangesAsync();
                    var line = new Line() { LineID = lineID.ID, ProtocolVersionID = createProtocolsViewModel.ProtocolVersion.ProtocolVersionID, LineNumber = 1, LineTypeID = 1 };
                    lines.Add(new ProtocolsLineViewModel { Line = line });
                }
                //create new tempjson 
                TempLinesJson tempLinesJson = new TempLinesJson { TempLinesJsonID = createProtocolsViewModel.UniqueGuid };
                tempLinesJson.SerializeViewModel(new ProtocolsLinesViewModel { Lines = lines });
                _context.Add(tempLinesJson);
                await _context.SaveChangesAsync();
                createProtocolsViewModel.Lines = await OrderLinesForViewAsync(true, protocolVersionID, createProtocolsViewModel.ModalType, createProtocolsViewModel.UniqueGuid, createProtocolsViewModel.ProtocolInstance);
            }
            else
            {
                createProtocolsViewModel.Lines = await OrderLinesForViewAsync(false, protocolVersionID, createProtocolsViewModel.ModalType, createProtocolsViewModel.UniqueGuid, createProtocolsViewModel.ProtocolInstance);
            }
            AppUtility.ParentFolderName parentFolderName = AppUtility.ParentFolderName.Protocols;
            string uploadProtocolsFolder = Path.Combine(_hostingEnvironment.WebRootPath, parentFolderName.ToString());
            string uploadProtocolsFolder2 = Path.Combine(uploadProtocolsFolder, protocol.ProtocolVersionID.ToString());
            FillDocumentsInfo(createProtocolsViewModel, parentFolderName, uploadProtocolsFolder2, protocolVersionID.ToString());
            Dictionary<Material, List<DocumentFolder>> MaterialFolders = FillMaterialDocumentsModel(protocol.Materials, Path.Combine(_hostingEnvironment.WebRootPath, AppUtility.ParentFolderName.Materials.ToString()));
            createProtocolsViewModel.MaterialDocuments = (Lookup<Material, List<DocumentFolder>>)MaterialFolders.ToLookup(o => o.Key, o => o.Value);
            return createProtocolsViewModel;
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
        private int GetSmallestLineTypeInListOfLines(IEnumerable<Line> lines)
        {
            var parentLines = lines.Where(l => l.ParentLineID == null).ToList();
            var smallestLineTypeID = parentLines[0].LineTypeID;
            while(parentLines.Count()>0)
            {
                var currNode = parentLines[0];
                var children = lines.Where(l => l.LineType.LineTypeParentID == currNode.LineTypeID).ToList();
                if(children.Count!=0)
                {
                    smallestLineTypeID = children[0].LineTypeID;                  
                }
                parentLines = children;
            }

            return smallestLineTypeID;
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
        public async Task SaveTempLines(List<Line> Lines, int ProtocolVersionID, Guid guid)
        {
            try
            {
                using (var transaction = _context.Database.BeginTransaction())
                {
                    try
                    {
                        var tempLines = _context.TempLinesJsons.Where(tl => tl.TempLinesJsonID == guid).FirstOrDefault().DeserializeJson<ProtocolsLinesViewModel>();

                        await UpdateLineContentAsync(tempLines, Lines);
                        await SaveTempLinesToDB(ProtocolVersionID, tempLines);
                        await _context.FunctionLines.Where(fl => fl.ProtocolVersionID == ProtocolVersionID).Where(fl => fl.IsTemporaryDeleted || fl.Line.IsTemporaryDeleted == true).ForEachAsync(fl => { _context.Remove(fl); });
                        await _context.SaveChangesAsync();                   
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
            catch (Exception ex)
            {
                Response.StatusCode = 500;
                await Response.WriteAsync(AppUtility.GetExceptionMessage(ex));
            }
        }

        private async Task SaveTempLinesToDB(int ProtocolVersionID, ProtocolsLinesViewModel tempLines)
        {
            _context.ChangeTracker.Entries().Where(e => e.Entity is Line).ToList().ForEach(e => { e.State = EntityState.Detached; });
            foreach (var line in tempLines.Lines)
            {
                line.Line.ProtocolVersionID = ProtocolVersionID;
                if (_context.Lines.Where(l => l.LineID == line.Line.LineID).AsNoTracking().Any())
                {

                    if (line.Line.IsTemporaryDeleted)
                    {
                        _context.Entry(line.Line).State = EntityState.Deleted;
                    }
                    else
                    {
                        _context.Entry(line.Line).State = EntityState.Modified;
                    }               
                }
                else
                {
                    if (!line.Line.IsTemporaryDeleted)
                    {                     
                        _context.Entry(line.Line).State = EntityState.Added;
                    }
                }
                if (line.Functions != null)
                {
                    foreach (var function in line.Functions)
                    {
                        if (_context.FunctionLines.Where(fl => fl.ID == function.ID).Any())
                        {
                            _context.Entry(function).State = EntityState.Modified;
                        }
                        else
                        {
                            _context.Entry(function).State = EntityState.Added;
                        }
                    }
                }

            }
            await _context.SaveChangesAsync();
        }

        private async Task ClearTempLinesJsonAsync(Guid guid)
        {
            var lineTypes = GetOrderLineTypeFromChildToParent();
            await _context.FunctionLines.Where(fl => fl.IsTemporaryDeleted).ForEachAsync(fl => { fl.IsTemporaryDeleted = false; _context.Update(fl); });
            await _context.SaveChangesAsync();

            var tempLinesJson = await _context.TempLinesJsons.Where(l => l.TempLinesJsonID == guid).FirstOrDefaultAsync();
            _context.Remove(tempLinesJson);
            await _context.SaveChangesAsync();

            var tempDeletedLines = _context.Lines.Where(tl => tl.IsTemporaryDeleted);

            foreach (var tempDeleted in tempDeletedLines)
            {
                tempDeleted.IsTemporaryDeleted = false;
                _context.Update(tempDeleted);
            }
            await _context.SaveChangesAsync();
        }
        private Dictionary<Material, List<DocumentFolder>> FillMaterialDocumentsModel(IEnumerable<Material> Materials, string uploadProtocolsFolder)
        {
            Dictionary<Material, List<DocumentFolder>> MaterialFolders = new Dictionary<Material, List<DocumentFolder>>();
            foreach (var material in Materials)
            {
                List<DocumentFolder> folders = new List<DocumentFolder>();
                string materialId = material.MaterialID.ToString();
                string uploadMaterialFolder2 = Path.Combine(uploadProtocolsFolder, materialId);
                base.GetExistingFileStrings(folders, AppUtility.FolderNamesEnum.Pictures, AppUtility.ParentFolderName.Materials, uploadMaterialFolder2, materialId);
                MaterialFolders.Add(material, folders);
            }

            return MaterialFolders;
        }

        [Authorize(Roles = "Protocols")]
        public async Task<IActionResult> AddMaterialModal(int materialTypeID, int ProtocolVersionID)
        {
            var MaterialCategory = _context.MaterialCategories.Where(mc => mc.MaterialCategoryID == materialTypeID).FirstOrDefault();

            var viewModel = new AddMaterialViewModel()
            {
                Material = new Material()
                {
                    MaterialCategoryID = materialTypeID,
                    MaterialCategory = MaterialCategory,
                    ProtocolVersionID = ProtocolVersionID
                }
            };
            return PartialView(viewModel);
        }
        [Authorize(Roles = "Protocols")]
        public async Task<IActionResult> MaterialInfoModal(int materialID, AppUtility.ProtocolModalType ModalType)
        {
            var material = _context.Materials.Where(m => m.MaterialID == materialID).FirstOrDefault();
            return PartialView(new AddMaterialViewModel { Material = material, ModalType = ModalType });
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
                return redirectToMaterialTab(materialDB.ProtocolVersionID);
            }
        }
        [HttpPost]
        [HttpGet]
        [Authorize(Roles = "Protocols")]
        public async Task<IActionResult> _Lines(List<Line> Lines, int lineTypeID, int currentLineID, int protocolVersionID, AppUtility.ProtocolModalType modalType, Guid guid)
        {
            using (var transaction = _context.Database.BeginTransaction())
            {
                try
                {
                    var orderedLineTypes = GetOrderLineTypeFromParentToChild();
                    var listOfLineTypeIDs = orderedLineTypes.Select(lt => lt.LineTypeID).ToList();
                    var tempLinesJson = await _context.TempLinesJsons.Where(tlj => tlj.TempLinesJsonID == guid).FirstOrDefaultAsync();
                    var tempLines = tempLinesJson.DeserializeJson<ProtocolsLinesViewModel>();
                    tempLines.Lines = tempLines.Lines.Where(tl => !tl.Line.IsTemporaryDeleted).ToList();
                    var currentLine = tempLines.Lines.Where(tl => tl.Line.LineID == currentLineID).FirstOrDefault().Line;
                    if (Lines != null)
                    {
                        //save all temp line data 
                        await UpdateLineContentAsync(tempLines, Lines);
                    }

                    if (lineTypeID == -1)
                    {
                        await DeleteTempLineWithChildrenAsync(tempLines, currentLine);
                    }
                    else
                    {
                        var newLineType = _context.LineTypes.Where(lt => lt.LineTypeID == lineTypeID).FirstOrDefault();

                        Line newLine = new Line();
                        ///this is to keep track of the last lineid
                        var templineID = new TempLineID();
                        _context.Add(templineID);
                        await _context.SaveChangesAsync();
                        newLine.LineID = templineID.ID;
                        newLine.LineTypeID = lineTypeID;
                        newLine.ProtocolVersionID = protocolVersionID;
                        if (newLine.LineNumber == 0)
                        {
                            newLine.LineNumber = 1;
                        }

                        if (currentLine != null)
                        {
                            var currentLineTypeIndex = listOfLineTypeIDs.IndexOf(currentLine.LineTypeID);
                            var newLineTypeIndex = listOfLineTypeIDs.IndexOf(newLineType.LineTypeID);
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
                                        newLine.ParentLine = parent.ParentLine;
                                        //we have to increment all the sibling parents
                                        var siblings = await tempLines.Lines.Where(tl => tl.Line.LineNumber > parent.LineNumber && tl.Line.ParentLineID == newLine.ParentLineID).ToListAsync();
                                        siblings.ForEach(tl => { tl.Line.LineNumber += 1; });
                                        break;
                                    }
                                    parent = tempLines.Lines.Where(tl => tl.Line.LineID == parent.ParentLineID).FirstOrDefault().Line;
                                }


                                if (newLineTypeIndex < currentLineTypeIndex)
                                {
                                    //get currentline siblings and make their parent point to new line
                                    var currentLineSiblings = tempLines.Lines.Where(lt => lt.Line.ParentLineID == currentLine.LineID && lt.Line.LineNumber > currentLine.LineNumber).ToList();
                                    currentLineSiblings.ForEach(tl =>
                                    {
                                        tl.Line.LineNumber -= currentLine.LineNumber;
                                        tl.Line.ParentLineID = newLine.LineID;
                                        tl.Line.ParentLine = newLine;
                                    });
                                    currentLine.ParentLine = tempLines.Lines.Where(tl => tl.Line.LineID == currentLine.ParentLineID).FirstOrDefault().Line;
                                    if (listOfLineTypeIDs.IndexOf(currentLine.ParentLine.LineTypeID) < newLineTypeIndex)
                                    {
                                        //make new line currents parent
                                        newLine.ParentLineID = currentLine.ParentLineID;
                                        newLine.ParentLine = currentLine.ParentLine;
                                        currentLine.ParentLineID = newLine.LineID;
                                        currentLine.ParentLine = newLine;
                                    }
                                }
                                else // types are the same
                                {
                                    //all curents children should point to new line
                                    tempLines.Lines.Where(tl => tl.Line.ParentLineID == currentLine.LineID).ToList().ForEach(tl => { tl.Line.ParentLineID = newLine.LineID; tl.Line.ParentLine = newLine; });
                                }
                            }
                            else
                            {
                                newLine.LineNumber = 1;
                                newLine.ParentLineID = currentLine.LineID;
                                var siblings = await tempLines.Lines.Where(tl => tl.Line.ParentLineID == newLine.ParentLineID).ToListAsync();
                                siblings.ForEach(tl => { tl.Line.LineNumber += 1; });
                            }

                            ProtocolsLineViewModel newLineViewModel = new ProtocolsLineViewModel
                            {
                                Line = newLine
                            };
                            tempLines.Lines.Add(newLineViewModel);
                        }

                    }
                    tempLinesJson.SerializeViewModel(tempLines);
                    _context.Update(tempLinesJson);
                    await _context.SaveChangesAsync();
                    await transaction.CommitAsync();
                }
                catch (Exception ex)
                {
                    Response.StatusCode = 500;
                    await transaction.RollbackAsync();
                    //  await Response.WriteAsync(AppUtility.GetExceptionMessage(ex));
                    var viewmodel = await OrderLinesForViewAsync(false, protocolVersionID, modalType, guid);
                    viewmodel.ErrorMessage = AppUtility.GetExceptionMessage(ex);
                    return PartialView("_Lines", viewmodel);
                }
            }

            return PartialView("_Lines", await OrderLinesForViewAsync(true, protocolVersionID, modalType, guid));
        }


        public bool CheckIfSerialNumberExists(string serialNumber)
        {
            return _context.Products.Where(p => p.Requests.Where(r => r.RequestStatusID == 3 || r.RequestStatusID == 7).Any()).Where(p => p.SerialNumber.Equals(serialNumber)).ToList().Any();
        }
        public bool CheckIfProtocolUniqueNumberExists(string uniqueNumber)
        {
            return _context.ProtocolVersions.Include(p=>p.Protocol).Select(p=>p.Protocol.UniqueCode+"V"+p.VersionNumber).Where(p =>p.Equals(uniqueNumber)).ToList().Any();
        }
        public bool ValidateUniqueProtocolNumber(string uniqueNumber, int protocolID)
        {
            return !_context.Protocols.Where(p => p.UniqueCode.Equals(uniqueNumber) && p.ProtocolID!=protocolID).ToList().Any();
        }
        private async Task UpdateLineContentAsync(ProtocolsLinesViewModel protocolsLinesViewModel, List<Line> Lines)
        {
            foreach (var line in Lines)
            {
                var temp = protocolsLinesViewModel.Lines.Where(l => l.Line.LineID == line.LineID).Select(l => l.Line).FirstOrDefault();
                if (temp != null)
                {
                    temp.Content = line.Content ?? "";
                }
            }
        }

        private async Task<ProtocolsLinesViewModel> OrderLinesForViewAsync(bool needsReordering, int protocolID, AppUtility.ProtocolModalType modalType, Guid guid, ProtocolInstance protocolInstance = null)
        {
            var currentLineID = protocolInstance?.CurrentLineID;
            var protocolInstanceID = protocolInstance?.ProtocolInstanceID;
            List<ProtocolsLineViewModel> refreshedLines = new List<ProtocolsLineViewModel>();
            Stack<ProtocolsLineViewModel> parentNodes = new Stack<ProtocolsLineViewModel>();
            ProtocolsLinesViewModel viewmodel = new ProtocolsLinesViewModel();
            var lineTypes = await _context.LineTypes.ToListAsync();
            var tlj = await _context.TempLinesJsons.Where(tlj => tlj.TempLinesJsonID == guid).FirstOrDefaultAsync();
            viewmodel = tlj.DeserializeJson<ProtocolsLinesViewModel>();
            viewmodel.Lines = viewmodel.Lines.Where(tl => !tl.Line.IsTemporaryDeleted).ToList();
            if (needsReordering)
            {
                viewmodel.Lines.Where(tl => tl.Line.ParentLineID == null).OrderByDescending(tl => tl.Line.LineNumber).ToList().ForEach(tl => { parentNodes.Push(tl); });
                int count = 0;
                while (!parentNodes.IsEmpty())
                {
                    var node = parentNodes.Pop();
                    //if protocolinstance is finished we will set them all and not according to currentline
                    if (currentLineID == node.Line.LineID && !protocolInstance.IsFinished)
                    {
                        refreshedLines.ForEach(l => l.IsDone = true);
                    }
                    viewmodel.Lines.Where(c => c.Line.ParentLineID == node.Line.LineID).OrderByDescending(tl => tl.Line.LineNumber).ToList().ForEach(c => { parentNodes.Push(c); });
                    node.LineTypes = lineTypes;
                    node.Index = count++;
                    node.LineNumberString = refreshedLines.Where(rl => rl.Line.LineID == node.Line.ParentLineID)?.FirstOrDefault()?.LineNumberString + node.Line.LineNumber + ".";
                    node.ModalType = modalType;
                    node.UniqueGuid = guid;
                    node.IsLast = parentNodes.IsEmpty();
                    node.IsDone = protocolInstance?.IsFinished ?? false;
                    node.LineChange = await _context.LineChanges.Where(lc => lc.LineID == node.Line.LineID && lc.ProtocolInstanceID == protocolInstanceID).FirstOrDefaultAsync();
                    refreshedLines.Add(node);
                }
                if (refreshedLines.Count == 0)
                {
                    refreshedLines.Add(new ProtocolsLineViewModel() { LineTypes = lineTypes, Index = 0, LineNumberString = 1 + "", IsLast = true });
                    viewmodel.Lines = refreshedLines;
                }
                viewmodel.Lines = refreshedLines;
            }
            else
            {
                bool endMarkDone = false;
                for (int i = 0; i < viewmodel.Lines.Count; i++)
                {
                    viewmodel.Lines[i].ModalType = modalType;
                    if (viewmodel.Lines[i].Line.LineID == currentLineID && !protocolInstance.IsFinished)
                    {
                        endMarkDone = true;
                    }
                    if (!endMarkDone)
                    {
                        viewmodel.Lines[i].IsDone = true;
                    }
                    if (viewmodel.Lines[i].IsLast)
                    {
                        viewmodel.Lines[i].IsDone = protocolInstance?.IsFinished ?? false;
                    }
                    viewmodel.Lines[i].LineChange = await _context.LineChanges.Where(lc => lc.LineID == viewmodel.Lines[i].Line.LineID && lc.ProtocolInstanceID == protocolInstanceID).FirstOrDefaultAsync();
                }

            }
            tlj.SerializeViewModel(viewmodel);
            _context.Update(tlj);
            //save the viewmodel to json
            await _context.SaveChangesAsync();
            return viewmodel;
        }

        private string GetLineNumberString(int LineID)
        {
            string lineNumberString = "";
            var line = _context.Lines.Where(l => l.LineID == LineID).FirstOrDefault();
            lineNumberString = line.LineNumber + ".";
            while (line.ParentLineID != null)
            {
                line = _context.Lines.Where(l => l.LineID == line.ParentLineID).FirstOrDefault();
                lineNumberString = line.LineNumber + "." + lineNumberString;
            }
            return lineNumberString;
        }
        private async Task DeleteTempLineWithChildrenAsync(ProtocolsLinesViewModel tempLines, Line line)
        {
            var siblingsAfter = tempLines.Lines.Where(tl => tl.Line.ParentLineID == line.ParentLineID && tl.Line.LineNumber > line.LineNumber).ToList();
            Stack<ProtocolsLineViewModel> nodes = new Stack<ProtocolsLineViewModel>();
            List<ProtocolsLineViewModel> nodeInOrderOfChildrenToParent = new List<ProtocolsLineViewModel>();
            var currLine = tempLines.Lines.Where(tl => tl.Line.LineID == line.LineID).FirstOrDefault();
            nodes.Push(currLine);
            //push all children on the stack
            while (!nodes.IsEmpty())
            {
                var curr = nodes.Pop();
                nodeInOrderOfChildrenToParent.Add(curr);
                var children = await tempLines.Lines.Where(tl => tl.Line.ParentLineID == curr.Line.LineID).ToListAsync();
                children.ForEach(tl => { nodes.Push(tl); });
            }
            foreach (var node in nodeInOrderOfChildrenToParent)
            {
                var permanentLine = await _context.Lines.Where(l => l.LineID == node.Line.LineID).FirstOrDefaultAsync();

                //if (permanentLine != null)
                //{
                //    permanentLine.IsTemporaryDeleted = true;
                //    _context.Update(permanentLine);
                //}
                //tempLines.Lines.Remove(node);
                node.Line.IsTemporaryDeleted = true;
            }

            //update all the siblings after number--
            siblingsAfter.ForEach(sa => { sa.Line.LineNumber--; });
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
                return redirectToMaterialTab(materialDB.ProtocolVersionID);
            }
        }

        [Authorize(Roles = "Protocols")]
        public async Task<IActionResult> AddFunctionModal(int FunctionTypeID, int LineID, int functionIndex, AppUtility.ProtocolModalType modalType, Guid guid)
        {
            var functionType = _context.FunctionTypes.Where(ft => ft.FunctionTypeID == FunctionTypeID).FirstOrDefault();
            var tempLinesJson = await _context.TempLinesJsons.Where(tlj => tlj.TempLinesJsonID == guid).FirstOrDefaultAsync();
            var tempLines = tempLinesJson.DeserializeJson<ProtocolsLinesViewModel>();
            var tempLine = tempLines.Lines.Where(tl => tl.Line.LineID == LineID).FirstOrDefault();
            var viewmodel = new AddLineFunctionViewModel
            {
                ModalType = modalType,
                UniqueGuid = guid,
                FunctionIndex = tempLine.Functions?.Count() ?? 0
            };
            if (tempLine.Functions?.Count() > functionIndex && functionIndex != -1)
            {
                viewmodel.Function = tempLine.Functions[functionIndex];
            }
            else
            {
                viewmodel.Function = new FunctionLine
                {
                    FunctionType = functionType,
                    FunctionTypeID = FunctionTypeID,
                    Line = tempLine.Line,
                    LineID = LineID,
                };
            }


            AppUtility.ParentFolderName parentFolderName = AppUtility.ParentFolderName.FunctionLine;
            string uploadProtocolsFolder = Path.Combine(_hostingEnvironment.WebRootPath, parentFolderName.ToString());
            string uploadProtocolsFolder2 = Path.Combine(uploadProtocolsFolder, viewmodel.Function.ID.ToString());
            switch (Enum.Parse<AppUtility.ProtocolFunctionTypes>(functionType.DescriptionEnum))
            {
                case AppUtility.ProtocolFunctionTypes.AddLinkToProduct:
                    GetFunctionLineLinkToProductDDls(viewmodel);
                    break;
                case AppUtility.ProtocolFunctionTypes.AddLinkToProtocol:
                    GetFunctionLineLinkToProtocolDDLs(viewmodel);
                    break;
                case AppUtility.ProtocolFunctionTypes.AddFile:
                case AppUtility.ProtocolFunctionTypes.AddImage:
                    var folderName = AppUtility.FolderNamesEnum.Files;
                    if (functionType.DescriptionEnum == AppUtility.ProtocolFunctionTypes.AddImage.ToString())
                    {
                        folderName = AppUtility.FolderNamesEnum.Pictures;
                    }
                    DocumentsModalViewModel documentsModalViewModel = new DocumentsModalViewModel()
                    {
                        FolderName = folderName,
                        ParentFolderName = AppUtility.ParentFolderName.FunctionLine,
                        ObjectID = viewmodel.Function.ID.ToString(),
                        SectionType = AppUtility.MenuItems.Protocols,
                        IsEdittable = modalType!=AppUtility.ProtocolModalType.Summary,
                        DontAllowMultiple = true,
                        ShowSwitch = false,
                        Guid = guid
                    };
                    base.FillDocumentsViewModel(documentsModalViewModel);
                    viewmodel.DocumentsModalViewModel = documentsModalViewModel;
                    break;
            }
            return PartialView(viewmodel);
        }

        [Authorize(Roles = "Protocols")]
        public async Task<IActionResult> AddResultsFunctionModal(int FunctionTypeID, int protocolInstanceID, int functionResultID, AppUtility.ProtocolModalType modalType, string closingTags)
        {
            var functionType = _context.FunctionTypes.Where(ft => ft.FunctionTypeID == FunctionTypeID).FirstOrDefault();
            var viewmodel = new AddResultsFunctionViewModel
            {
                ModalType = modalType,
                ClosingTags = closingTags
            };
            if (functionResultID !=0)
            {
                viewmodel.Function = _context.FunctionResults.Where(fr=>fr.ID == functionResultID).FirstOrDefault();
            }
            else
            {
                viewmodel.Function = new FunctionResult
                {
                    FunctionType = functionType,
                    FunctionTypeID = FunctionTypeID,
                    ProtocolInstanceID = protocolInstanceID,
                    ProtocolInstance = _context.ProtocolInstances.Where(pi => pi.ProtocolInstanceID == protocolInstanceID).FirstOrDefault()
                };
            }


            AppUtility.ParentFolderName parentFolderName = AppUtility.ParentFolderName.FunctionLine;
            string uploadProtocolsFolder = Path.Combine(_hostingEnvironment.WebRootPath, parentFolderName.ToString());
            string uploadProtocolsFolder2 = Path.Combine(uploadProtocolsFolder, viewmodel.Function.ID.ToString());
            switch (Enum.Parse<AppUtility.ProtocolFunctionTypes>(functionType.DescriptionEnum))
            {
                case AppUtility.ProtocolFunctionTypes.AddLinkToProduct:
                    GetFunctionLineLinkToProductDDls(viewmodel);
                    break;
                case AppUtility.ProtocolFunctionTypes.AddLinkToProtocol:
                    GetFunctionLineLinkToProtocolDDLs(viewmodel);
                    break;
                case AppUtility.ProtocolFunctionTypes.AddFile:
                case AppUtility.ProtocolFunctionTypes.AddImage:
                    var folderName = AppUtility.FolderNamesEnum.Files;
                    if (functionType.DescriptionEnum == AppUtility.ResultsFunctionTypes.AddImage.ToString())
                    {
                        folderName = AppUtility.FolderNamesEnum.Pictures;
                    }
                    DocumentsModalViewModel documentsModalViewModel = new DocumentsModalViewModel()
                    {
                        FolderName = folderName,
                        ParentFolderName = AppUtility.ParentFolderName.FunctionResults,
                        ObjectID = viewmodel.Function.ID.ToString(),
                        SectionType = AppUtility.MenuItems.Protocols,
                        IsEdittable = modalType != AppUtility.ProtocolModalType.Summary,
                        DontAllowMultiple = true,
                        ShowSwitch = false,
                        Guid = Guid.NewGuid()
                    };
                    base.FillDocumentsViewModel(documentsModalViewModel);
                    viewmodel.DocumentsModalViewModel = documentsModalViewModel;
                    
                    break;
            }
            return PartialView(viewmodel);
        }

        [HttpPost]
        [Authorize(Roles = "Protocols")]
        public async Task<IActionResult> AddResultsFunctionModal(AddResultsFunctionViewModel addResultsFunctionViewModel, ProtocolInstance protocolInstance, Guid? guid)
        {
            var functionType = _context.FunctionTypes.Where(ft=>ft.FunctionTypeID == addResultsFunctionViewModel.Function.FunctionTypeID).FirstOrDefault();

            var protocolInstanceDB = _context.ProtocolInstances.Where(r => r.ProtocolInstanceID == protocolInstance.ProtocolInstanceID).FirstOrDefault();
            protocolInstanceDB.TemporaryResultDescription = protocolInstance.TemporaryResultDescription;
            var functionResult = addResultsFunctionViewModel.Function;
            string renderedView = "";
            functionResult.IsTemporary = true;
            using (var transaction = _context.Database.BeginTransaction())
            {
                try
                {
                    if (addResultsFunctionViewModel.IsRemove)
                    {
                        functionResult.IsTemporaryDeleted = true;
                        _context.Entry(functionResult).State = EntityState.Modified;
                        _context.Update(protocolInstanceDB);
                    }
                    else
                    {
                        _context.Entry(functionResult).State = EntityState.Added;
                        await _context.SaveChangesAsync();
                        string functionIconHtml =  "<button  class='function p-0 m-0 no-box-shadow border-0' type='button' typeID='"+functionType.FunctionTypeID+"' modalType='"+addResultsFunctionViewModel.ModalType+"' guid='"+guid+"' value='"+functionResult.ID+ "'> <div functionID = '" + functionType.FunctionTypeID + "' class='" + functionType.IconActionClass + " line-function'><i class='" + functionType.Icon + "'></i></div></button>";
                        switch (Enum.Parse<AppUtility.ProtocolFunctionTypes>(functionType.DescriptionEnum))
                        {
                            case AppUtility.ProtocolFunctionTypes.AddLinkToProduct:
                                var product = _context.Products.Where(p => p.ProductID == addResultsFunctionViewModel.Function.ProductID).FirstOrDefault();
                                renderedView = " <a href='#' class='open-line-product function-result-node' functionResult='" + addResultsFunctionViewModel.Function.ID + "' value='" + product.ProductID + "'>" + product.ProductName + "</a> "+functionIconHtml;
                                break;
                            case AppUtility.ProtocolFunctionTypes.AddLinkToProtocol:
                                var protocol = _context.ProtocolVersions.Include(pv=>pv.Protocol).Include(p => p.Materials).Where(p => p.ProtocolVersionID == addResultsFunctionViewModel.Function.ProtocolVersionID).FirstOrDefault();
                                renderedView = " <a href='#' functionResult='" + addResultsFunctionViewModel.Function.ID + "' class='open-line-protocol function-result-node' value='" + protocol.ProtocolID + "'>" + protocol.Protocol.Name + " </a> "+functionIconHtml;
                                break;
                            case AppUtility.ProtocolFunctionTypes.AddFile:
                            case AppUtility.ProtocolFunctionTypes.AddImage:            
                                MoveDocumentsOutOfTempFolder(addResultsFunctionViewModel.Function.ID, AppUtility.ParentFolderName.FunctionResults, guid: guid);
                                var folderName = AppUtility.FolderNamesEnum.Files;
                                if (functionType.DescriptionEnum == AppUtility.ResultsFunctionTypes.AddImage.ToString())
                                {
                                    folderName = AppUtility.FolderNamesEnum.Pictures;
                                }
                                DocumentsModalViewModel documentsModalViewModel = new DocumentsModalViewModel()
                                {
                                    ObjectID = functionResult.ID.ToString(),
                                    ParentFolderName = AppUtility.ParentFolderName.FunctionResults,
                                    SectionType = AppUtility.MenuItems.Protocols,
                                    IsEdittable = true,
                                    Guid = guid ?? Guid.NewGuid(),
                                    FolderName = folderName
                                };

                                base.FillDocumentsViewModel(documentsModalViewModel);

                                renderedView = await RenderPartialViewToString("_DocumentCard", documentsModalViewModel);                               
                                break;
                            default:
                                renderedView = functionIconHtml;
                                break;
                        }
                        renderedView = "<div class='result-function my-3' functionResultID='" + functionResult.ID + "'>" + renderedView + "</div>";
                        var replaceableText = "<span class=\"focusedText\"></span>";
                        var tags = addResultsFunctionViewModel.ClosingTags?.Split(",") ?? new string[0];
                        var closingTags = "";
                        var openingTags = "";
                        foreach (var tag in tags)
                        {
                            closingTags += "</" + tag + ">";
                            openingTags = "<" + tag + ">" + openingTags;
                        }
                        var addedText = renderedView + " <div contenteditable='true' class= 'editable-span form-control-plaintext text-transform-none text added-div start-div'></div>";

                        if ( !protocolInstanceDB.TemporaryResultDescription.Contains(replaceableText))
                        {
                            protocolInstanceDB.TemporaryResultDescription += addedText;
                        }
                        else
                        {
                            addedText = closingTags + addedText + openingTags;
                            protocolInstanceDB.TemporaryResultDescription = protocolInstanceDB.TemporaryResultDescription.Replace(replaceableText, addedText);
                        }
                        _context.Update(protocolInstanceDB);

                    }
                    await _context.SaveChangesAsync();
                    await transaction.CommitAsync();
                }
                catch (Exception ex)
                {
                    Response.StatusCode = 500;
                    await transaction.RollbackAsync();
                }
            }
            return PartialView("_ResultsText", protocolInstanceDB);
        }

        [HttpGet]
        [Authorize(Roles = "Protocols")]
        public async Task<IActionResult> DeleteResultsDocumentModal(int FunctionResultID)
        {
            var functionResult = _context.FunctionResults.Where(fr => fr.ID == FunctionResultID).FirstOrDefault();
            var functionType = _context.FunctionTypes.Where(ft => ft.FunctionTypeID == functionResult.FunctionTypeID).FirstOrDefault();
            AppUtility.ParentFolderName parentFolderName = AppUtility.ParentFolderName.FunctionResults;
            string uploadReportsFolder = Path.Combine(_hostingEnvironment.WebRootPath, parentFolderName.ToString());
            string uploadReportsFolder2 = Path.Combine(uploadReportsFolder, FunctionResultID.ToString());
            var folderName = AppUtility.FolderNamesEnum.Files;
            if (functionType.DescriptionEnum == AppUtility.ResultsFunctionTypes.AddImage.ToString())
            {
                folderName = AppUtility.FolderNamesEnum.Pictures;
            }
            var deleteDocumentViewModel = new DeleteResultDocumentViewModel()
            {
                FunctionResult = functionResult,
                ResultID = functionResult.ProtocolInstanceID
            };

            deleteDocumentViewModel.DocumentsInfo = new List<DocumentFolder>();
            base.GetExistingFileStrings(deleteDocumentViewModel.DocumentsInfo, folderName, parentFolderName, uploadReportsFolder2, FunctionResultID.ToString());
            return PartialView(deleteDocumentViewModel);
        }


        [HttpPost]
        [Authorize(Roles = "Protocols")]
        public async Task<IActionResult> DeleteResultsDocumentModal(DeleteResultDocumentViewModel deleteDocumentViewModel, ProtocolInstance protocolInstance)
        {
            var protocolInstanceDB = _context.ProtocolInstances.Where(r => r.ProtocolInstanceID == deleteDocumentViewModel.ResultID).FirstOrDefault();
            using (var transaction = _context.Database.BeginTransaction())
            {
                try
                {
                    deleteDocumentViewModel.FunctionResult.IsTemporaryDeleted = true;
                    _context.Entry(deleteDocumentViewModel.FunctionResult).State = EntityState.Modified;
                    protocolInstanceDB.TemporaryResultDescription = protocolInstance.TemporaryResultDescription;
                    _context.Update(protocolInstanceDB);

                    await _context.SaveChangesAsync();
                    base.DeleteTemporaryDocuments(AppUtility.ParentFolderName.FunctionResults, ObjectID: deleteDocumentViewModel.FunctionResult.ID);

                    await transaction.CommitAsync();
                }
                catch (Exception ex)
                {

                }
            }
            return PartialView("_ResultsText", protocolInstanceDB);
        }



        [HttpPost]
        public async Task<IActionResult> SaveResults(ProtocolInstance protocolInstance, AddResultsFunctionViewModel addResultsFunctionViewModel)
        {
            var protocolInstanceDB = _context.ProtocolInstances.Where(r => r.ProtocolInstanceID == protocolInstance.ProtocolInstanceID).FirstOrDefault();
            using (var transaction = _context.Database.BeginTransaction())
            {
                try
                {

                    protocolInstanceDB.ResultDescription = protocolInstance.TemporaryResultDescription;
                    protocolInstanceDB.TemporaryResultDescription = null;
                    _context.Update(protocolInstanceDB);
                    var functionResults = _context.FunctionResults.Where(fr => fr.ProtocolInstanceID == protocolInstance.ProtocolInstanceID && fr.IsTemporary);
                    foreach (var functionResult in functionResults)
                    {
                        functionResult.IsTemporary = false;
                        _context.Update(functionResult);
                    }
                    var deletedFunctionResults = _context.FunctionResults.Where(fr => fr.ProtocolInstanceID == protocolInstance.ProtocolInstanceID && fr.IsTemporaryDeleted);
                    foreach (var fr in deletedFunctionResults)
                    {
                        _context.Remove(fr);
                    }
                    await _context.SaveChangesAsync();
                    await transaction.CommitAsync();
                }
                catch (Exception ex)
                {

                }
            }
            return new EmptyResult();
        }


        private void GetFunctionLineLinkToProtocolDDLs(AddFunctionViewModel<FunctionLine> viewmodel)
        {
            viewmodel.ProtocolCategories = _context.ProtocolCategories.ToList();
            viewmodel.ProtocolSubCategories = _context.ProtocolSubCategories.ToList();
            viewmodel.Creators = _context.Users.Select(u =>
                new SelectListItem() { Value = u.Id, Text = u.FirstName + u.LastName }).ToList();
            viewmodel.ProtocolVersions = _context.ProtocolVersions.Include(p=>p.Protocol).ToList();
        }

        private void GetFunctionLineLinkToProductDDls(AddFunctionViewModel<FunctionLine> viewmodel)
        {
            viewmodel.ParentCategories = _context.ParentCategories.ToList();
            viewmodel.ProductSubcategories = _context.ProductSubcategories.ToList();
            viewmodel.Products = _context.Products.Where(p => p.Requests.Where(r => r.RequestStatusID == 3 || r.RequestStatusID == 7).Any()).ToList();
            viewmodel.Vendors = _context.Vendors.ToList();
        }

        private void GetFunctionLineLinkToProtocolDDLs(AddFunctionViewModel<FunctionResult> viewmodel)
        {
            viewmodel.ProtocolCategories = _context.ProtocolCategories.ToList();
            viewmodel.ProtocolSubCategories = _context.ProtocolSubCategories.ToList();
            viewmodel.Creators = _context.Users.Select(u =>
                new SelectListItem() { Value = u.Id, Text = u.FirstName + u.LastName }).ToList();
            viewmodel.ProtocolVersions = _context.ProtocolVersions.ToList();
        }

        private void GetFunctionLineLinkToProductDDls(AddFunctionViewModel<FunctionResult> viewmodel)
        {
            viewmodel.ParentCategories = _context.ParentCategories.ToList();
            viewmodel.ProductSubcategories = _context.ProductSubcategories.ToList();
            viewmodel.Products = _context.Products.Where(p=>p.Requests.Where(r=>r.RequestStatusID==3 || r.RequestStatusID==7).Any()).ToList();
            viewmodel.Vendors = _context.Vendors.ToList();
        }

        [Authorize(Roles = "Protocols")]
        public async Task<IActionResult> LinkMaterialToProductModal(int materialID)
        {
            var material = _context.Materials.Where(m => m.MaterialID == materialID).FirstOrDefault();
            return PartialView(new AddMaterialViewModel { Material = material });
        }


        [HttpPost]
        [Authorize(Roles = "Protocols")]
        public async Task<IActionResult> LinkMaterialToProductModal(AddMaterialViewModel addMaterialViewModel)
        {
            var materialDB = _context.Materials.Where(m => m.MaterialID == addMaterialViewModel.Material.MaterialID).FirstOrDefault();
            using (var transaction = _context.Database.BeginTransaction())
            {
                try
                {
                    var product = _context.Products.Where(p => p.SerialNumber == addMaterialViewModel.Material.Product.SerialNumber).FirstOrDefault();
                    materialDB.ProductID = product.ProductID;
                    materialDB.Name = null;
                    _context.Entry(materialDB).State = EntityState.Modified;
                    await _context.SaveChangesAsync();
                    await transaction.CommitAsync();
                }
                catch (Exception ex)
                {
                    Response.StatusCode = 500;
                    await transaction.RollbackAsync();
                    return PartialView("LinkMaterialToProductModal", new AddMaterialViewModel { Material = _context.Materials.Where(m => m.MaterialID == addMaterialViewModel.Material.MaterialID).FirstOrDefault(), ErrorMessage = AppUtility.GetExceptionMessage(ex) });
                }
                return redirectToMaterialTab(materialDB.ProtocolVersionID);
            }
        }

        public async Task<IActionResult> _AddFunctionModal(int objectID, string uniqueNumber, int functionTypeID)
        {
            var functionType = _context.FunctionTypes.Where(ft => ft.FunctionTypeID == functionTypeID).FirstOrDefault();

            var viewmodel = new AddFunctionViewModel<FunctionBase>
            {
                Function = new FunctionBase()
            };
            viewmodel.Function.FunctionType = functionType;
            switch (Enum.Parse<AppUtility.ProtocolFunctionTypes>(functionType.DescriptionEnum))
            {
                case AppUtility.ProtocolFunctionTypes.AddLinkToProduct:
                    var product = _context.Products.IgnoreQueryFilters().Where(p=>!p.IsDeleted).Where(p => p.Requests.Where(r => r.RequestStatusID == 3 || r.RequestStatusID == 7).Any()).Where(p => p.ProductID == objectID || p.SerialNumber == uniqueNumber)
                         .Include(p => p.ProductSubcategory).FirstOrDefault();
                    if(product != null)
                    {
                        viewmodel.Function.Product = product;
                        viewmodel.ProductSubcategories = _context.ProductSubcategories.Where(ps => ps.ParentCategoryID == product.ProductSubcategory.ParentCategoryID).ToList();
                        viewmodel.Products = _context.Products.Where(p => p.ProductSubcategoryID == product.ProductSubcategoryID && product.VendorID == p.VendorID).Where(p => p.Requests.Where(r => r.RequestStatusID == 3 || r.RequestStatusID == 7).Any()).ToList();

                        viewmodel.Function.ProductID = product.ProductID;
                    }
                    else
                    {
                        viewmodel.Function.Product = new Product();
                        viewmodel.ProductSubcategories = _context.ProductSubcategories.ToList();
                        viewmodel.Products = _context.Products.Where(p => p.Requests.Where(r => r.RequestStatusID == 3 || r.RequestStatusID == 7).Any()).ToList();

                    }

                    viewmodel.ParentCategories = _context.ParentCategories.ToList();
                    viewmodel.Vendors = _context.Vendors.ToList();

                    break;
                case AppUtility.ProtocolFunctionTypes.AddLinkToProtocol:
                    var protocol = _context.ProtocolVersions.Include(p=>p.Protocol).Where(p => p.ProtocolVersionID == objectID || p.Protocol.UniqueCode == uniqueNumber).Include(ps => ps.Protocol.ProtocolSubCategory).FirstOrDefault();
                    viewmodel.Function.ProtocolVersion = protocol;
                    viewmodel.Function.ProtocolVersionID = protocol.ProtocolVersionID;
                    viewmodel.ProtocolCategories = _context.ProtocolCategories.ToList();
                    viewmodel.ProtocolSubCategories = _context.ProtocolSubCategories.Where(ps => ps.ProtocolCategoryTypeID == protocol.Protocol.ProtocolSubCategory.ProtocolCategoryTypeID).ToList();
                    viewmodel.Creators = _context.Users.Select(u =>
                        new SelectListItem() { Value = u.Id, Text = u.FirstName + u.LastName }).ToList();
                    viewmodel.ProtocolVersions = _context.ProtocolVersions.Include(p=>p.Protocol).Where(p => p.Protocol.ProtocolSubCategoryID == protocol.Protocol.ProtocolSubCategoryID && p.ApplicationUserCreatorID == protocol.ApplicationUserCreatorID).ToList();
                    break;
            }
            return PartialView(viewmodel);
        }

        [HttpPost]
        [Authorize(Roles = "Protocols")]
        public async Task<IActionResult> AddFunctionModal(AddLineFunctionViewModel addFunctionViewModel, List<Line> Lines)
        {
            using (var transaction = _context.Database.BeginTransaction())
            {
                var tempLinesJson = await _context.TempLinesJsons.Where(tlj => tlj.TempLinesJsonID == addFunctionViewModel.UniqueGuid).FirstOrDefaultAsync();
                var tempLines = tempLinesJson.DeserializeJson<ProtocolsLinesViewModel>();
                tempLines.Lines = tempLines.Lines.Where(tl => !tl.Line.IsTemporaryDeleted).ToList();
                await UpdateLineContentAsync(tempLines, Lines);
                var tempLine = tempLines.Lines.Where(tl => tl.Line.LineID == addFunctionViewModel.Function.LineID).FirstOrDefault();
                var protocolID = tempLine.Line.ProtocolVersionID;
                try
                {
                    if (addFunctionViewModel.IsRemove)
                    {
                        var functionType = _context.FunctionTypes.Where(f => f.FunctionTypeID == addFunctionViewModel.Function.FunctionTypeID).FirstOrDefault();
                        addFunctionViewModel.Function.IsTemporaryDeleted = true;
                        if (_context.FunctionLines.Where(fl => fl.ID == addFunctionViewModel.Function.ID).Any())
                        {
                            _context.Entry(addFunctionViewModel.Function).State = EntityState.Modified;
                        }
                        tempLine.Functions.RemoveAt(addFunctionViewModel.FunctionIndex);
                        if (functionType.DescriptionEnum == AppUtility.ProtocolFunctionTypes.AddFile.ToString() || functionType.DescriptionEnum == AppUtility.ProtocolFunctionTypes.AddImage.ToString())
                        {
                            base.DeleteTemporaryDocuments(AppUtility.ParentFolderName.FunctionLine, ObjectID: addFunctionViewModel.Function.ID);
                        }
                    }
                    else
                    {

                        var functionType = _context.FunctionTypes.Where(ft => ft.FunctionTypeID == addFunctionViewModel.Function.FunctionTypeID).FirstOrDefault();

                        if (addFunctionViewModel.Function.ID == 0)
                        {
                            var functionLineID = new FunctionLineID();
                            _context.Add(functionLineID);
                            await _context.SaveChangesAsync();
                            addFunctionViewModel.Function.ID = functionLineID.ID;
                        }
                        switch (Enum.Parse<AppUtility.ProtocolFunctionTypes>(functionType.DescriptionEnum))
                        {
                            case AppUtility.ProtocolFunctionTypes.AddLinkToProduct:
                                var product = _context.Products.Where(p => p.ProductID == addFunctionViewModel.Function.ProductID).FirstOrDefault();
                                tempLine.Line.Content += " <a href='#' class='open-line-product function-line-node' functionline='" + addFunctionViewModel.Function.ID + "' value='" + product.ProductID + "'>" + product.ProductName + "</a> " + " <div role='textbox' contenteditable  class='editable-span line input line-input text-transform-none'> </div>";
                                addFunctionViewModel.Function.Product = product;
                                break;
                            case AppUtility.ProtocolFunctionTypes.AddLinkToProtocol:
                                var protocol = _context.ProtocolVersions.Include(p=>p.Protocol).Include(p => p.Materials).Where(p => p.ProtocolVersionID == addFunctionViewModel.Function.ProtocolVersionID).FirstOrDefault();
                                tempLine.Line.Content += " <a href='#' functionline='" + addFunctionViewModel.Function.ID + "' class='open-line-protocol function-line-node' value='" + protocol.ProtocolVersionID + "'>" + protocol.Protocol.Name + " </a> " + " <div role='textbox' contenteditable  class='editable-span line input line-input text-transform-none'> </div>"; ;
                                addFunctionViewModel.Function.ProtocolVersion = protocol;
                                break;
                            case AppUtility.ProtocolFunctionTypes.AddFile:
                            case AppUtility.ProtocolFunctionTypes.AddImage:
                                MoveDocumentsOutOfTempFolder(addFunctionViewModel.Function.ID, AppUtility.ParentFolderName.FunctionLine, guid: addFunctionViewModel.UniqueGuid);
                                break;
                            case AppUtility.ProtocolFunctionTypes.AddStop:
                            case AppUtility.ProtocolFunctionTypes.AddTimer:
                            case AppUtility.ProtocolFunctionTypes.AddTip:
                            case AppUtility.ProtocolFunctionTypes.AddWarning:
                            case AppUtility.ProtocolFunctionTypes.AddComment:
                                break;
                        }
                        addFunctionViewModel.Function.FunctionType = functionType;
                        if (tempLine.Functions == null)
                        {
                            tempLine.Functions = new List<FunctionLine>();
                        }

                        if (tempLine.Functions?.Count() > addFunctionViewModel.FunctionIndex && addFunctionViewModel.FunctionIndex != -1)
                        {
                            tempLine.Functions[addFunctionViewModel.FunctionIndex] = addFunctionViewModel.Function;
                        }
                        else
                        {
                            tempLine.Functions.Add(addFunctionViewModel.Function);
                        }
                    }

                    tempLinesJson.SerializeViewModel(tempLines);
                    _context.Update(tempLinesJson);
                    await _context.SaveChangesAsync();
                    await transaction.CommitAsync();
                }
                catch (Exception ex)
                {
                    Response.StatusCode = 500;
                    await transaction.RollbackAsync();
                    //  await Response.WriteAsync(AppUtility.GetExceptionMessage(ex));
                }
                return PartialView("_Lines", await OrderLinesForViewAsync(false, tempLine.Line.ProtocolVersionID, addFunctionViewModel.ModalType, addFunctionViewModel.UniqueGuid));

            }
        }

        [HttpGet]
        [Authorize(Roles = "Protocols")]
        public async Task<JsonResult> FilterLinkToProtocol(int parentCategoryID, int subCategoryID, string creatorID)
        {
            IQueryable<ProtocolVersion> protocolsList = _context.ProtocolVersions.Include(p=>p.Protocol);
            if (subCategoryID != 0)
            {
                protocolsList = protocolsList.Where(p => p.Protocol.ProtocolSubCategoryID == subCategoryID);
            }
            else if (parentCategoryID != 0)
            {
                protocolsList = protocolsList.Where(p => p.Protocol.ProtocolSubCategory.ProtocolCategoryTypeID == parentCategoryID);
            }
            if (creatorID != null)
            {
                protocolsList = protocolsList.Where(p => p.ApplicationUserCreatorID == creatorID);
            }
            var protocolListJson = await protocolsList.Select(p => new { protocolID = p.ProtocolVersionID, name = p.Protocol.Name }).ToListAsync();
            var subCategoryList = await _context.ProtocolSubCategories.Where(ps => ps.ProtocolCategoryTypeID == parentCategoryID).Select(ps => new { subCategoryID = ps.ProtocolCategoryTypeID, subCategoryDescription = ps.ProtocolSubCategoryTypeDescription }).ToListAsync();
            return Json(new { ProtocolSubCategories = subCategoryList, Protocols = protocolListJson });
        }

        [HttpGet]
        [Authorize(Roles = "Protocols")]
        public async Task<JsonResult> FilterLinkToProduct(int parentCategoryID, int subCategoryID, int vendorID)
        {
            IQueryable<Product> products = _context.Products.Where(p => p.Requests.Where(r => r.RequestStatusID == 3 || r.RequestStatusID == 7).Any());
            if (subCategoryID != 0)
            {
                products = products.Where(p => p.ProductSubcategoryID == subCategoryID);
            }
            else if (parentCategoryID != 0)
            {
                products = products.Where(p => p.ProductSubcategory.ParentCategoryID == parentCategoryID);
            }
            if (vendorID != 0)
            {
                products = products.Where(p => p.VendorID == vendorID);
            }
            var productsJson = await products.Select(p => new { productID = p.ProductID, name = p.ProductName }).ToListAsync();
            var subCategoryList = await _context.ProductSubcategories.Where(ps => ps.ParentCategoryID == parentCategoryID).Select(ps => new { subCategoryID = ps.ProductSubcategoryID, subCategoryDescription = ps.ProductSubcategoryDescription }).ToListAsync();
            return Json(new { ProductSubCategories = subCategoryList, Products = productsJson });
        }

        [HttpPost]
        [Authorize(Roles = "Protocols")]
        public async Task<IActionResult> AddMaterialModal(AddMaterialViewModel addMaterialViewModel, Guid guid)
        {
            var Protocol = _context.Protocols.Where(p => p.ProtocolID == addMaterialViewModel.Material.ProtocolVersionID).FirstOrDefault();
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
                    base.MoveDocumentsOutOfTempFolder(addMaterialViewModel.Material.MaterialID, AppUtility.ParentFolderName.Materials, guid: guid);
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
            return redirectToMaterialTab(addMaterialViewModel.Material.ProtocolVersionID);
        }

        private IActionResult redirectToMaterialTab(int protocolID)
        {
            string uploadProtocolsFolder = Path.Combine(_hostingEnvironment.WebRootPath, AppUtility.ParentFolderName.Materials.ToString());
            var materials = _context.Materials.Include(m => m.Product).Where(m => m.ProtocolVersionID == protocolID);
            Dictionary<Material, List<DocumentFolder>> MaterialFolders = FillMaterialDocumentsModel(materials, uploadProtocolsFolder);
            return PartialView("_MaterialTab", new MaterialTabViewModel() { Materials = materials.ToList(), MaterialCategories = _context.MaterialCategories, Folders = (Lookup<Material, List<DocumentFolder>>)MaterialFolders.ToLookup(o => o.Key, o => o.Value) });
        }

        [Authorize(Roles = "Protocols")]
        public async Task<IActionResult> ProtocolsProductDetails(int? productID)
        {
            RequestItemViewModel requestItemViewModel = await GetProtocolsProductDetailsFunction(productID);
            return PartialView(requestItemViewModel);
        }
        [Authorize(Roles = "Protocols")]
        public async Task<IActionResult> _ProtocolsProductDetails(int? productID, List<string> lastUrls, bool backButtonClicked)
        {
            RequestItemViewModel requestItemViewModel = await GetProtocolsProductDetailsFunction(productID);
            if (backButtonClicked)
            {
                lastUrls.RemoveAt(lastUrls.Count - 1);
            }
            else
            {
                lastUrls.Add(Request.Path + Request.QueryString);
            }
            requestItemViewModel.LastUrls = lastUrls;
            return PartialView(requestItemViewModel);
        }

        private async Task<RequestItemViewModel> GetProtocolsProductDetailsFunction(int? productID)
        {
            var requestID = _context.Requests.IgnoreQueryFilters().Where(r => !r.IsDeleted).Where(r => r.ProductID == productID).OrderByDescending(r => r.ParentRequest.OrderDate).Select(r => r.RequestID).FirstOrDefault();
            var requestItemViewModel = await editModalViewFunction(requestID, isEditable: false);
            requestItemViewModel.SectionType = AppUtility.MenuItems.Protocols;
            return requestItemViewModel;
        }

        [Authorize(Roles = "Protocols")]
        public async Task<IActionResult> ProtocolsDetailsFloatModal(int? protocolID)
        {
            CreateProtocolsViewModel createProtocolsViewModel = await GetProtocolsDetailsFloatModalFunction(protocolID);
            createProtocolsViewModel.LastUrls = new List<string>() {Request.Path+Request.QueryString };
            return PartialView(createProtocolsViewModel);
        }

        [Authorize(Roles = "Protocols")]
        public async Task<IActionResult> _ProtocolsDetailsFloatModal(int? protocolID, List<string> lastUrls, bool backButtonClicked)
        {
            CreateProtocolsViewModel createProtocolsViewModel = await GetProtocolsDetailsFloatModalFunction(protocolID);
            if(backButtonClicked)
            {
                lastUrls.RemoveAt(lastUrls.Count-1);
            }
            else
            {
                lastUrls.Add(Request.Path + Request.QueryString);
            }

            createProtocolsViewModel.LastUrls = lastUrls;
            
            return PartialView(createProtocolsViewModel);
        }

        private async Task<CreateProtocolsViewModel> GetProtocolsDetailsFloatModalFunction(int? protocolID)
        {
            var protocol = _context.ProtocolVersions.Include(p=>p.Protocol).Where(p => p.ProtocolVersionID == protocolID).FirstOrDefault();
            var createProtocolsViewModel = new CreateProtocolsViewModel();
            createProtocolsViewModel.ModalType = AppUtility.ProtocolModalType.SummaryFloat;
            await FillCreateProtocolsViewModel(createProtocolsViewModel, protocol.Protocol.ProtocolTypeID, protocol.ProtocolVersionID);
            return createProtocolsViewModel;
        }

        [HttpGet]
        [Authorize(Roles = "Protocols")]
        public async Task<IActionResult> CreateNewVersion(int protocolVersionID)
        {
            TempData[AppUtility.TempDataTypes.MenuType.ToString()] = AppUtility.MenuItems.Protocols;
            TempData[AppUtility.TempDataTypes.SidebarType.ToString()] = AppUtility.SidebarEnum.ResearchProtocol;
            TempData[AppUtility.TempDataTypes.PageType.ToString()] = AppUtility.PageTypeEnum.ProtocolsCreate;
            var protocol = await _context.ProtocolVersions.Include(pv => pv.Protocol).Where(pv => pv.ProtocolVersionID == protocolVersionID).AsNoTracking().FirstOrDefaultAsync();

            using (var transaction = _context.Database.BeginTransaction())
            {
                try
                {
                    
                    protocol.VersionNumber += 1;
                    protocol.ProtocolVersionID = 0;
                    protocol.ApplicationUserCreatorID = _userManager.GetUserAsync(User).Result.Id;
                    var protocolLines = await _context.Lines.Include(l => l.LineType).Include(l => l.ParentLine)
                        .Include(l => l.LineChange).Include(l => l.FunctionLines)
                        .Where(l => l.ProtocolVersionID == protocolVersionID).AsNoTracking().ToListAsync();

                    _context.Entry(protocol).State = EntityState.Added;
                    await _context.SaveChangesAsync();
                    var parentLines = protocolLines.Where(pl => pl.ParentLineID == null);
                    foreach(var parent in parentLines)
                    {
                       await Copy(protocolLines, parent, protocol.ProtocolVersionID);
                    }                 
               
                    await transaction.CommitAsync();
                }
                catch (Exception ex)
                {

                }

            }

            CreateProtocolsViewModel viewmodel = new CreateProtocolsViewModel();
            await FillCreateProtocolsViewModel(viewmodel, protocol.Protocol.ProtocolTypeID, protocol.ProtocolVersionID);
            viewmodel.ModalType = AppUtility.ProtocolModalType.CreateNewVersion;
            return View("ResearchProtocol", viewmodel);
        }

        public async Task<Line> Copy(List<Line> lines, Line origin, int protocolVersionID, Line parent = null)
        {
            if (origin == null)
            {
                return null;
            }
            origin.ParentLineID = parent?.LineID;
            origin.ProtocolVersionID = protocolVersionID;
            var oldLineID = origin.LineID;
            var templineID = new TempLineID();
            _context.Add(templineID);
            await _context.SaveChangesAsync();         
            origin.LineID = templineID.ID;
            _context.Entry(origin).State = EntityState.Added;
            await _context.SaveChangesAsync();
            lines.Where(l => l.ParentLineID == oldLineID).Select(async x => await Copy(lines, x, protocolVersionID, origin)).ToList();
            return origin;
        }

        private List<Line> OrderLines(List<Line> lines) 
        {
            Stack<Line> linesStack = new Stack<Line>();
            List<Line> orderedLines = new List<Line>();
            lines.Where(l => l.ParentLineID == null).OrderByDescending(l => l.LineNumber).ToList().ForEach(tl => { linesStack.Push(tl); });
            while (!linesStack.IsEmpty())
            {
                var node = linesStack.Pop();
                
                lines.Where(c => c.ParentLineID == node.LineID).OrderByDescending(tl => tl.LineNumber).ToList().ForEach(c => { linesStack.Push(c); });
                orderedLines.Add(node);
            }
            return orderedLines;
        }

        [HttpPost]
        [Authorize(Roles = "Protocols")]
        public async Task<IActionResult> CreateProtocol(CreateProtocolsViewModel createProtocolsViewModel, List<Line> Lines, bool IncludeSaveLines)
        {

            using (var transaction = _context.Database.BeginTransaction())
            {
                try
                {
                    createProtocolsViewModel.ProtocolVersion.Urls = createProtocolsViewModel.ProtocolVersion.Urls?.Where(u => u.LinkDescription != null && u.Url != null)?.ToList();
      
                    if (createProtocolsViewModel.ProtocolVersion.Protocol.ProtocolID == 0)
                    {
                        var lastProtocolNum = GetUniqueNumber();
                        createProtocolsViewModel.ProtocolVersion.Protocol.UniqueCode = lastProtocolNum; 
                        _context.Entry(createProtocolsViewModel.ProtocolVersion.Protocol).State = EntityState.Added;
                        await _context.SaveChangesAsync();
                        createProtocolsViewModel.ProtocolVersion.CreationDate = DateTime.Now;
                        createProtocolsViewModel.ProtocolVersion.ApplicationUserCreatorID = _userManager.GetUserId(User);
                        _context.Entry(createProtocolsViewModel.ProtocolVersion).State = EntityState.Added;
                        await _context.SaveChangesAsync();
                        Lines.ForEach(l => { l.ProtocolVersion = createProtocolsViewModel.ProtocolVersion; _context.Add(l); _context.SaveChanges(); });
                        base.MoveDocumentsOutOfTempFolder(createProtocolsViewModel.ProtocolVersion.ProtocolVersionID, AppUtility.ParentFolderName.Protocols, guid: createProtocolsViewModel.UniqueGuid);
                    }
                    else
                    {
                        if(createProtocolsViewModel.ProtocolVersion.ProtocolVersionID ==0)
                        {
                            createProtocolsViewModel.ProtocolVersion.CreationDate = DateTime.Now;
                            createProtocolsViewModel.ProtocolVersion.ApplicationUserCreatorID = _userManager.GetUserId(User);
                            _context.Entry(createProtocolsViewModel.ProtocolVersion).State = EntityState.Added;
                            await _context.SaveChangesAsync();
                        }
                        else
                        {
                            var entries  =_context.ChangeTracker.Entries();
                            _context.Entry(createProtocolsViewModel.ProtocolVersion).State = EntityState.Modified;
                            await _context.SaveChangesAsync();
                        }            
                    }
                    foreach (var url in createProtocolsViewModel.ProtocolVersion.Urls)
                    {
                        if (url.LinkID == 0)
                        {
                            _context.Entry(url).State = EntityState.Added;
                            await _context.SaveChangesAsync();

                        }
                        else
                        {
                            _context.Entry(url).State = EntityState.Modified;
                            await _context.SaveChangesAsync();
                        }
                    }

                    //save lines
                    if(IncludeSaveLines)
                    {
                        var tempLinesJson = _context.TempLinesJsons.Where(tl => tl.TempLinesJsonID == createProtocolsViewModel.UniqueGuid).FirstOrDefault();
                        var tempLines = tempLinesJson.DeserializeJson<ProtocolsLinesViewModel>();
                        tempLines.Lines = tempLines.Lines.Where(tl => !tl.Line.IsTemporaryDeleted).ToList();
                        await UpdateLineContentAsync(tempLines, Lines);
                        tempLinesJson.SerializeViewModel(tempLines);
                        _context.Update(tempLinesJson);
                        await _context.SaveChangesAsync();
                        await SaveTempLinesToDB(createProtocolsViewModel.ProtocolVersion.ProtocolVersionID, tempLines);
                        await _context.FunctionLines.Where(fl => fl.Line.ProtocolVersionID == createProtocolsViewModel.ProtocolVersion.ProtocolVersionID).Where(fl => fl.IsTemporaryDeleted || fl.Line.IsTemporaryDeleted == true).ForEachAsync(fl => { _context.Remove(fl); });
                        await _context.SaveChangesAsync();
                    }
                    await transaction.CommitAsync();
                  
                }
                catch (Exception ex)
                {
                    createProtocolsViewModel = await FillCreateProtocolsViewModel(createProtocolsViewModel, createProtocolsViewModel.ProtocolVersion.ProtocolVersionID, createProtocolsViewModel.ProtocolVersion.Protocol.ProtocolTypeID);
                    createProtocolsViewModel.ErrorMessage = AppUtility.GetExceptionMessage(ex);
                    Response.StatusCode = 500;
                    await transaction.RollbackAsync();
                    //todo: delete the newly added documents
                    if (createProtocolsViewModel.ModalType == AppUtility.ProtocolModalType.Create)
                    {
                        return PartialView("_CreateProtocolTabs", createProtocolsViewModel);
                    }
                    else
                    {
                        return PartialView("_IndexTableWithEditProtocol", createProtocolsViewModel);
                    }
                }

                createProtocolsViewModel = await FillCreateProtocolsViewModel(createProtocolsViewModel, createProtocolsViewModel.ProtocolVersion.Protocol.ProtocolTypeID, createProtocolsViewModel.ProtocolVersion.ProtocolVersionID);
                if (createProtocolsViewModel.ModalType == AppUtility.ProtocolModalType.Create)
                {
                    return PartialView("_CreateProtocolTabs", createProtocolsViewModel);
                }
                else
                {
                    return PartialView("_IndexTableWithEditProtocol", createProtocolsViewModel);
                }
            }

        }

        [Authorize(Roles = "Protocols")]
        public async Task<IActionResult> _IndexTableWithEditProtocol(int protocolID)
        {
            CreateProtocolsViewModel viewmodel = new CreateProtocolsViewModel();
            viewmodel.ModalType = AppUtility.ProtocolModalType.Summary;
            viewmodel.Tab = 3;
            await FillCreateProtocolsViewModel(viewmodel, 0, protocolID);
            return PartialView(viewmodel);
        }

        [Authorize(Roles = "Protocols")]
        public async Task<IActionResult> _IndexTable(ProtocolsIndexObject protocolsIndexObject)
        {
            ProtocolsIndexViewModel viewmodel;
            viewmodel = await GetProtocolsIndexViewModelAsync(protocolsIndexObject);
            return PartialView(viewmodel);
        }

        [Authorize(Roles = "Protocols")]
        public async Task<IActionResult> _IndexTableData(ProtocolsIndexObject protocolsIndexObject)
        {
            ProtocolsIndexViewModel viewmodel;
            viewmodel = await GetProtocolsIndexViewModelAsync(protocolsIndexObject);
            return PartialView(viewmodel);
        }

        [Authorize(Roles = "Protocols")]
        public async Task<IActionResult> KitProtocol()
        {
            TempData[AppUtility.TempDataTypes.MenuType.ToString()] = AppUtility.MenuItems.Protocols;
            TempData[AppUtility.TempDataTypes.SidebarType.ToString()] = AppUtility.SidebarEnum.KitProtocol;
            TempData[AppUtility.TempDataTypes.PageType.ToString()] = AppUtility.PageTypeEnum.ProtocolsCreate;
            CreateProtocolsViewModel viewmodel = new CreateProtocolsViewModel();
            await FillCreateProtocolsViewModel(viewmodel, 2);
            return View(viewmodel);
        }

        [Authorize(Roles = "Protocols")]
        public async Task<IActionResult> SopProtocol()
        {
            TempData[AppUtility.TempDataTypes.MenuType.ToString()] = AppUtility.MenuItems.Protocols;
            TempData[AppUtility.TempDataTypes.SidebarType.ToString()] = AppUtility.SidebarEnum.SOPProtocol;
            TempData[AppUtility.TempDataTypes.PageType.ToString()] = AppUtility.PageTypeEnum.ProtocolsCreate;
            CreateProtocolsViewModel viewmodel = new CreateProtocolsViewModel();
            await FillCreateProtocolsViewModel(viewmodel, 3);
            viewmodel.ModalType = AppUtility.ProtocolModalType.Create;
            return View(viewmodel);
        }
        [Authorize(Roles = "Protocols")]
        public async Task<IActionResult> BufferCreating()
        {
            TempData[AppUtility.TempDataTypes.MenuType.ToString()] = AppUtility.MenuItems.Protocols;
            TempData[AppUtility.TempDataTypes.SidebarType.ToString()] = AppUtility.SidebarEnum.BufferCreating;
            TempData[AppUtility.TempDataTypes.PageType.ToString()] = AppUtility.PageTypeEnum.ProtocolsCreate;
            CreateProtocolsViewModel viewmodel = new CreateProtocolsViewModel();
            await FillCreateProtocolsViewModel(viewmodel, 4);
            viewmodel.ModalType = AppUtility.ProtocolModalType.Create;
            return View(viewmodel);
        }
        [Authorize(Roles = "Protocols")]
        public async Task<IActionResult> RobioticProtocol()
        {
            TempData[AppUtility.TempDataTypes.MenuType.ToString()] = AppUtility.MenuItems.Protocols;
            TempData[AppUtility.TempDataTypes.SidebarType.ToString()] = AppUtility.SidebarEnum.RoboticProtocol;
            TempData[AppUtility.TempDataTypes.PageType.ToString()] = AppUtility.PageTypeEnum.ProtocolsCreate;
            CreateProtocolsViewModel viewmodel = new CreateProtocolsViewModel();
            await FillCreateProtocolsViewModel(viewmodel, 5);
            viewmodel.ModalType = AppUtility.ProtocolModalType.Create;
            return View(viewmodel);
        }
        [Authorize(Roles = "Protocols")]
        public async Task<IActionResult> MaintenanceProtocol()
        {
            TempData[AppUtility.TempDataTypes.MenuType.ToString()] = AppUtility.MenuItems.Protocols;
            TempData[AppUtility.TempDataTypes.SidebarType.ToString()] = AppUtility.SidebarEnum.MaintenanceProtocol;
            TempData[AppUtility.TempDataTypes.PageType.ToString()] = AppUtility.PageTypeEnum.ProtocolsCreate;
            CreateProtocolsViewModel viewmodel = new CreateProtocolsViewModel();
            await FillCreateProtocolsViewModel(viewmodel, 6);
            viewmodel.ModalType = AppUtility.ProtocolModalType.Create;
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

        [Authorize(Roles = "Protocols")]
        public async Task<IActionResult> _Library(int? CategoryType = 1)
        {
            //TempData[AppUtility.TempDataTypes.MenuType.ToString()] = AppUtility.MenuItems.Protocols;
            //TempData[AppUtility.TempDataTypes.SidebarType.ToString()] = AppUtility.SidebarEnum.Library;
            //TempData[AppUtility.TempDataTypes.PageType.ToString()] = AppUtility.PageTypeEnum.ProtocolsResources;

            var resourceLibraryViewModel = new ResourceLibraryViewModel();

            resourceLibraryViewModel.ResourceCategories = _context.ResourceCategories;
            return PartialView(resourceLibraryViewModel);
        }

        [HttpGet]
        [Authorize(Roles = "Protocols")]
        public async Task<IActionResult> _ResourcesList(int? ResourceCategoryID, bool IsPersonal = false)
        {
            //TempData[AppUtility.TempDataTypes.MenuType.ToString()] = AppUtility.MenuItems.Protocols;
            //TempData[AppUtility.TempDataTypes.SidebarType.ToString()] = AppUtility.SidebarEnum.Library;
            //TempData[AppUtility.TempDataTypes.PageType.ToString()] = AppUtility.PageTypeEnum.ProtocolsResources;

            ResourcesListIndexViewModel ResourcesListIndexViewModel = new ResourcesListIndexViewModel() { IsFavoritesPage = false };

            ResourcesListIndexViewModel.ResourcesWithFavorites = _context.Resources
                .Where(r => IsPersonal ? r.ApplicationUserCreatorID == _userManager.GetUserId(User) && r.IsPersonal == true : r.IsPersonal == false)
                .Include(r => r.FavoriteResources)
                .Include(r => r.ResourceResourceCategories).ThenInclude(rrc => rrc.ResourceCategory)
                .Where(r => r.ResourceResourceCategories.Any(rrc => rrc.ResourceCategoryID == ResourceCategoryID))
                .Select(r => new ResourceWithFavorite
                {
                    Resource = r,
                    IsFavorite = r.FavoriteResources.Any(fr => fr.ApplicationUserID == _userManager.GetUserId(User))
                }).ToList();

            //if (IsPersonal)
            //{
            //    ResourcesListIndexViewModel.ResourcesWithFavorites = ResourcesListIndexViewModel.ResourcesWithFavorites.Where(r => r.Resource.ApplicationUserCreatorID == _userManager.GetUserId(User)).ToList();
            //}

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
            if (IsPersonal)
            {
                resourcesListViewModel.SectionType = AppUtility.SidebarEnum.Personal;
            }
            else
            {
                resourcesListViewModel.SectionType = AppUtility.SidebarEnum.Library;
            }

            return PartialView(resourcesListViewModel);
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
                    addResourceViewModel.Resource.ApplicationUserCreatorID = _userManager.GetUserId(User);
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
            return RedirectToAction("_Library");
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
            var resourceLibraryViewModel = new ResourceLibraryViewModel();

            resourceLibraryViewModel.ResourceCategories = _context.ResourceCategories;
            return View(resourceLibraryViewModel);
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
                .ShareResourcesReceived.Select(srr => srr.ShareID).ToList();

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
                                             select new ResourceWithFavorite { Resource = r, IsFavorite = fr.FavoriteID == null ? false : true, SharedByApplicationUser = sr.FromApplicationUser, ShareResourceID = sr.ShareID };

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
        public ActionResult DocumentsModal(string id, Guid Guid, AppUtility.FolderNamesEnum RequestFolderNameEnum, bool IsEdittable, bool showSwitch,
    AppUtility.MenuItems SectionType = AppUtility.MenuItems.Protocols, AppUtility.ParentFolderName parentFolderName = AppUtility.ParentFolderName.Protocols, bool dontAllowMultipleFiles = false)
        {
            DocumentsModalViewModel documentsModalViewModel = new DocumentsModalViewModel()
            {
                FolderName = RequestFolderNameEnum,
                ParentFolderName = parentFolderName,
                ObjectID = id == "" ? "0" : id,
                SectionType = SectionType,
                IsEdittable = IsEdittable,
                DontAllowMultiple = dontAllowMultipleFiles,
                ShowSwitch = showSwitch,
                Guid = Guid
            };

            base.FillDocumentsViewModel(documentsModalViewModel);
            return PartialView(documentsModalViewModel);
        }


        [HttpGet]
        [Authorize(Roles = "Protocols")]
        public ActionResult _DocumentsModalData(string id, Guid Guid, AppUtility.FolderNamesEnum RequestFolderNameEnum, bool IsEdittable, bool showSwitch,
  AppUtility.MenuItems SectionType = AppUtility.MenuItems.Protocols, AppUtility.ParentFolderName parentFolderName = AppUtility.ParentFolderName.Protocols, bool dontAllowMultipleFiles = false)
        {
            DocumentsModalViewModel documentsModalViewModel = new DocumentsModalViewModel()
            {
                FolderName = RequestFolderNameEnum,
                ParentFolderName = parentFolderName,
                ObjectID = id == "" ? "0" : id,
                SectionType = SectionType,
                IsEdittable = IsEdittable,
                DontAllowMultiple = dontAllowMultipleFiles,
                ShowSwitch = showSwitch,
                Guid = Guid
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
        //[RequestFormLimits(MultipartBodyLengthLimit = 104857600)]
        public void DocumentsModal(/*[FromBody]*/ DocumentsModalViewModel documentsModalViewModel)
        {
            base.DocumentsModal(documentsModalViewModel);
        }


        [Authorize(Roles = "Protocols")]
        private void FillDocumentsInfo(CreateProtocolsViewModel createProtocolsViewModel, AppUtility.ParentFolderName parentFolderName, string uploadFolder, string id)
        {
            createProtocolsViewModel.DocumentsInfo = new List<DocumentFolder>();

            base.GetExistingFileStrings(createProtocolsViewModel.DocumentsInfo, AppUtility.FolderNamesEnum.Info, parentFolderName, uploadFolder, id);
            base.GetExistingFileStrings(createProtocolsViewModel.DocumentsInfo, AppUtility.FolderNamesEnum.Pictures, parentFolderName, uploadFolder, id);
        }

        [Authorize(Roles = "Protocols")]
        public bool RemoveShare(int ShareID, AppUtility.ModelsEnum modelsEnum)
        {
            using (var transaction = _context.Database.BeginTransaction())
            {
                try
                {
                    switch (modelsEnum)
                    {
                        case AppUtility.ModelsEnum.Resource:
                            var sharedResource = _context.ShareResources.Where(sr => sr.ShareID == ShareID).FirstOrDefault();
                            _context.Remove(sharedResource);
                            break;
                        case AppUtility.ModelsEnum.Protocols:
                            var sharedProtocols = _context.ShareProtocols.Where(sr => sr.ProtocolVersionID == ShareID && sr.ToApplicationUserID == _userManager.GetUserId(User));
                            foreach (var sr in sharedProtocols)
                            {
                                _context.Remove(sr);
                            }
                            break;

                    }
                    _context.SaveChanges();
                    transaction.Commit();
                    return true;
                }
                catch (Exception ex)
                {
                    transaction.Rollback();
                    return false;
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
                case AppUtility.ModelsEnum.Protocols:
                    shareModalViewModel.ObjectDescription = _context.Protocols.Where(r => r.ProtocolID == ID).FirstOrDefault().Name;
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
                            case AppUtility.ModelsEnum.Protocols:
                                var PrevSharedProtocol = _context.ShareProtocols
                                    .Where(sr => sr.ProtocolVersionID == shareModalViewModel.ID && sr.FromApplicationUserID == currentUserID && sr.ToApplicationUserID == userID).FirstOrDefault();
                                if (PrevSharedProtocol != null)
                                {
                                    PrevSharedProtocol.TimeStamp = DateTime.Now;
                                    _context.Update(PrevSharedProtocol);
                                }
                                else
                                {
                                    var shareProtocol = new ShareProtocol()
                                    {
                                        ProtocolVersionID = shareModalViewModel.ID,
                                        FromApplicationUserID = currentUserID,
                                        ToApplicationUserID = userID,
                                        TimeStamp = DateTime.Now
                                    };
                                    _context.Update(shareProtocol);
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
                    if (lastReportNumber == null)
                    {
                        reportNumber += 1;
                    }
                    else
                    {
                        reportNumber += (Int32.Parse(lastReportNumber) + 1);
                    }

                    report.ReportNumber = reportNumber;
                    _context.Entry(report).State = EntityState.Added;
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
            report.TemporaryReportText = report.ReportText;
            var createReportViewModel = new CreateReportViewModel()
            {
                Report = report,
                FunctionTypes = functionTypes,
                ReportDateRange = AppUtility.GetWeekStartEndDates(report.DateCreated)
            };

            return View(createReportViewModel);
        }

        [HttpPost]
        [Authorize(Roles = "Protocols")]
        public async Task<IActionResult> CreateReport(CreateReportViewModel createReportViewModel)
        {
            var saveReportViewModel = new SaveReportViewModel();
            using (var transaction = _context.Database.BeginTransaction())
            {
                try
                {
                    var report = _context.Reports.Where(r => r.ReportID == createReportViewModel.ReportID).FirstOrDefault();
                    report.TemporaryReportText = createReportViewModel.Report.TemporaryReportText;
                    _context.Update(report);
                    await _context.SaveChangesAsync();

                    saveReportViewModel.ReportID = report.ReportID;
                    saveReportViewModel.ReportTitle = createReportViewModel.Report.ReportTitle;
                    await transaction.CommitAsync();
                }
                catch (Exception ex)
                {

                }
            }
            return PartialView("SaveReportModal", saveReportViewModel);
        }

        [Authorize(Roles = "Protocols")]
        public async Task<IActionResult> AddReportFunctionModal(int FunctionTypeID, int ReportID, int FunctionReportID, string closingTags)
        {
            var functionType = _context.FunctionTypes.Where(ft => ft.FunctionTypeID == FunctionTypeID).FirstOrDefault();

            var functionReport = new FunctionReport()
            {
                FunctionType = functionType,
                FunctionTypeID = functionType.FunctionTypeID
            };
            var viewmodel = new AddReportFunctionViewModel
            {
                ReportID = ReportID,
                Function = functionReport,
                ClosingTags = closingTags
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
                    //viewmodel.Protocols = _context.Protocols.ToList();
                    break;
                case AppUtility.ProtocolFunctionTypes.AddFile:
                    DocumentsModalViewModel documentsModalViewModel = new DocumentsModalViewModel()
                    {
                        FolderName = AppUtility.FolderNamesEnum.Files,
                        ParentFolderName = AppUtility.ParentFolderName.Reports,
                        ObjectID = functionReport.ID.ToString(),
                        SectionType = AppUtility.MenuItems.Protocols,
                        IsEdittable = true,
                        DontAllowMultiple = true,
                        ShowSwitch = false,
                        Guid = Guid.NewGuid()
                    };
                    base.FillDocumentsViewModel(documentsModalViewModel);
                    viewmodel.DocumentsModalViewModel = documentsModalViewModel;
                    break;
            }
            return PartialView(viewmodel);
        }

        [HttpPost]
        [Authorize(Roles = "Protocols")]
        public async Task<IActionResult> AddReportFunctionModal(AddReportFunctionViewModel addReportsFunctionViewModel, CreateReportViewModel createReportViewModel, Guid guid)
        {
            var functionType = _context.FunctionTypes.FirstOrDefault();

            var report = _context.Reports.Where(r => r.ReportID == addReportsFunctionViewModel.ReportID).FirstOrDefault();
            var functionReport = addReportsFunctionViewModel.Function;

            functionReport.IsTemporary = true;
            functionReport.ReportID = report.ReportID;
            using (var transaction = _context.Database.BeginTransaction())
            {
                try
                {
                    if (addReportsFunctionViewModel.IsRemove)
                    {
                        functionReport.IsTemporaryDeleted = true;
                        _context.Entry(functionReport).State = EntityState.Modified;
                        report.TemporaryReportText = createReportViewModel.Report.TemporaryReportText;
                        _context.Update(report);
                    }
                    else
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
                                _context.Entry(functionReport).State = EntityState.Added;
                                await _context.SaveChangesAsync();
                                MoveDocumentsOutOfTempFolder(functionReport.ID, AppUtility.ParentFolderName.Reports, guid: guid);

                                DocumentsModalViewModel documentsModalViewModel = new DocumentsModalViewModel()
                                {
                                    ObjectID = functionReport.ID.ToString(),
                                    ParentFolderName = AppUtility.ParentFolderName.Reports,
                                    SectionType = AppUtility.MenuItems.Protocols,
                                    IsEdittable = true,
                                    Guid = guid
                                };

                                base.FillDocumentsViewModel(documentsModalViewModel);

                                string renderedView = await RenderPartialViewToString("_DocumentCard", documentsModalViewModel);
                                var replaceableText = "<span class=\"focusedText\"></span>";
                                var tags = addReportsFunctionViewModel.ClosingTags?.Split(",") ?? new string[0];
                                var closingTags = "";
                                var openingTags = "";
                                foreach (var tag in tags)
                                {
                                    closingTags += "</" + tag + ">";
                                    openingTags = "<" + tag + ">" + openingTags;
                                }
                                var addedText = renderedView + " <div contenteditable='true' class= 'editable-span form-control-plaintext text-transform-none text added-div start-div'></div>";

                                if (!createReportViewModel.Report.TemporaryReportText.Contains(replaceableText))
                                {
                                    report.TemporaryReportText = createReportViewModel.Report.TemporaryReportText + addedText;
                                }
                                else
                                {
                                    addedText = closingTags + addedText + openingTags;
                                    report.TemporaryReportText = createReportViewModel.Report.TemporaryReportText.Replace(replaceableText, addedText);
                                }
                                _context.Update(report);
                                break;
                        }
                    }
                    await _context.SaveChangesAsync();
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


        [HttpGet]
        [Authorize(Roles = "Protocols")]
        public async Task<IActionResult> DeleteReportDocumentModal(int FunctionReportID)
        {
            var functionReport = _context.FunctionReports.Where(fr => fr.ID == FunctionReportID).FirstOrDefault();
            var functionType = _context.FunctionTypes.Where(ft => ft.FunctionTypeID == functionReport.FunctionTypeID).FirstOrDefault();
            AppUtility.ParentFolderName parentFolderName = AppUtility.ParentFolderName.Reports;
            string uploadReportsFolder = Path.Combine(_hostingEnvironment.WebRootPath, parentFolderName.ToString());
            string uploadReportsFolder2 = Path.Combine(uploadReportsFolder, FunctionReportID.ToString());

            var deleteDocumentViewModel = new DeleteReportDocumentViewModel()
            {
                FunctionReport = functionReport,
                ReportID = functionReport.ReportID
            };

            deleteDocumentViewModel.DocumentsInfo = new List<DocumentFolder>();
            base.GetExistingFileStrings(deleteDocumentViewModel.DocumentsInfo, AppUtility.FolderNamesEnum.Files, parentFolderName, uploadReportsFolder2, FunctionReportID.ToString());
            return PartialView(deleteDocumentViewModel);
        }

        [HttpPost]
        [Authorize(Roles = "Protocols")]
        public async Task<IActionResult> DeleteReportDocumentModal(DeleteReportDocumentViewModel deleteDocumentViewModel, CreateReportViewModel createReportViewModel)
        {
            var report = _context.Reports.Where(r => r.ReportID == deleteDocumentViewModel.ReportID).FirstOrDefault();
            using (var transaction = _context.Database.BeginTransaction())
            {
                try
                {
                    deleteDocumentViewModel.FunctionReport.IsTemporaryDeleted = true;
                    _context.Entry(deleteDocumentViewModel.FunctionReport).State = EntityState.Modified;
                    report.TemporaryReportText = createReportViewModel.Report.TemporaryReportText;
                    _context.Update(report);

                    await _context.SaveChangesAsync();
                    base.DeleteTemporaryDocuments(AppUtility.ParentFolderName.Reports, ObjectID: deleteDocumentViewModel.FunctionReport.ID);

                    await transaction.CommitAsync();
                }
                catch (Exception ex)
                {

                }
            }
            return PartialView("_ReportText", report);
        }


        [HttpPost]
        [Authorize(Roles = "Protocols")]
        public async Task<IActionResult> SaveReportModal(SaveReportViewModel saveReportViewModel)
        {
            var report = _context.Reports.Where(r => r.ReportID == saveReportViewModel.ReportID).FirstOrDefault();
            var reportTempFunctions = _context.FunctionReports.Where(fr => fr.ReportID == report.ReportID && fr.IsTemporary).ToList();
            var deletedReportFunctions = _context.FunctionReports.Where(fr => fr.ReportID == report.ReportID && fr.IsTemporaryDeleted).ToList();
            using (var transaction = _context.Database.BeginTransaction())
            {
                try
                {
                    if (saveReportViewModel.SaveReport)
                    {
                        report.ReportText = report.TemporaryReportText;
                        report.TemporaryReportText = null;
                        report.ReportTitle = saveReportViewModel.ReportTitle;
                        _context.Update(report);

                        foreach (var functionReport in reportTempFunctions)
                        {
                            functionReport.IsTemporary = false;
                            _context.Update(functionReport);
                        }
                        foreach (var functionReport in deletedReportFunctions)
                        {
                            _context.Remove(functionReport);
                        }
                        await _context.SaveChangesAsync();
                    }
                    else
                    {
                        report.TemporaryReportText = null;
                        _context.Update(report);

                        foreach (var functionReport in reportTempFunctions)
                        {
                            _context.Remove(functionReport);

                            string uploadFolder1 = Path.Combine(_hostingEnvironment.WebRootPath, AppUtility.ParentFolderName.Reports.ToString());
                            string uploadFolder2 = Path.Combine(uploadFolder1,functionReport.ID.ToString());
                            if (Directory.Exists(uploadFolder2))
                            {
                                Directory.Delete(uploadFolder2, true);
                            }
                        }
                        foreach (var functionReport in deletedReportFunctions)
                        {
                            functionReport.IsTemporaryDeleted = false;
                            _context.Update(functionReport);
                        }
                        await _context.SaveChangesAsync();
                    }
                    await transaction.CommitAsync();
                }
                catch (Exception ex)
                {

                }
            }
            var reportsIndexObject = new ReportsIndexObject()
            {
                PageType = AppUtility.PageTypeEnum.ProtocolsReports,
                ReportCategoryID = report.ReportCategoryID,
                SidebarType = AppUtility.SidebarEnum.WeeklyReports
            };
            return RedirectToAction("_ReportsIndexTable", reportsIndexObject);
        }

        [HttpGet]
        [Authorize(Roles = "Protocols")]
        public async Task<IActionResult> FavoriteProtocol(int protocolID, string FavType, AppUtility.SidebarEnum sidebarType)
        {
            var userID = _userManager.GetUserId(User);
            if (FavType == "favorite")
            {
                var favoriteProtocol = _context.FavoriteProtocols.Where(fr => fr.ProtocolVersionID == protocolID && fr.ApplicationUserID == userID).FirstOrDefault();
                if (favoriteProtocol == null)
                {
                    using (var transaction = _context.Database.BeginTransaction())
                    {
                        try
                        {
                            favoriteProtocol = new FavoriteProtocol()
                            {
                                ProtocolVersionID = protocolID,
                                ApplicationUserID = userID
                            };
                            _context.Add(favoriteProtocol);
                            await _context.SaveChangesAsync();
                            await transaction.CommitAsync();
                        }
                        //throw new Exception(); //check this after!
                        catch (Exception e)
                        {
                            await transaction.RollbackAsync();
                            await Response.WriteAsync(AppUtility.GetExceptionMessage(e));
                        }
                    }
                }
            }
            else if (FavType == "unlike")
            {
                using (var transaction = _context.Database.BeginTransaction())
                {
                    try
                    {
                        var favoriteRequest = _context.FavoriteProtocols
                            .Where(fr => fr.ApplicationUserID == userID)
                            .Where(fr => fr.ProtocolVersionID == protocolID).FirstOrDefault();
                        _context.Remove(favoriteRequest);
                        await _context.SaveChangesAsync();
                        await transaction.CommitAsync();
                    }
                    //throw new Exception(); //check this after!
                    catch (Exception e)
                    {
                        await transaction.RollbackAsync();
                        await Response.WriteAsync(AppUtility.GetExceptionMessage(e));
                    }
                }
                if (sidebarType == AppUtility.SidebarEnum.Favorites)
                {
                    ProtocolsIndexObject requestIndexObject = new ProtocolsIndexObject()
                    {
                        PageType = AppUtility.PageTypeEnum.ProtocolsProtocols,
                        SidebarType = sidebarType
                    };
                    return RedirectToAction("_IndexTable", requestIndexObject);
                }
            }
            return new EmptyResult();
        }

        public string GetUniqueNumber()
        {
            var serialLetter = "T";
            var serialnumberList = _context.Protocols.IgnoreQueryFilters().Select(p => int.Parse(p.UniqueCode.Substring(1))).ToList();
            var lastSerialNumberInt = serialnumberList.OrderBy(s => s).LastOrDefault();
            return serialLetter + (lastSerialNumberInt + 1);
        }
    }

}
