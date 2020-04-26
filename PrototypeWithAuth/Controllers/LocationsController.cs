using System;
using System.CodeDom;
using System.CodeDom.Compiler;
using System.Collections.Generic;
using System.Collections.Immutable;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore.Scaffolding.Metadata;
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
            return View();
        }
        [HttpGet]
        public IActionResult AddLocation()
        {
            return PartialView();
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
            int foriegnKey = 0; //retain primary key to be a foriegn key for next model
            for (int i = 0; i < addLocationTypeViewModel.Sublocations.Count; i++) 
            {
                switch (i)
                {
                    case 0:
                        LocationsTier1Model locationsTier1Model = new LocationsTier1Model();
                        locationsTier1Model.LocationsTier1ModelDescription = addLocationTypeViewModel.Sublocations[i];
                        locationsTier1Model.LocationsTier1ModelAbbreviation = addLocationTypeViewModel.Sublocations[i].Substring(0,1);
                        _context.Add(locationsTier1Model);
                        _context.SaveChanges();
                        var ltm = _context.LocationsTier1Models.Where(m => m.LocationsTier1ModelDescription == addLocationTypeViewModel.Sublocations[i]).FirstOrDefault();
                        foriegnKey = ltm.LocationsTier1ModelID;
                            break;
                    case 1:
                        LocationsTier2Model locationsTier2Model = new LocationsTier2Model();
                        locationsTier2Model.LocationsTier2ModelDescription = addLocationTypeViewModel.Sublocations[i];
                        locationsTier2Model.LocationsTier2ModelAbbreviation = addLocationTypeViewModel.Sublocations[i].Substring(0, 1);
                        locationsTier2Model.LocationsTier1ModelID = foriegnKey;
                        _context.Add(locationsTier2Model);
                        _context.SaveChanges();
                        var ltm2 = _context.LocationsTier2Models.Where(m => m.LocationsTier2ModelDescription == addLocationTypeViewModel.Sublocations[i]).FirstOrDefault();
                        foriegnKey = ltm2.LocationsTier2ModelID;
                        break;
                    case 2:
                        LocationsTier3Model locationsTier3Model = new LocationsTier3Model();
                        locationsTier3Model.LocationsTier3ModelDescription = addLocationTypeViewModel.Sublocations[i];
                        locationsTier3Model.LocationsTier3ModelAbbreviation = addLocationTypeViewModel.Sublocations[i].Substring(0, 1);
                        locationsTier3Model.LocationsTier2ModelID = foriegnKey;
                        _context.Add(locationsTier3Model);
                        _context.SaveChanges();
                        var ltm3 = _context.LocationsTier3Models.Where(m => m.LocationsTier3ModelDescription == addLocationTypeViewModel.Sublocations[i]).FirstOrDefault();
                        foriegnKey = ltm3.LocationsTier3ModelID;
                        break;
                    case 3:
                        LocationsTier4Model locationsTier4Model = new LocationsTier4Model();
                        locationsTier4Model.LocationsTier4ModelDescription = addLocationTypeViewModel.Sublocations[i];
                        locationsTier4Model.LocationsTier4ModelAbbreviation = addLocationTypeViewModel.Sublocations[i].Substring(0, 1);
                        locationsTier4Model.LocationsTier3ModelID = foriegnKey;
                        _context.Add(locationsTier4Model);
                        _context.SaveChanges();
                        var ltm4 = _context.LocationsTier4Models.Where(m => m.LocationsTier4ModelDescription == addLocationTypeViewModel.Sublocations[i]).FirstOrDefault();
                        foriegnKey = ltm4.LocationsTier4ModelID;
                        break;
                    case 4:
                        LocationsTier5Model locationsTier5Model = new LocationsTier5Model();
                        locationsTier5Model.LocationsTier5ModelDescription = addLocationTypeViewModel.Sublocations[i];
                        locationsTier5Model.LocationsTier5ModelAbbreviation = addLocationTypeViewModel.Sublocations[i].Substring(0, 1);
                        locationsTier5Model.LocationsTier5ModelID = foriegnKey;
                        _context.Add(locationsTier5Model);
                        _context.SaveChanges();
                        var ltm5 = _context.LocationsTier5Models.Where(m => m.LocationsTier5ModelDescription == addLocationTypeViewModel.Sublocations[i]).FirstOrDefault();
                        foriegnKey = ltm5.LocationsTier5ModelID;
                        break;
                    case 5:
                        LocationsTier6Model locationsTier6Model = new LocationsTier6Model();
                        locationsTier6Model.LocationsTier6ModelDescription = addLocationTypeViewModel.Sublocations[i];
                        locationsTier6Model.LocationsTier6ModelAbbreviation = addLocationTypeViewModel.Sublocations[i].Substring(0, 1);
                        locationsTier6Model.LocationsTier5ModelID = foriegnKey;
                        _context.Add(locationsTier6Model);
                        _context.SaveChanges();
                        var ltm6 = _context.LocationsTier6Models.Where(m => m.LocationsTier6ModelDescription == addLocationTypeViewModel.Sublocations[i]).FirstOrDefault();
                        foriegnKey = ltm6.LocationsTier6ModelID;
                        break;



                }
                
            }
            return RedirectToAction("Index");
        }


    }
}