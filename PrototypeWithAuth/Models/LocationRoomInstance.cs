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
       public int LocationNumber { get; set; }
       public int LocationRoomTypeID { get; set; }
       public LocationRoomType LocationRoomType { get; set; }
       public string LocationDisplayName
        {
            get
            {  
                if(LocationRoomType ==null)
                {
                    return "";
                }
                return LocationRoomType.LocationAbbreviation + LocationNumber;
            }
            private set {; }
        }
    }
}
