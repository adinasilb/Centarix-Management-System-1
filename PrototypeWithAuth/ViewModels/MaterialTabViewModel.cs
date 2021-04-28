using PrototypeWithAuth.AppData.UtilityModels;
using PrototypeWithAuth.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace PrototypeWithAuth.ViewModels
{
    public class MaterialTabViewModel
    {
        public IEnumerable<MaterialCategory> MaterialCategories { get; set; }
        public IEnumerable<Material> Materials { get; set; }
        public List<DocumentFolder> Folders { get; set; }

    }
}
