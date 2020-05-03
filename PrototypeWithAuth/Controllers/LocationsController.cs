using System;
using System.CodeDom;
using System.CodeDom.Compiler;
using System.Collections.Generic;
using System.Collections.Immutable;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
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
                LocationsDepthOfZero = _context.LocationInstances.Where(li => li.LocationType.Depth == 0),
                SubLocationInstances = _context.LocationInstances.Where(li => li.LocationType.Depth != 0)
            };
            
            return View(locationIndexViewModel);
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
                _context.Add(addLocationViewModel.LocationInstance);
                _context.SaveChanges();
                int parentLocationID = addLocationViewModel.LocationInstance.LocationInstanceID;
                foreach ( var sublocationInstance in subLocationViewModel.LocationInstances) 
                {
                    sublocationInstance.LocationInstanceParentID = parentLocationID;
                    _context.Add(sublocationInstance);
                    _context.SaveChanges();
                    parentLocationID = sublocationInstance.LocationInstanceID;
                }
              
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
                    if (listOfLocationNames.Count > 0 )
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