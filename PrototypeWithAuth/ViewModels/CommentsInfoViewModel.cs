using PrototypeWithAuth.AppData.UtilityModels;
using PrototypeWithAuth.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace PrototypeWithAuth.ViewModels
{
    public class CommentsInfoViewModel : ViewModelBase
    {
        public int Index { get; set; }
        public RequestComment Comment { get; set; }
        public bool IsEdit { get; set; }
    }
}
