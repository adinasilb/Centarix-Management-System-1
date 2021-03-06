using PrototypeWithAuth.AppData;
using PrototypeWithAuth.AppData.UtilityModels;
using PrototypeWithAuth.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace PrototypeWithAuth.ViewModels
{
    public class LocationTypeViewModel : ViewModelBase
    {
        public IEnumerable<LocationType> LocationTypes { get; set; }
        public AppUtility.MenuItems SectionType { get; set; }

    }
}
