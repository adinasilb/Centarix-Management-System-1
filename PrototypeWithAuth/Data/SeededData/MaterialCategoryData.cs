using PrototypeWithAuth.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace PrototypeWithAuth.Data.SeededData
{
    public class MaterialCategoryData
    {
        public static List<MaterialCategory> Get()
        {
            List<MaterialCategory> list = new List<MaterialCategory>();
            list.Add(new MaterialCategory
            {
                MaterialCategoryID = 1,
                MaterialCategoryDescription = "Reagents",
            });
            list.Add(new MaterialCategory
            {
                MaterialCategoryID = 2,
                MaterialCategoryDescription = "Plastics",
            });
            list.Add(new MaterialCategory
            {
                MaterialCategoryID = 3,
                MaterialCategoryDescription = "Equipment",
            });
            list.Add(new MaterialCategory
            {
                MaterialCategoryID = 4,
                MaterialCategoryDescription = "Buffers",
            });
          
            return list;
        }
    }
}
