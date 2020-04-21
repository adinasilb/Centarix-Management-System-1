using System;
using System.CodeDom;
using System.CodeDom.Compiler;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.CSharp;
using PrototypeWithAuth.ViewModels;
using PrototypeWithAuth.AppData;

namespace PrototypeWithAuth.Controllers
{
    public class LocationsController : Controller
    {
        private readonly IHostingEnvironment _hostingEnvironment;
        public LocationsController(IHostingEnvironment hostingEnvironment)
        {
            _hostingEnvironment = hostingEnvironment;
        }
        public IActionResult Index()
        {
            return View();
        }

        public IActionResult AddLocation()
        {
            return PartialView();
        }

        public IActionResult AddLocationType()
        {
            AddLocationTypeViewModel addLocationTypeViewModel = new AddLocationTypeViewModel
            {
                Locations = new List<string>()
            };
            addLocationTypeViewModel.Locations.Add("");
            return PartialView(addLocationTypeViewModel);
        }

        [HttpPost]
        [Obsolete]
        public IActionResult AddLocationType(AddLocationTypeViewModel addLocationTypeViewModel)
        {

            for (int i = 0; i < addLocationTypeViewModel.Locations.Count; i++)
            {
                var prevLocation = "";
                if (i > 0)
                {
                    prevLocation = addLocationTypeViewModel.Locations[i - 1];
                }
                var currentLocation = addLocationTypeViewModel.Locations[i];
                var futureLocation = "";
                if (i != (addLocationTypeViewModel.Locations.Count-1))
                {
                    futureLocation = addLocationTypeViewModel.Locations[i + 1];
                }

                AppUtility.CreateCsFile("Models.LocationModels", currentLocation, prevLocation, futureLocation, _hostingEnvironment);

                //CodeNamespace locationModelsNamespace = new CodeNamespace("Models.LocationModels");
                //nameSpace.Imports.Add(locationModelsNamespace);

                //if (prevSub == "")
                //{
                //    //create property
                //}
            }


            //CSharpCodeProvider csharpcodeprovider = new CSharpCodeProvider();


            return RedirectToAction("Index");
        }



    }
}