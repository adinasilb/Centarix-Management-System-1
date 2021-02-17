using PrototypeWithAuth.AppData.UtilityModels;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace PrototypeWithAuth.ViewModels
{
    public class LocationInstancePlace : ViewModelBase
    {
        public int LocationInstanceId { get; set; }
        public bool Placed { get; set; }
    }
}
