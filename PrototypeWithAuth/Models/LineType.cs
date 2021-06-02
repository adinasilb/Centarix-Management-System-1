using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace PrototypeWithAuth.Models
{
    public class LineType
    {
        [Key]
        public int LineTypeID { get; set; }
        public string LineTypeDescription { get; set; }
        public int? LineTypeParentID { get; set; }
        public LineType LineTypeParent { get; set; }
        public int? LineTypeChildID { get; set; }
        public LineType LineTypeChild { get; set; }

    }
}
