using PrototypeWithAuth.AppData.UtilityModels;
using PrototypeWithAuth.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace PrototypeWithAuth.ViewModels
{
    public class _RepairsViewModel : ViewModelBase
    {
        public int RequestID { get; set; }
        public Repair Repair { get; set; }
        public int RepairIndex { get; set; }
        public bool IsNew { get; set; }
    }
}
