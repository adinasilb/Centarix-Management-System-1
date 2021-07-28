using PrototypeWithAuth.AppData;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace PrototypeWithAuth.ViewModels
{
    public class FullRequestJson
    {
        public List<TempRequestViewModel> TempRequestViewModels { get; set; }
        public RequestIndexObject RequestIndexObject { get; set; }
    }
}
