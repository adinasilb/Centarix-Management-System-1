using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace PrototypeWithAuth.Models
{
    public class LocationRoomInstance
    {
        [Key]
        public int LocationRoomInstanceID { get; set; }
        public string LocationRoomInstanceName { get; set; }
        public string LocationRoomInstanceAbbrev { get; set; }
        public int LocationRoomTypeID { get; set; }
        public LocationRoomType LocationRoomType { get; set; }
    }
}
