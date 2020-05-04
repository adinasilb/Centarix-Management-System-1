using System;
using System.CodeDom;
using System.CodeDom.Compiler;
using System.Collections.Generic;
using System.Collections.Immutable;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
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
        public IActionResult Index()
        {
            LocationIndexViewModel locationIndexViewModel = new LocationIndexViewModel()
            {
                //exclude the box and cell from locationsDepthOfZero
                LocationsDepthOfZero = _context.LocationInstances.Where(li => li.LocationType.Depth == 0),
                SubLocationInstances = _context.LocationInstances.Where(li => li.LocationType.Depth != 0)
            };

            return View(locationIndexViewModel);
        }

        [HttpGet]
        public IActionResult SublocationIndex(int parentId)
        {
            List<LocationInstance> sublocationInstances = new List<LocationInstance>();
            bool go = true;
            while (go)
            {
                LocationInstance location = _context.LocationInstances.Where(x => x.LocationInstanceParentID == parentId).FirstOrDefault();
                if (location != null)
                {
                    sublocationInstances.Add(location);
                    parentId = location.LocationInstanceID;
                }
                else
                {
                    go = false;
                }
            }
            return PartialView(sublocationInstances);
        }

        [HttpGet]
        public IActionResult AddLocation()
        {
            AddLocationViewModel addLocationViewModel = new AddLocationViewModel
            {
                LocationTypesDepthOfZero = _context.LocationTypes.Where(lt => lt.Depth == 0),
                LocationInstance = new LocationInstance()
            };

            return PartialView(addLocationViewModel);
        }
        [HttpPost]
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
                string nameAbbreviation = addLocationViewModel.LocationInstance.LocationInstanceName;
                List<string> namesPlaceholder = new List<string>();
                List<List<int>> placeholderInstanceIds = new List<List<int>>(); //may need to be a list of lists
                bool first = true;

                for(int z=0; z<subLocationViewModel.LocationInstances.Count; z++)/*var locationInstance in subLocationViewModel.LocationInstances*/ //for each level in the sublevels
                {
                    string typeName = nameAbbreviation + _context.LocationTypes.Where(x => x.LocationTypeID == subLocationViewModel.LocationInstances[z].LocationTypeID)
                        .FirstOrDefault().LocationTypeName.Substring(0, 1);
                    string place = "";
                    int parentId = 0;
                    int height = subLocationViewModel.LocationInstances[z+1].Height;
                    int width = subLocationViewModel.LocationInstances[z+1].Width;
                    //go through each instance that should be created in this sublevel
                    //do two foreach instead of a for so we could calculate the place (ex. A1, B2 etc)
                    if (first)
                    {
                        parentId = addLocationViewModel.LocationInstance.LocationInstanceID;
                    }
                    else
                    {
                        parentId = placeholderInstanceIds[z][0]; //get the first id in the list in the depth before
                    }
                    for (int x=0; x < subLocationViewModel.LocationInstances[z].Height; x++)
                    {
                        //add letter to place
                        for (int y=0; y < subLocationViewModel.LocationInstances[z].Width; y++)
                        {
                            //add number to place
                            LocationInstance newSublocationInstance = new LocationInstance();
                        //change the depth?? in an if statement if it finished the last one??
                        }
                    }
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

        [HttpGet]
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

    }
}