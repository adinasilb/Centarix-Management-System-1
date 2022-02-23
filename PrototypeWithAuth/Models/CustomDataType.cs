using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace PrototypeWithAuth.Models
{
    public class CustomDataType : ModelBase
    {
        [Key]
        public int CustomDataTypeID { get; set; }
        public string Name { get; set; }

    }
}
