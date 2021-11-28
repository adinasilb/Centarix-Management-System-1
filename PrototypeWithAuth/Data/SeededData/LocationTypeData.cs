using PrototypeWithAuth.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace PrototypeWithAuth.Data.SeededData
{
    public class LocationTypeData
    {
        public static List<LocationType> Get()
        {
            List<LocationType> list = new List<LocationType>();
            list.Add(new LocationType
            {
                LocationTypeID = 100,
                LocationTypeName = "-196°C",
                LocationTypePluralName = "-196°C",
                LocationTypeChildID = 101,
                Depth = 0
            });
            list.Add(new LocationType
            {
                LocationTypeID = 101,
                LocationTypeName = "Rack",
                LocationTypePluralName = "Racks",
                LocationTypeNameAbbre = "R",
                LocationTypeParentID = 100,
                LocationTypeChildID = 102,
                Depth = 1
            });
            list.Add(new LocationType
            {
                LocationTypeID = 102,
                LocationTypeName = "Box",
                LocationTypePluralName = "Boxes",
                LocationTypeNameAbbre = "B",
                LocationTypeParentID = 101,
                LocationTypeChildID = 103,
                Depth = 2
            });
            list.Add(new LocationType
            {
                LocationTypeID = 103,
                LocationTypeName = "Box Unit",
                LocationTypeNameAbbre = "B",
                LocationTypePluralName = "Box Units",
                LocationTypeParentID = 102,
                Limit = 1,
                Depth = 3
            });
            list.Add(new LocationType
            {
                LocationTypeID = 200,
                LocationTypeName = "-80°C",
                LocationTypePluralName = "-80°C",
                LocationTypeChildID = 201,
                Depth = 0
            });
            list.Add(new LocationType
            {
                LocationTypeID = 201,
                LocationTypeName = "Floor",
                LocationTypePluralName = "Floors",
                LocationTypeNameAbbre = "F",
                LocationTypeParentID = 200,
                LocationTypeChildID = 202,
                Depth = 1
            });
            list.Add(new LocationType
            {
                LocationTypeID = 202,
                LocationTypeName = "Rack",
                LocationTypePluralName = "Racks",
                LocationTypeNameAbbre = "R",
                LocationTypeParentID = 201,
                LocationTypeChildID = 203,
                Depth = 2
            });
            list.Add(new LocationType
            {
                LocationTypeID = 203,
                LocationTypeName = "Shelf",
                LocationTypePluralName = "Shelves",
                LocationTypeNameAbbre = "S",
                LocationTypeParentID = 202,
                LocationTypeChildID = 204,
                Depth = 3
            });
            list.Add(new LocationType
            {
                LocationTypeID = 204,
                LocationTypeName = "Box",
                LocationTypePluralName = "Boxes",
                LocationTypeNameAbbre = "B",
                LocationTypeParentID = 203,
                LocationTypeChildID = 205,
                Depth = 4
            });
            list.Add(new LocationType
            {
                LocationTypeID = 205,
                LocationTypeName = "Box Unit",
                LocationTypePluralName = "Box Units",
                LocationTypeNameAbbre = "B",
                LocationTypeParentID = 204,
                Limit = 1,
                Depth = 5
            });
            list.Add(new LocationType
            {
                LocationTypeID = 300,
                LocationTypeName = "-20°C",
                LocationTypePluralName = "-20°C",
                LocationTypeChildID = 301,
                Depth = 0
            });
            list.Add(new LocationType
            {
                LocationTypeID = 301,
                LocationTypeName = "Shelf",
                LocationTypePluralName = "Shelves",
                LocationTypeNameAbbre = "S",
                LocationTypeParentID = 300,
                Depth = 1
            });
            list.Add(new LocationType
            {
                LocationTypeID = 400,
                LocationTypeName = "4°C",
                LocationTypePluralName = "4°C",
                LocationTypeChildID = 401,
                Depth = 0
            });
            list.Add(new LocationType
            {
                LocationTypeID = 401,
                LocationTypeName = "Shelf",
                LocationTypePluralName = "Shelves",
                LocationTypeNameAbbre = "S",
                LocationTypeParentID = 400,
                Depth = 1
            });
            list.Add(new LocationType
            {
                LocationTypeID = 500,
                LocationTypeName = "25°C",
                LocationTypePluralName = "25°C",
                LocationTypeChildID = 501,
                Depth = 0
            });

            list.Add(new LocationType
            {
                LocationTypeID = 501,
                LocationTypeName = "Lab Part",
                LocationTypePluralName = "Lab Parts",
                LocationTypeParentID = 500,
                LocationTypeChildID = 502,
                Depth = 2
            });
            list.Add(new LocationType
            {
                LocationTypeID = 502,
                LocationTypeName = "Section",
                LocationTypePluralName = "Sections",
                LocationTypeNameAbbre = "S",
                LocationTypeParentID = 501,
                Depth = 3
            });
            list.Add(new LocationType
            {
                LocationTypeID = 600,
                LocationTypeName = "Quartzy",
                LocationTypePluralName = "Quartzy",
                LocationTypeNameAbbre = "Q",
                LocationTypeParentID = null,
                Depth = 0
            });
            return list;
        }
    }
}
