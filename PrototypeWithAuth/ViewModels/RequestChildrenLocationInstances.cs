using PrototypeWithAuth.AppData.UtilityModels;
using PrototypeWithAuth.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace PrototypeWithAuth.ViewModels
{
    public class RequestChildrenLocationInstances : ViewModelBase
    {
        public LocationInstance LocationInstance { get; set; }
        public bool IsThisRequest { get; set; }
    }
}
