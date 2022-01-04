using PrototypeWithAuth.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace PrototypeWithAuth.Data.SeededData
{
    public class CategoryTypeData
    {
        public static List<CategoryType> Get()
        {
            List<CategoryType> list = new List<CategoryType>();
            list.Add(new CategoryType
            {
                CategoryTypeID = 1,
                CategoryTypeDescription = "Lab"
            });
            list.Add(new CategoryType
            {
                CategoryTypeID = 2,
                CategoryTypeDescription = "Operational"
            });
            return list;
        }
    }
}
