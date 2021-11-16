using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace PrototypeWithAuth.ViewModels
{
    public class WarningsModalViewModel
    {
        public TempRequestListViewModel TempRequestListViewModel { get; set; }
        public List<CommentsInfoViewModel> CommentsInfoViewModels { get; set; }
    }
}
