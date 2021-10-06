using System;
using System.CodeDom;
using System.CodeDom.Compiler;
using System.Collections.Generic;
using System.Collections.Immutable;
using System.Linq;
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
using PrototypeWithAuth.Data;
using PrototypeWithAuth.Models;

using PrototypeWithAuth.ViewModels;

namespace PrototypeWithAuth.Controllers
{
    public class LocationsController : SharedController
    {
        private readonly ApplicationDbContext _context;
        private readonly UserManager<ApplicationUser> _userManager;

        public LocationsController(ApplicationDbContext context, UserManager<ApplicationUser> userManager, IHostingEnvironment hostingEnvironment, IHttpContextAccessor httpContextAccessor, ICompositeViewEngine viewEngine)
           : base(context, userManager, hostingEnvironment, viewEngine, httpContextAccessor)
        {

            _context = context;
            _userManager = userManager;
        }

        [HttpGet]
        [Authorize(Roles = "Requests")]
        public async Task<IActionResult> IndexForInventory(AppUtility.PageTypeEnum PageType = AppUtility.PageTypeEnum.RequestSummary)
        {
            TempData[AppUtility.TempDataTypes.PageType.ToString()] = PageType;
            TempData[AppUtility.TempDataTypes.SidebarType.ToString()] = AppUtility.SidebarEnum.Location;
            TempData[AppUtility.TempDataTypes.MenuType.ToString()] = AppUtility.MenuItems.Requests;

            var locations = _context.LocationInstances.OfType<LocationInstance>()
                .Include(li => li.AllRequestLocationInstances).Where(li => li.LocationInstanceParentID == null).Include(li => li.LocationType);

            LocationInventoryIndexViewModel locationInventoryIndexViewModel = new LocationInventoryIndexViewModel()
            {
                LocationsDepthOfZero = locations
            };
            return View(locations);
        }


        [HttpGet]
        [Authorize(Roles = "Requests, LabManagement")]
        public async Task<IActionResult> Index(AppUtility.MenuItems SectionType)
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
                    LocationTypes = _context.LocationTypes.Where(lt => lt.Depth == 0),
                    SectionType = SectionType
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
        public IActionResult SublocationIndex(int parentId)
        {
            SublocationIndexViewModel sublocationIndexViewModel = new SublocationIndexViewModel()
            {
                SublocationInstances = _context.LocationInstances
                .Where(li => li.LocationInstanceParentID == parentId && !(li is TemporaryLocationInstance)).Include(li => li.LocationInstanceParent).Include(li => li.LocationRoomInstance).Include(li => li.LabPart).OrderBy(li => li.LocationNumber)
            };
            //need to load this up first because we can't check for the depth (using the locationtypes table) without getting the location type id of the parent id
            LocationInstance parentLocationInstance = _context.LocationInstances.OfType<LocationInstance>().Where(li => li.LocationInstanceID == parentId).Include(li => li.LocationInstanceParent).Include(li => li.LocationType).FirstOrDefault();
            int depth = _context.LocationTypes.Where(li => li.LocationTypeID == parentLocationInstance.LocationTypeID).FirstOrDefault().Depth;
            sublocationIndexViewModel.Depth = depth;

            /*
             * Right now in the js validation it should not allow anything to be 0 x 0; therefore, we can test by depth and not by a has children method
             */
            sublocationIndexViewModel.PrevLocationInstance = parentLocationInstance;


            //get the max depth this location instance can go
            LocationType locationType = new LocationType();
            if (sublocationIndexViewModel.SublocationInstances != null && sublocationIndexViewModel.SublocationInstances.Any()) //TODO: in the future handle this better
            {
                locationType = _context.LocationTypes.Where(lt => lt.LocationTypeID == sublocationIndexViewModel.SublocationInstances.FirstOrDefault().LocationTypeID).FirstOrDefault();

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
        public IActionResult VisualLocations(int VisualContainerId)
        {
            VisualLocationsViewModel visualLocationsViewModel = new VisualLocationsViewModel()
            {
                ParentLocationInstance = _context.LocationInstances.Where(m => m.LocationInstanceID == VisualContainerId).Include(m => m.LabPart).FirstOrDefault()
            };


            if (!visualLocationsViewModel.ParentLocationInstance.IsEmptyShelf)
            {
                visualLocationsViewModel.ChildrenLocationInstances =
                    _context.LocationInstances.Where(m => m.LocationInstanceParentID == visualLocationsViewModel.ParentLocationInstance.LocationInstanceID)
                    .Include(m => m.RequestLocationInstances).ThenInclude(rli => rli.Request).ThenInclude(r => r.Product).OrderBy(li => li.LocationNumber).ToList();

                LocationType locationType = new LocationType();
                if (visualLocationsViewModel.ChildrenLocationInstances != null && visualLocationsViewModel.ChildrenLocationInstances.Any()) //TODO: in the future handle this better
                {
                    locationType = _context.LocationTypes.Where(lt => lt.LocationTypeID == visualLocationsViewModel.ChildrenLocationInstances.FirstOrDefault().LocationTypeID).FirstOrDefault();

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
                var ParentID = visualLocationsViewModel.ParentLocationInstance.LocationInstanceParentID;
                visualLocationsViewModel.CurrentEmptyChild = visualLocationsViewModel.ParentLocationInstance;
                visualLocationsViewModel.ParentLocationInstance = _context.LocationInstances.Where(li => li.LocationInstanceID == ParentID).FirstOrDefault();
                visualLocationsViewModel.ChildrenLocationInstances = _context.LocationInstances.Where(li => li.LocationInstanceParentID == ParentID)
                    .Include(li => li.RequestLocationInstances).ThenInclude(rli => rli.Request).ThenInclude(r => r.Product).ToList();
            }

            return PartialView(visualLocationsViewModel);
        }
        [HttpGet]
        [Authorize(Roles = "Requests")]
        public IActionResult LocationIndex(int typeID)
        {
            LocationIndexViewModel locationIndexViewModel = new LocationIndexViewModel()
            {
                //exclude the box and cell from locationsDepthOfZero
                LocationsDepthOfZero = _context.LocationInstances.Where(li => li.LocationType.Depth == 0 && li.LocationTypeID == typeID && !(li is TemporaryLocationInstance)).Include(li => li.LocationRoomInstance),
                SubLocationInstances = _context.LocationInstances.Where(li => li.LocationType.Depth != 0 && li.LocationTypeID == typeID && !(li is TemporaryLocationInstance))
                //LocationTypeParentID = typeID
            };
            return PartialView(locationIndexViewModel);
        }

        [HttpGet]
        [Authorize(Roles = "Requests")]
        public IActionResult VisualLocationsZoom(int VisualContainerId, AppUtility.MenuItems SectionType = AppUtility.MenuItems.LabManagement)
        {
            VisualLocationsViewModel visualLocationsViewModel = new VisualLocationsViewModel()
            {
                ParentLocationInstance = _context.LocationInstances.Where(m => m.LocationInstanceID == VisualContainerId).FirstOrDefault(),
                SectionType = SectionType
            };
            visualLocationsViewModel.ChildrenLocationInstances =
                _context.LocationInstances.Where(m => m.LocationInstanceParentID == visualLocationsViewModel.ParentLocationInstance.LocationInstanceID)
                .Include(m => m.RequestLocationInstances).ThenInclude(rli => rli.Request).ThenInclude(r => r.Product).OrderBy(li => li.LocationNumber).ToList();
            visualLocationsViewModel.ChildrenLocationInstances.ForEach(cli => { cli.RequestLocationInstances = cli.RequestLocationInstances.Where(rli => rli.IsArchived == false); });
            return PartialView(visualLocationsViewModel);
        }

        [HttpGet]
        [Authorize(Roles = "LabManagement")]
        public IActionResult AddLocation()
        {
            AddLocationViewModel addLocationViewModel = new AddLocationViewModel
            {
                LocationTypesDepthOfZero = _context.LocationTypes.Where(lt => lt.Depth == 0),
                LocationInstance = new LocationInstance()
            };

            return PartialView(addLocationViewModel);
        }

        [HttpGet]
        [Authorize(Roles = "LabManagement")]
        public async Task<IActionResult> SubLocation(int ParentLocationTypeID)
        {
            SubLocationViewModel subLocationViewModel = new SubLocationViewModel();
            bool go = true;
            List<LocationType> listOfChildrenTypes = new List<LocationType>();

            var locationRooms = await _context.LocationRoomInstances.Select(lr => lr).OrderBy(lr => lr.LocationRoomTypeID)
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
                    subLocationViewModel.LabParts = await _context.LabParts.ToListAsync();
                    break;
            }
            while (go)
            {
                var currentRecord = _context.LocationTypes.Where(lt => lt.LocationTypeParentID == ParentLocationTypeID).FirstOrDefault();
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
            //return View();
        }

        [HttpPost]
        [Authorize(Roles = "Requests")]
        public async Task<IActionResult> AddLocation(AddLocationViewModel addLocationViewModel, SubLocationViewModel subLocationViewModel)
        {
            if (ModelState.IsValid) //make sure this allows for sublocations to be binded
            {
                List<LocationInstance> waitingLocations = new List<LocationInstance>();
                List<List<int>> placeholderInstanceIds = new List<List<int>>();
                bool first = true;
                var lastParent = new LocationInstance();
                string currentAbbrev = "";
                string typeName = "";
                int typeId, parentId, typeNumber = 0;
                int locationNumber = 1;
                using (var transaction = _context.Database.BeginTransaction())
                {
                    switch (subLocationViewModel.LocationTypeParentID)
                    {
                        case 100:
                            //save parent
                            addLocationViewModel.LocationInstance.Height = subLocationViewModel.LocationInstances[0].Height;
                            addLocationViewModel.LocationInstance.Width = 1;
                            addLocationViewModel.LocationInstance.LocationTypeID = subLocationViewModel.LocationTypeParentID;
                            addLocationViewModel.LocationInstance.LocationRoomInstanceID = subLocationViewModel.LocationInstances[0].LocationRoomInstanceID;

                            _context.Add(addLocationViewModel.LocationInstance);
                            await _context.SaveChangesAsync();

                            int previousH = addLocationViewModel.LocationInstance.Height;
                            int previousW = addLocationViewModel.LocationInstance.Width;
                            for (int a = 0; a < subLocationViewModel.LocationInstances.Count; a++)
                            {
                                waitingLocations.Clear();
                                placeholderInstanceIds.Add(new List<int>());
                                typeName = _context.LocationTypes.Where(x => x.LocationTypeID == subLocationViewModel.LocationInstances[a].LocationTypeID)
                                    .FirstOrDefault().LocationTypeName.Substring(0, 1);
                                typeId = subLocationViewModel.LocationInstances[a].LocationTypeID;
                                parentId = 0;
                                int height = 0;
                                int width = 0;
                                if (a == 0) //this should just be for rack
                                {
                                    height = subLocationViewModel.LocationInstances[1].Height;
                                    width = 1;
                                }
                                else if (a == 1)
                                {
                                    height = subLocationViewModel.LocationInstances[2].Height;
                                    width = subLocationViewModel.LocationInstances[2].Height;
                                }
                                int amountOfParentLevels = 1;
                                if (!first)
                                {
                                    amountOfParentLevels = placeholderInstanceIds[a - 1].Count;
                                }
                                for (int w = 0; w < amountOfParentLevels; w++)//until finished with names from the list before
                                {
                                    if (first)
                                    {
                                        //if this is the first level - locations with no parents
                                        parentId = addLocationViewModel.LocationInstance.LocationInstanceID;
                                        first = false;
                                    }
                                    else
                                    {
                                        parentId = placeholderInstanceIds[a - 1][w]; //get the first id in the list in the depth before
                                    }
                                    lastParent = _context.LocationInstances.OfType<LocationInstance>().Where(li => li.LocationInstanceID == parentId).FirstOrDefault();
                                    locationNumber = 1; //reset location number to order locations
                                    typeNumber = 1; //the number of this depth added to this name
                                                    //RESET THE HEIGHTS ANDS WIDTHS TO ACCOUNT FOR FIRSTS BEFORE RUNNIN OR ITLL CRASHs
                                    int sublocationHeight = subLocationViewModel.LocationInstances[a].Height;
                                    for (int x = 0; x < sublocationHeight; x++)
                                    {
                                        //add letter to place
                                        int unicode = x + 65;
                                        char character = (char)unicode;
                                        int sublocationWidth = subLocationViewModel.LocationInstances[a].Width;
                                        if (a == 2)
                                        {
                                            sublocationWidth = sublocationHeight;
                                            typeName = character.ToString();
                                            typeNumber = 1;
                                        }
                                        else if (sublocationWidth == 0)
                                        {
                                            sublocationWidth = 1;
                                        }
                                        for (int y = 0; y < sublocationWidth; y++)
                                        {
                                            //add number to place
                                            currentAbbrev = typeName + (typeNumber).ToString(); //add number to the type x + y is the current number but is zero based so add one
                                            typeNumber++; //increment this
                                            LocationInstance newSublocationInstance = new LocationInstance()
                                            {
                                                LocationInstanceAbbrev = currentAbbrev,
                                                LocationInstanceName = lastParent.LocationInstanceName + currentAbbrev,
                                                LocationInstanceParentID = parentId,
                                                Height = height,
                                                Width = width,
                                                LocationTypeID = typeId,
                                                LocationNumber = locationNumber
                                            };
                                            locationNumber++;
                                            _context.Add(newSublocationInstance);
                                            waitingLocations.Add(newSublocationInstance);
                                        }
                                    }
                                }
                                await _context.SaveChangesAsync();
                                foreach (var location in waitingLocations)
                                {
                                    placeholderInstanceIds[a].Add(location.LocationInstanceID);
                                }
                            }
                            break;
                        case 200://save parent
                            addLocationViewModel.LocationInstance.Height = subLocationViewModel.LocationInstances[0].Height;
                            addLocationViewModel.LocationInstance.Width = 1;
                            addLocationViewModel.LocationInstance.LocationTypeID = subLocationViewModel.LocationTypeParentID;
                            addLocationViewModel.LocationInstance.LocationRoomInstanceID = subLocationViewModel.LocationInstances[0].LocationRoomInstanceID;

                            _context.Add(addLocationViewModel.LocationInstance);
                            await _context.SaveChangesAsync();

                            previousH = addLocationViewModel.LocationInstance.Height;
                            previousW = addLocationViewModel.LocationInstance.Width;
                            for (int b = 0; b < subLocationViewModel.LocationInstances.Count; b++)
                            {
                                waitingLocations.Clear();
                                placeholderInstanceIds.Add(new List<int>());
                                typeName = _context.LocationTypes.Where(x => x.LocationTypeID == subLocationViewModel.LocationInstances[b].LocationTypeID)
                                    .FirstOrDefault().LocationTypeName.Substring(0, 1);
                                typeId = subLocationViewModel.LocationInstances[b].LocationTypeID;
                                parentId = 0;
                                int height = 0;
                                int width = 0;
                                if (b == 0)
                                {
                                    height = subLocationViewModel.LocationInstances[1].Height;
                                    width = 1;
                                }
                                else if (b == 3)
                                {
                                    height = subLocationViewModel.LocationInstances[4].Height;
                                    width = subLocationViewModel.LocationInstances[4].Height;
                                }
                                else
                                {
                                    height = subLocationViewModel.LocationInstances[b - 1].Height;
                                    width = subLocationViewModel.LocationInstances[b - 1].Height;
                                }
                                int amountOfParentLevels = 1;
                                if (!first)
                                {
                                    amountOfParentLevels = placeholderInstanceIds[b - 1].Count;
                                }
                                for (int w = 0; w < amountOfParentLevels; w++)//until finished with names from the list before
                                {
                                    if (first)
                                    {
                                        //if this is the first level - locations with no parents
                                        parentId = addLocationViewModel.LocationInstance.LocationInstanceID;
                                        first = false;
                                    }
                                    else
                                    {
                                        parentId = placeholderInstanceIds[b - 1][w]; //get the first id in the list in the depth before
                                    }
                                    lastParent = _context.LocationInstances.OfType<LocationInstance>().Where(li => li.LocationInstanceID == parentId).FirstOrDefault();
                                    if (lastParent != null && !lastParent.IsEmptyShelf)
                                    {
                                        locationNumber = 1;
                                        typeNumber = 1; //the number of this depth added to this name
                                                        //RESET THE HEIGHTS ANDS WIDTHS TO ACCOUNT FOR FIRSTS BEFORE RUNNIN OR ITLL CRASHs
                                        int sublocationHeight = subLocationViewModel.LocationInstances[b].Height;

                                        for (int x = 0; x < sublocationHeight; x++)
                                        {
                                            int unicode = x + 65;
                                            char character = (char)unicode;
                                            int sublocationWidth = subLocationViewModel.LocationInstances[b].Width;
                                            if (b == 4)
                                            {
                                                sublocationWidth = sublocationHeight;
                                                typeName = character.ToString();
                                                typeNumber = 1;
                                            }
                                            else if (sublocationWidth == 0)
                                            {
                                                sublocationWidth = 1;
                                            }
                                            for (int y = 0; y < sublocationWidth; y++)
                                            {
                                                currentAbbrev = typeName + (typeNumber).ToString();
                                                typeNumber++; //increment this

                                                LocationInstance newSublocationInstance = new LocationInstance()
                                                {
                                                    LocationInstanceAbbrev = currentAbbrev,
                                                    LocationInstanceName = lastParent.LocationInstanceName + currentAbbrev,
                                                    LocationInstanceParentID = parentId,
                                                    Height = height,
                                                    Width = width,
                                                    LocationNumber = locationNumber,
                                                    LocationTypeID = typeId
                                                };
                                                locationNumber++;
                                                if (b == 0) //Testing Shelves
                                                {
                                                    if (subLocationViewModel.EmptyShelves80?.ContainsKey(x) == true && subLocationViewModel.EmptyShelves80[x])
                                                    {
                                                        newSublocationInstance.IsEmptyShelf = true;
                                                    }
                                                }
                                                _context.Add(newSublocationInstance);
                                                waitingLocations.Add(newSublocationInstance);
                                            }
                                        }
                                    }
                                }
                                await _context.SaveChangesAsync();
                                foreach (var location in waitingLocations)
                                {
                                    placeholderInstanceIds[b].Add(location.LocationInstanceID);
                                }
                            }
                            break;
                        case 300:
                        case 400: //for now the same as 300
                            addLocationViewModel.LocationInstance.Height = subLocationViewModel.LocationInstances[0].Height;
                            addLocationViewModel.LocationInstance.Width = 1;
                            addLocationViewModel.LocationInstance.LocationTypeID = subLocationViewModel.LocationTypeParentID;
                            addLocationViewModel.LocationInstance.LocationRoomInstanceID = subLocationViewModel.LocationInstances[0].LocationRoomInstanceID;

                            _context.Add(addLocationViewModel.LocationInstance);
                            await _context.SaveChangesAsync();

                            typeName = _context.LocationTypes.Where(x => x.LocationTypeID == subLocationViewModel.LocationInstances[0].LocationTypeID)
                                    .FirstOrDefault().LocationTypeName.Substring(0, 1);
                            typeId = subLocationViewModel.LocationInstances[0].LocationTypeID;
                            typeNumber = 1;

                            parentId = 0;
                            parentId = addLocationViewModel.LocationInstance.LocationInstanceID;
                            lastParent = addLocationViewModel.LocationInstance;

                            int sublocationHeight1 = subLocationViewModel.LocationInstances[0].Height;

                            for (int x = 0; x < sublocationHeight1; x++)
                            {
                                waitingLocations.Clear();
                                int sublocationWidth1 = subLocationViewModel.LocationInstances[0].Width;
                                if (sublocationWidth1 == 0) { sublocationWidth1 = 1; }
                                for (int y = 0; y < sublocationWidth1; y++)
                                {
                                    currentAbbrev = typeName + (typeNumber).ToString();
                                    typeNumber++; //increment this
                                    LocationInstance newSublocationInstance = new LocationInstance()
                                    {
                                        LocationInstanceAbbrev = currentAbbrev,
                                        LocationInstanceName = lastParent.LocationInstanceName + currentAbbrev,
                                        LocationInstanceParentID = parentId,
                                        LocationTypeID = typeId,
                                        IsEmptyShelf = true
                                    };
                                    _context.Add(newSublocationInstance);
                                    waitingLocations.Add(newSublocationInstance);
                                }
                            }
                            await _context.SaveChangesAsync();
                            break;
                        case 500:
                            for (int i = 0; i < subLocationViewModel.LocationInstances.Count(); i++)
                            {
                                var childHeight = subLocationViewModel.LocationInstances[i].Height;

                                if (i == 0)
                                {
                                    var existingRoom = await _context.LocationInstances.OfType<LocationInstance>().Where(li => li.LocationTypeID == 500 && li.LocationRoomInstanceID == subLocationViewModel.LocationInstances[i].LocationRoomInstanceID).FirstOrDefaultAsync();
                                    if (existingRoom == null)
                                    {
                                        var room = await _context.LocationRoomInstances.Where(lp => lp.LocationRoomInstanceID == subLocationViewModel.LocationInstances[i].LocationRoomInstanceID).FirstOrDefaultAsync();
                                        subLocationViewModel.LocationInstances[i].LocationInstanceName = room.LocationRoomInstanceAbbrev;
                                        subLocationViewModel.LocationInstances[i].LocationInstanceAbbrev = room.LocationRoomInstanceAbbrev;
                                        subLocationViewModel.LocationInstances[i].LocationTypeID = 500;
                                        subLocationViewModel.LocationInstances[i].Width = 1;
                                        subLocationViewModel.LocationInstances[i].Height = 1;

                                        _context.Add(subLocationViewModel.LocationInstances[i]);
                                        lastParent = subLocationViewModel.LocationInstances[i];
                                    }
                                    else
                                    {
                                        existingRoom.Height = existingRoom.Height++;
                                        _context.Update(existingRoom);
                                        lastParent = existingRoom;
                                    }
                                    await _context.SaveChangesAsync();

                                }
                                else if (i == 1)
                                {
                                    subLocationViewModel.LocationInstances[i].LocationInstanceParentID = lastParent.LocationInstanceID;

                                    subLocationViewModel.LocationInstances[i].Width = 1;
                                    var labPart = await _context.LabParts.Where(lp => lp.LabPartID == subLocationViewModel.LocationInstances[i].LabPartID).FirstOrDefaultAsync();

                                    var labPartByTypeCount = _context.LocationInstances.OfType<LocationInstance>().Where(l => l.LabPartID == subLocationViewModel.LocationInstances[i].LabPartID && l.LocationInstanceParentID == subLocationViewModel.LocationInstances[i].LocationInstanceParentID).Count();

                                    var labPartNameAbrev = labPart.LabPartNameAbbrev;
                                    labPartNameAbrev += (labPartByTypeCount + 1);

                                    subLocationViewModel.LocationInstances[i].LocationInstanceName = lastParent.LocationInstanceName + labPartNameAbrev;
                                    subLocationViewModel.LocationInstances[i].LocationInstanceAbbrev = labPartNameAbrev;
                                    subLocationViewModel.LocationInstances[i].LocationNumber = labPartByTypeCount + 1;

                                    if (subLocationViewModel.LocationInstances.Count() > 2)
                                    {
                                        subLocationViewModel.LocationInstances[i].Height = subLocationViewModel.LocationInstances[i + 1].Height;
                                    }
                                    else
                                    {
                                        subLocationViewModel.LocationInstances[i].Height = 1;
                                    }

                                    subLocationViewModel.LocationInstances[i].IsEmptyShelf = !labPart.HasShelves;

                                    _context.Add(subLocationViewModel.LocationInstances[i]);
                                    await _context.SaveChangesAsync();
                                    lastParent = subLocationViewModel.LocationInstances[i];
                                }
                                else if (i == 2)
                                {
                                    var childLocationType = _context.LocationTypes.Where(lt => lt.LocationTypeID == subLocationViewModel.LocationInstances[i].LocationTypeID).FirstOrDefault();

                                    for (int y = 0; y < childHeight; y++)
                                    {
                                        currentAbbrev = childLocationType.LocationTypeNameAbbre + (y + 1);
                                        _context.Add(new LocationInstance()
                                        {
                                            LocationInstanceParentID = lastParent.LocationInstanceID,
                                            LocationTypeID = subLocationViewModel.LocationInstances[i].LocationTypeID,
                                            Height = subLocationViewModel.LocationInstances[i].Height = 1,
                                            Width = subLocationViewModel.LocationInstances[i].Width = 1,
                                            LocationInstanceAbbrev = currentAbbrev,
                                            LocationInstanceName = lastParent.LocationInstanceName + currentAbbrev,
                                            IsEmptyShelf = true,
                                            LocationNumber = locationNumber
                                        });
                                        locationNumber++;
                                    }
                                    await _context.SaveChangesAsync();
                                }
                            }
                            break;
                        default:
                            //add reference to parent
                            //filling up the heights and widths with the ones put in for the location below them
                            addLocationViewModel.LocationInstance.Height = subLocationViewModel.LocationInstances[0].Height;
                            addLocationViewModel.LocationInstance.Width = subLocationViewModel.LocationInstances[0].Width;
                            addLocationViewModel.LocationInstance.LocationTypeID = subLocationViewModel.LocationTypeParentID;
                            _context.Add(addLocationViewModel.LocationInstance);
                            await _context.SaveChangesAsync();

                            string nameAbbreviation = addLocationViewModel.LocationInstance.LocationInstanceName;

                            int prevHeight = addLocationViewModel.LocationInstance.Height;
                            int prevWidth = addLocationViewModel.LocationInstance.Width;
                            for (int z = 0; z < subLocationViewModel.LocationInstances.Count; z++)/*var locationInstance in subLocationViewModel.LocationInstances*/ //for each level in the sublevels
                            {
                                //initiate new lists of placeholders otherwise will get an error when you try to insert them
                                placeholderInstanceIds.Add(new List<int>());
                                //namesPlaceholder[z] = new List<string>();
                                //placeholderInstanceIds[z] = new List<int>();

                                typeName = _context.LocationTypes.Where(x => x.LocationTypeID == subLocationViewModel.LocationInstances[z].LocationTypeID)
                                    .FirstOrDefault().LocationTypeName.Substring(0, 1);
                                typeId = subLocationViewModel.LocationInstances[z].LocationTypeID;
                                parentId = 0;
                                int height = 0;
                                int width = 0;
                                if (z < subLocationViewModel.LocationInstances.Count - 1)
                                {

                                    height = subLocationViewModel.LocationInstances[z + 1].Height;
                                    width = subLocationViewModel.LocationInstances[z + 1].Width;
                                }
                                string attachedName = "";
                                int amountOfParentLevels = 1;
                                if (!first)
                                {
                                    amountOfParentLevels = placeholderInstanceIds[z - 1].Count;
                                }
                                for (int w = 0; w < amountOfParentLevels; w++)//until finished with names from the list before
                                {
                                    if (first)
                                    {
                                        //if this is the first level - locations with no parents
                                        parentId = addLocationViewModel.LocationInstance.LocationInstanceID;
                                        attachedName = typeName;
                                        first = false;
                                    }
                                    else
                                    {
                                        parentId = placeholderInstanceIds[z - 1][w]; //get the first id in the list in the depth before
                                        attachedName = typeName; //NEEDS TO BE DONE BETTER
                                    }
                                    typeNumber = 1; //the number of this depth added to this name
                                                    //RESET THE HEIGHTS ANDS WIDTHS TO ACCOUNT FOR FIRSTS BEFORE RUNNIN OR ITLL CRASHs
                                    for (int x = 0; x < subLocationViewModel.LocationInstances[z].Height; x++)
                                    {
                                        //add letter to place
                                        int unicode = x + 65;
                                        char character = (char)unicode;
                                        for (int y = 0; y < subLocationViewModel.LocationInstances[z].Width; y++)
                                        {
                                            //add number to place
                                            string currentName = attachedName + (typeNumber).ToString(); //add number to the type x + y is the current number but is zero based so add one
                                            typeNumber++; //increment this
                                            LocationInstance newSublocationInstance = new LocationInstance()
                                            {
                                                LocationInstanceName = currentName,
                                                LocationInstanceParentID = parentId,
                                                Height = height,
                                                Width = width,
                                                LocationTypeID = typeId
                                            };
                                            _context.Add(newSublocationInstance);
                                            await _context.SaveChangesAsync();
                                            placeholderInstanceIds[z].Add(newSublocationInstance.LocationInstanceID);
                                        }
                                    }
                                }

                            }
                            break;
                    }
                    await transaction.CommitAsync();
                }
            }
            return RedirectToAction("Index", "Locations", new { SectionType = AppUtility.MenuItems.LabManagement });
        }

        //private int GetLocationNumber(int typeID, bool isPart, bool isRoom)
        //{
        //    _context.LocationInstances.OfType<LocationInstance>().Where(l=>l.LocationTypeID == typeID && l.loap)
        //}


        [HttpGet]
        [Authorize(Roles = "Requests")]
        public async Task<IActionResult> HasShelfBlock(int id, int roomID)
        {
            var part = await _context.LabParts.Where(lp => lp.LabPartID == id).FirstOrDefaultAsync();
            var locationOfTypeCount = _context.LocationInstances.OfType<LocationInstance>().Where(li => li.LabPartID == id && roomID == li.LocationInstanceParentID).Count();
            var viewModel = new HasShelfViewModel() { HasShelves = part.HasShelves, LocationName = part.LabPartNameAbbrev + (locationOfTypeCount + 1) };
            return PartialView(viewModel);
        }

        [HttpGet]
        [Authorize(Roles = "Requests")]
        public async Task<string> GetLocationRoomName(int id)
        {
            var room = await _context.LocationRoomInstances.Where(lp => lp.LocationRoomInstanceID == id).FirstOrDefaultAsync();
            return room.LocationRoomInstanceAbbrev;
        }

        [HttpGet]
        [Authorize(Roles = "Requests")]
        public string GetLocationName(int locationId)
        {
            string locationName = _context.LocationInstances.Where(li => li.LocationInstanceID == locationId).Select(li => li.LocationInstanceName).FirstOrDefault();
            return locationName;
        }
    }

}