﻿using System;
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
    public class LocationsController : Controller
    {
        private readonly ApplicationDbContext _context;

        public LocationsController(ApplicationDbContext context)
        {
            _context = context;
        }

        [HttpGet]
        [Authorize(Roles = "Admin, OrdersAndInventory")]
        public async Task<IActionResult> IndexForInventory()
        {
            TempData["PageType"] = AppUtility.RequestPageTypeEnum.Inventory;

            var locations = _context.LocationInstances
                .Include(li => li.AllRequestLocationInstances).Where(li => li.LocationInstanceParentID == null).Include(li => li.LocationType);

            LocationInventoryIndexViewModel locationInventoryIndexViewModel = new LocationInventoryIndexViewModel()
            {
                LocationsDepthOfZero = locations
            };
            return View(locations);
        }


        [HttpGet]
        [Authorize(Roles = "Admin, OrdersAndInventory")]
        public IActionResult Index(AppUtility.MenuItems SectionType)
        {
            TempData["SectionType"] = SectionType;
            // Added by Dani because to make CSS work better
            if (SectionType.Equals(AppUtility.MenuItems.LabManagement))
            {
                TempData["PageType"] = AppUtility.LabManagementPageTypeEnum.Locations;
            }
            else
            {
                TempData["PageType"] = AppUtility.RequestPageTypeEnum.Location;
            }

            LocationTypeViewModel locationTypeViewModel = new LocationTypeViewModel()
            {
                LocationTypes = _context.LocationTypes.Where(lt => lt.Depth == 0),
                SectionType = SectionType
            };

            return View(locationTypeViewModel);
        }

        [HttpGet]
        [Authorize(Roles = "Admin, OrdersAndInventory")]
        public IActionResult SublocationIndex(int parentId)
        {
            SublocationIndexViewModel sublocationIndexViewModel = new SublocationIndexViewModel()
            {
                SublocationInstances = _context.LocationInstances
                .Where(li => li.LocationInstanceParentID == parentId).Include(li => li.LocationInstanceParent)
            };
            //need to load this up first because we can't check for the depth (using the locationtypes table) without getting the location type id of the parent id
            LocationInstance parentLocationInstance = _context.LocationInstances.Where(li => li.LocationInstanceID == parentId).Include(li => li.LocationInstanceParent).FirstOrDefault();
            int depth = _context.LocationTypes.Where(li => li.LocationTypeID == parentLocationInstance.LocationTypeID).FirstOrDefault().Depth;
            sublocationIndexViewModel.Depth = depth;

            /*
             * Right now in the js validation it should not allow anything to be 0 x 0; therefore, we can test by depth and not by a has children method
             */
            if (depth > 1)
            {
                sublocationIndexViewModel.PrevLocationInstance = parentLocationInstance;
                sublocationIndexViewModel.IsSmallestChild = false;
            }

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
        [Authorize(Roles = "Admin, OrdersAndInventory")]
        public IActionResult VisualLocations(int VisualContainerId)
        {
            VisualLocationsViewModel visualLocationsViewModel = new VisualLocationsViewModel()
            {
                ParentLocationInstance = _context.LocationInstances.Where(m => m.LocationInstanceID == VisualContainerId).FirstOrDefault()
            };
            if (!visualLocationsViewModel.ParentLocationInstance.IsEmpty)
            {
                visualLocationsViewModel.ChildrenLocationInstances =
                    _context.LocationInstances.Where(m => m.LocationInstanceParentID == visualLocationsViewModel.ParentLocationInstance.LocationInstanceID)
                    .Include(m => m.RequestLocationInstances).ThenInclude(rli => rli.Request).ThenInclude(r => r.Product).ToList();

                LocationType locationType = new LocationType();
                if (visualLocationsViewModel.ChildrenLocationInstances != null && visualLocationsViewModel.ChildrenLocationInstances.Any()) //TODO: in the future handle this better
                {
                    locationType = _context.LocationTypes.Where(lt => lt.LocationTypeID == visualLocationsViewModel.ChildrenLocationInstances.FirstOrDefault().LocationTypeID).FirstOrDefault();

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
        [Authorize(Roles = "Admin, OrdersAndInventory")]
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
        [Authorize(Roles = "Admin, OrdersAndInventory")]
        public IActionResult VisualLocationsZoom(int VisualContainerId)
        {
            VisualLocationsViewModel visualLocationsViewModel = new VisualLocationsViewModel()
            {
                ParentLocationInstance = _context.LocationInstances.Where(m => m.LocationInstanceID == VisualContainerId).FirstOrDefault()
            };
            visualLocationsViewModel.ChildrenLocationInstances =
                _context.LocationInstances.Where(m => m.LocationInstanceParentID == visualLocationsViewModel.ParentLocationInstance.LocationInstanceID)
                .Include(m => m.RequestLocationInstances).ThenInclude(rli => rli.Request).ThenInclude(r => r.Product).ToList();

            return PartialView(visualLocationsViewModel);
        }

        [HttpGet]
        [Authorize(Roles = "Admin, manager, OrdersAndInventory")]
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
        [Authorize(Roles = "Admin, OrdersAndInventory")]
        public IActionResult SubLocation(int ParentLocationTypeID)
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
                        new SelectListItem() { Value="12", Text="12 x 12"},
                        new SelectListItem() { Value="9", Text="9 x 9"}
                    };
                    subLocationViewModel.BoxTypes = boxList;
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
                subLocationViewModel.LocationInstances.Add(new LocationInstance());
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
        [Authorize(Roles = "Admin, manager, OrdersAndInventory")]
        public async Task<IActionResult> AddLocation(AddLocationViewModel addLocationViewModel, SubLocationViewModel subLocationViewModel)
        {
            if (ModelState.IsValid) //make sure this allows for sublocations to be binded
            {
                List<List<string>> namesPlaceholder = new List<List<string>>();
                List<List<int>> placeholderInstanceIds = new List<List<int>>();
                bool first = true;
                switch (subLocationViewModel.LocationTypeParentID)
                {
                    case 100:
                        //save parent
                        addLocationViewModel.LocationInstance.Height = subLocationViewModel.LocationInstances[0].Height;
                        addLocationViewModel.LocationInstance.Width = 1;
                        addLocationViewModel.LocationInstance.LocationTypeID = subLocationViewModel.LocationTypeParentID;
                        _context.Add(addLocationViewModel.LocationInstance);
                        await _context.SaveChangesAsync();

                        int lastCompanyLocNo = _context.LocationInstances.OrderByDescending(li => li.CompanyLocationNo).First().CompanyLocationNo;
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
                                        _context.SaveChanges(); //DO WE NEED THIS HERE OR CAN WE DO IT ONCE AT THE END
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

                        int CompanyLocNo = _context.LocationInstances.OrderByDescending(li => li.CompanyLocationNo).First().CompanyLocationNo + 1;
                        addLocationViewModel.LocationInstance.CompanyLocationNo = CompanyLocNo;

                        _context.Add(addLocationViewModel.LocationInstance);
                        await _context.SaveChangesAsync();

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
                            else if (b == 2)
                            {
                                height = subLocationViewModel.LocationInstances[3].Height;
                                width = subLocationViewModel.LocationInstances[3].Height;
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
                                    if (b == 3) { sublocationWidth = sublocationHeight; }
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
                                        _context.SaveChanges(); //DO WE NEED THIS HERE OR CAN WE DO IT ONCE AT THE END
                                        if (b == 0) //Testing Shelves
                                        {
                                            if (subLocationViewModel.EmptyShelves80.ContainsKey(x) && subLocationViewModel.EmptyShelves80[x])
                                            {
                                                newSublocationInstance.IsEmpty = true;
                                                _context.Update(newSublocationInstance);
                                                _context.SaveChanges();
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

                        int CoNo = _context.LocationInstances.OrderByDescending(li => li.CompanyLocationNo).First().CompanyLocationNo + 1;
                        addLocationViewModel.LocationInstance.CompanyLocationNo = CoNo;
                        string nameAbbrev1 = addLocationViewModel.LocationInstance.LocationInstanceName;

                        _context.Add(addLocationViewModel.LocationInstance);
                        await _context.SaveChangesAsync();

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
                                _context.SaveChanges(); //DO WE NEED THIS HERE OR CAN WE DO IT ONCE AT THE END
                            }
                        }
                        break;
                    case 500:
                        addLocationViewModel.LocationInstance.Height = subLocationViewModel.LocationInstances[0].Height;
                        addLocationViewModel.LocationInstance.Width = subLocationViewModel.LocationInstances[0].Width;
                        addLocationViewModel.LocationInstance.LocationTypeID = subLocationViewModel.LocationTypeParentID;

                        int CoNo2 = _context.LocationInstances.OrderByDescending(li => li.CompanyLocationNo).First().CompanyLocationNo + 1;
                        addLocationViewModel.LocationInstance.CompanyLocationNo = CoNo2;
                        string nameAbbrev2 = addLocationViewModel.LocationInstance.LocationInstanceName;

                        _context.Add(addLocationViewModel.LocationInstance);
                        await _context.SaveChangesAsync();

                        string typeName2 = _context.LocationTypes.Where(x => x.LocationTypeID == subLocationViewModel.LocationInstances[0].LocationTypeID)
                                .FirstOrDefault().LocationTypeName.Substring(0, 1);
                        int typeId2 = subLocationViewModel.LocationInstances[0].LocationTypeID;
                        string place2 = "";
                        int parentId2 = 0;
                        string attachedName2 = "";
                        //CoNo++;
                        //addLocationViewModel.LocationInstance.CompanyLocationNo = CoNo;
                        parentId1 = addLocationViewModel.LocationInstance.LocationInstanceID;
                        attachedName1 = nameAbbrev2 + typeName2;
                        int typeNumber2 = 1;

                        int sublocationHeight2 = subLocationViewModel.LocationInstances[0].Height;
                        for (int x = 0; x < sublocationHeight2; x++)
                        {
                            //add letter to place
                            int unicode = x + 65;
                            char character = (char)unicode;
                            place1 = character.ToString();
                            int sublocationWidth1 = subLocationViewModel.LocationInstances[0].Width;
                            //if (sublocationWidth1 == 0) { sublocationWidth1 = 1; }
                            for (int y = 0; y < sublocationWidth1; y++)
                            {
                                string FullPlace1 = place1 + (y + 1).ToString();
                                string currentName = attachedName1 + (typeNumber2).ToString(); //add number to the type x + y is the current number but is zero based so add one
                                typeNumber2++; //increment this
                                LocationInstance newSublocationInstance = new LocationInstance()
                                {
                                    LocationInstanceName = currentName,
                                    LocationInstanceParentID = parentId1,
                                    LocationTypeID = typeId2,
                                    CompanyLocationNo = CoNo2,
                                    Place = FullPlace1
                                };
                                _context.Add(newSublocationInstance);
                                _context.SaveChanges(); //DO WE NEED THIS HERE OR CAN WE DO IT ONCE AT THE END
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
                        _context.SaveChanges();

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
                                        _context.SaveChanges(); //DO WE NEED THIS HERE OR CAN WE DO IT ONCE AT THE END
                                        placeholderInstanceIds[z].Add(newSublocationInstance.LocationInstanceID);
                                        namesPlaceholder[z].Add(newSublocationInstance.LocationInstanceName);
                                    }
                                }
                            }

                        }
                        break;
                }

            }
            return RedirectToAction("Index", "Locations", new { SectionType = AppUtility.MenuItems.LabManagement });
        }


        [HttpGet]
        [Authorize(Roles = "Admin, OrdersAndInventory")]
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
        [Authorize(Roles = "Admin, OrdersAndInventory")]
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