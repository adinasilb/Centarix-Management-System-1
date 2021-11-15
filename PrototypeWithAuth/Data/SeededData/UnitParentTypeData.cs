using PrototypeWithAuth.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace PrototypeWithAuth.Data.SeededData
{
    public class UnitParentTypeData
    {
        public static List<UnitParentType> Get()
        {
            List<UnitParentType> list = new List<UnitParentType>();
            list.Add(new UnitParentType
            {
                UnitParentTypeID = 1,
                UnitParentTypeDescription = "Units"
            });
            list.Add(new UnitParentType
            {
                UnitParentTypeID = 2,
                UnitParentTypeDescription = "Weight/Volume"
            });
            list.Add(new UnitParentType
            {
                UnitParentTypeID = 3,
                UnitParentTypeDescription = "Test"
            });
            return list;
        }
    }
}
