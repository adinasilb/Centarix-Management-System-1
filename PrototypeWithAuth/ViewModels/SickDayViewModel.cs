using PrototypeWithAuth.AppData.UtilityModels;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace PrototypeWithAuth.ViewModels
{
    public class SickDayViewModel : ViewModelBase
    {
        public string PageType { get; set; }
        public DateTime SelectedDate { get; set; }
    }
}
