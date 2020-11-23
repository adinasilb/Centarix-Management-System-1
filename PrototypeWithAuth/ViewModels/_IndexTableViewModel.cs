using PrototypeWithAuth.AppData;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using X.PagedList;

namespace PrototypeWithAuth.ViewModels
{
    public class _IndexTableViewModel
    {
        public IPagedList<PrototypeWithAuth.Models.Request> PagedList { get; set; }
        public bool Request { get; set; }
        public bool Inventory { get; set; }
        public bool Summary { get; set; }
        public int RequestStatusID { get; set; }
        public bool IsEquipment { get; set; }

        //column widths: 
        public Dictionary<string, string> ColumnWidths {get; set;}
        public List<AppUtility.PriceSortEnum> PriceSorts { get; set; }
    }
}
