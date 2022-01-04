using PrototypeWithAuth.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace PrototypeWithAuth.Data.SeededData
{
    public class LocationRoomInstanceData
    {
        public static List<LocationRoomInstance> Get()
        {
            List<LocationRoomInstance> list = new List<LocationRoomInstance>();
            list.Add(new LocationRoomInstance
            {
                LocationRoomInstanceID = 1,
                LocationRoomTypeID = 1,
                LocationRoomInstanceName = "Laboratory 1",
                LocationRoomInstanceAbbrev = "L1",
            });
            list.Add(new LocationRoomInstance
            {
                LocationRoomInstanceID = 2,
                LocationRoomTypeID = 1,
                LocationRoomInstanceName = "Laboratory 2",
                LocationRoomInstanceAbbrev = "L2",
            });

            list.Add(new LocationRoomInstance
            {
                LocationRoomInstanceID = 3,
                LocationRoomInstanceName = "Tissue Culture 1",
                LocationRoomInstanceAbbrev = "TC1",
                LocationRoomTypeID = 2
            });

            list.Add(new LocationRoomInstance
            {
                LocationRoomInstanceID = 4,
                LocationRoomTypeID = 3,
                LocationRoomInstanceName = "Equipment Room 1",
                LocationRoomInstanceAbbrev = "E1"

            });
            list.Add(new LocationRoomInstance
            {
                LocationRoomInstanceID = 5,
                LocationRoomTypeID = 4,
                LocationRoomInstanceName = "Refrigerator Room 1",
                LocationRoomInstanceAbbrev = "R1"

            });
            list.Add(new LocationRoomInstance
            {
                LocationRoomInstanceID = 6,
                LocationRoomTypeID = 5,
                LocationRoomInstanceName = "Washing Room 1",
                LocationRoomInstanceAbbrev = "W1"
            });

            list.Add(new LocationRoomInstance
            {
                LocationRoomInstanceID = 7,
                LocationRoomTypeID = 6,
                LocationRoomInstanceName = "Storage Room 1",
                LocationRoomInstanceAbbrev = "S1"

            });
            list.Add(new LocationRoomInstance
            {
                LocationRoomInstanceID = 8,
                LocationRoomTypeID = 1,
                LocationRoomInstanceName = "DS-Lab 3",
                LocationRoomInstanceAbbrev = "DSL3",
            });
            list.Add(new LocationRoomInstance
            {
                LocationRoomInstanceID = 9,
                LocationRoomTypeID = 1,
                LocationRoomInstanceName = "DS-Lab 4",
                LocationRoomInstanceAbbrev = "DSL4",
            });
            list.Add(new LocationRoomInstance
            {
                LocationRoomInstanceID = 10,
                LocationRoomInstanceName = "DS-Tissue Culture 2",
                LocationRoomInstanceAbbrev = "DSTC2",
                LocationRoomTypeID = 2
            });
            list.Add(new LocationRoomInstance
            {
                LocationRoomInstanceID = 11,
                LocationRoomTypeID = 5,
                LocationRoomInstanceName = "DS-Washing Room 2",
                LocationRoomInstanceAbbrev = "DSW2"
            });
            list.Add(new LocationRoomInstance
            {
                LocationRoomInstanceID = 12,
                LocationRoomTypeID = 7,
                LocationRoomInstanceName = "Liquid Nitrogen Room 1",
                LocationRoomInstanceAbbrev = "LN1"

            });
            list.Add(new LocationRoomInstance
            {
                LocationRoomInstanceID =13,
                LocationRoomTypeID = 1,
                LocationRoomInstanceName = "Biomarker Lab 5",
                LocationRoomInstanceAbbrev = "BL5",
            });

            list.Add(new LocationRoomInstance
            {
                LocationRoomInstanceID = 14,
                LocationRoomTypeID = 6,
                LocationRoomInstanceName = "Storage Room 2",
                LocationRoomInstanceAbbrev = "S2"

            });

            list.Add(new LocationRoomInstance
            {
                LocationRoomInstanceID = 15,
                LocationRoomTypeID = 6,
                LocationRoomInstanceName = "Storage Room 3",
                LocationRoomInstanceAbbrev = "S3"

            });
            return list;
        }
    }
}
