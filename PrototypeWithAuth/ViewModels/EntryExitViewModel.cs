using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using static PrototypeWithAuth.AppData.AppUtility;

namespace PrototypeWithAuth.ViewModels
{
    public class EntryExitViewModel
    {
        public EntryExitEnum EntryExitEnum { get; set; }
        public DateTime? Entry { get; set; }
    }
}
