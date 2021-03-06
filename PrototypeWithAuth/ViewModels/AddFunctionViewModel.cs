using Microsoft.AspNetCore.Mvc.Rendering;
using PrototypeWithAuth.AppData;
using PrototypeWithAuth.AppData.UtilityModels;
using PrototypeWithAuth.Data;
using PrototypeWithAuth.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace PrototypeWithAuth.ViewModels
{
    public class AddFunctionViewModel<T> : ViewModelBase where T :FunctionBase
    {
        public T Function { get; set; }
        public List<Vendor> Vendors { get; set; }
        public List<Product> Products { get; set; }
        public List<ParentCategory> ParentCategories { get; set; }
        public List<ProductSubcategory> ProductSubcategories { get; set; }
        public List<SelectListItem> Creators { get; set; }
        public List<ProtocolCategory> ProtocolCategories { get; set; }
        public List<ProtocolSubCategory> ProtocolSubCategories { get; set; }
        public List<ProtocolVersion> ProtocolVersions { get; set; }
        public List<DocumentFolder> DocumentsInfo { get; set; }
        public bool IsRemove { get; set; }
        public DocumentsModalViewModel DocumentsModalViewModel { get; set; }
        public AppUtility.ProtocolModalType ModalType { get; set; }
        
    }
}
