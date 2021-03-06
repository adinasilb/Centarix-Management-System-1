using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.ComponentModel.DataAnnotations;
using PrototypeWithAuth.AppData.UtilityModels;

namespace PrototypeWithAuth.Models
{
    public class LocationInstance : ModelBase
    {
        //WHEN QUERY - NEED TO SPECIFY OfType<LocationInstance> TO NOT HAVE TEMPORARY LOCATIONS
        [Key]
        public int LocationInstanceID { get; set; }
        public int LocationTypeID { get; set; }
        public LocationType LocationType { get; set; }
        public ListImplementsModelBase<RequestLocationInstance> RequestLocationInstances { get; set; }
        public ListImplementsModelBase<RequestLocationInstance> AllRequestLocationInstances { get; set; } //refers to the ParentLocationInstanceID on the RLI
        public int? LocationInstanceParentID { get; set; }
        public LocationInstance LocationInstanceParent { get; set; }
        public int Height { get; set; }
        public int Width { get; set; }
      
        [Display(Name = "Location")]
        public string LocationInstanceName { get; set; }
        public string LocationInstanceAbbrev { get; set; }
        public bool IsEmptyShelf { get; set; }
        public bool ContainsItems { get; set; }
        public bool IsFull { get; set; }
        public int? LabPartID { get; set; }
        public LabPart LabPart { get; set; }
        public int LocationNumber { get; set; }
        public int? LocationRoomInstanceID { get; set; }
        public LocationRoomInstance LocationRoomInstance { get; set; }

    }
}
