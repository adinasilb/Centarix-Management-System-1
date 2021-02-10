using System;
using System.CodeDom;
using System.CodeDom.Compiler;
using System.Collections.Generic;
using System.Collections.Immutable;
using System.Linq;
using System.Runtime.ExceptionServices;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
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
    public class LocationsController : ControllerBase
    {
        private readonly ApplicationDbContext _context;

        public LocationsController(ApplicationDbContext context) : base(context)
        {
            _context = context;
        }

        [HttpGet]
        [Authorize(Roles = "Requests")]
        public async Task<IActionResult> IndexForInventory(AppUtility.PageTypeEnum PageType = AppUtility.PageTypeEnum.RequestSummary)
        {
            TempData[AppUtility.TempDataTypes.PageType.ToString()] = PageType;
            TempData[AppUtility.TempDataTypes.SidebarType.ToString()] = AppUtility.SidebarEnum.Location;
            TempData[AppUtility.TempDataTypes.MenuType.ToString()] = AppUtility.MenuItems.Requests;

            var locations = _context.LocationInstances
                .Include(li => li.AllRequestLocationInstances).Where(li => li.LocationInstanceParentID == null).Include(li => li.LocationType);

            LocationInventoryIndexViewModel locationInventoryIndexViewModel = new LocationInventoryIndexViewModel()
            {
                LocationsDepthOfZero = locations
            };
            return View(locations);
        }


        [HttpGet]
        [Authorize(Roles = "Requests, LabManagement")]
        public IActionResult Index(AppUtility.MenuItems SectionType)
        {
            TempData[AppUtility.TempDataTypes.MenuType.ToString()] = SectionType;
            TempData[AppUtility.TempDataTypes.SidebarType.ToString()] = AppUtility.SidebarEnum.List;
            // Added by Dani because to make CSS work better
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

            return View(locationTypeViewModel);
        }

        [HttpGet]
        [Authorize(Roles = "Requests")]
        public IActionResult SublocationIndex(int parentId)
        {
            SublocationIndexViewModel sublocationIndexViewModel = new SublocationIndexViewModel()
            {
                SublocationInstances = _context.LocationInstances
                .Where(li => li.LocationInstanceParentID == parentId).Include(li => li.LocationInstanceParent)
            };
            //need to load this up first because we can't check for the depth (using the locationtypes table) without getting the location type id of the parent id
            LocationInstance parentLocationInstance = _context.LocationInstances.Where(li => li.LocationInstanceID == parentId).Include(li => li.LocationInstanceParent).Include(li => li.LocationType).FirstOrDefault();
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
        [Authorize(Roles = "Requests")]
        public IActionResult VisualLocations(int VisualContainerId)
        {
            VisualLocationsViewModel visualLocationsViewModel = new VisualLocationsViewModel()
            {
                ParentLocationInstance = _context.LocationInstances.Where(m => m.LocationInstanceID == VisualContainerId).FirstOrDefault()
            };
            if (!visualLocationsViewModel.ParentLocationInstance.IsEmptyShelf)
            {
                visualLocationsViewModel.ChildrenLocationInstances =
                    _context.LocationInstances.Where(m => m.LocationInstanceParentID == visualLocationsViewModel.ParentLocationInstance.LocationInstanceID)
                    .Include(m => m.RequestLocationInstances).ThenInclude(rli => rli.Request).ThenInclude(r => r.Product).ToList();

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
                LocationsDepthOfZero = _context.LocationInstances.Where(li => li.LocationType.Depth == 0 && li.LocationTypeID == typeID),
                SubLocationInstances = _context.LocationInstances.Where(li => li.LocationType.Depth != 0 && li.LocationTypeID == typeID),
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
                .Include(m => m.RequestLocationInstances).ThenInclude(rli => rli.Request).ThenInclude(r => r.Product).ToList();

            return PartialView(visualLocationsViewModel);
        }

        [HttpGet]
        [Authorize(Roles = "Requests, LabManagement")]
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
        [Authorize(Roles = "Requests")]
        public async Task<IActionResult> SubLocation(int ParentLocationTypeID)
        {
            SubLocationViewModel subLocationViewModel = new SubLocationViewModel();
            bool go = true;
            List<LocationType> listOfChildrenTypes = new List<LocationType>();
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
                    var locationRooms = await _context.LocationRoomInstances.Include(l=>l.LocationRoomType).ToListAsync();
                    List<SelectListItem> locationRoomsSelectList = new List<SelectListItem>();
                    foreach(var r in locationRooms)
                    {
                        var description = r.LocationRoomType.LocationRoomTypeDescription;
                        var isMoreOfType = locationRooms.Where(l => l.LocationRoomType == r.LocationRoomType && l.LocationRoomInstanceID!=r.LocationRoomInstanceID).Any();
                        if(isMoreOfType)
                        {
                            description += r.LocationNumber;
                        }
                        locationRoomsSelectList.Add(new SelectListItem() { Value = r.LocationRoomInstanceID + "", Text = description });
                    }
                    subLocationViewModel.LocationRoomInstances = locationRoomsSelectList;
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
                subLocationViewModel.LocationInstances.Add(new LocationInstance() {});
                
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
                List<List<string>> namesPlaceholder = new List<List<string>>();
                List<List<int>> placeholderInstanceIds = new List<List<int>>();
                bool first = true;
                using (var transaction = _context.Database.BeginTransaction())
                {

                    switch (subLocationViewModel.LocationTypeParentID)
                    {
                        case 100:
                            //save parent
                            addLocationViewModel.LocationInstance.Height = subLocationViewModel.LocationInstances[0].Height;
                            addLocationViewModel.LocationInstance.Width = 1;
                            addLocationViewModel.LocationInstance.LocationTypeID = subLocationViewModel.LocationTypeParentID;
                            _context.Add(addLocationViewModel.LocationInstance);
                            await _context.SaveChangesAsync();
                            //placeholderInstanceIds.Add(new List<int>());
                            //placeholderInstanceIds[0].Add(addLocationViewModel.LocationInstance.LocationInstanceID);
                            int lastCompanyLocNo = 1;
                            if (_context.LocationInstances.Any())
                            {
                                lastCompanyLocNo = _context.LocationInstances.OrderByDescending(li => li.CompanyLocationNo).First().CompanyLocationNo + 1;
                            }
                            string nameAbbrev = addLocationViewModel.LocationInstance.LocationInstanceName;
                            int previousH = addLocationViewModel.LocationInstance.Height;
                            int previousW = addLocationViewModel.LocationInstance.Width;
                            for (int a = 0; a < subLocationViewModel.LocationInstances.Count; a++)
                            {
                                namesPlaceholder.Add(new List<string>());
                                placeholderInstanceIds.Add(new List<int>());
                                string typeName = _context.LocationTypes.Where(x => x.LocationTypeID == subLocationViewModel.LocationInstances[a].LocationTypeID)
                                    .FirstOrDefault().LocationTypeName.Substring(0, 1);
                                int typeId = subLocationViewModel.LocationInstances[a].LocationTypeID;
                                string place = "";
                                int parentId = 0;
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
                                string attachedName = "";
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
                                        lastCompanyLocNo++;
                                        addLocationViewModel.LocationInstance.CompanyLocationNo = lastCompanyLocNo;
                                        parentId = addLocationViewModel.LocationInstance.LocationInstanceID;
                                        attachedName = nameAbbrev + typeName;
                                        first = false;
                                    }
                                    else
                                    {
                                        parentId = placeholderInstanceIds[a - 1][w]; //get the first id in the list in the depth before
                                        attachedName = namesPlaceholder[a - 1][w] + typeName; //NEEDS TO BE DONE BETTER
                                    }
                                    int typeNumber = 1; //the number of this depth added to this name
                                                        //RESET THE HEIGHTS ANDS WIDTHS TO ACCOUNT FOR FIRSTS BEFORE RUNNIN OR ITLL CRASHs
                                    int sublocationHeight = subLocationViewModel.LocationInstances[a].Height;
                                    for (int x = 0; x < sublocationHeight; x++)
                                    {
                                        //add letter to place
                                        int unicode = x + 65;
                                        char character = (char)unicode;
                                        place = character.ToString();
                                        int sublocationWidth = subLocationViewModel.LocationInstances[a].Width;
                                        if (a == 2) { sublocationWidth = sublocationHeight; }
                                        else if (sublocationWidth == 0) { sublocationWidth = 1; }
                                        for (int y = 0; y < sublocationWidth; y++)
                                        {
                                            //add number to place
                                            string FullPlace = place + (y + 1).ToString();
                                            string currentName = attachedName + (typeNumber).ToString(); //add number to the type x + y is the current number but is zero based so add one
                                            typeNumber++; //increment this
                                            LocationInstance newSublocationInstance = new LocationInstance()
                                            {
                                                LocationInstanceName = currentName,
                                                LocationInstanceParentID = parentId,
                                                Height = height,
                                                Width = width,
                                                LocationTypeID = typeId,
                                                CompanyLocationNo = lastCompanyLocNo,
                                                Place = FullPlace
                                            };
                                            _context.Add(newSublocationInstance);
                                            try
                                            {
                                                _context.SaveChanges(); //DO WE NEED THIS HERE OR CAN WE DO IT ONCE AT THE END
                                                transaction.Commit();
                                            }
                                            catch (Exception e)
                                            {
                                                //DeleteLocationsIfFailed(addLocationViewModel.LocationInstance, placeholderInstanceIds);
                                                transaction.Rollback();
                                                return RedirectToAction("Index");
                                            }
                                            placeholderInstanceIds[a].Add(newSublocationInstance.LocationInstanceID);
                                            namesPlaceholder[a].Add(newSublocationInstance.LocationInstanceName);
                                        }
                                    }
                                }
                            }
                            break;
                        case 200://save parent
                            addLocationViewModel.LocationInstance.Height = subLocationViewModel.LocationInstances[0].Height;
                            addLocationViewModel.LocationInstance.Width = 1;
                            addLocationViewModel.LocationInstance.LocationTypeID = subLocationViewModel.LocationTypeParentID;

                            int CompanyLocNo = 1;
                            if (_context.LocationInstances.Any())
                            {
                                CompanyLocNo = _context.LocationInstances.OrderByDescending(li => li.CompanyLocationNo).First().CompanyLocationNo + 1;
                            }
                            addLocationViewModel.LocationInstance.CompanyLocationNo = CompanyLocNo;

                            _context.Add(addLocationViewModel.LocationInstance);
                            try
                            {
                                _context.SaveChanges(); //DO WE NEED THIS HERE OR CAN WE DO IT ONCE AT THE END
                            }
                            catch (Exception e)
                            {
                                //DeleteLocationsIfFailed(addLocationViewModel.LocationInstance, placeholderInstanceIds);
                                transaction.Rollback();
                                return RedirectToAction("Index");
                            }

                            nameAbbrev = addLocationViewModel.LocationInstance.LocationInstanceName;
                            previousH = addLocationViewModel.LocationInstance.Height;
                            previousW = addLocationViewModel.LocationInstance.Width;
                            for (int b = 0; b < subLocationViewModel.LocationInstances.Count; b++)
                            {
                                namesPlaceholder.Add(new List<string>());
                                placeholderInstanceIds.Add(new List<int>());
                                string typeName = _context.LocationTypes.Where(x => x.LocationTypeID == subLocationViewModel.LocationInstances[b].LocationTypeID)
                                    .FirstOrDefault().LocationTypeName.Substring(0, 1);
                                int typeId = subLocationViewModel.LocationInstances[b].LocationTypeID;
                                string place = "";
                                int parentId = 0;
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
                                string attachedName = "";
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
                                        CompanyLocNo++;
                                        addLocationViewModel.LocationInstance.CompanyLocationNo = CompanyLocNo;
                                        parentId = addLocationViewModel.LocationInstance.LocationInstanceID;
                                        attachedName = nameAbbrev + typeName;
                                        first = false;
                                    }
                                    else
                                    {
                                        parentId = placeholderInstanceIds[b - 1][w]; //get the first id in the list in the depth before
                                        attachedName = namesPlaceholder[b - 1][w] + typeName; //NEEDS TO BE DONE BETTER
                                    }
                                    int typeNumber = 1; //the number of this depth added to this name
                                                        //RESET THE HEIGHTS ANDS WIDTHS TO ACCOUNT FOR FIRSTS BEFORE RUNNIN OR ITLL CRASHs
                                    int sublocationHeight = subLocationViewModel.LocationInstances[b].Height;
                                    for (int x = 0; x < sublocationHeight; x++)
                                    {
                                        //add letter to place
                                        int unicode = x + 65;
                                        char character = (char)unicode;
                                        place = character.ToString();
                                        int sublocationWidth = subLocationViewModel.LocationInstances[b].Width;
                                        if (b == 4) { sublocationWidth = sublocationHeight; }
                                        else if (sublocationWidth == 0) { sublocationWidth = 1; }
                                        for (int y = 0; y < sublocationWidth; y++)
                                        {
                                            //add number to place
                                            string FullPlace = place + (y + 1).ToString();
                                            string currentName = attachedName + (typeNumber).ToString(); //add number to the type x + y is the current number but is zero based so add one
                                            typeNumber++; //increment this

                                            LocationInstance newSublocationInstance = new LocationInstance()
                                            {
                                                LocationInstanceName = currentName,
                                                LocationInstanceParentID = parentId,
                                                Height = height,
                                                Width = width,
                                                LocationTypeID = typeId,
                                                CompanyLocationNo = CompanyLocNo,
                                                Place = FullPlace
                                            };
                                            _context.Add(newSublocationInstance);
                                            try
                                            {
                                                _context.SaveChanges(); //DO WE NEED THIS HERE OR CAN WE DO IT ONCE AT THE END
                                            }
                                            catch (Exception e)
                                            {
                                                transaction.Rollback();
                                                //DeleteLocationsIfFailed(addLocationViewModel.LocationInstance, placeholderInstanceIds);
                                                return RedirectToAction("Index");
                                            }
                                            if (b == 0) //Testing Shelves
                                            {
                                                if (subLocationViewModel.EmptyShelves80?.ContainsKey(x) == true && subLocationViewModel.EmptyShelves80[x])
                                                {
                                                    newSublocationInstance.IsEmptyShelf = true;
                                                    _context.Update(newSublocationInstance);
                                                    try
                                                    {
                                                        _context.SaveChanges(); //DO WE NEED THIS HERE OR CAN WE DO IT ONCE AT THE END
                                                        transaction.Commit();
                                                    }
                                                    catch (Exception e)
                                                    {
                                                        transaction.Rollback();
                                                        //DeleteLocationsIfFailed(addLocationViewModel.LocationInstance, placeholderInstanceIds);
                                                        return RedirectToAction("Index");
                                                    }
                                                }
                                                else
                                                {
                                                    placeholderInstanceIds[b].Add(newSublocationInstance.LocationInstanceID);
                                                    namesPlaceholder[b].Add(newSublocationInstance.LocationInstanceName);
                                                }
                                            }
                                            else
                                            {
                                                placeholderInstanceIds[b].Add(newSublocationInstance.LocationInstanceID);
                                                namesPlaceholder[b].Add(newSublocationInstance.LocationInstanceName);
                                            }

                                        }
                                    }
                                }
                            }
                            break;
                        case 300:
                        case 400: //for now the same as 300
                            addLocationViewModel.LocationInstance.Height = subLocationViewModel.LocationInstances[0].Height;
                            addLocationViewModel.LocationInstance.Width = 1;
                            addLocationViewModel.LocationInstance.LocationTypeID = subLocationViewModel.LocationTypeParentID;

                            int CoNo = 1;
                            if (_context.LocationInstances.Any())
                            {
                                CoNo = _context.LocationInstances.OrderByDescending(li => li.CompanyLocationNo).First().CompanyLocationNo + 1;
                            }
                            addLocationViewModel.LocationInstance.CompanyLocationNo = CoNo;
                            string nameAbbrev1 = addLocationViewModel.LocationInstance.LocationInstanceName;

                            _context.Add(addLocationViewModel.LocationInstance);
                            try
                            {
                                _context.SaveChanges(); //DO WE NEED THIS HERE OR CAN WE DO IT ONCE AT THE END
                            }
                            catch (Exception e)
                            {
                                transaction.Rollback();
                                //DeleteLocationsIfFailed(addLocationViewModel.LocationInstance, placeholderInstanceIds);
                                return RedirectToAction("Index");
                            }

                            string typeName1 = _context.LocationTypes.Where(x => x.LocationTypeID == subLocationViewModel.LocationInstances[0].LocationTypeID)
                                    .FirstOrDefault().LocationTypeName.Substring(0, 1);
                            int typeId1 = subLocationViewModel.LocationInstances[0].LocationTypeID;
                            string place1 = "";
                            int parentId1 = 0;
                            string attachedName1 = "";
                            //CoNo++;
                            //addLocationViewModel.LocationInstance.CompanyLocationNo = CoNo;
                            parentId1 = addLocationViewModel.LocationInstance.LocationInstanceID;
                            attachedName1 = nameAbbrev1 + typeName1;
                            int typeNumber1 = 1;

                            int sublocationHeight1 = subLocationViewModel.LocationInstances[0].Height;

                            placeholderInstanceIds.Add(new List<int>());
                            for (int x = 0; x < sublocationHeight1; x++)
                            {
                                //add letter to place
                                int unicode = x + 65;
                                char character = (char)unicode;
                                place1 = character.ToString();
                                int sublocationWidth1 = subLocationViewModel.LocationInstances[0].Width;
                                if (sublocationWidth1 == 0) { sublocationWidth1 = 1; }
                                for (int y = 0; y < sublocationWidth1; y++)
                                {
                                    string FullPlace1 = place1 + (y + 1).ToString();
                                    string currentName = attachedName1 + (typeNumber1).ToString(); //add number to the type x + y is the current number but is zero based so add one
                                    typeNumber1++; //increment this
                                    LocationInstance newSublocationInstance = new LocationInstance()
                                    {
                                        LocationInstanceName = currentName,
                                        LocationInstanceParentID = parentId1,
                                        LocationTypeID = typeId1,
                                        CompanyLocationNo = CoNo,
                                        Place = FullPlace1
                                    };
                                    _context.Add(newSublocationInstance);
                                    try
                                    {
                                        _context.SaveChanges(); //DO WE NEED THIS HERE OR CAN WE DO IT ONCE AT THE END
                                        transaction.Commit();
                                    }
                                    catch (Exception e)
                                    {
                                        transaction.Rollback();
                                        //DeleteLocationsIfFailed(addLocationViewModel.LocationInstance, placeholderInstanceIds);
                                        return RedirectToAction("Index");
                                    }
                                    placeholderInstanceIds[0].Add(newSublocationInstance.LocationInstanceID);
                                }
                            }
                            break;
                        case 500:
                            addLocationViewModel.LocationInstance.Height = 1;
                            addLocationViewModel.LocationInstance.Width = 1;
                            _context.Add(addLocationViewModel.LocationInstance);
                            _context.SaveChanges();
                            for (int i = 0; i < subLocationViewModel.LocationInstances.Count(); i++)
                            {
                                var childHeight = subLocationViewModel.LocationInstances[i].Height;
                                if (i == subLocationViewModel.LocationInstances.Count()-1)
                                {
                                    for(int y=0; y<childHeight; y++)
                                    {
                                        _context.Add(new LocationInstance()
                                        {
                                            LocationInstanceParentID = subLocationViewModel.LocationInstances[i - 1].LocationInstanceID,
                                            LocationTypeID = subLocationViewModel.LocationInstances[i].LocationTypeID,
                                            Height = subLocationViewModel.LocationInstances[i].Height =1,
                                            Width = subLocationViewModel.LocationInstances[i].Width = 1,
                                            LocationInstanceName = "sdfsd"
                                    });
                                    }
                                    await _context.SaveChangesAsync();
                                }
                                else if(i == 0)
                                {
                                    subLocationViewModel.LocationInstances[i].LocationInstanceParentID = addLocationViewModel.LocationInstance.LocationInstanceID;
                                    subLocationViewModel.LocationInstances[i].Height =1;
                                    subLocationViewModel.LocationInstances[i].Width = 1;
                                    subLocationViewModel.LocationInstances[i].LocationInstanceName = "sdfsd";
                                    _context.Add(subLocationViewModel.LocationInstances[i]);
                                    await _context.SaveChangesAsync();
                                }
                                else
                                {
                                    subLocationViewModel.LocationInstances[i].LocationInstanceParentID = subLocationViewModel.LocationInstances[i-1].LocationInstanceID;
                            
                                    subLocationViewModel.LocationInstances[i].Width = 1;
                                    subLocationViewModel.LocationInstances[i].LocationInstanceName = "sdfsd";
                                    _context.Add(subLocationViewModel.LocationInstances[i]);
                                    if(subLocationViewModel.LocationInstances.Count()> 2)
                                    {
                                        subLocationViewModel.LocationInstances[i].Height = subLocationViewModel.LocationInstances[i + 1].Height;
                                    }
                                    else
                                    {
                                        subLocationViewModel.LocationInstances[i].Height = 1;
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
                            try
                            {
                                _context.SaveChanges(); //DO WE NEED THIS HERE OR CAN WE DO IT ONCE AT THE END
                            }
                            catch (Exception e)
                            {
                                transaction.Rollback();
                                //DeleteLocationsIfFailed(addLocationViewModel.LocationInstance, placeholderInstanceIds);
                                return RedirectToAction("Index");
                            }

                            int companyLocationNo = _context.LocationInstances.OrderByDescending(li => li.CompanyLocationNo).First().CompanyLocationNo;

                            string nameAbbreviation = addLocationViewModel.LocationInstance.LocationInstanceName;

                            int prevHeight = addLocationViewModel.LocationInstance.Height;
                            int prevWidth = addLocationViewModel.LocationInstance.Width;
                            for (int z = 0; z < subLocationViewModel.LocationInstances.Count; z++)/*var locationInstance in subLocationViewModel.LocationInstances*/ //for each level in the sublevels
                            {
                                //initiate new lists of placeholders otherwise will get an error when you try to insert them
                                namesPlaceholder.Add(new List<string>());
                                placeholderInstanceIds.Add(new List<int>());
                                //namesPlaceholder[z] = new List<string>();
                                //placeholderInstanceIds[z] = new List<int>();

                                string typeName = _context.LocationTypes.Where(x => x.LocationTypeID == subLocationViewModel.LocationInstances[z].LocationTypeID)
                                    .FirstOrDefault().LocationTypeName.Substring(0, 1);
                                int typeId = subLocationViewModel.LocationInstances[z].LocationTypeID;
                                string place = "";
                                int parentId = 0;
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
                                        companyLocationNo++;
                                        addLocationViewModel.LocationInstance.CompanyLocationNo = companyLocationNo;
                                        parentId = addLocationViewModel.LocationInstance.LocationInstanceID;
                                        attachedName = nameAbbreviation + typeName;
                                        first = false;
                                    }
                                    else
                                    {
                                        parentId = placeholderInstanceIds[z - 1][w]; //get the first id in the list in the depth before
                                        attachedName = namesPlaceholder[z - 1][w] + typeName; //NEEDS TO BE DONE BETTER
                                    }
                                    int typeNumber = 1; //the number of this depth added to this name
                                                        //RESET THE HEIGHTS ANDS WIDTHS TO ACCOUNT FOR FIRSTS BEFORE RUNNIN OR ITLL CRASHs
                                    for (int x = 0; x < subLocationViewModel.LocationInstances[z].Height; x++)
                                    {
                                        //add letter to place
                                        int unicode = x + 65;
                                        char character = (char)unicode;
                                        place = character.ToString();
                                        for (int y = 0; y < subLocationViewModel.LocationInstances[z].Width; y++)
                                        {
                                            //add number to place
                                            string FullPlace = place + (y + 1).ToString();
                                            string currentName = attachedName + (typeNumber).ToString(); //add number to the type x + y is the current number but is zero based so add one
                                            typeNumber++; //increment this
                                            LocationInstance newSublocationInstance = new LocationInstance()
                                            {
                                                LocationInstanceName = currentName,
                                                LocationInstanceParentID = parentId,
                                                Height = height,
                                                Width = width,
                                                LocationTypeID = typeId,
                                                Place = FullPlace
                                            };
                                            _context.Add(newSublocationInstance);
                                            try
                                            {
                                                _context.SaveChanges(); //DO WE NEED THIS HERE OR CAN WE DO IT ONCE AT THE END
                                                transaction.Commit();
                                            }
                                            catch (Exception e)
                                            {
                                                transaction.Rollback();
                                                //DeleteLocationsIfFailed(addLocationViewModel.LocationInstance, placeholderInstanceIds);
                                                return RedirectToAction("Index");
                                            }
                                            placeholderInstanceIds[z].Add(newSublocationInstance.LocationInstanceID);
                                            namesPlaceholder[z].Add(newSublocationInstance.LocationInstanceName);
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

        public void DeleteLocationsIfFailed(LocationInstance ParentLocationInstance, List<List<int>> placeholderIds)
        {
            _context.Remove(ParentLocationInstance);
            foreach (List<int> listID in placeholderIds)
            {
                foreach (int liID in listID)
                {
                    var currentInstance = _context.LocationInstances.Where(li => li.LocationInstanceID == liID).FirstOrDefault();
                    _context.Remove(currentInstance);
                    _context.SaveChanges();
                }
            }
        }

        [HttpGet]
        [Authorize(Roles = "Requests")]
        public async Task<IActionResult> HasShelfBlock(int id)
        {
            var part = await _context.LabParts.Where(lp => lp.LabPartID == id).FirstOrDefaultAsync();
            if(part.HasShelves)
            {
                return PartialView();
            }
            return PartialView("Default");
        }


        [HttpGet]
        [Authorize(Roles = "Requests")]
        public IActionResult AddLocationType()
        {
            AddLocationTypeViewModel addLocationTypeViewModel = new AddLocationTypeViewModel
            {
                Sublocations = new List<string>()
            };
            addLocationTypeViewModel.Sublocations.Add(""); // instantiating an empty sublocation, the rest will not go thru the view - its hard coded into the js
            return PartialView(addLocationTypeViewModel);
        }

        [HttpPost]
        [Authorize(Roles = "Requests")]
        public IActionResult AddLocationType(AddLocationTypeViewModel addLocationTypeViewModel)
        {
            int foriegnKeyParent = 0; //retain primary key to be a foriegn key for next model

            for (int i = 0; i < addLocationTypeViewModel.Sublocations.Count; i++)
            {
                if (i == 0)
                {
                    var listOfLocationNames = _context.LocationTypes.Where(lt => lt.LocationTypeName == addLocationTypeViewModel.Sublocations[0]).Where(lt => lt.Depth == 0).ToList();
                    if (listOfLocationNames.Count > 0)
                    {
                        AddLocationTypeViewModel addLocationTypeViewModelErrorDoubleName = new AddLocationTypeViewModel();
                        return View(addLocationTypeViewModelErrorDoubleName);
                    }
                }

                LocationType locationType = new LocationType()
                {
                    LocationTypeName = addLocationTypeViewModel.Sublocations[i],
                    Depth = i
                };

                if (foriegnKeyParent > 0)
                {
                    locationType.LocationTypeParentID = foriegnKeyParent;
                }
                _context.Add(locationType);
                _context.SaveChanges();
                if (foriegnKeyParent > 0)
                {
                    var prevRecord = _context.LocationTypes.Where(pr => pr.LocationTypeID == foriegnKeyParent).FirstOrDefault();
                    prevRecord.LocationTypeChildID = locationType.LocationTypeID;
                    _context.Update(locationType);
                    _context.SaveChanges();
                }
                foriegnKeyParent = locationType.LocationTypeID;

            };



            return RedirectToAction("Index");
        }

    }

}