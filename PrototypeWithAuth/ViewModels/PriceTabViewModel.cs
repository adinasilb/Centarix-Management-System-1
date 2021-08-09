using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using PrototypeWithAuth.Models;
using PrototypeWithAuth.Data;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc.Rendering;
using PrototypeWithAuth.AppData;
using System.IO;
using System.Text.Json.Serialization;
using Newtonsoft.Json;
using PrototypeWithAuth.AppData.UtilityModels;

namespace PrototypeWithAuth.ViewModels
{
    public class PriceTabViewModel
    {
        public List<Request> Requests { get; set; }
        public IEnumerable<SelectListItem> UnitTypeList { get; set; }
        public ILookup<UnitParentType, UnitType> UnitTypes { get; set; }
        public bool IsReorder { get; set; }
    }
}
