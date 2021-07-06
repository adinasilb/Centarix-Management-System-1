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
                .ThenInclude(p => p.ProtocolCategoryType).Include(p => p.ProtocolType).Include(p=>p.ProtocolInstances);
            var user = await _userManager.GetUserAsync(User);
            switch (protocolsIndexObject.PageType)
            {
                case AppUtility.PageTypeEnum.ProtocolsProtocols:
                    switch (protocolsIndexObject.SidebarType)
                    {
                        case AppUtility.SidebarEnum.List:
                            break;
                        case AppUtility.SidebarEnum.MyProtocols:
                            fullProtocolsList.Where(fl => fl.ApplicationUserCreatorID == user.Id);
                            break;
                        case AppUtility.SidebarEnum.Favorites:
                            var usersFavoriteProtocols = _context.FavoriteProtocols.Where(fr => fr.ApplicationUserID == _userManager.GetUserId(User))
                    .Select(fr => fr.ProtocolID);
                            fullProtocolsList = fullProtocolsList.Where(frl => usersFavoriteProtocols.Contains(frl.ProtocolID));
                            break;
                        case AppUtility.SidebarEnum.SharedWithMe:
                            var shareProtocols = _context.ShareProtocols.Where(fr => fr.ToApplicationUserID == _userManager.GetUserId(User))
                    .Select(fr => fr.ProtocolID);
                            fullProtocolsList = fullProtocolsList.Where(frl => shareProtocols.Contains(frl.ProtocolID));
                            break;
                        case AppUtility.SidebarEnum.LastProtocol:
                           fullProtocolsList = fullProtocolsList.Where(p => p.ProtocolInstances.Count()>0);
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
        private async Task<IPagedList<ProtocolsIndexPartialRowViewModel>> GetProtocolsColumnsAndRows(ProtocolsIndexObject protocolsIndexObject, IPagedList<ProtocolsIndexPartialRowViewModel> onePageOfProtocols, IQueryable<Protocol> ProtocolPassedInWithInclude)
        {
            List<IconColumnViewModel> iconList = new List<IconColumnViewModel>();
            var favoriteIcon = new IconColumnViewModel(" icon-favorite_border-24px", "#5F79E2", "protocol-favorite", "Favorite");
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
    .Select(p => new ProtocolsIndexPartialRowViewModel(p, p.ProtocolType, p.ProtocolSubCategory, p.ApplicationUserCreator, protocolsIndexObject, iconList, _context.FavoriteProtocols.Where(fr => fr.ProtocolID == p.ProtocolID).Where(fr => fr.ApplicationUserID == user.Id).FirstOrDefault(), user,
        _context.ProtocolInstances.Where(pi => pi.ProtocolID == p.ProtocolID && pi.ApplicationUserID == user.Id && !pi.IsFinished).OrderByDescending(pi => pi.StartDate).FirstOrDefault())).ToPagedListAsync(protocolsIndexObject.PageNumber == 0 ? 1 : protocolsIndexObject.PageNumber, 25);                            
                            break;
                        case AppUtility.SidebarEnum.MyProtocols:
                            iconList.Add(favoriteIcon);
                            popoverMoreIcon.IconPopovers = new List<IconPopoverViewModel>() { popoverShare, popoverStart };
                            iconList.Add(popoverMoreIcon);
                            onePageOfProtocols = await ProtocolPassedInWithInclude.OrderByDescending(p => p.CreationDate)
  .Select(p => new ProtocolsIndexPartialRowViewModel(p, p.ProtocolType, p.ProtocolSubCategory, protocolsIndexObject, iconList,
     _context.FavoriteProtocols.Where(fr => fr.ProtocolID == p.ProtocolID).Where(fr => fr.ApplicationUserID == user.Id).FirstOrDefault(), user,
             _context.ProtocolInstances.Where(pi => pi.ProtocolID == p.ProtocolID && pi.ApplicationUserID == user.Id && !pi.IsFinished).OrderByDescending(pi => pi.StartDate).FirstOrDefault())).ToPagedListAsync(protocolsIndexObject.PageNumber == 0 ? 1 : protocolsIndexObject.PageNumber, 25);

                            break;
                        case AppUtility.SidebarEnum.Favorites:
                            iconList.Add(favoriteIcon);
                            popoverMoreIcon.IconPopovers = new List<IconPopoverViewModel>() { popoverShare, popoverStart };
                            iconList.Add(popoverMoreIcon);
                            onePageOfProtocols = await ProtocolPassedInWithInclude.OrderByDescending(p => p.CreationDate)
   .Select(p => new ProtocolsIndexPartialRowViewModel(p, p.ProtocolType, p.ProtocolSubCategory, p.ApplicationUserCreator, protocolsIndexObject, iconList, 
                                                 _context.ShareProtocols
                .Where(fr => fr.ProtocolID == p.ProtocolID).Where(sr => sr.ToApplicationUserID == user.Id).Include(sr => sr.FromApplicationUser).FirstOrDefault(),
                                                  _context.FavoriteProtocols.Where(fr => fr.ProtocolID == p.ProtocolID).Where(fr => fr.ApplicationUserID == user.Id).FirstOrDefault(), user,
                                                          _context.ProtocolInstances.Where(pi => pi.ProtocolID == p.ProtocolID && pi.ApplicationUserID == user.Id && !pi.IsFinished).OrderByDescending(pi => pi.StartDate).FirstOrDefault()
                                        )).ToPagedListAsync(protocolsIndexObject.PageNumber == 0 ? 1 : protocolsIndexObject.PageNumber, 25);
                            break;
                        case AppUtility.SidebarEnum.SharedWithMe:
                            popoverMoreIcon.IconPopovers = new List<IconPopoverViewModel>() { popoverShare, popoverStart };
                            iconList.Add(popoverMoreIcon);
                            onePageOfProtocols = await ProtocolPassedInWithInclude.OrderByDescending(p => p.CreationDate)
.Select(p => new ProtocolsIndexPartialRowViewModel(p, p.ProtocolType, p.ProtocolSubCategory, protocolsIndexObject, iconList, p.ApplicationUserCreator,
                                              _context.ShareProtocols
             .Where(fr => fr.ProtocolID == p.ProtocolID).Where(sr => sr.ToApplicationUserID == user.Id).Include(sr => sr.FromApplicationUser).FirstOrDefault(), user, _context.ProtocolInstances.Where(pi => pi.ProtocolID == p.ProtocolID && pi.ApplicationUserID == user.Id && !pi.IsFinished).OrderByDescending(pi => pi.StartDate).FirstOrDefault()
                                     )).ToPagedListAsync(protocolsIndexObject.PageNumber == 0 ? 1 : protocolsIndexObject.PageNumber, 25); 
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
                                        protocolList.Add(new ProtocolProtocolInstance { Protocol = protocol, ProtocolInstance = currentProtocolInstances.ElementAt(i) });
                                    }
                                }
                            }
                            onePageOfProtocols = await protocolList.OrderByDescending(p => p.ProtocolInstance.EndDate)
.Select(p => new ProtocolsIndexPartialRowViewModel(p.Protocol, p.Protocol.ProtocolType, p.Protocol.ProtocolSubCategory, p.Protocol.ApplicationUserCreator, protocolsIndexObject, iconList, user, p.ProtocolInstance
                                     )).ToPagedListAsync(protocolsIndexObject.PageNumber == 0 ? 1 : protocolsIndexObject.PageNumber, 25);

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
            ).ToPagedListAsync(reportsIndexObject.PageNumber == 0 ? 1 : reportsIndexObject.PageNumber, 25);
            return onePageOfReports;
        }

        [Authorize(Roles = "Protocols")]
        public async Task<IActionResult> CurrentProtocols()
        {
            TempData[AppUtility.TempDataTypes.MenuType.ToString()] = AppUtility.MenuItems.Protocols;
            TempData[AppUtility.TempDataTypes.SidebarType.ToString()] = AppUtility.SidebarEnum.CurrentProtocols;
            TempData[AppUtility.TempDataTypes.PageType.ToString()] = AppUtility.PageTypeEnum.ProtocolsWorkflow;
            var user = await _userManager.GetUserAsync(User);
            var viewmodel = _context.ProtocolInstances.Where(p => p.ApplicationUserID == user.Id && !p.IsFinished).Include(p=>p.Protocol).ToList();
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
            var viewmodel = await GetProtocolsIndexViewModel(
                new ProtocolsIndexObject() { SectionType = AppUtility.MenuItems.Protocols, SidebarType = AppUtility.SidebarEnum.MyProtocols, PageType = AppUtility.PageTypeEnum.ProtocolsProtocols });

            return View(viewmodel);
        }

        [Authorize(Roles = "Protocols")]
        public async Task<IActionResult> ProtocolsFavorites()
        {
            TempData[AppUtility.TempDataTypes.MenuType.ToString()] = AppUtility.MenuItems.Protocols;
            TempData[AppUtility.TempDataTypes.SidebarType.ToString()] = AppUtility.SidebarEnum.Favorites;
            TempData[AppUtility.TempDataTypes.PageType.ToString()] = AppUtility.PageTypeEnum.ProtocolsProtocols;
            var viewmodel = await GetProtocolsIndexViewModel(
                new ProtocolsIndexObject() { SectionType = AppUtility.MenuItems.Protocols, SidebarType = AppUtility.SidebarEnum.Favorites, PageType = AppUtility.PageTypeEnum.ProtocolsProtocols });

            return View(viewmodel);
        }

        [Authorize(Roles = "Protocols")]
        public async Task<IActionResult> ProtocolsSharedWithMe()
        {
            TempData[AppUtility.TempDataTypes.MenuType.ToString()] = AppUtility.MenuItems.Protocols;
            TempData[AppUtility.TempDataTypes.SidebarType.ToString()] = AppUtility.SidebarEnum.SharedWithMe;
            TempData[AppUtility.TempDataTypes.PageType.ToString()] = AppUtility.PageTypeEnum.ProtocolsProtocols;
            var viewmodel = await GetProtocolsIndexViewModel(
                 new ProtocolsIndexObject() { SectionType = AppUtility.MenuItems.Protocols, SidebarType = AppUtility.SidebarEnum.SharedWithMe, PageType = AppUtility.PageTypeEnum.ProtocolsProtocols });

            return View(viewmodel);
        }
        [Authorize(Roles = "Protocols")]
        public async Task<IActionResult> LastProtocol()
        {
            TempData[AppUtility.TempDataTypes.MenuType.ToString()] = AppUtility.MenuItems.Protocols;
            TempData[AppUtility.TempDataTypes.SidebarType.ToString()] = AppUtility.SidebarEnum.LastProtocol;
            TempData[AppUtility.TempDataTypes.PageType.ToString()] = AppUtility.PageTypeEnum.ProtocolsProtocols;
            var viewmodel = await GetProtocolsIndexViewModel(new ProtocolsIndexObject() { SectionType = AppUtility.MenuItems.Protocols, SidebarType = AppUtility.SidebarEnum.LastProtocol, PageType = AppUtility.PageTypeEnum.ProtocolsProtocols });
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
        public async Task<IActionResult> StartProtocol(int ID, bool isContinue, int tab=3)
        {
            var user = await _userManager.GetUserAsync(User);
            CreateProtocolsViewModel viewmodel = new CreateProtocolsViewModel();
            viewmodel.Tab = tab;
            Protocol protocol = null;
            if(isContinue)
            {
                viewmodel.ProtocolInstance = await _context.ProtocolInstances.Where(p => p.ProtocolInstanceID == ID).Include(p=>p.Protocol).FirstOrDefaultAsync();
                protocol = viewmodel.ProtocolInstance.Protocol;
            }
            else
            {
                protocol = _context.Protocols.Where(p => p.ProtocolID == ID).FirstOrDefault();
                viewmodel.ProtocolInstance = new ProtocolInstance { ProtocolID = ID, StartDate = DateTime.Now, ApplicationUserID = user.Id, CurrentLineID = _context.Lines.Where(l => l.ProtocolID == ID && l.ParentLineID == null && l.LineNumber == 1).FirstOrDefault().LineID };
                _context.Add(viewmodel.ProtocolInstance);
                await _context.SaveChangesAsync();
            }
            viewmodel.ModalType = AppUtility.ProtocolModalType.CheckListMode;
            await FillCreateProtocolsViewModel(viewmodel, protocol.ProtocolTypeID, protocol.ProtocolID);         
            return PartialView("_IndexTableWithEditProtocol", viewmodel);
        }

        [Authorize(Roles = "Protocols")]
        public async Task<IActionResult> MoveToNextLine(int protocolInstanceID, int nextLineID, bool isLast)
        {
            var protocolInstance = await _context.ProtocolInstances.Where(pi => pi.ProtocolInstanceID == protocolInstanceID).FirstOrDefaultAsync();
            if(nextLineID>0 || isLast)
            {
                if(isLast)
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
            List<ProtocolsLineViewModel> refreshedLines = OrderLinesForView(protocolInstance.ProtocolID, AppUtility.ProtocolModalType.CheckListMode, protocolInstance);

            return PartialView("_Lines", new ProtocolsLinesViewModel { Lines = refreshedLines });
        }

        [Authorize(Roles = "Protocols")]
        public async Task<IActionResult> AddChangeModal(int protocolInstanceID, int currentLineID)
        {
            var protocolInstance = await _context.ProtocolInstances.Where(pi => pi.ProtocolInstanceID == protocolInstanceID).FirstOrDefaultAsync();            
            return PartialView(new LineChange() { ProtocolInstanceID = protocolInstanceID, LineID = currentLineID});
        }

        [HttpPost]
        [Authorize(Roles = "Protocols")]
        public async Task<IActionResult> AddChangeModal(LineChange lineChange)
        {
            var protocolInstance = await _context.ProtocolInstances.Where(pi => pi.ProtocolInstanceID == lineChange.ProtocolInstanceID).FirstOrDefaultAsync();

            List<ProtocolsLineViewModel> refreshedLines = OrderLinesForView(protocolInstance.ProtocolID, AppUtility.ProtocolModalType.CheckListMode, protocolInstance);

            return PartialView("_Lines", new ProtocolsLinesViewModel { Lines = refreshedLines });
        }
        private async Task<CreateProtocolsViewModel> FillCreateProtocolsViewModel(CreateProtocolsViewModel createProtocolsViewModel, int typeID, int protocolID = 0)
        {
            DeleteTemporaryDocuments(AppUtility.ParentFolderName.Protocols);
            var protocol = _context.Protocols
                .Include(p => p.Urls).Include(p => p.Materials)
                .ThenInclude(m => m.Product).Include(p => p.ProtocolSubCategory).Where(p => p.ProtocolID == protocolID).FirstOrDefault() ?? new Protocol();
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
            List<FunctionType> functionTypes = new List<FunctionType>();

            createProtocolsViewModel.Protocol = protocol;
            createProtocolsViewModel.ProtocolCategories = _context.ProtocolCategories;
            createProtocolsViewModel.ProtocolSubCategories = _context.ProtocolSubCategories;
            createProtocolsViewModel.MaterialCategories = _context.MaterialCategories;
            createProtocolsViewModel.LineTypes = _context.LineTypes.ToList();
           
            foreach (var functionType in Enum.GetValues(typeof(AppUtility.ProtocolFunctionTypes)))
            {
                functionTypes.Add(_context.FunctionTypes.Where(ft => ft.DescriptionEnum == functionType.ToString()).FirstOrDefault());
            }
            createProtocolsViewModel.FunctionTypes = functionTypes;
            await CopySelectedLinesToTempLineTable(protocol.ProtocolID);
            createProtocolsViewModel.TempLines = OrderLinesForView(protocolID, createProtocolsViewModel.ModalType, createProtocolsViewModel.ProtocolInstance);
            string uploadProtocolsFolder = Path.Combine(_hostingEnvironment.WebRootPath, AppUtility.ParentFolderName.Materials.ToString());
            string uploadProtocolsFolder2 = Path.Combine(uploadProtocolsFolder, protocol.ProtocolID.ToString());
            FillDocumentsInfo(createProtocolsViewModel, uploadProtocolsFolder2);
            Dictionary<Material, List<DocumentFolder>> MaterialFolders = FillMaterialDocumentsModel(protocol.Materials, uploadProtocolsFolder);
            createProtocolsViewModel.MaterialDocuments = (Lookup<Material, List<DocumentFolder>>)MaterialFolders.ToLookup(o => o.Key, o => o.Value);
            return createProtocolsViewModel;
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
        public async Task SaveTempLines(List<TempLine> TempLines, int ProtocolID)
        {
            try
            {
                using (var transaction = _context.Database.BeginTransaction())
                {
                    try
                    {
                        await UpdateLineContentAsync(TempLines);
                        var tempLines = _context.TempLines;
                        foreach (var line in tempLines)
                        {
                            _context.Update(TurnTempLineToLine(line));
                        }
                        await _context.SaveChangesAsync();
                        await _context.FunctionLines.Where(fl => fl.IsTemporary && fl.Line.IsTemporaryDeleted == false).ForEachAsync(fl => { fl.IsTemporary = false; _context.Update(fl); });
                        await _context.FunctionLines.Where(fl => fl.IsTemporaryDeleted || fl.Line.IsTemporaryDeleted == true).ForEachAsync(fl => { _context.Remove(fl); });
                        await _context.SaveChangesAsync();
                        await DeleteTemporaryDeletedLinesAsync();

                        await transaction.CommitAsync();
                    }
                    catch (Exception ex)
                    {
                        Response.StatusCode = 500;
                        await transaction.RollbackAsync();
                        //  await Response.WriteAsync(AppUtility.GetExceptionMessage(ex));
                    }
                }
                await CopySelectedLinesToTempLineTable(ProtocolID);
            }
            catch (Exception ex)
            {
                Response.StatusCode = 500;
                await Response.WriteAsync(AppUtility.GetExceptionMessage(ex));
            }
        }



        private async Task ClearTempLinesTableAsync()
        {
            var lineTypes = GetOrderLineTypeFromChildToParent();

            await _context.FunctionLines.Where(fl => fl.IsTemporary || fl.Line.IsTemporary).ForEachAsync(fl => { _context.Remove(fl); });
            await _context.FunctionLines.Where(fl => fl.IsTemporaryDeleted).ForEachAsync(fl => { fl.IsTemporaryDeleted = false; _context.Update(fl); });

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
                var tempDeletedLines = _context.Lines.Where(l => l.LineTypeID == lineType.LineTypeID).Where(tl => tl.IsTemporaryDeleted);
                foreach (var tempDeleted in tempDeletedLines)
                {
                    tempDeleted.IsTemporaryDeleted = false;
                    _context.Update(tempDeleted);
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
        public async Task<IActionResult> _Lines(List<TempLine> TempLines, int lineTypeID, int currentLineID, int protocolID, AppUtility.ProtocolModalType modalType)
        {
            using (var transaction = _context.Database.BeginTransaction())
            {
                try
                {
                    var currentLine = _context.TempLines.Include(tl => tl.ParentLine).Include(tl => tl.PermanentLine).Where(tl => tl.PermanentLineID == currentLineID).FirstOrDefault();
                    var orderedLineTypes = GetOrderLineTypeFromParentToChild();
                    if (TempLines != null)
                    {
                        //save all temp line data 
                        await UpdateLineContentAsync(TempLines);
                    }
                    if (lineTypeID == -1)
                    {
                        await DeleteTempLineWithChildrenAsync(currentLine);
                    }
                    else
                    {
                        var newLineType = _context.LineTypes.Where(lt => lt.LineTypeID == lineTypeID).FirstOrDefault();

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

                    }
                    await transaction.CommitAsync();
                }
                catch (Exception ex)
                {
                    Response.StatusCode = 500;
                    await transaction.RollbackAsync();
                    //  await Response.WriteAsync(AppUtility.GetExceptionMessage(ex));
                    return PartialView("_Lines", new ProtocolsLinesViewModel { Lines = OrderLinesForView(protocolID, modalType), ErrorMessage = AppUtility.GetExceptionMessage(ex) });
                }
            }

            List<ProtocolsLineViewModel> refreshedLines = OrderLinesForView(protocolID, modalType );

            return PartialView("_Lines", new ProtocolsLinesViewModel { Lines = refreshedLines });
        }


        public bool CheckIfSerialNumberExists(string serialNumber)
        {
            return _context.Products.Where(p => p.SerialNumber.Equals(serialNumber)).ToList().Any();
        }
        public bool CheckIfProtocolUniqueNumberExists(string uniqueNumber)
        {
            return _context.Protocols.Where(p => p.UniqueCode.Equals(uniqueNumber)).ToList().Any();
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

        private List<ProtocolsLineViewModel> OrderLinesForView(int protocolID, AppUtility.ProtocolModalType modalType, ProtocolInstance protocolInstance =null)
        {
            var currentLineID = protocolInstance?.CurrentLineID;
            var functionLine = _context.FunctionLines.Where(fl => fl.Line.ProtocolID == protocolID && fl.IsTemporaryDeleted == false).Include(fl => fl.FunctionType).ToList();
            List<ProtocolsLineViewModel> refreshedLines = new List<ProtocolsLineViewModel>();
            Stack<TempLine> parentNodes = new Stack<TempLine>();
            var lineTypes = _context.LineTypes.ToList();
            _context.TempLines.Where(tl => tl.ParentLineID == null).OrderByDescending(tl => tl.LineNumber).ToList().ForEach(tl => { parentNodes.Push(tl); });
            int count = 0;
            while (!parentNodes.IsEmpty())
            {
                var node = parentNodes.Pop();
                //if protocolinstance is finished we will set them all and not according to currentline
                if(currentLineID == node.PermanentLineID && !protocolInstance.IsFinished)
                {
                    refreshedLines.ForEach(l => l.IsDone = true);
                }
                _context.TempLines.Where(c => c.ParentLineID == (node.PermanentLineID)).OrderByDescending(tl => tl.LineNumber).ToList().ForEach(c => { parentNodes.Push(c); });

                refreshedLines.Add(new ProtocolsLineViewModel()
                {
                    LineTypes = lineTypes,
                    TempLine = node,
                    Index = count++,
                    LineNumberString = refreshedLines.Where(rl => rl.TempLine.PermanentLineID == node.ParentLineID)?.FirstOrDefault()?.LineNumberString + node.LineNumber + ".",
                    Functions = functionLine.Where(fl => fl.LineID == node.PermanentLineID),
                    ModalType = modalType,
                    IsLast = parentNodes.IsEmpty(),
                    IsDone = protocolInstance?.IsFinished??false
                });
            }
            if (refreshedLines.Count == 0)
            {
                refreshedLines.Add(new ProtocolsLineViewModel() { LineTypes = lineTypes, Index = 0, LineNumberString = 1 + "", IsLast = true });
            }
            return refreshedLines;
        }
        private async Task DeleteTemporaryDeletedLinesAsync()
        {
            var linesToDelete = await _context.Lines.Where(l => l.IsTemporaryDeleted).ToListAsync();
            var lineTypes = GetOrderLineTypeFromChildToParent();
            foreach (var lineType in lineTypes)
            {
                var linesByType = linesToDelete.Where(n => n.LineTypeID == lineType.LineTypeID);
                foreach (var line in linesByType)
                {
                    _context.Remove(line);
                }
                await _context.SaveChangesAsync();
            }
        }
        private async Task DeleteTempLineWithChildrenAsync(TempLine line)
        {
            var siblingsAfter = _context.TempLines.Where(tl => tl.ParentLineID == line.ParentLineID && tl.LineNumber > line.LineNumber).Include(tl => tl.PermanentLine).ToList();
            Stack<TempLine> nodes = new Stack<TempLine>();
            List<TempLine> nodeInOrderOfChildrenToParent = new List<TempLine>();
            nodes.Push(line);
            while (!nodes.IsEmpty())
            {
                var curr = nodes.Pop();
                nodeInOrderOfChildrenToParent.Add(curr);
                var children = await _context.TempLines.Where(tl => tl.ParentLineID == curr.PermanentLineID).Include(tl => tl.PermanentLine).ToListAsync();
                children.ForEach(tl => { nodes.Push(tl); });
            }
            var lineTypes = GetOrderLineTypeFromChildToParent();
            foreach (var lineType in lineTypes)
            {
                var nodesByType = nodeInOrderOfChildrenToParent.Where(n => n.LineTypeID == lineType.LineTypeID);
                foreach (var node in nodesByType)
                {
                    var permanentLine = node.PermanentLine;
                    _context.Remove(node);
                    if (node.PermanentLine != null)
                    {
                        if (node.PermanentLine.IsTemporary)
                        {
                            _context.Remove(permanentLine);
                        }
                        else
                        {
                            permanentLine.IsTemporaryDeleted = true;
                            _context.Update(permanentLine);
                        }

                    }

                }
                await _context.SaveChangesAsync();
            }

            //update all the siblings after number--
            siblingsAfter.ForEach(sa => { sa.LineNumber--; _context.Update(sa); });
            await _context.SaveChangesAsync();
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
        public async Task<IActionResult> AddFunctionModal(int FunctionTypeID, int LineID, int functionLineID, AppUtility.ProtocolModalType modalType)
        {
            var functionType = _context.FunctionTypes.Where(ft => ft.FunctionTypeID == FunctionTypeID).FirstOrDefault();
            var tempLine = _context.TempLines.Where(tl => tl.PermanentLineID == LineID).FirstOrDefault();
            var line = TurnTempLineToLine(tempLine);
            FunctionLine functionLine = _context.FunctionLines.Where(fl => fl.ID == functionLineID).FirstOrDefault();
            if (functionLine == null)
            {
                functionLine = new FunctionLine
                {
                    FunctionType = functionType,
                    FunctionTypeID = FunctionTypeID,
                    Line = line,
                    LineID = LineID,                   
                };
            }

            var viewmodel = new AddFunctionViewModel
            {
                FunctionLine = functionLine,
                ModalType = modalType
            };
            string uploadProtocolsFolder = Path.Combine(_hostingEnvironment.WebRootPath, AppUtility.ParentFolderName.FunctionLine.ToString());
            string uploadProtocolsFolder2 = Path.Combine(uploadProtocolsFolder, functionLine.ID.ToString());
            switch (Enum.Parse<AppUtility.ProtocolFunctionTypes>(functionType.DescriptionEnum))
            {
                case AppUtility.ProtocolFunctionTypes.AddLinkToProduct:
                    GetLinkToProductDDls(viewmodel);
                    break;
                case AppUtility.ProtocolFunctionTypes.AddLinkToProtocol:
                    GetLineToProtocolDDLs(viewmodel);
                    break;
                case AppUtility.ProtocolFunctionTypes.AddFile:
                    viewmodel.DocumentsInfo = new List<DocumentFolder>();
                    base.GetExistingFileStrings(viewmodel.DocumentsInfo, AppUtility.FolderNamesEnum.Files, uploadProtocolsFolder2);
                    break;
                case AppUtility.ProtocolFunctionTypes.AddImage:
                    viewmodel.DocumentsInfo = new List<DocumentFolder>();
                    base.GetExistingFileStrings(viewmodel.DocumentsInfo, AppUtility.FolderNamesEnum.Pictures, uploadProtocolsFolder2);
                    break;
            }
            return PartialView(viewmodel);
        }

        private void GetLineToProtocolDDLs(AddFunctionViewModel viewmodel)
        {
            viewmodel.ProtocolCategories = _context.ProtocolCategories.ToList();
            viewmodel.ProtocolSubCategories = _context.ProtocolSubCategories.ToList();
            viewmodel.Creators = _context.Users.Select(u =>
                new SelectListItem() { Value = u.Id, Text = u.FirstName + u.LastName }).ToList();
            viewmodel.Protocols = _context.Protocols.ToList();
        }

        private void GetLinkToProductDDls(AddFunctionViewModel viewmodel)
        {
            viewmodel.ParentCategories = _context.ParentCategories.ToList();
            viewmodel.ProductSubcategories = _context.ProductSubcategories.ToList();
            viewmodel.Products = _context.Products.ToList();
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
                return redirectToMaterialTab(materialDB.ProtocolID);
            }
        }

        public async Task<IActionResult> _AddFunctionModal(int objectID, string uniqueNumber, int functionTypeID)
        {
            var functionType = _context.FunctionTypes.Where(ft => ft.FunctionTypeID == functionTypeID).FirstOrDefault();

            var viewmodel = new AddFunctionViewModel
            {
                FunctionLine = new FunctionLine()
            };
            viewmodel.FunctionLine.FunctionType = functionType;
            switch (Enum.Parse<AppUtility.ProtocolFunctionTypes>(functionType.DescriptionEnum))
            {
                case AppUtility.ProtocolFunctionTypes.AddLinkToProduct:
                   var product = _context.Products.Where(p => p.ProductID == objectID || p.SerialNumber == uniqueNumber)
                        .Include(p=>p.ProductSubcategory).FirstOrDefault();
                    viewmodel.FunctionLine.Product = product;
                    viewmodel.FunctionLine.ProductID = product.ProductID;
                    viewmodel.ParentCategories = _context.ParentCategories.ToList();
                    viewmodel.ProductSubcategories = _context.ProductSubcategories.Where(ps => ps.ParentCategoryID == product.ProductSubcategory.ParentCategoryID).ToList();
                    viewmodel.Products = _context.Products.Where(p => p.ProductSubcategoryID == product.ProductSubcategoryID && product.VendorID == p.VendorID).ToList();
                    viewmodel.Vendors = _context.Vendors.ToList();

                    break;
                case AppUtility.ProtocolFunctionTypes.AddLinkToProtocol:
                    var protocol = _context.Protocols.Where(p => p.ProtocolID == objectID || p.UniqueCode == uniqueNumber).Include(ps=>ps.ProtocolSubCategory).FirstOrDefault();
                    viewmodel.FunctionLine.Protocol = protocol;
                    viewmodel.FunctionLine.ProtocolID = protocol.ProtocolID;
                    viewmodel.ProtocolCategories = _context.ProtocolCategories.ToList();
                    viewmodel.ProtocolSubCategories = _context.ProtocolSubCategories.Where(ps => ps.ProtocolCategoryTypeID == protocol.ProtocolSubCategory.ProtocolCategoryTypeID).ToList();
                    viewmodel.Creators = _context.Users.Select(u =>
                        new SelectListItem() { Value = u.Id, Text = u.FirstName + u.LastName }).ToList();
                    viewmodel.Protocols = _context.Protocols.Where(p => p.ProtocolSubCategoryID == protocol.ProtocolSubCategoryID && p.ApplicationUserCreatorID == protocol.ApplicationUserCreatorID).ToList();
                    break;
            }
            return PartialView(viewmodel);
        }

        [HttpPost]
        [Authorize(Roles = "Protocols")]
        public async Task<IActionResult> AddFunctionModal(AddFunctionViewModel addFunctionViewModel, List<TempLine> TempLines)
        {
            using (var transaction = _context.Database.BeginTransaction())
            {
                await UpdateLineContentAsync(TempLines);
                var line = _context.TempLines.Where(l => l.PermanentLineID == addFunctionViewModel.FunctionLine.LineID).FirstOrDefault();
                var protocolID = line.ProtocolID;
                try
                {
                    if (addFunctionViewModel.IsRemove)
                    {
                        addFunctionViewModel.FunctionLine.IsTemporaryDeleted = true;
                        _context.Entry(addFunctionViewModel.FunctionLine).State = EntityState.Modified;
                    }
                    else
                    {
                        await SaveTempFunctionLineAsync(addFunctionViewModel);
                        var functionType = _context.FunctionTypes.Where(ft => ft.FunctionTypeID == addFunctionViewModel.FunctionLine.FunctionTypeID).FirstOrDefault();

                        switch (Enum.Parse<AppUtility.ProtocolFunctionTypes>(functionType.DescriptionEnum))
                        {
                            case AppUtility.ProtocolFunctionTypes.AddLinkToProduct:
                                var product = _context.Products.Where(p => p.ProductID == addFunctionViewModel.FunctionLine.ProductID).FirstOrDefault();
                                line.Content += " <a href='#' class='open-line-product function-line-node' functionline='" + addFunctionViewModel.FunctionLine.ID + "' value='" + product.ProductID + "'>" + product.ProductName + "</a> " + " <div role='textbox' contenteditable  class='editable-span line input line-input text-transform-none'> </div>";
                                break;
                            case AppUtility.ProtocolFunctionTypes.AddLinkToProtocol:
                                var protocol = _context.Protocols.Include(p => p.Materials).Where(p => p.ProtocolID == addFunctionViewModel.FunctionLine.ProtocolID).FirstOrDefault();
                                line.Content += " <a href='#' functionline='" + addFunctionViewModel.FunctionLine.ID + "' class='open-line-protocol function-line-node' value='" + protocol.ProtocolID + "'>" + protocol.Name + " </a> " + " <div role='textbox' contenteditable  class='editable-span line input line-input text-transform-none'> </div>"; ;
                                break;
                            case AppUtility.ProtocolFunctionTypes.AddFile:
                            case AppUtility.ProtocolFunctionTypes.AddImage:
                                MoveDocumentsOutOfTempFolder(addFunctionViewModel.FunctionLine.ID, AppUtility.ParentFolderName.FunctionLine);
                                break;
                            case AppUtility.ProtocolFunctionTypes.AddStop:
                            case AppUtility.ProtocolFunctionTypes.AddTimer:
                            case AppUtility.ProtocolFunctionTypes.AddTip:
                            case AppUtility.ProtocolFunctionTypes.AddWarning:
                            case AppUtility.ProtocolFunctionTypes.AddComment:
                                break;
                        }
                        _context.Update(line);

                    }
                    await _context.SaveChangesAsync();

                    await transaction.CommitAsync();
                }
                catch (Exception ex)
                {
                    Response.StatusCode = 500;
                    await transaction.RollbackAsync();
                    //  await Response.WriteAsync(AppUtility.GetExceptionMessage(ex));
                }
                return PartialView("_Lines", new ProtocolsLinesViewModel { Lines = OrderLinesForView(line.ProtocolID, addFunctionViewModel.ModalType) });

            }
        }

        private async Task SaveTempFunctionLineAsync(AddFunctionViewModel addFunctionViewModel)
        {
            if (addFunctionViewModel.FunctionLine.ID == 0)
            {
                addFunctionViewModel.FunctionLine.IsTemporary = true;
            }
            _context.Entry(addFunctionViewModel.FunctionLine).State = EntityState.Added;
            await _context.SaveChangesAsync();
        }

        [HttpGet]
        [Authorize(Roles = "Protocols")]
        public async Task<JsonResult> FilterLinkToProtocol(int parentCategoryID, int subCategoryID, string creatorID)
        {
            IQueryable<Protocol> protocolsList = _context.Protocols;
            if (subCategoryID != 0)
            {
                protocolsList = protocolsList.Where(p => p.ProtocolSubCategoryID == subCategoryID);
            }
            else if (parentCategoryID != 0)
            {
                protocolsList = protocolsList.Where(p => p.ProtocolSubCategory.ProtocolCategoryTypeID == parentCategoryID);
            }
            if (creatorID != null)
            {
                protocolsList = protocolsList.Where(p => p.ApplicationUserCreatorID == creatorID);
            }
            var protocolListJson = await protocolsList.Select(p => new { protocolID = p.ProtocolID, name = p.Name }).ToListAsync();
            var subCategoryList = await _context.ProtocolSubCategories.Where(ps => ps.ProtocolCategoryTypeID == parentCategoryID).Select(ps => new { subCategoryID = ps.ProtocolCategoryTypeID, subCategoryDescription = ps.ProtocolSubCategoryTypeDescription }).ToListAsync();
            return Json(new { ProtocolSubCategories = subCategoryList, Protocols = protocolListJson });
        }

        [HttpGet]
        [Authorize(Roles = "Protocols")]
        public async Task<JsonResult> FilterLinkToProduct(int parentCategoryID, int subCategoryID, int vendorID)
        {
            IQueryable<Product> products = _context.Products;
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

        [Authorize(Roles = "Protocols")]
        public async Task<IActionResult> ProtocolsProductDetails(int? productID)
        {
            var requestID = _context.Requests.Where(r => r.ProductID == productID).OrderByDescending(r => r.ParentRequest.OrderDate).Select(r => r.RequestID).FirstOrDefault();
            var requestItemViewModel = await editModalViewFunction(requestID, isEditable: false);
            requestItemViewModel.SectionType = AppUtility.MenuItems.Protocols;
            return PartialView(requestItemViewModel);
        }

        [Authorize(Roles = "Protocols")]
        public async Task<IActionResult> ProtocolsDetailsFloatModal(int? protocolID)
        {
            var protocol = _context.Protocols.Where(p => p.ProtocolID == protocolID).FirstOrDefault();
            var createProtocolsViewModel = new CreateProtocolsViewModel();
            createProtocolsViewModel.ModalType = AppUtility.ProtocolModalType.SummaryFloat;
            await FillCreateProtocolsViewModel(createProtocolsViewModel, protocol.ProtocolTypeID, protocol.ProtocolID);
            return PartialView(createProtocolsViewModel);
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
                        await _context.SaveChangesAsync();
                        var tempLine = new TempLine() { ProtocolID = createProtocolsViewModel.Protocol.ProtocolID, LineNumber = 1, LineTypeID = 1 };
                        _context.Add(tempLine);
                        await _context.SaveChangesAsync();
                        _context.Add(TurnTempLineToLine(tempLine));
                        await _context.SaveChangesAsync();
                    }
                    else
                    {
                        _context.Entry(createProtocolsViewModel.Protocol).State = EntityState.Modified;
                        await _context.SaveChangesAsync();
                    }

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
                    createProtocolsViewModel = await FillCreateProtocolsViewModel(createProtocolsViewModel, createProtocolsViewModel.Protocol.ProtocolTypeID, createProtocolsViewModel.Protocol.ProtocolID);
                    if(createProtocolsViewModel.ModalType == AppUtility.ProtocolModalType.Create)
                    {
                        return PartialView("_CreateProtocolTabs", createProtocolsViewModel);
                    }
                    else
                    {
                        return PartialView("_IndexTableWithEditProtocol", createProtocolsViewModel);
                    }
                }
                catch (Exception ex)
                {
                    createProtocolsViewModel = await FillCreateProtocolsViewModel(createProtocolsViewModel, createProtocolsViewModel.Protocol.ProtocolID, createProtocolsViewModel.Protocol.ProtocolTypeID);
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
        public async Task<IActionResult> _IndexTable(bool IsFavorite = false)
        {
            ProtocolsIndexViewModel viewmodel; 
            if (IsFavorite)
            {
                viewmodel = await GetProtocolsIndexViewModel(
                new ProtocolsIndexObject() { SectionType = AppUtility.MenuItems.Protocols, SidebarType = AppUtility.SidebarEnum.Favorites, PageType = AppUtility.PageTypeEnum.ProtocolsProtocols });

            }
            else
            {
                viewmodel = await GetProtocolsIndexViewModel(new ProtocolsIndexObject() { });
            }
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
        public ActionResult DocumentsModal(string id, AppUtility.FolderNamesEnum RequestFolderNameEnum, bool IsEdittable, bool showSwitch,
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
                ShowSwitch =showSwitch
            };

            base.FillDocumentsViewModel(documentsModalViewModel);
            return PartialView(documentsModalViewModel);
        }


        [HttpGet]
        [Authorize(Roles = "Protocols")]
        public ActionResult _DocumentsModalData(string id, AppUtility.FolderNamesEnum RequestFolderNameEnum, bool IsEdittable, bool showSwitch,
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
                ShowSwitch = showSwitch
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
                            var sharedResource = _context.ShareResources.Where(sr => sr.ShareID == ShareID).FirstOrDefault();
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
                case AppUtility.ModelsEnum.Protocol:
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
                            case AppUtility.ModelsEnum.Protocol:
                                var PrevSharedProtocol = _context.ShareProtocols
                                    .Where(sr => sr.ProtocolID == shareModalViewModel.ID && sr.FromApplicationUserID == currentUserID && sr.ToApplicationUserID == userID).FirstOrDefault();
                                if (PrevSharedProtocol != null)
                                {
                                    PrevSharedProtocol.TimeStamp = DateTime.Now;
                                    _context.Update(PrevSharedProtocol);
                                }
                                else
                                {
                                    var shareProtocol = new ShareProtocol()
                                    {
                                        ProtocolID = shareModalViewModel.ID,
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
                FunctionReport = functionReport,
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
                    viewmodel.Protocols = _context.Protocols.ToList();
                    break;
                case AppUtility.ProtocolFunctionTypes.AddFile:
                    DeleteTemporaryDocuments(AppUtility.ParentFolderName.Reports);
                    DocumentsModalViewModel documentsModalViewModel = new DocumentsModalViewModel()
                    {
                        FolderName = AppUtility.FolderNamesEnum.Files,
                        ParentFolderName = AppUtility.ParentFolderName.Reports,
                        ObjectID = functionReport.ID.ToString(),
                        SectionType = AppUtility.MenuItems.Protocols,
                        IsEdittable = true,
                        DontAllowMultiple = true,
                        ShowSwitch = false
                    };
                    base.FillDocumentsViewModel(documentsModalViewModel);
                    viewmodel.DocumentsModalViewModel = documentsModalViewModel;
                    break;
            }
            return PartialView(viewmodel);
        }

        [HttpPost]
        [Authorize(Roles = "Protocols")]
        public async Task<IActionResult> AddReportFunctionModal(AddReportFunctionViewModel addReportsFunctionViewModel, CreateReportViewModel createReportViewModel)
        {
            var functionType = _context.FunctionTypes.FirstOrDefault();

            var report = _context.Reports.Where(r => r.ReportID == addReportsFunctionViewModel.ReportID).FirstOrDefault();
            var functionReport = addReportsFunctionViewModel.FunctionReport;

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
                                MoveDocumentsOutOfTempFolder(functionReport.ID, AppUtility.ParentFolderName.Reports);

                                DocumentsModalViewModel documentsModalViewModel = new DocumentsModalViewModel()
                                {
                                    ObjectID = functionReport.ID.ToString(),
                                    ParentFolderName = AppUtility.ParentFolderName.Reports,
                                    SectionType = AppUtility.MenuItems.Protocols,
                                    IsEdittable = true
                                };

                                base.FillDocumentsViewModel(documentsModalViewModel);

                                string renderedView = await RenderPartialViewToString("_DocumentCard", documentsModalViewModel);
                                var replaceableText = "<span class=\"focusedText\"></span>";
                                var tags = addReportsFunctionViewModel.ClosingTags?.Split(",")?? new string[0];
                                var closingTags = "";
                                var openingTags = "";
                                foreach(var tag in tags)
                                {
                                    closingTags += "</" + tag + ">";
                                    openingTags = "<" + tag + ">" + openingTags;
                                }
                                var addedText = closingTags + renderedView +" <div contenteditable='true' class= 'editable-span form-control-plaintext text-transform-none text added-div start-div'></div>" + openingTags;
                                if(createReportViewModel.Report.TemporaryReportText == null)
                                {
                                    createReportViewModel.Report.TemporaryReportText = "";
                                }
                                report.TemporaryReportText = createReportViewModel.Report.TemporaryReportText.Replace(replaceableText, addedText);
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
            string uploadReportsFolder = Path.Combine(_hostingEnvironment.WebRootPath, AppUtility.ParentFolderName.Reports.ToString());
            string uploadReportsFolder2 = Path.Combine(uploadReportsFolder, functionReport.ID.ToString());

            var deleteDocumentViewModel = new DeleteReportDocumentViewModel()
            {
                FunctionReport = functionReport,
                ReportID = functionReport.ReportID
            };

            deleteDocumentViewModel.DocumentsInfo = new List<DocumentFolder>();
            base.GetExistingFileStrings(deleteDocumentViewModel.DocumentsInfo, AppUtility.FolderNamesEnum.Files, uploadReportsFolder2);
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
                            string uploadFolder2 = Path.Combine(uploadFolder1, report.ReportID.ToString());
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
                var favoriteProtocol = _context.FavoriteProtocols.Where(fr => fr.ProtocolID == protocolID && fr.ApplicationUserID == userID).FirstOrDefault();
                if (favoriteProtocol == null)
                {
                    using (var transaction = _context.Database.BeginTransaction())
                    {
                        try
                        {
                            favoriteProtocol = new FavoriteProtocol()
                            {
                                ProtocolID = protocolID,
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
                            .Where(fr => fr.ProtocolID == protocolID).FirstOrDefault();
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
                    return RedirectToAction("_IndexTable", new { IsFavorite = true });
                }
            }
            return new EmptyResult();
        }

    }

}
