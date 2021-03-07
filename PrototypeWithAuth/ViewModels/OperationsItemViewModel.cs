using PrototypeWithAuth.AppData;
using PrototypeWithAuth.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace PrototypeWithAuth.ViewModels
{
    public class OperationsItemViewModel
    {
        public IEnumerable<ParentCategory> ParentCategories { get; set; }
        public IEnumerable<ProductSubcategory> ProductSubcategories { get; set; }
        public int RequestIndex { get; set; }
        public AppUtility.RequestModalType ModalType { get; set; }
    }
}
