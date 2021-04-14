using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace PrototypeWithAuth.Models
{
    public class ProtocolSubCategory
    {
        [Key]
        public int ProtocolSubCategoryTypeID { get; set; }
        public string ProtocolSubCategoryTypeDescription { get; set; }
        public int ProtocolCategoryTypeID { get; set; }
        public ProtocolCategory ProtocolCategoryType { get; set; }
    }
}
