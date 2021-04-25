using Microsoft.AspNetCore.Mvc;
using PrototypeWithAuth.AppData;
using PrototypeWithAuth.AppData.UtilityModels;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace PrototypeWithAuth.ViewModels
{
    public class ReorderViewModel : ViewModelBase
    {
        public RequestItemViewModel RequestItemViewModel { get; set; }
        public RequestIndexObject RequestIndexObject { get; set; }
    }
}
