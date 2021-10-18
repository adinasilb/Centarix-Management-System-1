using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using PrototypeWithAuth.AppData.UtilityModels;
using PrototypeWithAuth.Models;

namespace PrototypeWithAuth.ViewModels
{
    public class WorkFlowViewModel : ViewModelBase
    {
        public ProtocolInstance ProtocolInstance { get; set; }
        public string CurrentLineString { get; set; }
    }
}
