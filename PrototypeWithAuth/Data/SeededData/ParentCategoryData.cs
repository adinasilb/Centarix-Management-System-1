using PrototypeWithAuth.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace PrototypeWithAuth.Data.SeededData
{
    public class ParentCategoryData
    {
        public static List<ParentCategory> Get()
        {
            List<ParentCategory> list = new List<ParentCategory>();
            list.Add(new ParentCategory
            {
                ID = 1,
                Description = "Consumables",
                CategoryTypeID = 1
            });
            list.Add(new ParentCategory
            {
                ID = 2,
                Description = "Reagents And Chemicals",
                CategoryTypeID = 1
            });
            list.Add(new ParentCategory
            {
                ID = 3,
                Description = "Biological",
                CategoryTypeID = 1
            });
            list.Add(new ParentCategory
            {
                ID = 4,
                Description = "Reusable",
                CategoryTypeID = 1
            });
            list.Add(new ParentCategory
            {
                ID = 5,
                Description = "Safety",
                CategoryTypeID = 1
            });
            list.Add(new ParentCategory
            {
                ID = 6,
                Description = "General",
                CategoryTypeID = 1
            });
            list.Add(new ParentCategory
            {
                ID = 7,
                Description = "Clinical",
                CategoryTypeID = 1
            });
            list.Add(new ParentCategory
            {
                ID = 14,
                Description = "Samples",
                CategoryTypeID = 1,
                IsProprietary = true
            });
            list.Add(new ParentCategory
            {
                ID = 8,
                Description = "IT",
                CategoryTypeID = 2
            });
            list.Add(new ParentCategory
            {
                ID = 9,
                Description = "Day To Day",
                CategoryTypeID = 2
            });
            list.Add(new ParentCategory
            {
                ID = 10,
                Description = "Travel",
                CategoryTypeID = 2
            });
            list.Add(new ParentCategory
            {
                ID = 11,
                Description = "Advice",
                CategoryTypeID = 2
            });
            list.Add(new ParentCategory
            {
                ID = 12,
                Description = "Regulations",
                CategoryTypeID = 2
            });
            list.Add(new ParentCategory
            {
                ID = 13,
                Description = "Government",
                CategoryTypeID = 2
            });
            return list;
        }
    }
}
