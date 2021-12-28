using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using PrototypeWithAuth.Data;
using PrototypeWithAuth.Models;
using PrototypeWithAuth.AppData;
using Microsoft.AspNetCore.Authorization;
using Abp.Extensions;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc.ViewEngines;
using PrototypeWithAuth.AppData.UtilityModels;

namespace PrototypeWithAuth.Controllers
{
    public class ProductSubcategoriesController : SharedController
    {
        public ProductSubcategoriesController(ApplicationDbContext context, UserManager<ApplicationUser> userManager, IHostingEnvironment hostingEnvironment, IHttpContextAccessor httpContextAccessor, ICompositeViewEngine viewEngine)
           : base(context, userManager, hostingEnvironment, viewEngine, httpContextAccessor)
        {
        }

        // GET: ProductSubcategories
        [Authorize(Roles = "Requests, Operations, LabManagement")]
        public async Task<IActionResult> Index(AppUtility.PageTypeEnum PageType = AppUtility.PageTypeEnum.RequestRequest, AppUtility.MenuItems SectionType = AppUtility.MenuItems.Requests)
        {
            var categoryType = 1;
            if (SectionType == AppUtility.MenuItems.Operations)
            {
                categoryType = 2;
            }
            if (PageType == AppUtility.PageTypeEnum.LabManagementEquipment)
            {
                TempData[AppUtility.TempDataTypes.MenuType.ToString()] = AppUtility.MenuItems.LabManagement;
                TempData[AppUtility.TempDataTypes.SidebarType.ToString()] = AppUtility.SidebarEnum.Type;
                TempData[AppUtility.TempDataTypes.PageType.ToString()] = AppUtility.PageTypeEnum.LabManagementEquipment;
                var equipmentCategories = _productSubcategoriesProc.Read(new List<System.Linq.Expressions.Expression<Func<ProductSubcategory, bool>>>
                    { ps => ps.ParentCategoryID == 5 },
                    new List<ComplexIncludes<ProductSubcategory, ModelBase>>
                    {
                        new ComplexIncludes<ProductSubcategory, ModelBase>{ Include = ps => ps.ParentCategory, ThenInclude =
                        new ComplexIncludes<ModelBase, ModelBase>{Include = pc => ((ParentCategory)pc).CategoryType}}
                    });
                return View(await equipmentCategories.ToListAsync());
            }
            else if (categoryType == 1)
            {
                TempData[AppUtility.TempDataTypes.MenuType.ToString()] = AppUtility.MenuItems.Requests;
            }
            else if (categoryType == 2)
            {
                TempData[AppUtility.TempDataTypes.MenuType.ToString()] = AppUtility.MenuItems.Operations;
            }
            TempData[AppUtility.TempDataTypes.SidebarType.ToString()] = AppUtility.SidebarEnum.Type;
            TempData[AppUtility.TempDataTypes.PageType.ToString()] = PageType;
            var applicationDbContext = _productSubcategoriesProc.Read(new List<System.Linq.Expressions.Expression<Func<ProductSubcategory, bool>>>
            { ps => ps.ParentCategory.CategoryTypeID == categoryType},
            new List<ComplexIncludes<ProductSubcategory, ModelBase>>
            {
                new ComplexIncludes<ProductSubcategory, ModelBase>{Include = p => p.ParentCategory, ThenInclude = new ComplexIncludes<ModelBase, ModelBase>
                { Include = pc => ((ParentCategory)pc).CategoryType}}
            });
            if (PageType == AppUtility.PageTypeEnum.RequestRequest)
            {
                applicationDbContext = applicationDbContext.Where(ps => ps.ParentCategory.IsProprietary == false);
            }
            return View(await applicationDbContext.ToListAsync());
        }

        [HttpGet] //send a json to that the subcategory list is filtered
        [Authorize(Roles = "Requests, Operations")]
        public JsonResult GetSubCategoryList(int? ParentCategoryId)
        {
            var subCategoryList = _productSubcategoriesProc.Read().ToList();
            if (ParentCategoryId != null)
            {
                subCategoryList = _productSubcategoriesProc.Read(new List<System.Linq.Expressions.Expression<Func<ProductSubcategory, bool>>> { c => c.ParentCategoryID == ParentCategoryId }).ToList();
            }
            return Json(subCategoryList);
        }
        [HttpGet]
        [Authorize(Roles = "Requests, Operations")]
        public JsonResult FilterByCategoryType(List<int> SelectedCategoryTypes)
        {
            var requests = _requestsProc.Read(new List<System.Linq.Expressions.Expression<Func<Request, bool>>>
                { r => SelectedCategoryTypes.Contains(r.Product.ProductSubcategory.ParentCategory.CategoryTypeID) },
                new List<ComplexIncludes<Request, ModelBase>> {
                    new ComplexIncludes<Request, ModelBase>
                    {
                        Include = r => r.Product, ThenInclude = new ComplexIncludes<ModelBase, ModelBase> {Include = p => ((Product)p).Vendor }
                    },
                    new ComplexIncludes<Request, ModelBase> {
                        Include = p => p.Product.ProductSubcategory, ThenInclude = new ComplexIncludes<ModelBase, ModelBase>{ Include = ps => ((ProductSubcategory)ps).ParentCategory}
                        },
                    new ComplexIncludes<Request, ModelBase> { Include = r => r.ApplicationUserCreator }
                });
            var parentCategories = _parentCategoriesProc.Read(new List<System.Linq.Expressions.Expression<Func<ParentCategory, bool>>>
                { pc => SelectedCategoryTypes.Contains(pc.CategoryTypeID) }, new List<ComplexIncludes<ParentCategory, ModelBase>>
                { new ComplexIncludes<ParentCategory, ModelBase>{ Include = pc => pc.ProductSubcategories } });
            var parentCategoriesJson = parentCategories.Select(pc => new { parentCategoryID = pc.ParentCategoryID, parentCategoryDescription = pc.ParentCategoryDescription });
            var vendors = _vendorsProc.Read(new List<System.Linq.Expressions.Expression<Func<Vendor, bool>>>
            {
                v => v.VendorCategoryTypes.Select(vct=>vct.CategoryTypeID).Where(cti=> SelectedCategoryTypes.Contains(cti)).Any()
            }).Select(v => new { vendorID = v.VendorID, vendorName = v.VendorEnName });
            var subCategoryList = parentCategories.SelectMany(pc => pc.ProductSubcategories)
                .Select(ps => new { subCategoryID = ps.ProductSubcategoryID, subCategoryDescription = ps.ProductSubcategoryDescription });
            var workers = requests.Select(r => r.ApplicationUserCreator).Select(e => new { workerID = e.Id, workerName = e.FirstName + " " + e.LastName }).Distinct();
            return Json(new { Vendors = vendors, ProductSubcategories = subCategoryList, ParentCategories = parentCategoriesJson, Employees = workers });


        }

        [HttpGet]
        [Authorize(Roles = "Requests, Operations")]
        public JsonResult FilterByParentCategories(List<int> ParentCategoryIds)
        {
            var requests = _requestsProc.Read(new List<System.Linq.Expressions.Expression<Func<Request, bool>>>
                { r => ParentCategoryIds.Contains(r.Product.ProductSubcategory.ParentCategoryID) },
                new List<ComplexIncludes<Request, ModelBase>> {
                    new ComplexIncludes<Request, ModelBase>
                    {
                        Include = r => r.Product, ThenInclude = new ComplexIncludes<ModelBase, ModelBase> { Include = p => ((Product)p).Vendor }
                    },
                    new ComplexIncludes<Request, ModelBase>{ Include = r => r.Product.ProductSubcategory },
                    new ComplexIncludes<Request, ModelBase>{ Include = r => r.ApplicationUserCreator }
                });
            var vendors = requests.Select(r => r.Product.Vendor).Distinct().Select(v => new { vendorID = v.VendorID, vendorName = v.VendorEnName });
            var subCategoryList = _productSubcategoriesProc.Read(new List<System.Linq.Expressions.Expression<Func<ProductSubcategory, bool>>>
                { ps=>ParentCategoryIds.Contains(ps.ParentCategoryID) })
                .Select(ps => new { subCategoryID = ps.ProductSubcategoryID, subCategoryDescription = ps.ProductSubcategoryDescription });
            var workers = requests.Select(r => r.ApplicationUserCreator).Select(e => new { workerID = e.Id, workerName = e.FirstName + " " + e.LastName }).Distinct();
            return Json(new { Vendors = vendors, ProductSubcategories = subCategoryList, Employees = workers });

        }

        [HttpGet]
        [Authorize(Roles = "Requests, Operations")]
        public JsonResult FilterBySubCategories(List<int> SubCategoryIds)
        {
            var requests = _requestsProc.Read(new List<System.Linq.Expressions.Expression<Func<Request, bool>>> { r => SubCategoryIds.Contains(r.Product.ProductSubcategoryID) },
                new List<ComplexIncludes<Request, ModelBase>>
                {
                    new ComplexIncludes<Request, ModelBase>
                    {
                        Include = r => r.Product, ThenInclude = new ComplexIncludes<ModelBase, ModelBase> { Include = p => ((Product)p).Vendor }
                    },
                    new ComplexIncludes<Request, ModelBase> { Include = p => p.Product.ProductSubcategory },
                    new ComplexIncludes<Request, ModelBase> { Include = r => r.ApplicationUserCreator }
                });
            var vendors = requests.Select(r => r.Product.Vendor).Distinct().Select(v => new { vendorID = v.VendorID, vendorName = v.VendorEnName });
            var workers = requests.Select(r => r.ApplicationUserCreator).Select(e => new { workerID = e.Id, workerName = e.FirstName + " " + e.LastName }).Distinct();
            return Json(new { Vendors = vendors, Employees = workers });

        }


    }
}
