using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace PrototypeWithAuth.ViewModels
{
    public class RequestIndexPartialRowViewModel
    {
        public IEnumerable<RequestIndexPartialColumnViewModel> Columns { get; set; }
    
    }
}
