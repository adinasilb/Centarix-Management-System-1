using PrototypeWithAuth.AppData.UtilityModels;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace PrototypeWithAuth.ViewModels
{
    public class HasShelfViewModel : ViewModelBase
    {
        public string LocationName { get; set; }
        public bool HasShelves { get; set; }
    }
}
