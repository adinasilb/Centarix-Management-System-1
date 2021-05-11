using Abp.Extensions;
using Castle.DynamicProxy.Generators.Emitters.SimpleAST;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
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
    public class ProtocolsController : SharedController
    {
        private readonly ApplicationDbContext _context;
        private readonly UserManager<ApplicationUser> _userManager;
        private readonly IHostingEnvironment _hostingEnvironment;
        public ProtocolsController(ApplicationDbContext context, UserManager<ApplicationUser> userManager, IHostingEnvironment hostingEnvironment) : base(context, hostingEnvironment)
        {
            _context = context;
            _userManager = userManager;
            _hostingEnvironment = hostingEnvironment;
        }

        [Authorize(Roles = "Protocols")]
        public async Task<IActionResult> Index()
        {
            TempData[AppUtility.TempDataTypes.MenuType.ToString()] = AppUtility.MenuItems.Protocols;
            TempData[AppUtility.TempDataTypes.SidebarType.ToString()] = AppUtility.SidebarEnum.List;
            TempData[AppUtility.TempDataTypes.PageType.ToString()] = AppUtility.PageTypeEnum.ProtocolsProtocols;
            var viewmodel = await GetIndexViewModel(new ProtocolsIndexObject() { });

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
        private async Task<ProtocolsIndexViewModel> GetIndexViewModel(ProtocolsIndexObject protocolsIndexObject, SelectedProtocolsFilters selectedFilters = null)
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

            onePageOfProducts = await GetColumnsAndRows(protocolsIndexObject, onePageOfProducts, ProtocolsPassedInWithInclude);

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
        private async Task<IPagedList<RequestIndexPartialRowViewModel>> GetColumnsAndRows(ProtocolsIndexObject protocolsIndexObject, IPagedList<RequestIndexPartialRowViewModel> onePageOfProtocols, IQueryable<Protocol> ProtocolPassedInWithInclude)
        {
            List<IconColumnViewModel> iconList = new List<IconColumnViewModel>();
            var defaultImage = "/images/css/CategoryImages/placeholder.png";
            switch (protocolsIndexObject.PageType)
            {
                case AppUtility.PageTypeEnum.ProtocolsProtocols:
                    switch (protocolsIndexObject.SidebarType)
                    {
                        case AppUtility.SidebarEnum.List:
                            onePageOfProtocols = await GetListRows(protocolsIndexObject, onePageOfProtocols, ProtocolPassedInWithInclude, iconList, defaultImage);
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

        [Authorize(Roles = "Protocols")]
        private static async Task<IPagedList<RequestIndexPartialRowViewModel>> GetListRows(ProtocolsIndexObject requestIndexObject, IPagedList<RequestIndexPartialRowViewModel> onePageOfProtocols, IQueryable<Protocol> ProtocolPassedInWithInclude, List<IconColumnViewModel> iconList, string defaultImage)
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
                .Include(p => p.Urls).Include(p => p.Lines)
                .Include(p => p.Materials).ThenInclude(m => m.Product).Where(p => p.ProtocolID == protocolID).FirstOrDefault() ?? new Protocol();
            protocol.Urls??= new List<Link>();
            protocol.Materials ??= new List<Material>();
            protocol.Lines ??= new List<Line>();
            if (protocol.Urls.Count()< 2)
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
                LineTypes = _context.LineTypes.ToList()
            };
            string uploadProtocolsFolder = Path.Combine(_hostingEnvironment.WebRootPath, AppUtility.ParentFolderName.Materials.ToString());
            string uploadProtocolsFolder2 = Path.Combine(uploadProtocolsFolder, protocol.ProtocolID.ToString());
            FillDocumentsInfo(viewmodel, uploadProtocolsFolder2);
            Dictionary<Material, List<DocumentFolder>> MaterialFolders = FillMaterialDocumentsModel(protocol.Materials, uploadProtocolsFolder);
            viewmodel.MaterialDocuments = (Lookup<Material, List<DocumentFolder>>)MaterialFolders.ToLookup(o => o.Key, o => o.Value);
            return viewmodel;
        }

        private Dictionary<Material, List<DocumentFolder>> FillMaterialDocumentsModel(IEnumerable<Material> Materials, string uploadProtocolsFolder)
        {
            Dictionary<Material, List<DocumentFolder>> MaterialFolders = new Dictionary<Material, List<DocumentFolder>>();
            foreach (var material in Materials)
            {
                List<DocumentFolder> folders = new List<DocumentFolder>();
                string uploadMaterialFolder2 = Path.Combine(uploadProtocolsFolder, material.MaterialID.ToString());
                GetExistingFileStrings(folders, AppUtility.FolderNamesEnum.Pictures, uploadMaterialFolder2);
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

        [Authorize(Roles = "Protocols")]
        public async Task<IActionResult> _Line(int index, int lineTypeID, int currentLineTypeID,string lineNumberString, string parentLineNumberString, int parentLineID, int currentLineNumber)
        {
            var newLineNumberString = "";
            var newLineNumber = 0;
            switch(lineTypeID)
            {
                case 1:
                    switch (currentLineTypeID)
                    {
                        case 1:
                            //if current line type and new line type are same type
                            newLineNumber = currentLineNumber + 1;
                            newLineNumberString = parentLineNumberString +"."+newLineNumber;
                            break;
                        case 2:
                            //if current line is child of new line 
                     
                            break;
                        case 3:
                            //if current line is child of new line but not a direct child

                            break;
                    }
                    break;
                case 2:
                    switch (currentLineTypeID)
                    {
                        case 1:
                            //current line is parent of new line
                            newLineNumberString = lineNumberString + "."+1;
                            newLineNumber = 1;
                            break;
                        case 2:
                            //if current line type and new line type are same type
                            newLineNumber = currentLineNumber + 1;
                            newLineNumberString = parentLineNumberString+"." +newLineNumber;
                            break;
                        case 3:
                            //current line is child of new line
                          
                            break;
                    }
                    break;
                case 3:
                    switch (currentLineTypeID)
                    {
                        case 1:   //current line is parent of new line                  
                        case 2:
                            newLineNumberString = lineNumberString + "." + 1;
                            newLineNumber = 1;
                            break;
                        case 3:
                            //if current line type and new line type are same type
                            newLineNumber = currentLineNumber + 1;
                            newLineNumberString = parentLineNumberString+"." +newLineNumber;
                            break;
                    }
                    break;
            }
            var lineTypes = _context.LineTypes.ToList();
            return PartialView(new ProtocolsLineViewModel { Index = (index+1), Line = new Line { LineTypeID = lineTypeID, LineNumber=newLineNumber}, LineNumberString = newLineNumberString , LineTypes = lineTypes});
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
                    MoveDocumentsOutOfTempFolder(addMaterialViewModel.Material.MaterialID, AppUtility.ParentFolderName.Materials);
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
            return redirectToMaterialTab(addMaterialViewModel.Material.ProtocolID);
        }

        private IActionResult redirectToMaterialTab(int protocolID)
        {
            string uploadProtocolsFolder = Path.Combine(_hostingEnvironment.WebRootPath, AppUtility.ParentFolderName.Materials.ToString());
            var materials = _context.Materials.Include(m => m.Product).Where(m => m.ProtocolID == protocolID);
            Dictionary<Material, List<DocumentFolder>> MaterialFolders = FillMaterialDocumentsModel(materials, uploadProtocolsFolder);
            return PartialView("_MaterialTab", new MaterialTabViewModel() { Materials = materials, MaterialCategories = _context.MaterialCategories, Folders = (Lookup<Material, List<DocumentFolder>>)MaterialFolders.ToLookup(o => o.Key, o => o.Value) });
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
                    foreach (var url in createProtocolsViewModel.Protocol.Urls)
                    {
                        if (url.LinkID == 0)
                        {
                            _context.Entry(url).State = EntityState.Added;
                            await _context.SaveChangesAsync();
                            MoveDocumentsOutOfTempFolder(createProtocolsViewModel.Protocol.ProtocolID, AppUtility.ParentFolderName.Protocols);

                        }
                        else
                        {
                            _context.Entry(url).State = EntityState.Modified;
                            await _context.SaveChangesAsync();
                        }
                    }

                    createProtocolsViewModel = await FillCreateProtocolsViewModel(createProtocolsViewModel.Protocol.ProtocolTypeID, createProtocolsViewModel.Protocol.ProtocolID);
                    await transaction.CommitAsync();
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
        public async Task<IActionResult> WeeklyReportsCategories()
        {
            TempData[AppUtility.TempDataTypes.MenuType.ToString()] = AppUtility.MenuItems.Protocols;
            TempData[AppUtility.TempDataTypes.SidebarType.ToString()] = AppUtility.SidebarEnum.WeeklyReports;
            TempData[AppUtility.TempDataTypes.PageType.ToString()] = AppUtility.PageTypeEnum.ProtocolsReports;
            ReportCategoriesViewModel viewModel = new ReportCategoriesViewModel
            {
                PageType = AppUtility.PageTypeEnum.ProtocolsReports.ToString(),
                SidebarType = AppUtility.SidebarEnum.WeeklyReports.ToString(),
                ReportCategories = _context.ResourceCategories.Where(rc => rc.IsMain).ToList()
            };
            return View(viewModel);
        }

        [Authorize(Roles = "Protocols")]
        public async Task<IActionResult> WeeklyReportsList()
        {
            TempData[AppUtility.TempDataTypes.MenuType.ToString()] = AppUtility.MenuItems.Protocols;
            TempData[AppUtility.TempDataTypes.SidebarType.ToString()] = AppUtility.SidebarEnum.WeeklyReports;
            TempData[AppUtility.TempDataTypes.PageType.ToString()] = AppUtility.PageTypeEnum.ProtocolsReports;
            return View();
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

            switch (CategoryType)
            {
                case 2:
                    resourceLibraryViewModel.PageType = 2;
                    resourceLibraryViewModel.ResourceCategories = _context.ResourceCategories.Where(rc => rc.IsResourceType == true);
                    break;
                case 1:
                default:
                    resourceLibraryViewModel.PageType = 1;
                    resourceLibraryViewModel.ResourceCategories = _context.ResourceCategories.Where(rc => rc.IsResourceType != true);
                    break;
            }

            return View(resourceLibraryViewModel);
        }

        [HttpGet]
        [Authorize(Roles = "Protocols")]
        public async Task<IActionResult> ResourcesList(int? ResourceCategoryID, AppUtility.SidebarEnum SidebarEnum = AppUtility.SidebarEnum.Library)
        {
            TempData[AppUtility.TempDataTypes.MenuType.ToString()] = AppUtility.MenuItems.Protocols;
            TempData[AppUtility.TempDataTypes.SidebarType.ToString()] = SidebarEnum;
            TempData[AppUtility.TempDataTypes.PageType.ToString()] = AppUtility.PageTypeEnum.ProtocolsResources;

            ResourcesListViewModel resourcesListViewModel = new ResourcesListViewModel() { IsFavoritesPage = false };
            switch (SidebarEnum)
            {
                case AppUtility.SidebarEnum.Library:
                    resourcesListViewModel.ResourcesWithFavorites = _context.Resources
                        .Include(r => r.FavoriteResources)
                        .Include(r => r.ResourceResourceCategories).ThenInclude(rrc => rrc.ResourceCategory)
                        .Where(r => r.ResourceResourceCategories.Any(rrc => rrc.ResourceCategoryID == ResourceCategoryID))
                        .Select(r => new ResourceWithFavorite
                        {
                            Resource = r,
                            IsFavorite = r.FavoriteResources.Any(fr => fr.ApplicationUserID == _userManager.GetUserId(User))
                        }).ToList();

                    resourcesListViewModel.PaginationTabs = new List<string>() { "Library", _context.ResourceCategories.Where(rc => rc.ResourceCategoryID == ResourceCategoryID).FirstOrDefault().ResourceCategoryDescription };
                    break;
                case AppUtility.SidebarEnum.Favorites:
                    resourcesListViewModel.ResourcesWithFavorites = _context.FavoriteResources
                        .Include(fr => fr.Resource).ThenInclude(r => r.ResourceResourceCategories).ThenInclude(rrc => rrc.ResourceCategory)
                        .Where(fr => fr.ApplicationUserID == _userManager.GetUserId(User))
                        .Select(fr => new ResourceWithFavorite
                        {
                            Resource = fr.Resource,
                            IsFavorite = true
                        }).ToList();
                    resourcesListViewModel.IsFavoritesPage = true;
                    resourcesListViewModel.PaginationTabs = new List<string>() { };
                    break;
                case AppUtility.SidebarEnum.SharedWithMe:
                    break;
            }




            return View(resourcesListViewModel);
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
        public async Task<IActionResult> FavoriteResources(int ResourceID, bool Favorite = true, bool ReloadFavoritesPage = false)
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
            if (ReloadFavoritesPage)
            {
                return RedirectToAction("ResourcesList", new { SidebarEnum = AppUtility.SidebarEnum.Favorites });
            }
            else
            {
                return new EmptyResult();
            }
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
            return View();
        }
        [Authorize(Roles = "Protocols")]
        public async Task<IActionResult> ResourcesFavorites()
        {
            TempData[AppUtility.TempDataTypes.MenuType.ToString()] = AppUtility.MenuItems.Protocols;
            TempData[AppUtility.TempDataTypes.SidebarType.ToString()] = AppUtility.SidebarEnum.Favorites;
            TempData[AppUtility.TempDataTypes.PageType.ToString()] = AppUtility.PageTypeEnum.ProtocolsResources;
            return View();
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

            FillDocumentsViewModel(documentsModalViewModel);
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
        private void FillDocumentsInfo(CreateProtocolsViewModel createProtoclsViewModel, string uploadFolder)
        {
            createProtoclsViewModel.DocumentsInfo = new List<DocumentFolder>();

            GetExistingFileStrings(createProtoclsViewModel.DocumentsInfo, AppUtility.FolderNamesEnum.Info, uploadFolder);
            GetExistingFileStrings(createProtoclsViewModel.DocumentsInfo, AppUtility.FolderNamesEnum.Pictures, uploadFolder);
        }



    }
}
