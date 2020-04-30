using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.ComponentModel.DataAnnotations;
using Microsoft.EntityFrameworkCore;

namespace PrototypeWithAuth.Models
{
    public class LocationType
    {
        [Key]
        public int LocationTypeID { get; set; }
        public string LocationTypeName { get; set; }
        public int? LocationTypeParentID { get; set; }
        public int? LocationTypeChildID { get; set; }
        public LocationType LocationTypeParent { get; set; }
        public LocationType LocationTypeChild { get; set; }
        public int Depth { get; set; }
    }
}
