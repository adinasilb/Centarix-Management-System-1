using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using PrototypeWithAuth.AppData.UtilityModels;
using PrototypeWithAuth.Models;

namespace PrototypeWithAuth.ViewModels
{
    public class ProtocolsLinesViewModel : ViewModelBase
    { 
        public List<ProtocolsLineViewModel> Lines { get; set; }
    }
}
