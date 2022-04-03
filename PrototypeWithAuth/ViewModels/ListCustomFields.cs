using PrototypeWithAuth.AppData;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace PrototypeWithAuth.ViewModels
{
    public class ListCustomFields
    {
        public String CategoryName { get; set; }
        public CustomField[] CustomFields { get; set; }
    }
}
