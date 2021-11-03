using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using PrototypeWithAuth.AppData;
using PrototypeWithAuth.AppData.UtilityModels;
using PrototypeWithAuth.Models;

namespace PrototypeWithAuth.ViewModels
{
    public class DeleteListRequestViewModel : ViewModelBase
    {
        public Request Request { get; set; } 

        public RequestList List { get; set; }
    }
}
