using PrototypeWithAuth.AppData;
using PrototypeWithAuth.AppData.UtilityModels;
using PrototypeWithAuth.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace PrototypeWithAuth.ViewModels
{
    public class MaterialTabViewModel:ViewModelBase
    {
        public IEnumerable<MaterialCategory> MaterialCategories { get; set; }
        public List<Material> Materials { get; set; }
        public Lookup<Material,List<DocumentFolder>> Folders { get; set; }
        public AppUtility.ProtocolModalType ModalType { get; set; }

    }
}
