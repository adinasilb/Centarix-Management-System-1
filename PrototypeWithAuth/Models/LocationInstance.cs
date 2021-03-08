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
      
        [Display(Name = "Location")]
        public string LocationInstanceName { get; set; }
        public string LocationInstanceAbbrev { get; set; }
        public string Place { get; set; }
        public bool IsEmptyShelf { get; set; }
        public bool ContainsItems { get; set; }
        public bool IsFull { get; set; }
        [Display(Name = "No")]
        public int CompanyLocationNo { get; set; }
        public int? LabPartID { get; set; }
        public LabPart LabPart { get; set; }
        public int LocationNumber { get; set; }
        public int? LocationRoomTypeID { get; set; }
        public LocationRoomType LocationRoomType { get; set; }

    }
}
