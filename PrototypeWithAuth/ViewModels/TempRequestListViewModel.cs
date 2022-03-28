using PrototypeWithAuth.AppData;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using static PrototypeWithAuth.AppData.AppUtility;

namespace PrototypeWithAuth.ViewModels
{
    public class TempRequestListViewModel
    {
        public List<Guid> Guids { get; set; }
       // public int SequencePosition { get; set; }
        public OrderType OrderType { get; set; }
        public List<TempRequestViewModel> TempRequestViewModels { get; set; }
        public RequestIndexObject RequestIndexObject { get; set; }
    }
}
