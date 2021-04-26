using PrototypeWithAuth.AppData.UtilityModels;
using PrototypeWithAuth.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace PrototypeWithAuth.ViewModels
{
    public class CreateProtocolsViewModel : ViewModelBase
    {
        public Protocol Protocol { get; set; }
        public List<String> Tags { get; set; }
        public IEnumerable<ProtocolCategory> ProtocolCategories {get; set;}
        public IEnumerable<ProtocolSubCategory> ProtocolSubCategories { get; set; }
        public IEnumerable<MaterialCategory> MaterialCategories { get; set; }
        public List<DocumentFolder> DocumentsInfo { get;  set; }
    }
}
