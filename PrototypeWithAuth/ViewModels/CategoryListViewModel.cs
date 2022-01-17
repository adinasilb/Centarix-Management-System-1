using PrototypeWithAuth.AppData;
using PrototypeWithAuth.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace PrototypeWithAuth.ViewModels
{
    public class CategoryListViewModel
    {
        public List<CategoryBase> CategoryBases { get; set; }
        public ModelBase ModelBase { get; set; }
    }
}
