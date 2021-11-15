using PrototypeWithAuth.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace PrototypeWithAuth.Data.SeededData
{
    public class LocationRoomTypeData
    {
        public static List<LocationRoomType> Get()
        {
            List<LocationRoomType> list = new List<LocationRoomType>();
            list.Add(new LocationRoomType
            {
                LocationRoomTypeID = 1,
                LocationRoomTypeDescription = "Laboratory",
                LocationAbbreviation = "L"
            });
            list.Add(new LocationRoomType
            {
                LocationRoomTypeID = 2,
                LocationRoomTypeDescription = "Tissue Culture",
                LocationAbbreviation = "TC"
            });
            list.Add(new LocationRoomType
            {
                LocationRoomTypeID = 3,
                LocationRoomTypeDescription = "Equipment Room",
                LocationAbbreviation = "E"
            });
            list.Add(new LocationRoomType
            {
                LocationRoomTypeID = 4,
                LocationRoomTypeDescription = "Refrigerator Room",
                LocationAbbreviation = "R"
            });
            list.Add(new LocationRoomType
            {
                LocationRoomTypeID = 5,
                LocationRoomTypeDescription = "Washing Room",
                LocationAbbreviation = "W"
            });
            list.Add(new LocationRoomType
            {
                LocationRoomTypeID = 6,
                LocationRoomTypeDescription = "Storage Room",
                LocationAbbreviation = "S"
            });
            list.Add(new LocationRoomType
            {
                LocationRoomTypeID = 7,
                LocationRoomTypeDescription = "Liquid Nitrogen Room",
                LocationAbbreviation = "LN"
            });
            return list;
        }
    }
}
