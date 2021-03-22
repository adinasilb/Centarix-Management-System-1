using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace PrototypeWithAuth.ViewModels
{
    public class AccountingGeneralViewModel
    {
        public RequestIndexPartialViewModel RequestIndexPartialViewModel { get; set; }
        public List<int> Months { get; set; }
        public List<int> Years { get; set; }
    }
}
