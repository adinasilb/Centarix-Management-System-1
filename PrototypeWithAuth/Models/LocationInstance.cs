using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.ComponentModel.DataAnnotations;

namespace PrototypeWithAuth.Models
{
    public class LocationInstance
    {
        [Key]
        public int LocationInstanceID { get; set; }
        public int LocationTypeID { get; set; }
        public LocationType LocationType { get; set; }
        public IEnumerable<RequestLocationInstance> RequestLocationInstances { get; set; }
        public IEnumerable<RequestLocationInstance> AllRequestLocationInstances { get; set; } //refers to the ParentLocationInstanceID on the RLI
        public int? LocationInstanceParentID { get; set; }
        public LocationInstance LocationInstanceParent { get; set; }
        public int Height { get; set; }
        public int Width { get; set; }
      
        [Display(Name = "Name")]
        public string LocationInstanceName { get; set; }
        public string Place { get; set; }
        public bool IsFull { get; set; }
        public int CompanyLocationID { get; set; }
    }
}
