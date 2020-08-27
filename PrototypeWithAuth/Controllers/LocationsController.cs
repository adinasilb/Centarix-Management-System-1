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
        [Authorize (Roles = "Admin, OrdersAndInventory")]
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
                SublocationInstances = _context.LocationInstances.Where(li => li.LocationInstanceParentID == parentId).Include(li => li.LocationInstanceParent)
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
            var locationType = _context.LocationTypes.Where(lt => lt.LocationTypeID == sublocationIndexViewModel.SublocationInstances.FirstOrDefault().LocationTypeID).FirstOrDefault();
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
            visualLocationsViewModel.ChildrenLocationInstances =
                _context.LocationInstances.Where(m => m.LocationInstanceParentID == visualLocationsViewModel.ParentLocationInstance.LocationInstanceID)
                .Include(m => m.RequestLocationInstances).ThenInclude(rli => rli.Request).ThenInclude(r => r.Product).ToList();

            var locationType = _context.LocationTypes.Where(x => x.LocationTypeID == visualLocationsViewModel.ChildrenLocationInstances.FirstOrDefault().LocationTypeID).FirstOrDefault();
            if (locationType.LocationTypeChildID != null)
            {
                //is this enough or should we actually check for children
                visualLocationsViewModel.IsSmallestChild = false;
            }
            else
            {
                visualLocationsViewModel.IsSmallestChild = true;

            }

            return PartialView(visualLocationsViewModel);
        }
        [HttpGet]
        [Authorize(Roles = "Admin, OrdersAndInventory")]
        public IActionResult LocationIndex (int typeID )
        {
            LocationIndexViewModel locationIndexViewModel = new LocationIndexViewModel()
            {
                //exclude the box and cell from locationsDepthOfZero
                LocationsDepthOfZero = _context.LocationInstances.Where(li => li.LocationType.Depth == 0 && li.LocationTypeID==typeID),
                SubLocationInstances = _context.LocationInstances.Where(li => li.LocationType.Depth != 0 && li.LocationTypeID == typeID),
                LocationTypeParentID = typeID
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
            SubLocationViewModel subLocationViewModel = new SubLocationViewModel(); bool go = true;
            List<LocationType> listOfChildrenTypes = new List<LocationType>();
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
                //add reference to parent
                //filling up the heights and widths with the ones put in for the location below them
                addLocationViewModel.LocationInstance.Height = subLocationViewModel.LocationInstances[0].Height;
                addLocationViewModel.LocationInstance.Width = subLocationViewModel.LocationInstances[0].Width;
                _context.Add(addLocationViewModel.LocationInstance);
                _context.SaveChanges();


                //int depthOfNewInstance = 0; //corresponds to the sublevels list
                //int levelOfNewInstance = 3; //1 = go back a level, 2 = same level, 3 = create new level
                //List<string> nameAbbreviations = new List<string>()
                //{
                //    addLocationViewModel.LocationInstance.LocationInstanceName
                //};
                int companyLocationNo = _context.LocationInstances.OrderByDescending(li => li.CompanyLocationNo).First().CompanyLocationNo;

                string nameAbbreviation = addLocationViewModel.LocationInstance.LocationInstanceName;
                List<List<string>> namesPlaceholder = new List<List<string>>();
                List<List<int>> placeholderInstanceIds = new List<List<int>>(); //may need to be a list of lists
                bool first = true;

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
                    //else
                    //{
                    //}
                    //go through each instance that should be created in this sublevel
                    //do two foreach instead of a for so we could calculate the place (ex. A1, B2 etc)
                    //prev height , prev width

                    //DO WE HAVE TO DO THIS?????
                    //int heightTimes = 0;
                    //int widthTimes = 0;
                    //if (first) //set the heights and width of first b/c can't use the z of the one before
                    //{
                    //    heightTimes = add
                    //}
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
                                //add id and name to the lists
                                //change the depth?? in an if statement if it finished the last one??
                            }
                        }
                        //increment the name
                        //parent id = next parent id
                    }

                    //reset prev height and width
                    //name??
                    //parent id = next parent id
                }

                //while (!completed)
                //{
                //    LocationInstance sublocationInstance = new LocationInstance();
                //    string name = "";
                //    string place = "";
                //    int parentId = 0;
                //    string typeAbbrv = "";
                //    int height = 0;
                //    int width = 0;

                //    typeAbbrv = _context.LocationTypes.Where(x => x.LocationTypeID == subLocationViewModel.LocationInstances[depthOfNewInstance].
                //    LocationTypeID).FirstOrDefault().LocationTypeName.Substring(0, 1);

                //    //switch (levelOfNewInstance)
                //    //{
                //    //    case 1: //go back a level
                //    //        break;
                //    //    case 2: //stay on the same level
                //    //        break;
                //    //    case 3: //create a new level
                //    //        string newTypeNameFirstLetter = _context.LocationTypes.Where(x => x.LocationTypeID == subLocationViewModel.LocationInstances[level].LocationTypeID).FirstOrDefault().LocationTypeName.Substring(0, 1);
                //    //        if (first)//don't delete the last two places if it's the first
                //    //        {
                //    //            name = nameAbbreviation + newTypeNameFirstLetter + "1";
                //    //            first = false;
                //    //        }
                //    //        else
                //    //        {
                //    //            name = nameAbbreviation.Substring(0, nameAbbreviation.Length - 2) + newTypeNameFirstLetter + "1";
                //    //            //check if the lenght is right (i.e. zero or one based)
                //    //        }
                //    //        level = level + 1;
                //    //        break;
                //    //    default:
                //    //        //throw an error
                //    //        break;
                //    //}
                //}

                //int parentLocationID = addLocationViewModel.LocationInstance.LocationInstanceID;
                //string currentName = _context.LocationTypes.Where(x => x.LocationTypeID == addLocationViewModel.LocationInstance.LocationTypeID).FirstOrDefault().LocationTypeName;
                //for (int i = 0; i < subLocationViewModel.LocationInstances.Count; i++)
                //{
                //    LocationInstance sublocationInstance = subLocationViewModel.LocationInstances[i];
                //    var place = "";
                //    for (int x = 0; x < subLocationViewModel.LocationInstances[i].Height; x++)
                //    {
                //        place = ((char)x + 65).ToString(); //ASCII NOT WORKING
                //        for (int y = 0; y < subLocationViewModel.LocationInstances[i].Width; y++)
                //        {
                //            place = place + (y + 1).ToString();
                //            sublocationInstance.LocationInstanceParentID = parentLocationID;
                //            //this can be done out of the for loops
                //            var locationTypeName = _context.LocationTypes.Where(x => x.LocationTypeID == sublocationInstance.LocationTypeID).FirstOrDefault().LocationTypeName;
                //            var name = locationTypeName.Substring(0, 1) + (i + 1).ToString();
                //            bool NameCompleted = false;
                //            var currentInstanceId = (Int32)sublocationInstance.LocationInstanceParentID;
                //            while (!NameCompleted)
                //            {
                //                var parentLocationInstance = _context.LocationInstances.Where(x => x.LocationInstanceID == currentInstanceId).FirstOrDefault();
                //                if (parentLocationInstance != null)//can prob take out this if statement ask Faige
                //                {
                //                    name = parentLocationInstance.LocationInstanceName.Substring(0, 1) + name;
                //                    if (parentLocationInstance.LocationInstanceParentID != null)
                //                    {
                //                        currentInstanceId = (Int32)parentLocationInstance.LocationInstanceParentID;
                //                    }
                //                    else
                //                    {
                //                        NameCompleted = true;
                //                    }
                //                }
                //                else
                //                {//looks like this can be taken out
                //                    NameCompleted = true;
                //                }
                //            }
                //            sublocationInstance.LocationInstanceName = name;
                //            //sublocationInstance.Place = place;
                //            _context.Add(sublocationInstance);
                //            _context.SaveChanges();
                //        }
                //    }

                //parentLocationID = subLocationViewModel.LocationInstances[i].LocationInstanceID;
                //}

                //for now all redirects are outside if, but realy error should be outside if and only redirect if in if-statment
            }
            return RedirectToAction("Index");
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