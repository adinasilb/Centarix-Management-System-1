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
        public List<CustomField> DetailsCustomFields { get; set; }
        public List<CustomField> PriceCustomFields { get; set; }
        public List<CustomField> DocumentsCustomFields { get; set; }
        public List<CustomField> ReceivedCustomFields { get; set; }
    }
}
