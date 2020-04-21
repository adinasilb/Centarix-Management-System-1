using System;
using System.CodeDom;
using System.CodeDom.Compiler;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using PrototypeWithAuth.ViewModels;

namespace PrototypeWithAuth.Controllers
{
    public class LocationsController : Controller
    {
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
                Sublocations = new List<string>()
            };
            return PartialView(addLocationTypeViewModel);
        }

        [HttpPost]
        public IActionResult AddLocationType(AddLocationTypeViewModel addLocationTypeViewModel)
        {
            CodeNamespace codeNamespace;
            CodeTypeDeclaration codeTypeDeclaration;
            CodeCompileUnit codeCompileUnit;
            //CodeCompileUnit codeCompileUnit = new CodeCompileUnit();
            //CodeNamespace codeNamespace = new CodeNamespace("PrototypeWithAuth.Models.LocationModels");
            //codeCompileUnit.Namespaces.Add(codeNamespace);
            //CodeTypeDeclaration locationClassType = new CodeTypeDeclaration(addLocationTypeViewModel.Location.ToString());
            //codeNamespace.Types.Add(locationClassType);
            //CodeEntryPointMethod start = new CodeEntryPointMethod();
            //CodeMethodInvokeExpression codeMethodInvokeExpression = new CodeMethodInvokeExpression(
            //    new CodeTypeReferenceExpression("System.Console"),
            //    "WriteLine", new CodePrimitiveExpression("Hello World"));
            //start.Statements.Add(codeMethodInvokeExpression);
            //locationClassType.Members.Add(start);
            return RedirectToAction("Index");
        }


    }
}