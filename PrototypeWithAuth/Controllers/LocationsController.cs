using System;
using System.CodeDom;
using System.CodeDom.Compiler;
using System.Collections.Generic;
using System.Collections.Immutable;
using System.Linq;
using System.Linq.Expressions;
using System.Runtime.ExceptionServices;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.AspNetCore.Mvc.ViewEngines;
using Microsoft.CodeAnalysis;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Internal;
using Microsoft.EntityFrameworkCore.Scaffolding.Metadata;
using PrototypeWithAuth.AppData;
using PrototypeWithAuth.AppData.UtilityModels;
using PrototypeWithAuth.Data;
using PrototypeWithAuth.Models;

using PrototypeWithAuth.ViewModels;

namespace PrototypeWithAuth.Controllers
{
    public class LocationsController : SharedController
    {
        public LocationsController(ApplicationDbContext context, UserManager<ApplicationUser> userManager, IHostingEnvironment hostingEnvironment, IHttpContextAccessor httpContextAccessor, ICompositeViewEngine viewEngine)
           : base(context, userManager, hostingEnvironment, viewEngine, httpContextAccessor)
        {
        }

        [HttpGet]
        [Authorize(Roles = "Requests")]
        public async Task<IActionResult> IndexForInventory(AppUtility.PageTypeEnum PageType = AppUtility.PageTypeEnum.RequestSummary)
        {
            TempData[AppUtility.TempDataTypes.PageType.ToString()] = PageType;
            TempData[AppUtility.TempDataTypes.SidebarType.ToString()] = AppUtility.SidebarEnum.Location;
            TempData[AppUtility.TempDataTypes.MenuType.ToString()] = AppUtility.MenuItems.Requests;

            var locations = _locationInstancesProc.Read(new List<Expression<Func<LocationInstance, bool>>> { li => li.LocationInstanceParentID == null }, 
                new List<ComplexIncludes<LocationInstance, ModelBase>> { new ComplexIncludes<LocationInstance, ModelBase> { Include = li => li.AllRequestLocationInstances}, new ComplexIncludes<LocationInstance, ModelBase> { Include = li => li.LocationType } });
            LocationInventoryIndexViewModel locationInventoryIndexViewModel = new LocationInventoryIndexViewModel()
            {
                LocationsDepthOfZero = locations
            };
            return View(locations);
        }


        [HttpGet]
        [Authorize(Roles = "Requests, LabManagement")]
        public async Task<IActionResult> Index(AppUtility.MenuItems SectionType, string ErrorMessage)
        {
            TempData[AppUtility.TempDataTypes.MenuType.ToString()] = SectionType;
            TempData[AppUtility.TempDataTypes.SidebarType.ToString()] = AppUtility.SidebarEnum.List;

            if (await base.IsAuthorizedAsync(SectionType))
            {

                if (SectionType.Equals(AppUtility.MenuItems.LabManagement))
                {
                    TempData[AppUtility.TempDataTypes.PageType.ToString()] = AppUtility.PageTypeEnum.LabManagementLocations;
                }
                else
                {
                    TempData[AppUtility.TempDataTypes.PageType.ToString()] = AppUtility.PageTypeEnum.RequestLocation;
                }

                LocationTypeViewModel locationTypeViewModel = new LocationTypeViewModel()
                {
                    LocationTypes = _locationTypesProc.Read(new List<Expression<Func<LocationType, bool>>>{ lt => lt.Depth == 0 }),
                    SectionType = SectionType,
                    ErrorMessage = ErrorMessage
                };
                if (AppUtility.IsAjaxRequest(Request))
                {
                    return PartialView(locationTypeViewModel);
                }
                return View(locationTypeViewModel);
            }
            else
            {
                return Redirect(base.AccessDeniedPath);
            }
        }

        [HttpGet]
        [Authorize(Roles = "Requests")]
        public async Task<IActionResult> SublocationIndex(int parentId)
        {
            if (!AppUtility.IsAjaxRequest(Request))
            {
                return PartialView("InvalidLinkPage");
            }
            SublocationIndexViewModel sublocationIndexViewModel = new SublocationIndexViewModel()
            {
                SublocationInstances = _locationInstancesProc.Read(new List<Expression<Func<LocationInstance, bool>>> { li => li.LocationInstanceParentID == parentId && !(li is TemporaryLocationInstance) },
                 new List<ComplexIncludes<LocationInstance, ModelBase>> {
                     new ComplexIncludes<LocationInstance, ModelBase> { Include = li => li.LocationInstanceParent },
                     new ComplexIncludes<LocationInstance, ModelBase> { Include = li => li.LocationRoomInstance } ,
                     new ComplexIncludes<LocationInstance, ModelBase> { Include = li => li.LabPart }
                 })
                .OrderBy(li => li.LocationNumber)
            };
            //need to load this up first because we can't check for the depth (using the locationtypes table) without getting the location type id of the parent id
            LocationInstance parentLocationInstance = await _locationInstancesProc.ReadOneAsync(new List<Expression<Func<LocationInstance, bool>>> { li => li.LocationInstanceID == parentId },
                new List<ComplexIncludes<LocationInstance, ModelBase>> {
                     new ComplexIncludes<LocationInstance, ModelBase> { Include = li => li.LocationInstanceParent},
                     new ComplexIncludes<LocationInstance, ModelBase> { Include = li => li.LocationType }
                 });
            var parentLocationType = await _locationTypesProc.ReadOneAsync(new List<Expression<Func<LocationType, bool>>> { lt => lt.LocationTypeID == parentLocationInstance.LocationTypeID });
            
            sublocationIndexViewModel.Depth = parentLocationType.Depth;

            /*
             * Right now in the js validation it should not allow anything to be 0 x 0; therefore, we can test by depth and not by a has children method
             */
            sublocationIndexViewModel.PrevLocationInstance = parentLocationInstance;


            //get the max depth this location instance can go
            LocationType locationType = new LocationType();
            if (sublocationIndexViewModel.SublocationInstances != null && sublocationIndexViewModel.SublocationInstances.Any()) //TODO: in the future handle this better
            {
                locationType = await _locationTypesProc.ReadOneAsync(new List<Expression<Func<LocationType, bool>>> { lt => lt.LocationTypeID == sublocationIndexViewModel.SublocationInstances.FirstOrDefault().LocationTypeID });

            }
            if (locationType.LocationTypeChildID != null)
            {
                //is this enough or should we actually check for children
                sublocationIndexViewModel.IsSmallestChild = false;
            }
            else
            {
                sublocationIndexViewModel.IsSmallestChild = true;
            }
            return PartialView(sublocationIndexViewModel);
        }

        [HttpGet]
        [HttpPost]
        [Authorize(Roles = "Requests, LabManagement")]
        public async  Task<IActionResult> VisualLocations(int VisualContainerId)
        {
            if (!AppUtility.IsAjaxRequest(Request))
            {
                return PartialView("InvalidLinkPage");
            }
            VisualLocationsViewModel visualLocationsViewModel = new VisualLocationsViewModel()
            {
                ParentLocationInstance = await _locationInstancesProc.ReadOneAsync(new List<Expression<Func<LocationInstance, bool>>> { m => m.LocationInstanceID == VisualContainerId },
                new List<ComplexIncludes<LocationInstance, ModelBase>> {
                     new ComplexIncludes<LocationInstance, ModelBase> { Include = li => li.LabPart}
                 })
            };

            if (!visualLocationsViewModel.ParentLocationInstance.IsEmptyShelf)
            {
                visualLocationsViewModel.ChildrenLocationInstances =
                    _locationInstancesProc.Read(new List<Expression<Func<LocationInstance, bool>>> { m => m.LocationInstanceParentID == visualLocationsViewModel.ParentLocationInstance.LocationInstanceID },
                        new List<ComplexIncludes<LocationInstance, ModelBase>> {
                             new ComplexIncludes<LocationInstance, ModelBase> {
                                 Include = m => m.RequestLocationInstances,
                                 ThenInclude = new ComplexIncludes<ModelBase, ModelBase> {
                                     Include = rli => ((RequestLocationInstance)rli).Request,
                                    ThenInclude = new ComplexIncludes<ModelBase, ModelBase> { Include = r => ((Request)r).Product}
                                    }
                                }
                         }).ToList();
                    
                LocationType locationType = new LocationType();
                if (visualLocationsViewModel.ChildrenLocationInstances != null && visualLocationsViewModel.ChildrenLocationInstances.Any()) //TODO: in the future handle this better
                {
                    locationType = await _locationTypesProc.ReadOneAsync(new List<Expression<Func<LocationType, bool>>> { lt => lt.LocationTypeID == visualLocationsViewModel.ChildrenLocationInstances.FirstOrDefault().LocationTypeID });

                }
                else
                {
                    visualLocationsViewModel.Error = "No smaller locations can be found"; // CHECK IF THIS WORKS EVERYWHERE!!!!!!!!!! IMPT TODO
                }

                if (locationType.LocationTypeChildID != null)
                {
                    //is this enough or should we actually check for children
                    visualLocationsViewModel.IsSmallestChild = false;
                }
                else
                {
                    visualLocationsViewModel.IsSmallestChild = true;
                }
                visualLocationsViewModel.CurrentEmptyChild = new LocationInstance(); // so you don't run into null problems when checking on the view
            }
            else
            {             
                if (visualLocationsViewModel.ParentLocationInstance.IsEmptyShelf && visualLocationsViewModel.ParentLocationInstance.LabPartID!=null)
                {
                    visualLocationsViewModel.CurrentEmptyChild = visualLocationsViewModel.ParentLocationInstance;
                    visualLocationsViewModel.ChildrenLocationInstances = new List<LocationInstance>();
                    visualLocationsViewModel.ChildrenLocationInstances.Add(visualLocationsViewModel.ParentLocationInstance);
                }
                else
                {
                    var ParentID = visualLocationsViewModel.ParentLocationInstance.LocationInstanceParentID;
                    visualLocationsViewModel.CurrentEmptyChild = visualLocationsViewModel.ParentLocationInstance;
                    visualLocationsViewModel.ParentLocationInstance = await _locationInstancesProc.ReadOneAsync(new List<Expression<Func<LocationInstance, bool>>> { li => li.LocationInstanceID == ParentID });

                    visualLocationsViewModel.ChildrenLocationInstances = _locationInstancesProc.Read(new List<Expression<Func<LocationInstance, bool>>> { li => li.LocationInstanceParentID == ParentID },
                        new List<ComplexIncludes<LocationInstance, ModelBase>> {
                             new ComplexIncludes<LocationInstance, ModelBase> {
                                 Include = li => li.RequestLocationInstances,
                                 ThenInclude = new ComplexIncludes<ModelBase, ModelBase> {
                                     Include = rli => ((RequestLocationInstance)rli).Request,
                                    ThenInclude = new ComplexIncludes<ModelBase, ModelBase> { Include = r => ((Request)r).Product}
                                    }
                                }
                         }).ToList();
                }
            }

            return PartialView(visualLocationsViewModel);
        }
        [HttpGet]
        [Authorize(Roles = "Requests")]
        public IActionResult LocationIndex(int typeID)
        {
            if (!AppUtility.IsAjaxRequest(Request))
            {
                return PartialView("InvalidLinkPage");
            }
            LocationIndexViewModel locationIndexViewModel = new LocationIndexViewModel()
            {
                //exclude the box and cell from locationsDepthOfZero
                LocationsDepthOfZero = _locationInstancesProc.Read(new List<Expression<Func<LocationInstance, bool>>> { li => li.LocationType.Depth == 0 && li.LocationTypeID == typeID && !(li is TemporaryLocationInstance) },
                        new List<ComplexIncludes<LocationInstance, ModelBase>> {
                             new ComplexIncludes<LocationInstance, ModelBase> {
                                 Include = li => li.LocationRoomInstance,
                                }
                         }).ToList(),
                SubLocationInstances = _locationInstancesProc.Read(new List<Expression<Func<LocationInstance, bool>>> { li => li.LocationType.Depth != 0 && li.LocationTypeID == typeID && !(li is TemporaryLocationInstance) })
                //LocationTypeParentID = typeID
            };
            return PartialView(locationIndexViewModel);
        }

        [HttpGet]
        [Authorize(Roles = "Requests")]
        public async Task<IActionResult> VisualLocationsZoom(int VisualContainerId, AppUtility.MenuItems SectionType = AppUtility.MenuItems.LabManagement)
        {
            if (!AppUtility.IsAjaxRequest(Request))
            {
                return PartialView("InvalidLinkPage");
            }
            //this only works for 80 and 196 it needs to be redone to work with 20 and 25
            VisualLocationsViewModel visualLocationsViewModel = new VisualLocationsViewModel()
            {
                ParentLocationInstance = await _locationInstancesProc.ReadOneAsync(new List<Expression<Func<LocationInstance, bool>>> { m => m.LocationInstanceID == VisualContainerId }),
                SectionType = SectionType
            };
            visualLocationsViewModel.ChildrenLocationInstances = _locationInstancesProc.Read(new List<Expression<Func<LocationInstance, bool>>> { m => m.LocationInstanceParentID == visualLocationsViewModel.ParentLocationInstance.LocationInstanceID },
                        new List<ComplexIncludes<LocationInstance, ModelBase>> {
                             new ComplexIncludes<LocationInstance, ModelBase> {
                                 Include = li => li.RequestLocationInstances,
                                 ThenInclude = new ComplexIncludes<ModelBase, ModelBase> {
                                     Include = rli => ((RequestLocationInstance)rli).Request,
                                    ThenInclude = new ComplexIncludes<ModelBase, ModelBase> { Include = r => ((Request)r).Product}
                                    }
                                }
                         }).OrderBy(li => li.LocationNumber).ToList();
            //visualLocationsViewModel.ChildrenLocationInstances.ForEach(cli => { cli.RequestLocationInstances = cli.RequestLocationInstances.Where(rli => rli.IsArchived == false).ToList(); });

            visualLocationsViewModel.ChildrenLocationInstances.ForEach(cli =>
            {
                var modelBaseListLocations = new ListImplementsModelBase<RequestLocationInstance>();
                var nonArchivedLocations = cli.RequestLocationInstances.Where(rli => rli.IsArchived == false);
                foreach (var rli in nonArchivedLocations)
                {
                    modelBaseListLocations.Add(rli);
                }
                cli.RequestLocationInstances = modelBaseListLocations;
            });
            return PartialView(visualLocationsViewModel);
        }

        [HttpGet]
        [Authorize(Roles = "LabManagement")]
        public async Task<IActionResult> AddLocation()
        {
            if (!AppUtility.IsAjaxRequest(Request))
            {
                return PartialView("InvalidLinkPage");
            }
            AddLocationViewModel addLocationViewModel = new AddLocationViewModel
            {
                LocationTypesDepthOfZero = _locationTypesProc.Read(new List<Expression<Func<LocationType, bool>>> { lt => lt.Depth == 0 }),
                LocationInstance = new LocationInstance()
            };

            return PartialView(addLocationViewModel);
        }

        [HttpGet]
        [Authorize(Roles = "LabManagement")]
        public async Task<IActionResult> SubLocation(int ParentLocationTypeID)
        {
            if (!AppUtility.IsAjaxRequest(Request))
            {
                return PartialView("InvalidLinkPage");
            }
            SubLocationViewModel subLocationViewModel = new SubLocationViewModel();
            bool go = true;
            List<LocationType> listOfChildrenTypes = new List<LocationType>();

            var locationRooms = await _locationRoomInstancesProc.Read().OrderBy(lr => lr.LocationRoomTypeID)
                .ThenBy(lr => lr.LocationRoomInstanceID).ToListAsync();
            List<SelectListItem> locationRoomsSelectList = new List<SelectListItem>();
            foreach (var r in locationRooms)
            {
                locationRoomsSelectList.Add(new SelectListItem() { Value = r.LocationRoomInstanceID + "", Text = r.LocationRoomInstanceName });
            }
            subLocationViewModel.LocationRoomInstances = locationRoomsSelectList;

            switch (ParentLocationTypeID)
            {
                case 100:
                    List<SelectListItem> listItems = new List<SelectListItem>()
                    {
                        new SelectListItem() { Value="12", Text="12 x 12"},
                        new SelectListItem() { Value="9", Text="9 x 9"}
                    };
                    subLocationViewModel.BoxTypes = listItems;
                    break;
                case 200:
                    subLocationViewModel.EmptySelectList = Enumerable.Empty<SelectListItem>().ToList();
                    subLocationViewModel.EmptyShelves80 = new Dictionary<int, bool>();
                    subLocationViewModel.EmptyShelves80.Add(0, false);
                    subLocationViewModel.EmptyShelves80.Add(1, false);
                    subLocationViewModel.EmptyShelves80.Add(2, false);
                    List<SelectListItem> boxList = new List<SelectListItem>()
                    {
                        new SelectListItem() { Value="9", Text="9 x 9"}
                    };
                    subLocationViewModel.BoxTypes = boxList;
                    break;
                case 500:
                    subLocationViewModel.LabParts = await _labPartsProc.Read().ToListAsync();
                    break;
            }
            while (go)
            {
                var currentRecord = await _locationTypesProc.ReadOneAsync(new List<Expression<Func<LocationType, bool>>> { lt => lt.LocationTypeParentID == ParentLocationTypeID });
                if (currentRecord != null)
                {
                    listOfChildrenTypes.Add(currentRecord);
                    ParentLocationTypeID = currentRecord.LocationTypeID;
                }
                else
                {
                    go = false;
                }
            }

            subLocationViewModel.LocationTypes = listOfChildrenTypes;
            subLocationViewModel.LocationInstances = new List<LocationInstance>();
            foreach (var item in subLocationViewModel.LocationTypes)
            {
                subLocationViewModel.LocationInstances.Add(new LocationInstance() { });

            }
            if (AppUtility.IsAjaxRequest(Request))
            {
                return PartialView(subLocationViewModel);
            }
            else
            {
                return View(subLocationViewModel);
            }
        }

        [HttpPost]
        [Authorize(Roles = "Requests")]
        public async Task<IActionResult> AddLocation(AddLocationViewModel addLocationViewModel, SubLocationViewModel subLocationViewModel)
        {
            if (subLocationViewModel.LocationInstances[1].LabPartID==4)
            {
                ModelState.Remove("LocationInstances[2].Height");
            }

            if (ModelState.IsValid) //make sure this allows for sublocations to be binded
            {
                var locationUpdated = await _locationInstancesProc.UpdateAsync(addLocationViewModel, subLocationViewModel);
                if (locationUpdated.Bool)
                {
                    return RedirectToAction("Index", "Locations", new { SectionType = AppUtility.MenuItems.LabManagement, ErrorMessage = addLocationViewModel.ErrorMessage });
                }
                else
                {
                    addLocationViewModel.ErrorMessage += locationUpdated.String;
                }
            }
            else
            {
                addLocationViewModel.ErrorMessage += "Model State Invalid Exception";
            }
            addLocationViewModel.LocationTypesDepthOfZero = _locationTypesProc.Read(new List<Expression<Func<LocationType, bool>>> { lt => lt.Depth == 0 });
            return PartialView(addLocationViewModel);
        }

        //private int GetLocationNumber(int typeID, bool isPart, bool isRoom)
        //{
        //    _context.LocationInstances.OfType<LocationInstance>().Where(l=>l.LocationTypeID == typeID && l.loap)
        //}


        [HttpGet]
        [Authorize(Roles = "Requests")]
        public async Task<IActionResult> HasShelfBlock(int id, int roomID)
        {
            if (!AppUtility.IsAjaxRequest(Request))
            {
                return PartialView("InvalidLinkPage");
            }
            var part = await _labPartsProc.ReadOneAsync(new List<Expression<Func<LabPart, bool>>> { lp => lp.LabPartID == id });
            var locationOfTypeCount = _locationInstancesProc.Read(new List<Expression<Func<LocationInstance, bool>>> { li => li.LabPartID == id && roomID == li.LocationInstanceParent.LocationRoomInstanceID }).Count();
            var viewModel = new HasShelfViewModel() { HasShelves = part.HasShelves, LocationNameAbrev = part.LabPartNameAbbrev + (locationOfTypeCount + 1), LocationName = part.LabPartName};
            return PartialView(viewModel);
        }

        [HttpGet]
        [Authorize(Roles = "Requests")]
        public async Task<string> GetLocationRoomName(int id)
        {
            var room = await _locationRoomInstancesProc.ReadOneAsync(new List<Expression<Func<LocationRoomInstance, bool>>> { lp => lp.LocationRoomInstanceID == id });
            return room.LocationRoomInstanceAbbrev;
        }

        [HttpGet]
        [Authorize(Roles = "Requests")]
        public string GetLocationName(int locationId)
        {
            string locationName = _locationInstancesProc.ReadOne(new List<Expression<Func<LocationInstance, bool>>> { li => li.LocationInstanceID == locationId }).FirstOrDefault().LocationInstanceName;
            return locationName;
        }
    }

}