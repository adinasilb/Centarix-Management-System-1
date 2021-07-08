using PrototypeWithAuth.AppData.UtilityModels;
using PrototypeWithAuth.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace PrototypeWithAuth.ViewModels
{
    public class RequestPaymentsViewModel : ViewModelBase
    {
        public Request Request{get; set;}
        public Payment Payment { get; set; }
    }
}
