using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace PrototypeWithAuth.Models
{
    public class LocationRoomType
    {
        [Key]
        public int LocationRoomTypeID { get; set; }
        public string LocationRoomTypeDescription { get; set; }
        public string LocationAbbreviation { get; set; }
    }
}
