using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace PrototypeWithAuth.Models
{
    public class FunctionType
    {
        [Key]
        public int FunctionTypeID { get; set; }
        public string FunctionDescription { get; set; }
        public string Icon { get; set; }
        public string IconActionClass { get; set; }
        public string DescriptionEnum { get; set; }
    }
}
