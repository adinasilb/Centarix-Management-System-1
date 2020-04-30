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
        public IActionResult Index()
        {
            IEnumerable<LocationInstance> locationInstances = _context.LocationInstances.Where(li => li.LocationType.Depth == 0);
            return View(locationInstances);
        }
        [HttpGet]
        public IActionResult AddLocation()
        {
            AddLocationViewModel addLocationViewModel = new AddLocationViewModel
            {
                LocationTypesDepthOfZero = _context.LocationTypes.Where(lt => lt.Depth == 0)
            };

            return PartialView(addLocationViewModel);
        }
        [HttpPost]
        public async Task<IActionResult> AddLocation(AddLocationViewModel addLocationViewModel)
        {
            if (ModelState.IsValid)
            {

                _context.Add(addLocationViewModel.LocationInstance);
                _context.SaveChanges();
                //for now all redirects are outside if, but realy error should be outside if and only redirect if in if-statment
            }
            return RedirectToAction("Index");
        }
        //[HttpGet]
        //public IActionResult SubLocation(LocationsTier1Model locationsTier1Model)
        //{
        //    SubLocationViewModel subLocationViewModel = new SubLocationViewModel
        //    {
        //        locationsTier2Models = _context.LocationsTier2Models.Where(lt2 => lt2.LocationsTier1ModelID == locationsTier1Model.LocationsTier1ModelID),


        //    };

        //    subLocationViewModel.locationsTier3Models = _context.LocationsTier3Models.Where(lt3 => lt3.LocationsTier2ModelID == subLocationViewModel.locationsTier2Models.First().LocationsTier2ModelID);
        //    subLocationViewModel.locationsTier4Models = _context.LocationsTier4Models.Where(lt4 => lt4.LocationsTier3ModelID == subLocationViewModel.locationsTier3Models.First().LocationsTier3ModelID);
        //    subLocationViewModel.locationsTier5Models = _context.LocationsTier5Models.Where(lt5 => lt5.LocationsTier4ModelID == subLocationViewModel.locationsTier4Models.First().LocationsTier4ModelID);
        //    subLocationViewModel.locationsTier6Models = _context.LocationsTier6Models.Where(lt6 => lt6.LocationsTier5ModelID == subLocationViewModel.locationsTier5Models.First().LocationsTier5ModelID);
        //    return PartialView(subLocationViewModel);
        //}

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

        //[HttpGet] //send a json to that the subcategory list is filered
        //public JsonResult GetChildrenTypes(int LocationTypeID)
        //{
        //    bool go = true;
        //    List<LocationType> listOfChildrenTypes = new List<LocationType>();
        //    while (go)
        //    {
        //        var currentRecord = _context.LocationTypes.Where(lt => lt.LocationTypeParentID == LocationTypeID).FirstOrDefault();
        //        if (currentRecord != null)
        //        {
        //            listOfChildrenTypes.Add(currentRecord);
        //            LocationTypeID = currentRecord.LocationTypeID;
        //        }
        //        else
        //        {
        //            go = false;
        //        }
        //    }
        //    return Json(listOfChildrenTypes);

        //}


    }
}