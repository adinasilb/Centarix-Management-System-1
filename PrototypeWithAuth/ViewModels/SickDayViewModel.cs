using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace PrototypeWithAuth.ViewModels
{
    public class SickDayViewModel
    {
        public string PageType { get; set; }
        public DateTime SelectedDate { get; set; }
        public bool NeedsDoctorsNote { get; set; }
    }
}
