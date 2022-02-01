using PrototypeWithAuth.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace PrototypeWithAuth.ViewModels
{
    public class CustomField
    {
        public string FieldName { get; set; }
        public int CustomDataTypeID { get; set; }
        public IEnumerable<CustomDataType> CustomDataTypes { get; set; }
        public List<bool> Required { get; set; }
        public int CustomFieldCounter { get; set; }
    }
}
