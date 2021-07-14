using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace PrototypeWithAuth.ViewModels
{
    public class ExperimentListViewModel
    {
        public List<TDViewModel> Headers { get; set; }
        public List<List<TDViewModel>>ValuesPerRow { get; set; }
    }
}
