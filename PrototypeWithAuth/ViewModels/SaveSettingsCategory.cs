using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace PrototypeWithAuth.ViewModels
{
    public class SaveSettingsCategory
    {
        public string Category { get; set; }
        public bool Test { get; set; }
        public string DetailsCustomFields { get; set; }
        public string PriceCustomFields { get; set; }
        public string DocumentsCustomFields { get; set; }
        public string ReceivedCustomFields { get; set; }
    }
}
