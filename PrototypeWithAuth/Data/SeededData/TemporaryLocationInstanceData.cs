using PrototypeWithAuth.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace PrototypeWithAuth.Data.SeededData
{
    public class TemporaryLocationInstanceData
    {
        public static List<LocationInstance> Get()
        {
            List<LocationInstance> list = new List<LocationInstance>();
            list.Add(new LocationInstance
            {
                LocationInstanceID = -1,
                LocationInstanceAbbrev = "Quartzy",
                LocationInstanceName = "Quartzy Temporary",
                LocationTypeID = 600,
                LocationNumber = 1,
                IsEmptyShelf = true
            });
            return list;
        }
    }
}
