using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace PrototypeWithAuth.Data.SeededData
{
    public class DegreeData
    {
        public static List<LineType> Get()
        {
            List<LineType> list = new List<LineType>();
            list.Add(new LineType
            {
                LineTypeID = 1,
                LineTypeDescription = "Header",
                LineTypeChildID = 2
            });
            list.Add(new LineType
            {
                LineTypeID = 2,
                LineTypeDescription = "Sub Header",
                LineTypeParentID = 1,
                LineTypeChildID = 3
            });
            list.Add(new LineType
            {
                LineTypeID = 3,
                LineTypeDescription = "Step",
                LineTypeParentID = 2
            });
            return list;
        }
    }
}
